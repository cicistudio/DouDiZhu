using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CiCiStudio.CardFramework.CommonClass
{
    /// <summary>
    /// 扑克牌花色
    /// </summary>
    public enum CardType
    {
        /// <summary>
        /// 黑桃
        /// </summary>
        Spader,
        /// <summary>
        /// 红桃
        /// </summary>
        Heart,
        /// <summary>
        /// 梅花
        /// </summary>
        Club,
        /// <summary>
        /// 方片
        /// </summary>
        Diamond,
        /// <summary>
        /// 大王
        /// </summary>
        BigJoker,
        /// <summary>
        /// 小王
        /// </summary>
        SmallJoker

    }

    public enum GameStatus
    {
        Nothing,
        /// <summary>
        /// 游戏开始
        /// </summary>
        GameStart,
        /// <summary>
        /// 发牌阶段
        /// </summary>
        DealCard,
        /// <summary>
        /// 排序
        /// </summary>
        SortCard,
        /// <summary>
        /// 选择地主
        /// </summary>
        SelectHost,
        /// <summary>
        /// 左侧玩家出牌
        /// </summary>
        LeftLeadCard,
        /// <summary>
        /// 中间玩家出牌
        /// </summary>
        MiddleLeadCard,
        /// <summary>
        /// 右侧玩家出牌
        /// </summary>
        RightLeadCard,
        /// <summary>
        /// 游戏结束
        /// </summary>
        GameEnd
    }
}
