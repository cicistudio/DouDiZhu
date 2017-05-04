using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace CiCiStudio.CardFramework.CommonClass
{
    public class CardDiamond:CardBase
    {
        public override CardType CardType
        {
            get { return CardType.Diamond; }
        }

        public override void SetCard()
        {
            Brush brush = this.Card.FindResource("ImageBrushDiamond" + this.CardNumber.ToString()) as Brush;
            this.Card.CardLayout.Background = brush;
        }
    
    }
}
