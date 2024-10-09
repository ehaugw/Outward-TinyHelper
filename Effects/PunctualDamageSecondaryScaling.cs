using SideLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TinyHelper
{
    public class PunctualDamageSecondaryScaling : PunctualDamage
    {
        public DamageType.Types ScalingToType;
        public DamageType[] BaseDamage;

        public DamageType[] ScaledDamage
        {
            get
            {
                var original = DamageList.CreateEmptyCopy(new DamageList(this.BaseDamage)) + new DamageList(this.BaseDamage);
                var amplified = DamageList.CreateEmptyCopy(new DamageList(this.BaseDamage)) + new DamageList(this.BaseDamage);

                SourceCharacter?.Stats?.GetAmplifiedDamage(new Tag[] { }, ref amplified);
                var difference = amplified.TotalDamage - original.TotalDamage;
                original.Add(new DamageType() { Damage = difference, Type = ScalingToType });

                return original.List.ToArray();
            }
        }

        protected override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            Damages = ScaledDamage;
            base.ActivateLocally(_affectedCharacter, _infos);
        }
    }
}
