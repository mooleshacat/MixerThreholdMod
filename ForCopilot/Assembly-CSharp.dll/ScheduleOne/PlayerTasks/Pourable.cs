using System;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Trash;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ScheduleOne.PlayerTasks
{
	// Token: 0x02000355 RID: 853
	[RequireComponent(typeof(Accelerometer))]
	public class Pourable : Draggable
	{
		// Token: 0x17000389 RID: 905
		// (get) Token: 0x0600131B RID: 4891 RVA: 0x00052D59 File Offset: 0x00050F59
		// (set) Token: 0x0600131C RID: 4892 RVA: 0x00052D61 File Offset: 0x00050F61
		public bool IsPouring { get; protected set; }

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x0600131D RID: 4893 RVA: 0x00052D6A File Offset: 0x00050F6A
		// (set) Token: 0x0600131E RID: 4894 RVA: 0x00052D72 File Offset: 0x00050F72
		public float NormalizedPourRate { get; private set; }

		// Token: 0x0600131F RID: 4895 RVA: 0x00052D7C File Offset: 0x00050F7C
		protected virtual void Start()
		{
			if (this.autoSetCurrentQuantity)
			{
				this.currentQuantity = this.StartQuantity;
			}
			this.accelerometer = base.GetComponent<AverageAcceleration>();
			if (this.accelerometer == null)
			{
				this.accelerometer = base.gameObject.AddComponent<AverageAcceleration>();
			}
			this.particleMinSizes = new float[this.PourParticles.Length];
			this.particleMaxSizes = new float[this.PourParticles.Length];
			for (int i = 0; i < this.PourParticles.Length; i++)
			{
				this.particleMinSizes[i] = this.PourParticles[i].main.startSize.constantMin;
				this.particleMaxSizes[i] = this.PourParticles[i].main.startSize.constantMax;
			}
		}

		// Token: 0x06001320 RID: 4896 RVA: 0x00052E4B File Offset: 0x0005104B
		protected override void Update()
		{
			base.Update();
		}

		// Token: 0x06001321 RID: 4897 RVA: 0x00052E53 File Offset: 0x00051053
		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			this.UpdatePouring();
		}

		// Token: 0x06001322 RID: 4898 RVA: 0x00052E64 File Offset: 0x00051064
		protected virtual void UpdatePouring()
		{
			float num = Vector3.Angle(Vector3.up, this.PourPoint.forward);
			this.IsPouring = (num > this.AngleFromUpToPour && this.CanPour());
			this.NormalizedPourRate = 0f;
			if (this.IsPouring && this.currentQuantity > 0f)
			{
				float num2 = (0.3f + 0.7f * (num - this.AngleFromUpToPour) / (180f - this.AngleFromUpToPour)) * this.GetShakeBoost();
				this.NormalizedPourRate = num2;
				if (this.PourLoop != null)
				{
					this.PourLoop.VolumeMultiplier = num2 - 0.3f;
					if (!this.PourLoop.isPlaying)
					{
						this.PourLoop.Play();
					}
				}
				this.PourAmount(this.PourRate_L * num2 * Time.deltaTime);
				for (int i = 0; i < this.PourParticles.Length; i++)
				{
					ParticleSystem.MainModule main = this.PourParticles[i].main;
					float num3 = this.ParticleMinMultiplier * num2 * this.particleMinSizes[i];
					float num4 = this.ParticleMaxMultiplier * num2 * this.particleMaxSizes[i];
					main.startSize = new ParticleSystem.MinMaxCurve(num3, num4);
				}
				if (!this.PourParticles[0].isEmitting && this.currentQuantity > 0f)
				{
					for (int j = 0; j < this.PourParticles.Length; j++)
					{
						this.PourParticles[j].Play();
					}
				}
			}
			else
			{
				if (this.PourLoop != null && this.PourLoop.isPlaying)
				{
					this.PourLoop.Stop();
				}
				if (this.PourParticles[0].isEmitting)
				{
					for (int k = 0; k < this.PourParticles.Length; k++)
					{
						this.PourParticles[k].Stop(false, 1);
					}
				}
			}
			if (this.currentQuantity == 0f && this.PourParticles[0].isEmitting)
			{
				for (int l = 0; l < this.PourParticles.Length; l++)
				{
					this.PourParticles[l].Stop(false, 1);
				}
			}
		}

		// Token: 0x06001323 RID: 4899 RVA: 0x00053080 File Offset: 0x00051280
		private float GetShakeBoost()
		{
			return Mathf.Lerp(1f, this.ShakeBoostRate, Mathf.Clamp(this.accelerometer.Acceleration.y / 0.75f, 0f, 1f));
		}

		// Token: 0x06001324 RID: 4900 RVA: 0x000530B8 File Offset: 0x000512B8
		protected virtual void PourAmount(float amount)
		{
			if (!this.Unlimited)
			{
				this.currentQuantity = Mathf.Clamp(this.currentQuantity - amount, 0f, this.StartQuantity);
			}
			if (this.AffectsCoverage && this.IsPourPointOverPot())
			{
				this.TargetPot.SoilCover.QueuePour(this.PourPoint.position + this.PourPoint.forward * 0.05f);
			}
			if (!this.hasPoured)
			{
				if (this.onInitialPour != null)
				{
					this.onInitialPour();
				}
				this.hasPoured = true;
			}
		}

		// Token: 0x06001325 RID: 4901 RVA: 0x00053154 File Offset: 0x00051354
		protected bool IsPourPointOverPot()
		{
			Vector3 position = this.PourPoint.position;
			position.y = this.TargetPot.transform.position.y;
			return Vector3.Distance(position, this.TargetPot.transform.position) < this.TargetPot.PotRadius;
		}

		// Token: 0x06001326 RID: 4902 RVA: 0x000022C9 File Offset: 0x000004C9
		protected virtual bool CanPour()
		{
			return true;
		}

		// Token: 0x04001241 RID: 4673
		public Action onInitialPour;

		// Token: 0x04001242 RID: 4674
		[Header("Pourable settings")]
		public bool Unlimited;

		// Token: 0x04001243 RID: 4675
		public float StartQuantity = 10f;

		// Token: 0x04001244 RID: 4676
		public float PourRate_L = 0.25f;

		// Token: 0x04001245 RID: 4677
		public float AngleFromUpToPour = 90f;

		// Token: 0x04001246 RID: 4678
		[Tooltip("Multiplier for pour rate when pourable is shaken up and down")]
		public float ShakeBoostRate = 1.35f;

		// Token: 0x04001247 RID: 4679
		public bool AffectsCoverage;

		// Token: 0x04001248 RID: 4680
		[Header("Particles")]
		public float ParticleMinMultiplier = 0.8f;

		// Token: 0x04001249 RID: 4681
		public float ParticleMaxMultiplier = 1.5f;

		// Token: 0x0400124A RID: 4682
		[Header("Pourable References")]
		public ParticleSystem[] PourParticles;

		// Token: 0x0400124B RID: 4683
		public Transform PourPoint;

		// Token: 0x0400124C RID: 4684
		public AudioSourceController PourLoop;

		// Token: 0x0400124D RID: 4685
		[Header("Trash")]
		public TrashItem TrashItem;

		// Token: 0x0400124E RID: 4686
		[HideInInspector]
		public Pot TargetPot;

		// Token: 0x04001250 RID: 4688
		public float currentQuantity;

		// Token: 0x04001251 RID: 4689
		protected bool hasPoured;

		// Token: 0x04001252 RID: 4690
		protected bool autoSetCurrentQuantity = true;

		// Token: 0x04001253 RID: 4691
		private float[] particleMinSizes;

		// Token: 0x04001254 RID: 4692
		private float[] particleMaxSizes;

		// Token: 0x04001255 RID: 4693
		private AverageAcceleration accelerometer;
	}
}
