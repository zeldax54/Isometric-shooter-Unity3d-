using System;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using Assets.Scripts.Manager;
using UnityEngine;

namespace Assets.Scripts.DragsManagers
{

    public class Aim : MonoBehaviour
    {
    
        public GameObject TrajectoryPointMarker;
        public int Numberofpoints = 30;
        public float Angle=30;
        public Transform StartPoint;
        public float InitialDistance = 10;
        public bool HidePointsAfterThrow;
        public GameObject Boomzone;
        public bool Showboomzone;
        private GameObject _player;
        private VirtualJoystick _grenadeJoystick;
        private Vector3 _gravity;
        private List<GameObject> _trajectoryPoints;
         [HideInInspector]
        public Vector3 VelocityVector { get; set; }
        private GameObject _endPoint;
        private void Awake()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _grenadeJoystick = GetComponent<VirtualJoystick>();
            _gravity = Physics.gravity;
            _trajectoryPoints = new List<GameObject>(Numberofpoints);
            //EndPointAllInitialLogic
            _endPoint = new GameObject();
            _endPoint.transform.position= new Vector3(StartPoint.position.x, 0, StartPoint.position.z + InitialDistance);
            _endPoint.transform.SetParent(StartPoint.transform);
           HideBoomZone();
            //
            FillPoints();
        }


        private void FillPoints()
        {
            for (var i = 0; i < Numberofpoints; i++)
            {
                var dot = Instantiate(TrajectoryPointMarker);
                dot.GetComponent<Renderer>().enabled = false;
                _trajectoryPoints.Insert(i, dot);
            }
        }

        //private void Move(float ammout, Transform t)
        //{
        //    t.position.Set(t.position.x,0,t.position.z+5);
            
        //}



        private void FixedUpdate()
        {
            if (_grenadeJoystick.IsUsing() && _grenadeJoystick.IsActive())
            {

                // Move(_grenadeJoystick.Direccion().x, _endPoint);
               // float distance = Vector3.Distance(_grenadeJoystick.SatrtVector(), _grenadeJoystick.InputVectorR());
//Debug.Log(_grenadeJoystick.DisfromtheAnchored());
                _grenadeJoystick.RotateTarget(_player.transform);
                VelocityVector = Enesima(_endPoint.transform, Angle);
                UpdateWithNoLine(StartPoint.position, VelocityVector, _gravity);
                if (Showboomzone)
                {
                    if(!Boomzone.GetComponent<Renderer>().enabled)
                        Boomzone.GetComponent<Renderer>().enabled = true;
                    Boomzone.transform.position =
                       new Vector3(_endPoint.transform.position.x, 0.02f, _endPoint.transform.position.z);
                }
               
            }
            else if (HidePointsAfterThrow)
            {
                
                RemovePoint();
            }
    }


        public void HideBoomZone()
        {
            Boomzone.GetComponent<Renderer>().enabled = false;
        }


        //void UpdateTrajectory(Vector3 initialPosition, Vector3 initialVelocity, Vector3 gravity)
        //{
        //    float timeDelta = 1.0f / initialVelocity.magnitude; 
        //    Vector3 position = initialPosition;
        //    Vector3 velocity = initialVelocity;
        //    for (int i = 0; i < Numberofpoints; ++i)
        //    {
        //        Line.SetPosition(i, position);
        //        position += velocity * timeDelta + 0.5f * gravity * timeDelta * timeDelta;
        //        velocity += gravity * timeDelta;
        //    }
        //}

        private void UpdateWithNoLine(Vector3 initialPosition, Vector3 initialVelocity, Vector3 gravity)
        {
            float timeDelta = 1.0f / initialVelocity.magnitude;
            Vector3 position = initialPosition;
            Vector3 velocity = initialVelocity;
            for (var i = 0; i < Numberofpoints; ++i)
            {
                _trajectoryPoints[i].transform.position = position;
                _trajectoryPoints[i].GetComponent<Renderer>().enabled = true;
                _trajectoryPoints[i].transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(initialVelocity.y - (Physics.gravity.magnitude) * i, initialVelocity.x) * Mathf.Rad2Deg);
                position += velocity * timeDelta + 0.5f * gravity * timeDelta * timeDelta;
                velocity += gravity * timeDelta;
            }
        }

        public void RemovePoint()
        {
            foreach (var t in _trajectoryPoints)
                t.GetComponent<Renderer>().enabled = false;
        }
        public Vector3 Enesima(Transform target, float angle)
        {
           var dir = target.position - StartPoint.position;
            // get target direction 
            var h = dir.y; 
            // get height difference
            dir.y = 0; 
            // retain only the horizontal direction 
            var dist = dir.magnitude; 
            // get horizontal distance 
            var a = angle * Mathf.Deg2Rad; 
            // convert angle to radians 
            dir.y = dist * Mathf.Tan(a); 
            // set dir to the elevation angle 
            dist += h / Mathf.Tan(a); 
            // correct for small height differences 
            // calculate the velocity magnitude 
            var vel = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a)); 
            return vel * dir.normalized;
        }


        













    }
}
