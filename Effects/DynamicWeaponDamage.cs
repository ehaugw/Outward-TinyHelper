using Discord;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyHelper.Interfaces;
using uNature.Core.Sectors;

namespace TinyHelper.Effects
{
    public class DynamicWeaponDamage : WeaponDamage
    {
        public IDamageScaler DamageScaler = null;
        
        private void Recalculate()
        {
            Console.WriteLine("recalculated");
            Debug.WriteLine("recalculated");
            WeaponDamageMult = DamageScaler.GetWeaponDamageMult(this);
            OverrideDType = DamageScaler.GetOverrideDType(this);
            Damages = DamageScaler.GetDamages(this);
            Knockback = DamageScaler.GetKnockback(this);

        }
        protected override void StartInit()
        {
            Recalculate();
            base.StartInit();
        }

        public override Weapon BuildDamage(Character _targetCharacter, ref DamageList _list, ref float _knockback)
        {
            Recalculate();
            return base.BuildDamage(_targetCharacter, ref _list, ref _knockback);
        }

        protected override void BuildDamage(Weapon _weapon, Character _targetCharacter, bool _isSkillOrShield, ref DamageList _list, ref float _knockback)
        {
            Recalculate();
            base.BuildDamage(_weapon, _targetCharacter, _isSkillOrShield, ref _list, ref _knockback);
        }

        protected override void ActivateLocally(Character _targetCharacter, object[] _infos)
        {
            Recalculate();
            base.ActivateLocally(_targetCharacter, _infos);
        }

        protected override DamageList DealHit(Character _targetCharacter)
        {
            Recalculate();
            return base.DealHit(_targetCharacter);
        }

        public override float DamageMult(Character _targetCharacter, bool _isSkill)
        {
            Recalculate();
            return base.DamageMult(_targetCharacter, _isSkill);
        }

        public override float KnockbackMult(Character _targetCharacter, bool _isSkill)
        {
            Recalculate();
            return base.KnockbackMult(_targetCharacter, _isSkill);
        }
    }
}
