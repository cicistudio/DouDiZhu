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
namespace AIFrameWork
{
    public enum CardPlayerType
    {
        NoPlayer,
        LeftPlayer,
        MiddlePlayer,
        RightPlayer
    }

    public enum ScoreType
    {
        Pass,
        One,
        Two,
        Three,
        NoChoose
    }

    public interface IBase
    {
        /// <summary>
        /// 获取玩家姓名，会在桌面上显示出来
        /// </summary>
        /// <returns></returns>
        string GetPlayerName();
        /// <summary>
        /// 显示About对换框
        /// </summary>
        void About();
        /// <summary>
        /// 传入每回合的牌集合
        /// </summary>
        /// <param name="cardCollection">牌集合 A是14 2 是15，小王是16，大王是17</param>
        void SetCardCollection(int[] cardArray);
        /// <summary>
        /// 最后三张牌会亮一下，如果自己是地主，那么需要增加到自己的牌Collection中。
        /// </summary>
        /// <param name="cardCollection"></param>
        void SetHostLastThreeCard(int [] cardArray);
        /// <summary>
        /// 设置谁是地主
        /// </summary>
        /// <param name="playType">哪个是地主</param>
        void SetHost(CardPlayerType hostPlay);
        /// <summary>
        /// 设置位置
        /// </summary>
        /// <param name="playType">左侧，中间，还是右侧</param>
        void SetPlayerLocation(CardPlayerType playType);
        /// <summary>
        /// 获得该插件选择的分数，如果弃权选择，那么分数为-1
        /// </summary>
        /// <param name="markCollection">前面别的电脑选择的分数，可以根据其数量来判断自己是第几个选择的</param>
        /// <returns>返回插件选择的分数</returns>
        ScoreType GetChooseHostMark(List<int> markArray);
        /// <summary>
        /// 系统新建一个游戏时会调用此方法。可用于初始化数据。
        /// </summary>
        void NewCycle();

        /// <summary>
        /// 每个玩家的出牌阶段会调用此方法，如果该玩家没有出牌，也会调用该方法，但是cardOutPutArray为null
        /// </summary>
        /// <param name="player">是哪个玩家出的牌</param>
        /// <param name="cardOutPutCollection">出牌的集合</param>
        /// <param name="cycleIndex">是在第几回合出的牌，每回合从地主开始</param>
        void SetOutPutCard(CardPlayerType player,int [] cardOutPutArray,int cycleIndex);

        /// <summary>
        /// 当到了自己的出牌阶段，系统调用此方法，来获取AI出的是什么牌，如果返回为null那么说明，此次AI不出牌
        /// </summary>
        /// <param name="isCycleFirst">是否是该回合第一个出牌的人</param>
        int [] GetOutPutCard(bool isCycleFirst);
    }
}
