using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace CiCiStudio.CardFramework.CommonClass
{
    public class CardSpader: CardBase
    {
        public override CardType CardType
        {
            get { return CardType.Spader; }
        }

        public override void SetCard()
        {
            Brush brush = this.Card.FindResource("ImageBrushSpader" + this.CardNumber.ToString()) as Brush;
            this.Card.CardLayout.Background = brush;
        }
    }
}
