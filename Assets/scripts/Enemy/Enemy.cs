using System;
using UnityEngine;

public class Enemy : Entity
{
    #region Config
    [SerializeField] private float reactTime;
    public float ReactTime => reactTime;
    [SerializeField] private float chaseRange;
    public float ChaseRange => chaseRange;
    private float giveUpOnLostTargetTime = 4f;
    public float GiveUpOnLostTargetTime => giveUpOnLostTargetTime;
    #endregion
    #region Component
    public KinematicCharacterController Kinematic { get; private set; }
    public Health Health { get; private set; }
    public EnemyDetection Detection { get; private set; }
    public EnemyMovement Movement { get; private set; }
    public GroundSensor Ground { get; private set; }
    public EnemyCombat Combat { get; private set; }
    public KnockbackReceiver Knockback { get; private set; }
    public HitstunReceiver HitstunReceiver { get; private set; }
    public HitReceiver HitReceiver { get; private set; }
    #endregion

    #region Runtime Systems & Variable
    public VelocityResolver VelocityResolver { get; private set; } = new VelocityResolver();
    public ImpulseController ImpulseController { get; private set; } = new ImpulseController();
    public Entity CurrentTarget { get; private set; } = null;
    #endregion

    #region Gravity
    [SerializeField] private float gravityScale = 5f;
    public float GravityScale => gravityScale;
    #endregion

    #region StateMachine
    public EnemyStateMachine StateMachine { get; private set; }
    public EnemyIdleState IdleState { get; private set; }
    public EnemyChaseState ChaseState { get; private set; }
    public EnemyReactState ReactState { get; private set; }
    public EnemyAttackState AttackState { get; private set; }
    public EnemyRecoveryState RecoveryState { get; private set; }
    #endregion
    private void Awake()
    {
        StateMachine = new EnemyStateMachine();
        IdleState = new EnemyIdleState(this, StateMachine);
        ChaseState = new EnemyChaseState(this, StateMachine);
        ReactState = new EnemyReactState(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);
        RecoveryState = new EnemyRecoveryState(this, StateMachine);

        Kinematic = GetComponent<KinematicCharacterController>();
        Health = GetComponent<Health>();
        Detection = GetComponentInChildren<EnemyDetection>();
        Movement = GetComponent<EnemyMovement>();
        Ground = GetComponent<GroundSensor>();
        Combat = GetComponent<EnemyCombat>();
        Knockback = GetComponent<KnockbackReceiver>();
        HitstunReceiver = GetComponent<HitstunReceiver>();
        HitReceiver = GetComponent<HitReceiver>();

        Health.OnDeath += Die;
    }
    private void Start()
    {
        StateMachine.Initialize(IdleState);
        Knockback.Initialize(ImpulseController);
    }
    private void Die()
    {
        Destroy(gameObject);
    }
    private void Update()
    {
        StateMachine.CurrentState.FrameUpdate();
        Combat.SetTarget(Detection.Target);
        Combat.FrameUpdate();
    }
    private void FixedUpdate()
    {
        Ground.CheckGround(Kinematic.velocityY);

        VelocityResolver.SetBaseXY(Kinematic.velocityX, Kinematic.velocityY);

        float velocityY = VelocityResolver.baseVelocityY;
        float gravity = Physics2D.gravity.y * gravityScale * Time.fixedDeltaTime;
        VelocityResolver.SetBaseY(velocityY + gravity);

        Movement.PhysicsUpdate(VelocityResolver);
        Combat.PhysicsUpdate(VelocityResolver);
        ImpulseController.PhysicsUpdate(VelocityResolver);

        VelocityResolver.Resolve();
        Kinematic.SetVelocityXY(VelocityResolver.VelocityX, VelocityResolver.VelocityY);

        Kinematic.PhysicsUpdate();
    }

    #region CurrentTarget
    public void AcquireTarget(Entity target)
    {
        CurrentTarget = target;
    }
    public void ClearCurrentTarget()
    {
        CurrentTarget = null;
    }
    #endregion
}
