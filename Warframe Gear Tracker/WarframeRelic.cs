using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warframe_Gear_Tracker
{
    public class WarframeRelic : WarframeItem
    {
        public enum Tiers
        {
            Lith,
            Meso,
            Neo,
            Axi,
            Requiem,
            Unknown
        }
        public List<WarframeRelicReward> RelicRewards { get; set; }
        public Tiers Tier
        {
            get
            {
                if (Name.ToLower().StartsWith("lith"))
                {
                    return Tiers.Lith;
                }
                else if (Name.ToLower().StartsWith("meso"))
                {
                    return Tiers.Meso;
                }
                else if (Name.ToLower().StartsWith("neo"))
                {
                    return Tiers.Neo;
                }
                else if (Name.ToLower().StartsWith("axi"))
                {
                    return Tiers.Axi;
                }
                else if (Name.ToLower().StartsWith("requiem"))
                {
                    return Tiers.Requiem;
                }
                else
                {
                    return Tiers.Unknown;
                }
            }
        }
        public string RelicName => (Name.Split(' ').Count() >= 3) ? Name.Split(' ')[1] : Name;
    }
}
