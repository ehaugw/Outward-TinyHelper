using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TinyHelper
{
    public class TinyGameObjectManager
    {
        public static Transform GetOrMake(Transform parent, string child, bool setActive, bool dontDestroyOnLoad)
        {
            return parent.Find(child) ?? TinyGameObjectManager.MakeFreshObject(child, setActive, dontDestroyOnLoad, parent).transform;
        }
        public static GameObject InstantiateClone(GameObject sourceGameObject, bool setActive, bool dontDestroyOnLoad)
        {
            return InstantiateClone(sourceGameObject, sourceGameObject.name + "(Clone)", setActive, dontDestroyOnLoad);
        }
        public static GameObject InstantiateClone(GameObject sourceGameObject, string newGameObjectName, bool setActive, bool dontDestroyOnLoad)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(sourceGameObject);
            gameObject.SetActive(setActive);
            gameObject.name = newGameObjectName;
            if (dontDestroyOnLoad) UnityEngine.Object.DontDestroyOnLoad(gameObject);
            return gameObject;
        }

        public static Transform MakeFreshTransform(Transform parent, string child, bool setActive, bool dontDestroyOnLoad)
        {
            return MakeFreshObject(child, setActive, dontDestroyOnLoad, parent: parent).transform;
        }

        public static GameObject MakeFreshObject(string newGameObjectName, bool setActive, bool dontDestroyOnLoad, Transform parent = null)
        {
            GameObject gameObject = new GameObject(newGameObjectName);
            gameObject.SetActive(setActive);

            if (parent != null) gameObject.transform.SetParent(parent);
            
            if (dontDestroyOnLoad)
            {
                RecursiveDontDestroyOnLoad(gameObject.transform);
            }

            return gameObject;
        }

        public static void RecursiveDontDestroyOnLoad(Transform transform)
        {
            Transform parent = transform;

            while (parent.parent != null)
            {
                parent = parent.parent;
            }
            UnityEngine.Object.DontDestroyOnLoad(parent.gameObject);
        }
    }
}
