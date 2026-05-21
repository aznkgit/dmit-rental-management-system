using Microsoft.AspNetCore.Components;
using RentalManagementSystem.BLL;
using RentalManagementSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace RentalManagementWebApp.Components.RentalPages
{
    public partial class RentalEdit
    {

        [Parameter] public bool IsVisible { get; set; }

        [Parameter] public string Message { get; set; }

        [Parameter]
        public EventCallback<bool?> OnClose { get; set; }

        [Parameter]
        public int RentalId { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        private RentalTypeService RentalTypeService { get; set; } = default!;

        [Inject]
        private AddressService AddressService { get; set; } = default!;

        private AddressView? Address { get; set; }

        [Inject]
        private RentalDetailsService RentalDetailsService { get; set; } = default!;
        private RentalView? RentalDetails { get; set; }

        [Inject]
        private RentalEditService RentalEditService { get; set; } = default!;

        private List<RentalTypeView> RentalTypes { get; set; } = new();

        [Inject]
        private StudentService StudentService { get; set; } = default!;
        private string LastNameSearch { get; set; } = string.Empty;
        private List<StudentView> SearchResults { get; set; } = new();
        private List<string> ErrorList { get; set; } = new();

        private bool isChanged = false;
        private bool showDialog = false;
        private string dialogMessage = string.Empty;
        private TaskCompletionSource<bool?>? dialogCompletionSource;


        protected override void OnInitialized()
        {
            try
            {
                RentalTypes = RentalTypeService.GetRentalTypes();

                if (RentalId > 0)
                {
                    Address = AddressService.GetAddress(RentalId);
                    RentalDetails = RentalDetailsService.GetRental(RentalId);
                }
                else if (RentalId == 0)
                {
                    Address = new AddressView();
                    RentalDetails = new RentalView();
                }
            }
            catch (Exception ex)
            {
                ErrorList.Add($"Error: {ex.Message}");
            }
        }

        private void CloseDialog(bool? result)
        {
            OnClose.InvokeAsync(result);
        }

        private void SearchStudent()
        {
            ErrorList.Clear();
            try
            {
                if (string.IsNullOrWhiteSpace(LastNameSearch))
                {
                    ErrorList.Add("You must enter a last name.");
                    return;
                }

                SearchResults = StudentService.GetStudentsByLastName(LastNameSearch);
            }
            catch (Exception ex)
            {
                ErrorList.Add($"Error searching students: {ex.Message}");
            }
        }

        private void AddRenter(StudentView student)
        {
            ErrorList.Clear();
            try
            {
                if (RentalDetails == null) return;

                var alreadyInList = RentalDetails.Renters
                    .Any(r => r.StudentNumber == student.StudentNumber);
                if (alreadyInList)
                {
                    ErrorList.Add($"{student.FirstName} {student.LastName} is already added as a renter.");
                    return;
                }

                if (RentalId > 0)
                {
                    var renterView = RentalEditService.AddRenter(RentalId, student.StudentNumber);
                    RentalDetails.Renters.Add(renterView);
                }
                else
                {
                    RentalDetails.Renters.Add(new RenterView
                    {
                        RenterID = 0,
                        StudentNumber = student.StudentNumber,
                        StudentName = $"{student.FirstName} {student.LastName}",
                        EffectiveDate = DateTime.Today,
                        Active = true
                    });
                }

                isChanged = true;
            }
            catch (Exception ex)
            {
                ErrorList.Add($"Error adding renter: {ex.Message}");
            }
        }

        private void RemoveRenter(RenterView renter)
        {
            ErrorList.Clear();
            try
            {
                if (RentalId > 0 && renter.RenterID > 0)
                {
                    RentalEditService.RemoveRenter(renter.RenterID);
                }

                RentalDetails?.Renters.Remove(renter);
                isChanged = true;
            }
            catch (Exception ex)
            {
                ErrorList.Add($"Error removing renter: {ex.Message}");
            }
        }

        private async Task<bool?> ShowDialogAsync()
        {
            dialogCompletionSource = new TaskCompletionSource<bool?>();
            showDialog = true;
            await InvokeAsync(StateHasChanged);
            return await dialogCompletionSource.Task;
        }

        private async Task SimpleDialogResult(bool? result)
        {
            showDialog = false;
            dialogCompletionSource.SetResult(result);
            await InvokeAsync(StateHasChanged);
        }

        private async Task Cancel()
        {
            dialogMessage = "Do you wish to close the rental editor?";
            bool? result = await ShowDialogAsync();

            if (result == true)
            {
                NavigationManager.NavigateTo("/RentalPages/RentalList");
            }
        }

        private void SaveRental()
        {
            ErrorList.Clear();

            if (RentalDetails == null)
            {
                ErrorList.Add("Rental details are missing.");
                return;
            }

            if (RentalDetails.Renters.Count == 0)
            {
                ErrorList.Add("You must have at least one renter!");
            }

            if (RentalDetails.RentalTypeID == null || RentalDetails.RentalTypeID == 0)
            {
                ErrorList.Add("No rental type was provided!");
            }

            if (RentalDetails.MonthlyRent <= 0)
            {
                ErrorList.Add("Monthly rent must be greater than zero!");
            }

            if (RentalDetails.DamageDeposit <= 0)
            {
                ErrorList.Add("Damage deposit must be greater than zero!");
            }

            if (RentalDetails.MaxVacancy <= 0)
            {
                ErrorList.Add("Max vacancy must be greater than zero!");
            }

            if (RentalDetails.AvailableDate == null)
            {
                ErrorList.Add("Available date cannot be empty!");
            }

            if (RentalDetails.Vacancies < 0)
            {
                ErrorList.Add("Vacancies can't be less than 0.");
            }

            if (ErrorList.Any())
            {
                return;
            }

            try
            {
                var pendingRenters = RentalId == 0
                    ? RentalDetails.Renters.ToList()
                    : new List<RenterView>();

                int savedId = RentalEditService.SaveRental(RentalId, RentalDetails, Address!);

                foreach (var renter in pendingRenters)
                {
                    RentalEditService.AddRenter(savedId, renter.StudentNumber);
                }

                isChanged = false;
                NavigationManager.NavigateTo("/RentalPages/RentalList");
            }
            catch (Exception ex)
            {
                ErrorList.Add($"Error saving rental: {ex.Message}");
            }
        }

    }
}
