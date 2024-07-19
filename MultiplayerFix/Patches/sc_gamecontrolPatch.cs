using HarmonyLib;
using UnityEngine;

namespace AetharNet.Mods.ZumbiBlocks.MultiplayerFix.Patches;

[HarmonyPatch(typeof(sc_gamecontrol))]
public static class sc_gamecontrolPatch
{
    public static sc_gamecontrol Instance;
    public static sc_linguajar CurrentLanguage => Instance.language[Instance.languageset];
    
    [HarmonyPostfix]
    [HarmonyPatch(nameof(sc_gamecontrol.Start))]
    public static void RetrieveLanguagePack(sc_gamecontrol __instance)
    {
        // Store `sc_gamecontrol` instance to provide access to fields
        Instance = __instance;

        // Set the game to broadcast server
        Instance.saveinlist = true;
        // Set the game to use NAT punch-through if necessary
        Instance.usarNat = !Network.HavePublicAddress();
    }
}
