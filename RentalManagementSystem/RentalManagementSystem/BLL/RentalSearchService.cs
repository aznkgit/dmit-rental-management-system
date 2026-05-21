using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentalManagementSystem.DAL;
using RentalManagementSystem.Entities;
using RentalManagementSystem.ViewModels;

namespace RentalManagementSystem.BLL
{
    public class RentalSearchService
    {
        private readonly StarTed2024OctContext _starTed2024OctContext;

        internal RentalSearchService(StarTed2024OctContext starTed2024OctContext)
        {
            _starTed2024OctContext = starTed2024OctContext;
        }

        public List<RentalSearchView> GetRentals(string city, string community)
        {
            // error list 
            List<Exception> errorList = new List<Exception>();

            // at least one of city or community must be provided
            if (string.IsNullOrWhiteSpace(city) && string.IsNullOrWhiteSpace(community))
            {
                throw new ArgumentException("A City or Community must be provided.");
            }

            return _starTed2024OctContext.Rentals
                .Where(rental => !rental.RemoveFromViewFlag &&
                            (string.IsNullOrWhiteSpace(city) || rental.Address.City == city) &&
                            (string.IsNullOrWhiteSpace(community) || rental.Address.Community == community))
                .Select(rental => new RentalSearchView
                {
                    RentalID = rental.RentalID,
                    City = rental.Address.City,
                    Community = rental.Address.Community,
                    RentalType = rental.RentalType.Description,
                    MaxVacancy = rental.MaxVacancy,
                    AvailableVacancy = rental.MaxVacancy - rental.Renters.Count(renter => !renter.RemoveFromViewFlag),
                    MonthlyRent = rental.MonthlyRent,
                    DamageDeposit = rental.DamageDeposit
                })
                .OrderBy(anon => anon.City)
                .ThenBy(anon => anon.Community)
                .ToList();
        }

    }
}
