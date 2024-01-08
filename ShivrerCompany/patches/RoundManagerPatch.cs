using HarmonyLib;
using ShivrerCompany.util;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = System.Random;

namespace ShivrerCompany.patches
{
    [HarmonyPatch(typeof(RoundManager))]
    internal class RoundManagerPatch
    {
        [HarmonyPatch("GenerateNewLevelClientRpc")]
        [HarmonyPostfix]
        private static void GenerateNewLevelClientRpcPatch(int randomSeed)
        {
            Plugin.instance.Log("Generating New Level");
            UpdateMaterials(randomSeed);
        }

        private static void UpdateMaterials(int seed)
        {
            Random rand = new Random(seed);
            Material[] materials = (GameObject.Find("HangarShip/Plane.001").GetComponent<MeshRenderer>()).materials;
            materials[0].mainTexture = TextureUtil.LoadTexture(TextureUtil.POSTERS_TEX);
            //materials[1].mainTexture = TextureUtil.LoadTexture(TextureUtil.TIPS_TEX);
        }
    }
}
