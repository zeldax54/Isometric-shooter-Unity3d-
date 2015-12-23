using System;
using Assets.Scripts.DragsManagers;
using Assets.Scripts.Manager;
using UnityEngine;

namespace Assets.Scripts.Projectils
{
    public class GrenadeShootManager : MonoBehaviour
    {

        public GameObject Grenade;
        private GameObject _grenade;
        private VirtualJoystick _grenadeJoystick;
        private Aim _aimManager;
        private ParticleSystem _explosion;
      
        
        void Awake()
        {
            _grenadeJoystick = GetComponent<VirtualJoystick>();
            _aimManager = GetComponent<Aim>();
            _explosion = GetComponentInChildren<ParticleSystem>();
        
        }
       
        void Start () {
            _grenadeJoystick.Onclickup += _grenadeJoystick_Onclickup; ;
	
        }

        void _grenadeJoystick_Onclickup(bool isactive)
        {
            if (isactive)
            {
               
                _grenadeJoystick.DeactivateInput();
                ThrowGrenade();

            }
        }

     
        

        private void ThrowGrenade()
        {
           _grenade = Instantiate(Grenade);
            Vector3 pos = _aimManager.StartPoint.position;
            _grenade.transform.position = pos;
            _grenade.GetComponent<Rigidbody>().velocity = _aimManager.VelocityVector;
            GrenadeManager gm = _grenade.GetComponent<GrenadeManager>();
            gm.OnExplosion += gm_OnExplosion;
            gm.OnCollision += gm_OnCollision;
        }

        void gm_OnCollision()
        {
            _aimManager.Boomzone.GetComponent<Renderer>().enabled = false;
        }

        void gm_OnExplosion(Vector3 position)
        {
            _explosion.transform.position = position;
            _explosion.Play();
            _aimManager.RemovePoint();
            _aimManager.HideBoomZone();

        }




    }
}
