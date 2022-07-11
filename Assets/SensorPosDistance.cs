using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BraitenBerg
{
    public class SensorPosDistance : SensorBase
    {
        private List<GameObject> LayerObjects = new List<GameObject>();
        

        public override void ExtraRoutines()
        {
            GameObject[] gos = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
            
            foreach(GameObject go in gos)
            {
                //Debug.Log(Stimul.value.ToString() + " : " + go.layer.ToString());
                if(((1 << go.layer) & Stimul.value) != 0)
                {
                    if(go != transform.parent.parent.gameObject)
                        LayerObjects.Add(go);
                    //Debug.Log(go);
                }
            }
        }

        protected override float GetSensingValue()
        {
            var ForwardVec = GetForwardVector();
            var SensorPos = GetSensorPos();

            GameObject ClosestGO = null;
            float distance = float.PositiveInfinity;
            Vector2 closestVec = Vector2.zero;
            foreach(GameObject go in LayerObjects)
            {
                var goPos2D = new Vector2(go.transform.position.x, go.transform.position.y);
                var distanceVec = goPos2D - SensorPos;

                if(distanceVec.magnitude < distance)
                {
                    var angle = Vector2.SignedAngle(ForwardVec, distanceVec);
                    if(angle > -RightAngle && angle < LeftAngle)
                    {
                        ClosestGO = go;
                        distance = distanceVec.magnitude;
                        closestVec = distanceVec;
                    }
                }
            }

            Debug.DrawLine(SensorPos, SensorPos + closestVec, color);
            return MaxValue / (1.0f + Mathf.Exp(Skewness * (distance - Threshold)));
        }
    }
}