//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace TinyHelper.Customs
//{
//    public class CustomWeapon : CustomItem
//    {
//        public SwingSoundWeapon? SwingSound;
//        public CustomWeapon(out Item[] ItemReference) : base(out ItemReference)
//        {}

//        override public bool TryApplyCustomItem()
//        {
//            bool baseRes = base.TryApplyCustomItem();
//            if (baseRes && this.ItemReference[0] is Weapon)
//            {
//                ApplySwingSound(this);
//            }
//            return baseRes;
//        }

//        /// <summary>
//        /// Assigns the swing sound from CustomItem.SwingSoundType to the item in CustomItem.ItemReference[0]
//        /// </summary>
//        public static void ApplySwingSound(CustomWeapon customItem)
//        {
//            Weapon item = customItem.ItemReference[0] as Weapon;
//            item.SwingSoundType = customItem.SwingSound??item.SwingSoundType;
//        }
//    }
//}
