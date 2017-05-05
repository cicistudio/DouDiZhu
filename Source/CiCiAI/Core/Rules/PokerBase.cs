using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CiCiAI.Core.Combine;
/*************************************************************
 * Copyright by cicistudio 2000-2017
 * http://chengchen.cnblogs.com
 * https://github.com/cicistudio/
 * The Current code cannot be used in the Commercial software.
 * Mail to me if you have any queries. cicistudio@qq.com
 ************************************************************/
namespace CiCiAI.Core.Rules
{
    public abstract class PokerBase
    {
        public abstract List<CombineBaseInfo> GetCombineList(List<CommClass.Poker> pokerList);

        /// <summary>
        /// 从指定的牌中获取连顺
        /// </summary>
        /// <param name="pokerList">指定的牌</param>
        /// <param name="limitShun">几个算连顺，比如对子三个就算连顺，单只需要五个</param>
        /// <returns></returns>
        protected virtual List<string> GetAllShunZi(List<CommClass.Poker> pokerList, int limitShun)
        {

            pokerList.Sort();
            //一副牌极端在地主的情况下最多只会存在4个顺子的情况，因为（4*5 = 20）但这种可能性几乎为0。所以一般只要拆分三次就可以了。（三顺子的可能性也是非常小的）
            List<string> finalShunZiList = new List<string>();
            List<string> firstStepShunZiList = InitShunZi(pokerList, limitShun);


            foreach(string firstS in firstStepShunZiList)
            {
                List<string> secondStepShunZiLis = InitShunZi(CommClass.RemovePokerFromList(pokerList, firstS),limitShun);
                if(secondStepShunZiLis.Count == 0)
                {
                    finalShunZiList.Add(firstS);
                }
                else
                {
                    foreach(string secondS in secondStepShunZiLis)
                    {
                        List<string> thirdStepShunZiLis = InitShunZi(CommClass.RemovePokerFromList(CommClass.RemovePokerFromList(pokerList, firstS), secondS),limitShun);
                        if(thirdStepShunZiLis.Count ==0)
                        {
                            finalShunZiList.Add(firstS + "|" + secondS);
                        }
                        else
                        {
                            foreach(string thirdS in thirdStepShunZiLis)
                            {
                                finalShunZiList.Add(firstS + "|" + secondS + "|" + thirdS);
                            }
                        }
                    }
                }
            }
            //这种算法会导致重复数据，所以为了减少以后的深度打分计算，提升性能，这里把重复数据删掉。     
            return CommClass.RemoveDuplicatePokers(finalShunZiList);
        }

       

        private List<string> InitShunZi(List<CommClass.Poker> pokerList, int limitShun)
        {
            List<string> resultList = new List<string>();
            List<CommClass.Poker> newPokerList = pokerList.FindAll(i => i != CommClass.Poker.JKBig && i != CommClass.Poker.JKSmall && i != CommClass.Poker.P2);//去除大小王和2            
            List<CommClass.Poker> newPokerDistinctList = newPokerList.Distinct().ToList();
            newPokerDistinctList.Sort();
            foreach (CommClass.Poker p in newPokerDistinctList)
            {
                for (int totalCount = limitShun; totalCount <= 12; totalCount++)//多重遍历，因为最多可能有12连顺一起。
                {
                    string resultString = GetOneShunZi(totalCount, p, newPokerList,limitShun);
                    if(resultString != string.Empty)
                    {
                        resultList.Add(resultString);
                    }       
                }
            }
            return resultList.Distinct().ToList();
        }

        private string GetOneShunZi(int totalCount, CommClass.Poker currentPoker, List<CommClass.Poker> newPokerList, int limitShun)
        {
            int count = 1;
            List<int> foundPokerList = new List<int>();
            for (int i = 1; i <= 12; i++)//最多只有可能12连顺
            {
                int findPokerIndex = newPokerList.FindIndex(q => q == (CommClass.Poker)((int)currentPoker + i));
                foundPokerList.Add(findPokerIndex);
                if (findPokerIndex > 0)
                {
                    count++;
                    if(count == totalCount)//如果顺子满足总长度，那么就返回结果。
                    {
                        return CommClass.GetLianShunString(currentPoker, count);
                    }
                }
                else
                {
                    if(count>=limitShun)//如果有顺子超过limitShun个就返回结果。
                    {
                        return CommClass.GetLianShunString(currentPoker, count);
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }

            if (count >= limitShun)//如果有顺子超过5个就返回结果。
            {
                return CommClass.GetLianShunString(currentPoker, count);
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
