using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WatchWeb.Model.EFContext;

namespace WatchWeb.Common.Helpers
{
    public static class DbContextHelper
    {
        public static DataContext GetDbContext(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<DataContext>();
            return dbContext;
        }
    }
}
