using Nez;
using Nez.Sprites;

namespace game.Scenes
{
    internal class Background : Component, IUpdatable
    {
        private readonly float offset;
        private readonly int renderLayer;
        private readonly int scale;
        private readonly string texturePath;

        public Background(string texturePath, int renderLayer, int scale, float offset)
        {
            this.texturePath = texturePath;
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
            var texture = Entity.Scene.Content.LoadTexture(texturePath);

            Entity.AddComponent(new SpriteRenderer(texture)).SetRenderLayer(renderLayer);
            Entity.SetPosition(Screen.Center);
            Entity.SetScale(scale);
        }
    }
}