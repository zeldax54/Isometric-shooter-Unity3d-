using Assets.Scripts.Helpers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Manager
{
    public class ButtonsManager : MonoBehaviour,IPointerDownHandler
    {

        public bool UseCountDown;
        public float CountDownTime=15f;
        private bool _isActive=true;
        private float _time;
        private CalculationsHelper _ch;
        private Image _image;
        private Selectable.Transition _t;

        void Start ()
        {
            _ch = new CalculationsHelper();
            _image = GetComponent<Image>();
            _t = GetComponent<Button>().transition;
        }
	
       
        void Update () {
            if (!_isActive)
                CoutDownExecute();
        }


        public void OnPointerDown(PointerEventData eventData)
        {
            if (!UseCountDown)
                return;
            if (!_isActive)
            {
                eventData.eligibleForClick = false;
                GetComponent<Button>().transition = Selectable.Transition.None;
            }
            else
               _isActive = false;
            
          
            
        }

        private void CoutDownExecute()
        {
            _time += Time.deltaTime;
            _image.fillAmount = _ch.RepresentedPercent(_time, CountDownTime);
            if (_time >= CountDownTime)
            {
                _isActive = true;
                _time = 0;
                GetComponent<Button>().transition = _t;
            }
        }



    }
}
