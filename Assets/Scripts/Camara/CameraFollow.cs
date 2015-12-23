using UnityEngine;

namespace Assets.Scripts.Camara
{
    public class CameraFollow : MonoBehaviour
    {

        public Transform Target;
        private float smoothing = 5f;
        private Vector3 offset;

        private void Start()
        {
            offset = transform.position - Target.position;
        }

        private void FixedUpdate()
        {
            Vector3 targetCampos = Target.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetCampos, smoothing*Time.deltaTime);
        }
}
}
