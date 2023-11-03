using Scriptable_Objects.Code;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game_Scripts
{
    public class Ship : MonoBehaviour
    {
        public ShipData shipData;
        public GameObject bullet;

        [SerializeField] private int shipHealth;
        [SerializeField] private Vector2 shipPosition;

        private Rigidbody2D _rb;
        private float _shipRotationDirection;
        private float _shipThrust;

        // Start is called before the first frame update
        private void Awake()
        {
            shipHealth = shipData.shipStartingHealth;
            shipData.shipCurrentHealth = shipData.shipStartingHealth;
            _rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        private void Update()
        {
            shipData.shipPosition = shipPosition;
            if (_shipRotationDirection != 0)
                _rb.AddTorque(-_shipRotationDirection * shipData.shipRotationForce * Time.deltaTime);
            if (_shipThrust != 0)
                _rb.AddRelativeForce(Vector2.up * (shipData.shipThrustForce * Time.deltaTime), ForceMode2D.Force);

            BoundaryCheck();
        }

        private void BoundaryCheck()
        {
            var position = transform.position;
            var x = position.x;
            var y = position.y;

            if (x > 11f) x = x - 22f;
            if (x < -11f) x = x + 22f;
            if (y > 6) y = y - 12f;
            if (y < -6) y = y + 12f;

            transform.position = new Vector2(x, y);
        }

        public void Thrust(InputAction.CallbackContext ctx)
        {
            _shipThrust = ctx.ReadValue<float>();
        }

        public void Shoot(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                var tempTransform = transform;
                var theBullet = Instantiate(bullet, tempTransform.position, tempTransform.rotation);
                theBullet.GetComponent<Bullet>().Shoot(transform.up);
            }
        }

        public void Rotate(InputAction.CallbackContext ctx)
        {
            _shipRotationDirection = ctx.ReadValue<float>();
        }

        public void Damaged()
        {
            shipHealth--;
            shipData.ShipDamage();
        }
    }
}