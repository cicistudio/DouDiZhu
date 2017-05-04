using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CiCiCard.ConfigClass
{
    public class ConfigInfo
    {
        /// <summary>
        /// 左侧玩家AI路径
        /// </summary>
        public string LeftAIPath { get; set; }
        /// <summary>
        /// 右侧玩家AI路径
        /// </summary>
        public string RightAIPath { get; set; }
        /// <summary>
        /// 中间玩家AI路径
        /// </summary>
        public string MiddleAIPath { get; set; }
        /// <summary>
        /// 中间玩家是否使用AI，还是使用人脑。
        /// </summary>
        public bool IsMiddleAI { get; set; }
        /// <summary>
        /// 左侧玩家AI路径
        /// </summary>
        public string LeftAIShortPath { get; set; }
        /// <summary>
        /// 右侧玩家AI路径
        /// </summary>
        public string RightAIShortPath { get; set; }
        /// <summary>
        /// 中间玩家AI路径
        /// </summary>
        public string MiddleAIShortPath { get; set; }
        /// <summary>
        /// 左侧玩家AI的NameSpace
        /// </summary>
        public string LeftAIFullName { get; set; }
        /// <summary>
        /// 中间玩家的AI的NameSpace
        /// </summary>
        public string MiddleAIFullName { get; set; }
        /// <summary>
        /// 右侧玩家的AI的NameSpace
        /// </summary>
        public string RightAIFullName { get; set; }
        /// <summary>
        /// 是否显示所有的牌
        /// </summary>
        public bool IsShowAllCard { get; set; }
    }
}
