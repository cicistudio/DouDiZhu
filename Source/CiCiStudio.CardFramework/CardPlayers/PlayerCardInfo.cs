using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CiCiStudio.CardFramework.CommonClass;
using System.Windows;
using AIFrameWork;

namespace CiCiStudio.CardFramework.CardPlayers
{
    public class PlayerCardInfo
    {
        /// <summary>
        /// 对扑克的简单封装，提供扑克的花色以及点数
        /// </summary>
        public CardBase CardBase { get; set; }

        /// <summary>
        /// 当前牌的位置
        /// </summary>
        public Point Location { 
            get
            {
                if (CardIndex != -1 && CardIndex != -2 && CardIndex != -3)
                {
                    //不是最后三张牌
                    switch (CardPlayer)
                    {
                        case CardPlayerType.LeftPlayer:
                            return new Point(GameOptions.LeftPlayerFirstLocation.X, GameOptions.LeftPlayerFirstLocation.Y + GameOptions.CardSpace * CardIndex);
                        case CardPlayerType.MiddlePlayer:
                            return new Point(GameOptions.MiddlePlayerFirstLocation.X + GameOptions.CardSpace * CardIndex, GameOptions.MiddlePlayerFirstLocation.Y);
                        case CardPlayerType.RightPlayer:
                            return new Point(GameOptions.RightPlayerFirstLocation.X, GameOptions.RightPlayerFirstLocation.Y + GameOptions.CardSpace * CardIndex);                           
                    }
                }
                else
                {
                    switch (CardIndex)
                    {
                        case -1:
                            return new Point(-150, -150);
                        case -2:
                            return new Point(0, -150);
                        case -3:
                            return new Point(150, -150);
                    }
                }
                return new Point(0, 0);
            } 
        }

        /// <summary>
        /// 当前牌在谁的手中
        /// </summary>
        private CardPlayerType m_CardPlayer;

        public CardPlayerType CardPlayer
        {
            get { return m_CardPlayer; }
            set 
            { 
                m_CardPlayer = value;
                CardBase.Card.CardPlayType = value;
            }
        }

        public int CardIndex { get; set; }
    }
}
