using System.Collections;
using System.Collections.Generic;
using Scriptable_Objects.Code;
using Unity.Jobs;
using UnityEngine;

namespace Game_Scripts
{
    public struct SpawnerJob : IJob
    {
        private AsteroidData _asteroidData;
        private GameObject _asteroid;

        private float _firingAngle;
        private Vector2 _spawnPoint;
        private uint _seed;
        private Transform _transform;

        public SpawnerJob(Vector2 spawnPoint, float firingAngle, uint seed, AsteroidData asteroidData, GameObject asteroid, Transform transform)
        {
            _spawnPoint = spawnPoint;
            _firingAngle = firingAngle;
            _seed = seed;

            _asteroidData = asteroidData;
            _asteroid = asteroid;
            _transform = transform;
        }
        // Update is called once per frame
        public void Execute()
        {
            #region Check if asteroid maximum exists and if it has been reached

            if (_asteroidData.asteroidMaxAmountEnabled)
                if (_transform.childCount >= _asteroidData.asteroidMaxAmount)
                    return;

            #endregion

            var spawnPoint = Random.insideUnitCircle.normalized * _asteroidData.asteroidSpawnDistance;
            var firingAngle = Random.Range(-_asteroidData.asteroidFiringAngle, _asteroidData.asteroidFiringAngle);
            var rot = Quaternion.AngleAxis(firingAngle, new Vector3(0, 0, 1));

            var theAsteroid = Instantiate(_asteroid, spawnPoint, rot, _transform);

            Vector2 dir = rot * -spawnPoint;
            var forceMultiplier = Random.Range(0.8f, 1.6f);
            var massMultiplier = Random.Range(_asteroidData.asteroidMassMultiplierMinimum,
                _asteroidData.asteroidMassMultiplierMaximum);
            var width = Random.Range(0.75f, 1.25f);
            var height = 1 / width;

            theAsteroid.transform.localScale = new Vector2(width, height) * massMultiplier;
            theAsteroid.GetComponent<Rigidbody2D>().mass = _asteroidData.asteroidMass * massMultiplier;
            theAsteroid.GetComponent<Asteroid>().Kick(forceMultiplier, dir);
        }
    }
} 
