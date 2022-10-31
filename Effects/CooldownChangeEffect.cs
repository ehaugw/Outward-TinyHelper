namespace TinyHelper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Discord;

    public class CooldownChangeEffect : Effect
    {
        public float HitKnockbackCooldown = -1;

        protected override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            if (ParentItem is Skill skill && (HitKnockbackCooldown != -1 && _affectedCharacter.IsInKnockback))
            {
                At.SetValue(HitKnockbackCooldown, typeof(Skill), skill, "m_remainingCooldownTime");
            }
        }
    }
}
