using System;
using System.Collections.Generic;
using System.Linq;
using RentalManagementSystem.DAL;
using RentalManagementSystem.Entities;
using RentalManagementSystem.ViewModels;

namespace RentalManagementSystem.BLL
{
    public class RentalTypeService
    {
        private readonly StarTed2024OctContext _starTed2024OctContext;

        internal RentalTypeService(StarTed2024OctContext starTed2024OctContext)
        {
            _starTed2024OctContext = starTed2024OctContext;
        }

        public List<RentalTypeView> GetRentalTypes()
        {
            return _starTed2024OctContext.RentalTypes
                .Where(rentalType => !rentalType.RemoveFromViewFlag) 
                .Select(rentalType => new RentalTypeView
                {
                    RentalTypeID = rentalType.RentalTypeID,
                    Description = rentalType.Description
                })
                .OrderBy(anon => anon.Description)
                .ToList();
        }
    }
}
