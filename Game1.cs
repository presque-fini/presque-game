﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Nez;
using Nez.DeferredLighting;
using Nez.Sprites;

namespace game
{
    public class Game1 : Core
    {
        public Game1() : base()
        { }

        protected override void Initialize()
        {
            base.Initialize();

            const int LIGHT_LAYER = 1;
            const int FOREGROUND_LAYER = 5;
            const int PLAYER_LAYER = 6;
            const int OBJECT_LAYER = 7;
            const int BACKGROUND_LAYER = 8;

            var scene = Scene.CreateWithDefaultRenderer(Color.LightGoldenrodYellow);
            scene.SetDesignResolution(1280, 720, Scene.SceneResolutionPolicy.None);
            scene.AddRenderer(new DeferredLightingRenderer(0, LIGHT_LAYER, FOREGROUND_LAYER, PLAYER_LAYER, OBJECT_LAYER, BACKGROUND_LAYER));

            SetupLight(LIGHT_LAYER, scene);

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
            hero.AddComponent(new Hero(PLAYER_LAYER, LIGHT_LAYER));
            hero.SetPosition(new Vector2(Screen.Center.X, 600));

            // Camera
            var camera = new FollowCamera(hero, FollowCamera.CameraStyle.CameraWindow);
            camera.FocusOffset = new Vector2(0, 225);
            Debug.Log(camera.MapSize);
            var cameraEntity = scene.CreateEntity("camera");
            cameraEntity.AddComponent(camera);

            // Ground setup
            var ground = scene.CreateEntity("ground");
            ground.AddComponent(new BoxCollider(0, Screen.Height - 10, Screen.Width, 10));

            // Audio setup
            var audioFile = scene.Content.Load<Song>("Sound/ambient.wind-thunder-rain");
            MediaPlayer.Volume = 0.3f;
            MediaPlayer.Play(audioFile);

            Scene = scene;
        }

        private static void SetupLight(int LIGHT_LAYER, Scene scene)
        {
            var light = new PointLight(Color.White);
            light.SetIntensity(2f);
            light.SetRadius(600f);
            var lightEntity = scene.CreateEntity("light");
            lightEntity.AddComponent(light).SetRenderLayer(LIGHT_LAYER);
            lightEntity.SetPosition(new Vector2(Screen.Center.X, 250));
        }
    }
}