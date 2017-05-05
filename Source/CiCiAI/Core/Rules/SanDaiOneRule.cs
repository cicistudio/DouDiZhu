using CiCiAI.Core.Combine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*************************************************************
 * Copyright by cicistudio 2000-2017
 * http://chengchen.cnblogs.com
 * https://github.com/cicistudio/
 * The Current code cannot be used in the Commercial software.
 * Mail to me if you have any queries. cicistudio@qq.com
 ************************************************************/
namespace CiCiAI.Core.Rules
{
    public class SanDaiOneRule : SanDuiRule
    {
        public override List<CombineBaseInfo> GetCombineList(List<CommClass.Poker> pokerList)
        {
            List<CombineBaseInfo> comblieList = base.GetCombineList(pokerList);

            return comblieList;
        }
    }
}
