using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerPerspective : MonoBehaviour
{

    private Camera _camera;

    [SerializeField]
    private Transform targetTransform;

    [SerializeField]
    private float cameraMoveSpeedModifier = 0.2f;

    [SerializeField]
    float smoothFactor = 0.5f;

    [SerializeField]
    private float rotateSpeed = 20;

    [SerializeField, Range(90, 0)]
    private float angleMax = 85f;

    [SerializeField, Range(90, 0)]
    private float angleMin = 0f;

    [SerializeField]
    private float scrollSpeed = 10;



    private float zoomMin = -10;


    private float zoomMax = -50;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        //_camera.transform.LookAt(targetTransform);

        targetTransform.Rotate(10, 0, 0, relativeTo: Space.Self);
    }

    // Update is called once per frame
    void Update()
    {

        float rotateDegrees = rotateSpeed * Input.GetAxis("RotateVertical") * Time.deltaTime;
        if(rotateDegrees != 0)
        {
            var angle = Vector3.Angle(Vector3.up, targetTransform.up);
            //angle = -angle;
            var newAngle = Mathf.Clamp(angle + rotateDegrees, angleMin, angleMax);
            rotateDegrees = newAngle - angle;
        }

        targetTransform.Rotate(rotateDegrees, 0, 0, relativeTo:Space.Self);



        rotateDegrees = rotateSpeed * -Input.GetAxis("RotateHorizontal") * Time.deltaTime;
        targetTransform.Rotate(0, rotateDegrees, 0, relativeTo: Space.World);



        Vector3 forward = _camera.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);

        Vector3 move = forward * Input.GetAxis("Vertical") + _camera.transform.right * Input.GetAxis("Horizontal");

        targetTransform.Translate(cameraMoveSpeedModifier * Time.deltaTime * move, relativeTo:Space.World);



        if(Input.GetAxis("Mouse ScrollWheel") != 0)
        {

            float zoom = Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime;
            float futureX = _camera.transform.localPosition.z + zoom;


            if (futureX > zoomMin)
            {
                _camera.transform.localPosition = new Vector3(_camera.transform.localPosition.x, _camera.transform.localPosition.y, zoomMin);
            }
            else if (futureX < zoomMax)
            {
                _camera.transform.localPosition = new Vector3(_camera.transform.localPosition.x, _camera.transform.localPosition.y, zoomMax);
            }
            else
            {
                _camera.transform.localPosition = _camera.transform.localPosition = new Vector3(_camera.transform.localPosition.x, _camera.transform.localPosition.y, futureX);
            }
        }



        //if (_camera.transform.localPosition.x < -zoomMin)
        //{
        //    if (_camera.transform.localPosition.x >= -zoomMax)
        //    {
        //        _camera.transform.Translate(-Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime * _camera.transform.right);
        //    }
        //}
        //else
        //{
        //    _camera.transform.localPosition += Vector3.left / 100f;
        //}
        
    }
}
