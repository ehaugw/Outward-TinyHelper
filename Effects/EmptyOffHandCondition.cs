using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TinyHelper
{
    public class EmptyOffHandCondition : EffectCondition
    {
        public bool AllowDrawnTwoHandedInRight = false;
        public bool AllowSheathedTwoHandedInLeft = false;
        protected override bool CheckIsValid(Character _affectedCharacter)
        {
            return
                _affectedCharacter == null
                || (_affectedCharacter.LeftHandEquipment == null && !(_affectedCharacter?.CurrentWeapon?.TwoHanded ?? false)) //this means nothing is held in left hand
                || (!_affectedCharacter.Sheathed && AllowDrawnTwoHandedInRight && (_affectedCharacter.CurrentWeapon?.TwoHandedRight ?? true)) //two handers are OK while drawn
                || (_affectedCharacter.Sheathed && AllowSheathedTwoHandedInLeft && (_affectedCharacter.CurrentWeapon?.TwoHandedRight ?? true)); //two handers are OK while sheathed

        }

        public static Skill.ActivationCondition AddToSkill(Skill skill, bool allowDrawnTwoHandedInRight = false, bool allowSheathedTwoHandedInLeft = false)
        {
            var gameObj = new GameObject("EmptyOffhandCondition");

            var condition = new Skill.ActivationCondition();
            var myCondition = gameObj.AddComponent<EmptyOffHandCondition>();
            DontDestroyOnLoad(myCondition);
            gameObj.SetActive(false);

            condition.Condition = myCondition;
            myCondition.AllowDrawnTwoHandedInRight = allowDrawnTwoHandedInRight;
            myCondition.AllowSheathedTwoHandedInLeft = allowSheathedTwoHandedInLeft;

            At.SetValue("Requires an empty left hand.", typeof(Skill.ActivationCondition), condition, "m_defaultMessage");

            List<Skill.ActivationCondition> conditions = ((At.GetValue(typeof(Skill), skill, "m_additionalConditions") as Skill.ActivationCondition[])?.ToList()) ?? new List<Skill.ActivationCondition>();
            conditions.Add(condition);
            At.SetValue(conditions.ToArray(), typeof(Skill), skill, "m_additionalConditions");
            return condition;
        }
    }
}
