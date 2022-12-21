using System.Linq;
using UnityEngine;

namespace TinyHelper
{
    public class StatusEffectsCondition : EffectCondition
    {
        public string[] StatusEffectNames;
        public enum LogicType
        {
            Any,
            All
        };
        public LogicType Logic = LogicType.Any;

        protected override bool CheckIsValid(Character _affectedCharacter)
        {
            if (_affectedCharacter?.StatusEffectMngr is StatusEffectManager statusEffectManager)
            {
                var bools = StatusEffectNames.Select(x => statusEffectManager.GetStatusEffectOfName(x) != null);
                return (Logic == LogicType.Any ? bools.Any(x => x) : bools.All(x => x));

            }
            return false;
        }
    }
}
