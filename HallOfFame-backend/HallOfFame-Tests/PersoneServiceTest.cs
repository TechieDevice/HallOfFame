using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using HallOfFame_backend.Dtos;
using HallOfFame_backend.DataBase.Models;
using HallOfFame_backend.Services;
using System.Threading.Tasks;

namespace HallOfFame_Tests
{
    public class PersoneServiceTest : ServiceTestBase
    {
        private PersonService _personService;
        private const long defaultId1 = 1000;
        private const long defaultId2 = 1001;
        private const long defaultId3 = 1002;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _personService = new PersonService(_applicationContext, new EmptyLoggingService());
            PutTestData();
        }

        [Test]
        public async Task PersonService_Add_ShouldAdd()
        {
            var count = await _applicationContext.Persons.CountAsync();

            CreatePersonDto person = new CreatePersonDto
            {
                Name = "Jhon",
                DisplayName = "TestUser3",
                Skills = new System.Collections.Generic.List<CreateSkillDto>()
            };
            person.Skills.Add(new CreateSkillDto
            {          
                Name = "Testing",
                Level = 3
            });
            await _personService.AddPerson(person);

            var countAfterAdding = await _applicationContext.Persons.CountAsync();
            Assert.AreEqual(count, countAfterAdding - 1);
        }

        [Test]
        public async Task PersonService_Delete_ShouldDelete()
        {
            var count = await _applicationContext.Persons.CountAsync();

            await _personService.DeletePerson(defaultId2);

            var countAfterDeleting = await _applicationContext.Persons.CountAsync();
            var deletedPerson = await _applicationContext.Persons.FirstOrDefaultAsync(p => p.Id == defaultId2);

            Assert.AreEqual(count, countAfterDeleting+1);
            Assert.IsNull(deletedPerson, "Person is not null");
        }

        [Test]
        public async Task PersonService_Edit_ShouldEdit()
        {
            var person = new EditPersonDto
            {
                Name = "Bob",
                DisplayName = "TestUser1",
                Skills = new System.Collections.Generic.List<EditSkillDto>()
            };
            person.Skills.Add(new EditSkillDto
            {
                Name = "Testing",
                Level = 3
            });
            await _personService.EditPerson(defaultId1, person);
            var editedPerson = await _applicationContext.Persons.FirstOrDefaultAsync(p => p.Id == defaultId1);

            Assert.IsNotNull(editedPerson, "Person is null");
            Assert.AreEqual(person.Skills[0].Level, editedPerson.Skills[0].Level);
        }

        [Test]
        public async Task PersonService_Get_ShouldReturnPoint()
        {
            var getId = await _personService.GetPerson(defaultId1);
            var getList = await _personService.GetPersons();

            Assert.IsNotNull(getId, "Person is null");
            Assert.IsNotNull(getList, "List is null");
            Assert.AreNotEqual(getList.Count, 0);
        }

        private void PutTestData()
        {
            _applicationContext.Persons.Add(new Person
            {
                Id = defaultId1,
                Name = "Bob",
                DisplayName = "TestUser1"
            });
            _applicationContext.SaveChanges();

            _applicationContext.Skills.Add(new Skill
            {
                Id = defaultId1,
                Name = "Testing",
                Level = 1,
                PersonId = defaultId1
            });
            _applicationContext.SaveChanges();

            _applicationContext.Persons.Add(new Person
            {
                Id = defaultId2,
                Name = "Tom",
                DisplayName = "TestUser2"
            });
            _applicationContext.SaveChanges();

            _applicationContext.Skills.Add(new Skill
            {
                Id = defaultId2,
                Name = "Testing",
                Level = 2,
                PersonId = defaultId2
            });
            _applicationContext.SaveChanges();
        }
    }
}
