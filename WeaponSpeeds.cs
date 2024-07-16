using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinyHelper
{
    public class WeaponManager
    {
        public static Dictionary<Weapon.WeaponType, float> Speeds = new Dictionary<Weapon.WeaponType, float>
        {
            { Weapon.WeaponType.Sword_1H,   1.251f},
            { Weapon.WeaponType.Axe_1H,     1.399f},
            { Weapon.WeaponType.Mace_1H,    1.629f},
            { Weapon.WeaponType.Sword_2H,   1.710f},
            { Weapon.WeaponType.Axe_2H,     1.667f},
            { Weapon.WeaponType.Mace_2H,    2.036f},
            { Weapon.WeaponType.Spear_2H,   1.499f},
            { Weapon.WeaponType.Halberd_2H, 1.612f}
        };

        public static Dictionary<Weapon.WeaponType, float> HeavyImpactModifiers = new Dictionary<Weapon.WeaponType, float>
        {
            { Weapon.WeaponType.Sword_1H,   1.3f},
            { Weapon.WeaponType.Axe_1H,     1.3f},
            { Weapon.WeaponType.Mace_1H,    2.5f},
            { Weapon.WeaponType.Sword_2H,   1.5f},
            { Weapon.WeaponType.Axe_2H,     1.3f},
            { Weapon.WeaponType.Mace_2H,    2.0f},
            { Weapon.WeaponType.Spear_2H,   1.2f},
            { Weapon.WeaponType.Halberd_2H, 1.3f}
        };
    }
}
