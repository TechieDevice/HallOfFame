﻿using System.Collections.Generic;

namespace HallOfFame_backend.DataBase.Models
{
    public class Skill
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public byte Level { get; set; }
        public long PersonId { get; set; }

        public virtual Person Person { get; set; }
    }
}
