using UnityEngine;

public class CameraLookTarget : MonoBehaviour
{
    [SerializeField] private Entity target;
    [SerializeField] private float lookDistance = 3f;
    [SerializeField] private float switchSpeed = 10f;

    private float currentOffset;
    private KinematicCharacterController targetMovement;
    private void Start()
    {
        if (target == null)
        {
            Debug.Log("No target for camera");
            enabled = false;
            return;
        }
        targetMovement = target.GetComponent<KinematicCharacterController>();
        currentOffset = target.FacingDirection * lookDistance;
        Vector3 basePos = targetMovement != null ? (Vector3)targetMovement.InterpolatedPosition : target.transform.position;
        transform.position = basePos + new Vector3(currentOffset, 0f, 0f);
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        float targetOffset = target.FacingDirection * lookDistance;
        currentOffset = Mathf.MoveTowards(currentOffset, targetOffset, switchSpeed * Time.deltaTime);
        Vector3 basePos = targetMovement != null ? (Vector3)targetMovement.InterpolatedPosition : target.transform.position;
        transform.position = basePos + new Vector3(currentOffset, 0f, 0f);
    }
}
