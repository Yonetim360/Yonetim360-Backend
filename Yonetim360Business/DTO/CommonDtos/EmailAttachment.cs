using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.DTO.CommonDtos
{
    public class EmailAttachment
    {
        public string FileName { get; set; } = null!;
        public byte[] FileContent { get; set; } = null!;
        public string ContentType { get; set; } = "application/octet-stream";
    }
}
