using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Nez;
using Nez.DeferredLighting;

namespace game
{
    public class Game1 : Core
    {
        public Game1() : base()
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
            const int MOVABLE = 1;

            Scene scene = Scene.CreateWithDefaultRenderer(Color.LightGoldenrodYellow);
            scene.SetDesignResolution(1280, 720, Scene.SceneResolutionPolicy.None);
            scene.AddRenderer(new DeferredLightingRenderer(0, LIGHT_LAYER, FOREGROUND_LAYER, PLAYER_LAYER, OBJECT_LAYER, BACKGROUND_LAYER));

            SetupLight(LIGHT_LAYER, scene);

            var background = scene.CreateEntity("background");
            background.AddComponent(new Background("Scenes/background_dark_landscape", BACKGROUND_LAYER, 2, 0.8f));

            var foreground = scene.CreateEntity("foreground");
            foreground.AddComponent(new Foreground("rain", FOREGROUND_LAYER, 1, 1));

            //Radio setup
            var radioEntity = scene.CreateEntity("radio");
            radioEntity.AddComponent(new Radio(BACKGROUND_LAYER));
            radioEntity.SetTag(MOVABLE);

            // Player setup
            Entity hero = scene.CreateEntity("hero");
            hero.AddComponent(new Hero(PLAYER_LAYER, LIGHT_LAYER, MOVABLE));
            hero.SetPosition(new Vector2(Screen.Center.X, 600));

            // Camera
            FollowCamera camera = new FollowCamera(hero, FollowCamera.CameraStyle.CameraWindow);
            camera.FocusOffset = new Vector2(0, 225);
            Entity cameraEntity = scene.CreateEntity("camera");
            cameraEntity.AddComponent(camera);

            // Ground setup
            Entity ground = scene.CreateEntity("ground");
            ground.AddComponent(new BoxCollider(0, Screen.Height - 10, Screen.Width * 2, 10));

            // Audio setup
            Song audioFile = scene.Content.Load<Song>("Sound/ambient.wind-thunder-rain");
            MediaPlayer.Volume = 0.3f;
            MediaPlayer.Play(audioFile);

            Scene = scene;
        }

        private static void SetupLight(int LIGHT_LAYER, Scene scene)
        {
            PointLight light = new PointLight(Color.White);
            light.SetIntensity(2f);
            light.SetRadius(600f);
            Entity lightEntity = scene.CreateEntity("light");
            lightEntity.AddComponent(light).SetRenderLayer(LIGHT_LAYER);
            lightEntity.SetPosition(new Vector2(Screen.Center.X, 250));
        }
    }
}