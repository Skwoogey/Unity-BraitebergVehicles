using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BraitenBerg
{
    public class SensorColliderDistance : SensorBase
    {
        private List<GameObject> LayerObjects = new List<GameObject>();


        public override void ExtraRoutines()
        {
            GameObject[] gos = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];

            foreach (GameObject go in gos)
            {
                //Debug.Log(Stimul.value.ToString() + " : " + go.layer.ToString());
                if (((1 << go.layer) & Stimul.value) != 0)
                {
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
            foreach (GameObject go in LayerObjects)
            {
                var goCollider = go.GetComponent<Collider2D>();
                var nearPoint = goCollider.ClosestPoint(SensorPos);
                var distanceVec = nearPoint - SensorPos;

                if (distanceVec.magnitude < distance)
                {
                    var angle = Vector2.SignedAngle(ForwardVec, distanceVec);
                    if (angle > -RightAngle && angle < LeftAngle)
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
