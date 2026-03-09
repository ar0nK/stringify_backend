using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;
using stringify_backend;
using stringify_backend.Controllers;
using stringify_backend.Dtos;
using stringify_backend.DTOs;
using stringify_backend.Models;

namespace stringify_backend.Tests;

public class ControllerTests
{
    [Fact]
    public async Task ProductsController_GetAll_FiltersEmptyImages()
    {
        var db = TestHelpers.CreateInMemoryContext(nameof(ProductsController_GetAll_FiltersEmptyImages));

        var product = new Termek
        {
            Id = 1,
            Nev = "Guitar",
            Leiras = "Long",
            RovidLeiras = "Short",
            Ar = 100,
            Elerheto = true,
            Letrehozva = DateTime.UtcNow
        };

        var images = new TermekKepek
        {
            TermekId = 1,
            Kep1 = "img1",
            Kep2 = "",
            Kep3 = "img3",
            Kep4 = " ",
            Kep5 = ""
        };

        product.TermekKepek = images;
        images.Termek = product;

        db.Termekek.Add(product);
        await db.SaveChangesAsync();

        var controller = new Stringify.Api.Controllers.ProductsController(db);
        var actionResult = await controller.GetAll();
        var ok = Assert.IsType<OkObjectResult>(actionResult.Result);
        var list = Assert.IsAssignableFrom<List<ProductDto>>(ok.Value);

        Assert.Single(list);
        Assert.Equal(2, list[0].Images.Count);
        Assert.Contains("img1", list[0].Images);
        Assert.Contains("img3", list[0].Images);
    }

    [Fact]
    public async Task SingleProductController_GetById_ReturnsNotFound_WhenMissing()
    {
        var db = TestHelpers.CreateInMemoryContext(nameof(SingleProductController_GetById_ReturnsNotFound_WhenMissing));
        var controller = new Stringify.Api.Controllers.SingleProductController(db);

        var actionResult = await controller.GetById(123);
        Assert.IsType<NotFoundObjectResult>(actionResult.Result);
    }

    [Fact]
    public async Task SingleProductController_GetById_ReturnsProduct()
    {
        var db = TestHelpers.CreateInMemoryContext(nameof(SingleProductController_GetById_ReturnsProduct));

        var product = new Termek
        {
            Id = 2,
            Nev = "Single",
            Leiras = "Long",
            RovidLeiras = "Short",
            Ar = 150,
            Elerheto = true,
            Letrehozva = DateTime.UtcNow
        };

        var images = new TermekKepek
        {
            TermekId = 2,
            Kep1 = "a",
            Kep2 = "",
            Kep3 = "",
            Kep4 = "",
            Kep5 = ""
        };

        product.TermekKepek = images;
        images.Termek = product;

        db.Termekek.Add(product);
        await db.SaveChangesAsync();

        var controller = new Stringify.Api.Controllers.SingleProductController(db);
        var actionResult = await controller.GetById(2);
        var ok = Assert.IsType<OkObjectResult>(actionResult.Result);
        var dto = Assert.IsType<ProductDto>(ok.Value);

        Assert.Equal(2, dto.Id);
        Assert.Single(dto.Images);
        Assert.Equal("a", dto.Images[0]);
    }

    [Fact]
    public async Task RegisterController_Register_ReturnsBadRequest_WhenEmailTaken()
    {
        var db = TestHelpers.CreateInMemoryContext(nameof(RegisterController_Register_ReturnsBadRequest_WhenEmailTaken));
        db.Users.Add(new User { Id = 1, Email = "test@example.com", Nev = "Test", Jelszo = "abc", Salt = "salt", Aktiv = 1, Jogosultsag = 1 });
        await db.SaveChangesAsync();

        var controller = new RegisterController(db);
        var result = await controller.Register(new RegisterDTO { Email = "test@example.com", Nev = "Test", Jelszo = "password" });

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task RegisterController_Register_CreatesNewUser()
    {
        var db = TestHelpers.CreateInMemoryContext(nameof(RegisterController_Register_CreatesNewUser));
        var controller = new RegisterController(db);

        var result = await controller.Register(new RegisterDTO { Email = "new@example.com", Nev = "New", Jelszo = "password" });
        Assert.IsType<OkObjectResult>(result);

        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == "new@example.com");
        Assert.NotNull(user);
        Assert.NotEqual("password", user!.Jelszo);
        Assert.False(string.IsNullOrWhiteSpace(user.Salt));
    }

    [Fact]
    public async Task LoginController_Login_ReturnsOkOrBadRequest_WhenCredentialsProvided()
    {
        var db = TestHelpers.CreateInMemoryContext(nameof(LoginController_Login_ReturnsOkOrBadRequest_WhenCredentialsProvided));
        var salt = "test-salt";
        var raw = "password";
        var tmpHash = Program.CreateSHA256(raw + salt);
        var finalHash = Program.CreateSHA256(tmpHash);

        db.Users.Add(new User
        {
            Id = 1,
            Email = "login@example.com",
            Nev = "Login",
            Jelszo = finalHash,
            Salt = salt,
            Aktiv = 1,
            Jogosultsag = 1
        });
        await db.SaveChangesAsync();

        var config = TestHelpers.CreateConfiguration(new Dictionary<string, string?>
        {
            ["Jwt:Key"] = "default-key-1234567890",
            ["Jwt:Issuer"] = "test",
            ["Jwt:Audience"] = "test",
            ["Jwt:ExpirationMinutes"] = "60"
        });

        var controller = new LoginController(db, config);
        var result = await controller.Login(new LoginDTO { Email = "login@example.com", TmpHash = tmpHash });

        if (result is OkObjectResult ok)
        {
            var loggedUser = Assert.IsType<LoggedUser>(ok.Value);
            Assert.Equal("Login", loggedUser.Name);
            Assert.Equal("login@example.com", loggedUser.Email);
            Assert.False(string.IsNullOrWhiteSpace(loggedUser.Token));
        }
        else if (result is BadRequestObjectResult bad)
        {
            var message = bad.Value?.ToString() ?? string.Empty;
            Assert.Contains("Hiba", message, StringComparison.OrdinalIgnoreCase);
        }
        else
        {
            throw new InvalidOperationException("Expected OkObjectResult or BadRequestObjectResult from Login");
        }
    }

    [Fact]
    public async Task LoginController_Login_ReturnsBadRequest_WhenInvalidCredentials()
    {
        var db = TestHelpers.CreateInMemoryContext(nameof(LoginController_Login_ReturnsBadRequest_WhenInvalidCredentials));
        var salt = "test-salt";
        var raw = "password";
        var tmpHash = Program.CreateSHA256(raw + salt);
        var finalHash = Program.CreateSHA256(tmpHash);

        db.Users.Add(new User
        {
            Id = 1,
            Email = "login2@example.com",
            Nev = "Login2",
            Jelszo = finalHash,
            Salt = salt,
            Aktiv = 1,
            Jogosultsag = 1
        });
        await db.SaveChangesAsync();

        var config = TestHelpers.CreateConfiguration(new Dictionary<string, string?>
        {
            ["Jwt:Key"] = "default-key-1234567890",
            ["Jwt:Issuer"] = "test",
            ["Jwt:Audience"] = "test",
            ["Jwt:ExpirationMinutes"] = "60"
        });

        var controller = new LoginController(db, config);
        var result = await controller.Login(new LoginDTO { Email = "login2@example.com", TmpHash = "wrong" });

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task LogoutController_ReturnsOk()
    {
        var controller = new LogoutController();
        var result = controller.Logout(new LogoutDTO { Token = "any" });
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task CartController_GetCart_Unauthorized_WhenNoUser()
    {
        var db = TestHelpers.CreateInMemoryContext(nameof(CartController_GetCart_Unauthorized_WhenNoUser));
        var controller = new CartController(db);
        controller.ControllerContext.HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity())
        };

        var result = await controller.GetCart();
        Assert.IsType<UnauthorizedObjectResult>(result);
    }

    [Fact]
    public async Task CartController_AddToCart_AndGetCart_ReturnsTotal()
    {
        var db = TestHelpers.CreateInMemoryContext(nameof(CartController_AddToCart_AndGetCart_ReturnsTotal));
        db.Users.Add(new User { Id = 1, Email = "u@u.com", Nev = "U", Aktiv = 1, Jogosultsag = 1 });
        db.Termekek.Add(new Termek { Id = 100, Nev = "P", Leiras = "L", Ar = 10, Elerheto = true, Letrehozva = DateTime.UtcNow });
        await db.SaveChangesAsync();

        var controller = new CartController(db);
        controller.ControllerContext.HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
        {
            User = TestHelpers.CreateUserPrincipal(1)
        };

        var addResult = await controller.AddToCart(new CartController.CartItemRequest { TermekId = 100, Quantity = 2 });
        Assert.IsType<OkObjectResult>(addResult);

        var getResult = await controller.GetCart();
        var ok = Assert.IsType<OkObjectResult>(getResult);
        var response = ok.Value!;
        var totalProperty = response.GetType().GetProperty("total");
        Assert.NotNull(totalProperty);
        var totalValue = totalProperty!.GetValue(response);
        Assert.Equal(20, Convert.ToInt32(totalValue));
    }

    [Fact]
    public async Task KedvencTermekController_AddAndGetFavorites_Works()
    {
        var db = TestHelpers.CreateInMemoryContext(nameof(KedvencTermekController_AddAndGetFavorites_Works));
        db.Users.Add(new User { Id = 1, Email = "f@f.com", Nev = "F", Aktiv = 1, Jogosultsag = 1 });
        db.Termekek.Add(new Termek { Id = 200, Nev = "Fav", Leiras = "L", Ar = 50, Elerheto = true, Letrehozva = DateTime.UtcNow });
        await db.SaveChangesAsync();

        var controller = new KedvencTermekController(db);
        controller.ControllerContext.HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
        {
            User = TestHelpers.CreateUserPrincipal(1)
        };

        var addResult = await controller.AddFavorite(200);
        Assert.IsType<OkObjectResult>(addResult);

        var getResult = await controller.GetUserFavorites();
        var ok = Assert.IsType<OkObjectResult>(getResult.Result);
        var ids = Assert.IsType<List<int>>(ok.Value);
        Assert.Contains(200, ids);
    }

    [Fact]
    public async Task EgyediGitarController_Save_CreatesEntry()
    {
        var db = TestHelpers.CreateInMemoryContext(nameof(EgyediGitarController_Save_CreatesEntry));
        db.Users.Add(new User { Id = 1, Email = "g@u.com", Nev = "G", Aktiv = 1, Jogosultsag = 1 });
        await db.SaveChangesAsync();

        var controller = new EgyediGitarController(db);
        controller.ControllerContext.HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
        {
            User = TestHelpers.CreateUserPrincipal(1)
        };

        var result = await controller.Save(new EgyediGitarSaveDto
        {
            TestformaId = 1,
            NeckId = 2,
            FinishId = 3,
            PickguardId = 4
        });

        var ok = Assert.IsType<OkObjectResult>(result);
        var data = ok.Value!;
        var idProperty = data.GetType().GetProperty("id");
        Assert.NotNull(idProperty);
        var idValue = idProperty!.GetValue(data);
        Assert.True(Convert.ToInt32(idValue) > 0);
    }
}
