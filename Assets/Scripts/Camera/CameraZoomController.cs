using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomController : MonoBehaviour
{

    private Camera _camera;
    private float targetZoom;
    [SerializeField]
    private float zoomFactor = 3;
    [SerializeField]
    private float zoomLerpSpeed = 10;
    [SerializeField]
    private float minZoom = 1;
    [SerializeField]
    private float maxZoom = 100;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        targetZoom = _camera.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        float scrollInput;
        scrollInput = Input.GetAxis("Mouse ScrollWheel");

        targetZoom -= scrollInput * zoomFactor;
        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, targetZoom, Time.deltaTime * zoomLerpSpeed);
    }
}
