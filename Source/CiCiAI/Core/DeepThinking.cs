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
    class DeepThinking
    {

        static List<List<CommClass.PokerCombineType>> m_PokerCombineOrderList = new List<List<CommClass.PokerCombineType>>();


        public static DataTable GetEvaluateDT(List<CommClass.Poker> originPokerList, DataTable dt, int deep)
        {
            if (dt == null)
            {
                deep = 1;
                dt = new DataTable();
                dt.Columns.Add("ID", typeof(Int32));
                dt.Columns["ID"].AutoIncrement = true;//设置为自增列
                dt.Columns["ID"].AutoIncrementSeed = 1;
                dt.Columns["ID"].AutoIncrementStep = 1;
                dt.Columns.Add("ParentID",typeof(Int32));
                dt.Columns.Add("CombinedPokers",typeof(String));
                dt.Columns.Add("RemainPokerCombineTypes",typeof(String));
                dt.Columns.Add("Deep",typeof(Int32));
                dt.Columns.Add("Score", typeof(float));//根据当前牌计算分数

                List<CommClass.PokerCombineType> pokerCombineList = new List<CommClass.PokerCombineType>();
                foreach (string s in Enum.GetNames(typeof(CommClass.PokerCombineType)))
                {
                    pokerCombineList.Add((CommClass.PokerCombineType)Enum.Parse(typeof(CommClass.PokerCombineType), s));           
                }
                foreach(CommClass.PokerCombineType combineType in pokerCombineList)
                {
                    List<CombineBaseInfo> CombinedList = ArtificialNeural.GetCombinePokerList(originPokerList, combineType);
                    foreach (CombineBaseInfo comb in CombinedList)
                    {
                        DataRow dr = dt.NewRow();
                        dr["ParentID"] = 0;
                        dr["CombinedPokers"] = comb.CombinePokerString;
                        dr["RemainPokerCombineTypes"] = CommClass.GetRemainPokerCombineTypes(combineType);
                        dr["Deep"] = deep;
                        dr["Score"] = comb.Socre;
                        EvalutionForPokerCombineType(combineType, comb, dr);
                        dt.Rows.Add(dr);
                    }
                }             
            }
            else
            {
                DataRow[] pokerArray = dt.Select("Deep = " + (deep - 1));//选出所有上一层的有效思考结果
                foreach (DataRow dr in pokerArray)
                {
                    List<CommClass.PokerCombineType> pokerCombineList = new List<CommClass.PokerCombineType>();
                    foreach (string s in dr["RemainPokerCombineTypes"].ToString().Split(new char[] { ',' }))//剩余的没有选过的类型
                    {
                        pokerCombineList.Add((CommClass.PokerCombineType)Enum.Parse(typeof(CommClass.PokerCombineType), s));
                    }

                    foreach (CommClass.PokerCombineType combineType in pokerCombineList)
                    {
                        List<CommClass.Poker> remainPokerList = CommClass.RemovePokerFromList(originPokerList, dr["CombinedPokers"].ToString());
                        List<CombineBaseInfo> CombinedList = ArtificialNeural.GetCombinePokerList(remainPokerList, combineType);
                        foreach (CombineBaseInfo comb in CombinedList)
                        {
                            DataRow newDR = dt.NewRow();
                            newDR["ParentID"] = dr["ID"];
                            newDR["CombinedPokers"] = dr["CombinedPokers"].ToString() + "|" + comb.CombinePokerString;
                            newDR["RemainPokerCombineTypes"] = CommClass.GetRemainPokerCombineTypes(dr["RemainPokerCombineTypes"].ToString(), combineType);
                            newDR["Deep"] = deep;
                            newDR["Score"] = Convert.ToInt32(dr["Score"]) + comb.Socre;
                            EvalutionForPokerCombineType(combineType, comb, newDR);                           
                            dt.Rows.Add(newDR);
                        }
                    }
                }

            }     

            return dt;
        }

        private static void EvalutionForPokerCombineType(CommClass.PokerCombineType combineType, CombineBaseInfo comb, DataRow dr)
        {
            if (combineType == CommClass.PokerCombineType.LianDui)
            {
                //针对连对分数要特殊处理，连对每长一对，就要多加10分
                foreach (string l in comb.CombinePokerString.Split(new char[] { '|' }))
                {
                    dr["Score"] = Convert.ToInt32(dr["Score"]) + (l.Split(new char[] { ',' }).Length / 2 - 3) * 10;
                }
            }
            else if(combineType == CommClass.PokerCombineType.DuiZi)
            {
                 foreach (string l in comb.CombinePokerString.Split(new char[] { '|' }))
                 {
                     if(l == "2,2")
                     {
                         dr["Score"] = Convert.ToInt32(dr["Score"]) + 10;//有对2再加10分
                     }
                     else if(l == "A,A")
                     {
                         dr["Score"] = Convert.ToInt32(dr["Score"]) + 5;//有对A再加5分
                     }
                     dr["Score"] = Convert.ToInt32(dr["Score"]) + (int)CommClass.PockerCharToEnum(l.Split(new char[] { ',' })[0]) / 100.0;
                 }
            }
            else if(combineType == CommClass.PokerCombineType.SanDui)
            {
                foreach (string l in comb.CombinePokerString.Split(new char[] { '|' }))
                {
                    if (l == "2,2,2")
                    {
                        dr["Score"] = Convert.ToInt32(dr["Score"]) + 20;//有三张2再加20分
                    }
                    else if (l == "A,A,A")
                    {
                        dr["Score"] = Convert.ToInt32(dr["Score"]) + 10;//有三张A再加10分
                    }
                    else if (l == "K,K,K")
                    {
                        dr["Score"] = Convert.ToInt32(dr["Score"]) + 5;//有三张K再加5分
                    }
                }
            }
        }




        /// <summary>
        /// 根据当前的牌好坏进行估分
        /// </summary>
        /// <param name="pokerList">当前牌组合</param>
        /// <returns></returns>
        public static EvaluationInfo GetEvaluateSocre(List<CommClass.Poker> pokerList)
        {
            DataTable evaluateDT = null;

            for (int i = 1; i <= 5;i++ )
            {
               evaluateDT = GetEvaluateDT(pokerList, evaluateDT, i);
            }
            //加入单牌情况
            int maxDeep = Convert.ToInt32(evaluateDT.Compute("Max(Deep)", ""));
            //DataRow[] evaluateDRs = evaluateDT.Select("Deep = " + maxDeep);
            foreach (DataRow dr in evaluateDT.Rows)
            {
                List<CommClass.Poker> singlePokerList =CommClass.RemovePokerFromList(pokerList, dr["CombinedPokers"].ToString());              
                float score = 0;

                //没有单牌+15分
                if(singlePokerList.Count == 0)
                {
                    score -= 15;
                }
                else
                {
                    if (singlePokerList.Count == 1)
                    {
                        //如果只单一张，并且这张是2，A，K，Q这种大牌的话，要加分。
                        switch (singlePokerList[0])
                        {
                            case CommClass.Poker.P2:
                                score -= 5;
                                break;
                            case CommClass.Poker.A:
                                score -= 4;
                                break;
                            case CommClass.Poker.K:
                                score -= 3;
                                break;
                            case CommClass.Poker.Q:
                                score -= 2;
                                break;
                            case CommClass.Poker.J:
                                score -= 1;
                                break;
                        }

                    }
                    dr["CombinedPokers"] = dr["CombinedPokers"].ToString() + "|" + CommClass.ListToPockerString(singlePokerList);
                }

                //每单一张扣5分，单2 加1分，单A扣2 分，单K扣3分, 有大王，小王加4分
                int single = singlePokerList.Where(q => q != CommClass.Poker.P2 && q != CommClass.Poker.JKBig && q != CommClass.Poker.JKSmall).Count();
                if(single == 2)//如果只单1张牌怎不扣分，如果单两张，就扣4分
                {
                    score = 4;
                }
                else if(single ==3)//如果单3张牌，就扣8分
                {
                    score = 8;
                }
                else if (single == 4)//如果单4张牌，就扣12分
                {
                    score = 12;
                }
                else if(single>4)
                {
                    single = singlePokerList.Where(q => q != CommClass.Poker.P2 && q != CommClass.Poker.A && q != CommClass.Poker.K && q != CommClass.Poker.JKBig && q != CommClass.Poker.JKSmall).Count();
                    score = single * 5; //单4张以上，每张扣5分。

                    score += singlePokerList.Where(q => q == CommClass.Poker.A).Count() * 2;
                    score += singlePokerList.Where(q => q == CommClass.Poker.K).Count() * 3;
                }
                score = score - singlePokerList.Where(q => q == CommClass.Poker.JKBig).Count() * 5;
                score = score - singlePokerList.Where(q => q == CommClass.Poker.JKSmall).Count() * 4;
                score = score - singlePokerList.Where(q => q == CommClass.Poker.P2).Count();

                //思考的deep更少，且在少一层的情况下，就拆完了，应该给予加分奖励
                if(evaluateDT.Select("ParentID = " + dr["ID"].ToString()).Length == 0 && Convert.ToInt32(dr["deep"]) < maxDeep)
                {
                    score = score - (maxDeep -  Convert.ToInt32(dr["deep"])) * 6;//加分奖励
                }
                
                //根据单牌，加自己的分值，把相同的拆牌分距拉开。
                foreach (CommClass.Poker poker in singlePokerList)
                {
                    score = score - (float)poker / 100;
                }

                //根据 | 符号数量，微调拆分分值。
                score = score + (float)(dr["CombinedPokers"].ToString().Split(new char[] { '|' }).Length * 0.1);

                dr["Score"] = Convert.ToDouble(dr["Score"]) - score;
            }
            DataRow maxDR = evaluateDT.Select("", "Score desc")[0];
            EvaluationInfo eval = new EvaluationInfo() { PokerString = maxDR["CombinedPokers"].ToString(), Scores = Convert.ToInt32(maxDR["Score"]) };
            return eval;
        }





        ///// <summary>
        ///// 排列组合，递归遍历所有顺序。
        ///// 我的设计思想是这样的，把所有拆牌的可能性全部考虑到，然后按照不同的顺序排列，这样就有先拆和后拆的区别，那么拆牌方法不同，产生的结果也不一样。
        ///// 我们就是要遍历所有的拆牌结果，然后选择一个最优的结果。
        ///// 注意这里要处理每次拆牌后，剩余的牌拆牌情况，因为提前可以把不存在的拆除结果删掉，可以提升性能。而且因为每个顺序都可以遍历到，所以处理结果时候，无需重新对
        ///// 剩下的牌进行排列组合。
        ///// </summary>
        ///// <param name="s"></param>
        ///// <param name="from"></param>
        ///// <param name="to"></param>
        //private static void Permutation(List<CommClass.PokerCombineType> s, int from, int to)
        //{
        //    if (to <= 1)
        //        return;
        //    if (from == to)
        //    {
        //        //排序结果在这里处理
        //        m_PokerCombineOrderList.Add(s.ToList());
        //    }
        //    else
        //    {
        //        for (int i = from; i <= to; i++)
        //        {
        //            swap(s, i, from);
        //            Permutation(s, from + 1, to);
        //            swap(s, from, i);
        //        }
        //    }
        //}

        //private static void swap(List<CommClass.PokerCombineType> s, int i, int j)
        //{
        //    CommClass.PokerCombineType temp = s[i];
        //    s[i] = s[j];
        //    s[j] = temp;
        //}
    }
}
