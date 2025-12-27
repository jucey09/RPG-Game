using UnityEngine;
using UnityEngine.XR;

public class Player_GroundedState : PlayerState
{
    public Player_GroundedState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (rb.linearVelocity.y < 0 && player.groundDetected == false)
            stateMachine.ChangeState(player.fallState);

        if (input.Player.Jump.WasPressedThisFrame() || input.Player.CJump.WasPressedThisFrame())
            stateMachine.ChangeState(player.jumpState);

        if (input.Player.Attack.WasPressedThisFrame() || input.Player.CAttack.WasPressedThisFrame())
            stateMachine.ChangeState(player.basicAttackState);

        if (input.Player.CounterAttack.WasPressedThisFrame() || input.Player.CCounterAttack.WasPressedThisFrame())
            stateMachine.ChangeState(player.counterAttackState);
        
        if(input.Player.RangeAttack.WasPressedThisFrame()&& skillManager.swordThrow.CanUseSkill())
        {
            stateMachine.ChangeState(player.swordThrowState);
        }
        else if(input.Player.CRangedAttack.WasPressedThisFrame() && skillManager.swordThrow.CanUseSkill())    
        {
            stateMachine.ChangeState(player.swordThrowState);
        }
    }
}
