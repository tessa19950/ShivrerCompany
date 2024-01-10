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
            SANDSPIDER_TEX,
            NUTCRACKER_TEX,

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
            List<string> files = new List<string>();
            AddFilePaths(files, Paths.PluginPath);
            AddFilePaths(files, Path.Combine(Paths.PluginPath, "SuperSuccubus-ShivrerCompany"));
            AddFilePaths(files, Path.Combine(Paths.PluginPath, "SuperSuccubus-ShivrerCompany", "ShivrerAssets"));

            //for (int i = 0; i < files.Count; i++)
            //Plugin.instance.Log("File [" + (i + 1) + "/" + files.Count + "] [" + files[i] + "]");

            SANDSPIDER_TEX = FindTextureInFiles(files, "SandSpiderTex");
            NUTCRACKER_TEX = FindTextureInFiles(files, "NutcrackerTex");

            WALKIETALKIE_TEX = FindTextureInFiles(files, "WalkieTalkieTex");
            WALKIETALKIE_SCREEN_TEX = FindTextureInFiles(files, "WalkieTalkieScreenEmission");
            WALKIETALKIE_ICON = FindTextureInFiles(files, "WalkieTalkieIcon");
            FLASHLIGHT_TEX = FindTextureInFiles(files, "FlashlightTexture");
            FLASHLIGHT_ICON = FindTextureInFiles(files, "FlashlightIcon");
            TINY_FLASHLIGHT_TEX = FindTextureInFiles(files, "TinyFlashlightTex");
            TINY_FLASHLIGHT_ICON = FindTextureInFiles(files, "TinyFlashlightIcon");
            GIFTBOX_TEX = FindTextureInFiles(files, "GiftBoxTex");
            GIFTBOX_ICON = FindTextureInFiles(files, "GiftBoxIcon");
            AIRHORN_TEX = FindTextureInFiles(files, "AirhornTex");
            SODACAN_TEX = FindTextureInFiles(files, "SodaCanTex1");
            STOPSIGN_TEX = FindTextureInFiles(files, "StopSignTex");
            YIELDSIGN_TEX = FindTextureInFiles(files, "YieldSignTex");
            POSTERS_TEX = FindTextureInFiles(files, "posters");

            Plugin.instance.Log("All required textures have been loaded! <3");
        }

        private static void AddFilePaths(List<string> files, string path)
        {
            try
            {
                Plugin.instance.Log("Searching for files in [" + path + "]");
                files.AddRange(Directory.GetFiles(path));
            }
            catch (DirectoryNotFoundException) { }
        }

        public static string FindTextureInFiles(List<string> allfiles, string query)
        {
            foreach (string filePath in allfiles)
            {
                string[] splitPath = filePath.Split('\\');
                string fileName = splitPath[splitPath.Length - 1];
                if (fileName.Equals(query + ".png", StringComparison.OrdinalIgnoreCase))
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
