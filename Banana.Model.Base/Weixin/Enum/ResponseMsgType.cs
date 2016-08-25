using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banana.Model.Base.Weixin.Enum
{
    public enum ResponseMsgType
    {
        /// <summary>
        /// 回复文本消息
        /// </summary>
        Text,

        /// <summary>
        /// 回复图片消息
        /// </summary>
        Image,

        /// <summary>
        /// 回复语音消息
        /// </summary>
        Voice,

        /// <summary>
        /// 回复视频消息
        /// </summary>
        Video,

        /// <summary>
        /// 回复音乐消息
        /// </summary>
        Music,

        /// <summary>
        /// 回复图文消息
        /// </summary>
        News
    }
}
