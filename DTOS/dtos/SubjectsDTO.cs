using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.dtos
{
    public record SubjectsDTO
    {
        public long? mainSubjectId { get; set; }
        public long? subSubjectId { get; set; }
    }
}
