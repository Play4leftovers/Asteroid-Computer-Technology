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
        
        // Start is called before the first frame update
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Background"))
            {
                astData.asteroidSpawned--;
                gameObject.SetActive(false);
            }
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Ship"))
            {
                col.gameObject.GetComponent<Ship>().Damaged();
            }
        }

        public void Kick(float forceMultiplier, Vector2 dir)
        {
            _rb.velocity = dir.normalized * (astData.asteroidSpeed * forceMultiplier);
            _rb.AddTorque(Random.Range(-4f, 4f));
        }

        public void Break()
        {
            int score = astData.asteroidScoreValue;
            if (_rb.mass > 0.7f)
            {
                score += score;
            }

            astData.asteroidSpawned--;
            astData.AddScore(score);
            gameObject.SetActive(false);
        }
    }
}