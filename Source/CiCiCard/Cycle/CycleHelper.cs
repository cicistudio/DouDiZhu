using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Windows;
using System.Threading;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using CiCiCard.Dialogs;
using CiCiCard.ConfigClass;
using CiCiStudio.CardFramework;
using CiCiStudio.CardFramework.CommonClass;
using AIFrameWork;

namespace CiCiCard.Cycle
{
    public class CycleHelper
    {
        private static CycleHelper m_CycleHelper = null;
        private DispatcherTimer m_AnimationTimer = new DispatcherTimer();
        private List<CardBase> m_CardBaseCollection;
        private int m_OutPutIndex = 1;
        private int m_PlayerIndex = 0;
        private int m_HostSelectedIndex = 0;
        private List<int> m_HostSelectedMarkArray = new List<int>();
        MainWindow m_MainWindow = null;

        /// <summary>
        /// 使用单件模式，因为旧对象的计时器会干扰到新开的游戏。
        /// </summary>
        private CycleHelper()
        {
            m_AnimationTimer.Interval = TimeSpan.FromSeconds(GameOptions.PopCardSpeed);//在WPF中写委托调用界面控件实在是太麻烦了。用计时器来做。
            m_AnimationTimer.Tick += new EventHandler(AnimationTimer_Tick);
        }

        public static CycleHelper GetGameCycle()
        {
            if (m_CycleHelper == null)
            {
                m_CycleHelper = new CycleHelper();
            }
            return m_CycleHelper;
        }

        public void Play(MainWindow main)
        {
            m_MainWindow = main;
            m_AnimationTimer.Start();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            if (GameOptions.IsNeedWaiting)
            {
                return;
            }

            if (GameOptions.GameStatus == GameStatus.GameEnd)
            {
                m_AnimationTimer.Stop();
                //將所有的牌亮出來
                foreach(CardBase card in m_CardBaseCollection)
                {
                    card.SetCard();
                }

                SetScore();
                return;
            }
            PlayGame();
        }

        /// <summary>
        /// 设置分数
        /// </summary>
        private void SetScore()
        {
            //读取分数到对象
            ScoreInfo score = null;
            BinaryFormatter formatter = new BinaryFormatter();
            using (Stream stream = File.Open(Environment.CurrentDirectory + "\\Score.bin", FileMode.Open))
            {
                score = (ScoreInfo)formatter.Deserialize(stream);
                stream.Close();
            }
            ScoreDialog dialog = new ScoreDialog();
            //重置分数
            //炸弹个数   标准计分规则         
            //0		        3   *1  2^0
            //1		        6   *2  2^1
            //2		        12  *4  2^2
            //3		        24  *8  2^3
            //4		        48  *16 2^4
            //5		        96  *32 2^5
            int farmer = (int)(GetScoreFromCurrentMark() * Math.Pow(2, GameOptions.BombCount));
            int host = farmer * 2;
            if (GameOptions.IsHostWin)
            {
                if (GameOptions.CurrentHost == CardPlayerType.LeftPlayer)
                {
                    score.LeftScore += host;
                    dialog.LeftScore = " (+" + host +")";
                    score.MiddleScore -= farmer;
                    dialog.MiddleScore = " (-" + farmer +")";
                    score.RightScore -= farmer;
                    dialog.RightScore = " (-" + farmer +")";
                }
                else if (GameOptions.CurrentHost == CardPlayerType.MiddlePlayer)
                {
                    score.LeftScore -= farmer;
                    dialog.LeftScore = " (-" + farmer +")";
                    score.MiddleScore += host;
                    dialog.MiddleScore = " (+" + host + ")";
                    score.RightScore -= farmer;
                    dialog.RightScore = " (-" + farmer + ")";
                }
                else
                {
                    score.LeftScore -= farmer;
                    dialog.LeftScore = " (-" + farmer + ")";
                    score.MiddleScore -= farmer;
                    dialog.MiddleScore = " (-" + farmer + ")";
                    score.RightScore += host;
                    dialog.RightScore = " (+" + host + ")";
                }
            }
            else
            {
                if (GameOptions.CurrentHost == CardPlayerType.LeftPlayer)
                {
                    score.LeftScore -= host;
                    dialog.LeftScore = " (-" + host + ")";
                    score.MiddleScore += farmer;
                    dialog.MiddleScore = " (+" + farmer + ")";
                    score.RightScore += farmer;
                    dialog.RightScore = " (+" + farmer + ")";
                }
                else if (GameOptions.CurrentHost == CardPlayerType.MiddlePlayer)
                {
                    score.LeftScore += farmer;
                    dialog.LeftScore = " (+" + farmer + ")";
                    score.MiddleScore -= host;
                    dialog.MiddleScore = " (-" + host + ")";
                    score.RightScore += farmer;
                    dialog.RightScore = " (+" + farmer + ")";
                }
                else
                {
                    score.LeftScore += farmer;
                    dialog.LeftScore = " (+" + farmer + ")";
                    score.MiddleScore += farmer;
                    dialog.MiddleScore = " (+" + farmer + ")";
                    score.RightScore -= host;
                    dialog.RightScore = " (-" + host + ")";
                }
            }

            //写入分数到文件，并弹出分数对话框
            //创建一个文件流对象stream，指向文件MyFile.bin
            using (Stream stream = new FileStream(Environment.CurrentDirectory + "\\Score.bin", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                //通过formatter对象以二进制格式将obj对象序列化后到文件MyFile.bin中
                formatter.Serialize(stream, score);
                stream.Close();
            }

            if (dialog.ShowDialog().Value)
            {
                GameOptions.GameStatus = GameStatus.GameStart;
                GameOptions.IsNeedWaiting = false;
                Play(m_MainWindow);
            }
            else
            {
                m_MainWindow.CanvasTable.Children.Clear();
                #region SetHidden
                m_MainWindow.buttonMiOne.Visibility = Visibility.Hidden;
                m_MainWindow.buttonMiTwo.Visibility = Visibility.Hidden;
                m_MainWindow.buttonMiThree.Visibility = Visibility.Hidden;
                m_MainWindow.buttonMiFour.Visibility = Visibility.Hidden;
                m_MainWindow.labelLeft.Visibility = Visibility.Hidden;
                m_MainWindow.labelMiddle.Visibility = Visibility.Hidden;
                m_MainWindow.labelRight.Visibility = Visibility.Hidden;
                m_MainWindow.imageLeft.Visibility = Visibility.Hidden;
                m_MainWindow.imageMiddle.Visibility = Visibility.Hidden;
                m_MainWindow.imageRight.Visibility = Visibility.Hidden;
                #endregion
            }
        }

        private int GetScoreFromCurrentMark()
        {
            switch (GameOptions.CurrentMark)
            {
                case ScoreType.Pass:
                    return 0;
                case ScoreType.One:
                    return 1;
                case ScoreType.Two:
                    return 2;
                case ScoreType.Three:
                    return 3;
            }
            return 0;
        }

        private void PlayGame()
        {
            //这里就不用反射了，反射用多了，不好用于加密操作。
            CycleBase cycle = null;          
            switch (GameOptions.GameStatus)
            {
                case GameStatus.GameStart:
                    m_OutPutIndex = 1;
                    m_PlayerIndex = 0;
                    m_HostSelectedIndex = 0;
                    m_CardBaseCollection = CardHelper.GetCardCollection();//获得随机的一副牌。
                    m_HostSelectedMarkArray.Clear();
                    cycle = new CycleNewGame();
                    break;
                case GameStatus.DealCard:
                    cycle = new CycleDealCard();
                    break;
                case GameStatus.SortCard:
                    cycle = new CycleSortCard();
                    break;
                case GameStatus.SelectHost:
                    cycle = new CycleSelectHost();
                    break;
                case GameStatus.LeftLeadCard:
                    cycle = new CycleLeftLeadCard();
                    break;
                case GameStatus.MiddleLeadCard:
                    cycle = new CycleMiddleLeadCard();
                    break;
                case GameStatus.RightLeadCard:
                    cycle = new CycleRightLeadCard();
                    break;
                case GameStatus.GameEnd:
                    //cycle = new CycleGameEnd();
                    m_AnimationTimer.Stop();
                    break;
            }
            cycle.CardBaseCollection = m_CardBaseCollection;
            cycle.HostSelectedIndex = m_HostSelectedIndex;
            cycle.OutPutIndex = m_OutPutIndex;
            cycle.PlayerIndex = m_PlayerIndex;
            cycle.MainWindow = m_MainWindow;
            cycle.HostSelectedMarkArray = m_HostSelectedMarkArray;
            cycle.MoveNext();
            m_OutPutIndex = cycle.OutPutIndex;
            m_PlayerIndex = cycle.PlayerIndex;
            m_HostSelectedIndex = cycle.HostSelectedIndex;
        }
    }
}
