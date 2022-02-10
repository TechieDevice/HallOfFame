using HallOfFame_backend.Dtos;
using HallOfFame_backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HallOfFame_backend.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PersonController : Controller
    {
        private IPersonService _personService;

        public PersonController(IPersonService service)
        {
            _personService = service;
        }

        [HttpPost]
        public async Task<ActionResult> Post(CreatePersonDto userDto)
        {
            await _personService.AddPerson(userDto);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<GetPersonDto>> GetAll()
        {
            var result = await _personService.GetPersons();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetPersonDto>> Get(long id)
        {
            var result = await _personService.GetPerson(id);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(long id, EditPersonDto user)
        {
            await _personService.EditPerson(id, user);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            await _personService.DeletePerson(id);
            return Ok();
        }
    }
}
