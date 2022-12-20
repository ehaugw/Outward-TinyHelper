
using System.Linq;
using UnityEngine;

namespace TinyHelper
{
    public class AddNewStatusEffectRandom : AddStatusEffectRandom
    {
        protected override bool TryTriggerConditions()
        {
            var _statuses = Statuses.ToList();
            TinyHelpers.Shuffle(_statuses);

            ForceID = -1;
            for (int i = 0; i < _statuses.Count; i++)
            {
                var status = _statuses[i];

                if (status == null || m_affectedCharacter == null)
                {
                    continue;
                }

                if (!m_affectedCharacter.StatusEffectMngr.HasStatusEffect(status.EffectFamily))
                {
                    ForceID = Statuses.IndexOf(status);
                    break;
                }
            }

            return base.TryTriggerConditions();
        }
    }
}
