using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using stringify_backend.Models;

namespace stringify_backend.Tests;

public static class TestHelpers
{
    public static StringifyDbContext CreateInMemoryContext(string databaseName)
    {
        var options = new DbContextOptionsBuilder<StringifyDbContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;

        var context = new StringifyDbContext(options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        return context;
    }

    public static IConfiguration CreateConfiguration(Dictionary<string, string?> values)
    {
        return new ConfigurationBuilder().AddInMemoryCollection(values).Build();
    }

    public static ClaimsPrincipal CreateUserPrincipal(int userId)
    {
        var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) }, "Test");
        return new ClaimsPrincipal(identity);
    }
}
