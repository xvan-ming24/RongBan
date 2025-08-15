using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dto.Home.SignIn
{
    /// <summary>
    /// 任务完成结果数据传输对象
    /// 用于封装用户完成任务后的操作结果，提供给前端展示任务完成详情及奖励信息
    /// </summary>
    public class TaskCompleteResultDto
    {
        /// <summary>
        /// 任务完成操作是否成功
        /// true表示任务完成并获得奖励，false表示任务完成失败（如已完成当日任务、任务失效等）
        /// </summary>
        public bool Success { get; set; }



        /// <summary>
        /// 本次完成任务获得的萌力值奖励
        /// 不同任务奖励不同（如发布动态+20、分享应用+50等）
        /// </summary>
        public int Reward { get; set; }

        /// <summary>
        /// 完成任务后用户的当前萌力值总额
        /// 任务完成前萌力值加上本次奖励萌力值后的总量（用于等级展示）
        /// </summary>
        public int TotalCuteness { get; set; }
    }

}
