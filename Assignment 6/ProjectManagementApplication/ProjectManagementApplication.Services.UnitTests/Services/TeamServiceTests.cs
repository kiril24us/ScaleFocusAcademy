using Moq;
using ProjectManagementApplication.Data.Data;
using ProjectManagementApplication.Data.Entities;
using ProjectManagementApplication.Data.Interfaces;
using ProjectManagementApplication.Data.Repositories;
using ProjectManagementApplication.Services.Enums;
using ProjectManagementApplication.Services.Services;
using System.Collections.Generic;
using Xunit;

namespace ProjectManagementApplication.Services.UnitTests.Services
{
    public class TeamServiceTests
    {
        [Fact]
        public void AssignUser_ValidData_ReturnsSuccess()
        {
            //arrange
            var teamRepositoryStub = new Mock<ITeamRepository>();
            var userRepositoryStub = new Mock<IUserRepository>();
            userRepositoryStub.Setup(x => x.GetUserById(It.IsAny<int>())).ReturnsAsync(new User());
            teamRepositoryStub.Setup(x => x.GetTeamById(It.IsAny<int>())).ReturnsAsync(new Team());
            var sut = new TeamService(teamRepositoryStub.Object, userRepositoryStub.Object);

            //act
            var result = sut.AssignUser(1, 1);

            //assert
            Assert.Equal(Messages.Success, result.Result);
        }

        [Fact]
        public void CreateTeam_InvalidDataExistsTeamName_ReturnsTeamAlreadyExists()
        {
            //arrange
            var teamRepositoryStub = new Mock<ITeamRepository>();
            var userRepositoryStub = new Mock<IUserRepository>();
            teamRepositoryStub.Setup(x => x.GetTeamByName(It.IsAny<string>())).ReturnsAsync(new Team());
            var sut = new TeamService(teamRepositoryStub.Object, userRepositoryStub.Object);

            //act
            var result = sut.CreateTeam("test");

            //assert
            Assert.Equal(Messages.TeamAlreadyExist, result.Result.Item1);
        }

        [Fact]
        public void DeleteTeam_ValidData_CallsEdit()
        {
            //arrange
            var userRepositoryStub = new Mock<IUserRepository>();
            var teamRepositoryStub = new Mock<ITeamRepository>();
            teamRepositoryStub.Setup(teamRepo => teamRepo.GetTeamById(It.IsAny<int>())).ReturnsAsync(new Team());

            var sut = new TeamService(teamRepositoryStub.Object, userRepositoryStub.Object);

            //act
            var result = sut.DeleteTeam(1);

            //assert
            teamRepositoryStub.Verify(x => x.Edit(), Times.Once);
        }

        [Fact]
        public void GetAllTeams_Valid_ShouldReturnRepositoryCollection()
        {
            //arrange
            var userRepositoryStub = new Mock<IUserRepository>();
            var teamReositoryStub = new Mock<ITeamRepository>();

            List<Team> teams = new List<Team>
            {
                new Team { },
                new Team { },
                new Team { }
            };

            teamReositoryStub.Setup(teamRepo => teamRepo.GetAll()).ReturnsAsync(teams);

            var sut = new TeamService(teamReositoryStub.Object, userRepositoryStub.Object);

            //act
            var result = sut.GetAllTeams();

            //assert
            Assert.Equal(teams, result.Result);
        }

        [Fact]
        public void Create_ExistingTeamName_ReturnsTeamAlreadyExist()
        {
            //arrange
            var context = InMemoryContext.GetContext();
            Mock<TeamRepository> teamRepository = new Mock<TeamRepository>(context);
            Mock<UserRepository> userRepository = new Mock<UserRepository>(context);
            var teamService = new TeamService(teamRepository.Object, userRepository.Object);

            //act
            InMemoryContext.AddInMemoryTeam(context);
            var result = teamService.CreateTeam("test");

            //assert
            Assert.Equal(Messages.TeamAlreadyExist, result.Result.Item1);
        }

        [Fact]
        public void Delete_NotExistingInMemoryTeam_ReturnsFalse()
        {
            //arrange
            var context = InMemoryContext.GetContext();
            Mock<TeamRepository> teamRepository = new Mock<TeamRepository>(context);
            Mock<UserRepository> userRepository = new Mock<UserRepository>(context);
            var teamService = new TeamService(teamRepository.Object, userRepository.Object);

            //act
            var result = teamService.DeleteTeam(1);

            //assert
            Assert.False(result.Result);
        }

        [Fact]
        public void Delete_ExistingInMemoryTeam_ReturnsTrue()
        {
            //arrange
            var context = InMemoryContext.GetContext();
            Mock<TeamRepository> teamRepository = new Mock<TeamRepository>(context);
            Mock<UserRepository> userRepository = new Mock<UserRepository>(context);
            var teamService = new TeamService(teamRepository.Object, userRepository.Object);

            //act
            InMemoryContext.AddInMemoryTeam(context);
            var result = teamService.DeleteTeam(1);

            //assert
            Assert.True(result.Result);
        }
    }
}
