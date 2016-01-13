using UnityEngine;

namespace Assets.Scripts.ParticleSistems
{
    public class AnimatedUv : MonoBehaviour
    {
        public float ScrollSpeed=0.5f;
        private float _offset;
        public bool U=true;
        public bool V;
        private Material _mat;
        private Animator _anim;
     
        void Start ()
        {
            _mat = GetComponent<Renderer>().material;
            _anim = GetComponent<Animator>();

        }
       
        void Update ()
        {
            AnimateOffSet();
           
        }

        private void AnimateOffSet()
        {
            _offset = Time.time * ScrollSpeed % 1;
            if (U & V)
                _mat.mainTextureOffset = new Vector2(_offset, _offset);
            else if (U)
                _mat.mainTextureOffset = new Vector2(0, _offset);
            else if (V)
                _mat.mainTextureOffset = new Vector2(_offset, 0);
        }

        public void Flash()
        {
            _anim.SetTrigger("hit");
            
        }
        
     

    }
}
