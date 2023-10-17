using Scriptable_Objects.Code;
using UnityEngine;

namespace Game_Scripts
{
    public class Spawner : MonoBehaviour
    {
        public AsteroidData spawnData;

        public GameObject asteroid;

        private void Start()
        {
            InvokeRepeating(nameof(Spawn), 0f, spawnData.asteroidSpawnRate);
        }
        private void Spawn()
        {
            #region Check if asteroid maximum exists and if it has been reached

            if (spawnData.asteroidMaxAmountEnabled)
                if (transform.childCount >= spawnData.asteroidMaxAmount)
                    return;

            #endregion

            var spawnPoint = Random.insideUnitCircle.normalized * spawnData.asteroidSpawnDistance;
            var firingAngle = Random.Range(-spawnData.asteroidFiringAngle, spawnData.asteroidFiringAngle);
            var rot = Quaternion.AngleAxis(firingAngle, new Vector3(0, 0, 1));

            var theAsteroid = Instantiate(asteroid, spawnPoint, rot, transform);

            Vector2 dir = rot * -spawnPoint;
            var forceMultiplier = Random.Range(0.8f, 1.6f);
            var massMultiplier = Random.Range(spawnData.asteroidMassMultiplierMinimum,
                spawnData.asteroidMassMultiplierMaximum);
            var width = Random.Range(0.75f, 1.25f);
            var height = 1 / width;

            theAsteroid.transform.localScale = new Vector2(width, height) * massMultiplier;
            theAsteroid.GetComponent<Rigidbody2D>().mass = spawnData.asteroidMass * massMultiplier;
            theAsteroid.GetComponent<Asteroid>().Kick(forceMultiplier, dir);
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, spawnData.asteroidSpawnDistance);
        }
    }
}