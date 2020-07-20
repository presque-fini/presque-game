using game.Definitions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Nez;
using Nez.Sprites;

namespace game.InteractiveObjects
{
    public class Radio : Component, IUpdatable
    {
        private AudioEmitter emitter;
        private AudioListener listener;
        private SoundEffectInstance soundEffectInstance;

        public void Update()
        {
            listener.Position = new Vector3(Entity.Scene.Entities.FindEntity("hero").Position, 0);
            emitter.Position = new Vector3(Entity.Position, 0);
            soundEffectInstance.Apply3D(listener, emitter);

            Move();

            soundEffectInstance.Play();
        }

        private void Move()
        {
            if (Entity.Tag == (int) Layers.Tag.Active)
                Entity.Position = Entity.Scene.Entities.FindEntity("hero").Position;
        }

        public override void OnAddedToEntity()
        {
            listener = new AudioListener();
            emitter = new AudioEmitter();
            var collider = new BoxCollider {IsTrigger = true};

            var radioTexture = Entity.Scene.Content.LoadTexture("Assets/Radio-front");
            Entity.AddComponent(new SpriteRenderer(radioTexture)).SetRenderLayer((int) Layers.RenderLayer.Items);
            Entity.AddComponent(collider);
            Entity.SetPosition(new Vector2(400, 600));
            Entity.SetScale(1.5f);

            var radioSound = Entity.Scene.Content.LoadSoundEffect("Sound/effect_radio");
            soundEffectInstance = radioSound.CreateInstance();
        }
    }
}