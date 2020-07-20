using Nez;
using Nez.Sprites;

namespace game.Scenes
{
    internal class Foreground : Component, IUpdatable
    {
        private readonly string animationName;
        private readonly float offset;
        private readonly int renderLayer;
        private readonly int scale;

        public Foreground(string animationName, int renderLayer, int scale, float offset)
        {
            this.animationName = animationName;
            this.renderLayer = renderLayer;
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

            rain.SetRenderLayer(renderLayer);

            Entity.SetPosition(Screen.Center);
            Entity.SetScale(scale);

            rain.Play(animationName);
        }
    }
}