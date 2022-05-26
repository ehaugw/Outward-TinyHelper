using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TinyHelper
{
    public class ExtendedAddStatusEffect : AddStatusEffect
    {
        public bool ExtendDuration = false;

        protected override void ActivateLocally(Character _targetCharacter, object[] _infos)
        {
            var _statusEffect = this.Status;
            if (ExtendDuration && (_targetCharacter?.StatusEffectMngr?.HasStatusEffect(_statusEffect.IdentifierName) ?? false))
            {
                var oldStatusData = _statusEffect.StatusData;
                _statusEffect.StatusData = new StatusData(oldStatusData);

                float remainingExisting = _targetCharacter.StatusEffectMngr.GetStatusEffectOfName(_statusEffect.IdentifierName)?.RemainingLifespan ?? 0;
                float remainingNew = _statusEffect.RemainingLifespan > 0 ? _statusEffect.RemainingLifespan : _statusEffect.StartLifespan;
                _statusEffect.StatusData.LifeSpan = remainingNew + remainingExisting;

                base.ActivateLocally(_targetCharacter, _infos);

                _statusEffect.StatusData = oldStatusData;
            } else
            {
                base.ActivateLocally(_targetCharacter, _infos);
            }
        }
    }
}
