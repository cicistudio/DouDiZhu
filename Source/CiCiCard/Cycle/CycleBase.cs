using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Threading;
using System.IO;
using CiCiStudio.CardFramework.CommonClass;
using CiCiStudio.CardFramework.CardPlayers;
using CiCiStudio.CardFramework;
using CiCiCard.ConfigClass;
using AIFrameWork;


namespace CiCiCard.Cycle
{
    public abstract class CycleBase
    {
        /// <summary>
        /// 存放扑克的地方
        /// </summary>
        public List<CardBase> CardBaseCollection {get;set;}
        /// <summary>
        /// 主窗体
        /// </summary>
        public MainWindow MainWindow {get;set;}
        /// <summary>
        /// 第几圈
        /// </summary>
        public int OutPutIndex { get; set; }
        /// <summary>
        /// 从地主开始，第几个出牌的
        /// </summary>
        public int PlayerIndex { get; set; }

        /// <summary>
        /// 地主选择了几次了。
        /// </summary>
        public int HostSelectedIndex { get; set; }

        /// <summary>
        /// 用于存储游戏选择地主的分值记录
        /// </summary>
        public List<int> HostSelectedMarkArray { get; set; }

        protected abstract void NextStatus();
        
        public void MoveNext()
        {
            GameOptions.IsNeedWaiting = true;//每调用一个状态前必须设置为等待模式
            if (GameOptions.NoOutPutCardCount == 2)
            {   
                //每个回合开始时要清除一些标记和桌上的牌
                RemoveAllOutPutCards();
                SetLabelHide();
            }
            switch (GameOptions.GameStatus)
            {
                case GameStatus.LeftLeadCard:
                    RemoveOutPutCards(CardPlayerType.LeftPlayer);
                    break;
                case GameStatus.MiddleLeadCard:
                    RemoveOutPutCards(CardPlayerType.MiddlePlayer);
                    break;
                case GameStatus.RightLeadCard:
                    RemoveOutPutCards(CardPlayerType.RightPlayer);
                    break;
            }
            NextStatus();
        }

        //清除桌上打出的所有的牌
        private void RemoveAllOutPutCards()
        {
            var q = from c in CardBaseCollection
                    where c.Card.IsOutPut == true
                    select c;
            foreach (CardBase card in q.ToList())
            {
                if (MainWindow.CanvasTable.Children.Contains(card.Card))
                {
                    MainWindow.CanvasTable.Children.Remove(card.Card);
                }
            }
        }

        private void RemoveOutPutCards(CardPlayerType player)
        {
            var q = from c in CardBaseCollection
                    where c.Card.IsOutPut == true && c.Card.CardPlayType == player
                    select c;
            foreach (CardBase card in q)
            {
                if (MainWindow.CanvasTable.Children.Contains(card.Card))
                {
                    MainWindow.CanvasTable.Children.Remove(card.Card);
                }
            }
        }

        protected int[] GetPlayerCardArray(PlayerBase player)
        {
            var q = from c in player.CardCollection
                    orderby c.CardBase.CardNumber
                    select c.CardBase.CardNumber;
            List<int> arrayCollection = q.ToList();
            int[] array = new int[arrayCollection.Count];
            for (int i = 0; i < q.Count(); i++)
            {
                array[i] = arrayCollection[i];
            }
            return array;
        }

        protected void SetCardCollection(CardPlayerType player, int[] cardArray)
        {
            PluginManage.Invoke(player, "SetCardCollection", new object[] { cardArray });
#if DEBUG
            StringBuilder log = new StringBuilder();
            log.Append("--------------------SetCardCollection---------------------\r\n");
            switch (player)
            {
                case CardPlayerType.LeftPlayer:
                    log.Append("向左侧玩家传递牌：");
                    break;
                case CardPlayerType.MiddlePlayer:
                    log.Append("向中间玩家传递牌：");
                    break;
                case CardPlayerType.RightPlayer:
                     log.Append("向右侧玩家传递牌：");
                    break;
            }
            foreach (int i in cardArray)
            {
                log.Append(i);
                log.Append(",");
            }
            log.Append("\r\n");
            File.AppendAllText("log.txt",log.ToString());
            Thread.Sleep(500);
#endif
        }

        public void SetOutPutCardToEveryOne(CardPlayerType player, int[] cardArray)
        {
            PluginManage.Invoke(CardPlayerType.LeftPlayer, "SetOutPutCard", new object[] { player, cardArray, OutPutIndex });
            if (PluginManage.ConfigInfo.IsMiddleAI)
            {
                PluginManage.Invoke(CardPlayerType.MiddlePlayer, "SetOutPutCard", new object[] { player, cardArray, OutPutIndex });
            }
            PluginManage.Invoke(CardPlayerType.RightPlayer, "SetOutPutCard", new object[] { player, cardArray, OutPutIndex });
            if (cardArray != null && cardArray.Length != 0)
            {
                GameOptions.NoOutPutCardCount = 0;
            }
            else
            {
                GameOptions.NoOutPutCardCount++;
            }
#if DEBUG

                StringBuilder log = new StringBuilder();
                log.Append("--------------------SetOutPutCardToEveryOne---------------------\r\n");
                switch (player)
                {
                    case CardPlayerType.LeftPlayer:
                        log.Append("将左侧玩家出的牌传递给大家：");
                        break;
                    case CardPlayerType.MiddlePlayer:
                        log.Append("将中间玩家出的牌传递给大家：");
                        break;
                    case CardPlayerType.RightPlayer:
                        log.Append("将右侧玩家出的牌传递给大家：");
                        break;
                }
                if (cardArray != null && cardArray.Length != 0)
                {
                    foreach (int i in cardArray)
                    {
                        log.Append(i);
                        log.Append(",");
                    }
                }
                else
                {
                    log.Append("该玩家没有出牌！");
                }
                log.Append("\r\n");

                File.AppendAllText("log.txt", log.ToString());
                Thread.Sleep(500);

#endif
        }

        protected void SetHostToEveryOne(CardPlayerType player)
        {
            PluginManage.Invoke(CardPlayerType.LeftPlayer, "SetHost", new object[] { player });
            if (PluginManage.ConfigInfo.IsMiddleAI)
            {
                PluginManage.Invoke(CardPlayerType.MiddlePlayer, "SetHost", new object[] { player });
            }
            PluginManage.Invoke(CardPlayerType.RightPlayer, "SetHost", new object[] { player });
        }

        public void CheckGameEnd(CardPlayerType player)
        {
            List<PlayerCardInfo> playInfoCollection = null;
            switch (player)
            {
                case CardPlayerType.LeftPlayer:
                    playInfoCollection = PlayerHelper.LeftPlayer.CardCollection;
                    break;
                case CardPlayerType.MiddlePlayer:
                    playInfoCollection = PlayerHelper.MiddlePlayer.CardCollection;
                    break;
                case CardPlayerType.RightPlayer:
                    playInfoCollection = PlayerHelper.RightPlayer.CardCollection;
                    break;
            }
            if (playInfoCollection.Count == 0)
            {
                //如果当前地主就是当前牌先出结束的人，那么就可以判断是不是地主赢了。
                if (GameOptions.CurrentHost == player)
                {
                    GameOptions.IsHostWin = true;
                }
                else
                {
                    GameOptions.IsHostWin = false;
                }
                GameOptions.GameStatus = GameStatus.GameEnd;
            }
        }

        public void AnimationFinished()
        {
            GameOptions.MoveNextStatus();
            GameOptions.IsNeedWaiting = false;
        }

        protected void SetPlayerIndex()
        {
            PlayerIndex++;
            if (PlayerIndex == 3)
            {
                PlayerIndex = 0;
                OutPutIndex++;
            }
        }

        /// <summary>
        /// 使中间玩家可选或不可选牌
        /// </summary>
        /// <param name="canSelectedCard"></param>
        protected void SetMiddlePlayerCardSelected(bool canSelectedCard)
        {
            if (!PluginManage.ConfigInfo.IsMiddleAI)
            {
                foreach (PlayerCardInfo cardInfo in PlayerHelper.MiddlePlayer.CardCollection)
                {
                    cardInfo.CardBase.Card.SetCardSelected(canSelectedCard);
                }
            }
        }

        protected void SetLabelHide()
        {
            MainWindow.labelLeft.Visibility = Visibility.Hidden;
            MainWindow.labelMiddle.Visibility = Visibility.Hidden;
            MainWindow.labelRight.Visibility = Visibility.Hidden;
        }

        protected void ShowButtonsDealCard()
        {
            ShowButtonsChooseHost();
            MainWindow.buttonMiOne.Visibility = Visibility.Hidden;
            MainWindow.buttonMiTwo.Content = "出牌";
            MainWindow.buttonMiTwo.IsEnabled = false;
            MainWindow.buttonMiTwo.Foreground = Brushes.Gray;
            MainWindow.buttonMiThree.Content = "提示";
            MainWindow.buttonMiFour.Content = "跳过";
        }

        protected void ShowButtonsChooseHost()
        {
            MainWindow.buttonMiOne.Visibility = Visibility.Visible;
            MainWindow.buttonMiTwo.Visibility = Visibility.Visible;
            MainWindow.buttonMiThree.Visibility = Visibility.Visible;
            MainWindow.buttonMiFour.Visibility = Visibility.Visible;
            MainWindow.buttonMiOne.IsEnabled = true;
            MainWindow.buttonMiTwo.IsEnabled = true;
            MainWindow.buttonMiThree.IsEnabled = true;
            MainWindow.buttonMiFour.IsEnabled = true;
        }

        protected void HideButtonsDealCard()
        {
            MainWindow.buttonMiOne.Visibility = Visibility.Hidden;
            MainWindow.buttonMiTwo.Visibility = Visibility.Hidden;
            MainWindow.buttonMiThree.Visibility = Visibility.Hidden;
            MainWindow.buttonMiFour.Visibility = Visibility.Hidden;
        }

        protected void GetOutPutCardFromAILog(CardPlayerType player, int[] cardArray)
        {
#if DEBUG
            StringBuilder log = new StringBuilder();
            log.Append("--------------------GetOutPutCard-------" + GameOptions.NoOutPutCardCount + "-------" + OutPutIndex + "------\r\n");
            switch (player)
            {
                case CardPlayerType.LeftPlayer:
                    log.Append("左侧AI");
                    break;
                case CardPlayerType.MiddlePlayer:
                    log.Append("中间AI");
                    break;
                case CardPlayerType.RightPlayer:
                    log.Append("右侧AI");
                    break;
            }
            if (cardArray != null)
            {
                log.Append("出的牌为：");
                foreach (int i in cardArray)
                {
                    log.Append(i);
                    log.Append(",");
                }
            }
            else
            {
                log.Append("没有出牌。");
            }
            log.Append("\r\n");
            File.AppendAllText("log.txt", log.ToString());
            Thread.Sleep(500);
#endif
        }
    }
}
