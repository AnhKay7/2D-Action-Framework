using UnityEngine;

public class HitstunReceiver : MonoBehaviour, IHitstunnable
{

    private float hitstunEndTime = -100f;
    public bool IsHitstunned => hitstunEndTime > Time.time;
    public void ApplyHitstun(float duration)
    {
        hitstunEndTime = Mathf.Max(hitstunEndTime, Time.time + duration);
    }
}
