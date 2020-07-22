using Nez;

namespace game
{
    public class PresqueGame : Core
    {
        public PresqueGame() : base(1280, 720, false, "Presque game")
        {
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            // RegisterGlobalManager(new ImGuiManager());
            Scene.SetDefaultDesignResolution(1280, 720, Scene.SceneResolutionPolicy.ShowAllPixelPerfect);

            Scene = new World();
        }
    }
}