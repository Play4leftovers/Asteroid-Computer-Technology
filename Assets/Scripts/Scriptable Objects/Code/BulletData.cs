using UnityEngine;

namespace Scriptable_Objects.Code
{
    [CreateAssetMenu]
    public class BulletData : ScriptableObject
    {
        public float bulletSpeed;
        public int bulletMaxAmount;
        public int bulletHealth;
    }
}