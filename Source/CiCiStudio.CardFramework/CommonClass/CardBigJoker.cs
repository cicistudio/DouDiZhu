using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace CiCiStudio.CardFramework.CommonClass
{
    public class CardBigJoker : CardBase
    {
        public CardBigJoker()
        {
            base.Card.Name = "BigJoker";
            base.CardNumber = 17;
        }

        public override CardType CardType
        {
            get { return CardType.BigJoker; }
        }

        public override void SetCard()
        {
            Brush brush = this.Card.FindResource("ImageBrushBigJoker") as Brush;
            //设置Border的背景为brush，这样才会出现圆弧边角。
            this.Card.CardLayout.Background = brush;
        }
    }
}
