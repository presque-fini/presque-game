using System.Collections.Generic;
using game.Definitions;
using Nez;

namespace game
{
    public class InteractiveObjects
    {
        private IEnumerable<Entity> interactiveEntitiesList;
        private Scene scene;

        public InteractiveObjects(Scene scene)
        {
            this.scene = scene;
        }

        public IEnumerable<Entity> GetAll()
        {
            if (interactiveEntitiesList == null)
            {
                CreateList();
            }

            return interactiveEntitiesList;
        }

        public IEnumerable<Entity> CreateList()
        {
            interactiveEntitiesList = scene.FindEntitiesWithTag((int) Tag.Interactive);
            return interactiveEntitiesList;
        }
    }
}