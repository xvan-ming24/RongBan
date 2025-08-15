using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dto.PetStore
{
    public class ProductCategoryDto
    {

        public long Id { get; set; }

        public string CategoryName { get; set; } = null!;

        public long? ParentId { get; set; }

        public int? Sort { get; set; }
    }
}
