using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public class MoveRotateHelper  {
       

        public  void Rotate(Transform t,Vector3 position,Vector3 startPoint)
        {
            Vector3 r = position - startPoint;
            r.z = r.y;
            r.y = 0;
            Quaternion newRotaion = Quaternion.LookRotation(r);
            t.localRotation = newRotaion;
        }

        public  void Move(int ammount,Transform t)
        {
            t.position = new Vector3(t.position.x, t.position.y, t.position.z + ammount);
        }

    }
}
