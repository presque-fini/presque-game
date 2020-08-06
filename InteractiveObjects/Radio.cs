using game.Components;
using game.Definitions;
using game.Interfaces;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;

namespace game
{
    public class Radio : SoundEmitter, IUpdatable, IInteractive
    {
        private SoundEmitter soundEmitter;

        /// <summary>
        ///     Grab the radio and move it around.
        /// </summary>
        public void InteractWith()
        {
            if (Entity.Tag == (int) Tag.Active)
                Entity.Position = Entity.Scene.Entities.FindEntity("hero").Position;
        }

        public void Update()
        {
            InteractWith();
            soundEmitter.SetPosition(Entity.Position, Entity.Scene.Entities.FindEntity("hero").Position);
            soundEmitter.Play();
        }

        public override void OnAddedToEntity()
        {
            var radioTexture = Entity.Scene.Content.LoadTexture("Assets/Radio-front");
            var collider = new BoxCollider {IsTrigger = true};
            var renderer = new SpriteRenderer(radioTexture) {RenderLayer = (int) RenderLayer.Items};
            soundEmitter = new SoundEmitter {SoundName = "Sound/effect_radio"};

            Entity.AddComponent(renderer);
            Entity.AddComponent(collider);
            Entity.AddComponent(soundEmitter);
            Entity.SetPosition(new Vector2(400, 600));
            Entity.SetScale(1.5f);
        }
    }
}