using game.Definitions;
using Nez;
using Nez.Sprites;

namespace game
{
    internal class Foreground : Component, IUpdatable
    {
        private readonly string animationName;
        private readonly float offset;
        private readonly int scale;

        public Foreground(string animationName, int scale, float offset = 1f)
        {
            this.animationName = animationName;
            this.scale = scale;
            this.offset = offset;
        }

        public void Update()
        {
            var cameraPosition = Entity.Scene.Camera.Position;
            Entity.SetPosition(cameraPosition.X * offset, Entity.Position.Y);
        }

        public override void OnAddedToEntity()
        {
            var atlas = Entity.Scene.Content.LoadSpriteAtlas("Content/animations.atlas");
            var rain = Entity.AddComponent<SpriteAnimator>().AddAnimationsFromAtlas(atlas);
            rain.SetRenderLayer((int) Layers.RenderLayer.Foreground);

            Entity.SetPosition(Screen.Center);
            Entity.SetScale(scale);

            rain.Play(animationName);
        }
    }
}