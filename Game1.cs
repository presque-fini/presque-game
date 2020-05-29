using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using Nez.Textures;

namespace game
{
    public class Game1 : Core
    {        
        public Game1(): base(1138, 640, false, true, "Test game")
        { }

        protected override void Initialize()
        {
            base.Initialize();
            DebugRenderEnabled = true;

            var scene = Scene.CreateWithDefaultRenderer(Color.LightGoldenrodYellow);

            // Background image setup
            var landscape = scene.Content.LoadTexture("Scenes/BG_landscape");
            var backgroundEntity = scene.CreateEntity("background");
            backgroundEntity.AddComponent(new SpriteRenderer(landscape));
            backgroundEntity.SetLocalPosition(new Vector2(landscape.Width, landscape.Height));
            backgroundEntity.SetLocalScale(2f);

            // Player setup
            var heroTexture = scene.Content.Load<Texture2D>("Sprites/RX_ANIM_Idle_2");
            var heroSprites = Sprite.SpritesFromAtlas(heroTexture, 64, 64);
            var hero = scene.CreateEntity("hero");
            hero.AddComponent(new BoxCollider());
            hero.SetLocalPosition(new Vector2(400));

            // Ground setup
            var ground = scene.CreateEntity("ground");
            ground.AddComponent(new BoxCollider(0, 640, 1138, 640));

            // Animation setup
            var animator = hero.AddComponent<SpriteAnimator>();
            animator.AddAnimation("Idle", 10f, heroSprites[0], heroSprites[1], heroSprites[2], heroSprites[3], heroSprites[4], heroSprites[5]);
            animator.Play("Idle");

            Scene = scene;
        }
    }
}
