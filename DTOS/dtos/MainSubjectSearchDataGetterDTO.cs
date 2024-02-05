using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.dtos
{
    public record MainSubjectSearchDataGetterDTO
    {
        public long subSubjectId { get; init; }
        public string Title { get; init; }
        public long? MainSubjectId { get; init; }
        public List<SubjectTranslationDataGetterDTO> SubjectTranslations { get; set; }
    }
}
