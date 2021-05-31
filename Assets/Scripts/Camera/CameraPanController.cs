using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPanController : MonoBehaviour
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

    [SerializeField, Range(0, 90)]
    private float angleMax = 0f;

    [SerializeField, Range(-90, 0)]
    private float angleMin = -90f;


    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        _camera.transform.LookAt(targetTransform);
    }

    // Update is called once per frame
    void Update()
    {

        float rotateDegrees = rotateSpeed * -Input.GetAxis("RotateVertical") * Time.deltaTime;
        if(rotateDegrees != 0)
        {
            var angle = Vector3.Angle(Vector3.up, targetTransform.up);
            if (Vector3.Cross(Vector3.up, transform.up).z < 0) angle = -angle;
            var newAngle = Mathf.Clamp(angle + rotateDegrees, angleMin, angleMax);
            rotateDegrees = newAngle - angle;
        }

        targetTransform.Rotate(0, 0, rotateDegrees, relativeTo:Space.Self);

        rotateDegrees = rotateSpeed * Input.GetAxis("RotateHorizontal") * Time.deltaTime;
        targetTransform.Rotate(0, rotateDegrees, 0, relativeTo: Space.World);



        Vector3 forward = _camera.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);

        Vector3 move = forward * Input.GetAxis("Vertical") + _camera.transform.right * Input.GetAxis("Horizontal");

        targetTransform.Translate(cameraMoveSpeedModifier * Time.deltaTime * move, relativeTo:Space.World);

    }
}
