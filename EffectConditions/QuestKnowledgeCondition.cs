using NodeCanvas.BehaviourTrees;
using System.Linq;
using UnityEngine;

namespace TinyHelper
{
    public class QuestKnowledgeCondition : EffectCondition
    {
        public int[] Quests;
        public LogicType Logic = LogicType.Any;
        
        protected override bool CheckIsValid(Character _affectedCharacter)
        {
            return QuestRequirements.HasQuestKnowledge(_affectedCharacter, Quests, Logic);
        }
    }
}
