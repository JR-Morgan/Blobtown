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

    private Vector3 cameraOffset;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 200;
        _camera = Camera.main;
        cameraOffset = _camera.transform.position - targetTransform.position;
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


        float rotateInputX;
        rotateInputX = Input.GetAxis("RotateHorizontal") * cameraMoveSpeedModifier * Time.deltaTime * _camera.orthographicSize;
        //moveInputX = Input.GetAxis("Horizontal") * cameraMoveSpeedModifier * Time.deltaTime * targetTransform.position.x;
        float rotateInputY;
        rotateInputY = Input.GetAxis("RotateVertical") * cameraMoveSpeedModifier * Time.deltaTime * _camera.orthographicSize;
        //moveInputY = Input.GetAxis("Vertical") * cameraMoveSpeedModifier * Time.deltaTime * targetTransform.position.y;

        _camera.transform.Translate(new Vector3(rotateInputX, rotateInputY));




    }
}
