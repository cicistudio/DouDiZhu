using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using AIFrameWork.RuleClass;
using AIFrameWork.CardCompare;
/*************************************************************
 * Copyright by cicistudio 2000-2017
 * http://chengchen.cnblogs.com
 * https://github.com/cicistudio/
 * The Current code cannot be used in the Commercial software.
 * Mail to me if you have any queries. cicistudio@qq.com
 ************************************************************/
namespace AIFrameWork
{
    public class RuleHelper
    {
        public static RuleType GetRuleType(int[] cardArray)
        {
            Type[] typeArray = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type type in typeArray)
            {
                if (type.Namespace == "AIFrameWork.RuleClass" && type.Name !="RuleBase" && !type.Name.Contains("DisplayClass"))
                {
                    object obj = Assembly.GetExecutingAssembly().CreateInstance(type.FullName);
                    MethodInfo method = type.GetMethod("GetRuleType");
                    if(method == null)
                    {
                        return RuleType.OutOfRule;
                    }
                    RuleType outputType = (RuleType)method.Invoke(obj, new object[] { cardArray });
                    if (outputType != RuleType.OutOfRule)
                    {
                        return outputType;//只要符合出牌规则，就返回出牌属于哪个规则。
                    }
                }
            }
            return RuleType.OutOfRule;
        }

        /// <summary>
        /// 返回牌数组对比结果
        /// </summary>
        /// <param name="cardArray1">牌数组1</param>
        /// <param name="cardArray2">牌数组2</param>
        /// <returns>返回对比结果</returns>
        public static CardCompareResult GetCardCompareResult(int[] cardArray1, int[] cardArray2)
        {
            RuleType rule1 = GetRuleType(cardArray1);
            RuleType rule2 = GetRuleType(cardArray2);
            if (rule1 == RuleType.OutOfRule && rule2!= RuleType.OutOfRule)
            {
                return CardCompareResult.ParamOneOutOfRule;
            }
            else if (rule1 != RuleType.OutOfRule && rule2 == RuleType.OutOfRule)
            {
                return CardCompareResult.ParamTwoOutOfRule;
            }
            else if(rule1 == RuleType.OutOfRule && rule2 == RuleType.OutOfRule)
            {
                return CardCompareResult.ParamAllOutOfRule;
            }

            if (rule1 != RuleType.FourAndZero && rule1 != RuleType.JokersBomb && rule2 != RuleType.FourAndZero && rule2 != RuleType.JokersBomb)
            {
                if (rule1 != rule2)
                {
                    //如果没有炸弹，且是不同的规则就无法比较
                    return CardCompareResult.CanNotCompare;
                }
                else if (cardArray1.Length != cardArray2.Length)
                {
                    //如果没有炸弹，规则一样但数量不一样，则返回
                    return CardCompareResult.ParamNumberIsDifferent;
                }

                CompareBase compare = null;
                Type[] types = Assembly.GetExecutingAssembly().GetTypes();
                foreach (Type t in types)
                {
                    if (t.Name ==  "Comp" + rule1.ToString() && !t.Name.Contains("DisplayClass"))
                    {
                        compare = (CompareBase)Assembly.GetExecutingAssembly().CreateInstance(t.FullName);
                        break;
                    }
                }

                return compare.GetCardCompareResult(cardArray1, cardArray2);
            }

            //包含炸弹的单独处理
            if (rule1 == RuleType.JokersBomb)
            {
                return CardCompareResult.ParamOneIsBigger;
            }
            else if (rule2 == RuleType.JokersBomb)
            {
                return CardCompareResult.ParamOneIsSmaller;
            }
            else
            {
                CompFourAndZero compare = new CompFourAndZero();
                compare.Rule1 = rule1;
                compare.Rule2 = rule2;
                return compare.GetCardCompareResult(cardArray1, cardArray2);
            }
        }
    }
}
