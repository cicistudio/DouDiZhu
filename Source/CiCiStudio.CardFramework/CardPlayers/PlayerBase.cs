using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CiCiStudio.CardFramework.CardPlayers
{
    public abstract class PlayerBase
    {
        private List<PlayerCardInfo> m_CardCollection = new List<PlayerCardInfo>();

        public List<PlayerCardInfo> CardCollection
        {
            get { return m_CardCollection; }
            set { m_CardCollection = value; }
        }

        /// <summary>
        /// 是否是地主
        /// </summary>
        public bool IsAHost { get; set; }
    }
}
