using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CiCiStudio.CardFramework;
using CiCiStudio.CardFramework.CardPlayers;
using CiCiCard.ConfigClass;

namespace CiCiCard.Cycle
{
    public class CycleDealCard : CycleBase
    {
        protected override void NextStatus()
        {
            for (int i = 0; i < 54; i++)
            {
                CardAnimation animation = new CardAnimation(MainWindow, CardBaseCollection[i].Card);
                animation.CardIndex = i;
                if (i == 53)
                {
                    animation.DelaCardFinished = new CardAnimation.DelaCardFinishedDelegate(AnimationFinished);
                }
                PlayerCardInfo player = new PlayerCardInfo();
                player.CardBase = CardBaseCollection[i];
                if (PluginManage.ConfigInfo.IsMiddleAI && PluginManage.ConfigInfo.IsShowAllCard)
                {
                    player.CardBase.SetCard();//如果满足上面两个条件就显示所有的牌。
                }
                PlayerHelper.AddCardToPlayer(i, player);
                animation.MoveCard(player.Location.X, player.Location.Y, TimeSpan.FromSeconds(GameOptions.DealSpeed * i));
            } 
        }
    }
}
