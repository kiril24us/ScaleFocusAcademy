using Moq;
using ProjectManagementApplication.Data.Entities;
using ProjectManagementApplication.Data.Interfaces;
using ProjectManagementApplication.Services.Auth;
using Xunit;

namespace ProjectManagementApplication.Services.UnitTests.Services
{
    public class AuthenticationTests
    {
        [Fact]
        public void Login_ValidUsernameAndPassword_ShouldReturnAdminUser()
        {
            // arrange
            var userRepositoryStub = new Mock<IUserRepository>();

            User user = new User
            {
                Username = "admin",
                Password = "adminpass",
            };

            var sut = new Authentication(userRepositoryStub.Object);
            userRepositoryStub.Setup(userRepo => userRepo.GetUserByUsernameAndPassword("admin", "adminpass"))
                .ReturnsAsync(user);

            // act
            var result = sut.Login(user.Username, user.Password);

            // assert
            Assert.Equal("admin", result.Result.Username);
            Assert.Equal("adminpass", result.Result.Password);
        }
    }
}
