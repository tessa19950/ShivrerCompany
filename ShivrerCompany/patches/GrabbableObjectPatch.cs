using HarmonyLib;
using ShivrerCompany.util;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ShivrerCompany.patches
{
    [HarmonyPatch(typeof(GrabbableObject))]
    internal class GrabbableObjectPatch
    {
        [HarmonyPatch(nameof(GrabbableObject.Start))]
        [HarmonyPostfix]
        static void PatchGrabbableObject(GrabbableObject __instance)
        {
            if (__instance == null || __instance.itemProperties == null)
                return;

            if(__instance.mainObjectRenderer != null)
            {
                ReplaceTexturesInRenderer(__instance, __instance.mainObjectRenderer);
            }
            else
            {
                MeshRenderer[] renderers = __instance.transform.parent.GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer r in renderers)
                    ReplaceTexturesInRenderer(__instance, r);
            }
        }

        static void ReplaceTexturesInRenderer(GrabbableObject item, MeshRenderer r)
        {
            DebugTextures(item.itemProperties, r.gameObject.name, r.materials);
            if (r.sharedMaterial == null)
                return;

            string textureName = r.sharedMaterial.mainTexture != null ? r.sharedMaterial.mainTexture.name : "NULL";
            Plugin.instance.Log("Seeking Replacement for [" + textureName + "]...");
            Texture2D texture = GetReplacementTexture(textureName);
            if (texture != null)
            {
                r.sharedMaterial.color = Color.white;
                r.sharedMaterial.mainTexture = texture;
                Plugin.instance.Log("Replaced [" + item.itemProperties.name + "] with [" + texture.name + "]");
            }
        }

        static Texture2D GetReplacementTexture(string itemName)
        {
            if (itemName.Contains("yield", StringComparison.OrdinalIgnoreCase))
                return TextureUtil.LoadTexture(TextureUtil.YIELDSIGN_TEX);
            if (itemName.Contains("stop", StringComparison.OrdinalIgnoreCase))
                return TextureUtil.LoadTexture(TextureUtil.STOPSIGN_TEX);
            if (itemName.Contains("soda", StringComparison.OrdinalIgnoreCase))
                return TextureUtil.LoadTexture(TextureUtil.SODACAN_TEX);
            if (itemName.Contains("airhorn", StringComparison.OrdinalIgnoreCase))
                return TextureUtil.LoadTexture(TextureUtil.AIRHORN_TEX);
            if (itemName.Contains("gift", StringComparison.OrdinalIgnoreCase))
                return TextureUtil.LoadTexture(TextureUtil.GIFTBOX_TEX);
            if (itemName.Contains("tiny", StringComparison.OrdinalIgnoreCase))
                return TextureUtil.LoadTexture(TextureUtil.TINY_FLASHLIGHT_TEX);
            if (itemName.Contains("flashlight", StringComparison.OrdinalIgnoreCase))
                return TextureUtil.LoadTexture(TextureUtil.FLASHLIGHT_TEX);

            return null;
        }

        static void DebugTextures(Item item, string part, Material[] materials)
        {
            for(int i = 0; i < materials.Length; i++)
            {
                string textureName = (materials[i] == null) ? "NULL MAT" : (materials[i].mainTexture != null ? materials[i].mainTexture.name : "NULL TEX");
                string itemName = item != null ? item.itemName : "NULL";
                Plugin.instance.Log("Item [" + itemName + "] - GameObject:" + part + " has " + materials.Length + " slots. Slot " + i + " contains [" + textureName + "]");
            }
        }
    }
}
