using game.Characters;
using game.Definitions;
using game.InteractiveObjects;
using game.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Nez;
using Nez.DeferredLighting;
using Nez.ImGuiTools;

namespace game
{
    public class PresqueGame : Core
    {
        public PresqueGame() : base(1920, 1080)
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
            DebugRenderEnabled = true;

            RegisterGlobalManager(new ImGuiManager());

            var scene = Scene.CreateWithDefaultRenderer(Color.LightGoldenrodYellow);
            scene.SetDesignResolution(1280, 720, Scene.SceneResolutionPolicy.ShowAllPixelPerfect);
            scene.AddRenderer(new DeferredLightingRenderer(0, (int) Layers.RenderLayer.Light,
                (int) Layers.RenderLayer.Foreground,
                (int) Layers.RenderLayer.Player, (int) Layers.RenderLayer.Items, (int) Layers.RenderLayer.Background));

            SetupLight(scene);

            var background = scene.CreateEntity("background");
            background.AddComponent(new Background("Scenes/background_dark_landscape",
                (int) Layers.RenderLayer.Background, 2,
                0.8f));

            var foreground = scene.CreateEntity("foreground");
            foreground.AddComponent(new Foreground("rain", (int) Layers.RenderLayer.Foreground, 1, 1));

            //Radio setup
            var radioEntity = scene.CreateEntity("radio");
            var radio = new Radio();
            radioEntity.AddComponent(radio);
            radioEntity.SetTag((int) Layers.Tag.Interactive);

            // Player setup
            var hero = scene.CreateEntity("hero");
            hero.AddComponent(new Hero());
            hero.SetPosition(new Vector2(Screen.Center.X, 600));

            // Camera
            var camera = new FollowCamera(hero, FollowCamera.CameraStyle.CameraWindow)
            {
                FocusOffset = new Vector2(0, 225)
            };
            var cameraEntity = scene.CreateEntity("camera");
            cameraEntity.AddComponent(camera);

            // Ground setup
            var ground = scene.CreateEntity("ground");
            var groundCollider = new BoxCollider(0, Screen.Height - 10, Screen.Width * 2, 10)
            {
                CollidesWithLayers = (int) Layers.PhysicsLayer.Player
            };
            ground.AddComponent(groundCollider);

            // Audio setup
            var audioFile = scene.Content.Load<Song>("Sound/ambient.wind-thunder-rain");
            MediaPlayer.Volume = 0.3f;
            MediaPlayer.Play(audioFile);

            Scene = scene;
        }

        private static void SetupLight(Scene scene)
        {
            var light = new PointLight(Color.White);
            light.SetIntensity(2f);
            light.SetRadius(600f);

            var lightEntity = scene.CreateEntity("light");
            lightEntity.AddComponent(light).SetRenderLayer((int) Layers.RenderLayer.Light);
            lightEntity.SetPosition(new Vector2(Screen.Center.X, 250));
        }
    }
}