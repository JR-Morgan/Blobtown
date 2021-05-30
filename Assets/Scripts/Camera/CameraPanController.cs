using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPanController : MonoBehaviour
{
    private Camera _camera;
    [SerializeField]
    private float cameraMoveSpeedModifier = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        float moveInputX;
        moveInputX = Input.GetAxis("Horizontal") * cameraMoveSpeedModifier * Time.deltaTime * _camera.orthographicSize;
        float moveInputY;
        moveInputY = Input.GetAxis("Vertical") * cameraMoveSpeedModifier * Time.deltaTime * _camera.orthographicSize;

        

        _camera.transform.Translate(new Vector3(moveInputX,moveInputY));
    }
}
