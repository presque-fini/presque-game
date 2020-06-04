using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Nez;
using Nez.DeferredLighting;
using Nez.Sprites;
using Nez.Textures;

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

            const int LIGHT_LAYER = 1;
            const int FOREGROUND_LAYER = 5;
            const int PLAYER_LAYER = 6;
            const int OBJECT_LAYER = 7;
            const int BACKGROUND_LAYER = 8;

            Scene.SetDefaultDesignResolution(1280, 720, Scene.SceneResolutionPolicy.ShowAllPixelPerfect);
            var scene = Scene.CreateWithDefaultRenderer(Color.LightGoldenrodYellow);

            // Background image setup
            var landscape = scene.Content.LoadTexture("Scenes/background_dark_landscape");
            var backgroundEntity = scene.CreateEntity("background");
            backgroundEntity.AddComponent(new SpriteRenderer(landscape)).SetRenderLayer(BACKGROUND_LAYER);
            backgroundEntity.SetPosition(Screen.Center);

            // Foreground image setup
            SpriteAtlas atlas = scene.Content.LoadSpriteAtlas("Content/animations.atlas");
            var foreground = scene.CreateEntity("rain");
            var rain = foreground.AddComponent<SpriteAnimator>().AddAnimationsFromAtlas(atlas);
            rain.RenderLayer = FOREGROUND_LAYER;
            foreground.SetPosition(Screen.Center);
            rain.Play("rain");

            //Radio setup
            var radio = scene.CreateEntity("radio");
            radio.AddComponent(new Radio(BACKGROUND_LAYER));

            // Player setup
            var hero = scene.CreateEntity("hero");
            hero.AddComponent(new Hero(PLAYER_LAYER));
            hero.SetPosition(new Vector2(Screen.Center.X, 600));

            // Ground setup
            var ground = scene.CreateEntity("ground");
            ground.AddComponent(new BoxCollider(0, Screen.Height - 10, Screen.Width, 10));

            // Audio setup
            var audioFile = scene.Content.Load<Song>("Sound/ambient.wind-thunder-rain");
            MediaPlayer.Volume = 0.3f;
            MediaPlayer.Play(audioFile);

            // Deffered lightning
            // add the DeferredLightingRenderer to your Scene specifying which renderLayer contains your lights and an arbitrary number of renderLayers for it to render
            var deferredRenderer = scene.AddRenderer(new DeferredLightingRenderer(0, LIGHT_LAYER, FOREGROUND_LAYER, PLAYER_LAYER, OBJECT_LAYER, BACKGROUND_LAYER));
            var light = scene.CreateEntity("light");
            light.AddComponent(new PointLight(Color.White)).SetRenderLayer(LIGHT_LAYER);
            light.SetPosition(new Vector2(Screen.Center.X, 600));

            // optionally set ambient lighting
            // deferredRenderer.SetAmbientColor(Color.Blue);

            Scene = scene;
        }
    }
}