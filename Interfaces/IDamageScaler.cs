using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyHelper.Interfaces
{
    public interface IDamageScaler
    {
        float GetWeaponDamageMult(WeaponDamage weaponDamage);
        DamageType.Types GetOverrideDType(WeaponDamage weaponDamage);
        DamageType[] GetDamages(WeaponDamage weaponDamage);
        float GetKnockback(WeaponDamage weaponDamage);
    }
}
