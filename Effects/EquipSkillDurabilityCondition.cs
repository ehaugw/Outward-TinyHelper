using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TinyHelper
{
    public class EquipSkillDurabilityCondition : EquipDurabilityCondition
    {
        protected override bool CheckIsValid(Character _affectedCharacter)
        {
            if (transform.parent.parent.gameObject.GetComponent<Skill>() is Skill skill)
            {
                DurabilityRequired = skill.DurabilityCost;
            }
            return base.CheckIsValid(_affectedCharacter);
        }

        public static Skill.ActivationCondition AddToSkill(Skill skill, EquipmentSlot.EquipmentSlotIDs slot)
        {
            var activationConditions = TinyGameObjectManager.GetOrMake(skill.transform, "AdditionalActivationConditions", true, true);
            var gameObj = TinyGameObjectManager.GetOrMake(activationConditions, "EquipSkillDurabilityCondition", true, true).gameObject;

            var condition = new Skill.ActivationCondition();
            var myCondition = gameObj.AddComponent<EquipSkillDurabilityCondition>();
            myCondition.EquipmentSlot = slot;

            condition.Condition = myCondition;

            At.SetValue("A required piece of equipment is missing or too damaged to be used this way.", typeof(Skill.ActivationCondition), condition, "m_defaultMessage");

            List<Skill.ActivationCondition> conditions = ((At.GetValue(typeof(Skill), skill, "m_additionalConditions") as Skill.ActivationCondition[])?.ToList()) ?? new List<Skill.ActivationCondition>();
            conditions.Add(condition);
            At.SetValue(conditions.ToArray(), typeof(Skill), skill, "m_additionalConditions");
            return condition;
        }

        public static Skill.ActivationCondition AddToSkillNotBroken(Skill skill, EquipmentSlot.EquipmentSlotIDs slot)
        {
            var activationConditions = TinyGameObjectManager.GetOrMake(skill.transform, "AdditionalActivationConditions", true, true);
            var gameObj = TinyGameObjectManager.GetOrMake(activationConditions, "EquipDurabilityCondition", true, true).gameObject;

            var condition = new Skill.ActivationCondition();
            var myCondition = gameObj.AddComponent<EquipDurabilityCondition>();
            myCondition.EquipmentSlot = slot;
            myCondition.DurabilityRequired = 0;
            condition.Condition = myCondition;

            At.SetValue("A required piece of equipment is missing or broken.", typeof(Skill.ActivationCondition), condition, "m_defaultMessage");

            List<Skill.ActivationCondition> conditions = ((At.GetValue(typeof(Skill), skill, "m_additionalConditions") as Skill.ActivationCondition[])?.ToList()) ?? new List<Skill.ActivationCondition>();
            conditions.Add(condition);
            At.SetValue(conditions.ToArray(), typeof(Skill), skill, "m_additionalConditions");
            return condition;
        }
    }
}
