using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warframe_Gear_Tracker
{
    public class WarframeItem
    {
        static public Dictionary<string, bool> OwnedLog;
        public string UniqueName { get; set; }
        public string Name { get; set; }
        [JsonProperty("masteryReq")]
        public byte Mastery { get; set; }
        public bool Owned
        {
            get => OwnedLog.ContainsKey(UniqueName) && OwnedLog[UniqueName];

            set
            {
                if ((OwnedLog.ContainsKey(UniqueName) && OwnedLog[UniqueName]) != value)
                {
                    OwnedLog[UniqueName] = value;
                }
            }
        }
        public bool Needed { get; set; }
    }
}
