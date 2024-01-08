using HarmonyLib;
using ShivrerCompany.util;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ShivrerCompany.patches
{
    [HarmonyPatch(typeof(WalkieTalkie))]
    internal class WalkieTalkiePatch
    {
        [HarmonyPatch(nameof(WalkieTalkie.Start))]
        [HarmonyPostfix]
        static void PatchGiftbox(WalkieTalkie __instance)
        {
            Plugin.instance.Log("Replacing WalkieTalkie Texture...");
            MeshRenderer renderer = __instance.mainObjectRenderer;
            renderer.sharedMaterial.color = Color.white;
            renderer.sharedMaterial.mainTexture = TextureUtil.LoadTexture(TextureUtil.WALKIETALKIE_TEX);

            __instance.onMaterial.mainTexture = TextureUtil.LoadTexture(TextureUtil.WALKIETALKIE_TEX);
            __instance.offMaterial.mainTexture = TextureUtil.LoadTexture(TextureUtil.WALKIETALKIE_TEX);

            Plugin.instance.Log("Replaced WalkieTalkie Texture!");
        }
    }
}
