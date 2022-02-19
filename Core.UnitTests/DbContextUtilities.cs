using Core.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace Core.UnitTests
{
    public class DbContextUtilities
    {
        public static DbContextOptions<ApplicationDbContext> GetContextOptions()
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            return builder.Options;
        }
    }
}
