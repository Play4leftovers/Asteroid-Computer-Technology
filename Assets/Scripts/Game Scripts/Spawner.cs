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
        private GameObject asteroid;
        private List<GameObject> asteroidsToBeSpawned;
        private int asteroidsToBeSpawnedIndex;
        
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
        public Dictionary<string, Queue<GameObject>> asteroidDictionary;
        public Pool AsteroidPool;

        private JobHandle _jobHandle;

        private void Awake()
        {
            _kickDir = new NativeArray<Vector2>(10000, Allocator.Persistent);
            _localScale = new NativeArray<Vector2>(10000, Allocator.Persistent);
            _mass = new NativeArray<float>(10000, Allocator.Persistent);
            _kickForce = new NativeArray<float>(10000, Allocator.Persistent);
        }

        private void Start()
        {
            asteroidsToBeSpawned = new List<GameObject>();
            asteroidDictionary = new Dictionary<string, Queue<GameObject>>();
            asteroidData.asteroidSpawned = 0;
            
            Queue<GameObject> objectPool = new Queue<GameObject>();
            
            for (int i = 0; i < AsteroidPool.size; i++)
            {
                GameObject obj = Instantiate(AsteroidPool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            
            asteroidDictionary.Add(AsteroidPool.tag, objectPool);
            
            InvokeRepeating(nameof(Spawn), 0f, asteroidData.asteroidSpawnRate);
        }
        
        private void Spawn()
        {
            if (asteroidData.asteroidSpawned > AsteroidPool.size)
                return;
            
            var spawnPoint = Random.insideUnitCircle.normalized * asteroidData.asteroidSpawnDistance;
            asteroid = asteroidDictionary[AsteroidPool.tag].Dequeue();
            asteroidDictionary[AsteroidPool.tag].Enqueue(asteroid);
            
            asteroid.SetActive(true);
            asteroid.transform.position = spawnPoint;
            asteroidsToBeSpawned.Add(asteroid);
            SpawnerJob job = new SpawnerJob((uint)Random.Range(1, 1000),
                spawnPoint, asteroidData.asteroidMass, asteroidData.asteroidFiringAngle, asteroidData.asteroidMassMultiplierMinimum, asteroidData.asteroidMassMultiplierMaximum,
                 _kickDir, _localScale, _mass, _kickForce, asteroidsToBeSpawnedIndex);
            asteroidsToBeSpawnedIndex++;
            asteroidData.asteroidSpawned++;
            _jobHandle = job.Schedule();
        }

        private void LateUpdate()
        {
            _jobHandle.Complete();
            for (int i = 0; i < asteroidsToBeSpawnedIndex; i++)
            {
                asteroid = asteroidsToBeSpawned[i];
                asteroid.transform.localScale = _localScale[i];
                asteroid.GetComponent<Rigidbody2D>().mass = _mass[i];
                asteroid.GetComponent<Asteroid>().Kick(_kickForce[i], _kickDir[i]);
            }
            asteroidsToBeSpawned.Clear();
            asteroidsToBeSpawnedIndex = 0;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, asteroidData.asteroidSpawnDistance);
        }
    }
}