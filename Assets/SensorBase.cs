using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using UnityEditor;

namespace BraitenBerg
{
    public class SensorBase : MonoBehaviour
    {
        // Start is called before the first frame update
        protected GameObject VehicleObj;
        protected Rigidbody2D Vehicle;
        protected float LeftAngle;
        protected float RightAngle;

        private Vector2 RightBound;
        private Vector2 LeftBound;

        protected LayerMask Stimul;
        protected float Threshold;
        protected float MaxValue;
        protected float Skewness;
        private SensorValue SVvar;

        protected Color color;

        void Start()
        {
            color = transform.parent.gameObject.GetComponent<SensorPair>().color;
            color.a = 1.0f;


            VehicleObj = transform.parent.parent.gameObject;
            Vehicle = transform.parent.parent.gameObject.GetComponent<Rigidbody2D>();
            SVvar.SPindex = transform.parent.GetSiblingIndex();
            SVvar.Sindex = transform.GetSiblingIndex();

            Stimul      = transform.parent.gameObject.GetComponent<SensorPair>().Stimul;
            Threshold   = transform.parent.gameObject.GetComponent<SensorPair>().Threshold;
            MaxValue    = transform.parent.gameObject.GetComponent<SensorPair>().MaxValue;
            Skewness    = transform.parent.gameObject.GetComponent<SensorPair>().Skewness;

            ExtraRoutines();
        }

        public virtual void ExtraRoutines()
        {

        }

        public void SetLeftBound(float angle)
        {
            LeftAngle = angle;
        }

        public void SetRightBound(float angle)
        {
            RightAngle = angle;
        }

        //public void setstimulfilter(layermask nstimul)
        //{
        //    stimul = nstimul;
        //}

        protected Vector2 GetVectorFromAngle(float angle)
        {
            angle *= Mathf.Deg2Rad;
            Vector2 vec;
            vec.y = Mathf.Sin(angle);
            vec.x = Mathf.Cos(angle);
            return vec;
        }

        protected Vector2 GetForwardVector()
        {
            return GetVectorFromAngle(Vehicle.rotation + 90);
        }

        protected Vector2 GetSensorPos()
        {
            var SensorPosition = transform.TransformPoint(Vector3.zero);
            return new Vector2(SensorPosition.x, SensorPosition.y);
        }

        protected virtual float GetSensingValue()
        {
            return 1.0f;
        }


        void FixedUpdate()
        {
            var dir = Vehicle.rotation + 90;
            RightBound = GetVectorFromAngle(dir - RightAngle);
            LeftBound = GetVectorFromAngle(dir + LeftAngle);
            var SensorPosition = transform.TransformPoint(Vector3.zero);
            var SensorPos2D = new Vector2(SensorPosition.x, SensorPosition.y);
            Debug.DrawLine(SensorPos2D, SensorPos2D + RightBound, color);
            Debug.DrawLine(SensorPos2D, SensorPos2D + LeftBound, color);

            SVvar.Value = GetSensingValue();
            //Debug.Log("ReportingValue: " + SVvar);
            VehicleObj.SendMessage("RecieveSensorValue", SVvar);
        }
    }
}

