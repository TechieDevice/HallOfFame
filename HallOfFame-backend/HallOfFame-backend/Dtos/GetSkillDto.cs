using System.Collections.Generic;

namespace HallOfFame_backend.Dtos
{
    public class GetSkillDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public byte Level { get; set; }
        public long PersonId { get; set; }
    }
}
