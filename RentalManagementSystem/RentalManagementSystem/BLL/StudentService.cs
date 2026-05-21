using RentalManagementSystem.DAL;
using RentalManagementSystem.Entities;
using RentalManagementSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentalManagementSystem.BLL
{
    public class StudentService
    {
        private readonly StarTed2024OctContext _context;

        internal StudentService(StarTed2024OctContext context)
        {
            _context = context;
        }

        public List<StudentView> GetStudentsByLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentException("Student last name must be provided.");
            }

            return _context.Students
                .Where(student => !student.RemoveFromViewFlag && student.LastName.Contains(lastName))
                .Select(student => new StudentView
                {
                    StudentNumber = student.StudentNumber,
                    FirstName = student.FirstName,
                    LastName = student.LastName
                })
                .OrderBy(anon => anon.LastName)
                .ThenBy(anon => anon.FirstName)
                .ToList();
        }
    }
}
