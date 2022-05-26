using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TinyHelper
{
    public class CustomTexture
    {
        public enum SpriteBorderTypes
        {
            None,
            Item,
            TrainerSkill
        }

        public enum IconName
        {
            m_itemIcon,
            SkillTreeIcon
        }

        /// <summary>
        /// Makes a Texture2D from the image in the specified path
        /// </summary>
        /// <param name="filePath">The path is appended to plugin path, and must include file endings.</param>
        /// <returns></returns>
        public static Texture2D LoadTexture(string filePath, int mipCount, bool linear, FilterMode filterMode, float? mipMapBias = null)
        {
            filePath = TinyHelper.PLUGIN_ROOT_PATH + filePath;

            //Only open image once
            if (!LoadedTextures.ContainsKey(filePath)) LoadedTextures[filePath] = File.ReadAllBytes(filePath);

            //set correct texture type
            Texture2D texture = new Texture2D(4, 4, TextureFormat.DXT5, mipCount > 0, linear);
            texture.filterMode = filterMode;

            //Set the correct mipmap bias
            texture.mipMapBias = mipMapBias ?? texture.mipMapBias;

            texture.LoadImage(LoadedTextures[filePath]);
            texture.filterMode = FilterMode.Bilinear;

            //fileData = default;
            //GC.Collect();

            //load raw data into texture
            //texture.LoadRawTextureData(LoadedTextures[filePath]);

            return texture;
        }

        /// <summary>
        /// A null-safe dictionary containing the byde arrays of loaded images.
        /// </summary>
        private static Dictionary<string, byte[]> LoadedTextures
        {
            get
            {
                if (m_loadedTextures == null) m_loadedTextures = new Dictionary<string, byte[]>();
                return m_loadedTextures;
            }
        }
        private static Dictionary<string, byte[]> m_loadedTextures;

        /// <summary>
        /// Returns a sprite of the provided texture.
        /// </summary>
        /// <param name="texture">A texture that has been made from a byte array.</param>
        /// <param name="spriteBorderType">Border offsets to compensate for 9D frames. Incorrect value results in blury texture.</param>
        /// <returns></returns>
        public static Sprite MakeSprite(Texture2D texture, SpriteBorderTypes spriteBorderType)
        {
            float x0, x1, y0, y1;

            switch (spriteBorderType)
            {
                case SpriteBorderTypes.Item:
                    x0 = 1;
                    x1 = 2;
                    y0 = 2;
                    y1 = 3;
                    break;
                case SpriteBorderTypes.TrainerSkill:
                    x0 = 1;
                    x1 = 1;
                    y0 = 1;
                    y1 = 2;
                    break;
                default:
                    x0 = 0;
                    x1 = 0;
                    y0 = 0;
                    y1 = 0;
                    break;
            }

            Sprite sprite = Sprite.Create(texture, new Rect(x0, y0, (float)texture.width - x1, (float)texture.height - y1), new Vector2(0f, 0f), 100.0f, 0);
            return sprite;
        }

        ///// <summary>
        ///// Assigns a sprite to the m_itemIcon variable of the CustomItem.ReferenceItem
        ///// </summary>
        //public static void ApplyItemIcon(CustomItem customItem)
        //{
        //    if (customItem.IconPath != null)
        //    {
        //        Texture2D texture = CustomTexture.LoadTexture(customItem.PathPrefix + customItem.IconPath, 0, false, FilterMode.Point);
        //        Sprite sprite = CustomTexture.MakeSprite(texture, SpriteBorderTypes.Item);
        //        At.SetValue(sprite, typeof(Item), customItem.ItemReference[0], "m_itemIcon");
        //    }
        //}
    }
}
