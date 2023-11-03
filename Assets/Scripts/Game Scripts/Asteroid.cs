using System;
using Scriptable_Objects.Code;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game_Scripts
{
    public class Asteroid : MonoBehaviour
    {
        public AsteroidData astData;
        private Rigidbody2D _rb;
        private Spawner _spawner;
        
        // Start is called before the first frame update
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Background"))
            {
                Break();
            }
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Ship"))
            {
                col.gameObject.GetComponent<Ship>().Damaged();
            }
        }

        public void Creation(Spawner spawner)
        {
            _spawner = spawner;
        }
        
        public void Kick(float forceMultiplier, Vector2 dir)
        {
            _rb.velocity = dir.normalized * (astData.asteroidSpeed * forceMultiplier);
            _rb.AddTorque(Random.Range(-4f, 4f));
        }

        public void Break()
        {
            _spawner.Repool(this.gameObject);
            astData.asteroidSpawned--;
            gameObject.SetActive(false);
        }
    }
}