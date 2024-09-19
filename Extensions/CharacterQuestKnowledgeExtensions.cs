using System.Linq;

namespace CharacterQuestKnowledgeExtensions
{
    public static class CharacterQuestKnowledgeExtensions
    {
        public static bool IsItemLearnedLocal(this CharacterQuestKnowledge questKnowledge, int _itemID)
        {
            return questKnowledge.GetLearnedItems().Where(q => q.ItemID == _itemID && q.OwnerCharacter).Count() > 0;
        }

        public static bool IsQuestCompletedLocal(this CharacterQuestKnowledge questKnowledge, int _itemID)
        {
            return questKnowledge.GetLearnedItems().Where(q => q.ItemID == _itemID && q.OwnerCharacter && q is Quest quest && quest.IsCompleted).Count() > 0;
        }
    }
}