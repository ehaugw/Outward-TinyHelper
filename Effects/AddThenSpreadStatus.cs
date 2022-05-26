using System.Collections.Generic;
using System.Linq;

namespace TinyHelper
{
    public class AddThenSpreadStatus : AddStatusEffect
    {

        public float Range;

        protected override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            if (this.AffectController) _affectedCharacter = base.OwnerCharacter;

            if (_affectedCharacter?.StatusEffectMngr == null || Status == null) return;

            if (_affectedCharacter.StatusEffectMngr.HasStatusEffect(Status.IdentifierName))
            {
                List<Character> charsInRange = new List<Character>();
                CharacterManager.Instance.FindCharactersInRange(_affectedCharacter.CenterPosition, Range, ref charsInRange);

                foreach (Character chr in charsInRange.Where(c => !c.IsAlly(this.SourceCharacter)))
                {
                    base.ActivateLocally(chr, _infos);
                }
            }
            {
                base.ActivateLocally(_affectedCharacter, _infos);
            }
        }
    }
}
