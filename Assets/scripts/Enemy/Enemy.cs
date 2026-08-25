using System;
using UnityEngine;

public class Enemy : Entity
{
    #region Component
    public KinematicCharacterController Movement { get; private set; }
    public Health Health { get; private set; }
    public EnemyDetection Detection { get; private set; }
    public EnemyMovement MovementController { get; private set; }
    public GroundSensor Ground { get; private set; }
    public EnemyCombat Combat { get; private set; }
    public KnockbackReceiver Knockback { get; private set; }
    public HitstunReceiver HitstunReceiver { get; private set; }
    public HitReceiver HitReceiver { get; private set; }
    #endregion

    #region Runtime Systems
    public VelocityResolver VelocityResolver { get; private set; } = new VelocityResolver();
    public ImpulseController ImpulseController { get; private set; } = new ImpulseController();
    #endregion

    #region Gravity
    [SerializeField] private float gravityScale = 5f;
    public float GravityScale => gravityScale;
    #endregion

    private void Awake()
    {
        Movement = GetComponent<KinematicCharacterController>();
        Health = GetComponent<Health>();
        Detection = GetComponentInChildren<EnemyDetection>();
        MovementController = GetComponent<EnemyMovement>();
        Ground = GetComponent<GroundSensor>();
        Combat = GetComponent<EnemyCombat>();
        Knockback = GetComponent<KnockbackReceiver>();
        HitstunReceiver = GetComponent<HitstunReceiver>();
        HitReceiver = GetComponent<HitReceiver>();

        Health.OnDeath += Die;
    }
    private void Start()
    {
        Knockback.Initialize(ImpulseController);
    }
    private void Die()
    {
        Destroy(gameObject);
    }
    private void Update()
    {
        MovementController.SetTarget(Detection.Target);
        Combat.SetTarget(Detection.Target);
        Combat.FrameUpdate();
    }
    private void FixedUpdate()
    {
        Ground.CheckGround(Movement.velocityY);

        VelocityResolver.SetBaseXY(Movement.velocityX, Movement.velocityY);

        float velocityY = VelocityResolver.baseVelocityY;
        float gravity = Physics2D.gravity.y * gravityScale * Time.fixedDeltaTime;
        VelocityResolver.SetBaseY(velocityY + gravity);

        MovementController.PhysicsUpdate(VelocityResolver);
        Combat.PhysicsUpdate(VelocityResolver);
        ImpulseController.PhysicsUpdate(VelocityResolver);

        VelocityResolver.Resolve();
        Movement.SetVelocityXY(VelocityResolver.VelocityX, VelocityResolver.VelocityY);

        Movement.PhysicsUpdate();
    }
}
