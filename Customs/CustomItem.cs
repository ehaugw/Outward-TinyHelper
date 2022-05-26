//using Localizer;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//namespace TinyHelper.Customs
//{
//    public enum Behaviour
//    {
//        Add,
//        Purge,
//        Replace
//    }

//    public class CustomItem
//    {
//        public Item[] ItemReference;

//        public Behaviour EffectBehaviour;

//        //General
//        public int Target_ItemID;
//        public int New_ItemID;
//        public string Description;
//        public string Name;

//        //Tags
//        public List<string> Tags;
//        public Behaviour TagBehaviour;

//        //Common Stats
//        public float? BaseValue;
//        public float? RawWeight;
//        public float? MaxDurability;

//        public EquipmentSlot EquipSlot;
//        public Equipment.TwoHandedType? TwoHandType;
//        public Weapon.WeaponType? WeaponType;


//        //Graphic stuff
//        public string PathPrefix;
//        public string AssetBundlePath;
//        public string ItemVisuals_PrefabName;
//        public string SpecialVisuals_PrefabName;
//        public string IconPath;
//        public string ThumbnailPath;

//        public CustomItem(out Item[] ItemReference)
//        {
//            this.ItemReference = ItemReference = new Item[1];

//            QueueForInit(this);
//        }


//        /// <summary>
//        /// A list of CustomItem that can be expanded at any time during startup. The items are initiated at a suitable time.
//        /// </summary>
//        private static List<CustomItem> CustomItemStates
//        {
//            get {
//                if (m_customItemStates == null) m_customItemStates = new List<CustomItem>();
//                return m_customItemStates;
//            }
//            set { m_customItemStates = value; }
//        }
//        private static List<CustomItem> m_customItemStates;

//        /// <summary>
//        /// Add the customItem to the list of CustomItem that are yet to be initiated. This can be done at any time during startup.
//        /// </summary>
//        private static void QueueForInit(CustomItem customItem)
//        {
//            CustomItemStates.Add(customItem);
//        }

//        /// <summary>
//        /// Initiate a list of CustomItem, should be done only once during startup.
//        /// </summary>
//        public static void ApplyCustomItems(bool forceAll = false)
//        {
//            try
//            {
//                List<CustomItem> queue = CustomItemStates;
//                List<CustomItem> postpone = new List<CustomItem>();

//                while (queue.Count > 0)
//                {
//                    if (!queue.First().TryApplyCustomItem())
//                        postpone.Add(queue.First());
//                    queue.RemoveAt(0);

//                    //Empty queue means the queue has been iterated once and applicable items are applied.
//                    if (queue.Count == 0 && postpone.Count > 0)
//                    {
//                        queue = postpone;
//                        postpone = new List<CustomItem>();

//                        //Break of the loop if we do not wish to keep going till all are successfull
//                        if (!forceAll) return;
//                    }
//                }
//            } catch(Exception e)
//            {
//                Debug.Log(e.Message);
//            }
//        }

//        virtual public bool TryApplyCustomItem()
//        {
//            TinyHelper.TinyHelperPrint("start");
//            if (TryCloneTargetItem(this) == null) return false;
//            TinyHelper.TinyHelperPrint("name");
//            ApplyNameAndDescription(this);
//            TinyHelper.TinyHelperPrint("icon");
//            CustomTexture.ApplyItemIcon(this);
//            TinyHelper.TinyHelperPrint("tags");
//            CustomTag.ApplyTags(this);
//            TinyHelper.TinyHelperPrint("stats");
//            CustomItem.ApplyStats(this);
//            TinyHelper.TinyHelperPrint("visuals");
//            CustomAsset.ApplyVisuals(this);
//            TinyHelper.TinyHelperPrint("done");
//            return true;
//        }

//        /// <summary>
//        /// Clones item with ID = customItem.Target_ItemID, assigns ID = customItem.New_ItemID, and adds to ResourcesPrefabManager dictionary. The resulting item is assigned to customItem.ItemReference[0].
//        /// </summary>
//        /// <returns>Returns an array of the initiated item if successful or null if not.</returns>
//        public static Item[] TryCloneTargetItem(CustomItem customItem)
//        {
//            //The dictionary reference is needed to insert values when they are made
//            Dictionary<string, Item> dictionary = ItemPrefabDictionary;
            
//            //Fetch the target item
//            var targetItem = GetItem(customItem.Target_ItemID.ToString());


//            //Target_ItemID not found
//            if (targetItem == null)
//            {
//                TinyHelper.TinyHelperPrint("Could not find target item with ID " + customItem.Target_ItemID + ".");
//                return null;
//            }

//            //Target does not have an Item component
//            if (targetItem.gameObject.GetComponent<Item>() == null)
//            {
//                TinyHelper.TinyHelperPrint(targetItem.name + " is does not have an Item component.");
//                return null;
//            }

//            //Clone target item
//            GameObject newItemGameObject = UnityEngine.Object.Instantiate<GameObject>(targetItem.gameObject);
//            newItemGameObject.SetActive(false);
//            UnityEngine.Object.DontDestroyOnLoad(newItemGameObject);

//            //Fetch item, assign item identifier name and ID
//            Item item = newItemGameObject.GetComponent<Item>();
//            item.ItemID = customItem.New_ItemID;
//            item.name = customItem.New_ItemID.ToString() + "_" + customItem.Name.Replace(" ", "");

//            //Assign item to item reference
//            customItem.ItemReference[0] = item;
            
//            //Add item to prefab dictionary
//            ItemPrefabDictionary.Add(item.ItemID.ToString(), customItem.ItemReference[0]);

//            //return item
//            return customItem.ItemReference;


//        }

//        /// <summary>
//        /// Assigns the desired name and description to an item.
//        /// </summary>
//        /// <param name="customItem">CustomItem instance containing item reference and the desired name.</param>
//        public static void ApplyNameAndDescription(CustomItem customItem)
//        {
//            var name = customItem.Name ?? customItem.ItemReference[0].Name;
//            var desc = customItem.Description ?? customItem.ItemReference[0].Description;

//            ItemLocalization itemLocalization = new ItemLocalization(name, desc);
//            At.SetValue<string>(name, typeof(Item), customItem.ItemReference[0], "m_name");

//            if (ItemLocalizationDictionary.ContainsKey(customItem.New_ItemID))
//                ItemLocalizationDictionary[customItem.New_ItemID] = itemLocalization;
//            else
//                ItemLocalizationDictionary.Add(customItem.New_ItemID, itemLocalization);
//        }

//        /// <summary>
//        /// Applies the stats from CustomItem to CustomItem.ItemReference[0]
//        /// </summary>
//        public static void ApplyStats(CustomItem customItem)
//        {
//        }
        
//        // ----------------- HELPER FUNCTIONS ---------------- //

//        /// <summary>
//        /// Retusn the LocalizationManager.Instance.m_itemLocalization dictionary by reference.
//        /// </summary>
//        private static Dictionary<int, ItemLocalization> ItemLocalizationDictionary
//        {
//            get
//            {
//                if (m_ItemLocalizationDictionary == null) m_ItemLocalizationDictionary = At.GetValue(typeof(LocalizationManager), LocalizationManager.Instance, "m_itemLocalization") as Dictionary<int, ItemLocalization>;
//                return m_ItemLocalizationDictionary;
//            }
//        }
//        private static Dictionary<int, ItemLocalization> m_ItemLocalizationDictionary;
//        /// <summary>
//        /// Returns the ResourcesPrefabManager.Instance.ITEM_PREFABS dictionary by reference.
//        /// </summary>
//        private static Dictionary<string, Item> ItemPrefabDictionary
//        {
//            get
//            {
//                if (m_ItemPrefabDictionary == null) m_ItemPrefabDictionary = At.GetValue(typeof(ResourcesPrefabManager), null, "ITEM_PREFABS") as Dictionary<string, Item>;
//                return m_ItemPrefabDictionary;
//            }
//        }
//        private static Dictionary<string, Item> m_ItemPrefabDictionary;

//        /// <summary>
//        /// Get an item with ID = itemID from ResourcesPrefabManager dictionary.
//        /// </summary>
//        private static Item GetItem(int itemID)
//        {
//            return GetItem(itemID.ToString());
//        }

//        /// <summary>
//        /// Get an item with ID = itemID from ResourcesPrefabManager dictionary.
//        /// </summary>
//        private static Item GetItem(string itemID)
//        {
//            return ItemPrefabDictionary[itemID];
//        }
//    }
//}
