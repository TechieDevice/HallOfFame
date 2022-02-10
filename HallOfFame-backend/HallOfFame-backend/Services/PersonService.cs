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

        public async Task AddPerson(CreatePersonDto personDto)
        {          
            try
            {
                var person = ToModel(personDto);
                await _context.AddAsync(person);
                await _context.SaveChangesAsync();
                if (personDto.Skills != null)
                {
                    foreach (var skillDto in personDto.Skills)
                    {
                        await AddSkill(skillDto, person.Id);
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

                //if (personToDelete.Skills != null)
                //{
                //    for(var i = 0; i < personToDelete.Skills.Count; i++)
                //    {
                //        await DeleteSkill(personToDelete.Skills[i].Id);
                //    }
                //}

                _context.Remove(personToDelete);
                await _context.SaveChangesAsync();
            }
            catch (NullReferenceException e)
            {
                _logger.Error(e.Message);
                throw e;
            }
        }

        public async Task EditPerson(long personId, EditPersonDto personDto)
        {
            try
            {
                var existingPerson = await _context.Persons.FirstOrDefaultAsync(u => u.Id == personId);
                if (existingPerson == null)
                {
                    throw new Exception("incorrect id, no such 'person' in DB");
                }

                existingPerson.DisplayName = personDto.DisplayName;
                existingPerson.Name = personDto.Name;
                _context.Update(existingPerson);
                await _context.SaveChangesAsync();

                var dbSkillList = await GetSkills(personId);
            
                foreach (var skill in personDto.Skills)
                {
                    if (dbSkillList.Exists(s => s.Id == skill.Id))
                    {
                        await EditSkill(skill);
                    }
                    else
                    {
                        await AddSkill(ToCreateDto(skill), personId);
                    }
                }

                foreach (var skill in dbSkillList)
                {
                    if (!personDto.Skills.Exists(s => s.Id == skill.Id))
                    {
                        await DeleteSkill(skill.Id);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                throw e;
            }
        }

        public async Task<List<GetPersonDto>> GetPersons()
        {
            try
            {
                var result = await _context.Persons
                .AsNoTracking()
                .Select(p => ToDto(p))
                .ToListAsync();

                for (var i = 0; i < result.Count; i++)
                {
                    result[i].Skills = await GetSkills(result[i].Id);
                }

                return result;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                throw e;
            }
        }

        public async Task<GetPersonDto> GetPerson(long id)
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

        private async Task AddSkill(CreateSkillDto skillDto, long personId)
        {
            var skill = ToModel(skillDto, personId);
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

        private async Task EditSkill(EditSkillDto skillDto)
        {
            var existingSkill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == skillDto.Id);
            if (existingSkill == null)
            {
                throw new Exception("incorrect id, no such 'skill' in DB");
            }

            existingSkill.Level = skillDto.Level;
            existingSkill.Name = skillDto.Name;
            _context.Update(existingSkill);
            await _context.SaveChangesAsync();
        }

        private async Task<List<GetSkillDto>> GetSkills(long id)
        {
            var result = await _context.Skills
                .AsNoTracking()
                .Where(s => s.PersonId == id)
                .Select(s => ToDto(s))
                .ToListAsync();

            return result;
        }

        private Person ToModel(CreatePersonDto personDto)
        {
            return new Person
            {
                DisplayName = personDto.DisplayName,
                Name = personDto.Name
            };
        }

        private static GetPersonDto ToDto(Person person)
        {
            return new GetPersonDto
            {
                Id = person.Id,
                DisplayName = person.DisplayName,
                Name = person.Name,
            };
        }

        private Skill ToModel(CreateSkillDto skillDto, long personId)
        {
            return new Skill
            {
                Level = skillDto.Level,
                Name = skillDto.Name,
                PersonId = personId
            };
        }

        private static GetSkillDto ToDto(Skill skill)
        {
            return new GetSkillDto
            {
                Id = skill.Id,
                Level = skill.Level,
                Name = skill.Name,
                PersonId = skill.PersonId
            };
        }

        private static CreateSkillDto ToCreateDto(EditSkillDto skillDto)
        {
            return new CreateSkillDto
            {
                Level = skillDto.Level,
                Name = skillDto.Name
            };
        }
    }
}
