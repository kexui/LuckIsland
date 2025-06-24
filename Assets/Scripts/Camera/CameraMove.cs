using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;
    public float slideSpeed = 10;
    public float zoomedSpeed = 0.5f;
    public float minY = 7;
    public float maxY = 17;

    //private float currentTime = 0;//已经过的时间
    //private float totalTime = 1;//总共时间
    private void Update()
    {
        Slide();
        Zoom();
    }
    private void Slide()
    {
        Vector3 direction = gameInput.GetMovement();
        if (direction == Vector3.zero) return;
        Vector3 tagetPos = transform.position + direction;
        transform.position = Vector3.Lerp(transform.position, tagetPos, Time.deltaTime * slideSpeed);
    }
    private void Zoom()
    { 
        float zoom = gameInput.GetZoom();
        float y = transform.position.y - zoom * zoomedSpeed * Time.deltaTime;
        y = Mathf.Clamp(y, minY, maxY);
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
}
