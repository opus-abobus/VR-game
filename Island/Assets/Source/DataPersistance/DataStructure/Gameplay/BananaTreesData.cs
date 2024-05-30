using System;
using UnityEngine;

namespace DataPersistence.Gameplay
{
    [Serializable]
    public class BananaTreesData
    {
        public BananaTreeData[] data;

        public BananaTreesData() { }
        public BananaTreesData(BananaTreeData[] data)
        {
            this.data = data;
        }
    }

    [Serializable]
    public class BananaTreeData
    {
        public string key;

        public BananaRipeningData ripeningData;
        public BananaGrowthData growthData;

        public BananaTreeData() { }
        public BananaTreeData(string key, BananaRipeningData ripeningData, BananaGrowthData growthData)
        {
            this.key = key;
            this.ripeningData = ripeningData;
            this.growthData = growthData;
        }

        [Serializable]
        public class BananaRipeningData
        {
            public bool allowRipening;
            public BananaRipening.RipePhase ripePhase;
            public float ripeProgressPhase;
            public float phaseTimeInSeconds;
            public bool isBananaFallen;

            public BananaRipeningData() { }
            public BananaRipeningData(bool allowRipening, BananaRipening.RipePhase ripePhase, 
                float ripeProgressPhase, float phaseTimeInSeconds, bool isBananaFallen)
            {
                this.allowRipening = allowRipening;
                this.ripePhase = ripePhase;
                this.ripeProgressPhase = ripeProgressPhase;
                this.phaseTimeInSeconds = phaseTimeInSeconds;
                this.isBananaFallen = isBananaFallen;
            }
        }

        [Serializable]
        public class BananaGrowthData
        {
            public bool allowGrowth;
            public float growthProgress;
            public Vector3 startScale, endScale;
            public float timeToGrowthInSeconds;
            public bool hasGrown;

            public BananaGrowthData() { }
            public BananaGrowthData(bool allowGrowth, float growthProgress, Vector3 startScale, 
                Vector3 endScale, float timeToGrowthInSeconds, bool hasGrown)
            {
                this.allowGrowth = allowGrowth;
                this.growthProgress = growthProgress;
                this.startScale = startScale;
                this.endScale = endScale;
                this.timeToGrowthInSeconds = timeToGrowthInSeconds;
                this.hasGrown = hasGrown;
            }
        }
    }
}
