namespace TinyHelper
{
    using SideLoader;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;


    public class UseMana : Effect
    {

        public float UsedMana = 0;

        protected override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            _affectedCharacter.Stats.UseMana(null, UsedMana);
        }
    }
}
