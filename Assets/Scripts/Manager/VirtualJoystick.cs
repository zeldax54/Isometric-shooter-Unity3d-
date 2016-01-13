using System;
using System.Collections.Generic;
using Assets.Scripts.DragsManagers;
using Assets.Scripts.Helpers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Manager
{
    public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler, IEndDragHandler,IBeginDragHandler
    {
        private Image _backgroudImage, _joystickImage;
        private Vector3 _inputVector;
        private bool _isdragging;
        private bool _ispressing;
        private PointerEventData _p;
        private Vector3 _startPoint;
        private bool _shoot;
        public bool IsforMov;
        public bool IsforRotate;
        public Color DragColor;
        private Color _originalColor;
        private MoveRotateHelper _helper;
        private CalculationsHelper _helperCalc;
        private Animator _anim;
       
        #region CountDown
        public bool UseCountDown;
        private bool _isAviable=true;
        public float RicoverTime=5f;
        private float _time;
        #endregion

        #region Events

        public delegate void Onclickmanager(bool isactive);
        public event Onclickmanager Onclickup;
        #endregion

        private void Start()
        {
            _backgroudImage = GetComponent<Image>();
            _joystickImage = transform.GetChild(0).GetComponent<Image>();
            _startPoint = _joystickImage.rectTransform.position;
            _originalColor = _joystickImage.color;
            _helper = new MoveRotateHelper();
            _helperCalc = new CalculationsHelper();
            _anim = GetComponent<Animator>();
           
        }

        public bool IsShooting()
        {
            return _shoot;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(IsforRotate)
               _shoot = true;
            _joystickImage.color = DragColor;
           
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 position;
            _p = eventData;
            _isdragging = true;
           
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_backgroudImage.rectTransform,
                eventData.position,eventData.pressEventCamera,out position)&&_isAviable)
            {
                _inputVector = position - _joystickImage.rectTransform.sizeDelta;
                if (_inputVector.magnitude > _joystickImage.rectTransform.sizeDelta.x)
                    _inputVector = _inputVector.normalized*_joystickImage.rectTransform.sizeDelta.x;
                _joystickImage.rectTransform.anchoredPosition = _inputVector;
            }

        }



        private void Update()
        {
            if (_p != null && !_p.IsPointerMoving())
                _isdragging = false;
            if (UseCountDown)
            {
                if (!_isAviable)
                    FillImage();
            }
        }

   
        private void FillImage()
        {
            _time += Time.deltaTime;
            float x = _helperCalc.RepresentedPercent(_time, RicoverTime);
            _isAviable = false;
            _backgroudImage.fillAmount = x;
            if (_time>=RicoverTime) 
            {
                _anim.SetTrigger("anim");
                 _isAviable = true;
                _time = 0;
            }
        }




        public void DeactivateInput()
        {
            _backgroudImage.fillAmount = 0;
            _isAviable = false;
        }

        public bool IsUsing()
        {
            return _ispressing&&_isAviable;
        }



        public Vector3 InputVectorR()
        {
            return _inputVector;
        }

        public Vector3 SatrtVector()
        {
            return _startPoint;
        }

        public bool IsDragging()
        {
            return _isdragging;
        }

        public Vector2 DisfromtheAnchored()
        {
            return _joystickImage.rectTransform.anchoredPosition;
        }

        public float Size()
        {
            return _backgroudImage.rectTransform.sizeDelta.x*_backgroudImage.rectTransform.sizeDelta.y;
        }



        public void RotateTarget(Transform t)
        {
            if (_isdragging &&_isAviable)
                _helper.Rotate(t, _joystickImage.rectTransform.position,_startPoint);
        }

        //private void Rotate(Transform t)
        //{
        //    Vector3 r = _joystickImage.rectTransform.position - _startPoint;
        //    r.z = r.y;
        //    r.y = 0;   
        //    Quaternion newRotaion = Quaternion.LookRotation(r);
        //    t.localRotation = newRotaion;
        //}

        public void OnPointerUp(PointerEventData eventData)
        {
            StopDrag();
            if (Onclickup != null) Onclickup(_isAviable);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _ispressing = true;
            OnDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            
            StopDrag();
        }

        public bool IsActive()
        {
            return _isAviable;

        }


        public float Horizontal()
        {

            return Math.Abs(_inputVector.x) > 0.00001 ? _inputVector.x : Input.GetAxis("Horizontal");
        }

        public float Vertical()
        {
            return Math.Abs(_inputVector.y) > 0.00001 ? _inputVector.y : Input.GetAxis("Vertical");
        }



        private void StopDrag()
        {
            _joystickImage.color = _originalColor;
            _inputVector = Vector3.zero;
            _joystickImage.rectTransform.anchoredPosition = Vector3.zero;
            _ispressing = false;
            _isdragging = false;
            _shoot = false;
        }

        public Vector3 Direccion()
        {
           return _inputVector - _startPoint;
        }


    }
}
