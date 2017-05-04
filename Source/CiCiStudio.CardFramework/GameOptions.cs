using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using AIFrameWork;
using CiCiStudio.CardFramework.CommonClass;

namespace CiCiStudio.CardFramework
{
    public class GameOptions
    {
        private static double m_DealSpeed = 0.1;

        /// <summary>
        /// 发牌速度
        /// </summary>
        public static double DealSpeed
        {
            get { return GameOptions.m_DealSpeed; }
            set { GameOptions.m_DealSpeed = value; }
        }

        /// <summary>
        /// 牌之间的间隔
        /// </summary>
        public static int CardSpace
        {
            get { return 25; }
        }

        public static Point LeftPlayerFirstLocation
        {
            get { return new Point(-370, -150); }
        }

        public static Point RightPlayerFirstLocation
        {
            get { return new Point(390, -150); }
        }

        public static Point MiddlePlayerFirstLocation
        {
            get { return new Point(-200, 330); }
        }

        /// <summary>
        /// 出牌速度
        /// </summary>
        public static double PopCardSpeed
        {
            get { return 0.1; }
        }

        private static double m_SortSpeed = 0;
        /// <summary>
        /// 排序速度
        /// </summary>
        public static double SortSpeed
        {
            get { return GameOptions.m_SortSpeed; }
            set { GameOptions.m_SortSpeed = value; }
        }

        /// <summary>
        /// 当前回合的地主是谁
        /// </summary>
        public static CardPlayerType CurrentHost { get; set; }

        /// <summary>
        /// 地主是否赢了
        /// </summary>
        public static Boolean IsHostWin { get; set; }

        /// <summary>
        /// 出了多少次炸弹。
        /// </summary>
        public static int BombCount { get; set; }

        /// <summary>
        /// 当前回合的分数是多少。
        /// </summary>
        public static ScoreType CurrentScoreType { get; set; }

        private static GameStatus m_GameStatus;

        /// <summary>
        /// 当前一圈牌的状态
        /// </summary>
        public static GameStatus GameStatus
        {
            get { return GameOptions.m_GameStatus; }
            set { GameOptions.m_GameStatus = value; }
        }

        /// <summary>
        /// 系统是否需要等待
        /// </summary>
        public static bool IsNeedWaiting { get; set; }

        /// <summary>
        /// 最后一个出牌者出的牌记录。
        /// </summary>
        public static int[] LastOutPutCardArray { get; set; }

        private static int m_NoOutPutCardCount = 2;
        /// <summary>
        /// 记录有多少次没有出牌记录了。一旦有出牌记录，这个就清0，如果为2那么就说明是第一个出牌的人。
        /// 所以它的初始值为2
        /// </summary>
        public static int NoOutPutCardCount
        {
            get { return GameOptions.m_NoOutPutCardCount; }
            set { GameOptions.m_NoOutPutCardCount = value; }
        }

        private static ScoreType m_UserSelectMark = ScoreType.NoChoose;

        /// <summary>
        /// 用户选择的分数，初始化为NoChoose;
        /// </summary>
        public static ScoreType UserSelectMark
        {
            get { return GameOptions.m_UserSelectMark; }
            set { GameOptions.m_UserSelectMark = value; }
        }
        /// <summary>
        /// 当前选择的分是多少
        /// </summary>
        public static ScoreType CurrentMark { get; set; }

        /// <summary>
        /// 设置游戏的下一个状态。
        /// </summary>
        public static void MoveNextStatus()
        {
            switch (GameStatus)
            {
                case GameStatus.GameStart:
                    GameStatus = GameStatus.DealCard;
                    break;
                case GameStatus.DealCard:
                    GameStatus = GameStatus.SortCard;
                    break;
                case GameStatus.SortCard:
                    GameStatus = GameStatus.SelectHost;
                    break;
                case GameStatus.LeftLeadCard:
                    GameStatus = GameStatus.MiddleLeadCard;
                    break;
                case GameStatus.MiddleLeadCard:
                    GameStatus = GameStatus.RightLeadCard;
                    break;
                case GameStatus.RightLeadCard:
                    GameStatus = GameStatus.LeftLeadCard;
                    break;
            }
        }
    }
}
