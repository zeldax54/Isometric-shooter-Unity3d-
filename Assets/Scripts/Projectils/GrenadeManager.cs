using System.Threading;
using Assets.Scripts.DragsManagers;
using Assets.Scripts.Enemy;
using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.Projectils
{
    public class GrenadeManager : MonoBehaviour
    {

        public float ExplosionTime = 3f;
        public float ExplosionRadio=10f;
        public int Power=100;
        private float _time;
        private bool _isDetonating;//In order to prevent multiple Detonate Calls
        #region Events
       [HideInInspector]
        public delegate void OnExplosionManager(Vector3 position);
       [HideInInspector]
       public event OnExplosionManager OnExplosion;
       [HideInInspector]
       public delegate void OnCollisionManager();
        [HideInInspector]
        public event OnCollisionManager OnCollision;
      
        #endregion
      

        // Update is called once per frame


        private void FixedUpdate()
        {
            _time += Time.deltaTime;
            if (_time >= ExplosionTime && !_isDetonating)
                Detonate();
        }


        private void Detonate()
        {
            _isDetonating = true;
            if (OnExplosion != null) 
                OnExplosion(transform.position);
            Collider[] objs = Physics.OverlapSphere(gameObject.transform.position, ExplosionRadio);
           
             foreach (var o in objs)
             {
                 int distance = (int)Vector3.Distance(o.transform.position, transform.position);
                
                 //The toutorial dont make posible a good Inheritance (develop latter)
                 if (o.transform.tag == "Enemy")
                     o.GetComponent<EnemyHealth>().TakeDamage(Power - distance, null,true);
                 if (o.transform.tag == "Player")
                     o.GetComponent<PlayerHealth>().TakeDamage(Power - distance);
             }
            Destroy(gameObject);
        }
    

        private void OnCollisionEnter(Collision collisionInfo)
        {
            if (OnCollision != null)
                OnCollision();
            if (collisionInfo.gameObject.tag == "Enemy" 
                || collisionInfo.gameObject.name == "BoomZone")
                Detonate();
           
        }
    }
}
