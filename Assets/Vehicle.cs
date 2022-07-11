using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BraitenBerg
{

    struct SensorValue
    {
        public int SPindex;
        public int Sindex;
        public float Value;
    }

    public class Vehicle : MonoBehaviour
    {
        public float defaultAccel = 0.0f;
        public float LeftAccel = 0.0f;
        public float RightAccel = 0.0f;
        public Rigidbody2D body;

        private float[,] SensorValues;
        private int sensorsCount;
        private int sensorValuesRecieved;

        void Start()
        {
            SensorValues = new float[transform.childCount, 2];
            sensorsCount = SensorValues.Length;
            sensorValuesRecieved = 0;
        }

        void RecieveSensorValue(SensorValue SVvar)
        {
            SensorValues[SVvar.SPindex, SVvar.Sindex] = SVvar.Value;
            sensorValuesRecieved++;

            if(sensorValuesRecieved == sensorsCount)
            {
                MyFixedUpdate();
            }
        }
    
        void MyFixedUpdate()
        {
            sensorValuesRecieved = 0;
            LeftAccel = defaultAccel;
            RightAccel = defaultAccel;

            for(int i = 0; i < transform.childCount; i++)
            {
                var SP = transform.GetChild(i).gameObject.GetComponent<SensorPair>();
                float leftSensorVal = SensorValues[i, 0] * SP.connectionWeight;
                float rightSensorVal = SensorValues[i, 1] * SP.connectionWeight;
                Debug.Log("SP" + i.ToString() + ": " +
                    leftSensorVal.ToString() + "; " +
                    rightSensorVal.ToString()
                );
                if (SP.inverseConnection)
                {
                    LeftAccel += rightSensorVal;
                    RightAccel += leftSensorVal;
                }
                else
                {
                    LeftAccel += leftSensorVal;
                    RightAccel += rightSensorVal;
                }
            }

            float speed;
            float dir = (LeftAccel - RightAccel);
            if (LeftAccel >= 0 && RightAccel >= 0)
            {
                speed = Mathf.Max(LeftAccel, RightAccel);
            }
            else if (LeftAccel <= 0 && RightAccel <= 0)
            {
                speed = Mathf.Min(LeftAccel, RightAccel);
            }
            else
            {
                speed = RightAccel + LeftAccel;
            }

            if (speed != 0.0f)
            {
                dir /= speed;
                var radius = Mathf.Abs(1.0f / dir);
                Debug.Log("Radius: " + radius.ToString());
                var move = speed * Time.fixedDeltaTime;
                Debug.Log("Move: " + move.ToString());
                var moveRadians = move / radius;
                Debug.Log("MoveAngle: " + (moveRadians * Mathf.Rad2Deg).ToString());
                var initialAngle = body.rotation * Mathf.Deg2Rad;

                if (dir < -0.001f)
                {
                    Debug.Log("Rotate left");
                    Vector2 moveVec;

                    moveVec.x = Mathf.Cos(initialAngle + moveRadians) - Mathf.Cos(initialAngle);
                    moveVec.x *= radius;
                    moveVec.y = Mathf.Sin(initialAngle + moveRadians) - Mathf.Sin(initialAngle);
                    moveVec.y *= radius;

                    Debug.Log("( " +
                        moveVec.x.ToString() + ", " +
                        moveVec.y.ToString() + " )"
                    );
                    var oldPos = body.position;
                    var newPos = oldPos + moveVec;
                    Debug.DrawLine(oldPos, newPos, Color.red, 10);
                    body.position = newPos;

                    body.rotation += moveRadians * Mathf.Rad2Deg;
                }
                else if (dir > 0.001f)
                {
                    Debug.Log("Rotate right");
                    Vector2 moveVec;

                    moveVec.x = Mathf.Cos(initialAngle - moveRadians) - Mathf.Cos(initialAngle);
                    moveVec.x *= radius;
                    moveVec.y = Mathf.Sin(initialAngle - moveRadians) - Mathf.Sin(initialAngle);
                    moveVec.y *= radius;

                    Debug.Log("( " +
                        moveVec.x.ToString() + ", " +
                        moveVec.y.ToString() + " )"
                    );
                    var oldPos = body.position;
                    var newPos = oldPos - moveVec;
                    Debug.DrawLine(oldPos, newPos, Color.red, 10);
                    body.position = newPos;

                    body.rotation -= moveRadians * Mathf.Rad2Deg;
                }
                else
                {
                    Debug.Log("Straight");
                    Vector2 moveVec;

                    moveVec.x = Mathf.Cos(initialAngle + Mathf.PI / 2.0f);
                    moveVec.x *= move;
                    moveVec.y = Mathf.Sin(initialAngle + Mathf.PI / 2.0f);
                    moveVec.y *= move;

                    var oldPos = body.position;
                    var newPos = oldPos + moveVec;
                    Debug.DrawLine(oldPos, newPos, Color.red, 10);
                    body.position = newPos;
                }
            }
            else if (Mathf.Abs(dir) < 0.001f)
            {
                Debug.Log("Rotating");
                body.rotation -= (Time.fixedDeltaTime * dir) * Mathf.Rad2Deg;
            }
        }

        void notFixedUpdate()
        {


            float speed;
            float dir = (LeftAccel - RightAccel);
            if (LeftAccel >= 0 && RightAccel >= 0)
            {
                speed = Mathf.Max(LeftAccel, RightAccel);
            }
            else if (LeftAccel <= 0 && RightAccel <= 0)
            {
                speed = Mathf.Min(LeftAccel, RightAccel);
            }
            else
            {
                speed = RightAccel + LeftAccel;
            }

            if (speed != 0.0f)
            {
                dir /= speed;
                var radius = Mathf.Abs(1.0f / dir);
                Debug.Log("Radius: " + radius.ToString());
                var move = speed * Time.fixedDeltaTime;
                Debug.Log("Move: " + move.ToString());
                var moveRadians = move / radius;
                Debug.Log("MoveAngle: " + (moveRadians * Mathf.Rad2Deg).ToString());
                var initialAngle = body.rotation * Mathf.Deg2Rad;

                if (dir < 0.0f)
                {
                    Debug.Log("Rotate left");
                    Vector2 moveVec;

                    moveVec.x = Mathf.Cos(initialAngle + moveRadians) - Mathf.Cos(initialAngle);
                    moveVec.x *= radius;
                    moveVec.y = Mathf.Sin(initialAngle + moveRadians) - Mathf.Sin(initialAngle);
                    moveVec.y *= radius;

                    Debug.Log("( " +
                        moveVec.x.ToString() + ", " +
                        moveVec.y.ToString() + " )"
                    );
                    var oldPos = body.position;
                    var newPos = oldPos + moveVec;
                    Debug.DrawLine(oldPos, newPos, Color.red, 10);
                    body.position = newPos;

                    body.rotation += moveRadians * Mathf.Rad2Deg;
                }
                else if (dir > 0.0f)
                {
                    Debug.Log("Rotate right");
                    Vector2 moveVec;

                    moveVec.x = Mathf.Cos(initialAngle - moveRadians) - Mathf.Cos(initialAngle);
                    moveVec.x *= radius;
                    moveVec.y = Mathf.Sin(initialAngle - moveRadians) - Mathf.Sin(initialAngle);
                    moveVec.y *= radius;

                    Debug.Log("( " +
                        moveVec.x.ToString() + ", " +
                        moveVec.y.ToString() + " )"
                    );
                    var oldPos = body.position;
                    var newPos = oldPos - moveVec;
                    Debug.DrawLine(oldPos, newPos, Color.red, 10);
                    body.position = newPos;

                    body.rotation -= moveRadians * Mathf.Rad2Deg;
                }
                else
                {
                    Debug.Log("Straight");
                    Vector2 moveVec;

                    moveVec.x = Mathf.Cos(initialAngle + Mathf.PI / 2.0f);
                    moveVec.x *= move;
                    moveVec.y = Mathf.Sin(initialAngle + Mathf.PI / 2.0f);
                    moveVec.y *= move;

                    var oldPos = body.position;
                    var newPos = oldPos + moveVec;
                    Debug.DrawLine(oldPos, newPos, Color.red, 10);
                    body.position = newPos;
                }
            }
            else if(dir != 0.0f)
            {
                body.rotation -= (Time.fixedDeltaTime * dir) * Mathf.Rad2Deg;
            }
        }
    }
}

