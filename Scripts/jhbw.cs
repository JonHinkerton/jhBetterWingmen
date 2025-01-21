using HarmonyLib;
using Kingmaker;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Interfaces;
using Kingmaker.GameModes;
using Kingmaker.Mechanics.Entities;
using Kingmaker.Modding;
using Kingmaker.Networking;
using Kingmaker.PubSubSystem;
using Kingmaker.PubSubSystem.Core;
using Kingmaker.PubSubSystem.Core.Interfaces;
using Kingmaker.View.Spawners;
using Owlcat.Runtime.Core.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class jhbw 
{
    public static BaseUnitEntity Descriptor(this BaseUnitEntity entity)
    {
        return entity;
    }

    [HarmonyPatch]
    public static class Main
    {
        internal static LogChannel log;
        internal static OwlcatModification jhbwpmod;

        [OwlcatModificationEnterPoint]
        public static void Load(OwlcatModification mod)
        {
            jhbwpmod = mod;
            log = mod.Logger;

            Harmony harmony = new(jhbwpmod.Manifest.UniqueName);
            harmony.PatchAll();

            jhbwListener subscriber = new jhbwListener();
            EventBus.Subscribe(subscriber);

            log.Log("jhbw: Load");
        }

        public class jhbwListener : IAreaActivationHandler, ISubscriber
        {
            public void OnAreaActivated()
            {
                if (Game.Instance.CurrentMode == GameModeType.SpaceCombat)
                {
                    log.Log("jhbw: looking for Viper");
                    var AllBaseUnits = Game.Instance?.State?.AllBaseUnits;

                    foreach (BaseUnitEntity unit in AllBaseUnits)
                    {
                        BaseUnitEntity bu = unit.Descriptor();
                        if (bu.OriginalBlueprint.AssetGuid == "ca2239e8737940f9bf106961ef6f490d")
                        {
                            bu.SpawnPosition = bu.SpawnPosition + new Vector3(20, 10, 0);
                            bu.Position = bu.Position + new Vector3(20, 10, 0);
                            log.Log("jhbw: base position is " + unit.SpawnPosition + " position is " + bu.Position);
                        }
                    }
                }
            }
        }
    }
}
//"ca2239e8737940f9bf106961ef6f490d"