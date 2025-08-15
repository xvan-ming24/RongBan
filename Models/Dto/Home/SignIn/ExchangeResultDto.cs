using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dto.Home.SignIn
{
    /// <summary>
    /// 积分兑换结果数据传输对象
    /// 用于封装用户兑换积分商品后的操作结果，提供给前端展示兑换详情
    /// </summary>
    public class ExchangeResultDto
    {
        /// <summary>
        /// 兑换操作是否成功
        /// true表示兑换成功，false表示兑换失败（如积分不足、商品无库存等）
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 兑换操作提示消息
        /// 成功时返回兑换成功信息，失败时返回具体原因（如"积分不足"、"商品已下架"）
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 本次兑换消耗的积分数量
        /// 对应积分商品所需的兑换积分（points_required）
        /// </summary>
        public int PointsUsed { get; set; }

        /// <summary>
        /// 本次兑换获得的萌力值增量
        /// 根据规则：每消耗100积分增加3点萌力值（向下取整）
        /// </summary>
        public int CutenessAdded { get; set; }

        /// <summary>
        /// 兑换后剩余的积分总额
        /// 兑换前积分减去本次消耗积分后的余额
        /// </summary>
        public int RemainingPoints { get; set; }

        /// <summary>
        /// 兑换后用户的当前萌力值总额
        /// 兑换前萌力值加上本次新增萌力值后的总量（用于等级展示）
        /// </summary>
        public int CurrentCuteness { get; set; }
    }
}
