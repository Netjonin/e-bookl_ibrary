using NUnit.Framework;
using E_BOOKLIBRARY.Controllers;
using Microsoft.EntityFrameworkCore;
using E_BOOKLIBRARY.Models;
using Microsoft.AspNetCore.Identity;
using Moq;
using E_BOOKLIBRARY.Data.Db;
using E_BOOKLIBRARY.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using E_BOOKLIBRARY.Services;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;
using System;

namespace BookLibray.Tests.Services
{
    [TestFixture]
    class AuthTests
    {
        private IAuthService _auth;
        public Mock<UserManager<AppUser>> userManagerMock;
        public Mock<SignInManager<AppUser>> signinManagerMock;
        public Mock<IJWTService> jwtMock;

        [SetUp]
        public async Task Setup()
        {
            userManagerMock = new Mock<UserManager<AppUser>>(Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
            signinManagerMock = new Mock<SignInManager<AppUser>>(userManagerMock.Object,
                 new Mock<IHttpContextAccessor>().Object,
                 new Mock<IUserClaimsPrincipalFactory<AppUser>>().Object,
                 new Mock<IOptions<IdentityOptions>>().Object,
                 new Mock<ILogger<SignInManager<AppUser>>>().Object,
                 new Mock<IAuthenticationSchemeProvider>().Object,
                 new Mock<IUserConfirmation<AppUser>>().Object);
            jwtMock = new Mock<IJWTService>();


            _auth = new AuthService(jwtMock.Object, signinManagerMock.Object, userManagerMock.Object);
        }


        [Test]
        public async Task Login_ValidCred_ReturnLoginStatusAsTrue()
        {
            // Arrange
            var email = "test@email.com";
            var password = "123qweA";

            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(new AppUser { Email = email });
            userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<AppUser>())).ReturnsAsync(new List<string>());
            userManagerMock.Setup(x => x.GetClaimsAsync(It.IsAny<AppUser>())).ReturnsAsync(new List<Claim>());
            signinManagerMock.Setup(x => x.PasswordSignInAsync(It.IsAny<AppUser>(), It.IsAny<string>(), false, false)).ReturnsAsync(SignInResult.Success);
            jwtMock.Setup(x => x.GenerateToken(It.IsAny<AppUser>(), It.IsAny<List<string>>(), It.IsAny<IList<Claim>>())).Returns("Token");

            var expected = new LoginCredDTO
            {
                Id = It.IsAny<string>(),
                Token = It.IsAny<string>(),
                Status = true
            };

            // Act
            LoginCredDTO result = await _auth.Login(email, password, false);

            // Assert
            Assert.IsTrue(result.Status);
            Assert.IsInstanceOf<LoginCredDTO>(result);
        }

        [Test]
        public async Task Login_InvalidEmail_ReturnLoginStatusAsFalse()
        {
            // Arrange
            var email = "test@email.com";
            var password = "123qweA";

            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()));
            signinManagerMock.Setup(x => x.PasswordSignInAsync(It.IsAny<AppUser>(), It.IsAny<string>(), false, false)).ReturnsAsync(SignInResult.Failed);

            // Act
            var result = await _auth.Login(email, password, false);

            // Assert
            Assert.IsFalse(result.Status);
            Assert.IsInstanceOf<LoginCredDTO>(result);
        }

        [Test]
        public async Task Login_InvalidPassword_ReturnLoginStatusAsFalse()
        {
            // Arrange
            var email = "test@email.com";
            var password = "123qweA";

            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(new AppUser { Email = email });
            signinManagerMock.Setup(x => x.PasswordSignInAsync(It.IsAny<AppUser>(), It.IsAny<string>(), false, false)).ReturnsAsync(SignInResult.Failed);

            // Act
            var result = await _auth.Login(email, password, false);

            // Assert
            Assert.IsFalse(result.Status);
            Assert.IsInstanceOf<LoginCredDTO>(result);
        }

        [Test]
        public async Task GenerateEmailConfirmationToken_ExistingUser_ReturnToken()
        {
            var user = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@email.com"
            };
            userManagerMock.Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<AppUser>())).ReturnsAsync(Guid.NewGuid().ToString());

            var actual = await _auth.GenerateEmailConfirmationToken(user);

            Assert.That(actual.Length, Is.GreaterThan(0));
        }


        [Test]
        public async Task GenerateEmailConfirmationToken_NonExistingUser_ReturnEmptyToken()
        {
            var user = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@email.com"
            };
            userManagerMock.Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<AppUser>())).ReturnsAsync("");

            var actual = await _auth.GenerateEmailConfirmationToken(user);

            Assert.That(actual.Length, Is.EqualTo(0));
        }

        [Test]
        public async Task ConfirmEmail_ValidCredentials_ReturnSucceededIdentityResult()
        {
            var user = new AppUser
            {
                Email = "test@email.com",
                IsActive = false
            };
            var token = "YGsWUeHEIUWHetOHFEWI_H$EHIUtWrhsHFHdsEW8989Y8";
            userManagerMock.Setup(x => x.ConfirmEmailAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            var actual = await _auth.ConfirmEmail(user, token);

            Assert.IsTrue(actual.Succeeded);
        }

        [Test]
        public async Task ConfirmEmail_ValidCredentials_ActivatesUser()
        {
            var user = new AppUser
            {
                Email = "test@email.com",
                IsActive = false
            };
            var token = "YGsWUeHEIUWHetOHFEWI_H$EHIUtWrhsHFHdsEW8989Y8";
            userManagerMock.Setup(x => x.ConfirmEmailAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            var actual = await _auth.ConfirmEmail(user, token);

            Assert.IsTrue(user.IsActive);
        }


        [Test]
        public async Task Register_ValidCredentials_ReturnSucceededIdentityResult()
        {
            var user = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@email.com"
            };
            var password = "123qwe@Asd";
            userManagerMock.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            var actual = await _auth.Register(user, password);

            Assert.IsTrue(actual.Succeeded);
        }
    }
}