namespace TinyHelper
{
	using UnityEngine;

	public class TinyCharacterManager
    {
		public static Character SpawnCharacter(string uid, Vector3 position, Vector3 rotation)
		{

			Character character = CharacterManager.Instance.GetCharacter(uid);
			if (character != null)
			{
				return character;
			}
			else
			{

				object[] data = new object[]
				{
					4,
					"NewPlayerPrefab",
					uid,
					string.Empty
				};

				GameObject gameObject = PhotonNetwork.InstantiateSceneObject("_characters/NewPlayerPrefab", position, Quaternion.Euler(rotation), 0, data);
				gameObject.SetActive(false);
				Character component = gameObject.GetComponent<Character>();
				// this is where you can set it to not be AI
				component.SetUID(uid);
				At.SetValue<Character>(component, typeof(int), 1, "m_isAI");

    //            // fix Photon View component
    //            if (character.gameObject.GetComponent<PhotonView>() is PhotonView view)
    //            {
    //                int id = view.viewID;
    //                GameObject.DestroyImmediate(view);

    //                if (!PhotonNetwork.isNonMasterClientInRoom && id > 0)
    //                    PhotonNetwork.UnAllocateViewID(view.viewID);
    //            }

    //            // setup new view
    //            PhotonView pView = character.gameObject.AddComponent<PhotonView>();
    //            pView.ownerId = 0;
    //            pView.ownershipTransfer = OwnershipOption.Fixed;
    //            pView.onSerializeTransformOption = OnSerializeTransform.PositionAndRotation;
    //            pView.onSerializeRigidBodyOption = OnSerializeRigidBody.All;
    //            pView.synchronization = ViewSynchronization.Off;

    //            // fix photonview serialization components
    //            if (pView.ObservedComponents == null || pView.ObservedComponents.Count < 1)
    //                pView.ObservedComponents = new List<Component>() { character };

				//// set view ID last.
				//pView.viewID = PhotonNetwork.AllocateSceneViewID();

                return component;
			}
		}
	}
}
