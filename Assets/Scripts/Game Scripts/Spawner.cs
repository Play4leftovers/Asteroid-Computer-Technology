using System;
using System.Collections.Generic;
using System.Linq;
using Scriptable_Objects.Code;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace Game_Scripts
{
    public class Spawner : MonoBehaviour
    {
        public AsteroidData asteroidData;
        private GameObject _asteroid;
        private List<GameObject> _asteroidsToBeSpawned;
        private int _asteroidsToBeSpawnedIndex;
        
        private NativeArray<Vector2> _localScale;
        private NativeArray<Vector2> _kickDir;
        private NativeArray<float> _mass;
        private NativeArray<float> _kickForce;

        [System.Serializable]
        public class Pool
        {
            public string tag;
            public GameObject prefab;
            public int size;
        }
        public Queue<GameObject> asteroidDictionary;
        public Pool asteroidPool;

        private JobHandle _jobHandle;

        private void Awake()
        {
            _kickDir = new NativeArray<Vector2>(1000, Allocator.Persistent);
            _localScale = new NativeArray<Vector2>(1000, Allocator.Persistent);
            _mass = new NativeArray<float>(1000, Allocator.Persistent);
            _kickForce = new NativeArray<float>(1000, Allocator.Persistent);
        }

        private void Start()
        {
            _asteroidsToBeSpawned = new List<GameObject>();
            asteroidDictionary = new Queue<GameObject>();
            asteroidData.asteroidSpawned = 0;
            
            Queue<GameObject> objectPool = new Queue<GameObject>();
            
            for (int i = 0; i < asteroidPool.size; i++)
            {
                GameObject obj = Instantiate(asteroidPool.prefab);
                obj.gameObject.SetActive(false);
                objectPool.Enqueue(obj);
            }

            asteroidDictionary = objectPool;
            
            InvokeRepeating(nameof(Spawn), 0f, asteroidData.asteroidSpawnRate);
        }
        
        private void Spawn()
        {
            if (!asteroidDictionary.Any())
                return;
            
            var spawnPoint = Random.insideUnitCircle.normalized * asteroidData.asteroidSpawnDistance;
            _asteroid = asteroidDictionary.Dequeue();
            
            _asteroid.gameObject.SetActive(true);
            _asteroid.GetComponent<Asteroid>().Creation(this);
            _asteroid.transform.position = spawnPoint;
            _asteroidsToBeSpawned.Add(_asteroid);
            SpawnerJob job = new SpawnerJob((uint)Random.Range(1, 1000),
                spawnPoint, asteroidData.asteroidMass, asteroidData.asteroidFiringAngle, asteroidData.asteroidMassMultiplierMinimum, asteroidData.asteroidMassMultiplierMaximum,
                 _kickDir, _localScale, _mass, _kickForce, _asteroidsToBeSpawnedIndex);
            _asteroidsToBeSpawnedIndex++;
            asteroidData.asteroidSpawned++;
            _jobHandle = job.Schedule();
        }

        private void LateUpdate()
        {
            _jobHandle.Complete();
            for (int i = 0; i < _asteroidsToBeSpawnedIndex; i++)
            {
                _asteroid = _asteroidsToBeSpawned[i];
                _asteroid.transform.localScale = _localScale[i];
                _asteroid.GetComponent<Rigidbody2D>().mass = _mass[i];
                _asteroid.GetComponent<Asteroid>().Kick(_kickForce[i], _kickDir[i]);
            }
            _asteroidsToBeSpawned.Clear();
            _asteroidsToBeSpawnedIndex = 0;
        }

        public void Repool(GameObject asteroidToBeRepooled)
        {
            asteroidDictionary.Enqueue(asteroidToBeRepooled);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, asteroidData.asteroidSpawnDistance);
        }
    }
}