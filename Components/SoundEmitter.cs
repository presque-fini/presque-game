using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Nez;

namespace game.Components
{
    public class SoundEmitter : Component
    {
        private readonly AudioEmitter emitter;
        private readonly AudioListener listener;
        private SoundEffectInstance soundEffectInstance;

        public SoundEmitter()
        {
            listener = new AudioListener();
            emitter = new AudioEmitter();
        }

        public string SoundName { get; set; }

        public override void OnAddedToEntity()
        {
            var soundEffect = Entity.Scene.Content.LoadSoundEffect(SoundName);
            soundEffectInstance = soundEffect.CreateInstance();
        }

        public void SetPosition(Vector2 emitterPosition)
        {
            emitter.Position = new Vector3(emitterPosition, 0);
            soundEffectInstance?.Apply3D(listener, emitter);
        }

        public void SetPosition(Vector2 emitterPosition, Vector2 listenerPosition)
        {
            emitter.Position = new Vector3(emitterPosition, 0);
            listener.Position = new Vector3(listenerPosition, 0);
            soundEffectInstance?.Apply3D(listener, emitter);
        }

        public void Play()
        {
            soundEffectInstance?.Play();
        }

        public void Pause()
        {
            soundEffectInstance?.Pause();
        }
    }
}