using Microsoft.AspNetCore.Mvc;
using Moq;
using ProjectManagementApplication.Api.Controllers;
using ProjectManagementApplication.Data.Data;
using ProjectManagementApplication.Data.Repositories;
using ProjectManagementApplication.DTO.Requests.TeamRequests;
using ProjectManagementApplication.DTO.Requests.UserRequests;
using ProjectManagementApplication.Services.Enums;
using ProjectManagementApplication.Services.Interfaces;
using ProjectManagementApplication.Services.Services;
using System;
using Xunit;

namespace ProjectManagementApplication.Api.UnitTests.Controllers
{
    public class TeamsControllerTests
    {
        [Fact]
        public void Create_InvalidTeamName_ReturnsBadRequest()
        {
            //arrange
            Mock<ITeamService> teamServiceStub = new Mock<ITeamService>();
            Mock<TeamBaseRequestDTO> teamBaseDTO= new Mock<TeamBaseRequestDTO>();
            Mock<IUserService> userServiceStub = new Mock<IUserService>();

            var tuple = new Tuple<Messages, int>(Messages.TeamAlreadyExist, 0);
            UserCreateRequestDTO userCreateRequestDTO = new UserCreateRequestDTO();

            teamServiceStub.Setup(x => x.CreateTeam(It.IsAny<string>())).ReturnsAsync(tuple);

            var sut = new TeamsController(teamServiceStub.Object);

            //act
            var result = sut.Create(teamBaseDTO.Object);

            //assert
            ObjectResult objectResponse = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, objectResponse.StatusCode);
        }

        [Fact]
        public void Create_ValidData_ReturnsSuccess()
        {
            //arrange
            Mock<ITeamService> teamServiceStub = new Mock<ITeamService>();
            Mock<IUserService> userServiceStub = new Mock<IUserService>();
            Mock<TeamBaseRequestDTO> teamBaseDTO = new Mock<TeamBaseRequestDTO>();
            var tuple = new Tuple<Messages, int>(Messages.Success, 0);

            teamServiceStub.Setup(x => x.CreateTeam(It.IsAny<string>())).ReturnsAsync(tuple);

            var sut = new TeamsController(teamServiceStub.Object);

            //act
            var result = sut.Create(teamBaseDTO.Object);

            //assert
            ObjectResult objectResponse = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(201, objectResponse.StatusCode);
        }

        [Fact]
        public void Delete_InvalidData_ReturnsBadRequest()
        {
            //arrange
            Mock<ITeamService> teamServiceStub = new Mock<ITeamService>();
            Mock<IUserService> userServiceStub = new Mock<IUserService>();
            Mock<TeamBaseRequestDTO> teamBaseDTO = new Mock<TeamBaseRequestDTO>();

            teamServiceStub.Setup(x => x.DeleteTeam(It.IsAny<int>())).ReturnsAsync(false);

            var sut = new TeamsController(teamServiceStub.Object);

            //act
            var result = sut.Delete(It.IsAny<int>());

            //assert
            ObjectResult objectResponse = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, objectResponse.StatusCode);
        }

        [Fact]
        public void Delete_ValidData_ReturnsSuccess()
        {
            //arrange
            Mock<ITeamService> teamServiceStub = new Mock<ITeamService>();
            Mock<IUserService> userServiceStub = new Mock<IUserService>();
            Mock<TeamBaseRequestDTO> teamBaseDTO = new Mock<TeamBaseRequestDTO>();

            teamServiceStub.Setup(x => x.DeleteTeam(It.IsAny<int>())).ReturnsAsync(true);

            var sut = new TeamsController(teamServiceStub.Object);

            //act
            var result = sut.Delete(It.IsAny<int>());

            //assert
            ObjectResult objectResponse = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, objectResponse.StatusCode);
        }

        [Fact]
        public void Edit_ValidData_ReturnsSuccess()
        {
            //arrange
            Mock<ITeamService> teamServiceStub = new Mock<ITeamService>();
            Mock<IUserService> userServiceStub = new Mock<IUserService>();
            Mock<TeamBaseRequestDTO> teamBaseDTO = new Mock<TeamBaseRequestDTO>();

            teamServiceStub.Setup(x => x.EditTeam(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);

            var sut = new TeamsController(teamServiceStub.Object);

            //act
            var result = sut.Edit(It.IsAny<int>(), teamBaseDTO.Object);

            //assert
            ObjectResult objectResponse = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, objectResponse.StatusCode);
        }        
    }
}
