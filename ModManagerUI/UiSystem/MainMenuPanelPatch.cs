using HarmonyLib;
using Timberborn.CoreUI;
using Timberborn.MainMenuPanels;
using UnityEngine.UIElements;

namespace ModManagerUI.UiSystem
{
    [HarmonyPatch]
    public class MainMenuPanelPatch
    {
        [HarmonyPatch(typeof(MainMenuPanel), "GetPanel")]
        [HarmonyPostfix]
        public static void MainMenuPanelPostfix(ref VisualElement __result)
        {
            VisualElement root = __result.Query("MainMenuPanel");
            var button = new LocalizableButton
            {
                text = "Mod browser"
            };
            button.AddToClassList("menu-button");
            button.AddToClassList("menu-button--stretched");
            button.clicked += () => ModManagerPanel.Instance.OpenOptionsDelegate();
            root.Insert(7, button);
        }
    }
}
