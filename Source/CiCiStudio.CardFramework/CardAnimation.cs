using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Animation;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using CiCiStudio.CardFramework.CommonClass;
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
    /// 扑克动画相关类
    /// </summary>
    public class CardAnimation
    {
        public delegate void DelaCardFinishedDelegate();
        public DelaCardFinishedDelegate DelaCardFinished { get; set; }

        /// <summary>
        /// 每张扑克牌的控件名称
        /// </summary>
        DependencyObject m_Card;
        /// <summary>
        /// 动画将在主窗体上运行
        /// </summary>
        FrameworkElement m_MainWindows;

        DependencyProperty[] m_PropertyChainXArray;
        DependencyProperty[] m_PropertyChainYArray;

        /// <summary>
        /// 由于动画中会向x轴或向y轴移动，因此会注册两次Complete事件，应该只注册最后一个动画完成即可。
        /// </summary>
        private bool m_IsNeedAddCompleteEvent = false; 

        public int CardIndex { get; set; }

        public CardAnimation(DependencyObject card)
        {
            Init(card);
        }

        public CardAnimation(FrameworkElement win, DependencyObject card)
        {
            m_MainWindows = win;
            Init(card);
        }

        private void Init(DependencyObject card)
        {
            m_Card = card;
            m_PropertyChainXArray = new DependencyProperty[]
            {
                UserControl.RenderTransformProperty,
                TranslateTransform.XProperty
               // ScaleTransform.ScaleXProperty
            };
            m_PropertyChainYArray = new DependencyProperty[]
            {
                UserControl.RenderTransformProperty,
                TranslateTransform.YProperty
                // ScaleTransform.ScaleYProperty
            };
        }

        public void MoveCard(double toX, double toY, TimeSpan beginTime)
        {
            //将动画分为每一小段，那么当此动画完毕后，可触发Completed事件，在Complete事件中，将扑克牌控件的层次置顶。
            Storyboard story = new Storyboard();
            m_IsNeedAddCompleteEvent = false;
            story.Children.Add(GetMoveAnimation(toX,beginTime, m_PropertyChainXArray));
            m_IsNeedAddCompleteEvent = true;
            story.Children.Add(GetMoveAnimation(toY,beginTime, m_PropertyChainYArray));
            story.Begin(m_MainWindows);
        }

        public void MoveCard(double toX, double toY, TimeSpan beginTime, Storyboard story)
        {
            //所有动画纸生成一个Stroy对象
            //Storyboard story = new Storyboard();
            m_IsNeedAddCompleteEvent = false;
            story.Children.Add(GetMoveAnimation(toX, beginTime, m_PropertyChainXArray));
            m_IsNeedAddCompleteEvent = true;
            story.Children.Add(GetMoveAnimation(toY, beginTime, m_PropertyChainYArray));
            //story.Begin(m_MainWindows);
        }

        private Timeline GetMoveAnimation(double to, TimeSpan beginTime, DependencyProperty[] propertyChain)
        {
            return GetMoveAnimation(to, beginTime, TimeSpan.FromSeconds(GameOptions.DealSpeed), propertyChain);
        }

        private Timeline GetMoveAnimation(double to, TimeSpan beginTime, TimeSpan spendTime, DependencyProperty[] propertyChain)
        {
            DoubleAnimation myAnimation = new DoubleAnimation()
            {
                //From = form,
                To = to,
                Duration = new Duration(spendTime),
                BeginTime = beginTime
            };

            Storyboard.SetTarget(myAnimation, m_Card);
            //Storyboard.SetTargetName(myAnimation, m_CardName);
            Storyboard.SetTargetProperty(myAnimation, new PropertyPath("(0).(1)", propertyChain));
            if (m_IsNeedAddCompleteEvent)
            {
                myAnimation.Completed += new EventHandler(myAnimation_Completed);
            }
            return myAnimation;
        }

        private void myAnimation_Completed(object sender, EventArgs e)
        {
            //每发完一张牌，就把这张牌置顶显示，这样在动画中，就不会感觉是从底层抽牌出来了。
            Canvas.SetZIndex((UserControl)m_Card, this.CardIndex);
            //DelaCardFinished();//每次牌移动后，都会触发此委托。
            if (this.CardIndex == 53)
            {
                this.CardIndex = -1;
                DelaCardFinished();
            }
        }
    }
}
