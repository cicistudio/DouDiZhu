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
namespace AIFrameWork
{
    /// <summary>
    /// 牌对比结果枚举
    /// </summary>
    public enum CardCompareResult
    {
        /// <summary>
        /// 参数一不符合任何规则
        /// </summary>
        ParamOneOutOfRule,
        /// <summary>
        /// 参数二不符合任何规则
        /// </summary>
        ParamTwoOutOfRule,
        /// <summary>
        /// 参数一和参数二都不符合规则
        /// </summary>
        ParamAllOutOfRule,
        /// <summary>
        /// 参数一更大
        /// </summary>
        ParamOneIsBigger,
        /// <summary>
        /// 参数一更小。
        /// </summary>
        ParamOneIsSmaller,
        /// <summary>
        /// 参数一和参数二相等
        /// </summary>
        ParamOneAndTwoEqual,
        /// <summary>
        /// 不同的牌组合无法对比
        /// </summary>
        CanNotCompare,
        /// <summary>
        /// 参数一和参数二，数量不一样。
        /// </summary>
        ParamNumberIsDifferent
    }

    /// <summary>
    /// 针对所有规则的枚举
    /// </summary>
    public enum RuleType
    {
        /// <summary>
        /// 不符合规则
        /// </summary>
        OutOfRule,
        /// <summary>
        /// 单牌
        /// </summary>
        Single,
        /// <summary>
        /// 一对
        /// </summary>
        TwoGroup,
        /// <summary>
        /// 三联对以及以上
        /// </summary>
        ThreeGroupMore,
        /// <summary>
        /// 三张牌相同的牌
        /// </summary>
        ThreeAndZero,
        /// <summary>
        /// 3带1
        /// </summary>
        ThreeAndOne,
        /// <summary>
        /// 3带2
        /// </summary>
        ThreeAndTwo,
        /// <summary>
        /// 炸弹
        /// </summary>
        FourAndZero,
        /// <summary>
        /// 4带2
        /// </summary>
        FourAndTwo,
        /// <summary>
        /// 小五支
        /// </summary>
        FiveAndMore,
        /// <summary>
        /// 不带翅膀的飞机，3+3 这种，可以连很多3
        /// </summary>
        PlaneNoWing,
        /// <summary>
        /// 3+3+1+1
        /// </summary>
        PlaneOneWing,
        /// <summary>
        /// 3+3+2+2
        /// </summary>
        PlaneTwoWing,
        /// <summary>
        /// 3+3+3+1+1+1
        /// </summary>
        PlaneOneWingMore,
        /// <summary>
        /// 3+3+3+2+2
        /// </summary>
        PlaneTwoWingMore,
        /// <summary>
        /// 双王炸弹
        /// </summary>
        JokersBomb
    }
}
