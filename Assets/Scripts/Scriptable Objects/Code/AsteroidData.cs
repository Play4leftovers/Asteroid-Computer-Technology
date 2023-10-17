using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Scriptable_Objects.Code
{
    [CreateAssetMenu]
    public class AsteroidData : ScriptableObject
    {
        public bool asteroidMaxAmountEnabled;
        public int asteroidMaxAmount;
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