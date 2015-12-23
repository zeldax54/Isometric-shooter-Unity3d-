using Assets.Scripts.Manager;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerMovment : MonoBehaviour
    {

        public float Speed = 6f;
        private Vector3 _movment;
        private Animator _anim;
        private Rigidbody _playerRigidbody;
        private int _floorMask;
        private const float CamRightLenght = 100f;
        public VirtualJoystick JoystickMove;
        public VirtualJoystick JoysticRotate;
       

        private void Awake()
        {
            _floorMask = LayerMask.GetMask("Floor");
            _anim = GetComponent<Animator>();
            _playerRigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
           // Move(h,v);
            float hJoystick = JoystickMove.Horizontal();
            float vJoystick = JoystickMove.Vertical();
           
            Move(h,v);
            Animating(h, v);
          //  Turning();
            /////
            Move(hJoystick, vJoystick);
            JoysticRotate.RotateTarget(transform);
            Animating(hJoystick,vJoystick);
          
           
        }

        public void Move(float h, float v)
        {
            _movment.Set(h, 0, v);
            _movment = _movment.normalized*Speed*Time.deltaTime;
            _playerRigidbody.MovePosition(transform.position + _movment);
        }

        private void Turning()
        {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit floorHit;
            if (Physics.Raycast(camRay, out floorHit, CamRightLenght, _floorMask))
            {
                Vector3 playerToMouse = floorHit.point - transform.position;
                playerToMouse.y = 0f;
                Quaternion newRotaion = Quaternion.LookRotation(playerToMouse);
               
                _playerRigidbody.MoveRotation(newRotaion);
            }
        }

    

        private void Animating(float h, float v)
        {

            _anim.SetBool("IsWalking", h != 0 || v != 0);
        }

    
    }
}
