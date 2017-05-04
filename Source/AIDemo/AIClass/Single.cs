using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIDemo.AIClass
{
    public class Single : AIBase
    {
        public override int[] GetCardByReflect(OutPutCardInfo info)
        {
            //对于单张牌来说，即使是队友打的，也有必要插上一脚。重写这个类。
            if (info.CardArray[0] > 10 && CheckIsPartern(info))
            {
                //如果是10以上的牌，就不打了。
                return null;
            }
            return GetOutPutCard(info);     
        }

        public override int[] GetOutPutCard(OutPutCardInfo info)
        {
            List<int> singleKind = base.GetSingleKindCollection(AIOptions.CurrentCardArray);//获得当前手中牌的所有单牌。
            var query = from int c in singleKind
                    where c > info.CardArray[0]
                    orderby c
                    select c;
            if (query.Count() > 0)
            {
                return new int[] { query.First() };
            }
            else
            {
                //拆牌来打。
                if (info.CardArray[0] > 11)
                {
                    var q = from int c in AIOptions.CurrentCardArray
                            where c > info.CardArray[0]
                            orderby c
                            select c;
                    if (q.Count() > 0)
                    {
                        return new int[] { q.First() };
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
