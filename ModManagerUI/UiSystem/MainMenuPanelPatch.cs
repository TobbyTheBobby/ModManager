using HarmonyLib;
using Timberborn.CoreUI;
using Timberborn.MainMenuPanels;
using UnityEngine.UIElements;

namespace ModManagerUI.UiSystem
{
    [HarmonyPatch]
    public class MainMenuPanelPatch
    {
        [HarmonyPatch(typeof(MainMenuPanel), "Load")]
        [HarmonyPostfix]
        public static void MainMenuPanelPostfix(ref VisualElement ____root)
        {
            VisualElement root = ____root.Query("MainMenuPanel");
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
