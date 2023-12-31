using System;
using System.Collections.Generic;
using Game_Scripts;
using Unity.VisualScripting;
using UnityEngine;

namespace Scriptable_Objects.Code
{
    [CreateAssetMenu]
    public class AsteroidData : ScriptableObject
    {
        public int asteroidSpawned;
        public int asteroidScoreValue;
        public float asteroidSpeed;
        public float asteroidFiringAngle;
        public float asteroidSpawnRate;
        public float asteroidSpawnDistance;
        public float asteroidMass;
        public float asteroidMassMultiplierMinimum;
        public float asteroidMassMultiplierMaximum;
        public float asteroidScore;

        public void AddScore(int score)
        {
            asteroidScore += score;
        }
    }
}