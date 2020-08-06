using game.Definitions;
using Microsoft.Xna.Framework;
using Nez;

namespace game
{
    public class Map
    {
        private const string name = "Content/World/example.tmx";
        private readonly Scene scene;

        public Map(Scene scene)
        {
            this.scene = scene;
        }

        public Vector2 SpawnPosition { get; private set; } = Vector2.Zero;

        public void Load()
        {
            var tiledMap = scene.Content.LoadTiledMap(name);
            var tiledEntity = scene.CreateEntity("tiledEntity");
            tiledEntity.AddComponent(new TiledMapRenderer(tiledMap, "Background"))
                .SetRenderLayer((int) RenderLayer.Player);

            var objectLayer = tiledMap.GetObjectGroup("Objects").Objects;
            foreach (var tmxObject in objectLayer)
                if (tmxObject.Name == "Spawn")
                    SpawnPosition = new Vector2(tmxObject.X, tmxObject.Y);
        }
    }
}