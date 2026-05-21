using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RentalManagementSystem.BLL;
using RentalManagementSystem.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalManagementSystem
{
    public static class StarTedExtension
    {
        public static void AddRentalManagementDependencies(this IServiceCollection services,
            Action<DbContextOptionsBuilder> options)
        {
            services.AddDbContext<StarTed2024OctContext>(options);

            // register services from BLL

            // RentalSearchService
            services.AddTransient<RentalSearchService>((serviceProvider) =>
            {
                var context = serviceProvider.GetService<StarTed2024OctContext>();

                return new RentalSearchService(context);
            });

            // RentalTypeService
            services.AddTransient<RentalTypeService>((serviceProvider) =>
            {
                var context = serviceProvider.GetService<StarTed2024OctContext>();

                return new RentalTypeService(context);
            });

            // AddressService
            services.AddTransient<AddressService>((serviceProvider) =>
            {
                var context = serviceProvider.GetService<StarTed2024OctContext>();

                return new AddressService(context);
            });

            // RentalDetailsService
            services.AddTransient<RentalDetailsService>((serviceProvider) =>
            {
                var context = serviceProvider.GetService<StarTed2024OctContext>();

                return new RentalDetailsService(context);
            });

            // StudentService
            services.AddTransient<StudentService>((serviceProvider) =>
            {
                var context = serviceProvider.GetService<StarTed2024OctContext>();

                return new StudentService(context);
            });

            // RentalEditService
            services.AddTransient<RentalEditService>((serviceProvider) =>
            {
                var context = serviceProvider.GetService<StarTed2024OctContext>();

                return new RentalEditService(context);
            });
        }
    }
}
