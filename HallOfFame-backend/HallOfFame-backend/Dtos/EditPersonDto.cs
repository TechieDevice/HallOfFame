using System.Collections.Generic;

namespace HallOfFame_backend.Dtos
{
    public class EditPersonDto
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public List<EditSkillDto> Skills { get; set; }
    }
}
