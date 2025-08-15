using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dto.Home.SignIn
{
    /// <summary>
    /// 用户任务信息数据传输对象
    /// 用于封装用户可执行的任务详情及完成状态，供前端展示任务列表
    /// </summary>
    public class UserTaskDto
    {
        /// <summary>
        /// 任务唯一标识ID
        /// 用于前端发起任务完成请求时指定具体任务
        /// </summary>
        public long TaskId { get; set; }

        /// <summary>
        /// 任务名称
        /// 描述任务内容（如"发布动态"、"分享应用"等）
        /// </summary>
        public string TaskName { get; set; }

        /// <summary>
        /// 完成任务可获得的萌力值奖励
        /// 不同任务奖励不同（如发布动态+20、浏览商品5秒+80等）
        /// </summary>
        public int Reward { get; set; } // 萌力值奖励

        /// <summary>
        /// 任务是否已完成
        /// true表示今日已完成该任务（不可重复领取奖励），false表示可完成
        /// </summary>
        public bool IsCompleted { get; set; }
    }
}
