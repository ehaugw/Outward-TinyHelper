//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace TinyHelper.Customs
//{
//    public class CustomTag
//    {
//        /// <summary>
//        /// Convert tag names to tags.
//        /// </summary>
//        /// <param name="tags">A list of strings containing the names of the desired tags.</param>
//        /// <returns>Returns a list of tags containing the tags that matched the strings in the input list.</returns>
//        public static List<Tag> SafeGetTagsFromStrings(List<string> tags)
//        {
//            TinyHelper.TinyHelperPrint("Input tag list: " + tags);
//            var l = TagSourceManager.Instance.DbTags.Where(tag => tags.Contains(tag.TagName)).ToList();
//            TinyHelper.TinyHelperPrint("Output tag list count " + l.Count());

//            return l;
//        }

//        public static List<String> GetSafeTags(List<String> tags)
//        {
//            List<string> newTags = new List<string>();

//            foreach (string tag in tags)
//            {
//                if (TagSourceManager.Instance.DbTags.FirstOrDefault(x => x.TagName == tag || x.UID == tag) != null)
//                {
//                    newTags.Add(tag);
//                }
//            }
//            return newTags;
//        }

//        /// <summary>
//        /// Applies, replaces or overrides tags from CustomItem.Tags depending on the CustomItem.TagBehaviour.
//        /// </summary>
//        /// <param name=""></param>
//        public static void ApplyTags(CustomItem customItem)
//        {
//            Item item = customItem.ItemReference[0];

//            TagSource tagSource = item.gameObject.GetComponent<TagSource>();

//            //Destroy tags if purge
//            if (customItem.TagBehaviour == Behaviour.Purge && tagSource != null)
//            {
//                UnityEngine.Object.Destroy(tagSource);
//                tagSource = null;
//            }

//            if (customItem.Tags == null) return;

//            //Add a tag component if not existing
//            if (tagSource == null)
//                tagSource = item.gameObject.AddComponent<TagSource>();

//            //Make tag list depending on tab behaviour
//            List<TagSourceSelector> tags = At.GetValue(typeof(TagListSelectorComponent), tagSource, "m_tagSelectors") as List<TagSourceSelector>;

//            foreach (Tag tag in CustomTag.SafeGetTagsFromStrings(customItem.Tags))
//                tags.Add(new TagSourceSelector(tag));

//            tagSource.RefreshTags();
//            At.SetValue(tagSource, typeof(Item), item, "m_tagSource");
//        }
//    }
//}
