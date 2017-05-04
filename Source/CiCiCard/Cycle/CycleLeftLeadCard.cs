using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CiCiCard.ConfigClass;
using AIFrameWork;
using CiCiStudio.CardFramework.CommonClass;
using CiCiStudio.CardFramework.CardPlayers;
using System.Windows.Media.Animation;
using CiCiStudio.CardFramework;
using System.Windows;

namespace CiCiCard.Cycle
{
    public class CycleLeftLeadCard :CycleBase
    {
        protected override void NextStatus()
        {
            HideButtonsDealCard();
            SetMiddlePlayerCardSelected(false);
            int[] cardArray = (int[])PluginManage.Invoke(CardPlayerType.LeftPlayer, "GetOutPutCard", new object[] { GameOptions.NoOutPutCardCount == 2 });

#if DEBUG
            GetOutPutCardFromAILog(CardPlayerType.LeftPlayer, cardArray);
#endif 
            if (GameOptions.NoOutPutCardCount == 2 && (cardArray == null || cardArray.Length == 0))
            {
                throw new Exception("左侧AI插件出现了问题,它必须至少要出一张牌。");
            }

            if (cardArray != null && cardArray.Length > 0)
            {
                RuleType rule = RuleHelper.GetRuleType(cardArray);
                if (rule == RuleType.OutOfRule)
                {
                    throw new Exception("左侧AI插件出现了问题，他出的牌不符合规范！");
                }
                else if (rule == RuleType.FourAndZero || rule == RuleType.JokersBomb)
                {
                    GameOptions.BombCount++;//如果有炸弹出现，就增加统计。
                }

                if (GameOptions.NoOutPutCardCount != 2)
                {
                    //如果和上一家出牌规则不符，或者小于等于上一家牌。就说明有问题。
                    CardCompareResult result = RuleHelper.GetCardCompareResult(cardArray, GameOptions.LastOutPutCardArray);
                    if (result != CardCompareResult.ParamOneIsBigger)
                    {
                        throw new Exception("左侧AI插件出现了问题，他出的牌小于上家出的牌");
                    }
                }

                GameOptions.LastOutPutCardArray = cardArray;
                //GameOptions.NoOutPutCardCount = 0;//恢复为0
                //出牌以及动画
                List<CardBase> outPutCardCollection = new List<CardBase>();
                foreach (int n in cardArray)
                {
                    var q = from c in PlayerHelper.LeftPlayer.CardCollection
                            where c.CardBase.CardNumber == n
                            select c;
                    if (q.Count() == 0)
                    {
                        throw new Exception("左侧AI插件出现了问题，他想出的牌" + n + "没有找到，游戏结束！");
                    }
                    outPutCardCollection.Add(q.First().CardBase);
                    q.First().CardBase.Card.IsOutPut = true;
                    PlayerHelper.LeftPlayer.CardCollection.Remove(q.First());
                }
                SetCardCollection(CardPlayerType.LeftPlayer, GetPlayerCardArray(PlayerHelper.LeftPlayer));

                //重新排序
                Storyboard story = new Storyboard();
               // GameOptions.SortSpeed = 0.001;
                PlayerHelper.SortSinglePlayerCard(PlayerHelper.LeftPlayer.CardCollection, CardPlayerType.LeftPlayer, MainWindow.CanvasTable, story);
                story.Begin();

                //出牌动画
                Storyboard storyOutPut = new Storyboard();
                int i = 0;
                foreach (CardBase card in outPutCardCollection)
                {
                    CardAnimation animation = new CardAnimation(MainWindow, card.Card);
                    MainWindow.CanvasTable.Children.Remove(card.Card);
                    MainWindow.CanvasTable.Children.Add(card.Card);//换顺序
                    card.SetCard();
                    card.Card.SetCardSelected(false);
                    animation.MoveCard(GameOptions.LeftPlayerFirstLocation.X + GameOptions.CardSpace + card.Card.Width, GameOptions.LeftPlayerFirstLocation.Y + i * GameOptions.CardSpace, TimeSpan.FromSeconds(i * 0.1), storyOutPut);
                    i++;
                }
                storyOutPut.Completed += new EventHandler(storyOutPut_Completed);
                storyOutPut.Begin();
                //出牌后，检查游戏是否结束
                CheckGameEnd(CardPlayerType.LeftPlayer);
                MainWindow.labelLeft.Visibility = Visibility.Hidden;
            }
            else
            {
                //显示Pass出牌信息
                MainWindow.labelLeft.Visibility = Visibility.Visible;
                MainWindow.labelLeft.Content = "跳过";
               // GameOptions.NoOutPutCardCount ++;//恢复为0
            }
            SetOutPutCardToEveryOne(CardPlayerType.LeftPlayer, cardArray);
            SetPlayerIndex();
            if (cardArray == null || cardArray.Length == 0)
            {
                AnimationFinished();//如果cardArray为Null就不会触发storyOutPut_Completed事件
            }
        }

        private void storyOutPut_Completed(object sender, EventArgs e)
        {
            AnimationFinished();
        }
    }
}
