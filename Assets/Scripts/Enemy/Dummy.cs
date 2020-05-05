using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    }

    void Update()
    {
        stateDict[current].StateUpdate();
    }

    private void FixedUpdate()
    {
        stateDict[current].StateFixedUpdate();
    }

    protected override void OnHitboxTrigger(HitInfo info)
    {
        //Debug.Log(info.sourceBox);
        if (current == STATES.waiting)
        {
            waitingState.OnHit();
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
    float recoilLength = 3f;
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
