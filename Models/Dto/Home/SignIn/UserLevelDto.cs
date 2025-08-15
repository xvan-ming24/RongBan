using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dto.Home.SignIn
{
    /// <summary>
    /// 用户等级信息数据传输对象
    /// 用于封装用户当前的萌力值、等级及升级进度，供前端展示等级相关信息
    /// </summary>
    public class UserLevelDto
    {
        /// <summary>
        /// 用户当前的萌力值总量
        /// 由签到、任务完成、积分兑换等行为累积获得
        /// </summary>
        public int CutenessValue { get; set; }

        /// <summary>
        /// 当前等级名称
        /// 对应v1-v5等级体系（如"v1"、"v3"等）
        /// </summary>
        public string LevelName { get; set; } // v1-v5

        /// <summary>
        /// 升级到下一等级所需的萌力值
        /// 例如：v1升级到v2需要达到1001萌力值
        /// </summary>
        public int NextLevelRequired { get; set; }

        /// <summary>
        /// 当前等级的升级进度百分比
        /// 计算方式：(当前萌力值 - 当前等级最小值) / (下一等级所需值 - 当前等级最小值) * 100
        /// 用于前端展示进度条
        /// </summary>
        public int ProgressPercent { get; set; }
    }
}
