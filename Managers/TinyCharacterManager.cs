using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyHelper
{
	using SideLoader;
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

				return component;
			}
		}
	}
}
