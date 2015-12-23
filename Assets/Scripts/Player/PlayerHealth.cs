using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Player
{
    public class PlayerHealth : MonoBehaviour
    {

        public int StartingHealth = 100;
        public int CurrentHealth;
        public Slider HealthSlider;
        public Image DamageImage;
        public AudioClip DeathClip;
        public float FlashSpeed=5f;
        public Color FlashColor = new Color(1f, 0f, 0f, 0.1f);
        private Animator _anim;
        private AudioSource _playerAudio;
        private PlayerMovment _playerMovment;
        private PlayerShooting _playerShooting;
        private bool _isDead;
        private bool _damage;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
            _playerAudio = GetComponent<AudioSource>();
            _playerMovment = GetComponent<PlayerMovment>();
            _playerShooting = GetComponentInChildren<PlayerShooting>();
            CurrentHealth = StartingHealth;
        }

        void Update ()
        {
            DamageImage.color = _damage ? FlashColor : Color.Lerp(DamageImage.color, Color.clear, FlashSpeed*Time.deltaTime);
            _damage = false;
        }

        public void TakeDamage(int ammount)
        {
            _damage = true;
            CurrentHealth -= ammount;
            HealthSlider.value = CurrentHealth;
            _playerAudio.Play();
            if (CurrentHealth <= 0 && !_isDead)
            {
                Death();
            }
        }

        private void Death()
        {
            _isDead = true;
            _anim.SetTrigger("Die");
            _playerAudio.clip = DeathClip;
            _playerAudio.Play();
            _playerMovment.enabled = false;
            _playerShooting.enabled = false;
        }

        public bool IsAlive()
        {
            return CurrentHealth > 0;
        }

    }
}
