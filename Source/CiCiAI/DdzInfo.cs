using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*************************************************************
 * Copyright by cicistudio 2000-2017
 * http://chengchen.cnblogs.com
 * https://github.com/cicistudio/
 * The Current code cannot be used in the Commercial software.
 * Mail to me if you have any queries. cicistudio@qq.com
 ************************************************************/
namespace CiCiAI
{
    public class DdzInfo
    {

        /// <summary>
        /// 地主是左侧，右侧还是中间
        /// </summary>
        public static CommClass.PlayerSide HostSide { get; set; }
        /// <summary>
        /// 自己的位置
        /// </summary>
        public static CommClass.PlayerSide PlayerSide { get; set; }
        /// <summary>
        /// 自己的手牌
        /// </summary>
        public static List<CommClass.Poker> MyPokers { get; set; }
        /// <summary>
        /// 我打出去的牌
        /// </summary>
        public static List<CommClass.Poker> MyOutPokers { get; set; }
        /// <summary>
        /// 上家打出去的牌
        /// </summary>
        public static List<CommClass.Poker> UpOutPokers { get; set; }
        /// <summary>
        /// 下家打出去的牌
        /// </summary>
        public static List<CommClass.Poker> DownOutPokers { get; set; }
        /// <summary>
        /// 估算上家的牌, 后面int是1-100的可能性，如果是100则意味着肯定有这个牌
        /// </summary>
        public static Dictionary<CommClass.Poker, int> UpEvaluatePokers { get; set; }
        /// <summary>
        /// 估算下家的牌, 后面int是1-100的可能性，如果是100则意味着肯定有这个牌
        /// </summary>
        public static Dictionary<CommClass.Poker, int> DownEvaluatePokers { get; set; }
    }
}
