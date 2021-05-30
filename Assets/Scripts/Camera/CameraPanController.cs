using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPanController : MonoBehaviour
{
    private Camera _camera;
    private Vector3 initialVector;

    [SerializeField]
    private Transform targetTransform;

    [SerializeField]
    private float cameraMoveSpeedModifier = 0.2f;

    [SerializeField]
    float smoothFactor = 0.5f;

    //[SerializeField]
    private float rotateSpeed = 20;

    //[SerializeField]
    private float angleMax = 30f;

    private float angleMin = 10f;




    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 200;
        _camera = Camera.main;
        //cameraOffset = _camera.transform.position - targetTransform.position;
        initialVector = transform.position - targetTransform.position;
        initialVector.y = 0;
    }

    // Update is called once per frame
    void Update()
    {

        _camera.transform.LookAt(targetTransform);

        Vector3 forward = new Vector3(_camera.transform.forward.x, 0, _camera.transform.forward.z);
        Vector3 rightMovement = _camera.transform.right * Input.GetAxis("Horizontal");
        Vector3 upMovement = forward * Input.GetAxis("Vertical");

        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

        //transform.forward = heading;

        Vector3 move = forward * Input.GetAxis("Vertical") + _camera.transform.right * Input.GetAxis("Horizontal");

        targetTransform.Translate(cameraMoveSpeedModifier * Time.deltaTime * move);


        transform.RotateAround(targetTransform.position, Vector3.up, rotateSpeed * Time.deltaTime * Input.GetAxis("RotateHorizontal"));



        if(Input.GetAxis("RotateVertical") != 0)
        {
            float rotateDegrees = rotateSpeed * -Input.GetAxis("RotateVertical");
            Vector3 direction = targetTransform.position - transform.position; // Vector from camera to player
            Vector3 directionRight = Vector3.Cross(direction, Vector3.up); // right vector of the vector above


            float angle = Vector3.Angle(Vector3.up, direction);

            if (Vector3.Cross(Vector3.up, direction).z < 0) angle = -angle;
            float newAngle = Mathf.Clamp(angle + rotateDegrees, angleMin, angleMax);

            rotateDegrees = newAngle - rotateDegrees;

            transform.RotateAround(targetTransform.position, directionRight, rotateDegrees);
        }
        





        //float rotateInputX;;
        //rotateInputX = Input.GetAxis("RotateHorizontal") * cameraMoveSpeedModifier * Time.deltaTime * _camera.transform.right;
        //moveInputX = Input.GetAxis("Horizontal") * cameraMoveSpeedModifier * Time.deltaTime * targetTransform.position.x;
        //float rotateInputY;
        //rotateInputY = Input.GetAxis("RotateVertical") * cameraMoveSpeedModifier * Time.deltaTime * _camera.orthographicSize;
        //moveInputY = Input.GetAxis("Vertical") * cameraMoveSpeedModifier * Time.deltaTime * targetTransform.position.y;
        //_camera.transform.Translate(new Vector3(rotateInputX, rotateInputY));




    }
}
