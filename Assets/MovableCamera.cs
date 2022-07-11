using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableCamera : MonoBehaviour
{
    // Start is called before the first frame update
    private bool IsGrabbed = false;
    private float grabDistance = 1.0f;
    private Vector3 cursorPos;
    private Camera m_Cam;

    public float scrollScale = 0.95f;

    void Start()
    {
        m_Cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //Debug.Log("MouseDown");
            cursorPos = Input.mousePosition;
            cursorPos.z = 0;
            var distance = transform.position - cursorPos;
            IsGrabbed = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            IsGrabbed = false;
        }

        if (IsGrabbed)
        {
            var newCursorPos = Input.mousePosition;
            newCursorPos.z = 0;

            transform.position = transform.position - (m_Cam.ScreenToWorldPoint(newCursorPos) - m_Cam.ScreenToWorldPoint(cursorPos));
            cursorPos = newCursorPos;
        }

        m_Cam.orthographicSize *= Mathf.Pow(scrollScale, Input.mouseScrollDelta.y);
    }
}
