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
    public class AddressService
    {
        private readonly StarTed2024OctContext _starTed2024OctContext;

        internal AddressService(StarTed2024OctContext starTed2024OctContext)
        {
            _starTed2024OctContext = starTed2024OctContext;
        }

        public AddressView? GetAddress(int rentalId)
        {
            if (rentalId <= 0)
            {
                throw new ArgumentException("RentalID must be greater than zero.");
            }

            // retrieve address info based on rentalId argument
            var address = _starTed2024OctContext.Rentals
                .Where(rental => rental.RentalID == rentalId && !rental.RemoveFromViewFlag)
                .Select(rental => new AddressView
                {
                    AddressID = rental.Address.AddressID,
                    Unit = rental.Address.Unit,
                    Street = rental.Address.Street,
                    Community = rental.Address.Community,
                    City = rental.Address.City,
                    ProvinceState = rental.Address.ProvinceState,
                    CountryCode = rental.Address.CountryCode,
                    PostalCodeZip = rental.Address.PostalCodeZip,
                    LandLordID = rental.Address.LandLordID,
                    LandLord = rental.Address.LandLord.Name
                })
                .FirstOrDefault();

            return address;
        }
    }
}
