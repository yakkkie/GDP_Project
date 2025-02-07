using FiniteStateMachine;
using System.Collections.Generic;
using UnityEngine;


public class MouseFSM : FSM
{
    public Dictionary<MouseState.MouseStateName, MouseState> StateDict = new Dictionary<MouseState.MouseStateName, MouseState>();

    public MouseState currentState;

    //public MouseState_IDLE MouseState_IDLE;
    //public MouseState_HOLDITEM MouseState_HOLDITEM;
    //public MouseState_DROPITEM MouseState_DROPITEM;


    public Animator animator;

    public void ChangeState(MouseState nextState)
    {
        currentState.ExitState();

        currentState = nextState;
        currentState.EnterState();
    }

    public void Update()
    {
        currentState.UpdateState();
    }


}
