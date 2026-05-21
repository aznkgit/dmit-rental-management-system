using System;
using System.Linq;
using RentalManagementSystem.DAL;
using RentalManagementSystem.Entities;
using RentalManagementSystem.ViewModels;

namespace RentalManagementSystem.BLL
{
    public class RentalEditService
    {
        private readonly StarTed2024OctContext _context;

        internal RentalEditService(StarTed2024OctContext context)
        {
            _context = context;
        }

        public int SaveRental(int rentalId, RentalView rentalDetails, AddressView address)
        {
            if (rentalDetails == null) throw new ArgumentNullException(nameof(rentalDetails));
            if (address == null) throw new ArgumentNullException(nameof(address));

            if (rentalId == 0)
            {
                var newAddress = new Address
                {
                    Number = address.Number ?? string.Empty,
                    Unit = address.Unit,
                    Street = address.Street ?? string.Empty,
                    Community = address.Community,
                    City = address.City ?? string.Empty,
                    ProvinceState = address.ProvinceState,
                    PostalCodeZip = address.PostalCodeZip,
                    CountryCode = address.CountryCode,
                    LandLordID = address.LandLordID,
                    RemoveFromViewFlag = false
                };
                _context.Addresses.Add(newAddress);
                _context.SaveChanges();

                var newRental = new Rental
                {
                    AddressID = newAddress.AddressID,
                    RentalTypeID = rentalDetails.RentalTypeID,
                    MonthlyRent = rentalDetails.MonthlyRent,
                    DamageDeposit = rentalDetails.DamageDeposit,
                    MaxVacancy = rentalDetails.MaxVacancy,
                    AvailableDate = rentalDetails.AvailableDate,
                    RemoveFromViewFlag = false
                };
                _context.Rentals.Add(newRental);
                _context.SaveChanges();
                return newRental.RentalID;
            }
            else
            {
                var existingRental = _context.Rentals
                    .FirstOrDefault(r => r.RentalID == rentalId && !r.RemoveFromViewFlag);

                if (existingRental == null)
                    throw new InvalidOperationException($"Rental {rentalId} not found.");

                var existingAddress = _context.Addresses
                    .FirstOrDefault(a => a.AddressID == existingRental.AddressID);

                if (existingAddress != null)
                {
                    existingAddress.Unit = address.Unit;
                    existingAddress.Street = address.Street ?? existingAddress.Street;
                    existingAddress.Community = address.Community;
                    existingAddress.City = address.City ?? existingAddress.City;
                }

                existingRental.RentalTypeID = rentalDetails.RentalTypeID;
                existingRental.MonthlyRent = rentalDetails.MonthlyRent;
                existingRental.DamageDeposit = rentalDetails.DamageDeposit;
                existingRental.MaxVacancy = rentalDetails.MaxVacancy;
                existingRental.AvailableDate = rentalDetails.AvailableDate;

                _context.SaveChanges();
                return rentalId;
            }
        }

        public RenterView AddRenter(int rentalId, int studentNumber)
        {
            var alreadyRenter = _context.Renters
                .Any(r => r.RentalID == rentalId && r.StudentNumber == studentNumber && !r.RemoveFromViewFlag);

            if (alreadyRenter)
                throw new InvalidOperationException("This student is already a renter for this rental.");

            var renter = new Renter
            {
                RentalID = rentalId,
                StudentNumber = studentNumber,
                EffectiveDate = DateTime.Today,
                Active = true,
                RemoveFromViewFlag = false
            };
            _context.Renters.Add(renter);
            _context.SaveChanges();

            var student = _context.Students.Find(studentNumber);
            return new RenterView
            {
                RenterID = renter.RenterID,
                StudentNumber = studentNumber,
                StudentName = student != null ? $"{student.FirstName} {student.LastName}" : studentNumber.ToString(),
                EffectiveDate = renter.EffectiveDate,
                Active = renter.Active
            };
        }

        public void RemoveRenter(int renterId)
        {
            var renter = _context.Renters.Find(renterId);
            if (renter == null)
                throw new InvalidOperationException($"Renter {renterId} not found.");

            renter.RemoveFromViewFlag = true;
            _context.SaveChanges();
        }
    }
}
