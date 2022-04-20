using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warframe_Gear_Tracker
{
    public class WarframeIngredient : WarframeItem
    {
        public new string Name
        {
            get
            {
                if (App.AllItems.ContainsKey(UniqueName))
                {
                    return App.AllItems[UniqueName].Name;
                }
                else
                {
                    return System.IO.Path.GetFileName(UniqueName);
                }
            }
        }
        public string ItemType { get => UniqueName; set => UniqueName = value; }
        public uint ItemCount { get; set; }
        public string ProductCategory { get; set; }
    }
}
