using System;
using ScheduleOne.NPCs;
using ScheduleOne.Police;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Tools
{
	// Token: 0x0200088A RID: 2186
	[RequireComponent(typeof(Rigidbody))]
	public class CombatNPCDetector : MonoBehaviour
	{
		// Token: 0x06003BB4 RID: 15284 RVA: 0x000FC9EC File Offset: 0x000FABEC
		private void Awake()
		{
			Rigidbody rigidbody = base.GetComponent<Rigidbody>();
			if (rigidbody == null)
			{
				rigidbody = base.gameObject.AddComponent<Rigidbody>();
			}
			rigidbody.isKinematic = true;
		}

		// Token: 0x06003BB5 RID: 15285 RVA: 0x000FCA1C File Offset: 0x000FAC1C
		private void FixedUpdate()
		{
			if (this.timeSinceLastContact < 0.1f)
			{
				this.contactTime += Time.fixedDeltaTime;
				if (this.contactTime >= this.ContactTimeForDetection)
				{
					this.contactTime = 0f;
					if (this.onDetected != null)
					{
						this.onDetected.Invoke();
					}
				}
			}
			else
			{
				this.contactTime = 0f;
			}
			this.timeSinceLastContact += Time.fixedDeltaTime;
		}

		// Token: 0x06003BB6 RID: 15286 RVA: 0x000FCA94 File Offset: 0x000FAC94
		private void OnTriggerStay(Collider other)
		{
			NPC componentInParent = other.GetComponentInParent<NPC>();
			if (componentInParent != null && (!this.DetectOnlyInCombat || componentInParent.behaviour.CombatBehaviour.Active))
			{
				this.timeSinceLastContact = 0f;
				return;
			}
			PoliceOfficer policeOfficer = componentInParent as PoliceOfficer;
			if (policeOfficer != null && (!this.DetectOnlyInCombat || policeOfficer.PursuitBehaviour.Active))
			{
				this.timeSinceLastContact = 0f;
				return;
			}
		}

		// Token: 0x04002AA3 RID: 10915
		public bool DetectOnlyInCombat;

		// Token: 0x04002AA4 RID: 10916
		public UnityEvent onDetected;

		// Token: 0x04002AA5 RID: 10917
		public float ContactTimeForDetection = 0.5f;

		// Token: 0x04002AA6 RID: 10918
		private float contactTime;

		// Token: 0x04002AA7 RID: 10919
		private float timeSinceLastContact = 100f;
	}
}
