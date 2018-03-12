using System;
using System.Collections.Generic;
using System.Text;

namespace SM64ModelImporter
{
    public class TextureLibrary : ReadWrite
    {
        public Dictionary<string, TextureImage> textures = new Dictionary<string, TextureImage>();

        public void Remove(TextureImage image)
        {
            string key = null;
            foreach (KeyValuePair<string, TextureImage> a in textures)
                if (a.Value == image)
                {
                    key = a.Key;
                    break;
                }
            if (key != null)
                textures.Remove(key);
        }

        public void SaveSettings(FileParser.Block block)
        {
            foreach (KeyValuePair<string, TextureImage> texture in textures)
                block.SetString("Texture " + texture.Key, texture.Value.format.ToString() + ", " + texture.Value.bpp.ToString());
        }

        public void LoadSettings(FileParser.Block block)
        {
            foreach (KeyValuePair<string, TextureImage> texture in textures)
            {
                string settings = block.GetString("Texture " + texture.Key, false);
                string[] split = settings.Split(',');
                try
                {
                    texture.Value.format = (TextureImage.TextureFormat)Enum.Parse(typeof(TextureImage.TextureFormat), split[0].Trim(), true);
                }
                catch { }
                if (split.Length > 1)
                    try
                    {
                        texture.Value.bpp = (TextureImage.BitsPerPixel)Enum.Parse(typeof(TextureImage.BitsPerPixel), split[1].Trim(), true);
                    }
                    catch { }
            }
        }
    }
}
