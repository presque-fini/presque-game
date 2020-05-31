using Microsoft.Xna.Framework;
using Nez;

namespace game
{
    public class Game1 : Core
    {
        public Game1() : base(1138, 640, false, true, "Test game")
        { }

        protected override void Initialize()
        {
            base.Initialize();
            DebugRenderEnabled = true;
            Scene.SetDefaultDesignResolution(1138, 640, Scene.SceneResolutionPolicy.ShowAllPixelPerfect);

            var scene = Scene.CreateWithDefaultRenderer(Color.LightGoldenrodYellow);

            // Background image setup
            /*
            var landscape = scene.Content.LoadTexture("Scenes/BG_landscape");
            var backgroundEntity = scene.CreateEntity("background");
            backgroundEntity.AddComponent(new SpriteRenderer(landscape));
            backgroundEntity.SetLocalPosition(new Vector2(landscape.Width, landscape.Height));
            backgroundEntity.SetLocalScale(2f);
            */

            // Player setup
            var hero = scene.CreateEntity("hero");
            hero.AddComponent(new Hero());
            hero.AddComponent(new BoxCollider());
            hero.AddComponent(new Mover());
            hero.Position = new Vector2(400);
            hero.Scale = new Vector2(2);

            // Ground setup
            var ground = scene.CreateEntity("ground");
            ground.AddComponent(new BoxCollider(0, 600, 1138, 640));

            Scene = scene;
        }
    }
}