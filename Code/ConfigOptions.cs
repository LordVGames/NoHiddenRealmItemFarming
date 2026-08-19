using System;
using System.Collections.Generic;
using System.Text;
using BepInEx.Configuration;
using MiscFixes.Modules;
namespace NoHiddenRealmItemFarming;


public static class ConfigOptions
{
    private const string _sectionName = "config options are here";
    internal static ConfigEntry<bool> ShowDebugLogging;


    public static ConfigEntry<ItemPreventionType> ItemPreventionMode;
    public enum ItemPreventionType
    {
        NoDropping = 0,
        MakeTemporary
    }


    public static ConfigEntry<string> ItemTiersWhitelisted;
    internal static string[] ItemTiersWhitelistedList;
    private static void ItemTiersWhitelisted_SettingChanged(object sender, EventArgs e)
    {
        ItemTiersWhitelistedList = ItemTiersWhitelisted.Value.Split(',');
    }


    public static ConfigEntry<string> StagesBlacklisted;
    internal static string[] StagesBlacklistedList;
    private static void StagesBlacklisted_SettingChanged(object sender, EventArgs e)
    {
        StagesBlacklistedList = StagesBlacklisted.Value.Split(',');
    }




    internal static void BindConfigOptions(ConfigFile config)
    {
        ShowDebugLogging = config.BindOption(
            _sectionName,
            "Show debug logging",
            "does what it says",
            false
        );
        ItemPreventionMode = config.BindOption(
            _sectionName,
            "How to prevent item drops",
            "Choose the way item drops in hidden realms should be prevented/nerfed. MakeTemporary requires the Alloyed Collective DLC.",
            ItemPreventionType.NoDropping
        );


        StagesBlacklisted = config.BindOption(
            _sectionName,
            "Blacklisted stage names",
            "Use this to prevent item drops in certain stages/hidden realms. Uses the internal name of the stages (use DebugTooalkit's next_stage command to find those out), separate each with a comma.",
            "goldshores,arena"
        );
        StagesBlacklistedList = StagesBlacklisted.Value.Split(',');
        StagesBlacklisted.SettingChanged += StagesBlacklisted_SettingChanged;


        ItemTiersWhitelisted = config.BindOption(
            _sectionName,
            "Whitelisted item tier indices",
            "Use this to allow certain item tiers to always drop in blacklisted hidden realms. Uses the indices/numbers of item tiers (use DebugToolkit's list_itemtier command to find those out), separate each with a comma.\n\nThe default number 4 is for boss items and 1002 is for Starstorm 2s sibylline item tier. I'm not sure if modded item tiers can change when more are added so double check if the sibylline item tier number is correct in your modpack.",
            "4,1002"
        );
        ItemTiersWhitelistedList = ItemTiersWhitelisted.Value.Split(',');
        ItemTiersWhitelisted.SettingChanged += ItemTiersWhitelisted_SettingChanged;
    }
}
