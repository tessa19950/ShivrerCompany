using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using System.Linq;
using ShivrerCompany.util;

namespace ShivrerCompany.patches
{
    [HarmonyPatch(typeof(Terminal))]
    internal class TerminalPatch
    {
        static Dictionary<TerminalNode, UnityAction<Terminal>> CUSTOM_EVENT_MAP = new Dictionary<TerminalNode, UnityAction<Terminal>>();

        static string GIMME_GIMME_MESSAGE =
            "Fine, fine. I know all you want is to play with your little toys.\n" +
            "I need you to hold on tight, while Daddy orders you some things online. <3\n" +
            "Go wait outside for the mailman to arrive, okay?\n" +
            "   - xoxo The Company\n\n\n";

        static int MONEY_REQUESTED = 0;
        static int[] MONEY_AMOUNTS = new int[]{ 500, 250, 100, 50, 0 };
        static string[] MONEY_PLEASE_MESSAGES = new string[] {
                "Alright then Shiv, but only because you hold a special place in my tentacly heart!\n" +
                "Here you go kitten, have some money \\(*v*)/\n" +
                "   - xoxo The Company\n\n\n",
                "Nahww, you know I have a weak spot for you <3 you little stinker.\n" +
                "For this once, I'll give you 250 extra credits\n" +
                "   - xoxo The Company\n\n\n",
                "Don't get greedy now pookie! Daddy's money doesn't grow on his back...\n" +
                "Or like, whatever you'd call my incomprehensibly horrifying phsyical manifestation.\n" +
                "   - xoxo The Company\n\n\n",
                "I'll give you a just few more credits now, but is your last allowance Shivvy,\n" +
                "Papa loves you very much, but you're going to have to make your own credits some day.\n" +
                "   - The Company\n\n\n",
                "No more credits for you babygirl .\\_/. You've made me very upset now!\n" +
                "Go outside and get some more trinkets for Daddy, then we can talk.\n" +
                "   - The Company\n\n\n"
        };

        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        static void AwakePatch(ref Terminal __instance)
        {
            Plugin.instance.Log("Patching Terminal...");

            StartOfRound round = UnityEngine.Object.FindObjectOfType<StartOfRound>();
            List<Item> allItems = round.allItemsList.itemsList;
            ItemUtil.ApplyCustomItemChanges(ref __instance, allItems);

            TerminalNodesList terminalNodes = __instance.terminalNodes;
            AddExactSentence(ref terminalNodes, "moneyplease",
                MONEY_PLEASE_MESSAGES[0],
                (terminal) => 
                {
                    int amount = MONEY_AMOUNTS[0];
                    if (!terminal.IsServer)
                    {
                        Plugin.instance.Log("Trying to add money on the Server");
                        terminal.SyncGroupCreditsServerRpc(terminal.groupCredits + amount, 1);
                        Plugin.instance.Log("Added money on the Server");
                    }
                    else
                    {
                        Plugin.instance.Log("Trying to add money on the Client");
                        terminal.SyncGroupCreditsClientRpc(terminal.groupCredits + amount, 1);
                        Plugin.instance.Log("Added money on the Client");
                    }
                    MONEY_REQUESTED++;
                }
            );
            AddExactSentence(ref terminalNodes, "gimmegimme",
                GIMME_GIMME_MESSAGE,
                (terminal) =>
                {
                    terminal.orderedItemsFromTerminal.Add(ItemUtil.GetBuyableIdByName(ref terminal, allItems, "airhorn"));
                    terminal.orderedItemsFromTerminal.Add(ItemUtil.GetBuyableIdByName(ref terminal, allItems, "yield"));
                    terminal.orderedItemsFromTerminal.Add(ItemUtil.GetBuyableIdByName(ref terminal, allItems, "gift"));
                    terminal.orderedItemsFromTerminal.Add(ItemUtil.GetBuyableIdByName(ref terminal, allItems, "soda"));
                    terminal.orderedItemsFromTerminal.Add(ItemUtil.GetBuyableIdByName(ref terminal, allItems, "stop"));
                    if (!terminal.IsServer)
                    {
                        Plugin.instance.Log("Trying to buy toys on the Server");
                        terminal.useCreditsCooldown = true;
                        terminal.BuyItemsServerRpc(terminal.orderedItemsFromTerminal.ToArray(), terminal.groupCredits, 5);
                        terminal.orderedItemsFromTerminal.Clear();
                        Plugin.instance.Log("Bought toys on the Server");
                    }
                    else
                    {
                        Plugin.instance.Log("Trying to buy toys on the Client");
                        terminal.SyncGroupCreditsClientRpc(terminal.groupCredits, 5);
                        Plugin.instance.Log("Bought toys on the Client");
                    }
                }
            );
            ItemUtil.AddTerminalItem(terminalNodes, "ShivBucks", 6, 12);
        }

        static void AddExactSentence(ref TerminalNodesList nodes, string sentence, string description, UnityAction<Terminal> nodeEvent)
        {
            TerminalNode customNode = new TerminalNode();
            customNode.terminalEvent = sentence;
            customNode.name = sentence;
            customNode.displayText = description;

            TerminalKeyword verbKey = new TerminalKeyword();
            verbKey.word = sentence;
            verbKey.specialKeywordResult = customNode;
            ItemUtil.AddKeyword(ref nodes, verbKey);

            nodes.specialNodes.Add(customNode);
            CUSTOM_EVENT_MAP.Add(customNode, nodeEvent);

            Plugin.instance.Log("Terminal Event Added [" + sentence + "]");
        }

        [HarmonyPatch(nameof(Terminal.RunTerminalEvents))]
        [HarmonyPostfix]
        static void RunTerminalEventsPatch(ref Terminal __instance, TerminalNode node)
        {
            string nodeName = (node == null) ? "null" : node.name;
            Plugin.instance.Log("Looking for Custom Nodes called [" + nodeName + "]...");
            UnityAction<Terminal> action;
            CUSTOM_EVENT_MAP.TryGetValue(node, out action);
            if (action != null)
            {
                Plugin.instance.Log("Invoking action for Custom Node [" + nodeName + "]");
                action.Invoke(__instance);
            }
        }

        // WEIRD TESTING CODE THATS JUST FOR ME TO GET THE FLOW OF THINGS
        [HarmonyPatch(nameof(Terminal.OnSubmit))]
        [HarmonyPrefix]
        static void OnSubmitPatch(ref Terminal __instance)
        {
            string nodeName = (__instance.currentNode == null) ? "null" : __instance.currentNode.name;
            Plugin.instance.Log("Player Submitted Keyword! Current Node is [" + nodeName + "]");
        }

        [HarmonyPatch("ParseWord")]
        [HarmonyPostfix]
        static void ParseWordPatch(ref TerminalKeyword __result)
        {
            string keywordName = (__result == null) ? "null" : __result.word;
            Plugin.instance.Log("Parsing found Keyword [" + keywordName + "]");
        }

        [HarmonyPatch("ParsePlayerSentence")]
        [HarmonyPrefix]
        static void ParsePlayerSentencePatch()
        {
            Plugin.instance.Log("Parsing Player Sentence");
        }

        [HarmonyPatch(nameof(Terminal.LoadNewNode))]
        [HarmonyPrefix]
        static void LoadNewNodePatch(TerminalNode node)
        {
            Plugin.instance.Log("Loading Node [" + node.name + "]");
        }
    }
}
