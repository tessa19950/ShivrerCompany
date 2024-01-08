using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShivrerCompany.patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    internal class StartOfRoundPatch
    {
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        static void AwakePatch(ref StartOfRound __instance)
        {
            
        }
    }
}
