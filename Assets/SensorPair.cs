using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorPair : MonoBehaviour
{
    public GameObject LeftSensor;
    public GameObject RightSensor;
    public Color color;
     
    public LayerMask Stimul;
    public float FOV_InnerAngle;
    public float FOV_OutterAngle;
    public bool inverseConnection = false;
    public float connectionWeight = 1.0f;

    public float Threshold = 0.0f;
    public float MaxValue = 1.0f;
    public float Skewness = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        color.a = 0.3f;

        LeftSensor.GetComponent<SpriteRenderer>().color = color;
        LeftSensor.SendMessage("SetLeftBound", FOV_OutterAngle);
        LeftSensor.SendMessage("SetRightBound", FOV_InnerAngle);
        //LeftSensor.SendMessage("SetStimulFilter", Stimul);

        RightSensor.GetComponent<SpriteRenderer>().color = color;
        RightSensor.SendMessage("SetLeftBound", FOV_InnerAngle);
        RightSensor.SendMessage("SetRightBound", FOV_OutterAngle);
        //RightSensor.SendMessage("SetStimulFilter", Stimul);
    }
}
