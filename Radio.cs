using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Nez;
using Nez.Sprites;

namespace game
{
    internal class Radio : Component, IUpdatable
    {
        private SoundEffectInstance soundEffect;
        private AudioListener listener;
        private AudioEmitter emitter;

        public override void OnAddedToEntity()
        {
            listener = new AudioListener();
            emitter = new AudioEmitter();

            var radioTexture = Entity.Scene.Content.LoadTexture("Assets/Radio-front");
            Entity.AddComponent(new SpriteRenderer(radioTexture));
            Entity.SetPosition(new Vector2(400, 600));
            Entity.SetScale(1.5f);

            var radioSound = Entity.Scene.Content.LoadSoundEffect("Sound/effect_radio");
            soundEffect = radioSound.CreateInstance();
        }

        void IUpdatable.Update()
        {
            listener.Position = new Vector3(Entity.Scene.Entities.FindEntity("hero").Position, 0);
            emitter.Position = new Vector3(Entity.Position, 0);
            soundEffect.Apply3D(listener, emitter);

            if (soundEffect.State != SoundState.Playing)
            {
                soundEffect.Play();
            }
        }
    }
}