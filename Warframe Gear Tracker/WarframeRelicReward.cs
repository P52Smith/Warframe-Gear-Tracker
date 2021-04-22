using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warframe_Gear_Tracker
{
    public class WarframeRelicReward
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum Rarity
        {
            Common,
            Uncommon,
            Rare
        }

        public Rarity rarity { get; set; }
        public string RewardName { get; set; }
    }
}
