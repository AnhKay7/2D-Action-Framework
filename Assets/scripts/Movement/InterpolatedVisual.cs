using UnityEngine;

[DefaultExecutionOrder(-100)]
public class InterpolatedVisual : MonoBehaviour
{
    [SerializeField] private Transform visual;
    private KinematicCharacterController movement;
    private Vector3 originalVisualOffet;

    private void Awake()
    {
        movement = GetComponent<KinematicCharacterController>();
        originalVisualOffet = visual.localPosition;
    }

    private void LateUpdate()
    {
        Vector3 offset = transform.TransformVector(originalVisualOffet);
        Vector2 pos = movement.InterpolatedPosition;
        visual.position = new Vector3(pos.x, pos.y, transform.position.z) + offset;
    }
}
