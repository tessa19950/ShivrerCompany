using HarmonyLib;
using ShivrerCompany.util;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace ShivrerCompany.patches
{
    [HarmonyPatch(typeof(EnemyAI))]
    internal class EnemyAIPatch
    {
        [HarmonyPatch(nameof(EnemyAI.Start))]
        [HarmonyPostfix]
        static void PatchEnemy(EnemyAI __instance)
        {
            if (__instance == null)
                return;
            foreach(SkinnedMeshRenderer r in __instance.skinnedMeshRenderers)
                ReplaceTexturesInRenderer(__instance, r);
        }

        static void ReplaceTexturesInRenderer(EnemyAI enemy, SkinnedMeshRenderer r)
        {
            DebugTextures(enemy, r.gameObject.name, r.materials);
            if (r.sharedMaterial == null)
                return;

            string textureName = r.sharedMaterial.mainTexture != null ? r.sharedMaterial.mainTexture.name : "NULL";
            Plugin.instance.Log("Seeking Replacement for Enemy Texture [" + textureName + "]...");
            Texture2D texture = GetReplacementTexture(textureName);
            if (texture != null)
            {
                r.sharedMaterial.color = Color.white;
                r.sharedMaterial.mainTexture = texture;
                Plugin.instance.Log("Replaced Enemy Texture [" + enemy.name + "] with [" + texture.name + "]");
            }
        }
        static Texture2D GetReplacementTexture(string enemyTextureName)
        {
            if (enemyTextureName.Contains("spider", StringComparison.OrdinalIgnoreCase))
                return TextureUtil.LoadTexture(TextureUtil.SANDSPIDER_TEX);
            if (enemyTextureName.Contains("nutcracker", StringComparison.OrdinalIgnoreCase))
                return TextureUtil.LoadTexture(TextureUtil.NUTCRACKER_TEX);

            return null;
        }

        static void DebugTextures(EnemyAI enemy, string part, Material[] materials)
        {
            for (int i = 0; i < materials.Length; i++)
            {
                string textureName = (materials[i] == null) ? "NULL MAT" : (materials[i].mainTexture != null ? materials[i].mainTexture.name : "NULL TEX");
                string enemyName = enemy.enemyType != null ? enemy.enemyType.enemyName : "NULL";
                Plugin.instance.Log("Item [" + enemyName + "] - GameObject:" + part + " has " + materials.Length + " slots. Slot " + i + " contains [" + textureName + "]");
            }
        }
    }
}
