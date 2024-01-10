using BepInEx;
using BepInEx.Configuration;
using GameNetcodeStuff;
using HarmonyLib;
using ShivrerCompany.patches;
using ShivrerCompany.util;
using System.IO;
using UnityEngine;

namespace ShivrerCompany
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin instance;
        private readonly Harmony harmony = new Harmony(PluginInfo.PLUGIN_GUID);

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }

            // Plugin startup logic
            Log("Shivrer is munching on the Lethal Company code...");

            TextureUtil.LoadTexturePaths();

            harmony.PatchAll(typeof(Plugin));
            harmony.PatchAll(typeof(RoundManagerPatch));
            harmony.PatchAll(typeof(TerminalPatch));

            harmony.PatchAll(typeof(WalkieTalkiePatch));
            harmony.PatchAll(typeof(GrabbableObjectPatch));

            Log($"Plugin {PluginInfo.PLUGIN_GUID} is loaded! <3");
        }

        public void Log(string message)
        {
            Logger.LogInfo(message);
        }
    }
}
