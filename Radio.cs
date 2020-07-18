using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using System.Linq;

namespace game
{
    public class Radio : Component, IUpdatable
    {
        private SoundEffectInstance soundEffectInstance;
        private AudioListener listener;
        private AudioEmitter emitter;
        private Texture2D radioTexture;

        private void Move()
        {
            if (Entity.Tag == (int) Game1.Tag.Active)
                Entity.Position = Entity.Scene.Entities.FindEntity("hero").Position;
        }

        public Radio()
        {
        }

        public override void OnAddedToEntity()
        {
            listener = new AudioListener();
            emitter = new AudioEmitter();
            var collider = new BoxCollider {IsTrigger = true};

            radioTexture = Entity.Scene.Content.LoadTexture("Assets/Radio-front");
            Entity.AddComponent(new SpriteRenderer(radioTexture)).SetRenderLayer((int) Game1.RenderLayer.Items);
            Entity.AddComponent(collider);
            Entity.SetPosition(new Vector2(400, 600));
            Entity.SetScale(1.5f);

            var radioSound = Entity.Scene.Content.LoadSoundEffect("Sound/effect_radio");
            soundEffectInstance = radioSound.CreateInstance();
        }

        void IUpdatable.Update()
        {
            listener.Position = new Vector3(Entity.Scene.Entities.FindEntity("hero").Position, 0);
            emitter.Position = new Vector3(Entity.Position, 0);
            soundEffectInstance.Apply3D(listener, emitter);

            Move();

            soundEffectInstance.Play();
        }
    }
}