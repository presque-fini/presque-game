using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Nez;
using Nez.Sprites;
using Nez3D;

namespace game
{
    public class Game1 : Core
    {
        public Game1() : base(1280, 720, false, true, "Test game")
        { }

        protected override void Initialize()
        {
            base.Initialize();
            DebugRenderEnabled = true;
            Scene.SetDefaultDesignResolution(1280, 720, Scene.SceneResolutionPolicy.ShowAllPixelPerfect);

            var scene = Scene.CreateWithDefaultRenderer(Color.LightGoldenrodYellow);

            // Background image setup
            var landscape = scene.Content.LoadTexture("Scenes/background_dark_landscape");
            var backgroundEntity = scene.CreateEntity("background");
            backgroundEntity.AddComponent(new SpriteRenderer(landscape));
            backgroundEntity.SetPosition(new Vector2(landscape.Width/2, landscape.Height/2));

            // Player setup
            var hero = scene.CreateEntity("hero");
            hero.AddComponent(new Hero());
            hero.AddComponent(new BoxCollider());
            hero.AddComponent(new Mover());
            hero.Position = new Vector2(400);

            // Ground setup
            var ground = scene.CreateEntity("ground");
            ground.AddComponent(new BoxCollider(0, landscape.Height-10, landscape.Width, 10));

            // Audio setup
            // var audioFile = scene.Content.Load<Song>("Sound/radio");
            // MediaPlayer.Play(audioFile);

            Scene = scene;
        }
    }
}