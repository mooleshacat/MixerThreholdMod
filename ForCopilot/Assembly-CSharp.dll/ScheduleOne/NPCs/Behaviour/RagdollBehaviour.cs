using System;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000566 RID: 1382
	public class RagdollBehaviour : Behaviour
	{
		// Token: 0x06002105 RID: 8453 RVA: 0x000885AC File Offset: 0x000867AC
		private void Start()
		{
			base.InvokeRepeating("InfrequentUpdate", 0f, 0.1f);
		}

		// Token: 0x06002106 RID: 8454 RVA: 0x000885C4 File Offset: 0x000867C4
		private void InfrequentUpdate()
		{
			if (this.Seizure)
			{
				Rigidbody[] ragdollRBs = base.Npc.Avatar.RagdollRBs;
				for (int i = 0; i < ragdollRBs.Length; i++)
				{
					ragdollRBs[i].AddForce(UnityEngine.Random.insideUnitSphere * this.SeizureForce, 5);
				}
			}
		}

		// Token: 0x06002108 RID: 8456 RVA: 0x00088624 File Offset: 0x00086824
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.RagdollBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.RagdollBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06002109 RID: 8457 RVA: 0x0008863D File Offset: 0x0008683D
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.RagdollBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.RagdollBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600210A RID: 8458 RVA: 0x00088656 File Offset: 0x00086856
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600210B RID: 8459 RVA: 0x00088664 File Offset: 0x00086864
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400196B RID: 6507
		public bool Seizure;

		// Token: 0x0400196C RID: 6508
		public float SeizureForce = 1f;

		// Token: 0x0400196D RID: 6509
		private bool dll_Excuted;

		// Token: 0x0400196E RID: 6510
		private bool dll_Excuted;
	}
}
