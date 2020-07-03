using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;

namespace game
{
    internal class Foreground : Component, IUpdatable
    {
        private string animationName;
        private int renderLayer;
        private int scale;
        private float offset;

        public Foreground(string animationName, int renderLayer, int scale, float offset)
        {
            this.animationName = animationName;
            this.renderLayer = renderLayer;
            this.scale = scale;
            this.offset = offset;
        }

        public override void OnAddedToEntity()
        {
            var atlas = Entity.Scene.Content.LoadSpriteAtlas("Content/animations.atlas");
            var rain = Entity.AddComponent<SpriteAnimator>().AddAnimationsFromAtlas(atlas);

            rain.SetRenderLayer(renderLayer);

            Entity.SetPosition(Screen.Center);
            Entity.SetScale(scale);

            rain.Play(animationName);
        }

        void IUpdatable.Update()
        {
            var cameraPosition = Entity.Scene.Camera.Position;
            Entity.SetPosition(cameraPosition.X * offset, Entity.Position.Y);
        }
    }
}