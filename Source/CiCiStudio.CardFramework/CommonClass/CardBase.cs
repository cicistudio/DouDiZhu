using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace CiCiStudio.CardFramework.CommonClass
{
    public abstract class CardBase
    {
        private Card m_Card = new Card();

        /// <summary>
        /// 存储具体的某张扑克控件
        /// </summary>
        public Card Card
        {
            get { return m_Card; }
            set { m_Card = value; }
        }

        /// <summary>
        /// 用于设置牌的点数，注意：A虽然是1，但是比K还要大，所以设置为14，同理2的大小设置为15，小王为16，大王为17，用于排序操作
        /// </summary>
        public int CardNumber { get; set; }
        
        public CardBase()
        {
            //初始化的时候永远都是背面朝上
            Brush brush = m_Card.FindResource("ImageBrushFaceDown") as Brush;
            m_Card.CardLayout.Background = brush;//设置Border(CardLayout)的背景为brush，这样才会出现圆弧边角。
            m_Card.SetCardSelected(false);//默认所有的卡片都不可以被选择。
            //转换为TranslateTransform为动画做准备
            m_Card.RenderTransform = new TranslateTransform();
        }

        public abstract CardType CardType { get; }
        
        /// <summary>
        /// 根据牌的点数，设置牌的正面图案
        /// </summary>
        public abstract void SetCard();

        public void SetCardBackground()
        {
            Brush brush = m_Card.FindResource("ImageBrushFaceDown") as Brush;
            m_Card.CardLayout.Background = brush;
        }
    }
}
