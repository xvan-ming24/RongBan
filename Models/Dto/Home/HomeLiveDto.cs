using Rongban.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dto.Home
{
    public class HomeLiveDto
    {
        public long Id { get; set; }

        public long? HostId { get; set; }

        public string HostNickName { get; set; } = null!;

        public string LiveTitle { get; set; } = null!;

        public string LiveUrl { get; set; } = null!;

        public int? OnlineNum { get; set; }

        public int? Sort { get; set; }

        public byte? LiveStatus { get; set; }

        public DateTime? CreateTime { get; set; }

    }
}
