using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dto.PetCircle
{
    public class PublishUpdatesDto
    {
        /// <summary>
        /// 宠友圈动态数据传输对象，用于API返回动态信息
        /// </summary>
        public class PetMomentDto
        {
            /// <summary>
            /// 动态ID
            /// </summary>
            public long Id { get; set; }

            /// <summary>
            /// 发布用户ID
            /// </summary>
            public long UserId { get; set; }

            /// <summary>
            /// 用户名（用于展示，不存储在动态表中）
            /// </summary>
            public string Nickname { get; set; }

            /// <summary>
            /// 用户头像URL（用于展示，不存储在动态表中）
            /// </summary>
            public string UserAvatar { get; set; }

            /// <summary>
            /// 动态内容
            /// </summary>
            public string Content { get; set; }

            /// <summary>
            /// 图片URLs，多个用逗号分隔
            /// </summary>
            public string ImageUrls { get; set; }

            /// <summary>
            /// 点赞数量
            /// </summary>
            public long? LikeCount { get; set; }

            /// <summary>
            /// 评论数量
            /// </summary>
            public long? CommentCount { get; set; }

            /// <summary>
            /// 分享数量
            /// </summary>
            public long? ShareCount { get; set; }

            /// <summary>
            /// 发布时间（原始时间）
            /// </summary>
            public DateTime? CreateTime { get; set; }

            /// <summary>
            /// 发布时间的格式化显示（yyyy-MM-dd HH:mm:ss）
            /// </summary>
            public string CreateTimeDisplay { get; set; }
        }

        /// <summary>
        /// 创建动态的数据传输对象，用于接收API请求参数
        /// </summary>
        public class CreatePetMomentDto
        {
            /// <summary>
            /// 动态内容（必填）
            /// </summary>
            public string Content { get; set; }

            /// <summary>
            /// 图片URLs，多个用逗号分隔（可选）
            /// </summary>
            public string ImageUrls { get; set; }
        }
    }
}
