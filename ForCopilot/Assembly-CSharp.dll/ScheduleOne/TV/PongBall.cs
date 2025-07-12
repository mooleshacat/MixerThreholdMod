using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.TV
{
	// Token: 0x020002A8 RID: 680
	public class PongBall : MonoBehaviour
	{
		// Token: 0x06000E2A RID: 3626 RVA: 0x000045B1 File Offset: 0x000027B1
		private void FixedUpdate()
		{
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x0003EC64 File Offset: 0x0003CE64
		private void OnCollisionEnter(Collision collision)
		{
			if (collision.collider.gameObject.name == "LeftGoal")
			{
				this.Game.GoalHit(Pong.ESide.Left);
			}
			else if (collision.collider.gameObject.name == "RightGoal")
			{
				this.Game.GoalHit(Pong.ESide.Right);
			}
			if (this.RB.velocity.y < 0.1f && collision.collider.GetComponent<PongPaddle>() != null)
			{
				float magnitude = this.RB.velocity.magnitude;
				this.RB.AddForce(new Vector3(0f, UnityEngine.Random.Range(-this.RandomForce, this.RandomForce), 0f), 2);
				this.RB.velocity = this.RB.velocity.normalized * magnitude;
			}
			if (this.onHit != null)
			{
				this.onHit.Invoke();
			}
		}

		// Token: 0x04000E96 RID: 3734
		public Pong Game;

		// Token: 0x04000E97 RID: 3735
		public RectTransform Rect;

		// Token: 0x04000E98 RID: 3736
		public Rigidbody RB;

		// Token: 0x04000E99 RID: 3737
		public float RandomForce = 0.5f;

		// Token: 0x04000E9A RID: 3738
		public UnityEvent onHit;
	}
}
