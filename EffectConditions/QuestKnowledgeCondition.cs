using System.Linq;
using UnityEngine;

namespace TinyHelper
{
    public class QuestKnowledgeCondition : EffectCondition
    {
        public int[] Quests;
        public enum LogicType
        {
            Any,
            All
        };
        public LogicType Logic = LogicType.Any;

        protected override bool CheckIsValid(Character _affectedCharacter)
        {
            if (_affectedCharacter?.Inventory?.QuestKnowledge is CharacterQuestKnowledge q)
            {
                var bools = Quests.Select(x => q.IsItemLearned(x));
                return (Logic == LogicType.Any ? bools.Any(x => x) : bools.All(x => x));
            }
            return false;
        }
    }
}
