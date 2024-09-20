using InstanceIDs;
using System.Collections.Generic;
using System.Linq;
using TinyHelper;

namespace CharacterExtensions
{
    public static class CharacterExtensions
    {
        public static List<Item> EquippedOnBag(this Character character)
        {
            return character?.Inventory?.GetOwnedItems(TinyTagManager.GetOrMakeTag(IDs.EquipmentTag))?.Where(x => x.DisplayedOnBag)?.ToList();
        }
    }
}