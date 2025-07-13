using System;
using ScheduleOne.Audio;
using UnityEngine;

namespace ScheduleOne.Vehicles
{
	// Token: 0x0200081B RID: 2075
	public class Wheel : MonoBehaviour
	{
		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x06003876 RID: 14454 RVA: 0x000EDE2A File Offset: 0x000EC02A
		// (set) Token: 0x06003877 RID: 14455 RVA: 0x000EDE32 File Offset: 0x000EC032
		public bool isStatic { get; protected set; }

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x06003878 RID: 14456 RVA: 0x000EDE3B File Offset: 0x000EC03B
		// (set) Token: 0x06003879 RID: 14457 RVA: 0x000EDE43 File Offset: 0x000EC043
		public bool IsDrifting { get; protected set; }

		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x0600387A RID: 14458 RVA: 0x000EDE4C File Offset: 0x000EC04C
		public bool IsDrifting_Smoothed
		{
			get
			{
				return this.DriftTime > 0.2f;
			}
		}

		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x0600387B RID: 14459 RVA: 0x000EDE5B File Offset: 0x000EC05B
		// (set) Token: 0x0600387C RID: 14460 RVA: 0x000EDE63 File Offset: 0x000EC063
		public float DriftTime { get; protected set; }

		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x0600387D RID: 14461 RVA: 0x000EDE6C File Offset: 0x000EC06C
		// (set) Token: 0x0600387E RID: 14462 RVA: 0x000EDE74 File Offset: 0x000EC074
		public float DriftIntensity { get; protected set; }

		// Token: 0x0600387F RID: 14463 RVA: 0x000EDE80 File Offset: 0x000EC080
		protected virtual void Start()
		{
			this.vehicle = base.GetComponentInParent<LandVehicle>();
			this.wheelCollider.ConfigureVehicleSubsteps(5f, 12, 15);
			this.defaultForwardStiffness = this.wheelCollider.forwardFriction.stiffness;
			this.defaultSidewaysStiffness = this.wheelCollider.sidewaysFriction.stiffness;
			this.wheelTransform = base.transform;
		}

		// Token: 0x06003880 RID: 14464 RVA: 0x000EDEEB File Offset: 0x000EC0EB
		protected virtual void LateUpdate()
		{
			this.lastFramePosition = this.wheelTransform.position;
		}

		// Token: 0x06003881 RID: 14465 RVA: 0x000EDF00 File Offset: 0x000EC100
		private void FixedUpdate()
		{
			if (this.wheelCollider.enabled && !this.vehicle.Agent.KinematicMode && this.vehicle.DistanceToLocalCamera < 40f)
			{
				Vector3 position;
				Quaternion rotation;
				this.wheelCollider.GetWorldPose(ref position, ref rotation);
				this.wheelModel.transform.position = position;
				if (this.vehicle.localPlayerIsDriver)
				{
					this.modelContainer.transform.localRotation = Quaternion.identity;
					this.wheelModel.transform.rotation = rotation;
				}
				else
				{
					Vector3 vector = this.wheelTransform.position - this.lastFramePosition;
					float xAngle = this.wheelTransform.InverseTransformVector(vector).z / (6.2831855f * this.wheelCollider.radius) * 360f;
					this.wheelModel.transform.Rotate(xAngle, 0f, 0f, Space.Self);
					this.modelContainer.transform.localEulerAngles = new Vector3(0f, this.wheelCollider.steerAngle, 0f);
				}
				if (this.DriftParticlesEnabled)
				{
					this.DriftParticles.transform.position = this.wheelTransform.position - Vector3.up * this.wheelCollider.radius;
				}
			}
			if (!this.vehicle.localPlayerIsDriver)
			{
				this.DriftParticles.Stop();
				this.DriftAudioSource.Stop();
				return;
			}
			if (this.vehicle.isStatic)
			{
				return;
			}
			this.ApplyFriction();
			this.CheckDrifting();
			this.UpdateDriftEffects();
			this.UpdateDriftAudio();
		}

		// Token: 0x06003882 RID: 14466 RVA: 0x000EE0B0 File Offset: 0x000EC2B0
		private void CheckDrifting()
		{
			if (!this.wheelCollider.enabled)
			{
				this.IsDrifting = false;
				this.DriftTime = 0f;
				this.DriftIntensity = 0f;
				return;
			}
			if (Mathf.Abs(this.vehicle.speed_Kmh) < 8f)
			{
				this.IsDrifting = false;
				this.DriftTime = 0f;
				this.DriftIntensity = 0f;
				return;
			}
			this.wheelCollider.GetGroundHit(ref this.wheelData);
			this.IsDrifting = ((Mathf.Abs(this.wheelData.sidewaysSlip) > 0.2f || Mathf.Abs(this.wheelData.forwardSlip) > 0.8f) && Mathf.Abs(this.vehicle.speed_Kmh) > 2f);
			float a = Mathf.Clamp01(Mathf.Abs(this.wheelData.sidewaysSlip));
			float b = Mathf.Clamp01(Mathf.Abs(this.wheelData.forwardSlip));
			this.DriftIntensity = Mathf.Max(a, b);
			if (this.IsDrifting)
			{
				this.DriftTime += Time.fixedDeltaTime;
			}
			else
			{
				this.DriftTime = 0f;
			}
			if (this.DEBUG_MODE)
			{
				Debug.Log("Sideways slip: " + this.wheelData.sidewaysSlip.ToString() + "\nForward slip: " + this.wheelData.forwardSlip.ToString());
				Debug.Log("Drifting: " + this.IsDrifting.ToString());
			}
		}

		// Token: 0x06003883 RID: 14467 RVA: 0x000EE23C File Offset: 0x000EC43C
		private void UpdateDriftEffects()
		{
			if (this.IsDrifting_Smoothed && this.DriftParticlesEnabled)
			{
				if (!this.DriftParticles.isPlaying)
				{
					this.DriftParticles.Play();
					return;
				}
			}
			else if (this.DriftParticles.isPlaying)
			{
				this.DriftParticles.Stop();
			}
		}

		// Token: 0x06003884 RID: 14468 RVA: 0x000EE28C File Offset: 0x000EC48C
		private void UpdateDriftAudio()
		{
			if (!this.DriftAudioEnabled)
			{
				return;
			}
			if (this.IsDrifting_Smoothed && this.DriftIntensity > 0.2f && !this.DriftAudioSource.isPlaying)
			{
				this.DriftAudioSource.Play();
			}
			if (this.DriftAudioSource.isPlaying)
			{
				float volumeMultiplier = Mathf.Clamp01(Mathf.InverseLerp(0.2f, 1f, this.DriftIntensity));
				this.DriftAudioSource.VolumeMultiplier = volumeMultiplier;
			}
		}

		// Token: 0x06003885 RID: 14469 RVA: 0x000EE304 File Offset: 0x000EC504
		private void ApplyFriction()
		{
			this.forwardCurve = this.wheelCollider.forwardFriction;
			this.forwardCurve.stiffness = this.defaultForwardStiffness * ((this.vehicle.handbrakeApplied && this.vehicle.isOccupied) ? this.ForwardStiffnessMultiplier_Handbrake : 1f);
			this.wheelCollider.forwardFriction = this.forwardCurve;
			this.sidewaysCurve = this.wheelCollider.sidewaysFriction;
			this.sidewaysCurve.stiffness = this.defaultSidewaysStiffness * ((this.vehicle.handbrakeApplied && this.vehicle.isOccupied) ? this.SidewayStiffnessMultiplier_Handbrake : 1f);
			this.wheelCollider.sidewaysFriction = this.sidewaysCurve;
		}

		// Token: 0x06003886 RID: 14470 RVA: 0x000EE3C8 File Offset: 0x000EC5C8
		public virtual void SetIsStatic(bool s)
		{
			this.isStatic = s;
			if (this.isStatic)
			{
				this.wheelCollider.enabled = false;
				this.wheelModel.transform.localPosition = new Vector3(this.wheelModel.transform.localPosition.x, -this.wheelCollider.suspensionDistance * this.wheelCollider.suspensionSpring.targetPosition, this.wheelModel.transform.localPosition.z);
				this.staticCollider.enabled = true;
				this.GroundWheelModel();
				return;
			}
			this.wheelCollider.enabled = true;
			this.staticCollider.enabled = false;
		}

		// Token: 0x06003887 RID: 14471 RVA: 0x000EE478 File Offset: 0x000EC678
		private void GroundWheelModel()
		{
			this.wheelModel.localPosition = Vector3.zero;
		}

		// Token: 0x06003888 RID: 14472 RVA: 0x000EE498 File Offset: 0x000EC698
		public bool IsWheelGrounded()
		{
			WheelHit wheelHit;
			return this.wheelCollider.GetGroundHit(ref wheelHit);
		}

		// Token: 0x04002837 RID: 10295
		public const float SIDEWAY_SLIP_THRESHOLD = 0.2f;

		// Token: 0x04002838 RID: 10296
		public const float FORWARD_SLIP_THRESHOLD = 0.8f;

		// Token: 0x04002839 RID: 10297
		public const float DRIFT_AUDIO_THRESHOLD = 0.2f;

		// Token: 0x0400283A RID: 10298
		public const float MIN_SPEED_FOR_DRIFT = 8f;

		// Token: 0x0400283B RID: 10299
		public const float WHEEL_ANIMATION_DISTANCE = 40f;

		// Token: 0x0400283C RID: 10300
		public bool DEBUG_MODE;

		// Token: 0x0400283D RID: 10301
		[Header("References")]
		public Transform wheelModel;

		// Token: 0x0400283E RID: 10302
		public Transform modelContainer;

		// Token: 0x0400283F RID: 10303
		public WheelCollider wheelCollider;

		// Token: 0x04002840 RID: 10304
		public Transform axleConnectionPoint;

		// Token: 0x04002841 RID: 10305
		public Collider staticCollider;

		// Token: 0x04002842 RID: 10306
		public ParticleSystem DriftParticles;

		// Token: 0x04002843 RID: 10307
		[Header("Settings")]
		public bool DriftParticlesEnabled = true;

		// Token: 0x04002844 RID: 10308
		public float ForwardStiffnessMultiplier_Handbrake = 0.5f;

		// Token: 0x04002845 RID: 10309
		public float SidewayStiffnessMultiplier_Handbrake = 0.5f;

		// Token: 0x04002846 RID: 10310
		[Header("Drift Audio")]
		public bool DriftAudioEnabled;

		// Token: 0x04002847 RID: 10311
		public AudioSourceController DriftAudioSource;

		// Token: 0x04002848 RID: 10312
		private float defaultForwardStiffness = 1f;

		// Token: 0x04002849 RID: 10313
		private float defaultSidewaysStiffness = 1f;

		// Token: 0x0400284E RID: 10318
		private LandVehicle vehicle;

		// Token: 0x0400284F RID: 10319
		private Vector3 lastFramePosition = Vector3.zero;

		// Token: 0x04002850 RID: 10320
		private WheelHit wheelData;

		// Token: 0x04002851 RID: 10321
		private WheelFrictionCurve forwardCurve;

		// Token: 0x04002852 RID: 10322
		private WheelFrictionCurve sidewaysCurve;

		// Token: 0x04002853 RID: 10323
		private Transform wheelTransform;
	}
}
