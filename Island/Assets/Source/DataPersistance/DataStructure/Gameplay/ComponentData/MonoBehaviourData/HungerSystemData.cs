using System;

namespace DataPersistence.Gameplay
{
    [Serializable]
    public class HungerSystemData : MonoBehaviourData
    {
        public float satiety;
        public float health;

        public HungerSystemData() { }
        public HungerSystemData(float health, float satiety)
        {
            this.health = health;
            this.satiety = satiety;
        }
    }
}