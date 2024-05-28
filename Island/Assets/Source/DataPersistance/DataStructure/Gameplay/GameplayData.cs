using System;

namespace DataPersistence.Gameplay
{
    [Serializable]
    public class GameplayData
    {
        public string displayName;

        public int difficultyID;

        public PlayerData playerData;

        public float dayTimeProgress;

        public BananaTreesData bananaTreesData;

        public DynamicObjectsData dynamicObjectsData;

        public GameplayData() {
            displayName = "New game started by " + Environment.UserName + " at " + DateTime.Now;
        }

        public GameplayData(int difficultyID)
        {
            this.difficultyID = difficultyID;
            displayName = "New game started by " + Environment.UserName + " at " + DateTime.Now;
        }
    }
}
