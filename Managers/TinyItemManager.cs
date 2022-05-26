using Localizer;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TinyHelper
{
    public enum Behaviour
    {
        Add,
        Purge,
        Replace
    }

    public class TinyItemManager
    {
        public static Item MakeItem(
            int newID,
            int targetID,
            string name = null,
            string description = null,
            string identifierName = null
            )
        {
            Item item = TryCloneTargetItem(newID: newID, targetID: targetID, identifierName: newID.ToString() + "_" + identifierName);
            ApplyNameAndDescription(item: item, name: name, description: description);
            return item;
        }

        public static Skill MakeSkill(
            int newID,
            int targetID,
            string name = null,
            string description = null,
            string identifierName = null,
            int? manaCost = 0,
            int? staminaCost = 0,
            int? healthCost = 0,
            bool? ignoreLearnNotification = null,
            bool? hideInUI = null
        )
        {
            Skill skill = MakeItem(newID: newID, targetID: targetID, name: name, description: description, identifierName: identifierName) as Skill;

            if (manaCost != null)
                skill.ManaCost = manaCost ?? 0;
            if (healthCost != null)
                skill.HealthCost = healthCost ?? 0;
            if (staminaCost != null)
                skill.StaminaCost = staminaCost ?? 0;

            skill.IgnoreLearnNotification = ignoreLearnNotification ?? false;

            return skill;
        }

        public static Item TryCloneTargetItem(int newID, int targetID,
            string identifierName = null)
        {
            //The dictionary reference is needed to insert values when they are made
            Dictionary<string, Item> dictionary = ItemPrefabDictionary;

            //Fetch the target item
            var targetItem = GetItem(targetID.ToString());


            //Target_ItemID not found
            if (targetItem == null)
            {
                TinyHelper.TinyHelperPrint("Could not find target item with ID " + targetID + ".");
                return null;
            }

            //Target does not have an Item component
            if (targetItem.gameObject.GetComponent<Item>() == null)
            {
                TinyHelper.TinyHelperPrint(targetItem.name + " is does not have an Item component.");
                return null;
            }

            //Clone target item
            GameObject newItemGameObject = UnityEngine.Object.Instantiate<GameObject>(targetItem.gameObject);
            newItemGameObject.SetActive(false);
            UnityEngine.Object.DontDestroyOnLoad(newItemGameObject);

            //Fetch item, assign item identifier name and ID
            Item item = newItemGameObject.GetComponent<Item>();
            item.ItemID = newID;
            item.name = identifierName ?? (newID.ToString() + "_" + item.Name.Replace(" ", ""));

            //Add item to prefab dictionary
            ItemPrefabDictionary.Add(item.ItemID.ToString(), item);

            //return item
            return item;
        }

        public static void ApplyNameAndDescription(Item item, string name, string description)
        {
            ItemLocalization itemLocalization = new ItemLocalization(name, description);
            At.SetValue<string>(name, typeof(Item), item, "m_name");

            if (ItemLocalizationDictionary.ContainsKey(item.ItemID))
            {
                ItemLocalizationDictionary[item.ItemID] = itemLocalization;
            }
            else
            {
                ItemLocalizationDictionary.Add(item.ItemID, itemLocalization);
            }
        }


        // ----------------- HELPER FUNCTIONS ---------------- //

        /// <summary>
        /// Retusn the LocalizationManager.Instance.m_itemLocalization dictionary by reference.
        /// </summary>
        private static Dictionary<int, ItemLocalization> ItemLocalizationDictionary
        {
            get
            {
                if (m_ItemLocalizationDictionary == null) m_ItemLocalizationDictionary = At.GetValue(typeof(LocalizationManager), LocalizationManager.Instance, "m_itemLocalization") as Dictionary<int, ItemLocalization>;
                return m_ItemLocalizationDictionary;
            }
        }
        private static Dictionary<int, ItemLocalization> m_ItemLocalizationDictionary;
        /// <summary>
        /// Returns the ResourcesPrefabManager.Instance.ITEM_PREFABS dictionary by reference.
        /// </summary>
        private static Dictionary<string, Item> ItemPrefabDictionary
        {
            get
            {
                if (m_ItemPrefabDictionary == null) m_ItemPrefabDictionary = At.GetValue(typeof(ResourcesPrefabManager), null, "ITEM_PREFABS") as Dictionary<string, Item>;
                return m_ItemPrefabDictionary;
            }
        }
        private static Dictionary<string, Item> m_ItemPrefabDictionary;

        /// <summary>
        /// Get an item with ID = itemID from ResourcesPrefabManager dictionary.
        /// </summary>
        private static Item GetItem(int itemID)
        {
            return GetItem(itemID.ToString());
        }

        /// <summary>
        /// Get an item with ID = itemID from ResourcesPrefabManager dictionary.
        /// </summary>
        private static Item GetItem(string itemID)
        {
            return ItemPrefabDictionary[itemID];
        }
    }
}
