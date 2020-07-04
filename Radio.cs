using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;

namespace game
{
    internal class Radio : Component, IUpdatable
    {
        private SoundEffectInstance soundEffectInstance;
        private AudioListener listener;
        private AudioEmitter emitter;
        private readonly int renderLayer;
        private Texture2D radioTexture;

        public Rectangle GetBounds()
        {
            return new Rectangle(Entity.Position.ToPoint(), new Point(radioTexture.Width, radioTexture.Height));
        }

        public Radio(int renderLayer)
        {
            this.renderLayer = renderLayer;
        }

        public override void OnAddedToEntity()
        {
            listener = new AudioListener();
            emitter = new AudioEmitter();

            radioTexture = Entity.Scene.Content.LoadTexture("Assets/Radio-front");
            Entity.AddComponent(new SpriteRenderer(radioTexture)).SetRenderLayer(renderLayer);
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

            if (soundEffectInstance.State != SoundState.Playing)
            {
                soundEffectInstance.Play();
            }
        }
    }
}