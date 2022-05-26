using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinyHelper
{
    public class TinyTagManager
    {
        public static Tag GetOrMakeTag(string name)
        {
            var tag = TagSourceManager.Instance.DbTags.FirstOrDefault(x => x.TagName == name);
            if (tag == Tag.None)
            {
                tag = new Tag(TagSourceManager.TagRoot, name);
                tag.SetTagType(Tag.TagTypes.Custom);

                TagSourceManager.Instance.DbTags.Add(tag);
                TagSourceManager.Instance.RefreshTags(true);
                return tag;
            }
            else
            {
                return tag;
            }
        }

        public static String[] GetOrMakeTags(string[] names)
        {
            foreach (var name in names)
            {
                GetOrMakeTag(name);
            }
            return names;
        }

        public static String[] GetSafeTags(String[] tags)
        {
            List<string> newTags = new List<string>();

            foreach (string tag in tags)
            {
                if (TagSourceManager.Instance.DbTags.FirstOrDefault(x => x.TagName == tag || x.UID == tag) != null)
                {
                    newTags.Add(tag);
                }
            }
            return newTags.ToArray();
        }
    }
}
