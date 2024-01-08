using BepInEx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace ShivrerCompany.util
{
    internal class TextureUtil
    {
        public static string
            WALKIETALKIE_TEX,
            WALKIETALKIE_SCREEN_TEX,
            WALKIETALKIE_ICON,
            FLASHLIGHT_TEX,
            FLASHLIGHT_ICON,
            TINY_FLASHLIGHT_TEX,
            TINY_FLASHLIGHT_ICON,
            GIFTBOX_TEX,
            GIFTBOX_ICON,
            AIRHORN_TEX,
            SODACAN_TEX,
            STOPSIGN_TEX,
            YIELDSIGN_TEX,
            POSTERS_TEX;

        public static void LoadTexturePaths()
        {
            string[] files = Directory.GetFiles(Path.Combine(Paths.PluginPath, "ShivrerAssets"));

            WALKIETALKIE_TEX = FindTexturePath(files, "WalkieTalkieTex.png");
            WALKIETALKIE_SCREEN_TEX = FindTexturePath(files, "WalkieTalkieScreenEmission.png");
            WALKIETALKIE_ICON = FindTexturePath(files, "WalkieTalkieIcon.png");
            FLASHLIGHT_TEX = FindTexturePath(files, "FlashlightTexture.png");
            FLASHLIGHT_ICON = FindTexturePath(files, "FlashlightIcon.png");
            TINY_FLASHLIGHT_TEX = FindTexturePath(files, "TinyFlashlightTex.png");
            TINY_FLASHLIGHT_ICON = FindTexturePath(files, "TinyFlashlightIcon.png");
            GIFTBOX_TEX = FindTexturePath(files, "GiftBoxTex.png");
            GIFTBOX_ICON = FindTexturePath(files, "GiftBoxIcon.png");
            AIRHORN_TEX = FindTexturePath(files, "AirhornTex.png");
            SODACAN_TEX = FindTexturePath(files, "SodaCanTex1.png");
            STOPSIGN_TEX = FindTexturePath(files, "StopSignTex.png");
            YIELDSIGN_TEX = FindTexturePath(files, "YieldSignTex.png");
            POSTERS_TEX = FindTexturePath(files, "posters.png");
        }

        public static string FindTexturePath(string[] allfiles, string query)
        {
            Plugin.instance.Log("Searching for " + query);
            foreach (string filePath in allfiles)
            {
                string[] splitPath = filePath.Split('\\');
                string fileName = splitPath[splitPath.Length - 1];
                if (fileName.Equals(query, System.StringComparison.OrdinalIgnoreCase))
                {
                    Plugin.instance.Log("Found " + query + " at [" + filePath + "]");
                    return filePath;
                }
            }
            Plugin.instance.Log("WARNING: Couldn't find file with name [" + query + "]");
            return "ERROR";
        }

        private static Dictionary<string, Texture2D> LOADED_TEXTURES = new Dictionary<string, Texture2D>();

        public static Texture2D LoadTexture(string path)
        {
            Texture2D texture;
            LOADED_TEXTURES.TryGetValue(path, out texture);
            if (texture != null)
                return texture;

            texture = new Texture2D(2, 2);
            ImageConversion.LoadImage(texture, File.ReadAllBytes(path));
            LOADED_TEXTURES.Add(path, texture);
            return texture;
        }
    }
}
