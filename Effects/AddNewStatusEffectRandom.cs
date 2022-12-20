
using System.Linq;
using UnityEngine;

namespace TinyHelper
{
    public class AddNewStatusEffectRandom : AddStatusEffectRandom
    {
        protected override bool TryTriggerConditions()
        {
            Debug.Log("before shuffle");
            var _statuses = Statuses.ToList();
            TinyHelpers.Shuffle(_statuses);
            Debug.Log("after shuffle");

            Debug.Log("before selection");
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
            Debug.Log("after selection");


            return base.TryTriggerConditions();
        }
        //protected override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        //{
           
        //}
    }
}
