namespace TinyHelper
{
    using SideLoader;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;


    public class RemoveItemFromInventory : Effect
    {
        public int ItemID;
        public int Amount = 1;
        public bool AffectOwner = false;

        protected override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            if (AffectOwner)
            {
                _affectedCharacter = base.OwnerCharacter;
            }

            if (_affectedCharacter.Inventory is CharacterInventory inventory)
            {
                inventory.RemoveItem(ItemID, Amount);
            }
        }
    }
}
