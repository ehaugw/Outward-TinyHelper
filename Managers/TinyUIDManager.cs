using UnityEngine;

namespace TinyHelper
{
    public class TinyUIDManager
    {
        public static UID MakeUID(string name, string modGUID, string category)
        {
            if (modGUID == null || name == null || category == null)
            {
                Debug.LogError(string.Format("TinyUIDManager.MakeUID({0}, {1}, {2} returned a random UID. This will cause trouble in multiplayer!", name, modGUID, category));
                return new UID();
            } else
            {
                return new UID((modGUID + "." + category + "." + name).Replace(" ", "").ToLower());
            }
        }
    }
}
