using Assets.Scripts.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Manager
{
    public class GameOver : MonoBehaviour
    {
        public PlayerHealth PlayerHealth;
        public float RestarDelay = 5f;

        private Animator _anim;
        private float _restartTimer;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
        }

        private void Update()
        {
            if (PlayerHealth.IsAlive()) return;
            _anim.SetTrigger("GameOver");
            _restartTimer += Time.deltaTime;
            if(_restartTimer>RestarDelay)
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
       
        }

    }
}
