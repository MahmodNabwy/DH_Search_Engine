using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS.dtos
{
    public class PublicationDetailDataGetterDTO
    {
        public long Id { get; set; }
        public string PublicationCover { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime PublishDate { get; set; }
        public string PdfUrl { get; set; }
        public string ExcelUrl { get; set; }
        public int? Month { get; set; }
        public int? Quarter { get; set; }
        public int Year { get; set; }
        public List<InfographicDataGetterDTO> Infographics { get; set; }
        public StudyPeriodDataGetterDTO StudyPeriod { get; set; }
    }
}
