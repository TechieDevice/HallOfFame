using HallOfFame_backend.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HallOfFame_backend.Services.Interfaces
{
    public interface IPersonService
    {
        public Task AddPerson(PersonDto userDto);

        public Task DeletePerson(long id);

        public Task EditPerson(long id, PersonDto userDto);

        public Task<List<PersonDto>> GetPersons();

        public Task<PersonDto> GetPerson(long id);
    }
}
