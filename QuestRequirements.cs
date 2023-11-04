using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyHelper
{
    public class QuestRequirements
    {
        public static bool HasQuestKnowledge(Character character, int[] questIDs, LogicType logicType, bool inverted=false)
        {
            bool result = false;
            if (character?.Inventory?.QuestKnowledge is CharacterQuestKnowledge q)
            {
                var bools = questIDs.Select(x => q.IsItemLearned(x));
                result = result || (logicType == LogicType.Any ? bools.Any(x => x) : bools.All(x => x));
            }
            return result ^ inverted;
        }
    }
}
