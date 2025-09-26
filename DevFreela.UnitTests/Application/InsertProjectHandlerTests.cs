using DevFreela.Application.Commands.InsertProject;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using FluentAssertions;
using Moq;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevFreela.UnitTests.Application {
    public class InsertProjectHandlerTests {
        [Fact]
        public async Task InsertDataAreOk_Insert_Success_NSubstitute() {
            // Arrange
            const int id = 1;
            var repository = Substitute.For<IProjectRepository>();
            repository.Add(Arg.Any<Project>()).Returns(Task.FromResult(id));

            var command = new InsertProjectCommand {
                Title = "Project A",
                Description = "Descrição do Projeto",
                TotalCost = 20000,
                IdClient = 1,
                IdFreelancer = 2
            };

            var handler = new InsertProjectHandler(repository);

            // Act
            var result = await handler.Handle(command, new CancellationToken());

            // Assert
            Assert.True(result.IsSuccess);

            // Assert usando FluentAssertions
            result.IsSuccess.Should().BeTrue();

            Assert.Equal(id, result.Data);

            //
            result.Data.Should().Be(id);

            await repository.Received(id).Add(Arg.Any<Project>());
        }

        [Fact]
        public async Task InsertDataAreOk_Insert_Success_Moq() {
            // Arrange
            const int id = 1;

            //var mock = new Mock<IProjectRepository>();
            //mock.Setup(r => r.Add(It.IsAny<Project>())).ReturnsAsync(id);

            var repository = Mock
                .Of<IProjectRepository>(r => r.Add(It.IsAny<Project>()) == Task.FromResult(id));

            var command = new InsertProjectCommand {
                Title = "Project A",
                Description = "Descrição do Projeto",
                TotalCost = 20000,
                IdClient = 1,
                IdFreelancer = 2
            };

            var handler = new InsertProjectHandler(repository);

            // Act
            var result = await handler.Handle(command, new CancellationToken());

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(id, result.Data);
            //

            //mock.Verify(m => m.Add(It.IsAny<Project>()), Times.Once);
            Mock.Get(repository).Verify(m => m.Add(It.IsAny<Project>()), Times.Once);
        }
    }
}
