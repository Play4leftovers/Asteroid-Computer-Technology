using System.Collections;
using System.Collections.Generic;
using Scriptable_Objects.Code;
using Unity.Jobs;
using UnityEngine;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;

namespace Game_Scripts
{
    public struct SpawnerJob : IJob
    {
        private uint _seed;
        private Vector2 _spawnPoint;
        
        private float _asteroidMass;
        private float _asteroidFiringAngle;
        private float _asteroidMassMultiplierMinimum;
        private float _asteroidMassMultiplierMaximum;
        private int _asteroidsToBeSpawnedIndex;

        [NativeDisableContainerSafetyRestriction]
        private NativeArray<float> _mass;
        [NativeDisableContainerSafetyRestriction]
        private NativeArray<Vector2> _kickDir;
        [NativeDisableContainerSafetyRestriction]
        private NativeArray<Vector2> _localScale;
        [NativeDisableContainerSafetyRestriction]
        private NativeArray<float> _kickForce;

        public SpawnerJob(uint seed, Vector2 spawnPoint, float asteroidMass, float asteroidFiringAngle,
            float asteroidMassMultiplierMinimum, float asteroidMassMultiplierMaximum, NativeArray<Vector2> kickDir,
            NativeArray<Vector2> localScale, NativeArray<float> mass,
            NativeArray<float> kickForce, int asteroidsToBeSpawnedIndex)
        {
            _seed = seed;

            _spawnPoint = spawnPoint;
            _asteroidMass = asteroidMass;
            _asteroidFiringAngle = asteroidFiringAngle;
            _asteroidMassMultiplierMinimum = asteroidMassMultiplierMinimum;
            _asteroidMassMultiplierMaximum = asteroidMassMultiplierMaximum;
            
            _mass = mass;
            _kickDir = kickDir;
            _localScale = localScale;
            _kickForce = kickForce;
            _asteroidsToBeSpawnedIndex = asteroidsToBeSpawnedIndex;
        }
        
        public void Execute()
        {
            var random = new Unity.Mathematics.Random(_seed);
            
            var firingAngle = random.NextFloat(-_asteroidFiringAngle, _asteroidFiringAngle);
            var rot = Quaternion.AngleAxis(firingAngle, new Vector3(0, 0, 1));

            Vector2 dir = rot * -_spawnPoint;
            var forceMultiplier = random.NextFloat(0.8f, 1.6f);
            var massMultiplier = random.NextFloat(_asteroidMassMultiplierMinimum,
                _asteroidMassMultiplierMaximum);
            var width = random.NextFloat(0.9f, 1.1f);
            var height = 1 / width;
            
            _mass[_asteroidsToBeSpawnedIndex] = _asteroidMass * massMultiplier;
            _kickDir[_asteroidsToBeSpawnedIndex] = dir;
            _localScale[_asteroidsToBeSpawnedIndex] = new Vector2(width, height) * massMultiplier;
            _kickForce[_asteroidsToBeSpawnedIndex] = forceMultiplier;
        }
    }
} 
