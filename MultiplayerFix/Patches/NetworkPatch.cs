using HarmonyLib;
using UnityEngine;

namespace AetharNet.Mods.ZumbiBlocks.MultiplayerFix.Patches;

[HarmonyPatch(typeof(Network))]
public static class NetworkPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(Network.Connect), [typeof(string), typeof(int), typeof(string)])]
    public static bool RedirectMethod(string password)
    {
        var selectedHost = sc_gamecontrolPatch.Instance.hostData[sc_gamecontrolPatch.Instance.server_select];
        
        if (selectedHost.passwordProtected)
        {
            Network.Connect(selectedHost, password);
        }
        else
        {
            Network.Connect(selectedHost);
        }

        return false;
    }
}
