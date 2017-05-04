using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CiCiStudio.CardFramework;
using CiCiCard.ConfigClass;
using System.Windows.Media.Animation;
using AIFrameWork;
using CiCiStudio.CardFramework.CardPlayers;
using CiCiStudio.CardFramework.CommonClass;
using System.Windows;
using System.Collections;
using System.Windows.Media;
using System.IO;
using System.Threading;

namespace CiCiCard.Cycle
{
    public class CycleSelectHost : CycleBase
    {
        protected override void NextStatus()
        {
            if (SetRandomHost())
            {
                //将最后三张牌发给地主之前，先翻开，让大家看看
                CardBaseCollection[51].SetCard();
                CardBaseCollection[52].SetCard();
                CardBaseCollection[53].SetCard();

                switch (GameOptions.CurrentHost)
                {
                    case CardPlayerType.LeftPlayer:
                        MainWindow.imageLeft.Visibility = Visibility.Visible;
                        SetHostToEveryOne(CardPlayerType.LeftPlayer);
                        GameOptions.GameStatus = GameStatus.LeftLeadCard;
                        break;
                    case CardPlayerType.MiddlePlayer:
                        MainWindow.imageMiddle.Visibility = Visibility.Visible;
                        SetHostToEveryOne(CardPlayerType.MiddlePlayer);
                        GameOptions.GameStatus = GameStatus.MiddleLeadCard;
                        break;
                    case CardPlayerType.RightPlayer:
                        MainWindow.imageRight.Visibility = Visibility.Visible;
                        SetHostToEveryOne(CardPlayerType.RightPlayer);
                        GameOptions.GameStatus = GameStatus.RightLeadCard;
                        break;
                }

                //处理最后三张牌发给地主的动画
                Storyboard story = new Storyboard();
                for (int i = 51; i < 54; i++)
                {
                    CardAnimation animation = new CardAnimation(MainWindow, CardBaseCollection[i].Card);
                    PlayerCardInfo player = new PlayerCardInfo();
                    player.CardBase = CardBaseCollection[i];
                    PlayerHelper.AddToPlayer(player, GameOptions.CurrentHost);
                    Point point = new Point();
                    switch (GameOptions.CurrentHost)
                    {
                        case CardPlayerType.LeftPlayer:
                            point.X = GameOptions.LeftPlayerFirstLocation.X;
                            point.Y = GameOptions.LeftPlayerFirstLocation.Y + 8 * GameOptions.CardSpace;
                            break;
                        case CardPlayerType.MiddlePlayer:
                            point.X = GameOptions.MiddlePlayerFirstLocation.X + 8 * GameOptions.CardSpace;
                            point.Y = GameOptions.MiddlePlayerFirstLocation.Y;
                            break;
                        case CardPlayerType.RightPlayer:
                            point.X = GameOptions.RightPlayerFirstLocation.X;
                            point.Y = GameOptions.RightPlayerFirstLocation.Y + 8 * GameOptions.CardSpace;
                            break;
                    }
                    animation.MoveCard(point.X, point.Y, TimeSpan.FromSeconds((i-50)*0.2), story);
                }
                story.Completed += new EventHandler(story_Completed);
                story.Begin();
            }
            else
            {
                return;//如果地主没有选出来，就返回。
            }

            if (!PluginManage.ConfigInfo.IsMiddleAI)
            {
                foreach (PlayerCardInfo cardInfo in PlayerHelper.MiddlePlayer.CardCollection)
                {
                    //如果地主选出来了，就将牌是否可以选择注册到委托中去。
                    cardInfo.CardBase.Card.CardSelected = new Card.CardSelectedDelegate(CardSelected);
                    //cardInfo.CardBase.Card.CanSelected = true;
                }
            }

            //通知所有玩家，将他们的手牌给他们。
            SetCardCollection(CardPlayerType.LeftPlayer, GetPlayerCardArray(PlayerHelper.LeftPlayer));
            SetCardCollection(CardPlayerType.RightPlayer, GetPlayerCardArray(PlayerHelper.RightPlayer));
            if (PluginManage.ConfigInfo.IsMiddleAI)
            {
                SetCardCollection(CardPlayerType.MiddlePlayer, GetPlayerCardArray(PlayerHelper.MiddlePlayer));
            }
        }

        /// <summary>
        /// 用于鼠标点击牌是否被选择，用委托会调用此方法。
        /// </summary>
        /// <param name="isSelected">牌是否被选择</param>
        private void CardSelected(bool isSelected)
        {
            ArrayList cardArray = new ArrayList();
            foreach (PlayerCardInfo cardInfo in PlayerHelper.MiddlePlayer.CardCollection)
            {
                if (cardInfo.CardBase.Card.IsSelected)
                {
                    cardArray.Add(cardInfo.CardBase.CardNumber);
                }
            }
            int[] cards = new int[cardArray.Count];
            for (int i = 0; i < cardArray.Count; i++)
            {
                cards[i] = (int)cardArray[i];
            }
            //检查选择的牌是否符合规则
            RuleType rule = RuleHelper.GetRuleType(cards);
            if (GameOptions.NoOutPutCardCount != 2)
            {
                //如果和上一家出牌规则不符，或者小于等于上一家牌。就说明有问题。
                CardCompareResult result = RuleHelper.GetCardCompareResult(cards, GameOptions.LastOutPutCardArray);
                if (result != CardCompareResult.ParamOneIsBigger)
                {
                    rule = RuleType.OutOfRule;
                }
            }

            if (rule == RuleType.OutOfRule)
            {
                MainWindow.buttonMiTwo.IsEnabled = false;
                MainWindow.buttonMiTwo.Foreground = Brushes.Gray;
            }
            else
            {
                MainWindow.buttonMiTwo.IsEnabled = true;
                MainWindow.buttonMiTwo.Foreground = Brushes.Red;
            }
        }

        private void story_Completed(object sender, EventArgs e)
        {
            //重新给地主理牌
           // GameOptions.SortSpeed = 0.1;
            Storyboard story = new Storyboard();
            switch (GameOptions.CurrentHost)
            {
                case CardPlayerType.LeftPlayer:
                    SetLastThreeBackground();
                    PlayerHelper.LeftPlayer.CardCollection = PlayerHelper.SortSinglePlayerCard(PlayerHelper.LeftPlayer.CardCollection, GameOptions.CurrentHost, MainWindow.CanvasTable, story);
                    break;
                case CardPlayerType.MiddlePlayer:
                    if (!PluginManage.ConfigInfo.IsMiddleAI)
                    {
                        ShowButtonsChooseHost();
                    }
                    PlayerHelper.MiddlePlayer.CardCollection = PlayerHelper.SortSinglePlayerCard(PlayerHelper.MiddlePlayer.CardCollection, GameOptions.CurrentHost, MainWindow.CanvasTable, story);
                    break;
                case CardPlayerType.RightPlayer:
                    SetLastThreeBackground();
                    PlayerHelper.RightPlayer.CardCollection= PlayerHelper.SortSinglePlayerCard(PlayerHelper.RightPlayer.CardCollection, GameOptions.CurrentHost, MainWindow.CanvasTable, story);
                    break;
            }
            story.Completed += new EventHandler(story1_Completed);
            story.Begin();
            //AnimationFinished();
#if DEBUG
            //写入调试日志
            StringBuilder log = new StringBuilder ();
            log.Append("------------------------------------------------------------\r\n");
            switch (GameOptions.CurrentHost)
            {
                case CardPlayerType.LeftPlayer:
                    log.Append("当前地主为左侧玩家");
                   break;
                case CardPlayerType.MiddlePlayer:
                   log.Append("当前地主为中间玩家");
                    break;
                case CardPlayerType.RightPlayer:
                    log.Append("当前地主为右侧玩家");
                     break;
            }
            log.Append("，所有玩家手上的牌的情况。\r\n");
            log.Append("左侧玩家：");
            foreach (PlayerCardInfo cardinfo in PlayerHelper.LeftPlayer.CardCollection)
            {
                log.Append(cardinfo.CardBase.CardNumber);
                log.Append(",");
            }
            log.Append("\r\n中间玩家：");
            foreach (PlayerCardInfo cardinfo in PlayerHelper.MiddlePlayer.CardCollection)
            {
                log.Append(cardinfo.CardBase.CardNumber);
                log.Append(",");
            }
            log.Append("\r\n右侧玩家：");
            foreach (PlayerCardInfo cardinfo in PlayerHelper.RightPlayer.CardCollection)
            {
                log.Append(cardinfo.CardBase.CardNumber);
                log.Append(",");
            }
            log.Append("\r\n");
            File.AppendAllText("Log.txt", log.ToString());
            Thread.Sleep(500);
#endif
        }

        private void story1_Completed(object sender, EventArgs e)
        {
            GameOptions.IsNeedWaiting = false;
        }

        /// <summary>
        /// 将最后三张牌重新设置为背面。
        /// </summary>
        private void SetLastThreeBackground()
        {
            if (!PluginManage.ConfigInfo.IsShowAllCard && !PluginManage.ConfigInfo.IsMiddleAI)
            {
                CardBaseCollection[51].SetCardBackground();
                CardBaseCollection[52].SetCardBackground();
                CardBaseCollection[53].SetCardBackground();
            }
        }


        /// <summary>
        /// 随机选择玩家叫牌
        /// <returns>返回是否有人叫牌，有人叫就会True，没有人叫就为False</returns>
        /// </summary>
        private bool SetRandomHost()
        {
            if (GameOptions.UserSelectMark == ScoreType.NoChoose)
            {
                ScoreType s;
                Random ran = new Random();
                int host = ran.Next(3);  
                for (int i = 0; i < 3; i++)
                {
                    switch (host)
                    {
                        case 0:
                            s = LeftSelect();
                            if (GameOptions.CurrentMark < s)
                            {
                                GameOptions.CurrentMark = s;
                                GameOptions.CurrentHost = CardPlayerType.LeftPlayer;
                            }
                            HostSelectedIndex++;
                            break;
                        case 1:
                            if (PluginManage.ConfigInfo.IsMiddleAI)
                            {
                                s = MiddleSelect();
                                if (GameOptions.CurrentMark < s)
                                {
                                    GameOptions.CurrentMark = s;
                                    GameOptions.CurrentHost = CardPlayerType.MiddlePlayer;
                                }
                                HostSelectedIndex++;
                            }
                            else
                            {
                                ShowButtonsChooseHost();
                                switch (GameOptions.CurrentMark)
                                {
                                    case ScoreType.One:
                                        MainWindow.buttonMiOne.IsEnabled = false;
                                        MainWindow.buttonMiOne.Foreground = Brushes.Gray;
                                        break;
                                    case ScoreType.Two:
                                        MainWindow.buttonMiOne.IsEnabled = false;
                                        MainWindow.buttonMiTwo.IsEnabled = false;
                                        MainWindow.buttonMiOne.Foreground = Brushes.Gray;
                                        MainWindow.buttonMiTwo.Foreground = Brushes.Gray;
                                        break;
                                }
                                HostSelectedIndex++;
                                return false;//地主还没有选出来，需要用户来选
                            }
                           
                            break;                           
                        case 2:
                            s = RightSelect();
                            if (GameOptions.CurrentMark < s)
                            {
                                GameOptions.CurrentMark = s;
                                GameOptions.CurrentHost = CardPlayerType.RightPlayer;
                            }
                            HostSelectedIndex++;
                            break;
                    }
                    if (GameOptions.CurrentMark == ScoreType.Three)
                    {
                        return true;//已经选到了
                    }
                    host++;
                    if (host == 3)
                    {
                        host = 0;//从头开始选
                    }
                }    
            }
            else
            {
                //用户选择了，现在接着选
                if (GameOptions.CurrentMark < GameOptions.UserSelectMark)
                {
                    GameOptions.CurrentMark = GameOptions.UserSelectMark;
                    GameOptions.CurrentHost = CardPlayerType.MiddlePlayer;
                }
                if (GameOptions.CurrentMark != ScoreType.Three)
                {
                    ScoreType s;
                    switch (HostSelectedIndex)
                    {
                        case 1:
                            //只有中间玩家选过地主
                            s = RightSelect();
                            HostSelectedIndex++;
                            if (GameOptions.CurrentMark < s)
                            {
                                GameOptions.CurrentMark = s;
                                GameOptions.CurrentHost = CardPlayerType.RightPlayer;
                            }
                            if (GameOptions.CurrentMark != ScoreType.Three)
                            {
                                s = LeftSelect();
                                HostSelectedIndex++;
                                if (GameOptions.CurrentMark < s)
                                {
                                    GameOptions.CurrentMark = s;
                                    GameOptions.CurrentHost = CardPlayerType.LeftPlayer;
                                }
                            }
                            break;
                        case 2:
                            //只有右侧玩家没有选择地主
                            s = RightSelect();
                            HostSelectedIndex++;
                            if (GameOptions.CurrentMark < s)
                            {
                                GameOptions.CurrentMark = s;
                                GameOptions.CurrentHost = CardPlayerType.RightPlayer;
                            }
                            break;
                    }
                }
            }

            if (HostSelectedIndex == 3 || GameOptions.CurrentMark == ScoreType.Three)
            {
                //全部筛选结束了，可以判断，是否都是pass
                if (GameOptions.CurrentMark == ScoreType.Pass)
                {
                    GameOptions.CurrentHost = CardPlayerType.NoPlayer;
                    GameOptions.GameStatus = GameStatus.GameEnd;
                    GameOptions.IsNeedWaiting = false;
                }
                return GameOptions.CurrentMark != ScoreType.Pass;
            }
            else
            {
                return false;
            }
        }

        private ScoreType LeftSelect()
        {
            HideButtonsDealCard();
            ScoreType score  = GetScoreFromPlugin(CardPlayerType.LeftPlayer, HostSelectedMarkArray);
            HostSelectedMarkArray.Add(GetMarkFromEnum(score));
            MainWindow.labelLeft.Visibility = Visibility.Visible;
            MainWindow.labelLeft.Content = GetScoreString(score);
            return score;
        }

        private ScoreType MiddleSelect()
        {
            HideButtonsDealCard();
            ScoreType score = GetScoreFromPlugin(CardPlayerType.MiddlePlayer, HostSelectedMarkArray);
            HostSelectedMarkArray.Add(GetMarkFromEnum(score));
            MainWindow.labelMiddle.Content = GetScoreString(score);
            MainWindow.labelMiddle.Visibility = Visibility.Visible;
            return score;
        }

        private ScoreType RightSelect()
        {
            HideButtonsDealCard();
            ScoreType score = GetScoreFromPlugin(CardPlayerType.RightPlayer, HostSelectedMarkArray);
            HostSelectedMarkArray.Add(GetMarkFromEnum(score));
            MainWindow.labelRight.Visibility = Visibility.Visible;
            MainWindow.labelRight.Content = GetScoreString(score);
            return score;
        }

        private int GetMarkFromEnum(ScoreType score)
        {
            switch (score)
            {
                case ScoreType.One:
                    return 1;
                case ScoreType.Two:
                    return 2;
                case ScoreType.Three:
                    return 3;
                case ScoreType.Pass:
                    return -1;
            }
            return -1;
        }

        private string GetScoreString(ScoreType score)
        {
            switch (score)
            {
                case ScoreType.One:
                    return "1分";
                case ScoreType.Two:
                    return "2分";
                case ScoreType.Three:
                    return "3分";
                case ScoreType.Pass:
                    return "跳过";
            }
            return string.Empty;
        }


        private ScoreType GetScoreFromPlugin(CardPlayerType player, List<int> markArray)
        {
            return (ScoreType)PluginManage.Invoke(player, "GetChooseHostMark", new object[] { markArray });
        }

    }
}
