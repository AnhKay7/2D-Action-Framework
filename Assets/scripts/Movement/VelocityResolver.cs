public class VelocityResolver
{
    public float VelocityX { get; private set; }
    public float VelocityY { get; private set; }

    public float baseVelocityX { get; private set; }
    public float baseVelocityY { get; private set; }

    private float? overrideVelocityX;
    private float? overrideVelocityY;

    private float? dominantVelocityX;
    private float? dominantVelocityY;
    public void SetBaseXY(float x, float y)
    {
        baseVelocityX = x;
        baseVelocityY = y;
    }
    public void SetBaseX(float x)
    {
        baseVelocityX = x;
    }
    public void SetBaseY(float y)
    {
        baseVelocityY = y;
    }
    public void RequestOverrideX(float x)
    {
        overrideVelocityX = x;
    }
    public void RequestOverrideY(float y)
    {
        overrideVelocityY = y;
    }
    public void RequestDominant(float x, float y)
    {
        dominantVelocityX = x;
        dominantVelocityY = y;
    }
    public void RequestDominantX(float x)
    {
        dominantVelocityX = x;
    }
    public void RequestDominantY(float y)
    {
        dominantVelocityY = y;
    }
    public void Resolve()
    {
        VelocityX = baseVelocityX;
        VelocityY = baseVelocityY;

        if (overrideVelocityX.HasValue)
        {
            VelocityX = overrideVelocityX.Value;
        }
        if (overrideVelocityY.HasValue)
        {
            VelocityY = overrideVelocityY.Value;
        }

        if (dominantVelocityX.HasValue)
        {
            VelocityX = dominantVelocityX.Value;
        }
        if (dominantVelocityY.HasValue)
        {
            VelocityY = dominantVelocityY.Value;
        }
        ResetRequest();
    }
    private void ResetRequest()
    {
        overrideVelocityX = null;
        overrideVelocityY = null;
        dominantVelocityX = null;
        dominantVelocityY = null;
    }
}
