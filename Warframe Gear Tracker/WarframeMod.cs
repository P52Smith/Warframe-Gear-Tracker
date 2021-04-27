using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Warframe_Gear_Tracker
{
    public class WarframeMod : WarframeItem
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum Polarities
        {
            Unknown,
            [EnumMember(Value = "AP_ATTACK")] Madurai,
            [EnumMember(Value = "AP_DEFENSE")] Vazarin,
            [EnumMember(Value = "AP_TACTIC")] Naramon,
            [EnumMember(Value = "AP_WARD")] Unairu,
            [EnumMember(Value = "AP_POWER")] Zenurik,
            [EnumMember(Value = "AP_PRECEPT")] Precept,
            [EnumMember(Value = "AP_UMBRA")] Umbra,
            [EnumMember(Value = "AP_UNIVERSAL")] None
        }

        public Polarities Polarity { get; set; }
        public WarframeRarity Rarity { get; set; }
        public int BaseDrain { get; set; }
        public int FusionLimit { get; set; }
        public int MaxDrain => (BaseDrain >= 0) ? (BaseDrain + FusionLimit) : (BaseDrain - FusionLimit);
        public string CompatName { get; set; }
        public string Type { get; set; }
    }
}
