//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;

//namespace TinyHelper.Customs
//{
//    public class CustomAsset
//    {
//        public static void ApplyVisuals(CustomItem customItem)
//        {
//            //fetch the item
//            Item item = customItem.ItemReference[0];

//            //load the asset bundle
//            AssetBundle assetBundle = LoadAssetBundle(customItem.PathPrefix + customItem.AssetBundlePath);

//            //make new transform for the visual, and set it to remain during scene change
//            Transform transform = UnityEngine.Object.Instantiate<Transform>(item.VisualPrefab);
//            transform.gameObject.SetActive(false);
//            UnityEngine.Object.DontDestroyOnLoad(transform);
            
//            //assign the transform to the target item
//            item.VisualPrefab = transform;


//            GameObject meshGameObject = assetBundle.LoadAsset<GameObject>(customItem.ItemVisuals_PrefabName);
//            if (meshGameObject != null)
//            {
//                foreach (Transform transform2 in transform)
//                {
//                    if (transform2.GetComponent<BoxCollider>() && transform2.GetComponent<MeshRenderer>())
//                    {
//                        transform2.gameObject.SetActive(false);
//                    }
//                }

//                GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(meshGameObject);
//                gameObject3.transform.parent = transform.transform;

//                if (item is Weapon)
//                {
//                    gameObject3.transform.rotation = Quaternion.Euler(0, -90, 0);
//                }
//            }
//        }

//        /// <summary>
//        /// Loads an asset bundle and inserts it into the cache directory before return.
//        /// </summary>
//        /// <param name="assetBundlePath">The path is appended to plugins path.</param>
//        /// <returns></returns>
//        public static AssetBundle LoadAssetBundle(string assetBundlePath)
//        {
//            string fullpath = TinyHelper.PLUGIN_ROOT_PATH + assetBundlePath;

//            if (!AssetBundles.ContainsKey(fullpath)) AssetBundles.Add(fullpath, AssetBundle.LoadFromFile(fullpath));
//            return AssetBundles[fullpath];
//        }

//        /// <summary>
//        /// A null-safe dictionary containing the loaded asset bundles.
//        /// </summary>
//        public static Dictionary<string, AssetBundle> AssetBundles
//        {
//            get
//            {
//                if (m_assetBundles == null)
//                    m_assetBundles = new Dictionary<string, AssetBundle>();
//                return m_assetBundles;
//            }
//        }
//        static private Dictionary<string, AssetBundle> m_assetBundles;


//    }
//}
