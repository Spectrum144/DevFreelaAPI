using Bogus;
using DevFreela.Application.Commands.DeleteProject;
using DevFreela.Application.Commands.InsertProject;
using DevFreela.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevFreela.UnitTests.Fakes {
    public class FakeDataHelper {

        private static readonly Faker _faker = new();

        private static Project CreateFakeProjectV1() {
            // Colocando dados fakes para instanciar um projeto
            // 
            return new Project(
                // Title
                _faker.Commerce.ProductName(),
                // Description
                _faker.Lorem.Sentence(),
                // IdClient
                _faker.Random.Int(1,100),
                // IdFreelancer
                _faker.Random.Int(1,100),
                // Price
                _faker.Random.Decimal(1000,10000)
                );
        }

        private static readonly Faker<Project> _projectFaker = new Faker<Project>()
            .CustomInstantiator(f => new Project(
                // Title
                f.Commerce.ProductName(),
                // Description
                f.Lorem.Sentence(),
                // IdClient
                f.Random.Int(1, 100),
                // IdFreelancer
                f.Random.Int(1, 100),
                // Price
                f.Random.Decimal(1000, 10000)
                ));

        private static readonly Faker<InsertProjectCommand> _insertProjectCommandFaker = new Faker<InsertProjectCommand>()
            .RuleFor(c => c.Title, f => f.Commerce.ProductName())
            .RuleFor(c => c.Description, f => f.Lorem.Sentence())
            .RuleFor(c => c.IdFreelancer, f => f.Random.Int(1, 100))
            .RuleFor(c => c.IdClient, f => f.Random.Int(1, 100))
            .RuleFor(c => c.TotalCost, f => f.Random.Decimal(1000, 10000));

        public static Project CreateFakeProject() => _projectFaker.Generate();

        public static List<Project> CreateFakeProjectList() => _projectFaker.Generate(5);
    
        public static InsertProjectCommand CreateFakerInsertProjectCommand()
            => _insertProjectCommandFaker.Generate();

        public static DeleteProjectCommand CreateFakerDeleteProjectCommand(int id)
            => new(id);

    }
}
