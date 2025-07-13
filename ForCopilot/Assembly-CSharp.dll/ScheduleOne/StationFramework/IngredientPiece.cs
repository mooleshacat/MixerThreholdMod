using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.PlayerTasks;
using UnityEngine;

namespace ScheduleOne.StationFramework
{
	// Token: 0x02000903 RID: 2307
	[RequireComponent(typeof(Draggable))]
	public class IngredientPiece : MonoBehaviour
	{
		// Token: 0x170008AF RID: 2223
		// (get) Token: 0x06003E78 RID: 15992 RVA: 0x00107070 File Offset: 0x00105270
		// (set) Token: 0x06003E79 RID: 15993 RVA: 0x00107078 File Offset: 0x00105278
		public float CurrentDissolveAmount { get; private set; }

		// Token: 0x06003E7A RID: 15994 RVA: 0x00107081 File Offset: 0x00105281
		private void Start()
		{
			base.InvokeRepeating("CheckLiquid", 0f, 0.05f);
			this.draggable = base.GetComponent<Draggable>();
			this.defaultDrag = this.draggable.NormalRBDrag;
		}

		// Token: 0x06003E7B RID: 15995 RVA: 0x001070B5 File Offset: 0x001052B5
		private void Update()
		{
			if (this.DisableInteractionInLiquid && this.CurrentLiquidContainer != null)
			{
				this.draggable.ClickableEnabled = false;
			}
		}

		// Token: 0x06003E7C RID: 15996 RVA: 0x001070D9 File Offset: 0x001052D9
		private void FixedUpdate()
		{
			this.UpdateDrag();
		}

		// Token: 0x06003E7D RID: 15997 RVA: 0x001070E4 File Offset: 0x001052E4
		private void UpdateDrag()
		{
			if (this.CurrentLiquidContainer != null)
			{
				Vector3 a = -this.draggable.Rb.velocity.normalized;
				float d = this.CurrentLiquidContainer.Viscosity * this.draggable.Rb.velocity.magnitude * 100f * this.LiquidFrictionMultiplier;
				this.draggable.Rb.AddForce(a * d, 5);
			}
		}

		// Token: 0x06003E7E RID: 15998 RVA: 0x00107168 File Offset: 0x00105368
		private void CheckLiquid()
		{
			this.CurrentLiquidContainer = null;
			if (!this.DetectLiquid)
			{
				return;
			}
			Collider[] array = Physics.OverlapSphere(base.transform.position, 0.001f, 1 << LayerMask.NameToLayer("Task"), 2);
			for (int i = 0; i < array.Length; i++)
			{
				LiquidVolumeCollider liquidVolumeCollider;
				if (array[i].isTrigger && array[i].TryGetComponent<LiquidVolumeCollider>(out liquidVolumeCollider))
				{
					this.CurrentLiquidContainer = liquidVolumeCollider.LiquidContainer;
					return;
				}
			}
		}

		// Token: 0x06003E7F RID: 15999 RVA: 0x001071DC File Offset: 0x001053DC
		public void DissolveAmount(float amount, bool showParticles = true)
		{
			if (this.CurrentDissolveAmount >= 1f)
			{
				return;
			}
			this.CurrentDissolveAmount = Mathf.Clamp01(this.CurrentDissolveAmount + amount);
			this.ModelContainer.transform.localScale = Vector3.one * (1f - this.CurrentDissolveAmount);
			if (showParticles)
			{
				if (!this.DissolveParticles.isPlaying)
				{
					this.DissolveParticles.Play();
				}
				if (this.dissolveParticleRoutine != null)
				{
					base.StopCoroutine(this.dissolveParticleRoutine);
				}
				this.dissolveParticleRoutine = base.StartCoroutine(this.<DissolveAmount>g__DissolveParticlesRoutine|19_0());
			}
		}

		// Token: 0x06003E81 RID: 16001 RVA: 0x00107292 File Offset: 0x00105492
		[CompilerGenerated]
		private IEnumerator <DissolveAmount>g__DissolveParticlesRoutine|19_0()
		{
			yield return new WaitForSeconds(0.2f);
			this.DissolveParticles.Stop();
			this.dissolveParticleRoutine = null;
			yield break;
		}

		// Token: 0x04002C89 RID: 11401
		public const float LIQUID_FRICTION = 100f;

		// Token: 0x04002C8B RID: 11403
		public LiquidContainer CurrentLiquidContainer;

		// Token: 0x04002C8C RID: 11404
		[Header("References")]
		public Transform ModelContainer;

		// Token: 0x04002C8D RID: 11405
		public ParticleSystem DissolveParticles;

		// Token: 0x04002C8E RID: 11406
		[Header("Settings")]
		public bool DetectLiquid = true;

		// Token: 0x04002C8F RID: 11407
		public bool DisableInteractionInLiquid = true;

		// Token: 0x04002C90 RID: 11408
		[Range(0f, 2f)]
		public float LiquidFrictionMultiplier = 1f;

		// Token: 0x04002C91 RID: 11409
		private Draggable draggable;

		// Token: 0x04002C92 RID: 11410
		private float defaultDrag;

		// Token: 0x04002C93 RID: 11411
		private Coroutine dissolveParticleRoutine;
	}
}
