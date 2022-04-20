using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warframe_Gear_Tracker
{
    public class WarframeRecipe : WarframeItem
    {
        public new string Name
        {
            get
            {
                if (App.AllItems.ContainsKey(resultType))
                {
                    return $"{App.AllItems[resultType].Name} Blueprint";
                }
                else
                {
                    return $"{System.IO.Path.GetFileName(resultType)} Blueprint";
                }
            }
        }
        public string resultType { get; set; }
        public List<WarframeIngredient> Ingredients { get; set; }
    }
}
