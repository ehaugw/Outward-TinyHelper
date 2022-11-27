using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TinyHelper
{
    public class SpecificImbueCondition : EffectCondition
    {
        public ImbueEffectPreset imbueEffectPreset;
        bool inverse = false;

        protected override bool CheckIsValid(Character _affectedCharacter)
        {
            return _affectedCharacter?.CurrentWeapon is Weapon weapon && weapon.HasImbuePreset(imbueEffectPreset);
        }

        public static Skill.ActivationCondition AddToSkill(Skill skill, ImbueEffectPreset imbueEffectPreset, bool inverse = false)
        {
            var gameObj = new GameObject("SpecificImbueCondition");

            var condition = new Skill.ActivationCondition();
            var myCondition = gameObj.AddComponent<SpecificImbueCondition>();
            DontDestroyOnLoad(myCondition);
            gameObj.SetActive(false);

            condition.Condition = myCondition;
            myCondition.inverse = inverse;
            myCondition.imbueEffectPreset = imbueEffectPreset;

            At.SetValue("You do not have the required imbue effect.", typeof(Skill.ActivationCondition), condition, "m_defaultMessage");

            List<Skill.ActivationCondition> conditions = ((At.GetValue(typeof(Skill), skill, "m_additionalConditions") as Skill.ActivationCondition[])?.ToList()) ?? new List<Skill.ActivationCondition>();
            conditions.Add(condition);
            At.SetValue(conditions.ToArray(), typeof(Skill), skill, "m_additionalConditions");
            return condition;
        }
    }
}
