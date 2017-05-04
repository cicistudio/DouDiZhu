using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using AIDemo.ErrorClass;

namespace AIDemo.AIClass
{
    public abstract class AIBase
    {
        /// <summary>
        /// 用于反射调用的，AI应该出牌的集合。这里使用虚函数，因为很多种队友出的组合牌，我们都不用打。如果需要打，那么就重写这个方法。
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public virtual int[] GetCardByReflect(OutPutCardInfo info)
        {
            //如果是队友那么就不出牌。
            if (CheckIsPartern(info))
            {
                return null;
            }
            return GetOutPutCard(info);//不是队友就出牌
        }

        /// <summary>
        /// 获取应该打什么牌
        /// </summary>
        /// <param name="lastCardArray">上个玩家，和他出的牌</param>
        /// <returns></returns>
        public abstract int[] GetOutPutCard(OutPutCardInfo info);

        /// <summary>
        /// 检查是不是队友
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected bool CheckIsPartern(OutPutCardInfo info)
        {
            if (AIOptions.IsHostMySelf)
            {
                //如果自己是地主
                return false;
            }
            //如果出牌者是地主那么就不是队友，反之就是队友。
            return info.Player != AIOptions.CurrentHost;
        }

        /// <summary>
        /// 获得相同牌的集合
        /// </summary>
        /// <param name="cardArray">牌数组</param>
        /// <param name="card">多少张相同的牌</param>
        /// <returns></returns>
        protected List<int> GetKindCollection(int[] cardArray, int card)
        {
            List<int> kindCollection = new List<int>();
            foreach (int i in cardArray)
            {
                var q = from c in cardArray
                        where c == i
                        select c;
                if (q.Count() == card)
                {
                    if (!kindCollection.Contains(i))
                    {
                        kindCollection.Add(i);
                    }
                }
            }
            return kindCollection;
        }

        protected List<int> GetKindCollection(ArrayList cardArray, int card)
        {
            List<int> kindCollection = new List<int>();
            foreach (int i in cardArray)
            {
                var q = from int c in cardArray
                        where c == i
                        select c;
                if (q.Count() == card)
                {
                    if (!kindCollection.Contains(i))
                    {
                        kindCollection.Add(i);
                    }
                }
            }
            return kindCollection;
        }

        protected List<int> GetKindCollection2(ArrayList cardArray, int card)
        {
            List<int> kindCollection = new List<int>();
            foreach (int i in cardArray)
            {
                var q = from int c in cardArray
                        where c == i && c<15
                        select c;
                if (q.Count() == card)
                {
                    if (!kindCollection.Contains(i))
                    {
                        kindCollection.Add(i);
                    }
                }
            }
            return kindCollection;
        }

        /// <summary>
        /// 获得所有的5张以上连牌
        /// </summary>
        /// <param name="cardArray"></param>
        /// <returns></returns>
        protected List<List<int>> GetContinueKindCollection(ArrayList cardArray)
        {
            try
            {
                List<List<int>> kindConCollection = new List<List<int>>();
                var query = (from int c in cardArray
                             where c < 15
                             orderby c
                             select c).Distinct();
                if (query.Count() == 0)
                {
                    return kindConCollection;//没有就返回
                }
                int n = 0;
                int min = query.First();
                List<int> kind = query.ToList();
                kind.Add(-1);
                foreach (int i in kind)
                {
                    if (i - min != n)
                    {
                        //由于是测试DEMO，就不加入很多判断了。
                        if (n >= 5)
                        {
                            List<int> collection = new List<int>();
                            for (int j = min; j < min + n; j++)
                            {
                                collection.Add(j);
                            }
                            kindConCollection.Add(collection);
//#if DEBUG
//                            StringBuilder log = new StringBuilder();
//                            log.Append("得到一个5张连牌:");
//                            foreach (int j in collection)
//                            {
//                                log.Append(j);
//                                log.Append(",");
//                            }
//                            log.Append("\r\n");
//                            File.AppendAllText(Application.StartupPath + "\\AILog.txt",log.ToString());
//#endif
                        }

                        min = i;//重新开始计算
                        n = 0;
                    }
                    n++;
                }

                return kindConCollection;
            }
            catch(FiveContinueExeception)
            {
                throw;
            }

        }

        /// <summary>
        /// 获取所有的单牌
        /// </summary>
        /// <param name="cardArray">牌数组</param>
        /// <returns></returns>
        protected List<int> GetSingleKindCollection(ArrayList cardArray)
        {
            try
            {
                List<int> singleCardCollection = new List<int>();
                foreach (int i in cardArray)
                {
                    var q = from int c in cardArray
                            where c == i && c < 16
                            select c;
                    if (q.Count() == 1)
                    {
                        singleCardCollection.Add(i);//单牌集合
                    }
                }
                if (singleCardCollection.Count == 0)
                {
                    return singleCardCollection;//没有单牌
                }
                singleCardCollection.Sort();

                //去除可能成顺的连牌
                List<List<int>> kindConCollection = GetContinueKindCollection(cardArray);
                foreach (List<int> kind in kindConCollection)
                {
                    foreach (int i in kind)
                    {
                        singleCardCollection.Remove(i);
                    }
                }

                return singleCardCollection;
            }
            catch (SingleKindExeception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获得大小王的情况
        /// </summary>
        /// <param name="cardArray"></param>
        /// <returns></returns>
        protected List<int> GetJokerCollection(ArrayList cardArray)
        {
            var q = from int c in cardArray
                    where c >= 16
                    select c;
            return q.ToList();
        }

        /// <summary>
        /// 获得2的情况
        /// </summary>
        /// <param name="cardArray"></param>
        /// <returns></returns>
        protected List<int> GetFifteenCollection(ArrayList cardArray)
        {
            var q = from int c in cardArray
                    where c == 15
                    select c;
            return q.ToList();
        }

    }
}
