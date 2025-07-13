using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.NPCs
{
	// Token: 0x020004A2 RID: 1186
	public class NPCSpeedController : MonoBehaviour
	{
		// Token: 0x060018EE RID: 6382 RVA: 0x0006E179 File Offset: 0x0006C379
		private void Awake()
		{
			this.AddSpeedControl(new NPCSpeedController.SpeedControl("default", 0, this.DefaultWalkSpeed));
		}

		// Token: 0x060018EF RID: 6383 RVA: 0x0006E194 File Offset: 0x0006C394
		private void FixedUpdate()
		{
			NPCSpeedController.SpeedControl highestPriorityControl = this.GetHighestPriorityControl();
			this.ActiveSpeedControl = highestPriorityControl;
			if (this.Movement.DEBUG)
			{
				Debug.Log("Active speed control: " + highestPriorityControl.id + ", speed : " + highestPriorityControl.speed.ToString());
			}
			this.Movement.MovementSpeedScale = highestPriorityControl.speed * this.SpeedMultiplier;
		}

		// Token: 0x060018F0 RID: 6384 RVA: 0x0006E1F9 File Offset: 0x0006C3F9
		private NPCSpeedController.SpeedControl GetHighestPriorityControl()
		{
			return this.speedControlStack[0];
		}

		// Token: 0x060018F1 RID: 6385 RVA: 0x0006E208 File Offset: 0x0006C408
		public void AddSpeedControl(NPCSpeedController.SpeedControl control)
		{
			NPCSpeedController.SpeedControl speedControl = this.speedControlStack.Find((NPCSpeedController.SpeedControl x) => x.id == control.id);
			if (speedControl != null)
			{
				speedControl.priority = control.priority;
				speedControl.speed = control.speed;
				return;
			}
			for (int i = 0; i < this.speedControlStack.Count; i++)
			{
				if (control.priority >= this.speedControlStack[i].priority)
				{
					this.speedControlStack.Insert(i, control);
					return;
				}
			}
			this.speedControlStack.Add(control);
		}

		// Token: 0x060018F2 RID: 6386 RVA: 0x0006E2B8 File Offset: 0x0006C4B8
		public NPCSpeedController.SpeedControl GetSpeedControl(string id)
		{
			return this.speedControlStack.Find((NPCSpeedController.SpeedControl x) => x.id == id);
		}

		// Token: 0x060018F3 RID: 6387 RVA: 0x0006E2E9 File Offset: 0x0006C4E9
		public bool DoesSpeedControlExist(string id)
		{
			return this.GetSpeedControl(id) != null;
		}

		// Token: 0x060018F4 RID: 6388 RVA: 0x0006E2F8 File Offset: 0x0006C4F8
		public void RemoveSpeedControl(string id)
		{
			NPCSpeedController.SpeedControl speedControl = this.speedControlStack.Find((NPCSpeedController.SpeedControl x) => x.id == id);
			if (speedControl != null)
			{
				this.speedControlStack.Remove(speedControl);
			}
		}

		// Token: 0x04001609 RID: 5641
		[Header("Settings")]
		[Range(0f, 1f)]
		public float DefaultWalkSpeed = 0.08f;

		// Token: 0x0400160A RID: 5642
		public float SpeedMultiplier = 1f;

		// Token: 0x0400160B RID: 5643
		[Header("References")]
		public NPCMovement Movement;

		// Token: 0x0400160C RID: 5644
		protected List<NPCSpeedController.SpeedControl> speedControlStack = new List<NPCSpeedController.SpeedControl>();

		// Token: 0x0400160D RID: 5645
		[Header("Debug")]
		public NPCSpeedController.SpeedControl ActiveSpeedControl;

		// Token: 0x020004A3 RID: 1187
		[Serializable]
		public class SpeedControl
		{
			// Token: 0x060018F6 RID: 6390 RVA: 0x0006E363 File Offset: 0x0006C563
			public SpeedControl(string id, int priority, float speed)
			{
				this.id = id;
				this.priority = priority;
				this.speed = speed;
			}

			// Token: 0x0400160E RID: 5646
			public string id;

			// Token: 0x0400160F RID: 5647
			public int priority;

			// Token: 0x04001610 RID: 5648
			public float speed;
		}
	}
}
