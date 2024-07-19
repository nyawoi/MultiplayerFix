using System.Net;
using System.Net.Sockets;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace AetharNet.Mods.ZumbiBlocks.MultiplayerFix;

[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
public class MultiplayerFix : BaseUnityPlugin
{
    public const string PluginGUID = "AetharNet.Mods.ZumbiBlocks.MultiplayerFix";
    public const string PluginAuthor = "wowi";
    public const string PluginName = "MultiplayerFix";
    public const string PluginVersion = "0.1.2";

    internal new static ManualLogSource Logger;

    private static ConfigEntry<string> ServerAddress;
    private static ConfigEntry<int> MasterPort;
    private static ConfigEntry<int> FacilitatorPort;

    private void Awake()
    {
        Logger = base.Logger;

        ServerAddress = Config.Bind(
            "Network",
            "ServerAddress",
            "server.zumbi.wiki",
            "The IP address or hostname of the server");
        
        MasterPort = Config.Bind(
            "Network",
            "MasterPort",
            MasterServer.port,
            "The port the MasterServer service is running on");
        
        FacilitatorPort = Config.Bind(
            "Network",
            "FacilitatorPort",
            Network.natFacilitatorPort,
            "The port the Facilitator service is running on");
        
        MasterServer.ipAddress = ResolveServer(ServerAddress.Value);
        MasterServer.port = MasterPort.Value;
        Network.natFacilitatorIP = MasterServer.ipAddress;
        Network.natFacilitatorPort = FacilitatorPort.Value;
        
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGUID);
    }

    private static string ResolveServer(string server)
    {
        foreach (var address in Dns.GetHostEntry(server).AddressList)
        {
            if (address.AddressFamily == AddressFamily.InterNetwork)
            {
                return address.ToString();
            }
        }

        return null;
    }
}
