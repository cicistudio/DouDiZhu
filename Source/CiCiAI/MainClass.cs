using AIFrameWork;
using CiCiAI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
/*************************************************************
 * Copyright by cicistudio 2000-2017
 * http://chengchen.cnblogs.com
 * https://github.com/cicistudio/
 * The Current code cannot be used in the Commercial software.
 * Mail to me if you have any queries. cicistudio@qq.com
 ************************************************************/
namespace CiCiAI
{
    /// <summary>
    /// 设计的通用接口，很多参数是用字符串而没有采用C#的编程标准。
    /// 这样设计的好处是便于VC++ 等其他编程语言编写AI插件。
    /// 斗地主不用考虑花色情况。
    /// </summary>
    public class MainClass: IBase
    {       
        #region 初始化各个变量的值
        private static void InitSystem()
        {
            DdzInfo.HostLocation = CommClass.PlayerLocation.None;
            DdzInfo.HostSide = CommClass.PlayerSide.None;
            if (DdzInfo.UpEvaluatePokers == null)
            {
                DdzInfo.UpEvaluatePokers = new Dictionary<CommClass.Poker, int>();
            }
            if (DdzInfo.UpOutPokers == null)
            {
                DdzInfo.UpOutPokers = new List<CommClass.Poker>();
            }
            if (DdzInfo.MyOutPokers == null)
            {
                DdzInfo.MyOutPokers = new List<CommClass.Poker>();
            }
            if (DdzInfo.MyPokers == null)
            {
                DdzInfo.MyPokers = new List<CommClass.Poker>();
            }
            if (DdzInfo.DownEvaluatePokers == null)
            {
                DdzInfo.DownEvaluatePokers = new Dictionary<CommClass.Poker, int>();
            }
            if (DdzInfo.DownOutPokers == null)
            {
                DdzInfo.DownOutPokers = new List<CommClass.Poker>();
            }
            DdzInfo.UpEvaluatePokers.Clear();
            DdzInfo.UpOutPokers.Clear();
            DdzInfo.MyOutPokers.Clear();
            DdzInfo.MyPokers.Clear();
            DdzInfo.DownEvaluatePokers.Clear();
            DdzInfo.DownOutPokers.Clear();
        }
        #endregion

        /// <summary>
        /// 传入每回合的牌集合
        /// </summary>
        /// <param name="cardArray">A是14 2 是15，小王是16，大王是17</param>
        public void SetCardCollection(int[] cardArray)
        {
            //疑问如果自己是地主，那么最后三张牌会不会再下一回合发回来？
            string pokers = CommClass.GetPokerStringFromIntArray(cardArray);
            DdzInfo.MyPokers = CommClass.PockerStringToList(pokers);
        }

        /// <summary>
        /// 最后三张牌会亮一下，如果自己是地主，那么需要增加到自己的牌Collection中。
        /// </summary>
        /// <param name="cardArray"></param>
        public void SetHostLastThreeCard(int[] cardArray)
        {
            string pokers = CommClass.GetPokerStringFromIntArray(cardArray);
            List<CommClass.Poker> threePokers = CommClass.PockerStringToList(pokers);
            foreach (CommClass.Poker p in threePokers)
            {
                DdzInfo.MyPokers.Add(p);
            }
        }

        /// <summary>
        /// 设置谁是地主
        /// </summary>
        /// <param name="playType">哪个是地主</param>
        public void SetHost(CardPlayerType hostPlay)
        {
            if(hostPlay == CardPlayerType.LeftPlayer)
            {
                DdzInfo.HostSide = CommClass.PlayerSide.Left;
            }
            else if(hostPlay == CardPlayerType.RightPlayer)
            {
                DdzInfo.HostSide = CommClass.PlayerSide.Right;
            }
            else if(hostPlay == CardPlayerType.MiddlePlayer)
            {
                DdzInfo.HostSide = CommClass.PlayerSide.Middle;
            }


        }

        /// <summary>
        /// 设置位置
        /// </summary>
        /// <param name="playType">左侧，中间，还是右侧</param>
        public void SetPlayerLocation(CardPlayerType playType)
        {

        }

        /// <summary>
        /// 获得该插件选择的分数，如果弃权选择，那么分数为-1
        /// </summary>
        /// <param name="markCollection">前面别的电脑选择的分数，可以根据其数量来判断自己是第几个选择的</param>
        /// <returns>返回插件选择的分数</returns>
        public ScoreType GetChooseHostMark(List<int> markArray)
        {
            int max = 0;
            if(markArray.Count >0)
            {
                max = markArray.Max();
            }
            EvaluationInfo eval = DeepThinking.GetEvaluateSocre(DdzInfo.MyPokers);//获取插件评估分数
            if(eval.Scores<55)
            {
                return ScoreType.Pass;
            }
            else if(eval.Scores>=55 && eval.Scores <65)
            {
                if(max >=1 )//如果被上家叫1分的牌了。
                {
                    return ScoreType.Pass;
                }
                else
                {
                    return ScoreType.One;
                }            
            }
            else if (eval.Scores >= 65 && eval.Scores < 75)
            {
                if(max>=2)
                {
                    return ScoreType.Pass;
                }
                else
                {
                    return ScoreType.Two;
                }
               
            }
            else
            {
                if (max >= 3)
                {
                    return ScoreType.Pass;
                }
                else
                {
                    return ScoreType.Three;
                }
            }
        }

        /// <summary>
        /// 系统新建一个游戏时会调用此方法。可用于初始化数据。
        /// </summary>
        public void NewCycle()
        {
            InitSystem();
        }

        /// <summary>
        /// 每个玩家的出牌阶段会调用此方法，如果该玩家没有出牌，也会调用该方法，但是cardOutPutArray为null
        /// </summary>
        /// <param name="player">是哪个玩家出的牌</param>
        /// <param name="cardOutPutCollection">出牌的集合</param>
        /// <param name="cycleIndex">是在第几回合出的牌，每回合从地主开始</param>
        public void SetOutPutCard(CardPlayerType player, int[] cardOutPutArray, int cycleIndex)
        {
            //此方法可用于用于保存其他玩家出的牌，AI会决定是否跟牌。
            //此方法也适用于高级分析，推测对方的剩余的牌是什么。
        }

        /// <summary>
        /// 当到了自己的出牌阶段，系统调用此方法，来获取AI出的是什么牌，如果返回为null那么说明，此次AI不出牌
        /// </summary>
        /// <param name="isCycleFirst">是否是该回合第一个出牌的人</param>
        public int[] GetOutPutCard(bool isCycleFirst)
        {
            if(isCycleFirst)//如果是自己出牌那么将根据最优化的拆牌方法进行出牌。
            {

            }
            
            return null;
        }

        public void About()
        {
            MessageBox.Show("This Plug-in is developed by CiCi Stuido @2017, this version will be much more smart than AIDEMO plug-in");
        }
        public string GetPlayerName()
        {
            return "CiCi";
        }

        public EvaluationInfo TestEvaluationSocre(string pokers)
        {
            //string pokers = CommClass.GetPokerStringFromIntArray(cardArray);
            DdzInfo.MyPokers = CommClass.PockerStringToList(pokers);
            //Test
            return DeepThinking.GetEvaluateSocre(DdzInfo.MyPokers);
        }
    }
}
