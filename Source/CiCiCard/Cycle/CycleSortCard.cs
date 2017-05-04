using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CiCiStudio.CardFramework;
using CiCiStudio.CardFramework.CardPlayers;
using System.Windows.Media.Animation;

namespace CiCiCard.Cycle
{
    public class CycleSortCard : CycleBase
    {
        protected override void NextStatus()
        {
            GameOptions.IsNeedWaiting = true;
           // GameOptions.SortSpeed = 0.001;
            Storyboard story = new Storyboard();
            PlayerHelper.SortAllPlayerCard(MainWindow, MainWindow.CanvasTable, story);
            story.Completed += new EventHandler(story_Completed);
            story.Begin();
        }

        private void story_Completed(object sender, EventArgs e)
        {
            AnimationFinished();
        }
    }
}
