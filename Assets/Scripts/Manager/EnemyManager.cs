using Assets.Scripts.Player;
using UnityEngine;


namespace Assets.Scripts.Manager
{
    public class EnemyManager : MonoBehaviour
    {

        public PlayerHealth PlayerHealth;
        public GameObject Enemy;
        public float SpawnTime=3f;
        public Transform SpawnPoints;

        private void Start()
        {
            InvokeRepeating("Spawn",SpawnTime,SpawnTime);
        }

        private void Spawn()
        {
            if(!PlayerHealth.IsAlive())
                return;
            Instantiate(Enemy, SpawnPoints.position, SpawnPoints.rotation);
        }


    }
}
