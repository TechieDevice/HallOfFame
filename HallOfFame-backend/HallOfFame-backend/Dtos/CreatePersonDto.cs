using System.Collections.Generic;

namespace HallOfFame_backend.Dtos
{
    public class CreatePersonDto
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public List<CreateSkillDto> Skills { get; set; }
    }
}
