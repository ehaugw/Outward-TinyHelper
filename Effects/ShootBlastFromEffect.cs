using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TinyHelper.Effects
{
    public class ShootBlastFromEffect : ShootBlast
    {
        public override void Setup(TargetingSystem _targetSystem, Transform _parent)
        {
            base.Setup(SourceCharacter?.TargetingSystem ?? _targetSystem, _parent);
        }

        //protected override void ActivateLocally(Character _targetCharacter, object[] _infos)
        //{
        //    _infos[0] = _targetCharacter.transform.position + LocalCastPositionAdd;
        //    _infos[1] = Vector3.zero;
        //    base.ActivateLocally(_targetCharacter, _infos);
        //}
    }
}
