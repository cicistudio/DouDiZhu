using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CiCiStudio.CardFramework.CommonClass;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using AIFrameWork;

namespace CiCiStudio.CardFramework.CardPlayers
{
    public class PlayerHelper
    {

        private static int m_CardCount = 0;
        
        private static LeftPlayer m_LeftPlayer = new LeftPlayer();

        public static LeftPlayer LeftPlayer
        {
            get { return PlayerHelper.m_LeftPlayer; }
            set { PlayerHelper.m_LeftPlayer = value; }
        }

        private static RightPlayer m_RightPlayer = new RightPlayer();

        public static RightPlayer RightPlayer
        {
            get { return PlayerHelper.m_RightPlayer; }
            set { PlayerHelper.m_RightPlayer = value; }
        }

        private static MiddlePlayer m_MiddlePlayer = new MiddlePlayer();

        public static MiddlePlayer MiddlePlayer
        {
            get { return PlayerHelper.m_MiddlePlayer; }
            set { PlayerHelper.m_MiddlePlayer = value; }
        }

        /// <summary>
        /// 清除所有的牌，用于重新发牌
        /// </summary>
        public static void ClearPlayerCard()
        {
            m_LeftPlayer.CardCollection.Clear();
            m_RightPlayer.CardCollection.Clear();
            m_MiddlePlayer.CardCollection.Clear();
            m_CardCount = 0;
        }

        /// <summary>
        /// 对所有玩家的牌进行重新排序
        /// </summary>
        public static void SortAllPlayerCard(FrameworkElement main, Canvas canvasTable, Storyboard story)
        {
            double saveSpeed = GameOptions.DealSpeed;
            m_LeftPlayer.CardCollection = SortSinglePlayerCard(m_LeftPlayer.CardCollection, CardPlayerType.LeftPlayer, canvasTable, story);//左侧玩家排序
            m_MiddlePlayer.CardCollection = SortSinglePlayerCard(m_MiddlePlayer.CardCollection, CardPlayerType.MiddlePlayer, canvasTable, story);//中间玩家排序
            m_RightPlayer.CardCollection = SortSinglePlayerCard(m_RightPlayer.CardCollection, CardPlayerType.RightPlayer, canvasTable, story);//右侧玩家排序。           
        }

        //private static void story_Completed(object sender, EventArgs e)
        //{
        //    GameOptions.MoveNextStatus();
        //    GameOptions.IsNeedWaiting = false;
        //}

        public static List<PlayerCardInfo> SortSinglePlayerCard(List<PlayerCardInfo> playerCollection, CardPlayerType player, Canvas canvasTable, Storyboard story)
        {
            //使用Linq语法
            var query = from card in playerCollection
                        orderby card.CardBase.CardNumber descending, card.CardBase.CardType//根据Number和类型排序,前者是倒序，后者顺序，先以前者为准。前者为同样的值，则再按照后者排序。
                        select card;
            List<PlayerCardInfo> cardInfoCollection = query.ToList();
            List<PlayerCardInfo> cards = new List<PlayerCardInfo>();
            for (int i = 0; i < playerCollection.Count; i++)
            {
                cardInfoCollection[i].CardIndex = i;
                CardAnimation animation = new CardAnimation(cardInfoCollection[i].CardBase.Card);
                Point point = GetLocationFromIndex(i, player);
                animation.MoveCard(point.X, point.Y, TimeSpan.FromSeconds(GameOptions.SortSpeed * i), story);
                //将其上下位置重新排列
                canvasTable.Children.Remove(cardInfoCollection[i].CardBase.Card);
                canvasTable.Children.Add(cardInfoCollection[i].CardBase.Card);
                //cardInfoCollection[i].CardBase.SetCard();
                cards.Add(cardInfoCollection[cardInfoCollection.Count - i - 1]);
            }    
            return cards;
        }

        /// <summary>
        /// 根据index和使用者就可以获得当前牌的位置
        /// </summary>
        private static Point GetLocationFromIndex(int index, CardPlayerType player)
        {
            switch (player)
            {
                case CardPlayerType.LeftPlayer:
                    return new Point(GameOptions.LeftPlayerFirstLocation.X, GameOptions.LeftPlayerFirstLocation.Y + GameOptions.CardSpace * index);
                case CardPlayerType.MiddlePlayer:
                    return new Point(GameOptions.MiddlePlayerFirstLocation.X + GameOptions.CardSpace * index, GameOptions.MiddlePlayerFirstLocation.Y);
                case CardPlayerType.RightPlayer:
                    return new Point(GameOptions.RightPlayerFirstLocation.X, GameOptions.RightPlayerFirstLocation.Y + GameOptions.CardSpace * index);
            }
            return new Point(0, 0);
        }
       
        /// <summary>
        /// 将牌发给不同的玩家
        /// </summary>
        /// <param name="i">发牌的第几张牌</param>
        /// <param name="card">玩家的对象</param>
        public static void AddCardToPlayer(int i,PlayerCardInfo card)
        {
            card.CardIndex = m_CardCount;
            if (i < 51)
            {
                switch (i % 3)
                {
                    case 0:
                        m_LeftPlayer.CardCollection.Add(card);
                        card.CardPlayer = CardPlayerType.LeftPlayer;
                        
                        break;
                    case 1:
                        m_MiddlePlayer.CardCollection.Add(card);
                        card.CardPlayer = CardPlayerType.MiddlePlayer;
                        card.CardBase.SetCard();
                        //player.CardBase.Card.SetCardSelected(true);//只有自己的牌才可以被选择。
                        break;
                    case 2:
                        m_RightPlayer.CardCollection.Add(card);
                        card.CardPlayer = CardPlayerType.RightPlayer;
                        m_CardCount++;
                        break;
                }
            }
            else
            {
                //最后三张牌。
                switch (i)
                {
                    case 51:
                        card.CardIndex = -1;
                        break;
                    case 52:
                        card.CardIndex = -2;
                        break;
                    case 53:
                        card.CardIndex = -3;
                        break;
                }
            }
        }

        public static void AddToPlayer(PlayerCardInfo player,CardPlayerType playerType)
        {
            player.CardIndex = m_CardCount;
            player.CardPlayer = playerType;
            switch (playerType)
            {
                case CardPlayerType.LeftPlayer:
                    m_LeftPlayer.CardCollection.Add(player);
                    break;
                case CardPlayerType.MiddlePlayer:
                    m_MiddlePlayer.CardCollection.Add(player);                  
                    break;
                case CardPlayerType.RightPlayer:
                    m_RightPlayer.CardCollection.Add(player);
                    break;
            }
            m_CardCount++;
        }
    }
}
