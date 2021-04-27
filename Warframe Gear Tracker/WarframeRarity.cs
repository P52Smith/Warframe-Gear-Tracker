using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warframe_Gear_Tracker
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum WarframeRarity
    {
        Unknown,
        Common,
        Uncommon,
        Rare,
        Legendary
    }
}
