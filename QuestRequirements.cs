﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyHelper
{
    using CharacterQuestKnowledgeExtensions;
    public class QuestRequirements
    {
        public static bool HasQuestEvent(string questEventUID)
        {
            return QuestEventManager.Instance.GetEventCurrentStack(questEventUID) > 0;
        }

        public static bool HasQuestKnowledge(Character character, int[] questIDs, LogicType logicType, bool inverted=false, bool requireCompleted=false)
        {
            bool result = false;
            if (character?.Inventory?.QuestKnowledge is CharacterQuestKnowledge q)
            {
                var bools = questIDs.Select(x => q.IsItemLearned(x) && (q.IsQuestCompleted(x) || !requireCompleted));
                result = result || (logicType == LogicType.Any ? bools.Any(x => x) : bools.All(x => x));
            }
            return result ^ inverted;
        }

        public static bool HasQuestKnowledgeLocal(Character character, int[] questIDs, LogicType logicType, bool inverted = false, bool requireCompleted = false)
        {
            bool result = false;
            if (character?.Inventory?.QuestKnowledge is CharacterQuestKnowledge q)
            {
                var bools = questIDs.Select(x => q.IsItemLearnedLocal(x) && (q.IsQuestCompletedLocal(x) || !requireCompleted));
                result = result || (logicType == LogicType.Any ? bools.Any(x => x) : bools.All(x => x));
            }
            return result ^ inverted;
        }
    }
}