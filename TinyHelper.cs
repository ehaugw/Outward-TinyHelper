using HarmonyLib;

namespace TinyHelper
{
    using UnityEngine;
    using BepInEx;
    using System.IO;
    using System;
    using SideLoader;

    [BepInPlugin(GUID, NAME, VERSION)]
    public class TinyHelper : BaseUnityPlugin
    {
        public const string GUID = "com.ehaugw.tinyhelper";
        public const string VERSION = "3.0.0";
        public const string NAME = "Tiny Helper";

        public static event Action OnPrefabLoaded = delegate () { };
        //public static Func<Character, Character, DamageList, float, float> GetDamageToDelay = delegate (Character character, Character dealer, DamageList damageList, float damage) { return 0; };

        //public static Func<Item, string, string> OnDescriptionModified = delegate (Item item, string description) { return description; };
        public delegate void DescriptionModifier(Item item, ref string description);
        public static DescriptionModifier OnDescriptionModified = delegate(Item item, ref string description) {};

        internal void Awake()
        {
            var harmony = new Harmony(GUID);
            harmony.PatchAll();
        }

        private static int tinyHelperPrintedMessages = 0;
        public static void TinyHelperPrint(string str)
        {
            Debug.Log("TinyHelper #" + tinyHelperPrintedMessages++ + ": " + str);
        }

        /// <summary>
        /// The path of the folder that contains all the BepInEx mods
        /// </summary>
        static public string PLUGIN_ROOT_PATH
        {
            get
            {
                return typeof(TinyHelper).Assembly.Location + @"\..\..\";
                //return Paths.BepInExRootPath + @"\plugins\";
            }
        }

        [HarmonyPatch(typeof(ResourcesPrefabManager), "Load")]
        public class ResourcesPrefabManager_Load
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (ResourcesPrefabManager.Instance?.Loaded ?? false)
                {
                    OnPrefabLoaded();
                }
            }
        }

        //[HarmonyPatch(typeof(ItemListDisplay), "MatchFilter")]
        //public class ItemListDisplay_MatchFilter
        //{
        //    [HarmonyPostfix]
        //    public static bool Postfix(Item __instance, ref bool __result)
        //    {
        //        if (__instance.OwnerCharacter != null && __instance.HasTag(TinyTagManager.GetOrMakeTag(InstanceIDs.IDs.InvisitibleItemTag)))
        //        {
        //            __result = false;
        //            return false;
        //        }
        //        return true;
        //    }
        //}

        [HarmonyPatch(typeof(Item), "Description", MethodType.Getter)]
        //[HarmonyAfter(new string[] {"com.sinai.sideloader"})]
        public class Item_Description
        {
            [HarmonyPostfix]
            public static void Postfix(Item __instance, ref string __result)
            {
                TinyHelper.OnDescriptionModified(__instance, ref __result);
                //DescriptionModifier(__instance, __result);
            }
        }

        //[HarmonyPatch(typeof(Item), "GetSyncDataFromSaveData")]
        //public class Item_GetSyncDataFromSaveData
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix(Item __instance, ref string __result)
        //    {
        //        Debug.Log("Loaded item: " + __instance.name " + as xml: " + __result);
        //    }
        //}
    }
}