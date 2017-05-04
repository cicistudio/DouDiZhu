using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using CiCiStudio.CardFramework.CardPlayers;
using CiCiCard.ConfigClass;
using AIFrameWork;
using CiCiStudio.CardFramework.CommonClass;
using System.Windows.Controls;
using CiCiStudio.CardFramework;
using System.IO;
using System.Threading;
using System.Windows.Media;


namespace CiCiCard.Cycle
{
    public class CycleNewGame : CycleBase, IGameClear
    {
        protected override void NextStatus()
        {
            ClearCard();
            NewCycle();
            SetPlayerLocation();
            foreach (CardBase card in CardBaseCollection)
            {
                MainWindow.CanvasTable.Children.Insert(0, card.Card);
                //居中显示所有牌
                Canvas.SetLeft(card.Card, 450);
                Canvas.SetTop(card.Card, 200);
                // card.Card.GotFocus += new RoutedEventHandler(OnControlGotFocus);
                //注册名称
                //this.RegisterName(card.Card.Name, card.Card);
            }
            AnimationFinished();
        }

        #region IGameClear
        public void ClearCard()
        {
            MainWindow.CanvasTable.Children.Clear();
            PlayerHelper.ClearPlayerCard();
            SetControlHiden();
            GameOptions.NoOutPutCardCount = 2;
            GameOptions.UserSelectMark = ScoreType.NoChoose;
            GameOptions.CurrentMark = ScoreType.Pass;
            GameOptions.BombCount = 0;//炸弹重置为0
#if DEBUG
            File.Delete("log.txt");
#endif
        }

        private void SetControlHiden()
        {
            //按钮
            MainWindow.buttonMiOne.Content = "1分";
            MainWindow.buttonMiOne.IsEnabled = true;
            MainWindow.buttonMiOne.Visibility = Visibility.Hidden;
            MainWindow.buttonMiOne.Foreground = Brushes.Red;
            MainWindow.buttonMiTwo.Content = "2分";
            MainWindow.buttonMiTwo.IsEnabled = true;
            MainWindow.buttonMiTwo.Visibility = Visibility.Hidden;
            MainWindow.buttonMiTwo.Foreground = Brushes.Red;
            MainWindow.buttonMiThree.Content = "3分";
            MainWindow.buttonMiThree.IsEnabled = true;
            MainWindow.buttonMiThree.Visibility = Visibility.Hidden;
            MainWindow.buttonMiThree.Foreground = Brushes.Red;
            MainWindow.buttonMiFour.Content = "跳过";
            MainWindow.buttonMiFour.IsEnabled = true;
            MainWindow.buttonMiFour.Visibility = Visibility.Hidden;
            MainWindow.buttonMiFour.Foreground = Brushes.Red;
            MainWindow.imageLeft.Visibility = Visibility.Hidden;
            MainWindow.imageMiddle.Visibility = Visibility.Hidden;
            MainWindow.imageRight.Visibility = Visibility.Hidden;
            //Lable
            SetLabelHide();
        }
        #endregion

        private void SetPlayerLocation()
        {
            PluginManage.Invoke(CardPlayerType.LeftPlayer, "SetPlayerLocation", new object[] { CardPlayerType.LeftPlayer });
            if (PluginManage.ConfigInfo.IsMiddleAI)
            {
                PluginManage.Invoke(CardPlayerType.MiddlePlayer, "SetPlayerLocation", new object[] { CardPlayerType.MiddlePlayer });
            }
            PluginManage.Invoke(CardPlayerType.RightPlayer, "SetPlayerLocation", new object[] { CardPlayerType.RightPlayer });
#if DEBUG
            StringBuilder log = new StringBuilder();
            log.Append("--------------------SetPlayerLocation---------------------\r\n");
            log.Append("通知各个玩家座位情况\r\n");
            File.AppendAllText("Log.txt", log.ToString());
            Thread.Sleep(500);
#endif
        }

        private void NewCycle()
        {
            PluginManage.Invoke(CardPlayerType.LeftPlayer, "NewCycle");
            if (PluginManage.ConfigInfo.IsMiddleAI)
            {
                PluginManage.Invoke(CardPlayerType.MiddlePlayer, "NewCycle");
            }
            PluginManage.Invoke(CardPlayerType.RightPlayer, "NewCycle");
#if DEBUG
            StringBuilder log = new StringBuilder();
            log.Append("--------------------NewCycle---------------------\r\n");
            log.Append("通知各个玩家新游戏已经开始了\r\n");
            File.AppendAllText("Log.txt", log.ToString());
            Thread.Sleep(500);
#endif
        }
    }
}
