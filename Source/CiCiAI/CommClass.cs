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
namespace CiCiAI
{
    public class CommClass
    {
        public enum PlayerSide
        {
            None,
            Left,
            Middle,
            Right
        }

        public enum PlayerLocation
        {
            None,
            Up,
            MySelf,
            Down
        }

        public enum PokerCombineType
        {
            NONE = 0,
            ZhaDan = 1,  
            ShunZi = 2,
            FeiJi = 3,
            LianDui = 4,
            SanDui = 5,
            DuiZi = 6
        }

        public enum Poker
        {
            None = 0,           
            P3 = 3,
            P4 = 4,
            P5 = 5,
            P6 = 6,
            P7 = 7,
            P8 = 8,
            P9 = 9,
            P10 = 10,
            J = 11,
            Q = 12,
            K = 13,
            A = 14,
            P2 = 15,
            JKSmall = 16,
            JKBig = 17
        }

        public static Poker PockerCharToEnum(string Poker)
        {
            switch(Poker)
            {
                case "2":
                    return CommClass.Poker.P2;
                case "3":
                    return CommClass.Poker.P3;
                case "4":
                    return CommClass.Poker.P4;
                case "5":
                    return CommClass.Poker.P5;
                case "6":
                    return CommClass.Poker.P6;
                case "7":
                    return CommClass.Poker.P7;
                case "8":
                    return CommClass.Poker.P8;
                case "9":
                    return CommClass.Poker.P9;
                case "10":
                    return CommClass.Poker.P10;
                case "J":
                    return CommClass.Poker.J;
                case "Q":
                    return CommClass.Poker.Q;
                case "K":
                    return CommClass.Poker.K;
                case "A":
                    return CommClass.Poker.A;
                case "JKSmall":
                    return CommClass.Poker.JKSmall;
                case "JKBig":
                    return CommClass.Poker.JKBig;
            }

            throw new Exception("Invalid Input");
        }

        public static string EnumToPockerChar(Poker poker)
        {
            switch (poker)
            {
                case Poker.P2:
                    return "2";
                case Poker.P3:
                    return "3";
                case Poker.P4:
                    return "4";
                case Poker.P5:
                    return "5";
                case Poker.P6:
                    return "6";
                case Poker.P7:
                    return "7";
                case Poker.P8:
                    return "8";
                case Poker.P9:
                    return "9";
                case Poker.P10:
                    return "10";
                case Poker.J:
                    return "J";
                case Poker.Q:
                    return "Q";
                case Poker.K:
                    return "K";
                case Poker.A:
                    return "A";
                case Poker.JKSmall:
                    return "JKSmall";
                case Poker.JKBig:
                    return "JKBig";
            }
            throw new Exception("Invalid Input");
        }

        public static List<Poker> PockerStringToList(string pokers)
        {
            List<Poker> PokerList = new List<Poker>();
            foreach (string s in pokers.Split(new char[] { ',' }))
            {
                PokerList.Add(CommClass.PockerCharToEnum(s));
            }
            return PokerList;
        }

        public static string ListToPockerString(List<Poker> pokers)
        {
            if (pokers.Count == 0) return string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach (Poker p in pokers)
            {
                sb.Append(EnumToPockerChar(p));
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        public static string GetLianShunString(Poker poker, int count)
        {
            StringBuilder sb = new StringBuilder();
            for(int i=0;i<count;i++)
            {
                sb.Append(EnumToPockerChar((Poker)((int)poker + i)));
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        public static List<Poker> RemovePokerFromList (List<Poker> originList, List<Poker> removeList)
        {
            List<Poker> newPokerList = originList.ToList();
            foreach(Poker p in removeList)
            {
                newPokerList.RemoveAt(newPokerList.FindIndex(q => q == p));
            }
            return newPokerList;
        }

        public static List<Poker> RemovePokerFromList(List<Poker> originList, string removeList)
        {
            removeList = removeList.Replace('|', ',');
            return RemovePokerFromList(originList, PockerStringToList(removeList));
        }

        public static List<string> RemoveDuplicatePokers(List<string> pokerList)
        {
            List<string> SortedPokerList = new List<string>();
            foreach(string pokers in pokerList)
            {
                List<string> pockerSplitList = pokers.Split(new char[] { '|' }).ToList();
                pockerSplitList.Sort();
                StringBuilder SortedSB = new StringBuilder();
                foreach(string p in pockerSplitList)
                {
                    SortedSB.Append(p);
                    SortedSB.Append("|");
                }
                SortedSB.Remove(SortedSB.Length - 1, 1);
                SortedPokerList.Add(SortedSB.ToString());
            }
            return SortedPokerList.Distinct().ToList();
        }


        public static string GetRemainPokerCombineTypes(PokerCombineType removedType)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in Enum.GetNames(typeof(PokerCombineType)))
            {
                if (s == "NONE") continue;
                sb.Append((int)Enum.Parse(typeof(PokerCombineType), s));
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            
            int removedTypeID = (int)removedType;
            return GetRemainPokerCombineTypes(sb.ToString(), removedTypeID);
        }

        public static string GetRemainPokerCombineTypes(string currentCombinTypes, PokerCombineType removedType)
        {
            int removedTypeID = (int)removedType;
            return GetRemainPokerCombineTypes(currentCombinTypes, removedTypeID);
        }

        public static string GetRemainPokerCombineTypes(string currentCombinTypes, int removedTypeID)
        {
            if (currentCombinTypes.IndexOf(removedTypeID.ToString()) == currentCombinTypes.Length -1)
            {
                return currentCombinTypes.Replace("," + removedTypeID.ToString(), "");
            }
            else
            {
                return currentCombinTypes.Replace(removedTypeID.ToString() + ",", "").Replace(removedTypeID.ToString(), "");
            }
        }

        public static string GetPokerStringFromIntArray(int[] pockers)
        {
            StringBuilder sb = new StringBuilder();
            foreach(int p in pockers)
            {
                //注意: A算14，2算15,小王16，大王17
                if(p>=3 && p<=10)
                {
                    sb.Append(p.ToString());
                    sb.Append(",");
                }
                else if(p==11)
                {
                    sb.Append("J");
                    sb.Append(",");
                }
                else if (p == 12)
                {
                    sb.Append("Q");
                    sb.Append(",");
                }
                else if (p == 13)
                {
                    sb.Append("K");
                    sb.Append(",");
                }
                else if (p == 14)
                {
                    sb.Append("A");
                    sb.Append(",");
                }
                else if (p == 15)
                {
                    sb.Append("2");
                    sb.Append(",");
                }
                else if (p == 16)
                {
                    sb.Append("JKSmall");
                    sb.Append(",");
                }
                else if (p == 17)
                {
                    sb.Append("JKBig");
                    sb.Append(",");
                }
            }

            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
    }
}
