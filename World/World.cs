using game.Definitions;
using Microsoft.Xna.Framework;
using Nez;
using Nez.DeferredLighting;

namespace game
{
    public class World : Scene
    {
        public override void Initialize()
        {
            ClearColor = Color.LightGray;
            AddRenderer(new DefaultRenderer());
            AddRenderer(new DeferredLightingRenderer
            (
                0,
                (int) RenderLayer.Light,
                (int) RenderLayer.Foreground,
                (int) RenderLayer.Player,
                (int) RenderLayer.Items,
                (int) RenderLayer.Background
            ));
            
            var map = new Map(this);
            map.Load();

            var john = CreateEntity("john");
            john.AddComponent(new Hero());
            john.SetPosition(map.SpawnPosition);

            var camera = new FollowCamera(john, FollowCamera.CameraStyle.CameraWindow);
            camera.FocusOffset = new Vector2(0, 225);
            var cameraEntity = CreateEntity("camera");
            cameraEntity.AddComponent(camera);

            var light = new PointLight(Color.White);
            light.SetIntensity(2);
            light.SetRadius(600);
            var lightEntity = CreateEntity("light");
            lightEntity.AddComponent(light).SetRenderLayer((int) RenderLayer.Light);
            lightEntity.SetPosition(new Vector2(Screen.Center.X, 250));

            /*
            var radioEntity = CreateEntity("radio");
            var radio = new Radio();
            radioEntity.AddComponent(radio);
            radioEntity.SetTag((int)Layers.Tag.Interactive);
            
            // Audio setup
            var audioFile = scene.Content.Load<Song>("Sound/ambient.wind-thunder-rain");
            MediaPlayer.Volume = 0.3f;
            MediaPlayer.Play(audioFile);
            */
        }
    }
}