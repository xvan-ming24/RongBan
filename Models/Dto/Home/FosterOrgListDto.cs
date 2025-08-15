using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dto.Home
{
    public class FosterOrgListDto
    {
        /// <summary>
        /// 门店ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 门店名称
        /// </summary>
        public string OrgName { get; set; }

        /// <summary>
        /// 评价星级
        /// 后续添加，暂不实现
        /// </summary>
        // public decimal StarRating { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 环境图片
        /// 后续添加，暂不实现
        /// </summary>
        // public List<string> EnvironmentImages { get; set; }
    }
}
