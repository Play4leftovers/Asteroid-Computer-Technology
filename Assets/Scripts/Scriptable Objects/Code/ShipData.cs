using UnityEngine;

namespace Scriptable_Objects.Code
{
    [CreateAssetMenu]
    public class ShipData : ScriptableObject
    {
        public int shipStartingHealth;
        public int shipCurrentHealth;
        public float shipThrustForce;
        public float shipRotationForce;
        public Vector2 shipPosition;
        public Vector2 shipStartPosition;

        public void ShipDamage()
        {
            shipCurrentHealth--;
        }
    }
}