using System;
using ScheduleOne.Audio;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Vehicles.Sound
{
	// Token: 0x0200081C RID: 2076
	public class VehicleSound : MonoBehaviour
	{
		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x0600388A RID: 14474 RVA: 0x000EE505 File Offset: 0x000EC705
		// (set) Token: 0x0600388B RID: 14475 RVA: 0x000EE50D File Offset: 0x000EC70D
		public LandVehicle Vehicle { get; private set; }

		// Token: 0x0600388C RID: 14476 RVA: 0x000EE518 File Offset: 0x000EC718
		protected virtual void Awake()
		{
			this.Vehicle = base.GetComponentInParent<LandVehicle>();
			if (this.Vehicle == null)
			{
				return;
			}
			this.Vehicle.onHandbrakeApplied.AddListener(new UnityAction(this.HandbrakeApplied));
			this.Vehicle.onVehicleStart.AddListener(new UnityAction(this.EngineStart));
			this.EngineIdleSource.VolumeMultiplier = 0f;
			this.EngineLoopSource.VolumeMultiplier = 0f;
			this.Vehicle.onCollision.AddListener(new UnityAction<Collision>(this.OnCollision));
		}

		// Token: 0x0600388D RID: 14477 RVA: 0x000EE5B4 File Offset: 0x000EC7B4
		protected virtual void FixedUpdate()
		{
			this.UpdateIdle();
		}

		// Token: 0x0600388E RID: 14478 RVA: 0x000EE5BC File Offset: 0x000EC7BC
		private void UpdateIdle()
		{
			if (this.Vehicle.isOccupied)
			{
				this.currentIdleVolume = Mathf.MoveTowards(this.currentIdleVolume, 1f, Time.fixedDeltaTime * 2f);
				float time = Mathf.Abs(this.Vehicle.VelocityCalculator.Velocity.magnitude * 3.6f / this.Vehicle.TopSpeed);
				this.EngineLoopSource.AudioSource.pitch = this.EngineLoopPitchCurve.Evaluate(time) * this.EngineLoopPitchMultiplier;
				this.EngineLoopSource.VolumeMultiplier = this.EngineLoopVolumeCurve.Evaluate(time) * this.VolumeMultiplier;
				if (!this.EngineLoopSource.AudioSource.isPlaying)
				{
					this.EngineLoopSource.Play();
				}
			}
			else
			{
				this.currentIdleVolume = Mathf.MoveTowards(this.currentIdleVolume, 0f, Time.fixedDeltaTime * 2f);
				if (this.EngineLoopSource.AudioSource.isPlaying)
				{
					this.EngineLoopSource.Stop();
				}
			}
			this.EngineIdleSource.VolumeMultiplier = this.currentIdleVolume * this.VolumeMultiplier;
			if (this.currentIdleVolume > 0f)
			{
				if (!this.EngineIdleSource.AudioSource.isPlaying)
				{
					this.EngineIdleSource.Play();
					return;
				}
			}
			else
			{
				this.EngineIdleSource.Stop();
			}
		}

		// Token: 0x0600388F RID: 14479 RVA: 0x000EE713 File Offset: 0x000EC913
		protected void HandbrakeApplied()
		{
			this.HandbrakeSource.VolumeMultiplier = this.VolumeMultiplier;
			this.HandbrakeSource.Play();
		}

		// Token: 0x06003890 RID: 14480 RVA: 0x000EE731 File Offset: 0x000EC931
		protected void EngineStart()
		{
			this.EngineStartSource.VolumeMultiplier = this.VolumeMultiplier;
			this.EngineStartSource.Play();
		}

		// Token: 0x06003891 RID: 14481 RVA: 0x000EE74F File Offset: 0x000EC94F
		public void Honk()
		{
			this.HonkSource.Play();
		}

		// Token: 0x06003892 RID: 14482 RVA: 0x000EE75C File Offset: 0x000EC95C
		private void OnCollision(Collision collision)
		{
			float num = collision.relativeVelocity.magnitude * this.Vehicle.Rb.mass;
			if (collision.gameObject.layer == LayerMask.NameToLayer("NPC"))
			{
				num *= 0.2f;
			}
			if (num < this.MinCollisionMomentum)
			{
				return;
			}
			if (Time.time - this.lastCollisionTime < 0.5f && num < this.lastCollisionMomentum)
			{
				return;
			}
			float t = Mathf.InverseLerp(this.MinCollisionMomentum, this.MaxCollisionMomentum, num);
			this.ImpactSound.VolumeMultiplier = Mathf.Lerp(this.MinCollisionVolume, this.MaxCollisionVolume, t);
			this.ImpactSound.PitchMultiplier = Mathf.Lerp(this.MaxCollisionPitch, this.MinCollisionPitch, t);
			this.ImpactSound.transform.position = collision.contacts[0].point;
			this.ImpactSound.Play();
			this.lastCollisionTime = Time.time;
			this.lastCollisionMomentum = num;
		}

		// Token: 0x04002854 RID: 10324
		public const float COLLISION_SOUND_COOLDOWN = 0.5f;

		// Token: 0x04002855 RID: 10325
		public float VolumeMultiplier = 1f;

		// Token: 0x04002856 RID: 10326
		[Header("References")]
		public AudioSourceController EngineStartSource;

		// Token: 0x04002857 RID: 10327
		public AudioSourceController EngineIdleSource;

		// Token: 0x04002858 RID: 10328
		public AudioSourceController EngineLoopSource;

		// Token: 0x04002859 RID: 10329
		public AudioSourceController HandbrakeSource;

		// Token: 0x0400285A RID: 10330
		public AudioSourceController HonkSource;

		// Token: 0x0400285B RID: 10331
		public AudioSourceController ImpactSound;

		// Token: 0x0400285C RID: 10332
		[Header("Impact Sounds")]
		public float MinCollisionMomentum = 3000f;

		// Token: 0x0400285D RID: 10333
		public float MaxCollisionMomentum = 20000f;

		// Token: 0x0400285E RID: 10334
		public float MinCollisionVolume = 0.2f;

		// Token: 0x0400285F RID: 10335
		public float MaxCollisionVolume = 1f;

		// Token: 0x04002860 RID: 10336
		public float MinCollisionPitch = 0.6f;

		// Token: 0x04002861 RID: 10337
		public float MaxCollisionPitch = 1.1f;

		// Token: 0x04002862 RID: 10338
		[Header("Engine Loop Settings")]
		public AnimationCurve EngineLoopPitchCurve;

		// Token: 0x04002863 RID: 10339
		public float EngineLoopPitchMultiplier = 1f;

		// Token: 0x04002864 RID: 10340
		public AnimationCurve EngineLoopVolumeCurve;

		// Token: 0x04002866 RID: 10342
		private float currentIdleVolume;

		// Token: 0x04002867 RID: 10343
		private float lastCollisionTime;

		// Token: 0x04002868 RID: 10344
		private float lastCollisionMomentum;
	}
}
