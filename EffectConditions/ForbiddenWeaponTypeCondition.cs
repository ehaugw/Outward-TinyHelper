using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TinyHelper
{
    public class ForbiddenWeaponTypeCondition: EffectCondition
    {
        public List<Weapon.WeaponType> ForbiddenWeaponTypes = new List<Weapon.WeaponType> { };
        protected override bool CheckIsValid(Character _affectedCharacter)
        {
            return !(_affectedCharacter?.LeftHandEquipment is Weapon weaponLeft && ForbiddenWeaponTypes.Contains(weaponLeft.Type)) && !(_affectedCharacter?.CurrentWeapon is Weapon weaponRight && ForbiddenWeaponTypes.Contains(weaponRight.Type));
        }

        public static Skill.ActivationCondition AddToSkill(Skill skill, List<Weapon.WeaponType> forbiddenWeaponTypes)
        {
            var gameObj = new GameObject("ForbiddenWeaponTypeCondition");

            var condition = new Skill.ActivationCondition();
            var myCondition = gameObj.AddComponent<ForbiddenWeaponTypeCondition>();
            DontDestroyOnLoad(myCondition);
            gameObj.SetActive(false);

            condition.Condition = myCondition;
            myCondition.ForbiddenWeaponTypes= forbiddenWeaponTypes;

            At.SetValue("Can not be used with a " + forbiddenWeaponTypes.OrConcat() + ".", typeof(Skill.ActivationCondition), condition, "m_defaultMessage");

            List<Skill.ActivationCondition> conditions = ((At.GetValue(typeof(Skill), skill, "m_additionalConditions") as Skill.ActivationCondition[])?.ToList()) ?? new List<Skill.ActivationCondition>();
            conditions.Add(condition);
            At.SetValue(conditions.ToArray(), typeof(Skill), skill, "m_additionalConditions");
            return condition;
        }
    }
}
