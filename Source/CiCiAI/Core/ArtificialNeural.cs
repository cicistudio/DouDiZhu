using CiCiAI.Core.Combine;
using CiCiAI.Core.Rules;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
/*************************************************************
 * Copyright by cicistudio 2000-2017
 * http://chengchen.cnblogs.com
 * https://github.com/cicistudio/
 * The Current code cannot be used in the Commercial software.
 * Mail to me if you have any queries. cicistudio@qq.com
 ************************************************************/

namespace CiCiAI.Core
{
    public class ArtificialNeural
    {
        public static List<CombineBaseInfo> GetCombinePokerList(List<CommClass.Poker> pokerList, CommClass.PokerCombineType comType)
        {
            PokerBase pBase = null;
            switch(comType)
            {
                case CommClass.PokerCombineType.DuiZi:
                    pBase = new DuiZiRule();
                    break;
                case CommClass.PokerCombineType.FeiJi:
                    pBase = new FeiJiRule();
                    break;
                case CommClass.PokerCombineType.LianDui:
                    pBase = new  LianDuiRule();
                    break;
                case CommClass.PokerCombineType.SanDui:
                    pBase = new SanDuiRule();
                    break;
                case CommClass.PokerCombineType.ShunZi:
                    pBase = new ShunZiRule();
                    break;
                case CommClass.PokerCombineType.ZhaDan:
                    pBase = new ZhaDanRule();
                    break;
            }
            return pBase.GetCombineList(pokerList);
        }
    }
}
