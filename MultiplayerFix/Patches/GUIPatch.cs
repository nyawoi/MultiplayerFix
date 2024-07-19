using System;
using HarmonyLib;
using UnityEngine;

namespace AetharNet.Mods.ZumbiBlocks.MultiplayerFix.Patches;

[HarmonyPatch(typeof(GUI))]
public static class GUIPatch
{
    private static readonly int[] MultiplayerOptionsLineIndices = [101, 102, 103, 104];

    private static int GameState => sc_gamecontrolPatch.Instance.gamestate;
    private static Texture ButtonImage => sc_gamecontrolPatch.Instance.newhud[0];
    
    [HarmonyPrefix]
    [HarmonyPatch(nameof(GUI.Button), [typeof(Rect), typeof(string)])]
    public static bool DisableMultiplayerButtons(string text)
    {
        foreach (var index in MultiplayerOptionsLineIndices)
        {
            if (sc_gamecontrolPatch.CurrentLanguage.frase[index] == text)
            {
                return false;
            }
        }

        return true;
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(nameof(GUI.DrawTexture), [typeof(Rect), typeof(Texture), typeof(ScaleMode), typeof(bool), typeof(float)])]
    public static bool DisableMultiplayerButtonImages(Rect position, Texture image)
    {
        if (GameState != 25 || image != ButtonImage) return true;
        
        return !ValuesMatch(position.yMin, Screen.height / 2 - 50) &&
               !ValuesMatch(position.yMin, Screen.height / 2 - 0);
    }

    private static bool ValuesMatch(float valueOne, float valueTwo, float tolerance = 0.01f)
    {
        return Math.Abs(valueOne - valueTwo) < tolerance;
    }
}
