using System.Collections;
using Assets.Scripts.Enemy;
using Assets.Scripts.ParticleSistems;
using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    public class ShieldManager : MonoBehaviour
    {
        public float MaxAbsorbDamage = 100f;
        public float MaxAbsorbTime = 15f;
        private float _totalAbsorb;
        private float _time;
        private bool _iscounting;
        private AnimatedUv _animManager;
        public PlayerHealth PlayerHealth;
        private bool _isactive;
        private bool _enemycollider;
        private Collider _c;
        public ParticleSystem EndParticles;
        public ParticleSystem Glow;
        [Header("Return absorved damege after finish")]
        public bool ReturnDamage;


        private void Start()
        {
            _animManager = GetComponent<AnimatedUv>();
            Glow.Stop();
            gameObject.GetComponent<Renderer>().enabled = false;
        }


        private void Update()
        {
            if (_enemycollider)
                AbsorbandFlash(_c);
            if (!_isactive) return;
            if (_iscounting)
                CountTime();
            if (_totalAbsorb >= MaxAbsorbDamage || _time >= MaxAbsorbTime)
                DeActivateShield();
        }



        private void OnTriggerEnter(Collider c)
        {
            Isenemy(c);

        }

        private void OnTriggerStay(Collider c)
        {
            Isenemy(c);
        }

        private void OnTriggerExit(Collider c)
        {
            _enemycollider = false;

        }

        private void Isenemy(Collider c)
        {
            if (c.tag != "Enemy") return;
            _enemycollider = true;
            _c = c;
        }



        private void AbsorbandFlash(Collider c)
        {

            if (c != null && c.tag == "Enemy")
            {

                var e = c.gameObject.GetComponent<EnemyAttack>();
                if (e.IsAtacking)
                {
                    _animManager.Flash();
                    _totalAbsorb += e.AttackDamage;
                }

            }
        }

        public void ActivateShield()
        {
            if (!_isactive & PlayerHealth.IsAlive())
            {
                _iscounting = true;
                Glow.Play();
                _totalAbsorb = 0;
                gameObject.GetComponent<Renderer>().enabled = true;
                _isactive = true;
                PlayerHealth.Superman = true;
            }
        }

      

        private void DeActivateShield()
        {
            if (ReturnDamage && _totalAbsorb>0)
            {
                //Put this in a Generic Functions for explosions(develop latter)
                Collider[] objs = Physics.OverlapSphere(gameObject.transform.position, 3);
                foreach (var o in objs)
                {
                    int distance = (int)Vector3.Distance(o.transform.position, transform.position);
                    //The toutorial dont make posible a good Inheritance (develop latter)
                    if (o.transform.tag == "Enemy")
                        o.GetComponent<EnemyHealth>().TakeDamage(Mathf.Abs((int)_totalAbsorb) - distance, null, true);
                }
            }
            PlayerHealth.Superman = false;
            _isactive = false;
            Glow.Stop();
            EndParticles.Play();
            gameObject.GetComponent<Renderer>().enabled = false;
            _time = 0;
        }

        private void CountTime()
        {
            _time += Time.deltaTime;
        }

}
}
