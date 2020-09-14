using UnityEngine;

namespace XD
{
    public class Projectile : MonoBehaviour
    {
        public Skeleton skeleton; //TODO: change this to generic enemy class
        public float velocity = 0.5f;
        public float radius = 0.2f;
        public float damage = 0.1f;

        public void Init(Skeleton sk_controller) { skeleton = sk_controller; }
        private void Start() { StartProjectile(); }
        private void Update() { UpdateProjectile(Time.deltaTime); }

        public virtual void StartProjectile(){}
        public virtual void UpdateProjectile(float dt)
        {
            if (!skeleton.gameObject.activeInHierarchy)
            {
                OnHit();
                return;
            }
            Vector3 skeletonPos = skeleton.transform.TransformPoint(skeleton.capsule.center);
            if(Vector3.Distance(transform.position, skeletonPos) <= radius) //check hits
                OnHit();
            else //keep following the enemy
            {
                Vector3 dir = (skeletonPos - transform.position).normalized;
                transform.position += (dir * velocity * dt);
            }
        }

        public virtual void OnHit() 
        {
            skeleton.ReceiveHit(damage);
            Destroy(gameObject); //TODO: pool
        }
    }   
}
