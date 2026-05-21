using Microsoft.AspNetCore.Components;
using RentalManagementSystem.BLL;
using RentalManagementSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentalManagementWebApp.Components.RentalPages
{
    public partial class RentalList
    {
        [Inject]
        private RentalSearchService RentalService { get; set; } = default!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        private string? City { get; set; }
        private string? Community { get; set; }
        private List<RentalSearchView>? Rentals { get; set; }
        //error message list
        private List<string> ErrorList { get; set; } = new(); 
        private bool isLoading = false;

        private async Task SearchRentals()
        {
            // Clear previous errors
            ErrorList.Clear();

            if (string.IsNullOrWhiteSpace(City) && string.IsNullOrWhiteSpace(Community))
            {
                ErrorList.Add("Please provide a City or Community for the search.");
                return;
            }

            isLoading = true;
            try
            {
                // Retrieve rentals based on input criteria
                Rentals = await Task.Run(() => RentalService.GetRentals(City, Community));

                if (Rentals != null && Rentals.Count > 0)
                {
                    Console.WriteLine($"Retrieved {Rentals.Count} rentals.");
                }
                else
                {
                    ErrorList.Add("No rentals match the search criteria.");
                }
            }
            catch (Exception ex)
            {
                ErrorList.Add($"Error fetching rentals: {ex.Message}");
                Rentals = new List<RentalSearchView>(); // clear rentals on error
            }
            finally
            {
                isLoading = false;
            }
        }

        private void CreateNewRental()
        {
            // Redirect to new rental page

            NavigationManager.NavigateTo("/RentalPages/RentalEdit/0");
        }

        private void EditRental(int rentalId)
        {
            // Redirect to specific rental edit based on id
            NavigationManager.NavigateTo($"/RentalPages/RentalEdit/{rentalId}");
        }
    }
}
