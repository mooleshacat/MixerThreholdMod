using System;
using ScheduleOne.PlayerTasks;
using UnityEngine;

namespace ScheduleOne.Growing
{
	// Token: 0x020008BB RID: 2235
	public class VialCap : Clickable
	{
		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x06003C64 RID: 15460 RVA: 0x000FE841 File Offset: 0x000FCA41
		// (set) Token: 0x06003C65 RID: 15461 RVA: 0x000FE849 File Offset: 0x000FCA49
		public bool Removed { get; protected set; }

		// Token: 0x06003C66 RID: 15462 RVA: 0x000FE852 File Offset: 0x000FCA52
		public override void StartClick(RaycastHit hit)
		{
			base.StartClick(hit);
			this.Pop();
		}

		// Token: 0x06003C67 RID: 15463 RVA: 0x000FE864 File Offset: 0x000FCA64
		private void Pop()
		{
			this.RigidBody = base.gameObject.AddComponent<Rigidbody>();
			this.Removed = true;
			this.Collider.enabled = false;
			this.RigidBody.isKinematic = false;
			this.RigidBody.useGravity = true;
			base.transform.SetParent(null);
			this.RigidBody.AddRelativeForce(Vector3.forward * 1.5f, 2);
			this.RigidBody.AddRelativeForce(Vector3.up * 0.5f, 2);
			this.RigidBody.AddTorque(Vector3.up * 1.5f, 2);
			UnityEngine.Object.Destroy(base.gameObject, 3f);
			base.enabled = false;
		}

		// Token: 0x04002B34 RID: 11060
		public Collider Collider;

		// Token: 0x04002B35 RID: 11061
		private Rigidbody RigidBody;
	}
}
