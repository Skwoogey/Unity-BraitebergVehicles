using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableSun : MonoBehaviour
{
    // Start is called before the first frame update
    private bool IsGrabbed = false;
    private float grabDistance = 1.0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("MouseDown");
            var cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cursorPos.z = 0;
            var distance = transform.position - cursorPos;
            //Debug.Log("Distance: " + distance.magnitude.ToString());
            //Debug.DrawLine(cursorPos, transform.position, Color.black, 1);
            if (distance.magnitude < grabDistance)
                IsGrabbed = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            IsGrabbed = false;
        }

        if(IsGrabbed)
        {
            var cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cursorPos.z = 0;
            transform.position = cursorPos;
        }
    }
}
