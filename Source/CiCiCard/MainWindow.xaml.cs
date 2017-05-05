using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Collections;
using CiCiCard.Dialogs;
using CiCiCard.ConfigClass;
using CiCiStudio.CardFramework;
using CiCiStudio.CardFramework.CommonClass;
using CiCiCard.Cycle;
using CiCiStudio.CardFramework.CardPlayers;
using AIFrameWork;
/*************************************************************
 * Copyright by cicistudio 2000-2017
 * http://chengchen.cnblogs.com
 * https://github.com/cicistudio/
 * The Current code cannot be used in the Commercial software.
 * Mail to me if you have any queries. cicistudio@qq.com
 ************************************************************/
namespace CiCiCard
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private CycleMiddleLeadCard m_MiddleCycle = new CycleMiddleLeadCard();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void NewGame()
        {
            GameOptions.GameStatus = GameStatus.GameStart;
            GameOptions.IsNeedWaiting = false;
            CycleHelper.GetGameCycle().Play(this);
        }

        #region 事件

        private void menuItemNewGame_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NewGame();
        }

        private void menuItemExit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	this.Close();
        }

        private void menuItemSettings_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            PlayerSettingDialog dialog = new PlayerSettingDialog();
            dialog.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!PluginManage.LoadPluginFromXML())
                {
                    this.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void buttonMiOne_Click(object sender, RoutedEventArgs e)
        {
            if (GameOptions.GameStatus == GameStatus.SelectHost)
            {
                //地主选分
                GameOptions.UserSelectMark = ScoreType.One;
                GameOptions.IsNeedWaiting = false;
            }
        }

        private void buttonMiTwo_Click(object sender, RoutedEventArgs e)
        {
            if (GameOptions.GameStatus == GameStatus.SelectHost)
            {
                GameOptions.UserSelectMark = ScoreType.Two;
                GameOptions.IsNeedWaiting = false;
            }
            else
            {
                //出牌
                OutPutCardByUser();
            }
        }

        private void OutPutCardByUser()
        {
            var query = from c in PlayerHelper.MiddlePlayer.CardCollection
                        where c.CardBase.Card.IsSelected == true
                        orderby c.CardBase.CardNumber
                        select c;
            List<PlayerCardInfo> cardInfoCollection = query.ToList();
            int[] cardArray = new int[cardInfoCollection.Count];
            for (int i = 0; i < cardInfoCollection.Count; i++)
            {
                cardArray[i] = cardInfoCollection[i].CardBase.CardNumber;
            }
            DealCard(cardArray);
        }

        private void buttonMiThree_Click(object sender, RoutedEventArgs e)
        {
            if (GameOptions.GameStatus == GameStatus.SelectHost)
            {
                GameOptions.UserSelectMark = ScoreType.Three;
                GameOptions.IsNeedWaiting = false;
            }
            else
            {
                //提示
                MessageBox.Show("暂时尚未开发此功能", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void buttonMiFour_Click(object sender, RoutedEventArgs e)
        {
            if (GameOptions.GameStatus == GameStatus.SelectHost)
            {
                GameOptions.UserSelectMark = ScoreType.Pass;
                GameOptions.IsNeedWaiting = false;
            }
            else
            {
                //跳过
               // GameOptions.NoOutPutCardCount++;
                labelMiddle.Visibility = Visibility.Visible;
                labelMiddle.Content = "跳过";
                m_MiddleCycle.SetOutPutCardToEveryOne(CardPlayerType.MiddlePlayer, null);
                //将所有的牌都收回
                foreach (PlayerCardInfo cardInfo in PlayerHelper.MiddlePlayer.CardCollection)
                {
                    if (cardInfo.CardBase.Card.IsSelected)
                    {
                        cardInfo.CardBase.Card.UnSelectCard();
                    }
                }
                m_MiddleCycle.AnimationFinished();
            }
        }

        private void DealCard(int[] cardArray)
        {

            if (cardArray != null && cardArray.Length > 0)
            {
                // GameOptions.NoOutPutCardCount = 0;
                RuleType rule = RuleHelper.GetRuleType(cardArray);
                if (rule == RuleType.FourAndZero || rule == RuleType.JokersBomb)
                {
                    GameOptions.BombCount++;//如果有炸弹出现，就增加统计。
                }

                GameOptions.LastOutPutCardArray = cardArray;
                //出牌以及动画
                List<CardBase> outPutCardCollection = new List<CardBase>();
                foreach (int n in cardArray)
                {
                    var q = from c in PlayerHelper.MiddlePlayer.CardCollection
                            where c.CardBase.CardNumber == n && c.CardBase.Card.IsSelected == true
                            select c;
                    if (q.Count() == 0)
                    {
                        throw new Exception("系统出现未知问题，请与开发者联系！");
                    }
                    outPutCardCollection.Add(q.First().CardBase);
                    q.First().CardBase.Card.IsOutPut = true;
                    PlayerHelper.MiddlePlayer.CardCollection.Remove(q.First());
                }

                //重新排序
                Storyboard story = new Storyboard();
                // GameOptions.SortSpeed = 0.001;
                PlayerHelper.SortSinglePlayerCard(PlayerHelper.MiddlePlayer.CardCollection, CardPlayerType.MiddlePlayer, this.CanvasTable, story);
                story.Begin();

                //出牌动画
                Storyboard storyOutPut = new Storyboard();
                int i = 0;
                foreach (CardBase card in outPutCardCollection)
                {
                    CardAnimation animation = new CardAnimation(this, card.Card);
                    this.CanvasTable.Children.Remove(card.Card);
                    this.CanvasTable.Children.Add(card.Card);//换顺序
                    card.SetCard();
                    card.Card.SetCardSelected(false);
                    animation.MoveCard(GameOptions.MiddlePlayerFirstLocation.X + i * GameOptions.CardSpace, GameOptions.MiddlePlayerFirstLocation.Y - card.Card.Height - GameOptions.CardSpace, TimeSpan.FromSeconds(i * 0.1), storyOutPut);
                    i++;
                }
                storyOutPut.Completed += new EventHandler(storyOutPut_Completed);
                storyOutPut.Begin();
                //出牌后，检查游戏是否结束
                m_MiddleCycle.CheckGameEnd(CardPlayerType.MiddlePlayer);
                this.labelMiddle.Visibility = Visibility.Hidden;
            }
            m_MiddleCycle.SetOutPutCardToEveryOne(CardPlayerType.MiddlePlayer, cardArray);
            if (cardArray == null || cardArray.Length == 0)
            {
                m_MiddleCycle.AnimationFinished();//如果cardArray为Null就不会触发storyOutPut_Completed事件
            }
        }

        private void storyOutPut_Completed(object sender, EventArgs e)
        {
            m_MiddleCycle.AnimationFinished();
        }

        private void menuItemScore_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ScoreDialog dialog = new ScoreDialog();
            if (dialog.ShowDialog().Value)
            {
                NewGame();
            }
        }

        private void Window_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (GameOptions.GameStatus == GameStatus.MiddleLeadCard && !PluginManage.ConfigInfo.IsMiddleAI && buttonMiTwo.IsEnabled)
            {
                //出牌
                OutPutCardByUser();
            }
        }

        private void menuItemAbout_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            About dialog = new About();
            dialog.ShowDialog();
        }

        private void menuItemOptions_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
        }

        #endregion
    }
}
