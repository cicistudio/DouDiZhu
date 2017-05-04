using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace CiCiStudio.CardFramework.CommonClass
{
    public class CardSmallJoker: CardBase
    {
        public CardSmallJoker()
        {
            Card.Name = "SmallJoker";
            base.CardNumber = 16;
        }

        public override CardType CardType
        {
            get { return CardType.SmallJoker; }
        }

        public override void SetCard()
        {
            Brush brush = this.Card.FindResource("ImageBrushSmallJoker") as Brush;
            this.Card.CardLayout.Background = brush;
        }
    }
}
