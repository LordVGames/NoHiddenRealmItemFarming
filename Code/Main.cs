using DropSourceForItems;
using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.DetourTypes;
using MonoDetour.HookGen;
using MonoMod.Cil;
using RoR2;
using RoR2.ContentManagement;
using RoR2.ExpansionManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
namespace NoHiddenRealmItemFarming;


[MonoDetourTargets(typeof(PickupDropletController), GenerateControlFlowVariants = true)]
internal static class Main
{
    private static readonly AssetReferenceT<ExpansionDef> _alloyedCollectiveReference = new(RoR2BepInExPack.GameAssetPaths.Version_1_39_0.RoR2_DLC3.DLC3_asset);
    private static ExpansionDef _alloyedCollective;


    [MonoDetourHookInitialize]
    private static void Setup()
    {
        Mdh.RoR2.PickupDropletController.CreatePickupDroplet_RoR2_GenericPickupController_CreatePickupInfo_UnityEngine_Vector3_UnityEngine_Vector3.ControlFlowPrefix(GoAwayHiddenRealmFarming);


        AssetAsyncReferenceManager<ExpansionDef>.LoadAsset(_alloyedCollectiveReference).Completed += (handle) =>
        {
            _alloyedCollective = handle.Result;
        };
    }


    private static ReturnFlow GoAwayHiddenRealmFarming(ref GenericPickupController.CreatePickupInfo pickupInfo, ref Vector3 position, ref Vector3 velocity)
    {
        if (!ConfigOptions.StagesBlacklistedList.Contains(Stage.instance.sceneDef.cachedName))
        {
            return ReturnFlow.None;
        }
        if (
            pickupInfo.pickup == null
            || pickupInfo.pickup.isTempItem
            || pickupInfo.GetPickupDropSource() != null
            // casting the pickupdef tier to an int for the tier index and back to a string because the tier as a string is the tier name not the index
            // fucking stupid
            || ConfigOptions.ItemTiersWhitelistedList.Contains(((int)PickupCatalog.GetPickupDef(pickupInfo.pickup.pickupIndex).itemTier).ToString())
        )
        {
            return ReturnFlow.None;
        }
        Log.Debug("preventing/nerfing non chest item drop in a hidden realm!");


        if (ConfigOptions.ItemPreventionMode.Value == ConfigOptions.ItemPreventionType.NoDropping || !Run.instance.IsExpansionEnabled(_alloyedCollective))
        {
            return ReturnFlow.SkipOriginal;
        }
        else
        {
            UniquePickup literallyTemp = pickupInfo.pickup;
            literallyTemp.decayValue = 1f;
            pickupInfo.pickup = literallyTemp;
            return ReturnFlow.None;
        }
    }
}