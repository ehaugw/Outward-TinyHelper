using SideLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TinyHelper
{
    public class CooldownChangeWeaponDamageTargetHealth : WeaponDamageTargetHealth
    {
        public float ExecuteSetCooldown = -1;
        public float HitKnockbackCooldown = -1;

        protected override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            if (ParentItem is Skill skill)
            {
                if (HitKnockbackCooldown != -1 && _affectedCharacter.IsInKnockback)
                {
                    At.SetValue(HitKnockbackCooldown, typeof(Skill), skill, "m_remainingCooldownTime");
                }

                bool wasDead = _affectedCharacter.IsDead;
                base.ActivateLocally(_affectedCharacter, _infos);
                if (!wasDead && _affectedCharacter.IsDead && ExecuteSetCooldown != -1)
                {
                    At.SetValue(ExecuteSetCooldown, typeof(Skill), skill, "m_remainingCooldownTime");
                }

            }

        }
    }
}
