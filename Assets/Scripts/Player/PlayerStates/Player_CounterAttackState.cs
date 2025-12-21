using UnityEngine;

public class Player_CounterAttackState : PlayerState
{
    private Player_Combat combat;
    private bool counterdSombody;
    public Player_CounterAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        combat = player.GetComponent<Player_Combat>();
    }
    public override void Enter()
    {
        base.Enter();
        stateTimer = combat.GetCounterRecoveryDurtaion();

        counterdSombody = combat.CounterAttackPerformed();
        anim.SetBool("counterAttackPerformed", counterdSombody);

    }
    public override void Update()
    {
        base.Update();

        player.SetVelocity(0, rb.linearVelocity.y);
        
        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);

        if (stateTimer < 0 && counterdSombody == false)
            stateMachine.ChangeState(player.idleState);
    }
}
