using Localizer;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace TinyHelper
{
    public class TinyEffectManager
    {
        /// <summary>
        /// </summary>
        /// <param name="effectName">used for both the readable name and the identifier name of the status effect</param>
        /// <param name="familyName">used for both the identifier name of the effect family</param>
        /// <param name="description">the visual description of the effect</param>
        /// <param name="lifespan">the lifespan of the effect in seconds. set it to -1 for a permanent effect</param>
        /// <param name="refreshRate">the time interval in seconds between each application of the effects of the status effect</param>
        /// <param name="stackBehavior">what happens if the status effect manager already has the effect active</param>
        /// <param name="targetStatusName">the effect to clone for visual FX and icon</param>
        /// <param name="isMalusEffect">set to true if this is a debuff, or to false if it is not</param>
        /// <param name="tagID">used to tell if this is a boon, hex, generic buff and so on</param>
        /// <param name="uid"></param>
        /// <param name="modGUID"></param>
        /// <param name="iconFileName">The path is appended to plugin path, and must include file endings.</param>
        /// <returns>returns an Outward StatusEffect instance</returns>
        public static StatusEffect MakeStatusEffectPrefab(string effectName, string familyName, string description, float lifespan, float refreshRate, StatusEffectFamily.StackBehaviors stackBehavior, string targetStatusName, bool isMalusEffect,
            string tagID = null,
            UID? uid = null,
            string modGUID = null,
            string iconFileName = null,
            string displayName = null,
            string rootPath = null
        )
        {
            Dictionary<string, StatusEffect> statusEffectDictionary = At.GetValue(typeof(ResourcesPrefabManager), null, "STATUSEFFECT_PREFABS") as Dictionary<string, StatusEffect>;

            //Make a new effect family
            var effectFamily = MakeStatusEffectFamiliy(familyName, stackBehavior, -1, StatusEffectFamily.LengthTypes.Short);

            //Make a new game object
            var gameObject = TinyGameObjectManager.InstantiateClone(statusEffectDictionary[targetStatusName].gameObject, effectName, false, true);
            //var gameObject = MakeFreshObject(effectName, false, true);

            //Add/edit the status effect component to the game object.
            StatusEffect newStatusEffect = statusEffectDictionary[effectName] = gameObject.GetComponent<StatusEffect>() ?? gameObject.AddComponent<StatusEffect>();
            At.SetValue(effectName, typeof(StatusEffect), newStatusEffect, "m_identifierName");
            At.SetValue(effectFamily, typeof(StatusEffect), newStatusEffect, "m_bindFamily");
            At.SetValue(displayName ?? effectName, typeof(StatusEffect), newStatusEffect, "m_nameLocKey");
            At.SetValue(description, typeof(StatusEffect), newStatusEffect, "m_descriptionLocKey");
            newStatusEffect.RefreshRate = refreshRate;
            newStatusEffect.IsMalusEffect = isMalusEffect;
            At.SetValue(StatusEffect.EffectSignatureModes.Reference, typeof(StatusEffect), newStatusEffect, "m_effectSignatureMode");
            At.SetValue(StatusEffect.FamilyModes.Bind, typeof(StatusEffect), newStatusEffect, "m_familyMode");
            newStatusEffect.RequiredStatus = null;
            newStatusEffect.RemoveRequiredStatus = false;
            if (iconFileName != null) newStatusEffect.OverrideIcon = CustomTexture.MakeSprite(CustomTexture.LoadTexture(iconFileName, 0, false, FilterMode.Point, rootPath: rootPath), CustomTexture.SpriteBorderTypes.None);

            //Make a proper tag source selector, which applies tags to the effect.
            var tagSourceSelector = (tagID != null ? new TagSourceSelector(TagSourceManager.Instance.GetTag(tagID)) : new TagSourceSelector());
            At.SetValue(tagSourceSelector, typeof(StatusEffect), newStatusEffect, "m_effectType");

            //Clone and update StatusData
            var statusData = newStatusEffect.StatusData = new StatusData(newStatusEffect.StatusData);
            statusData.LifeSpan = lifespan;
            List<StatusData> statusStack = At.GetValue(typeof(StatusEffect), newStatusEffect, "m_statusStack") as List<StatusData>;
            statusStack[0] = statusData;

            //EffectSignature
            UnityEngine.Object.Destroy(gameObject.GetComponentInChildren<EffectSignature>().gameObject);
            EffectSignature effectSignature = TinyGameObjectManager.MakeFreshObject("Signature", true, true/*, gameObject.transform*/).AddComponent<EffectSignature>();
            effectSignature.name = "Signature";
            effectSignature.SignatureUID = uid ?? TinyUIDManager.MakeUID(effectName, modGUID, "Status Effect");
            effectFamily.EffectSignature = statusData.EffectSignature = effectSignature;

            //Family Selector
            var aa = new StatusEffectFamilySelector();
            aa.Set(effectFamily);
            At.SetValue(aa, typeof(StatusEffect), newStatusEffect, "m_stackingFamily");

            //statusData.RefreshData();
            return newStatusEffect;
        }
        public static StatusEffectFamily MakeStatusEffectFamiliy(string familyName, StatusEffectFamily.StackBehaviors stackBehavior, int maxStackCount, StatusEffectFamily.LengthTypes lengthType, UID? uid = null, string modGUID = null)
        {
            var effectFamily = new StatusEffectFamily();

            uid = uid ?? (modGUID != null ? TinyUIDManager.MakeUID(familyName, modGUID, "Status Effect Family") : UID.Generate());

            At.SetValue<UID>((UID) uid, typeof(StatusEffectFamily), effectFamily, "m_uid"); ;
            effectFamily.Name = familyName;
            effectFamily.StackBehavior = stackBehavior;
            effectFamily.MaxStackCount = maxStackCount;
            effectFamily.LengthType = lengthType;

            return effectFamily;
        }
        public static void SetNameAndDesc(EffectPreset imbueEffect, string name, string desc)
        {
            ItemLocalization value = new ItemLocalization(name, desc);

            if (At.GetValue(typeof(LocalizationManager), LocalizationManager.Instance, "m_itemLocalization") is Dictionary<int, ItemLocalization> dictionary)
            {
                if (dictionary.ContainsKey(imbueEffect.PresetID))
                    dictionary[imbueEffect.PresetID] = value;
                else
                    dictionary.Add(imbueEffect.PresetID, value);
                //At.SetValue<Dictionary<int, ItemLocalization>>(dictionary, typeof(LocalizationManager), LocalizationManager.Instance, "m_itemLocalization");
            }
        }
        public static AddStatusEffectBuildUp MakeStatusEffectBuildup(Transform effectTransform, string statusEffectName, float buildup)
        {
            //AddStatusEffectBuildUp addedEffect = GetOrMake(effect.transform, effectsTransfromName, true, true).gameObject.AddComponent<AddStatusEffectBuildUp>();
            AddStatusEffectBuildUp addedEffect = effectTransform.gameObject.AddComponent<AddStatusEffectBuildUp>();

            addedEffect.BuildUpValue = buildup;
            addedEffect.Status = ResourcesPrefabManager.Instance.GetStatusEffectPrefab(statusEffectName);
            addedEffect.OverrideEffectCategory = EffectSynchronizer.EffectCategories.Hit;
            //addedEffect.SyncType = Effect.SyncTypes.OwnerSync;

            return addedEffect;
        }
        public static AddStatusEffect MakeStatusEffectChance(Transform effectTransform, string statusEffectName, int chance)
        {
            //AddStatusEffect addedEffect = GetOrMake(effect.transform, effectsTransfromName, true, true).gameObject.AddComponent<AddStatusEffect>();
            AddStatusEffect addedEffect = effectTransform.gameObject.AddComponent<AddStatusEffect>();
            addedEffect.SetChanceToContract(chance);
            addedEffect.Status = ResourcesPrefabManager.Instance.GetStatusEffectPrefab(statusEffectName);
            addedEffect.OverrideEffectCategory = EffectSynchronizer.EffectCategories.Hit;
            return addedEffect;
        }
        public static WeaponDamage MakeWeaponDamage(Transform effectTransform, float baseDamage, float damageScaling, DamageType.Types damageType, float knockback)
        {
            WeaponDamage addedEffect = effectTransform.gameObject.AddComponent<WeaponDamage>();
            addedEffect.WeaponDamageMult = 1+damageScaling;//For some reason, 1 means 0 scaling.
            addedEffect.OverrideDType = damageType;
            addedEffect.Damages = new DamageType[] { new DamageType(damageType, baseDamage) };
            addedEffect.OverrideEffectCategory = EffectSynchronizer.EffectCategories.Hit;
            addedEffect.Knockback = knockback;
            //addedEffect.SyncType = Effect.SyncTypes.OwnerSync;

            return addedEffect;
        }


        public static void MakeAbsorbHealth(Transform effectTransform, float absorbRatio) {
            var addedEffect = effectTransform.gameObject.AddComponent<AddAbsorbHealth>();
            At.SetValue<float>(absorbRatio, typeof(AddAbsorbHealth), addedEffect, "m_healthRatio");
        }

        //public static ImbueEffectPreset MakeImbuePreset(int imbueID, string name, string description, string iconFileName, int? visualEffectID, float flatDamage, float scalingDamage, DamageType.Types damageType, float knockback, string statusEffect, List<Skill> replaceOnSkills, int chanceToContract = 0, int buildUp = 0)
        public static ImbueEffectPreset MakeImbuePreset(int imbueID, string name, string description,
            string iconFileName = null,
            int? visualEffectID = null,
            List<Skill> replaceOnSkills = null,
            string statusEffectName = null,
            int? chanceToContract = null,
            int? buildUp = null,
            float? scalingDamage = null,
            float? flatDamage = null,
            float? knockback = null,
            DamageType.Types? damageType = null
        )
        {
            Dictionary<int, EffectPreset> dictionary = (Dictionary<int, EffectPreset>)At.GetValue(typeof(ResourcesPrefabManager), null, "EFFECTPRESET_PREFABS");
            if (!dictionary.ContainsKey(imbueID))
            {
                GameObject gameObject = new GameObject(imbueID.ToString() + "_" + name.Replace(" ", ""));
                gameObject.SetActive(true);
                UnityEngine.Object.DontDestroyOnLoad(gameObject);

                ImbueEffectPreset newEffect = gameObject.AddComponent<ImbueEffectPreset>();
                newEffect.name = imbueID.ToString() + "_" + name.Replace(" ", "");

                At.SetValue<int>(imbueID, typeof(EffectPreset), newEffect, "m_StatusEffectID");
                At.SetValue<string>(name, typeof(ImbueEffectPreset), newEffect, "m_imbueNameKey");
                At.SetValue<string>(description, typeof(ImbueEffectPreset), newEffect, "m_imbueDescKey");

                if (visualEffectID != null)
                {
                    newEffect.ImbueStatusIcon = ((ImbueEffectPreset)dictionary[(int)visualEffectID]).ImbueStatusIcon;
                    newEffect.ImbueFX = ((ImbueEffectPreset)dictionary[(int)visualEffectID]).ImbueFX;
                }
                if (iconFileName != null)
                {
                    newEffect.ImbueStatusIcon = CustomTexture.MakeSprite(CustomTexture.LoadTexture(iconFileName, 0, false, FilterMode.Point), CustomTexture.SpriteBorderTypes.None);
                }

                SetNameAndDesc(newEffect, name, description);
                dictionary.Add(imbueID, newEffect);

                if (statusEffectName != null && (chanceToContract??0) > 0)
                    MakeStatusEffectChance(TinyGameObjectManager.GetOrMake(newEffect.transform, "Effects", true, true), statusEffectName, (chanceToContract ?? 0));
                if (statusEffectName != null && (buildUp??0) > 0)
                    MakeStatusEffectBuildup(TinyGameObjectManager.GetOrMake(newEffect.transform, "Effects", true, true), statusEffectName, (buildUp ?? 0));
                if ((scalingDamage??0) > 0 || (flatDamage??0) > 0 || (knockback??0) > 0)
                    MakeWeaponDamage(TinyGameObjectManager.GetOrMake(newEffect.transform, "Effects", true, true), (flatDamage ?? 0), (scalingDamage ?? 0), damageType ?? DamageType.Types.Count, (knockback ?? 0));

                foreach (var replaceOnSkill in replaceOnSkills ?? new List<Skill> { })
                {
                    if (replaceOnSkill != null)
                    {
                        replaceOnSkill.GetComponentInChildren<ImbueWeapon>().ImbuedEffect = newEffect;
                    }
                }

                return newEffect;
            }
            return null;

        }

        public static EffectPreset GetEffectPreset(int effectID)
        {
            Dictionary<int, EffectPreset> dictionary = (Dictionary<int, EffectPreset>)At.GetValue(typeof(ResourcesPrefabManager), null, "EFFECTPRESET_PREFABS");
            if (dictionary.ContainsKey(effectID) && dictionary[effectID] is EffectPreset effect)
            {
                return effect;
            }
            return null;
        }

        /// <summary>
        /// Add a status effect to a character for an arbitrary duration rather than the predefined status effect lifespan.
        /// </summary>
        /// <param name="character">Character to add the status effect to</param>
        /// <param name="statusEffect">Status Effect reference to add to the character. This does NOT have to be a freshly instantiated clone.</param>
        /// <param name="duration">The custom duration, measured in seconds.</param>
        public static void AddStatusEffectForDuration(Character character, StatusEffect statusEffect, float duration, Character source = null)
        {
            var oldStatusData = statusEffect.StatusData;
            statusEffect.StatusData = new StatusData(oldStatusData);
            statusEffect.StatusData.LifeSpan = duration;
           
            character.StatusEffectMngr.AddStatusEffect(statusEffect, source);
            statusEffect.StatusData = oldStatusData;
        }

        public static void ChangeEffectPresetDamageTypeData(EffectPreset effect, DamageType.Types originalType, DamageType.Types newType)
        {
            foreach (var punctualDamage in effect.gameObject.GetComponentsInChildren<PunctualDamage>())
            {
                if (punctualDamage is WeaponDamage weaponDamage)
                {
                    if (weaponDamage.OverrideDType == originalType)
                    {
                        weaponDamage.OverrideDType = newType;
                    }
                }
                
                foreach (DamageType damage in punctualDamage.Damages)
                {
                    if (damage.Type == originalType) damage.Type = newType;
                }
            }
        }
    }
}
