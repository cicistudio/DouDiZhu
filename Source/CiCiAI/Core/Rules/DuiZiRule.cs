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
    public class DuiZiRule: PokerBase
    {
        public override List<CombineBaseInfo> GetCombineList(List<CommClass.Poker> pokerList)
        {
            List<CombineBaseInfo> resultList = new List<CombineBaseInfo>();
            DuiZiInfo info = new DuiZiInfo();
            info.CombinePokerString =  GetAllDuiZi(pokerList);
            if(info.CombinePokerString!= string.Empty)
            {
                resultList.Add(info);
            }
            return resultList;
        }
        private string GetAllDuiZi(List<CommClass.Poker> pokerList)
        {
            List<string> resultList = new List<string>();
            foreach(CommClass.Poker p in pokerList)
            {
                if (pokerList.FindAll(i => i == p).Count >= 2 && pokerList.FindAll(i => i == p).Count != 4)
                {
                    string reuslt = CommClass.EnumToPockerChar(p);
                    reuslt = reuslt + "," + reuslt;
                    if (!resultList.Contains(reuslt))
                    {
                        resultList.Add(reuslt);
                    }
                }
            }
            StringBuilder sb = new StringBuilder();
            foreach (string s in resultList)
            {
                sb.Append(s);
                sb.Append("|");
            }
            if (sb.ToString() != string.Empty)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }

        protected List<string> GetDuiZiList(List<CommClass.Poker> pokerList)
        {
            if (GetAllDuiZi(pokerList) == string.Empty)
            {
                return new List<string>();
            }
            else
            {
                return GetAllDuiZi(pokerList).Split(new char[] { '|' }).ToList();
            }
        }
    }
}
