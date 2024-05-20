using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WatchWeb.Model.EFContext;

namespace WatchWeb.Model
{
    public static class SeedData
    {
        public static Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<DataContext>();

            try
            {
                var seedDataIds = PermissionSeed.AllPermissions.Select(x => x.Id).ToList();
                var currentPermission = context.Permission.Where(x => seedDataIds.Contains(x.Id)).Select(x => x.Id).ToList();
                var newPermission = seedDataIds
                .Where(d => !currentPermission.Contains(d))
                .ToList();

                if (newPermission.Any())
                {
                    var permissions = PermissionSeed.AllPermissions;

                    context.Permission.AddRange(permissions);
                    context.SaveChanges();
                }

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {

                throw;
            }


        }
    }
}
