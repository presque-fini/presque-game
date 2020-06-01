using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Nez;
using Nez.Sprites;

namespace game
{
    public class Game1 : Core
    {
        public Game1() : base(1280, 720, false, true, "Test game")
        { }

        protected override void Initialize()
        {
            base.Initialize();
            //DebugRenderEnabled = true;
            Scene.SetDefaultDesignResolution(1280, 720, Scene.SceneResolutionPolicy.ShowAllPixelPerfect);

            var scene = Scene.CreateWithDefaultRenderer(Color.LightGoldenrodYellow);

            // Background image setup
            var landscape = scene.Content.LoadTexture("Scenes/background_dark_landscape");
            var backgroundEntity = scene.CreateEntity("background");
            backgroundEntity.AddComponent(new SpriteRenderer(landscape));
            backgroundEntity.SetPosition(new Vector2(landscape.Width / 2, landscape.Height / 2));

            // Foreground image setup
            SpriteAtlas atlas = scene.Content.LoadSpriteAtlas("Content/animations.atlas");
            var foreground = scene.CreateEntity("rain");
            var rain = foreground.AddComponent<SpriteAnimator>().AddAnimationsFromAtlas(atlas);
            rain.RenderLayer = -10;
            foreground.SetPosition(new Vector2(landscape.Width / 2, landscape.Height / 2));
            rain.Play("rain");

            //Radio setup
            var radio = scene.CreateEntity("radio");
            radio.AddComponent(new Radio());

            // Player setup
            var hero = scene.CreateEntity("hero");
            hero.AddComponent(new Hero());

            // Ground setup
            var ground = scene.CreateEntity("ground");
            ground.AddComponent(new BoxCollider(0, landscape.Height - 10, landscape.Width, 10));

            // Audio setup
            var audioFile = scene.Content.Load<Song>("Sound/ambient.wind-thunder-rain");
            MediaPlayer.Volume = 0.3f;
            MediaPlayer.Play(audioFile);

            Scene = scene;
        }
    }
}