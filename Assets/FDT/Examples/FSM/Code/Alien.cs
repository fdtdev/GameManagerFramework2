using UnityEngine;
using System.Collections;
using com.FDT.Common;

public class Alien : MonoBehaviour {

	public Vector2 dir;
	public Rigidbody2D alien;
	public SpriteRenderer spr;
	public CircleCollider2D c2d;
	public float vel = 1f;
	public FSM fsm;
	protected float sittingWait = 0;
	[CustomRange(1f ,60f)]
	public float SitMinTime=20f;

	[CustomRange(1f , 5f)]
	public float IdleMaxTime = 3f;
	protected float IdleTime = 0f;

	public void SelectDirection()
	{
		dir.x = Random.Range(-1f, 1f);
		dir.y = Random.Range(-1f, 1f);
		dir = dir.normalized;
	}
	public void InitWalking()
	{
		fsm.ChangeState("Walk");
	}
	public void SitStart()
	{
		sittingWait = Random.Range(SitMinTime, SitMinTime+2f);
	}
	public void SitUpdate(float delta, float t, float rt)
	{
		if (t>=sittingWait)
			fsm.ChangeState("Idle");
	}
	public void WalkStart()
	{
		sittingWait = Random.Range(SitMinTime, SitMinTime+5f);
	}
	public void WalkUpdate(float delta, float t, float rt)
	{
		if (t >sittingWait)
		{
			fsm.ChangeState("Sit");
			return;
		}
		Vector3 pos = alien.position;
		pos.x+=(dir.x*vel*delta);
		pos.y+=(dir.y*vel * delta);
		if (dir.x<0)
			spr.flipX = true;
		else
			spr.flipX = false;
		if (!CheckCollision(pos))
			alien.MovePosition(pos);
		else
			fsm.ChangeState("Idle");
	}
	protected bool CheckCollision(Vector3 p)
	{
		c2d.enabled = false;
		Vector2 cp = p;
		cp.x += c2d.offset.x;
		cp.y += c2d.offset.y;
		var c = Physics2D.OverlapCircle(cp, c2d.radius);
		c2d.enabled = true;
		if (c!= null)
			return true;
		else
			return false;
	}
	public void IdleStart()
	{
		IdleTime = Random.Range(1f, IdleMaxTime);
	}
	public void IdleUpdate(float delta, float t, float rt)
	{
		if (t>IdleTime)
		{
			SelectDirection();
			Vector3 pos = alien.position;
			pos.x+=(dir.x*c2d.radius*0.5f);
			pos.y+=(dir.y*c2d.radius*0.5f);
			if (!CheckCollision(pos))
				fsm.ChangeState("Walk");
		}
	}
}
