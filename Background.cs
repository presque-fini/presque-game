using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;

namespace game
{
    internal class Background : Component, IUpdatable
    {
        private string texturePath;
        private int renderLayer;
        private Entity backgroundEntity;

        public Background(string texturePath, int renderLayer)
        {
            this.texturePath = texturePath;
            this.renderLayer = renderLayer;
        }
        public override void OnAddedToEntity()
        {
            Texture2D texture = Entity.Scene.Content.LoadTexture(texturePath);
            backgroundEntity = Entity.Scene.CreateEntity("background");

            backgroundEntity.AddComponent(new SpriteRenderer(texture)).SetRenderLayer(renderLayer);
            backgroundEntity.SetPosition(Screen.Center);
            backgroundEntity.SetScale(1.2f);
        }
        void IUpdatable.Update()
        {
            var cameraPosition = Entity.Scene.Camera.Position;
            const float offset = 0.8f;

            backgroundEntity.SetPosition(cameraPosition.X * offset, backgroundEntity.Position.Y);
        }
    }
}