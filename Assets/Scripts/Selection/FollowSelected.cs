using UnityEngine;

public class FollowSelected : MonoBehaviour
{
    private Quaternion rotation;
    void Start()
    {
        InputManager.Instance.OnSelectableChange.AddListener(SetSelected);
        rotation = this.transform.rotation;
        
    }

    private void SetSelected(Selectable s)
    {
        Vector3 position = transform.localPosition;

        transform.parent = s.transform;

        this.transform.localPosition = position;
        this.transform.localRotation = rotation;
    }

}
