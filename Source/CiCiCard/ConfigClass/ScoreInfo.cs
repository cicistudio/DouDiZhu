using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CiCiCard.ConfigClass
{
    [Serializable]
    public class ScoreInfo
    {
        private int m_LeftScore = 100;

        public int LeftScore
        {
            get { return m_LeftScore; }
            set { m_LeftScore = value; }
        }

        private int m_MiddleScore = 100;

        public int MiddleScore
        {
            get { return m_MiddleScore; }
            set { m_MiddleScore = value; }
        }

        private int m_RightScore = 100;

        public int RightScore
        {
            get { return m_RightScore; }
            set { m_RightScore = value; }
        }
    }
}
