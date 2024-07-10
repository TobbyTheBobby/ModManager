using ModManager;
using UnityEngine;

namespace ModManagerUI.UiSystem
{
    public class StatusIconLoader : Singleton<StatusIconLoader>
    {
        private Sprite? _unknownSprite;
        private Sprite? _compatibleSprite;
        private Sprite? _incompatibleSprite;

        public Sprite UnknownSprite => _unknownSprite ??= LoadSprite("ui/images/mods/status-unknown");
        public Sprite CompatibleSprite => _compatibleSprite ??= LoadSprite("ui/images/mods/status-compatible");
        public Sprite IncompatibleSprite => _incompatibleSprite ??= LoadSprite("ui/images/mods/status-incompatible");

        private static readonly float ImageSizeMultiplier = 0.6f;

        private static Sprite LoadSprite(string path)
        {
            var texture = ModManagerPanel.AssetLoader.Load<Texture2D>(path);
            // Scaling doesnt work ¯\_(ツ)_/¯
            // texture.Reinitialize(Mathf.RoundToInt(texture.width * ImageSizeMultiplier), Mathf.RoundToInt(texture.height * ImageSizeMultiplier));
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }
}