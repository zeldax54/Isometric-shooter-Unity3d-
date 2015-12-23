using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class EnemyAttack : MonoBehaviour {

        public float TimeBetweenAttacks = 0.5f;
        public int AttackDamage = 10;

        private Animator _anim;
        private GameObject _player;
        private PlayerHealth _playerHealth;
        private bool _playerInRange;
        private float _timer;

        private EnemyHealth _enemyhealth;

        private void Awake()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _playerHealth = _player.GetComponent<PlayerHealth>();
            _anim = GetComponent<Animator>();
            _enemyhealth = GetComponent<EnemyHealth>();
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == _player)
                _playerInRange = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == _player)
             _playerInRange = false;
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= TimeBetweenAttacks && _playerInRange && _enemyhealth.CurrentHealth>0)
            {
                Attack();
            }
            if (_playerHealth.CurrentHealth <= 0)
            {
                _anim.SetTrigger("PlayerDeath");
            }
        }

        private void Attack()
        {
            _timer = 0;
            if (_playerHealth.CurrentHealth > 0)
            {
                _playerHealth.TakeDamage(AttackDamage);
            }
        }

    }
}
