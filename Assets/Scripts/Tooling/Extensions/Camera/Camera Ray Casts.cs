using UnityEngine;

public static class CameraRayCasts
{
    public static Ray ForwardsRay(this Camera camera, Vector3 point)
    {
        Vector3 cameraCenter = camera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, camera.nearClipPlane));
        return new Ray(cameraCenter, camera.transform.forward);
    }

    public static Ray ForwardsRay(this Camera camera) => ForwardsRay(camera, new Vector3(Screen.width / 2f, Screen.height / 2f, camera.nearClipPlane));
}
