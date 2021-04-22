using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warframe_Gear_Tracker
{
    public class WarframeWeapon : WarframeItem
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum WeaponType
        {
            LongGuns,
            Pistols,
            Melee,
            SentinelWeapons,
            SpaceGuns,
            SpaceMelee,
            SpecialItems,
            CrewShipWeapons
        }

        [JsonProperty("ProductCategory")]
        public WeaponType Type { get; set; }
        public byte Slot { get; set; }
    }
}
