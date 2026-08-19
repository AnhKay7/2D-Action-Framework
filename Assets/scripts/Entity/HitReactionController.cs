using UnityEngine;

public class HitReactionController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color hitColor = Color.orange;
    [SerializeField] private float flashDuration = 0.08f;

    private Color originalColor;
    private float remainingFlashTime;
    private bool isFlashing;

    private void Awake()
    {
        originalColor = spriteRenderer.color;
    }
    private void Update()
    {
        if (!isFlashing)
            return;

        remainingFlashTime -= Time.unscaledDeltaTime;

        if (remainingFlashTime <= 0)
            StopFlash();
    }
    public void ReactionToHit()
    {
        StartFlash();
    }
    private void StartFlash()
    {
        spriteRenderer.color = hitColor;
        remainingFlashTime = flashDuration;
        isFlashing = true;
    }
    private void StopFlash()
    {
        spriteRenderer.color = originalColor;
        remainingFlashTime = -100f;
        isFlashing = false;
    }
}
