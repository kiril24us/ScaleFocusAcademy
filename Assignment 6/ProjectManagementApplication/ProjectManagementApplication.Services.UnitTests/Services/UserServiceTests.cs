using Moq;
using ProjectManagementApplication.Data.Entities;
using ProjectManagementApplication.Data.Interfaces;
using ProjectManagementApplication.Services.Enums;
using ProjectManagementApplication.Services.Services;
using System.Collections.Generic;
using Xunit;

namespace ProjectManagementApplication.Services.UnitTests.Services
{   
    public class UserServiceTests
    {
        [Fact]
        public void CreateUser_ValidData_ReturnsSuccess()
        {
            //arrange
            var userRepositoryStub = new Mock<IUserRepository>();
            var teamReositoryStub = new Mock<ITeamRepository>();

            var sut = new UserService(userRepositoryStub.Object, teamReositoryStub.Object);

            //act
            var result = sut.CreateUser("test", "test", "test", "test", 1, 1);

            //assert
            Assert.Equal(Messages.Success, result.Result);
        }
       
        [Fact]
        public void CreateUser_ExistingUsernameOrPassword_ReturnsChangeUsernameOrPassword()
        {
            //arrange
            var userRepositoryStub = new Mock<IUserRepository>();
            var teamRepositoryStub = new Mock<ITeamRepository>();

            userRepositoryStub.Setup(userRepo => userRepo.GetUserByUsernameAndPassword(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new User());
            var sut = new UserService(userRepositoryStub.Object, teamRepositoryStub.Object);

            //act
            var result = sut.CreateUser("test", "test", "test", "test", 1, 1);

            //assert
            Assert.Equal(Messages.ChangeUsernameOrPassword, result.Result);
        }

        [Fact]
        public void DeleteUser_ValidData_ReturnsTrue()
        {
            //arrange
            var userRepositoryStub = new Mock<IUserRepository>();
            var teamReositoryStub = new Mock<ITeamRepository>();

            userRepositoryStub.Setup(userRepo => userRepo.GetUserById(It.IsAny<int>())).ReturnsAsync(new User());
            userRepositoryStub.Setup(userRepo => userRepo.Edit()).ReturnsAsync(true);

            var sut = new UserService(userRepositoryStub.Object, teamReositoryStub.Object);

            //act
            var result = sut.DeleteUser(1);

            //assert
            Assert.True(result.Result);
        }

        [Fact]
        public void DeleteUser_NoSuchUser_ReturnsFalse()
        {
            //arrange
            var userRepositoryStub = new Mock<IUserRepository>();
            var teamReositoryStub = new Mock<ITeamRepository>();

            userRepositoryStub.Setup(userRepo => userRepo.GetUserById(It.IsAny<int>())).ReturnsAsync((User)null);

            var sut = new UserService(userRepositoryStub.Object, teamReositoryStub.Object);

            //act
            var result = sut.DeleteUser(1);

            //assert
            Assert.False(result.Result);
        }

        [Fact]
        public void DeleteUser_ValidData_CallsEdit()
        {
            //arrange
            var userRepositoryStub = new Mock<IUserRepository>();
            var teamRepositoryStub = new Mock<ITeamRepository>();
            userRepositoryStub.Setup(userRepo => userRepo.GetUserById(It.IsAny<int>())).ReturnsAsync(new User());

            var sut = new UserService(userRepositoryStub.Object, teamRepositoryStub.Object);

            //act
            var result = sut.DeleteUser(1);

            //assert
            userRepositoryStub.Verify(x => x.Edit(), Times.Once);
        }

        [Fact]
        public void EditUser_ValidData_ReturnsTrue()
        {
            //arrange
            var userRepositoryStub = new Mock<IUserRepository>();
            var teamReositoryStub = new Mock<ITeamRepository>();

            userRepositoryStub.Setup(userRepo => userRepo.GetUserById(It.IsAny<int>())).ReturnsAsync(new User());
            userRepositoryStub.Setup(userRepo => userRepo.Edit()).ReturnsAsync(true);

            var sut = new UserService(userRepositoryStub.Object, teamReositoryStub.Object);

            //act
            var result = sut.EditUser(1, "test", "test", "test", "test");

            //assert
            Assert.True(result.Result);
        }

        [Fact]
        public void EditUser_NoSuchUser_ReturnsFalse()
        {
            //arrange
            var userRepositoryStub = new Mock<IUserRepository>();
            var teamReositoryStub = new Mock<ITeamRepository>();

            userRepositoryStub.Setup(userRepo => userRepo.GetUserById(It.IsAny<int>())).ReturnsAsync((User)null);

            var sut = new UserService(userRepositoryStub.Object, teamReositoryStub.Object);

            //act
            var result = sut.EditUser(1, "test", "test", "test", "test");

            //assert
            Assert.False(result.Result);
        }

        [Fact]
        public void GetAllUsers_Valid_ShouldReturnRepositoryCollection()
        {
            //arrange
            var userRepositoryStub = new Mock<IUserRepository>();
            var teamReositoryStub = new Mock<ITeamRepository>();

            List<User> users = new List<User>
            {
                new User { },
                new User { },
                new User { }
            };

            userRepositoryStub.Setup(userRepo => userRepo.GetAll()).ReturnsAsync(users);

            var sut = new UserService(userRepositoryStub.Object, teamReositoryStub.Object);

            //act
            var result = sut.GetAllUsers();

            //assert
            Assert.Equal(users, result.Result);
        }      
    }
}
