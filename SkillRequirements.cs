using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyHelper
{
    public class SkillRequirements
    {
        public static bool SafeHasSkillKnowledge(Character character, int skillID)
        {
            return character?.Inventory?.SkillKnowledge?.IsItemLearned(skillID) ?? false;
        }
    }
}
