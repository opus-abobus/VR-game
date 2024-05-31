using System;
using UnityEngine;

namespace DataPersistence.Gameplay
{
    [Serializable]
    public class BananaTreeManagerData : SpawnerData
    {
        public RipeningData ripeningData;
        public GrowthData growthData;

        public BananaTreeManagerData() { }
        public BananaTreeManagerData(string key, RipeningData ripeningData, GrowthData growthData)
        {
            this.key = key;
            this.ripeningData = ripeningData;
            this.growthData = growthData;
        }

        [Serializable]
        public class RipeningData : MonoBehaviourData
        {
            public bool allowRipening;
            public BananaRipening.RipePhase ripePhase;
            public float ripeProgressPhase;
            public float phaseTimeInSeconds;
            public bool isBananaFallen;

            public RipeningData() { }

            public RipeningData(bool allowRipening, BananaRipening.RipePhase ripePhase, float ripeProgressPhase,
                float phaseTimeInSeconds, bool isBananaFallen)
            {
                this.allowRipening = allowRipening;
                this.ripePhase = ripePhase;
                this.ripeProgressPhase = ripeProgressPhase;
                this.phaseTimeInSeconds = phaseTimeInSeconds;
                this.isBananaFallen = isBananaFallen;
            }
        }

        [Serializable]
        public class GrowthData : MonoBehaviourData
        {
            public bool allowGrowth;
            public float growthProgress;
            public Vector3 startScale, endScale;
            public float timeToGrowthInSeconds;
            public bool hasGrown;
            public float startRipeningMoment;

            public GrowthData() { }

            public GrowthData(bool allowGrowth, float growthProgress, Vector3 startScale, Vector3 endScale,
                float timeToGrowthInSeconds, bool hasGrown, float startRipeningMoment)
            {
                this.allowGrowth = allowGrowth;
                this.growthProgress = growthProgress;
                this.startScale = startScale;
                this.endScale = endScale;
                this.timeToGrowthInSeconds = timeToGrowthInSeconds;
                this.hasGrown = hasGrown;
                this.startRipeningMoment = startRipeningMoment;
            }
        }
    }
}