﻿using Il2CppTLD.Cooking;
using static ItemRarities.Main;

namespace ItemRarities
{
    #region User Interface Harmony Patches

    #region Inventory
    // The item info in the inventory to the right, when an item is selected on the right.
    [HarmonyPatch(typeof(ItemDescriptionPage), nameof(ItemDescriptionPage.UpdateGearItemDescription))]
    public static class ItemDescriptionPage_RarityLabelPatch
    {
        static UILabel? rarityLabel;
        static void Postfix(ItemDescriptionPage __instance, GearItem gi)
        {
            if (__instance.m_ItemNameLabel == null) return;

            string itemName = gi.name;
            Rarity itemRarity = GetRarity(itemName);
            Color rarityColor = GetColorForRarity(itemRarity);

            if (rarityLabel == null)
            {
                rarityLabel = UnityEngine.Object.Instantiate(__instance.m_ItemNameLabel);
                rarityLabel.transform.SetParent(__instance.m_ItemNameLabel.transform.parent, false);
                rarityLabel.transform.localPosition = new Vector3(__instance.m_ItemNameLabel.transform.localPosition.x,
                                                                  __instance.m_ItemNameLabel.transform.localPosition.y - -25,
                                                                  __instance.m_ItemNameLabel.transform.localPosition.z);
                rarityLabel.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
            }

            rarityLabel.text = itemRarity.ToString();
            rarityLabel.color = rarityColor;
        }
    }

    // Actions menu, when you go to unload, harvest, repair, etc.
    [HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.UpdateLabels))]
    public static class PanelInventoryExamine_RarityLabelPatch
    {
        static UILabel? rarityLabel;
        static void Postfix(Panel_Inventory_Examine __instance)
        {
            if (__instance.m_Item_Label == null) return;

            string itemName = __instance.m_GearItem.name;
            Rarity itemRarity = gearRarities.ContainsKey(itemName) ? gearRarities[itemName] : Rarity.INVALID;
            Color rarityColor = GetColorForRarity(itemRarity);

            if (rarityLabel == null)
            {
                rarityLabel = UnityEngine.Object.Instantiate(__instance.m_Item_Label);

                rarityLabel.transform.SetParent(__instance.m_Item_Label.transform.parent, false);
                rarityLabel.transform.localPosition = new Vector3(__instance.m_Item_Label.transform.localPosition.x,
                                                                  __instance.m_Item_Label.transform.localPosition.y - -25,
                                                                  __instance.m_Item_Label.transform.localPosition.z);

                rarityLabel.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
            }

            rarityLabel.text = itemRarity.ToString();
            rarityLabel.color = rarityColor;
        }
    }
    #endregion

    #region Inventory Miscellaneous
    // Shows the rarity label above the clothing item, in the clothing ui from the inventory.
    [HarmonyPatch(typeof(Panel_Clothing), nameof(Panel_Clothing.GetCurrentlySelectedGearItem))]
    public static class PanelClothing_RarityLabelPatch
    {
        static UILabel? clothingRarityLabel;
        static void Postfix(Panel_Clothing __instance, ref GearItem __result)
        {
            if (__instance.m_ItemDescriptionPage == null || __instance.m_ItemDescriptionPage.m_ItemNameLabel == null) return;

            string itemName = __result.name;
            Rarity itemRarity = GetRarity(itemName);
            Color rarityColor = GetColorForRarity(itemRarity);

            if (clothingRarityLabel == null)
            {
                clothingRarityLabel = UnityEngine.Object.Instantiate(__instance.m_ItemDescriptionPage.m_ItemNameLabel);

                clothingRarityLabel.transform.SetParent(__instance.m_ItemDescriptionPage.m_ItemNameLabel.transform.parent, false);
                clothingRarityLabel.transform.localPosition = new Vector3(__instance.m_ItemDescriptionPage.m_ItemNameLabel.transform.localPosition.x,
                                                                  __instance.m_ItemDescriptionPage.m_ItemNameLabel.transform.localPosition.y - -25,
                                                                  __instance.m_ItemDescriptionPage.m_ItemNameLabel.transform.localPosition.z);

                clothingRarityLabel.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
            }

            clothingRarityLabel.text = itemRarity.ToString();
            clothingRarityLabel.color = rarityColor;
        }
    }

    // Shows rarity label in the Crafting section within the Inventory or Workshop tables.
    [HarmonyPatch(typeof(Panel_Crafting), nameof(Panel_Crafting.RefreshSelectedBlueprint))]
    public static class PanelCrafting_RarityLabelPatch
    {
        static UILabel? rarityLabel;

        static void Postfix(Panel_Crafting __instance)
        {
            if (__instance.m_SelectedName == null) return;

            int selectedIndex = __instance.m_CurrentBlueprintIndex;
            string itemName = __instance.m_FilteredBlueprints[selectedIndex]?.m_CraftedResult?.name ?? "Unknown";

            Rarity itemRarity = gearRarities.ContainsKey(itemName) ? gearRarities[itemName] : Rarity.INVALID;
            Color rarityColor = GetColorForRarity(itemRarity);

            if (rarityLabel == null)
            {
                rarityLabel = UnityEngine.Object.Instantiate(__instance.m_SelectedName);
                rarityLabel.transform.SetParent(__instance.m_SelectedName.transform.parent, false);
                rarityLabel.transform.localPosition = new Vector3(__instance.m_SelectedName.transform.localPosition.x,
                                                                  __instance.m_SelectedName.transform.localPosition.y - -25,
                                                                  __instance.m_SelectedName.transform.localPosition.z);
                rarityLabel.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
            }

            rarityLabel.text = itemRarity.ToString();
            rarityLabel.color = rarityColor;
        }
    }

    // Adds rarity label to the Cooking section within the Inventory.
    [HarmonyPatch(typeof(Panel_Cooking), nameof(Panel_Cooking.GetSelectedCookableItem))]
    public static class PanelCooking_RarityLabelPatch
    {
        static UILabel? rarityLabel;
        static void Postfix(Panel_Cooking __instance, ref CookableItem __result)
        {
            if (__instance.m_Label_CookedItemName == null) return;

            if (__result == null || __result.m_GearItem == null)
            {
                if (rarityLabel != null)
                {
                    rarityLabel.enabled = false;
                }
                return;
            }

            string itemName = __result.m_GearItem.name;
            Rarity itemRarity = gearRarities.ContainsKey(itemName) ? gearRarities[itemName] : Rarity.INVALID;
            Color rarityColor = GetColorForRarity(itemRarity);

            if (rarityLabel == null)
            {
                rarityLabel = UnityEngine.Object.Instantiate(__instance.m_Label_CookedItemName);

                rarityLabel.transform.SetParent(__instance.m_Label_CookedItemName.transform.parent, false);
                rarityLabel.transform.localPosition = new Vector3(__instance.m_Label_CookedItemName.transform.localPosition.x,
                                                                  __instance.m_Label_CookedItemName.transform.localPosition.y - -25,
                                                                  __instance.m_Label_CookedItemName.transform.localPosition.z);

                rarityLabel.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
            }

            rarityLabel.text = itemRarity.ToString();
            rarityLabel.color = rarityColor;
            rarityLabel.enabled = true;
        }
    }

    // This harmony patch duplicates the rarity label to the Milling interface when a milling machine is accessed.
    [HarmonyPatch(typeof(Panel_Milling), nameof(Panel_Milling.GetSelected))]
    public static class PanelMilling_RarityLabelPatch
    {
        static UILabel? rarityLabel;
        static void Postfix(Panel_Milling __instance, ref GearItem __result)
        {
            if (__instance.m_NameLabel == null) return;

            string itemName = __result.name;
            Rarity itemRarity = gearRarities.ContainsKey(itemName) ? gearRarities[itemName] : Rarity.INVALID;
            Color rarityColor = GetColorForRarity(itemRarity);

            if (rarityLabel == null)
            {
                rarityLabel = UnityEngine.Object.Instantiate(__instance.m_NameLabel);

                rarityLabel.transform.SetParent(__instance.m_NameLabel.transform.parent, false);
                rarityLabel.transform.localPosition = new Vector3(__instance.m_NameLabel.transform.localPosition.x,
                                                                  __instance.m_NameLabel.transform.localPosition.y - -25,
                                                                  __instance.m_NameLabel.transform.localPosition.z);

                rarityLabel.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
            }

            rarityLabel.text = itemRarity.ToString();
            rarityLabel.color = rarityColor;
        }
    }
    #endregion

    #region Heads Up Display (HUD)
    // Adds the rarity label to the Radial Menu.
    [HarmonyPatch(typeof(Panel_ActionsRadial), nameof(Panel_ActionsRadial.GetActionText))]
    public static class PanelActionsRadial_RarityLabelPatch
    {
        internal static UILabel? rarityLabel;

        private static readonly HashSet<string> excludedNames = new HashSet<string>
        {
            "PACKSETTINGS_Pilgrim",
            "NAVIGATION",
            "CAMPCRAFT",
            "FIRST AID",
            "DRINK",
            "LIGHT SOURCES",
            "FOOD",
            "WEAPONS",
            "DROP DECOY",
            "OPEN MAP",
            "ROCK CACHE",
            "STATUS",
            "FIRE",
            "PASS TIME",
            "ICE FISHING HOLE",
            "SNOW SHELTER"
        };
        static void Postfix(Panel_ActionsRadial __instance, RadialMenuArm arm)
        {
            if (__instance.m_SegmentLabel == null) return;

            if (rarityLabel == null)
            {
                rarityLabel = UnityEngine.Object.Instantiate(__instance.m_SegmentLabel);

                rarityLabel.transform.SetParent(__instance.m_SegmentLabel.transform.parent, false);
                rarityLabel.transform.localPosition = new Vector3(__instance.m_SegmentLabel.transform.localPosition.x,
                                                                  __instance.m_SegmentLabel.transform.localPosition.y - -20,
                                                                  __instance.m_SegmentLabel.transform.localPosition.z);

                rarityLabel.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
            }

            if (arm != null && arm.m_GearItem != null)
            {
                string itemName = arm.m_GearItem.name;
                Rarity itemRarity = gearRarities.ContainsKey(itemName) ? gearRarities[itemName] : Rarity.INVALID;
                Color rarityColor = GetColorForRarity(itemRarity);

                if (!excludedNames.Contains(itemName) && !string.IsNullOrEmpty(itemName))
                {
                    rarityLabel.text = itemRarity.ToString();
                    rarityLabel.color = rarityColor;
                    rarityLabel.gameObject.SetActive(true);
                    return;
                }
            }

            if (rarityLabel != null)
            {
                rarityLabel.gameObject.SetActive(false);
            }
        }
    }

    // This patch allows the rarity label to change inside the first patch.
    [HarmonyPatch(typeof(Panel_ActionsRadial), nameof(Panel_ActionsRadial.UpdateDisplayText))]
    public static class PanelActionsRadial_UpdateDisplayText_Patch
    {
        public static void Postfix(Panel_ActionsRadial __instance)
        {
            UILabel? label = PanelActionsRadial_RarityLabelPatch.rarityLabel;

            bool isHoveredOverAnyItem = false;

            foreach (var arm in __instance.m_RadialArms)
            {
                if (arm != null && arm.IsHoveredOver() && !arm.IsEmpty())
                {
                    isHoveredOverAnyItem = true;
                    break;
                }
            }

            if (label != null && !isHoveredOverAnyItem)
            {
                label.gameObject.SetActive(false);
            }
        }
    }
    #endregion

    #region Miscellaneous
    // This displays the rarity label inside of the Inspect mode, when an item is picked up from the floor - or searched.
    [HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.InitLabelsForGear))]
    public static class PlayerManager_RarityLabelPatch
    {
        static UILabel? rarityLabel;
        static void Postfix(PlayerManager __instance)
        {
            Panel_HUD? actualHUDPanel = __instance.m_HUD.GetPanel();
            if (actualHUDPanel == null || actualHUDPanel.m_InspectMode_Title == null) return;

            GearItem? currentGearItem = __instance.m_Gear;
            if (currentGearItem == null) return;

            string itemName = currentGearItem.name;
            Rarity itemRarity = gearRarities.ContainsKey(itemName) ? gearRarities[itemName] : Rarity.INVALID;
            Color rarityColor = GetColorForRarity(itemRarity);

            if (rarityLabel == null)
            {
                rarityLabel = UnityEngine.Object.Instantiate(actualHUDPanel.m_InspectMode_Title);
                rarityLabel.alignment = NGUIText.Alignment.Center;

                rarityLabel.transform.SetParent(actualHUDPanel.m_InspectMode_Title.transform.parent, false);
                rarityLabel.transform.localPosition = new Vector3(actualHUDPanel.m_InspectMode_Title.transform.localPosition.x - 366,
                                                                  actualHUDPanel.m_InspectMode_Title.transform.localPosition.y - 290,
                                                                  actualHUDPanel.m_InspectMode_Title.transform.localPosition.z);

                rarityLabel.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
            }

            rarityLabel.text = itemRarity.ToString();
            rarityLabel.color = rarityColor;
        }
    }
    #endregion

    #region Future Harmony Patches?
    // Possible Harmony Patches which can be used in the future?
    /* [HarmonyPatch(typeof(Panel_GearSelect), nameof(Panel_GearSelect.Update))] // Need to find an alternative method. Slightly broken, all labels disapear after No Tools is selected
            public static class Panel_GearSelectAddRarityLabelPatch
            {
                static UILabel? rarityLabel;

                private static readonly HashSet<string> excludedNames = new HashSet<string>
                {
                    "NO TOOL",
                };
                static void Postfix(Panel_GearSelect __instance)
                {
                    if (__instance.m_Label == null) return;

                    string itemName = __instance.m_Label.text;
                    Rarity itemRarity = gearRarities.ContainsKey(itemName) ? gearRarities[itemName] : Rarity.INVALID;
                    Color rarityColor = GetColorForRarity(itemRarity);

                    if (excludedNames.Contains(itemName))
                    {
                        if (rarityLabel != null)
                        {
                            rarityLabel.gameObject.SetActive(false);
                        }
                        return;
                    }

                    if (string.IsNullOrEmpty(itemName))
                    {
                        if (rarityLabel != null)
                        {
                            rarityLabel.gameObject.SetActive(false);
                        }
                        return;
                    }

                    if (rarityLabel == null)
                    {
                        rarityLabel = UnityEngine.Object.Instantiate(__instance.m_Label);

                        rarityLabel.transform.SetParent(__instance.m_Label.transform.parent, false);
                        rarityLabel.transform.localPosition = new Vector3(__instance.m_Label.transform.localPosition.x,
                                                                          __instance.m_Label.transform.localPosition.y - -15,
                                                                          __instance.m_Label.transform.localPosition.z);

                        rarityLabel.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
                    }

                    rarityLabel.text = itemRarity.ToString();
                    rarityLabel.color = rarityColor;
                }
            } */

    // Commented out because its not as visually altering as these other methods.
    // Need to find an alternative method - and a way to get the GearItem for the rarity to change.
    /* [HarmonyPatch(typeof(Panel_HUD), nameof(Panel_HUD.Update))]
    public static class PanelHUD_RarityLabelPatch
    {
        static void Postfix(Panel_HUD __instance)
        {
            if (__instance.m_Label_ObjectName == null) return;

            string itemName = __instance.m_Label_ObjectName.text;
            Rarity itemRarity = gearRarities.ContainsKey(itemName) ? gearRarities[itemName] : Rarity.Default;
            Color rarityColor = GetColorForRarity(itemRarity);

            __instance.m_Label_ObjectName.color = rarityColor;
        }
    } */
    #endregion

    #endregion
}