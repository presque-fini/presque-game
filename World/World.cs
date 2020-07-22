﻿using game.Characters;
using game.Definitions;
using Microsoft.Xna.Framework;
using Nez;
using Nez.DeferredLighting;
using Nez.Tiled;

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
                (int) Layers.RenderLayer.Light,
                (int) Layers.RenderLayer.Foreground,
                (int) Layers.RenderLayer.Player,
                (int) Layers.RenderLayer.Items,
                (int) Layers.RenderLayer.Background
            ));

            var tiledMap = Content.LoadTiledMap("Content/World/example.tmx");
            var tiledEntity = CreateEntity("tiledEntity");
            tiledEntity.AddComponent(new TiledMapRenderer(tiledMap, "Background"))
                .SetRenderLayer((int) Layers.RenderLayer.Player);

            var objectLayer = tiledMap.GetObjectGroup("Objects").Objects;
            Vector2 spawn = Vector2.Zero;
            foreach (var tmxObject in objectLayer)
            {
                if (tmxObject.Name == "Spawn")
                    spawn = new Vector2(tmxObject.X, tmxObject.Y);
            }
            var john = CreateEntity("john");
            john.AddComponent(new Hero());
            john.SetPosition(new Vector2(spawn.X, spawn.Y));

            var camera = new FollowCamera(john, FollowCamera.CameraStyle.CameraWindow);
            camera.FocusOffset = new Vector2(0, 225);
            var cameraEntity = CreateEntity("camera");
            cameraEntity.AddComponent(camera);

            var light = new PointLight(Color.White);
            light.SetIntensity(2);
            light.SetRadius(600);
            var lightEntity = CreateEntity("light");
            lightEntity.AddComponent(light).SetRenderLayer((int) Layers.RenderLayer.Light);
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