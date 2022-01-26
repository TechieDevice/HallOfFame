using HallOfFame_backend.DataBase;
using HallOfFame_backend.Dtos;
using HallOfFame_backend.DataBase.Models;
using HallOfFame_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HallOfFame_backend.Services
{
    public class PersonService : IPersonService
    {
        private ApplicationContext _context;
        private ILoggerService _logger;

        public PersonService(ApplicationContext context, ILoggerService logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddPerson(PersonDto personDto)
        {          
            try
            {
                var person = ToModel(personDto);
                await _context.AddAsync(person);
                await _context.SaveChangesAsync();
                if (personDto.Skills != null)
                {
                    foreach (SkillDto skillDto in personDto.Skills)
                    {
                        skillDto.PersonId = person.Id;
                        await AddSkill(skillDto);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                throw e;
            }          
        }

        public async Task DeletePerson(long id)
        {
            try
            {
                var personToDelete = await _context.Persons.FirstOrDefaultAsync(u => u.Id == id);
                if (personToDelete == null)
                {
                    throw new Exception("incorrect id, no such 'person' in DB");
                }

                if (personToDelete.Skills != null)
                {
                    for(int i = 0; i < personToDelete.Skills.Count; i++)
                    {
                        await DeleteSkill(personToDelete.Skills[i].Id);
                    }
                }

                _context.Remove(personToDelete);
                await _context.SaveChangesAsync();
            }
            catch (NullReferenceException e)
            {
                _logger.Error(e.Message);
                throw e;
            }
        }

        public async Task EditPerson(long id, PersonDto personDto)
        {
            try
            {
                var existingPerson = await _context.Persons.FirstOrDefaultAsync(u => u.Id == id);
                if (existingPerson == null)
                {
                    throw new Exception("incorrect id, no such 'person' in DB");
                }

                existingPerson.DisplayName = personDto.DisplayName;
                existingPerson.Name = personDto.Name;
                _context.Update(existingPerson);
                await _context.SaveChangesAsync();

                var dbSkillList = await GetSkills(id);
            
                foreach (SkillDto skill in personDto.Skills)
                {
                    skill.PersonId = id;
                    if (dbSkillList.Exists(s => s.Id == skill.Id))
                    {
                        await EditSkill(skill);
                    }
                    else
                    {
                        await AddSkill(skill);
                    }
                }

                foreach (SkillDto skill in dbSkillList)
                {
                    if (!personDto.Skills.Exists(s => s.Id == skill.Id))
                    {
                        await DeleteSkill(skill.Id.Value);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                throw e;
            }
        }

        public async Task<List<PersonDto>> GetPersons()
        {
            try
            {
                var result = await _context.Persons
                .AsNoTracking()
                .Select(p => ToDto(p))
                .ToListAsync();

                for (int i = 0; i < result.Count; i++)
                {
                    result[i].Skills = await GetSkills(result[i].Id.Value);
                }

                return result;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                throw e;
            }
        }

        public async Task<PersonDto> GetPerson(long id)
        {
            try
            {
                var person = await _context.Persons.FirstOrDefaultAsync(p => p.Id == id);

                if (person == null)
                {
                    throw new Exception("incorrect id, no such 'person' in DB");
                }

                var personDto = ToDto(person);
                personDto.Skills = await GetSkills(id);

                return personDto;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                throw e;
            }
        }

        private async Task AddSkill(SkillDto skillDto)
        {
            var skill = ToModel(skillDto);
            await _context.AddAsync(skill);
            await _context.SaveChangesAsync();
        }

        private async Task DeleteSkill(long id)
        {
            var skillToDelete = await _context.Skills.FirstOrDefaultAsync(s => s.Id == id);
            if (skillToDelete == null)
            {
                throw new Exception("incorrect id, no such 'skill' in DB");
            }

            _context.Remove(skillToDelete);
            await _context.SaveChangesAsync();
        }

        private async Task EditSkill(SkillDto skillDto)
        {
            Skill existingSkill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == skillDto.Id.Value);
            if (existingSkill == null)
            {
                throw new Exception("incorrect id, no such 'skill' in DB");
            }

            existingSkill.Level = skillDto.Level;
            existingSkill.Name = skillDto.Name;
            _context.Update(existingSkill);
            await _context.SaveChangesAsync();
        }

        private async Task<List<SkillDto>> GetSkills(long id)
        {
            var result = await _context.Skills
                .AsNoTracking()
                .Where(s => s.PersonId == id)
                .Select(s => ToDto(s))
                .ToListAsync();

            return result;
        }

        private Person ToModel(PersonDto personDto)
        {
            return new Person
            {
                DisplayName = personDto.DisplayName,
                Name = personDto.Name
            };
        }

        private static PersonDto ToDto(Person person)
        {
            return new PersonDto
            {
                Id = person.Id,
                DisplayName = person.DisplayName,
                Name = person.Name,
            };
        }

        private Skill ToModel(SkillDto skillDto)
        {
            return new Skill
            {
                Level = skillDto.Level,
                Name = skillDto.Name,
                PersonId = skillDto.PersonId.Value
            };
        }

        private static SkillDto ToDto(Skill skill)
        {
            return new SkillDto
            {
                Id = skill.Id,
                Level = skill.Level,
                Name = skill.Name,
                PersonId = skill.PersonId
            };
        }
    }
}
