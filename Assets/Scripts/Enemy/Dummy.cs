using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dummy : Enemy
{
    #region State Declaration
    public enum STATES
    {
        waiting,
        recoiling
    }
    WaitingState waitingState;
    RecoilingState recoilingState;

    Dictionary<STATES, State> stateDict;
    #endregion

    [SerializeField] STATES current = default;

    InputMaster controls;
    Vector3 locamotionVel;
    float maxLocamotionSpeed;
    Vector3 influenceVel;
    [Tooltip("How much to multiply the influence by each tick to dampen it")]
    float influenceDampening;
    Rigidbody rb = null;
    [SerializeField] Transform target = null;

    void Awake()
    {
        HitboxSetup();
        #region State Initalization
        // Init States
        waitingState = new WaitingState(this);
        recoilingState = new RecoilingState(this);

        // Init dictionary
        stateDict = new Dictionary<STATES, State>();
        stateDict.Add(STATES.waiting, waitingState);
        stateDict.Add(STATES.recoiling, recoilingState);
        #endregion
        controls = new InputMaster();
        rb = transform.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        locamotionVel = new Vector3();
        maxLocamotionSpeed = 0.05f;
        influenceVel = new Vector3();
        influenceDampening = 0.8f;

    }

    void Update()
    {
        stateDict[current].StateUpdate();
        Keyboard kb = InputSystem.GetDevice<Keyboard>();
        if (kb.pKey.wasPressedThisFrame)
        {
            //Debug.Log("Jump");
            //rb.MovePosition(rb.position + Vector3.forward * 4);
            rb.AddForce(Vector3.up * 100);
        }
    }

    private void FixedUpdate()
    {
        stateDict[current].StateFixedUpdate();
        Move();
    }

    public void Move()
    {
        if (target != null)
        {
            Vector3 towardsTarget = (target.position - rb.position).normalized;
            Debug.DrawRay(rb.position, towardsTarget * 2, Color.red);
            locamotionVel = towardsTarget * maxLocamotionSpeed;
        }
        

        rb.MovePosition(rb.position + locamotionVel + influenceVel);
        influenceVel *= influenceDampening;
    }

    protected override void OnHitboxTrigger(HitInfo info)
    {
        //Debug.Log(info.sourceBox);
        if (current == STATES.waiting)
        {
            waitingState.OnHit();
            Vector3 impactVel = (transform.position - info.attackerPos).normalized;
            influenceVel += impactVel;
        }
    }

    public void ChangeState(STATES _nextState)
    {
        stateDict[current].OnExit();
        current = _nextState;
        stateDict[current].OnEnter();
    }
}

#region States
class State
{
    protected Dummy SM;
    public State(Dummy _SM)
    {
        SM = _SM;
    }

    public virtual void OnEnter() { }
    public virtual void StateUpdate() { }
    public virtual void StateFixedUpdate() { }
    public virtual void OnExit() { }
}

class WaitingState : State
{
    public WaitingState(Dummy _SM) : base(_SM) { }

    public override void OnEnter()
    {
        
    }

    public void OnHit()
    {
        SM.ChangeState(Dummy.STATES.recoiling);
    }
}

class RecoilingState : State
{
    float recoilLength = 1f;
    float recoilTime;

    Quaternion targetRotQuat;
    public RecoilingState(Dummy _SM) : base(_SM) { }

    public override void OnEnter()
    {
        recoilTime = Time.time;
        Vector3 targetRotEuler = SM.transform.eulerAngles;
        targetRotEuler.y -= 180;
        targetRotQuat = Quaternion.Euler(targetRotEuler);
    }

    public override void StateFixedUpdate()
    {
        SM.transform.rotation = Quaternion.RotateTowards(SM.transform.rotation, targetRotQuat, 3f);

        if (Time.time - recoilTime > recoilLength)
        {
            SM.ChangeState(Dummy.STATES.waiting);
            return;
        }
    }
}
#endregion
