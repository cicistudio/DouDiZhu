using CiCiAI.Core.Combine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CiCiAI.Core
{
    public class OutputInfo
    {
        public List<DanPaiInfo> DanPaiList { get; set; }
        public List<DuiZiInfo> DuiZiList{ get; set; }
        public List<FeiJiInfo> FeijiList { get; set; }
        public List<HuoJianInfo> HuoJianList { get; set; }
        public List<LianDuiInfo> LianDuiList { get; set; }
        public List<SanDuiInfo> SanDuiList { get; set; }
        public List<ShunZiInfo> ShunZiList { get; set; }
        public List<ZhaDanInfo> ZhaDanList { get; set; }
    }
}
