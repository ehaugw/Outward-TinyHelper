namespace TinyHelper
{
    using SideLoader;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;


    public class AffectCorruption : Effect
    {

        public float AffectQuantity = 0;
        public bool AffectOwner = false;
        public bool IsRaw = false;
        protected override KeyValuePair<string, Type>[] GenerateSignature()
        {
            return new KeyValuePair<string, Type>[1]
            {
            new KeyValuePair<string, Type>("Value", typeof(float))
            };
        }

        public override void SetValue(string[] _data)
        {
            if (_data == null || _data.Length >= 1)
            {
                float.TryParse(_data[0], out AffectQuantity);
            }
        }

        protected override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            if (AffectOwner)
            {
                _affectedCharacter = base.OwnerCharacter;
            }

            if (_affectedCharacter != null && _affectedCharacter.Alive && (bool)_affectedCharacter.PlayerStats)
            {
                _affectedCharacter.PlayerStats.AffectCorruptionLevel(AffectQuantity, !IsRaw);
            }
        }
    }
}
