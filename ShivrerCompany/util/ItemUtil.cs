using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace ShivrerCompany.util
{
    internal class ItemUtil
    {
        public static void ApplyCustomItemChanges(ref Terminal terminal, List<Item> itemsList)
        {
            Item tinyFlashlight = TryGetItemByName(itemsList, "flashlight");
            UpdateItemIcon(tinyFlashlight, TextureUtil.TINY_FLASHLIGHT_ICON);

            Item proFlashlight = TryGetItemByName(itemsList, "pro-flashlight");
            UpdateItemIcon(proFlashlight, TextureUtil.FLASHLIGHT_ICON);

            Item walkie = TryGetItemByName(itemsList, "walkie");
            UpdateItemIcon(walkie, TextureUtil.WALKIETALKIE_ICON);

            Item airhorn = TryGetItemByName(itemsList, "airhorn");
            TryAddBuyableItem(ref terminal, airhorn);

            Item yield = TryGetItemByName(itemsList, "yield");
            TryAddBuyableItem(ref terminal, yield);

            Item gift = TryGetItemByName(itemsList, "gift");
            TryAddBuyableItem(ref terminal, gift);

            Item soda = TryGetItemByName(itemsList, "soda");
            TryAddBuyableItem(ref terminal, soda);

            Item stop = TryGetItemByName(itemsList, "stop");
            TryAddBuyableItem(ref terminal, stop);
        }

        public static void UpdateItemIcon(Item item, string iconPath)
        {
            Plugin.instance.Log("Updating Item Icon for [" + item.itemName + "]");
            Texture2D texture = TextureUtil.LoadTexture(iconPath);
            Sprite sprite = Sprite.Create(new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 256, texture);
            item.itemIcon = sprite;
            Plugin.instance.Log("Applied Texture [" + texture + "]");
        }

        public static void TryAddBuyableItem(ref Terminal __instance, Item item)
        {
            Plugin.instance.Log("Trying to add a new item to the Terminal!");
            if (item == null)
                return;

            List<Item> buyableItems = [.. __instance.buyableItemsList];
            buyableItems.Add(item);
            TerminalKeyword keyword = new TerminalKeyword();
            keyword.word = item.itemName;
            AddKeyword(ref __instance.terminalNodes, keyword);
            Plugin.instance.Log("Added [" + item.name + "] to the buyable items list.");

            __instance.buyableItemsList = buyableItems.ToArray();
        }

        public static int GetBuyableIdByName(ref Terminal __instance, List<Item> allItems, string name)
        {
            Item item = TryGetItemByName(allItems, name);
            if (item == null)
                return -1;
            for (int i = 0; i < __instance.buyableItemsList.Length; i++)
            {
                if (__instance.buyableItemsList[i].Equals(item))
                    return i;
            }
            return -1;
        }

        public static Item TryGetItemByName(List<Item> allItems, string query)
        {
            Plugin.instance.Log("Searching for item [" + query + "]...");
            foreach (Item item in allItems)
            {
                if (item.itemName.Contains(query, StringComparison.OrdinalIgnoreCase))
                    return item;
            }
            Plugin.instance.Log("WARNING: Couldn't find item with name [" + query + "]!");
            return null;
        }

        public static void AddKeyword(ref TerminalNodesList nodes, TerminalKeyword keyword)
        {
            List<TerminalKeyword> newKeywords = [.. nodes.allKeywords];
            newKeywords.Add(keyword);
            nodes.allKeywords = newKeywords.ToArray();
        }

        public static void AddTerminalItem(TerminalNodesList nodes, string name, int cost, int index)
        {
            TerminalNode customNode = new TerminalNode();
            customNode.displayText = "ShivBucks";
            customNode.itemCost = cost;
            customNode.buyItemIndex = index;

            Plugin.instance.Log("Terminal Item Added [" + name + "]");
        }

    }
}
