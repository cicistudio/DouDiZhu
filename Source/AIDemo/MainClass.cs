using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIFrameWork;
using System.Windows.Forms;

namespace AIDemo
{
    public class MainClass : IBase
    {
        #region 实现接口

        public string GetPlayerName()
        {
            return "DEMO";
        }

        public void About()
        {
            MessageBox.Show("AI测试程序，只提供简单的出牌逻辑供开发者参考使用。", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void SetCardCollection(int[] cardArray)
        {
            AIOptions.CurrentCardArray.Clear();
            AddToCurrentCardArray(cardArray);
        }

        public void SetHostLastThreeCard(int [] cardArray)
        {
            if (AIOptions.IsHostMySelf)
            {
                //如果自己是地主就加到自己的牌中去。
                AddToCurrentCardArray(cardArray);
            }
        }

        public void SetHost(CardPlayerType hostPlay)
        {
            AIOptions.CurrentHost = hostPlay;
        }

        public void SetPlayerLocation(CardPlayerType playType)
        {
            AIOptions.CurrentLocation = playType;
        }

        public ScoreType GetChooseHostMark(List<int> markArray)
        {
            Random ran = new Random();
            int score = ran.Next(4);
            
            if (markArray.Count>0 && score <= markArray.Max())
            {
                return ScoreType.Pass;
            }
            switch (score)
            {
                case 0:
                   return ScoreType.One;
                case 1:
                    return ScoreType.Two;
                case 2:
                    return ScoreType.Three;
                case 3:
                    return ScoreType.Pass;
            }
            return ScoreType.Three;//测试AI：如果自己有叫牌机会，永远都叫牌。
        }

        public void NewCycle()
        {
            //新游戏开始
            AIHelper.ClearGame();
        }

        public void SetOutPutCard(CardPlayerType player, int [] cardOutPutArray, int cycleIndex)
        {
            if (cardOutPutArray != null && cardOutPutArray.Length != 0)
            {
                OutPutCardInfo info = new OutPutCardInfo()
                {
                    CardArray = cardOutPutArray,
                    Player = player,
                };
                AIOptions.OutPutCardStackInOneCycle.Push(info);

                if (player == AIOptions.CurrentHost)
                {
                    //如果是地主就记录它出牌。
                    foreach (int i in cardOutPutArray)
                    {
                        AIOptions.HostOutPutCardArray.Add(i);
                    }
                }
            }
        }

        public int [] GetOutPutCard(bool isCycleFirst)
        {
           return AIHelper.GetOutPutCard(isCycleFirst);
        }
        #endregion

        private void AddToCurrentCardArray(int[] cardArray)
        {
            foreach (int i in cardArray)
            {
                AIOptions.CurrentCardArray.Add(i);
            }
        }
    }
}
