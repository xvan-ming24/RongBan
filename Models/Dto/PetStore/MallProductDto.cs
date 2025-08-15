using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dto.PetStore
{
    public class MallProductDto
    {
        public long Id { get; set; }

        public string ProductName { get; set; } = null!;

        public long CategoryId { get; set; }

        public string? ApplicablePetType { get; set; }

        public decimal Price { get; set; }

        public decimal? OriginalPrice { get; set; }

        public int? Stock { get; set; }

        public int? SalesVolume { get; set; }

        public string? Specification { get; set; }

        public string? Description { get; set; }

        public string? CoverUrl { get; set; }

        public byte? Status { get; set; }
    }
}
