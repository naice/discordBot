using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiteAPI.Model
{
    public class ItemMenuItem
    {
        public string Description { get; set; }
        public string Value { get; set; }
    }

    public class ItemItemDescription
    {
        public string Description { get; set; }
        public IList<ItemMenuItem> Menuitems { get; set; }
        public string SecondaryDescription { get; set; }
    }

    public class Item
    {
        public int ChildItemId { get; set; }
        public string DeviceName { get; set; }
        public int IconId { get; set; }
        public ItemItemDescription ItemDescription { get; set; }
        public int ItemId { get; set; }
        public int ItemTier { get; set; }
        public int Price { get; set; }
        public int RootItemId { get; set; }
        public string ShortDesc { get; set; }
        public bool StartingItem { get; set; }
        public string Type { get; set; }
        public string itemIcon_URL { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public string itemIcon_URL_Cache { get { return Path.Combine(CacheConfig.GodImagePath, $"Item_{ItemId}_icon{Path.GetExtension(itemIcon_URL)}"); } }
        public object ret_msg { get; set; }
    }
}
