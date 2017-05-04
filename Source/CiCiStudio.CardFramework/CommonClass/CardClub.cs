using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace CiCiStudio.CardFramework.CommonClass
{
    public class CardClub : CardBase
    {
        public override CardType CardType
        {
            get
            {
                return CardType.Club;
            }
        }

        public override void SetCard()
        {
            Brush brush = this.Card.FindResource("ImageBrushClub" + this.CardNumber.ToString()) as Brush;
            this.Card.CardLayout.Background = brush;
        }
    }
}
