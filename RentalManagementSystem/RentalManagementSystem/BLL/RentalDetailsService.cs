using System;
using System.Collections.Generic;
using System.Linq;
using RentalManagementSystem.DAL;
using RentalManagementSystem.Entities;
using RentalManagementSystem.ViewModels;

namespace RentalManagementSystem.BLL
{
    public class RentalDetailsService
    {
        private readonly StarTed2024OctContext _starTed2024OctContext;

        internal RentalDetailsService(StarTed2024OctContext starTed2024OctContext)
        {
            _starTed2024OctContext = starTed2024OctContext;
        }

        public RentalView? GetRental(int rentalId)
        {
            if (rentalId < 0)
            {
                throw new ArgumentException("Rental ID must be greater than zero.");
            }

            return _starTed2024OctContext.Rentals
                .Where(rental => rental.RentalID == rentalId && !rental.RemoveFromViewFlag)
                .Select(rental => new RentalView
                {
                    RentalTypeID = rental.RentalTypeID,
                    DamageDeposit = rental.DamageDeposit,
                    MonthlyRent = rental.MonthlyRent,
                    MaxVacancy = rental.MaxVacancy,
                    AvailableDate = rental.AvailableDate,
                    Vacancies = rental.Renters.Count(renter => !renter.RemoveFromViewFlag),
                    Renters = rental.Renters
                        .Where(renter => !renter.RemoveFromViewFlag)
                        .Select(renter => new RenterView
                        {
                            RenterID = renter.RenterID,
                            StudentNumber = renter.StudentNumber,
                            StudentName = renter.StudentNumberNavigation.FirstName + " " + renter.StudentNumberNavigation.LastName,
                            EmergencyContactName = renter.EmergencyContactName,
                            EmergencyContactNumber = renter.EmergencyContactNumber,
                            EffectiveDate = renter.EffectiveDate,
                            Active = renter.Active
                        })
                        .ToList()
                })
                .FirstOrDefault();
        }
    }
}
