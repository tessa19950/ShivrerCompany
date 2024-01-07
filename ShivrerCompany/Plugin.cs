using BepInEx;
using BepInEx.Configuration;
using GameNetcodeStuff;
using HarmonyLib;
using ShivrerCompany.patches;
using System.IO;
using UnityEngine;

namespace ShivrerCompany
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin instance;
        private readonly Harmony harmony = new Harmony(PluginInfo.PLUGIN_GUID);
        private static readonly string PLUGIN_ASSETS_FOLDER = "";

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }

            // Plugin startup logic
            Log("Shivrer is munching on the Lethal Company code...");
            harmony.PatchAll(typeof(Plugin));
            harmony.PatchAll(typeof(FlashlightItemPatch));
            LoadTextures();

            Log($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }

        public static string FLASHLIGHT_TEX;

        private void LoadTextures()
        {
            string[] files = Directory.GetFiles(Path.Combine(Paths.PluginPath, "ShivrerAssets"));
            foreach (string file in files)
            {
                Log("Found File [" + file + "]");
                if (file.Contains("FlashlightTexture"))
                    FLASHLIGHT_TEX = file;
            }
        }

        public void Log(string message)
        {
            Logger.LogInfo(message);
        }
    }
}
