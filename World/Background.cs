using game.Definitions;
using Nez;
using Nez.Sprites;

namespace game
{
    internal class Background : Component, IUpdatable
    {
        private readonly float offset;
        private readonly int scale;
        private readonly string texturePath;

        public Background(string texturePath, int scale, float offset = 0.8f)
        {
            this.texturePath = texturePath;
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

            Entity.AddComponent(new SpriteRenderer(texture)).SetRenderLayer((int) RenderLayer.Background);
            Entity.SetPosition(Screen.Center);
            Entity.SetScale(scale);
        }
    }
}