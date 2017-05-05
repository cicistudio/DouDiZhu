using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class CardHelper
    {
        /// <summary>
        /// 初始化牌，并洗牌，通过此函数可以获得一副随机的扑克牌。
        /// </summary>
        /// <returns></returns>
        public static List<CardBase> GetCardCollection()
        {
            List<CardBase> cardCollection = new List<CardBase>();

            for (int i = 0; i < 4; i++)
            {
                for (int j = 3; j <= 15; j++)//从3到 15 A(A算作14) 2 算作15
                {
                    CardBase card = null;
                    switch (i)
                    {
                        case 0:
                            card = new CardClub();
                            card.Card.Name = "CardClub" + j.ToString();
                            break;
                        case 1:
                            card = new CardDiamond();
                            card.Card.Name = "CardDiamond" + j.ToString();
                            break;
                        case 2:
                            card = new CardHeart();
                            card.Card.Name = "CardHeart" + j.ToString();
                            break;
                        case 3:
                            card = new CardSpader();
                            card.Card.Name = "CardSpader" + j.ToString();
                            break;
                    }
                    card.CardNumber = j;
                    cardCollection.Add(card);
                }
            }
            //最后来添加大小王
            cardCollection.Add(new CardBigJoker());
            cardCollection.Add(new CardSmallJoker());
            return SetCardRnd(cardCollection);
        }

        /// <summary>
        /// 将生成的扑克随机打乱(洗牌操作)
        /// </summary>
        /// <param name="oldCardCollection">已经生成的扑克牌集合</param>
        /// <returns></returns>
        private static List<CardBase> SetCardRnd(List<CardBase> oldCardCollection)
        {
            Random ran = new Random();
            List<CardBase> cardCollection = new List<CardBase>();
            for (int i = 53; i >= 0; i--)
            {
                int cardIndex = ran.Next(0, i + 1);//随机数可以取到下限值，但是不能取到上限值。
                cardCollection.Add(oldCardCollection[cardIndex]);
                oldCardCollection.RemoveAt(cardIndex);
            }
            return cardCollection;
        }
    }
}
