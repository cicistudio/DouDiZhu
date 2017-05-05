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
    public class SanDuiRule: PokerBase
    {
        public override List<CombineBaseInfo> GetCombineList(List<CommClass.Poker> pokerList)
        {
            List<string> sanDuiList = GetAllSanDui(pokerList);
            if(sanDuiList.Count == 0)
            {
                return new List<CombineBaseInfo>();
            }

            List<CombineBaseInfo> resultList = new List<CombineBaseInfo>();
            SanDuiInfo info = new SanDuiInfo();
            foreach (string s in sanDuiList)
            {
                info.CombinePokerString = info.CombinePokerString + s + "|";             
            }
            info.CombinePokerString = info.CombinePokerString.TrimEnd(new char[] { '|' });
            resultList.Add(info);
            return resultList;
        }
        protected List<string> GetAllSanDui(List<CommClass.Poker> pokerList)
        {
            List<string> resultList = new List<string>();
            foreach (CommClass.Poker p in pokerList)
            {
                //if (pokerList.FindAll(i => i == p).Count >= 3 && pokerList.FindAll(i => i == p).Count != 4)
                if (pokerList.FindAll(i => i == p).Count >= 3)
                {
                    string reuslt = CommClass.EnumToPockerChar(p);
                    reuslt = reuslt + "," + reuslt + "," + reuslt;
                    if (!resultList.Contains(reuslt))
                    {
                        resultList.Add(reuslt);
                    }
                }
            }
            return resultList;
        }
    }
}
