using System;

namespace DataPersistence.Gameplay
{
    [Serializable]
    public class GameTimeData : MonoBehaviourData
    {
        public float dayTimeProgress;

        public GameTimeData() { }
        public GameTimeData(float dayTimeProgress)
        {
            this.dayTimeProgress = dayTimeProgress;
        }
    }
}