using System;

namespace DataPersistence
{
    [Serializable]
    public class GameplayData
    {
        //[NonSerialized] private string displayName;

        public int difficultyID;

        public GameplayData() { }

        public GameplayData(int difficultyID)
        {
            //this.displayName = displayName;
            this.difficultyID = difficultyID;
        }
    }
}
