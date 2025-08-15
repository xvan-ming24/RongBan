using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dto.Home.SignIn
{
    // 签到结果DTO
    /// <summary>
    /// 签到操作的结果数据传输对象
    /// 用于封装用户签到后的各项结果信息，供前端展示
    /// </summary>
    public class CheckinResultDto
    {
        /// <summary>
        /// 签到操作是否成功
        /// true表示签到成功，false表示签到失败（如已签到）
        /// </summary>
        public bool Success { get; set; }


        /// <summary>
        /// 本次签到获得的积分奖励
        /// 基础为10点，连续5天额外+100，连续7天额外+200
        /// </summary>
        public int PointsRewarded { get; set; }

        /// <summary>
        /// 本次签到获得的萌力值奖励
        /// 基础为10点，连续5天额外+100，连续7天额外+200
        /// </summary>
        public int CutenessRewarded { get; set; }

        /// <summary>
        /// 当前连续签到天数
        /// 为null时表示签到失败，不为null时表示当前连续签到的天数
        /// </summary>
        public int? ContinuousDays { get; set; }

        /// <summary>
        /// 签到后用户的当前积分总额
        /// 为null时表示签到失败，不为null时表示最新的积分余额
        /// </summary>
        public int? CurrentPoints { get; set; }

        /// <summary>
        /// 签到后用户的当前萌力值总额
        /// 为null时表示签到失败，不为null时表示最新的萌力值总量（影响等级判定）
        /// </summary>
        public int? CurrentCuteness { get; set; }
    }
}

