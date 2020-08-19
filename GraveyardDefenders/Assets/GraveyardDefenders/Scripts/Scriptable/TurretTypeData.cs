using UnityEngine;

namespace XD
{
    public enum TURRET_TYPE
    {
        NONE,
        FIRE,
        ROCK,
        CRYSTAL,
    }

    [CreateAssetMenu(menuName = "XD/TurretType")]
    public class TurretTypeData : ScriptableObject
    {
        public TURRET_TYPE type;
        public RESOURCE_TYPE resource;
        public GameObject projectile;
        public GameObject turretVisuals;
        public float throw_periode;
        public int projectiles_per_charge;
    }   
}
