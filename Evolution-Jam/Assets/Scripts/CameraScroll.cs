using UnityEngine;
public class CameraScroll : MonoBehaviour
{
    Camera Cam;
    private void Start()
    {
        Cam = Camera.main;
    }
    void FixedUpdate()
    {
        Cam.orthographicSize = Mathf.Clamp(Cam.orthographicSize - Input.mouseScrollDelta.y, 3, 15);
    }
}
