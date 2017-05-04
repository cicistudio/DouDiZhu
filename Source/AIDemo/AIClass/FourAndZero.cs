using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIDemo.AIClass
{
    public class FourAndZero:AIBase
    {
        public override int[] GetOutPutCard(OutPutCardInfo info)
        {
            //List<int> fourKinds = base.GetKindCollection(AIOptions.CurrentCardArray, 4);
            //var query = from c in fourKinds
            //            where c > info.CardArray[0]
            //            orderby c
            //            select c;
            //if (query.Count() > 0)
            //{
            //    return new int[] { query.First(), query.First(), query.First(), query.First() };
            //}
            //else
            //{
            //    return null;
            //}
            return null;
        }
    }
}
