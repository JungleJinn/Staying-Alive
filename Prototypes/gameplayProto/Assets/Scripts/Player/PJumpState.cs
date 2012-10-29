using UnityEngine;
using System.Collections;

public class PJumpState : FSMState<Player>
{
	private float jumpTimer;

	public override void Enter(Player player)
	{
		player.renderer.material.color = new Color(1, 0, 1);
		jumpTimer = 0;

		Vector3 vel = player.rigidbody.velocity;
		vel.y = player.settings.JumpAcceleration.y;
		player.rigidbody.velocity = vel;

		player.JumpKeyReleased = false;
	}

	public override void Execute(Player player)
	{
		jumpTimer += Time.deltaTime;

		if (jumpTimer > player.settings.MaxJumpTime || !Input.GetKey(player.settings.KeyJump))
		{
			player.FSM.ChangeState(player.FallState);
		}
        // player 2 controls singing
        if ((Input.GetKey(player.settings.KeyPlayer2Input) && player.oscManager.Singing) || Input.GetKey(player.settings.DEBUG_KeySinging))
		{
			player.FSM.ChangeState(player.FloatState);
		}
	}

	public override void ExecuteFixed(Player player)
	{
		//player.rigidbody.AddForce(
		//    Vector3.up * player.settings.jumpSpeed.y * Time.deltaTime,
		//    ForceMode.Impulse);

		if (Input.GetKey(player.settings.KeyRight))
		{
			player.rigidbody.AddForce(
				Vector3.right * player.settings.JumpAcceleration.x * Time.deltaTime,
				ForceMode.Impulse);
		}
		else if (Input.GetKey(player.settings.KeyLeft))
		{
			player.rigidbody.AddForce(
				Vector3.left * player.settings.JumpAcceleration.x * Time.deltaTime,
				ForceMode.Impulse);
		}

		if (Mathf.Abs(player.rigidbody.velocity.x) > player.settings.MaxHorizontalJumpVelocity)
		{
			Vector3 vel = player.rigidbody.velocity;
			vel.x = player.settings.MaxHorizontalJumpVelocity * (player.rigidbody.velocity.x > 0 ? 1 : -1);
			player.rigidbody.velocity = vel;
		}


	}

	public override void Exit(Player player)
	{
	}
}
