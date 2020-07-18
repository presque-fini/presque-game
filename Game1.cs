using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Nez;
using Nez.DeferredLighting;

namespace game
{
    public class Game1 : Core
    {
        public Game1() : base(1280, 720)
        {
        }

        public enum RenderLayer
        {
            None,
            Light,
            Foreground,
            Player,
            Items,
            Background
        }

        public enum PhysicsLayer
        {
            None,
            Player,
            Background
        }

        public enum Tag
        {
            None,
            Interactive,
            Active,
            Inactive
        }

        protected override void Initialize()
        {
            base.Initialize();
            DebugRenderEnabled = true;

            var scene = Scene.CreateWithDefaultRenderer(Color.LightGoldenrodYellow);
            scene.SetDesignResolution(1280, 720, Scene.SceneResolutionPolicy.ShowAllPixelPerfect);
            scene.AddRenderer(new DeferredLightingRenderer(0, (int) RenderLayer.Light, (int) RenderLayer.Foreground,
                (int) RenderLayer.Player, (int) RenderLayer.Items, (int) RenderLayer.Background));

            SetupLight(scene);

            var background = scene.CreateEntity("background");
            background.AddComponent(new Background("Scenes/background_dark_landscape", (int) RenderLayer.Background, 2,
                0.8f));

            var foreground = scene.CreateEntity("foreground");
            foreground.AddComponent(new Foreground("rain", (int) RenderLayer.Foreground, 1, 1));

            //Radio setup
            var radioEntity = scene.CreateEntity("radio");
            var radio = new Radio();
            radioEntity.AddComponent(radio);
            radioEntity.SetTag((int) Tag.Interactive);

            // Player setup
            var hero = scene.CreateEntity("hero");
            hero.AddComponent(new Hero());
            hero.SetPosition(new Vector2(Screen.Center.X, 600));

            // Camera
            var camera = new FollowCamera(hero, FollowCamera.CameraStyle.CameraWindow);
            camera.FocusOffset = new Vector2(0, 225);
            var cameraEntity = scene.CreateEntity("camera");
            cameraEntity.AddComponent(camera);

            // Ground setup
            var ground = scene.CreateEntity("ground");
            var groundCollider = new BoxCollider(0, Screen.Height - 10, Screen.Width * 2, 10)
            {
                CollidesWithLayers = (int) PhysicsLayer.Player
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
            lightEntity.AddComponent(light).SetRenderLayer((int) RenderLayer.Light);
            lightEntity.SetPosition(new Vector2(Screen.Center.X, 250));
        }
    }
}