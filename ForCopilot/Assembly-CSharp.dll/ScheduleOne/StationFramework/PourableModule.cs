using System;
using ScheduleOne.Audio;
using ScheduleOne.PlayerTasks;
using ScheduleOne.Tools;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.StationFramework
{
	// Token: 0x0200090B RID: 2315
	public class PourableModule : ItemModule
	{
		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x06003EA3 RID: 16035 RVA: 0x0010777B File Offset: 0x0010597B
		// (set) Token: 0x06003EA4 RID: 16036 RVA: 0x00107783 File Offset: 0x00105983
		public bool IsPouring { get; protected set; }

		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x06003EA5 RID: 16037 RVA: 0x0010778C File Offset: 0x0010598C
		// (set) Token: 0x06003EA6 RID: 16038 RVA: 0x00107794 File Offset: 0x00105994
		public float NormalizedPourRate { get; private set; }

		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x06003EA7 RID: 16039 RVA: 0x0010779D File Offset: 0x0010599D
		// (set) Token: 0x06003EA8 RID: 16040 RVA: 0x001077A5 File Offset: 0x001059A5
		public float LiquidLevel { get; protected set; } = 1f;

		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x06003EA9 RID: 16041 RVA: 0x001077AE File Offset: 0x001059AE
		public float NormalizedLiquidLevel
		{
			get
			{
				return this.LiquidLevel / this.LiquidCapacity_L;
			}
		}

		// Token: 0x06003EAA RID: 16042 RVA: 0x001077C0 File Offset: 0x001059C0
		protected virtual void Start()
		{
			this.particleMinSizes = new float[this.PourParticles.Length];
			this.particleMaxSizes = new float[this.PourParticles.Length];
			for (int i = 0; i < this.PourParticles.Length; i++)
			{
				this.particleMinSizes[i] = this.PourParticles[i].main.startSize.constantMin;
				this.particleMaxSizes[i] = this.PourParticles[i].main.startSize.constantMax;
				ParticleSystem.CollisionModule collision = this.PourParticles[i].collision;
				LayerMask layerMask = collision.collidesWith;
				layerMask |= 1 << LayerMask.NameToLayer("Task");
				collision.collidesWith = layerMask;
				collision.sendCollisionMessages = true;
				this.PourParticles[i].gameObject.AddComponent<ParticleCollisionDetector>().onCollision.AddListener(new UnityAction<GameObject>(this.ParticleCollision));
			}
			if (this.LiquidContainer != null)
			{
				this.SetLiquidLevel(this.DefaultLiquid_L);
			}
		}

		// Token: 0x06003EAB RID: 16043 RVA: 0x001078DC File Offset: 0x00105ADC
		public override void ActivateModule(StationItem item)
		{
			base.ActivateModule(item);
			if (this.DraggableConstraint != null)
			{
				this.DraggableConstraint.SetContainer(item.transform.parent);
			}
			if (this.Draggable != null)
			{
				this.Draggable.ClickableEnabled = true;
			}
		}

		// Token: 0x06003EAC RID: 16044 RVA: 0x0010792E File Offset: 0x00105B2E
		protected virtual void FixedUpdate()
		{
			if (!base.IsModuleActive)
			{
				return;
			}
			this.UpdatePouring();
			this.UpdatePourSound();
			if (this.timeSinceFillableHit > 0.25f)
			{
				this.activeFillable = null;
			}
			this.timeSinceFillableHit += Time.fixedDeltaTime;
		}

		// Token: 0x06003EAD RID: 16045 RVA: 0x0010796C File Offset: 0x00105B6C
		protected virtual void UpdatePouring()
		{
			float num = Vector3.Angle(Vector3.up, this.PourPoint.forward);
			this.IsPouring = (num > this.AngleFromUpToPour && this.CanPour());
			this.NormalizedPourRate = 0f;
			if (this.IsPouring && this.NormalizedLiquidLevel > 0f)
			{
				float num2 = 0.3f + 0.7f * (num - this.AngleFromUpToPour) / (180f - this.AngleFromUpToPour);
				this.NormalizedPourRate = num2;
				this.PourAmount(num2 * this.PourRate * Time.deltaTime);
				for (int i = 0; i < this.PourParticles.Length; i++)
				{
					ParticleSystem.MainModule main = this.PourParticles[i].main;
					float num3 = 1f;
					if (this.LiquidContainer != null)
					{
						num3 = Mathf.Clamp(this.LiquidContainer.CurrentLiquidLevel, 0.3f, 1f);
					}
					float num4 = this.ParticleMinMultiplier * num2 * this.particleMinSizes[i] * num3;
					float num5 = this.ParticleMaxMultiplier * num2 * this.particleMaxSizes[i] * num3;
					main.startSize = new ParticleSystem.MinMaxCurve(num4, num5);
					main.startColor = this.PourParticlesColor;
				}
				if (!this.PourParticles[0].isEmitting && this.NormalizedLiquidLevel > 0f)
				{
					for (int j = 0; j < this.PourParticles.Length; j++)
					{
						this.PourParticles[j].Play();
					}
				}
			}
			else if (this.PourParticles[0].isEmitting)
			{
				for (int k = 0; k < this.PourParticles.Length; k++)
				{
					this.PourParticles[k].Stop(false, 1);
				}
			}
			if (this.NormalizedLiquidLevel == 0f && this.PourParticles[0].isEmitting)
			{
				for (int l = 0; l < this.PourParticles.Length; l++)
				{
					this.PourParticles[l].Stop(false, 1);
				}
			}
		}

		// Token: 0x06003EAE RID: 16046 RVA: 0x00107B6C File Offset: 0x00105D6C
		private void UpdatePourSound()
		{
			if (this.PourSound == null)
			{
				return;
			}
			if (this.NormalizedPourRate > 0f)
			{
				this.PourSound.VolumeMultiplier = this.NormalizedPourRate;
				if (!this.PourSound.isPlaying)
				{
					this.PourSound.Play();
					return;
				}
			}
			else if (this.PourSound.isPlaying)
			{
				this.PourSound.Stop();
			}
		}

		// Token: 0x06003EAF RID: 16047 RVA: 0x00107BD7 File Offset: 0x00105DD7
		public virtual void ChangeLiquidLevel(float change)
		{
			this.LiquidLevel = Mathf.Clamp(this.LiquidLevel + change, 0f, this.LiquidCapacity_L);
			if (this.LiquidContainer != null)
			{
				this.LiquidContainer.SetLiquidLevel(this.NormalizedLiquidLevel, false);
			}
		}

		// Token: 0x06003EB0 RID: 16048 RVA: 0x00107C17 File Offset: 0x00105E17
		public virtual void SetLiquidLevel(float level)
		{
			this.LiquidLevel = Mathf.Clamp(level, 0f, this.LiquidCapacity_L);
			if (this.LiquidContainer != null)
			{
				this.LiquidContainer.SetLiquidLevel(this.NormalizedLiquidLevel, false);
			}
		}

		// Token: 0x06003EB1 RID: 16049 RVA: 0x00107C50 File Offset: 0x00105E50
		protected virtual void PourAmount(float amount)
		{
			Physics.RaycastAll(this.PourPoint.position, Vector3.down, 1f, 1 << LayerMask.NameToLayer("Task"));
			if (!this.OnlyEmptyOverFillable || (this.activeFillable != null && this.activeFillable.FillableEnabled))
			{
				this.ChangeLiquidLevel(-amount);
				if (this.activeFillable != null)
				{
					this.activeFillable.AddLiquid(this.LiquidType, amount, this.LiquidColor);
				}
			}
		}

		// Token: 0x06003EB2 RID: 16050 RVA: 0x00107CD8 File Offset: 0x00105ED8
		private void ParticleCollision(GameObject other)
		{
			Fillable componentInParent = other.GetComponentInParent<Fillable>();
			if (componentInParent != null && componentInParent.enabled)
			{
				this.timeSinceFillableHit = 0f;
				this.activeFillable = componentInParent;
			}
		}

		// Token: 0x06003EB3 RID: 16051 RVA: 0x000022C9 File Offset: 0x000004C9
		protected virtual bool CanPour()
		{
			return true;
		}

		// Token: 0x04002CB2 RID: 11442
		[Header("Settings")]
		public string LiquidType = "Liquid";

		// Token: 0x04002CB3 RID: 11443
		public float PourRate = 0.2f;

		// Token: 0x04002CB4 RID: 11444
		public float AngleFromUpToPour = 90f;

		// Token: 0x04002CB5 RID: 11445
		public bool OnlyEmptyOverFillable = true;

		// Token: 0x04002CB6 RID: 11446
		public float LiquidCapacity_L = 0.25f;

		// Token: 0x04002CB7 RID: 11447
		public Color LiquidColor;

		// Token: 0x04002CB8 RID: 11448
		public float DefaultLiquid_L = 1f;

		// Token: 0x04002CB9 RID: 11449
		[Header("References")]
		public ParticleSystem[] PourParticles;

		// Token: 0x04002CBA RID: 11450
		public Transform PourPoint;

		// Token: 0x04002CBB RID: 11451
		public LiquidContainer LiquidContainer;

		// Token: 0x04002CBC RID: 11452
		public Draggable Draggable;

		// Token: 0x04002CBD RID: 11453
		public DraggableConstraint DraggableConstraint;

		// Token: 0x04002CBE RID: 11454
		public AudioSourceController PourSound;

		// Token: 0x04002CBF RID: 11455
		[Header("Particles")]
		public Color PourParticlesColor;

		// Token: 0x04002CC0 RID: 11456
		public float ParticleMinMultiplier = 0.8f;

		// Token: 0x04002CC1 RID: 11457
		public float ParticleMaxMultiplier = 1.5f;

		// Token: 0x04002CC2 RID: 11458
		private float[] particleMinSizes;

		// Token: 0x04002CC3 RID: 11459
		private float[] particleMaxSizes;

		// Token: 0x04002CC4 RID: 11460
		private Fillable activeFillable;

		// Token: 0x04002CC5 RID: 11461
		private float timeSinceFillableHit = 10f;
	}
}
