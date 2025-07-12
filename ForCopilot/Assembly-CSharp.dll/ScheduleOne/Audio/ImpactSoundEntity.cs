using System;
using ScheduleOne.Combat;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Audio
{
	// Token: 0x020007EC RID: 2028
	[RequireComponent(typeof(Rigidbody))]
	public class ImpactSoundEntity : MonoBehaviour
	{
		// Token: 0x060036C1 RID: 14017 RVA: 0x000E68E4 File Offset: 0x000E4AE4
		public void Awake()
		{
			PhysicsDamageable component = base.GetComponent<PhysicsDamageable>();
			if (component != null)
			{
				PhysicsDamageable physicsDamageable = component;
				physicsDamageable.onImpacted = (Action<Impact>)Delegate.Combine(physicsDamageable.onImpacted, new Action<Impact>(this.OnImpacted));
			}
			this.rb = base.GetComponent<Rigidbody>();
		}

		// Token: 0x060036C2 RID: 14018 RVA: 0x000E6930 File Offset: 0x000E4B30
		private void OnImpacted(Impact impact)
		{
			if (Vector3.SqrMagnitude(impact.Hit.point - PlayerSingleton<PlayerCamera>.Instance.transform.position) > 1600f)
			{
				return;
			}
			if (Time.time - this.lastImpactTime < 0.25f)
			{
				return;
			}
			float impactForce = impact.ImpactForce;
			if (impactForce < 4f)
			{
				return;
			}
			Singleton<SFXManager>.Instance.PlayImpactSound(this.Material, impact.Hit.point, impactForce);
		}

		// Token: 0x060036C3 RID: 14019 RVA: 0x000E69AC File Offset: 0x000E4BAC
		private void OnCollisionEnter(Collision collision)
		{
			if (Time.time - this.lastImpactTime < 0.25f)
			{
				return;
			}
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			if (Vector3.SqrMagnitude(collision.contacts[0].point - PlayerSingleton<PlayerCamera>.Instance.transform.position) > 1600f)
			{
				return;
			}
			Rigidbody rigidbody = collision.rigidbody;
			float num = collision.relativeVelocity.magnitude;
			float num2 = this.rb.mass;
			if (rigidbody != null)
			{
				num2 = Mathf.Min(num2, rigidbody.mass);
			}
			num *= num2;
			if (num < 4f)
			{
				return;
			}
			this.lastImpactTime = Time.time;
			Singleton<SFXManager>.Instance.PlayImpactSound(this.Material, collision.contacts[0].point, num);
		}

		// Token: 0x04002701 RID: 9985
		public const float MIN_IMPACT_MOMENTUM = 4f;

		// Token: 0x04002702 RID: 9986
		public const float COOLDOWN = 0.25f;

		// Token: 0x04002703 RID: 9987
		public ImpactSoundEntity.EMaterial Material;

		// Token: 0x04002704 RID: 9988
		private float lastImpactTime;

		// Token: 0x04002705 RID: 9989
		private Rigidbody rb;

		// Token: 0x020007ED RID: 2029
		public enum EMaterial
		{
			// Token: 0x04002707 RID: 9991
			Wood,
			// Token: 0x04002708 RID: 9992
			HollowMetal,
			// Token: 0x04002709 RID: 9993
			Cardboard,
			// Token: 0x0400270A RID: 9994
			Glass,
			// Token: 0x0400270B RID: 9995
			Plastic,
			// Token: 0x0400270C RID: 9996
			Basketball,
			// Token: 0x0400270D RID: 9997
			SmallHollowMetal,
			// Token: 0x0400270E RID: 9998
			PlasticBag,
			// Token: 0x0400270F RID: 9999
			Punch,
			// Token: 0x04002710 RID: 10000
			BaseballBat
		}
	}
}
