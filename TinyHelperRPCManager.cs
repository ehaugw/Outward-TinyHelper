using InstanceIDs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TinyHelper
{
    public class TinyHelperRPCManager : Photon.MonoBehaviour
    {
        public static TinyHelperRPCManager Instance;

        internal void Start()
        {
            Instance = this;

            var view = this.gameObject.AddComponent<PhotonView>();
            view.viewID = IDs.TinyHelperRPCPhotonID;
            Debug.Log("Registered TinyHelpertRPC with ViewID " + this.photonView.viewID);
        }

        [PunRPC]
        public void ApplyAddImbueEffectRPC(string weaponGUID, int infusionID, float duration)
        {
            Weapon weapon = ItemManager.Instance.GetItem(weaponGUID) as Weapon;
            weapon.AddImbueEffect(ResourcesPrefabManager.Instance.GetEffectPreset(infusionID) as ImbueEffectPreset, duration);
        }

        [PunRPC]
        public void CharacterForceCancelRPC(string characterGUID, bool bool1, bool bool2)
        {
            Character character = CharacterManager.Instance.GetCharacter(characterGUID);
            character.ForceCancel(bool1, bool2);
        }
    }
}
