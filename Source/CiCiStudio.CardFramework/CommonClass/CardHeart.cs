using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace CiCiStudio.CardFramework.CommonClass
{
    public class CardHeart : CardBase
    {
        public override CardType CardType
        {
            get { return CardType.Heart; }
        }

        public override void SetCard()
        {
            Brush brush = this.Card.FindResource("ImageBrushHeart" + this.CardNumber.ToString()) as Brush;
            this.Card.CardLayout.Background = brush;
        }
    }
}
