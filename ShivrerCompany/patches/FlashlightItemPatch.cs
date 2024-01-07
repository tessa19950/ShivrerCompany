using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ShivrerCompany.patches
{
    [HarmonyPatch(typeof(FlashlightItem))]
    internal class FlashlightItemPatch
    {
        [HarmonyPatch(nameof(FlashlightItem.Start))]
        [HarmonyPostfix]
        static void patchFlashlight(ref MeshRenderer ___mainObjectRenderer)
        {
            Plugin.instance.Log("Replacing Flashlight Texture...");

            Texture2D texture = new Texture2D(2, 2);
            ImageConversion.LoadImage(texture, File.ReadAllBytes(Plugin.FLASHLIGHT_TEX));

            ___mainObjectRenderer.sharedMaterial.color = Color.white;
            ___mainObjectRenderer.sharedMaterial.mainTexture = texture;

            Plugin.instance.Log("Replaced Flashlight Texture!");
        }

    }
}
