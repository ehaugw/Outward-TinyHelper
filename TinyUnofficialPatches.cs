using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyHelper
{
    class TinyUnofficialPatches
    {

    }

    [HarmonyPatch(typeof(Skill), "HasEnoughMana")]
    public class Skill_HasEnoughMana
    {
        [HarmonyPostfix]
        public static void Postfix(Skill __instance, ref bool _tryingToActivate, ref bool __result)
        {
            if (__instance.ManaCost <= 0)
            {
                __result = true;
            }
        }
    }

    [HarmonyPatch(typeof(Skill), "HasEnoughStamina")]
    public class Skill_HasEnoughStamina
    {
        [HarmonyPostfix]
        public static void Postfix(Skill __instance, ref bool _tryingToActivate, ref bool __result)
        {
            if (__instance.StaminaCost <= 0)
            {
                __result = true;
            }
        }
    }

    [HarmonyPatch(typeof(Skill), "HasEnoughHealth")]
    public class Skill_HasEnoughHealth
    {
        [HarmonyPostfix]
        public static void Postfix(Skill __instance, ref bool _tryingToActivate, ref bool __result)
        {
            if (__instance.HealthCost <= 0)
            {
                __result = true;
            }
        }
    }
}
