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
    public class FeiJiRule: SanDuiRule
    {
        public override List<CombineBaseInfo> GetCombineList(List<CommClass.Poker> pokerList)
        {
            List<string> allSanDui = base.GetAllSanDui(pokerList);
            List<CommClass.Poker> sanPokerList = new List<CommClass.Poker>();
            foreach (string dui in allSanDui)
            {
                //取第一个组成单牌，再来判断是否是顺子
                sanPokerList.Add(CommClass.PockerCharToEnum(dui.Split(new char[] { ',' })[0]));
            }
            List<string> allShunList = this.GetAllShunZi(sanPokerList, 2);
            List<CombineBaseInfo> resultList = new List<CombineBaseInfo>();
            foreach (string shun in allShunList)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string s1 in shun.Split(new char[] { '|' }))
                {
                    foreach (string s2 in s1.Split(new char[] { ',' }))
                    {
                        sb.Append(s2);
                        sb.Append(",");
                        sb.Append(s2);
                        sb.Append(",");
                        sb.Append(s2);
                        sb.Append(",");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append("|");
                }
                sb.Remove(sb.Length - 1, 1);

                FeiJiInfo info = new FeiJiInfo();
                info.CombinePokerString = sb.ToString();
                resultList.Add(info);
            }

            return resultList;
        }
    }
}
