using SideLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TinyHelper
{
    public class ExecutionWeaponDamage : WeaponDamage
    {
        public float SetCooldown = -1;

        protected override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        { 
            bool wasDead = _affectedCharacter.IsDead;
            base.ActivateLocally(_affectedCharacter, _infos);
            if (!wasDead && _affectedCharacter.IsDead && ParentItem is Skill skill)
            {
                if (SetCooldown != -1)
                {
                    At.SetValue(SetCooldown, typeof(Skill), skill, "m_remainingCooldownTime");
                }
            }
        }
    }
}
