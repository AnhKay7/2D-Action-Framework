using UnityEngine;

public class HitstopController : MonoBehaviour
{
    public static HitstopController Instance { get; private set; }
    private bool isHitstopActive = false;
    private float remainingTime = -100f;
    private float? previousTimeScale;

    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        if (!isHitstopActive)
            return;

        remainingTime -= Time.unscaledDeltaTime;

        if (remainingTime <= 0)
            StopHitstop();
    }
    public void DoHitstop(float duration)
    {
        remainingTime = Mathf.Max(remainingTime, duration);
        if (!isHitstopActive)
            StartHitstop(duration);
    }
    private void StartHitstop(float duration)
    {
        isHitstopActive = true;
        previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        remainingTime = Mathf.Max(remainingTime, duration);
    }
    private void StopHitstop()
    {
        isHitstopActive = false;
        Time.timeScale = previousTimeScale.Value;
        remainingTime = -100f;
    }
    private void EmergencyReset()
    {
        isHitstopActive = false;
        Time.timeScale = (previousTimeScale != null ? previousTimeScale.Value : 1f);
        remainingTime = -100f;
    }
    private void OnDestroy()
    {
        if (isHitstopActive)
            EmergencyReset();
    }
    private void OnDisable()
    {
        if (isHitstopActive)
            EmergencyReset();
        if (Instance == this)
            Instance = null;
    }
    private void OnEnable()
    {
        Instance = this;
    }
}
