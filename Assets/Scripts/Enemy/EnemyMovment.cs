using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class EnemyMovment : MonoBehaviour
    {

        private Transform _player;
        private PlayerHealth _playerHealth;
        private EnemyHealth _enemyHealth;

        private NavMeshAgent _nav;

        private void Awake()
        {
            _player = GameObject.FindGameObjectWithTag("Player").transform;
            _nav = GetComponent<NavMeshAgent>();
            _playerHealth = _player.GetComponent<PlayerHealth>();
            _enemyHealth = GetComponent<EnemyHealth>();
        }

        
        void Update ()
        {
            if(_playerHealth.IsAlive()&&_enemyHealth.IsAlive())
               _nav.SetDestination(_player.position);
            else
            _nav.enabled = false;
        }
    }
}
