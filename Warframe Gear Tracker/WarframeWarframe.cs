using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warframe_Gear_Tracker
{
    public class WarframeWarframe : WarframeItem
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum WarframeType
        {
            Suits,
            SpaceSuits,
            MechSuits
        }

        [JsonProperty("ProductCategory")]
        public WarframeType Type { get; set; }
    }
}
