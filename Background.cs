using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;

namespace game
{
    internal class Background : Component, IUpdatable
    {
        private string texturePath;
        private int renderLayer;
        private int scale;
        private float offset;

        public Background(string texturePath, int renderLayer, int scale, float offset)
        {
            this.texturePath = texturePath;
            this.renderLayer = renderLayer;
            this.scale = scale;
            this.offset = offset;
        }

        public override void OnAddedToEntity()
        {
            var texture = Entity.Scene.Content.LoadTexture(texturePath);

            Entity.AddComponent(new SpriteRenderer(texture)).SetRenderLayer(renderLayer);
            Entity.SetPosition(Screen.Center);
            Entity.SetScale(scale);
        }

        void IUpdatable.Update()
        {
            var cameraPosition = Entity.Scene.Camera.Position;
            Entity.SetPosition(cameraPosition.X * offset, Entity.Position.Y);
        }
    }
}