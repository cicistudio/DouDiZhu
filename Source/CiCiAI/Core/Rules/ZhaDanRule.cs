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
    public class ZhaDanRule:PokerBase
    {
        public override List<CombineBaseInfo> GetCombineList(List<CommClass.Poker> pokerList)
        {

            List<CombineBaseInfo> resultList = new List<CombineBaseInfo>();
            ZhaDanInfo info = new ZhaDanInfo();
            info.CombinePokerString = GetAllZhaDan(pokerList);
            if(info.CombinePokerString!= string.Empty)
            {
                resultList.Add(info);
            }
            return resultList;
        }
        /// <summary>
        /// 获取当前牌中的所有炸弹, 为了简化使用字符串作为每组顺子的表达方式。
        /// </summary>
        /// <param name="pokerList">当前牌组合</param>
        /// <returns></returns>
        private string GetAllZhaDan(List<CommClass.Poker> pokerList)
        {
            List<CommClass.Poker> resultList = new List<CommClass.Poker>();
            bool hasJKBig = false, hasJKSmall = false;
            foreach (CommClass.Poker p in pokerList)
            {
                if (pokerList.Where(i => i == p).Count() == 4)
                {
                    if (!resultList.Contains(p))
                    {
                        resultList.Add(p);
                    }
                }
                if (p == CommClass.Poker.JKBig)
                {
                    hasJKBig = true;
                }
                if (p == CommClass.Poker.JKSmall)
                {
                    hasJKSmall = true;
                }
            }

            List<string> resultStringList = new List<string>();
            if (hasJKBig && hasJKSmall)//是否有王炸
            {
                resultStringList.Add("JKSmall,JKBig");
            }

            foreach (CommClass.Poker p in resultList)
            {
                 resultStringList.Add(CommClass.EnumToPockerChar(p) + "," + CommClass.EnumToPockerChar(p) + "," + CommClass.EnumToPockerChar(p) + "," + CommClass.EnumToPockerChar(p));
            }

            StringBuilder sb = new StringBuilder();
            foreach(string s in resultStringList)
            {
                sb.Append(s);
                sb.Append("|");
            }
            if(sb.ToString()!= string.Empty)
            {
                sb.Remove(sb.Length - 1, 1);
            }           
            return sb.ToString();
        }
    }
}
