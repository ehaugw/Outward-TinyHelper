using SideLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace TinyHelper
{
    public class ToggleEffect : Effect
    {
        public StatusEffect StatusEffectInstance;

        protected override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            if (_affectedCharacter.StatusEffectMngr.HasStatusEffect(StatusEffectInstance.IdentifierName))
            {
                _affectedCharacter.StatusEffectMngr.CleanseStatusEffect(StatusEffectInstance.name);
            }
            else
            {
                _affectedCharacter.StatusEffectMngr.AddStatusEffect(StatusEffectInstance, this.SourceCharacter);
            }
        }
    }
}
