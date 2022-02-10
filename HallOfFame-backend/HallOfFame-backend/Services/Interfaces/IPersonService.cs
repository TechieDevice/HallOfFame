using HallOfFame_backend.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HallOfFame_backend.Services.Interfaces
{
    public interface IPersonService
    {
        public Task AddPerson(CreatePersonDto userDto);

        public Task DeletePerson(long id);

        public Task EditPerson(long id, EditPersonDto userDto);

        public Task<List<GetPersonDto>> GetPersons();

        public Task<GetPersonDto> GetPerson(long id);
    }
}
