using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using AIFrameWork;
/*************************************************************
 * Copyright by cicistudio 2000-2017
 * http://chengchen.cnblogs.com
 * https://github.com/cicistudio/
 * The Current code cannot be used in the Commercial software.
 * Mail to me if you have any queries. cicistudio@qq.com
 ************************************************************/
namespace CiCiStudio.CardFramework
{
    /// <summary>
    /// Card.xaml 的交互逻辑
    /// </summary>
    public partial class Card : UserControl
    {
        public Card()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 该牌是属于哪个用户
        /// </summary>
        public CardPlayerType CardPlayType { get; set; }

        /// <summary>
        /// 用于判断改牌是否被用户选择了（鼠标点击了）。
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// 标记为是否已经是打出过的牌
        /// </summary>
        public bool IsOutPut { get; set; }

        private bool m_CanSelected = false;
        /// <summary>
        /// 用于判断牌是否可选
        /// </summary>
        public bool CanSelected { get {return m_CanSelected;} }

        /// <summary>
        /// 用于调用牌是否被选择或者没有选择的委托。
        /// </summary>
        /// <param name="isSelected">是否被选择</param>
        public delegate void CardSelectedDelegate(bool isSelected);
        public CardSelectedDelegate CardSelected { get; set; }

        /// <summary>
        /// 设置纸牌是否可以选择
        /// </summary>
        /// <param name="canSelected">true为可以选择，false为不可以选择</param>
        public void SetCardSelected(bool canSelected)
        {
            m_CanSelected = canSelected;
            Storyboard onEnterStoryboard = (Storyboard)FindResource("OnPokerMouseEnter");
            Storyboard onLeaveStoryboard = (Storyboard)FindResource("OnPokerMouseLeave");
            //将动画设置为Leave时的状态，就相当于屏蔽了动画。
            OnPokerMouseEnter_BeginStoryboard.Storyboard = canSelected ? onEnterStoryboard : onLeaveStoryboard;
        }

        /// <summary>
        /// 移动牌的方法
        /// </summary>
        /// <param name="offsetX">要移动的X偏移量</param>
        /// <param name="offsetY">要移动的Y偏移量</param>
        public void MoveCard(double offsetX, double offsetY)
        {
            //获取到前台动画对象
            Storyboard storyboard = FindResource("MoveCardStoryboard") as Storyboard;
            //X偏移量
            ((DoubleAnimation)storyboard.Children[0]).By = offsetX;
            //Y偏移量
            ((DoubleAnimation)storyboard.Children[1]).By = offsetY;
            storyboard.Begin(this, HandoffBehavior.Compose);//将新的动画追加到尾部。
        }

        /// <summary>
        /// 选中一张牌的方法
        /// </summary>
        public void SelectCard()
        {
            //确认当前没有被选中且牌可以被选中
            if (!IsSelected && CanSelected)
            {               
                switch (CardPlayType)
                {
                    case CardPlayerType.MiddlePlayer:
                        MoveCard(0, -20);
                        break;
                    //case CardPlayerType.LeftPlayer:
                    //    MoveCard(-30, 0);
                    //    break;
                    //case CardPlayerType.RightPlayer:
                    //    MoveCard(30, 0);
                    //    break;
                }
                IsSelected = true;
                CardSelected(true);
            }
        }

        /// <summary>
        /// 取消选中一张牌的方法
        /// </summary>
        public void UnSelectCard()
        {
            //确认当前已经被选中
            if (IsSelected)
            {              
                switch (CardPlayType)
                {
                    case CardPlayerType.MiddlePlayer:
                        MoveCard(0, 20);
                        break;
                    //case CardPlayerType.LeftPlayer:
                    //    MoveCard(30, 0);
                    //    break;
                    //case CardPlayerType.RightPlayer:
                    //    MoveCard(-30, 0);
                    //    break;
                }
                IsSelected = false;
                CardSelected(false);
            }
        }

        private void SelectBackground_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (CanSelected)
            {
                if (IsSelected)
                {
                    UnSelectCard();
                }
                else
                {
                    SelectCard();
                }
            }
        }

    }
}
