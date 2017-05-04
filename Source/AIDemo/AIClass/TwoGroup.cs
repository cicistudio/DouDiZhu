using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIDemo.AIClass
{
    public class TwoGroup : AIBase
    {
        //对子的情况也有必要插一脚
        public override int[] GetCardByReflect(OutPutCardInfo info)
        {
            if (info.CardArray.Max() > 10 && CheckIsPartern(info))
            {
                return null;//10以上的牌就不打了
            }
            return base.GetCardByReflect(info);
        }

        public override int[] GetOutPutCard(OutPutCardInfo info)
        {
            List<int> twoKinds = base.GetKindCollection(AIOptions.CurrentCardArray, 2);//从自己的牌中找出所有成对的情况
            var query = from c in twoKinds
                        where c > info.CardArray[0]//找出自己能大出对方出的牌，并且排序
                        orderby c
                        select c;
            if (query.Count() > 0)
            {
                return new int[] { query.First(),query.First() };//此程序为DEMO程序，策略是有牌就打。
            }
            else
            {
                return null;
            }
        }
    }
}
