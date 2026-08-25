using UnityEngine;

public class CombatFeedbackController : MonoBehaviour
{
    public static CombatFeedbackController Instance { get; private set; }
    [SerializeField] private Entity localPlayer;
    [SerializeField] private float cameraShakeImpact;

    [SerializeField] private float playerAttackImpactHitstop;
    [SerializeField] private float playerHitImpactHitstop;
    private void Awake()
    {
        Instance = this;
    }
    public void OnHit(Entity attacker, Entity target)
    {
        if (attacker != localPlayer && target != localPlayer)
            return;
        if (attacker == localPlayer)
            HitstopController.Instance.DoHitstop(playerAttackImpactHitstop);
        if (target == localPlayer)
            HitstopController.Instance.DoHitstop(playerHitImpactHitstop);

        TriggerCameraShake();
    }
    private void TriggerCameraShake()
    {
        return;
    }
}
