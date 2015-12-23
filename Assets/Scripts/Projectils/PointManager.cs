using UnityEngine;

namespace Assets.Scripts.Projectils
{
    public class PointManager : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
           
            if (other.name.Contains("Grenade"))
                gameObject.GetComponent<Renderer>().enabled = false;
        }
   
    
 
    }
}
