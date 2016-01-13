using Assets.Scripts.Manager;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts.Enemy
{
    public class EnemyHealth : MonoBehaviour
    {

        public int StartingHelth = 100;
        public int CurrentHealth;
        public float SinkSpeed = 2.5f;
        public int ScoreValue = 10;
        public AudioClip DeathAudioClip;

        private Animator _anim;
        private AudioSource _enemyAudio;
        private ParticleSystem _hitParticles;
        private CapsuleCollider _capsuleCollider;
        private bool _isDeath;
        private bool _isSinking;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
            _enemyAudio = GetComponent<AudioSource>();
            _hitParticles = GetComponentInChildren<ParticleSystem>();
            _capsuleCollider = GetComponent<CapsuleCollider>();
            CurrentHealth = StartingHelth;
        }

        private void Update()
        {
            if (_isSinking)
                transform.Translate(-Vector3.up*SinkSpeed*Time.deltaTime);
        }

        public void TakeDamage(int amount, Vector3? hitPoint ,bool isgrenade=false)
        {
           
            if(_isDeath)
                return;
            _enemyAudio.Play();
            CurrentHealth -= amount;
            if (hitPoint != null)
            {
                _hitParticles.transform.position = (Vector3) hitPoint;
                _hitParticles.Play();
            }
            if (CurrentHealth <= 0)
                Death(isgrenade);
        }

        private void Death(bool isgrenade)
        {
            _isDeath = true;
            _capsuleCollider.isTrigger = true;
            _anim.SetTrigger("Death");
            _enemyAudio.clip = DeathAudioClip;
            if (!isgrenade)
              _enemyAudio.Play();
        }

        public void StartSinking()
        {
            _isSinking = true;
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            ScoreManager.Score += ScoreValue;
            Destroy(gameObject,1f);
        }

        public bool IsAlive()
        {
            return CurrentHealth > 0;
        }
    }
}
