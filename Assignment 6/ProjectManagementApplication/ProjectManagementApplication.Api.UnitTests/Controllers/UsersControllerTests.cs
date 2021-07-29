using Microsoft.AspNetCore.Mvc;
using Moq;
using ProjectManagementApplication.Api.Controllers;
using ProjectManagementApplication.Data.Entities;
using ProjectManagementApplication.Data.Enums;
using ProjectManagementApplication.DTO.Requests.UserRequests;
using ProjectManagementApplication.DTO.Responses.UserResponses;
using ProjectManagementApplication.Services.Enums;
using ProjectManagementApplication.Services.Interfaces;
using System;
using System.Collections.Generic;
using Xunit;

namespace ProjectManagementApplication.Api.UnitTests.Controllers
{
    public class UsersControllerTests
    {
        [Fact]
        public void Login_ValidUser_ReturnsStatusCode200()
        {
            //arrange
            Mock<IUserService> userServiceStub = new Mock<IUserService>();
            Mock<IAuthentication> authenticationStub = new Mock<IAuthentication>();
            Mock<UserLoginRequestDTO> userLoginRequest = new Mock<UserLoginRequestDTO>();

            authenticationStub.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new User());
            var sut = new UsersController(userServiceStub.Object, authenticationStub.Object);

            //act
            var result = sut.Login(userLoginRequest.Object);

            //assert
            ObjectResult objectResponse = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, objectResponse.StatusCode);
        }

        [Fact]
        public void Login_InvalidUser_ReturnsBadRequest()
        {
            //arrange
            Mock<IUserService> userServiceStub = new Mock<IUserService>();
            Mock<IAuthentication> authenticationStub = new Mock<IAuthentication>();
            Mock<UserLoginRequestDTO> userLoginRequest = new Mock<UserLoginRequestDTO>();

            authenticationStub.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((User)null);

            var sut = new UsersController(userServiceStub.Object, authenticationStub.Object);

            //act
            var result = sut.Login(userLoginRequest.Object);

            //assert
            ObjectResult objectResponse = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, objectResponse.StatusCode);
        }

        [Fact]
        public void Create_InvalidDataUsernameOrPassword_ReturnsBadRequest()
        {
            //arrange
            Mock<IUserService> userServiceStub = new Mock<IUserService>();
            Mock<IAuthentication> authenticationStub = new Mock<IAuthentication>();
            Mock<UserCreateRequestDTO> userCreateRequestDTO = new Mock<UserCreateRequestDTO>();

            userServiceStub.Setup(x => x.CreateUser(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<int>())).ReturnsAsync(Messages.ChangeUsernameOrPassword);

            var sut = new UsersController(userServiceStub.Object, authenticationStub.Object);

            //act
            var result = sut.Create(userCreateRequestDTO.Object);

            //assert
            ObjectResult objectResponse = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, objectResponse.StatusCode);
        }

        [Fact]
        public void Create_InvalidDataTeamNotFound_ReturnsBadRequest()
        {
            //arrange
            Mock<IUserService> userServiceStub = new Mock<IUserService>();
            Mock<IAuthentication> authenticationStub = new Mock<IAuthentication>();
            Mock<UserCreateRequestDTO> userCreateRequestDTO = new Mock<UserCreateRequestDTO>();

            userServiceStub.Setup(x => x.CreateUser(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<int>())).ReturnsAsync(Messages.TeamNotFound);

            var sut = new UsersController(userServiceStub.Object, authenticationStub.Object);

            //act
            var result = sut.Create(userCreateRequestDTO.Object);

            //assert
            ObjectResult objectResponse = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, objectResponse.StatusCode);
        }

        [Fact]
        public void Create_ValidData_ReturnsSuccess()
        {
            //arrange
            Mock<IUserService> userServiceStub = new Mock<IUserService>();
            Mock<IAuthentication> authenticationStub = new Mock<IAuthentication>();
            Mock<UserCreateRequestDTO> userCreateRequestDTO = new Mock<UserCreateRequestDTO>();
            userServiceStub.Setup(x => x.GetUserByUsernameAndPassword(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new User
                {
                    Id = 1,
                    Username = "test",
                    FirstName = "test",
                    LastName = "test",
                    Role = Enum.Parse<Role>(1.ToString()),
                    Teams = new List<Team> { new Team { Name = "test" } }
                });

            userServiceStub.Setup(x => x.CreateUser(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<int>())).ReturnsAsync(Messages.Success);

            var sut = new UsersController(userServiceStub.Object, authenticationStub.Object);

            //act
            var result = sut.Create(userCreateRequestDTO.Object);

            //assert
            ObjectResult objectResponse = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(201, objectResponse.StatusCode);
        }

        [Fact]
        public void Create_DefaultSwitchCase_ThrowsArgumentOutOfRangeException()
        {
            //arrange
            Mock<IUserService> userServiceStub = new Mock<IUserService>();
            Mock<IAuthentication> authenticationStub = new Mock<IAuthentication>();
            Mock<UserCreateRequestDTO> userCreateRequestDTO = new Mock<UserCreateRequestDTO>();

            userServiceStub.Setup(x => x.CreateUser(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<int>())).Throws(new ArgumentOutOfRangeException());

            var sut = new UsersController(userServiceStub.Object, authenticationStub.Object);

            //act
            var result = sut.Create(userCreateRequestDTO.Object);

            //assert    
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => result);
        }

        [Fact]
        public void Delete_ValidData_ReturnsSuccess()
        {
            //arrange
            Mock<IUserService> userServiceStub = new Mock<IUserService>();
            Mock<IAuthentication> authenticationStub = new Mock<IAuthentication>();

            userServiceStub.Setup(x => x.DeleteUser(It.IsAny<int>())).ReturnsAsync(true);

            var sut = new UsersController(userServiceStub.Object, authenticationStub.Object);

            //act
            var result = sut.Delete(It.IsAny<int>());

            //assert
            ObjectResult objectResponse = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, objectResponse.StatusCode);
        }

        [Fact]
        public void Delete_InvalidData_ReturnsBadRequest()
        {
            //arrange
            Mock<IUserService> userServiceStub = new Mock<IUserService>();
            Mock<IAuthentication> authenticationStub = new Mock<IAuthentication>();

            userServiceStub.Setup(x => x.DeleteUser(It.IsAny<int>())).ReturnsAsync(false);

            var sut = new UsersController(userServiceStub.Object, authenticationStub.Object);

            //act
            var result = sut.Delete(It.IsAny<int>());

            //assert
            ObjectResult objectResponse = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, objectResponse.StatusCode);
        }

        [Fact]
        public void Edit_ValidData_ReturnsSuccess()
        {
            //arrange
            Mock<IUserService> userServiceStub = new Mock<IUserService>();
            Mock<IAuthentication> authenticationStub = new Mock<IAuthentication>();
            Mock<UserEditRequestDTO> userEditRequestDTO = new Mock<UserEditRequestDTO>();

            userServiceStub.Setup(x => x.EditUser(
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>())).ReturnsAsync(true);

            var sut = new UsersController(userServiceStub.Object, authenticationStub.Object);

            //act
            var result = sut.Edit(It.IsAny<int>(), userEditRequestDTO.Object);

            //assert
            ObjectResult objectResponse = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, objectResponse.StatusCode);
        }

        [Fact]
        public void Edit_InvalidData_ReturnsBadRequest()
        {
            //arrange
            Mock<IUserService> userServiceStub = new Mock<IUserService>();
            Mock<IAuthentication> authenticationStub = new Mock<IAuthentication>();
            Mock<UserEditRequestDTO> userEditRequestDTO = new Mock<UserEditRequestDTO>();

            userServiceStub.Setup(x => x.EditUser(
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>())).ReturnsAsync(false);

            var sut = new UsersController(userServiceStub.Object, authenticationStub.Object);

            //act
            var result = sut.Edit(It.IsAny<int>(), userEditRequestDTO.Object);

            //assert
            ObjectResult objectResponse = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, objectResponse.StatusCode);
        }

        [Fact]
        public void GetAll_ValidData_ReturnsUsersDTO()
        {
            //arrange
            Mock<IUserService> userServiceStub = new Mock<IUserService>();
            Mock<IAuthentication> authenticationStub = new Mock<IAuthentication>();
            Mock<UserGetAllResponseDTO> userEditRequestDTO = new Mock<UserGetAllResponseDTO>();

            List<User> users = new List<User>
            {
                new User { },
                new User { },
                new User { }
            };

            userServiceStub.Setup(userRepo => userRepo.GetAllUsers()).ReturnsAsync(users);

            var sut = new UsersController(userServiceStub.Object, authenticationStub.Object);

            //act
            var result = sut.GetAll();

            //assert
            ObjectResult objectResponse = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, objectResponse.StatusCode);
        }
    }
}

