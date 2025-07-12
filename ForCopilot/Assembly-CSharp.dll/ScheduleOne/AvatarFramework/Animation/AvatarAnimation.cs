using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Skating;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.AvatarFramework.Animation
{
	// Token: 0x020009D0 RID: 2512
	public class AvatarAnimation : MonoBehaviour
	{
		// Token: 0x17000986 RID: 2438
		// (get) Token: 0x060043DD RID: 17373 RVA: 0x0011CB14 File Offset: 0x0011AD14
		// (set) Token: 0x060043DE RID: 17374 RVA: 0x0011CB1C File Offset: 0x0011AD1C
		public bool IsCrouched { get; protected set; }

		// Token: 0x17000987 RID: 2439
		// (get) Token: 0x060043DF RID: 17375 RVA: 0x0011CB25 File Offset: 0x0011AD25
		public bool IsSeated
		{
			get
			{
				return this.CurrentSeat != null;
			}
		}

		// Token: 0x17000988 RID: 2440
		// (get) Token: 0x060043E0 RID: 17376 RVA: 0x0011CB33 File Offset: 0x0011AD33
		// (set) Token: 0x060043E1 RID: 17377 RVA: 0x0011CB3B File Offset: 0x0011AD3B
		public float TimeSinceSitEnd { get; protected set; } = 1000f;

		// Token: 0x17000989 RID: 2441
		// (get) Token: 0x060043E2 RID: 17378 RVA: 0x0011CB44 File Offset: 0x0011AD44
		// (set) Token: 0x060043E3 RID: 17379 RVA: 0x0011CB4C File Offset: 0x0011AD4C
		public AvatarSeat CurrentSeat { get; protected set; }

		// Token: 0x1700098A RID: 2442
		// (get) Token: 0x060043E4 RID: 17380 RVA: 0x0011CB55 File Offset: 0x0011AD55
		// (set) Token: 0x060043E5 RID: 17381 RVA: 0x0011CB5D File Offset: 0x0011AD5D
		public bool StandUpAnimationPlaying { get; protected set; }

		// Token: 0x1700098B RID: 2443
		// (get) Token: 0x060043E6 RID: 17382 RVA: 0x0011CB66 File Offset: 0x0011AD66
		// (set) Token: 0x060043E7 RID: 17383 RVA: 0x0011CB6E File Offset: 0x0011AD6E
		public bool IsAvatarCulled { get; private set; }

		// Token: 0x060043E8 RID: 17384 RVA: 0x0011CB78 File Offset: 0x0011AD78
		protected virtual void Awake()
		{
			this.initialCullingMode = this.animator.cullingMode;
			this.animator.cullingMode = 0;
			this.avatar = base.GetComponent<Avatar>();
			this.avatar.onRagdollChange.AddListener(new UnityAction<bool, bool, bool>(this.RagdollChange));
			this.standUpFromBackBoneTransforms = new BoneTransform[this.Bones.Length];
			this.standUpFromFrontBoneTransforms = new BoneTransform[this.Bones.Length];
			this.ragdollBoneTransforms = new BoneTransform[this.Bones.Length];
			this.standingBoneTransforms = new BoneTransform[this.Bones.Length];
			for (int i = 0; i < this.Bones.Length; i++)
			{
				this.standUpFromBackBoneTransforms[i] = new BoneTransform();
				this.standUpFromFrontBoneTransforms[i] = new BoneTransform();
				this.ragdollBoneTransforms[i] = new BoneTransform();
				this.standingBoneTransforms[i] = new BoneTransform();
			}
			this.PopulateBoneTransforms(this.standingBoneTransforms);
			base.InvokeRepeating("InfrequentUpdate", UnityEngine.Random.Range(0f, 0.5f), 0.1f);
		}

		// Token: 0x060043E9 RID: 17385 RVA: 0x0011CC88 File Offset: 0x0011AE88
		protected virtual void Start()
		{
			this.PopulateAnimationStartBoneTransforms(this.StandUpFromFrontClipName, this.standUpFromFrontBoneTransforms);
			this.PopulateAnimationStartBoneTransforms(this.StandUpFromBackClipName, this.standUpFromBackBoneTransforms);
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
				TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
				instance2.onMinutePass = (Action)Delegate.Combine(instance2.onMinutePass, new Action(this.MinPass));
			}
			Player componentInParent = base.GetComponentInParent<Player>();
			if (componentInParent != null)
			{
				Player player = componentInParent;
				player.onSkateboardMounted = (Action<Skateboard>)Delegate.Combine(player.onSkateboardMounted, new Action<Skateboard>(this.SkateboardMounted));
				Player player2 = componentInParent;
				player2.onSkateboardDismounted = (Action)Delegate.Combine(player2.onSkateboardDismounted, new Action(this.SkateboardDismounted));
			}
			this.framesActive = 0;
		}

		// Token: 0x060043EA RID: 17386 RVA: 0x0011CD67 File Offset: 0x0011AF67
		private void OnDestroy()
		{
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			}
		}

		// Token: 0x060043EB RID: 17387 RVA: 0x0011CD96 File Offset: 0x0011AF96
		private void OnEnable()
		{
			this.framesActive = 0;
		}

		// Token: 0x060043EC RID: 17388 RVA: 0x0011CDA0 File Offset: 0x0011AFA0
		private void Update()
		{
			if (this.IsSeated)
			{
				this.TimeSinceSitEnd = 0f;
			}
			else
			{
				this.TimeSinceSitEnd += Time.deltaTime;
			}
			if (this.seatRoutine == null && this.CurrentSeat != null)
			{
				base.transform.position = this.CurrentSeat.SittingPoint.position + AvatarAnimation.SITTING_OFFSET * base.transform.localScale.y;
				base.transform.rotation = this.CurrentSeat.SittingPoint.rotation;
			}
			if (base.gameObject.activeInHierarchy)
			{
				this.framesActive++;
			}
		}

		// Token: 0x060043ED RID: 17389 RVA: 0x0011CE5A File Offset: 0x0011B05A
		private void InfrequentUpdate()
		{
			this.UpdateAnimationActive(false);
		}

		// Token: 0x060043EE RID: 17390 RVA: 0x0011CE64 File Offset: 0x0011B064
		private void MinPass()
		{
			if (this == null || this.animator == null)
			{
				return;
			}
			if (Time.timeSinceLevelLoad > 3f && this.animator.cullingMode != this.initialCullingMode)
			{
				this.animator.cullingMode = this.initialCullingMode;
			}
		}

		// Token: 0x060043EF RID: 17391 RVA: 0x0011CEBC File Offset: 0x0011B0BC
		private void UpdateAnimationActive(bool forceWriteIdle = false)
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			float num = Vector3.SqrMagnitude(PlayerSingleton<PlayerCamera>.Instance.transform.position - base.transform.position);
			bool flag = num < 1600f * QualitySettings.lodBias;
			if (flag && num > 225f)
			{
				flag = (Vector3.Dot(PlayerSingleton<PlayerCamera>.Instance.transform.forward, base.transform.position - PlayerSingleton<PlayerCamera>.Instance.transform.position) > 0f);
			}
			if (Time.timeSinceLevelLoad < 3f)
			{
				flag = true;
			}
			if (!this.AllowCulling)
			{
				flag = true;
			}
			bool isAvatarCulled = this.IsAvatarCulled;
			this.IsAvatarCulled = false;
			if (this.avatar.UseImpostor && this.UseImpostor)
			{
				if (!flag)
				{
					this.IsAvatarCulled = true;
				}
				if (!flag && !this.avatar.Impostor.gameObject.activeSelf)
				{
					this.avatar.BodyContainer.gameObject.SetActive(false);
					this.avatar.Impostor.EnableImpostor();
					return;
				}
				if (flag && this.avatar.Impostor.gameObject.activeSelf)
				{
					this.avatar.BodyContainer.gameObject.SetActive(true);
					this.avatar.Impostor.DisableImpostor();
				}
			}
			this.animator.enabled = (this.animationEnabled && flag);
			if (!this.IsAvatarCulled)
			{
				this.animator.SetBool("Sitting", this.IsSeated);
				if (isAvatarCulled && this.avatar.CurrentEquippable != null)
				{
					this.avatar.CurrentEquippable.InitializeAnimation();
				}
			}
		}

		// Token: 0x060043F0 RID: 17392 RVA: 0x0011D06E File Offset: 0x0011B26E
		public void SetDirection(float dir)
		{
			this.animator.SetFloat("Direction", dir);
		}

		// Token: 0x060043F1 RID: 17393 RVA: 0x0011D081 File Offset: 0x0011B281
		public void SetStrafe(float strafe)
		{
			this.animator.SetFloat("Strafe", strafe);
		}

		// Token: 0x060043F2 RID: 17394 RVA: 0x0011D094 File Offset: 0x0011B294
		public void SetTimeAirborne(float airbone)
		{
			this.animator.SetFloat("TimeAirborne", airbone);
		}

		// Token: 0x060043F3 RID: 17395 RVA: 0x0011D0A7 File Offset: 0x0011B2A7
		public void SetCrouched(bool crouched)
		{
			this.IsCrouched = crouched;
			this.animator.SetBool("isCrouched", crouched);
		}

		// Token: 0x060043F4 RID: 17396 RVA: 0x0011D0C1 File Offset: 0x0011B2C1
		public void SetGrounded(bool grounded)
		{
			this.animator.SetBool("isGrounded", grounded);
		}

		// Token: 0x060043F5 RID: 17397 RVA: 0x0011D0D4 File Offset: 0x0011B2D4
		public void Jump()
		{
			this.animator.SetTrigger("Jump");
		}

		// Token: 0x060043F6 RID: 17398 RVA: 0x0011D0E6 File Offset: 0x0011B2E6
		public void SetAnimationEnabled(bool enabled)
		{
			this.animationEnabled = enabled;
			this.UpdateAnimationActive(false);
		}

		// Token: 0x060043F7 RID: 17399 RVA: 0x0011D0F8 File Offset: 0x0011B2F8
		public void Flinch(Vector3 forceDirection, AvatarAnimation.EFlinchType flinchType)
		{
			Vector3 vector = base.transform.InverseTransformDirection(forceDirection);
			AvatarAnimation.EFlinchDirection eflinchDirection;
			if (Mathf.Abs(vector.z) > Mathf.Abs(vector.x))
			{
				if (vector.z > 0f)
				{
					eflinchDirection = AvatarAnimation.EFlinchDirection.Forward;
				}
				else
				{
					eflinchDirection = AvatarAnimation.EFlinchDirection.Backward;
				}
			}
			else if (vector.x > 0f)
			{
				eflinchDirection = AvatarAnimation.EFlinchDirection.Right;
			}
			else
			{
				eflinchDirection = AvatarAnimation.EFlinchDirection.Left;
			}
			if (flinchType != AvatarAnimation.EFlinchType.Light)
			{
				switch (eflinchDirection)
				{
				case AvatarAnimation.EFlinchDirection.Forward:
					this.animator.SetTrigger("Flinch_Heavy_Forward");
					break;
				case AvatarAnimation.EFlinchDirection.Backward:
					this.animator.SetTrigger("Flinch_Heavy_Backward");
					break;
				case AvatarAnimation.EFlinchDirection.Left:
					this.animator.SetTrigger("Flinch_Heavy_Left");
					break;
				case AvatarAnimation.EFlinchDirection.Right:
					this.animator.SetTrigger("Flinch_Heavy_Right");
					break;
				}
				if (this.onHeavyFlinch != null)
				{
					this.onHeavyFlinch.Invoke();
				}
				return;
			}
			switch (eflinchDirection)
			{
			case AvatarAnimation.EFlinchDirection.Forward:
				this.animator.SetTrigger("Flinch_Forward");
				return;
			case AvatarAnimation.EFlinchDirection.Backward:
				this.animator.SetTrigger("Flinch_Backward");
				return;
			case AvatarAnimation.EFlinchDirection.Left:
				this.animator.SetTrigger("Flinch_Left");
				return;
			case AvatarAnimation.EFlinchDirection.Right:
				this.animator.SetTrigger("Flinch_Right");
				return;
			default:
				return;
			}
		}

		// Token: 0x060043F8 RID: 17400 RVA: 0x0011D224 File Offset: 0x0011B424
		public void PlayStandUpAnimation()
		{
			AvatarAnimation.<>c__DisplayClass75_0 CS$<>8__locals1 = new AvatarAnimation.<>c__DisplayClass75_0();
			CS$<>8__locals1.<>4__this = this;
			this.StandUpAnimationPlaying = true;
			if (this.onStandupStart != null)
			{
				this.onStandupStart.Invoke();
			}
			this.PopulateBoneTransforms(this.ragdollBoneTransforms);
			CS$<>8__locals1.standUpFromBack = this.ShouldGetUpFromBack();
			this.PopulateAnimationStartBoneTransforms(this.StandUpFromFrontClipName, this.standUpFromFrontBoneTransforms);
			this.PopulateAnimationStartBoneTransforms(this.StandUpFromBackClipName, this.standUpFromBackBoneTransforms);
			CS$<>8__locals1.finalBoneTransforms = (CS$<>8__locals1.standUpFromBack ? this.standUpFromBackBoneTransforms : this.standUpFromFrontBoneTransforms);
			if (this.standUpRoutine != null)
			{
				base.StopCoroutine(this.standUpRoutine);
			}
			this.standUpRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<PlayStandUpAnimation>g__StandUpRoutine|0());
		}

		// Token: 0x060043F9 RID: 17401 RVA: 0x0011D2DC File Offset: 0x0011B4DC
		protected void RagdollChange(bool oldValue, bool ragdoll, bool playStandUpAnim)
		{
			bool flag = oldValue && !ragdoll && playStandUpAnim;
			if (ragdoll && this.IsSeated)
			{
				if (this.CurrentSeat != null)
				{
					this.CurrentSeat.SetOccupant(null);
					this.CurrentSeat = null;
				}
				this.animator.SetBool("Sitting", false);
				base.GetComponentInParent<NPCMovement>().SpeedController.RemoveSpeedControl("seated");
			}
			if (ragdoll && this.standUpRoutine != null)
			{
				base.StopCoroutine(this.standUpRoutine);
			}
			if (oldValue && !ragdoll)
			{
				this.AlignPositionToHips();
			}
			if (!flag)
			{
				this.SetAnimationEnabled(!ragdoll);
				return;
			}
			this.PlayStandUpAnimation();
		}

		// Token: 0x060043FA RID: 17402 RVA: 0x0011D380 File Offset: 0x0011B580
		private void AlignPositionToHips()
		{
			Vector3 position = this.HipBone.position;
			Quaternion rotation = this.HipBone.rotation;
			base.transform.position = this.HipBone.position;
			RaycastHit raycastHit;
			if (Physics.Raycast(base.transform.position, Vector3.down, ref raycastHit, 10f, this.GroundingMask))
			{
				base.transform.position = new Vector3(base.transform.position.x, raycastHit.point.y, base.transform.position.z);
			}
			base.transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(this.ShouldGetUpFromBack() ? (-this.HipBone.up) : this.HipBone.up, Vector3.up), Vector3.up);
			this.HipBone.position = position;
			this.HipBone.rotation = rotation;
		}

		// Token: 0x060043FB RID: 17403 RVA: 0x0011D47C File Offset: 0x0011B67C
		private bool ShouldGetUpFromBack()
		{
			return Vector3.Angle(this.HipBone.forward, Vector3.up) < 90f;
		}

		// Token: 0x060043FC RID: 17404 RVA: 0x0011D49C File Offset: 0x0011B69C
		private void PopulateBoneTransforms(BoneTransform[] boneTransforms)
		{
			for (int i = 0; i < this.Bones.Length; i++)
			{
				boneTransforms[i].Position = this.Bones[i].localPosition;
				boneTransforms[i].Rotation = this.Bones[i].localRotation;
			}
		}

		// Token: 0x060043FD RID: 17405 RVA: 0x0011D4E8 File Offset: 0x0011B6E8
		private List<Pose> GetBoneTransforms()
		{
			List<Pose> list = new List<Pose>();
			for (int i = 0; i < this.Bones.Length; i++)
			{
				list.Add(new Pose(this.Bones[i].localPosition, this.Bones[i].localRotation));
			}
			return list;
		}

		// Token: 0x060043FE RID: 17406 RVA: 0x0011D534 File Offset: 0x0011B734
		private void PopulateAnimationStartBoneTransforms(string clipName, BoneTransform[] boneTransforms)
		{
			Vector3 position = this.animator.transform.position;
			Quaternion rotation = this.animator.transform.rotation;
			if (this.animator.runtimeAnimatorController == null)
			{
				return;
			}
			foreach (AnimationClip animationClip in this.animator.runtimeAnimatorController.animationClips)
			{
				if (animationClip.name == clipName)
				{
					animationClip.SampleAnimation(this.animator.gameObject, 0f);
					this.PopulateBoneTransforms(boneTransforms);
					break;
				}
			}
			this.animator.transform.position = position;
			this.animator.transform.rotation = rotation;
		}

		// Token: 0x060043FF RID: 17407 RVA: 0x0011D5EC File Offset: 0x0011B7EC
		public void SetTrigger(string trigger)
		{
			if (string.IsNullOrEmpty(trigger))
			{
				return;
			}
			this.animator.SetTrigger(trigger);
			this.UpdateAnimationActive(true);
		}

		// Token: 0x06004400 RID: 17408 RVA: 0x0011D60A File Offset: 0x0011B80A
		public void ResetTrigger(string trigger)
		{
			this.animator.ResetTrigger(trigger);
		}

		// Token: 0x06004401 RID: 17409 RVA: 0x0011D618 File Offset: 0x0011B818
		public void SetBool(string id, bool value)
		{
			this.animator.SetBool(id, value);
			this.UpdateAnimationActive(true);
		}

		// Token: 0x06004402 RID: 17410 RVA: 0x0011D630 File Offset: 0x0011B830
		public void SetSeat(AvatarSeat seat)
		{
			AvatarAnimation.<>c__DisplayClass85_0 CS$<>8__locals1 = new AvatarAnimation.<>c__DisplayClass85_0();
			CS$<>8__locals1.<>4__this = this;
			if (seat == this.CurrentSeat)
			{
				return;
			}
			if (this.CurrentSeat != null)
			{
				this.CurrentSeat.SetOccupant(null);
			}
			this.CurrentSeat = seat;
			if (this.CurrentSeat != null)
			{
				this.CurrentSeat.SetOccupant(base.GetComponentInParent<NPC>());
			}
			this.animator.SetBool("Sitting", this.IsSeated);
			CS$<>8__locals1.startPos = base.transform.position;
			CS$<>8__locals1.startRot = base.transform.rotation;
			if (this.seatRoutine != null)
			{
				Singleton<CoroutineService>.Instance.StopCoroutine(this.seatRoutine);
			}
			if (this.CurrentSeat != null)
			{
				CS$<>8__locals1.endPos = this.CurrentSeat.SittingPoint.position + AvatarAnimation.SITTING_OFFSET * base.transform.localScale.y;
				CS$<>8__locals1.endRot = this.CurrentSeat.SittingPoint.rotation;
				this.seatRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SetSeat>g__Lerp|0(false));
				base.GetComponentInParent<NPCMovement>().SpeedController.AddSpeedControl(new NPCSpeedController.SpeedControl("seated", 100, -1f));
				return;
			}
			CS$<>8__locals1.endPos = base.transform.parent.position;
			CS$<>8__locals1.endRot = base.transform.parent.rotation;
			this.seatRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SetSeat>g__Lerp|0(true));
		}

		// Token: 0x06004403 RID: 17411 RVA: 0x0011D7C4 File Offset: 0x0011B9C4
		public void SkateboardMounted(Skateboard board)
		{
			this.IKController.BodyIK.solvers.pelvis.target = board.Animation.PelvisAlignment.Transform;
			this.IKController.BodyIK.solvers.spine.target = board.Animation.SpineAlignment.Transform;
			this.IKController.BodyIK.solvers.leftFoot.target = board.Animation.LeftFootAlignment.Transform;
			this.IKController.BodyIK.solvers.rightFoot.target = board.Animation.RightFootAlignment.Transform;
			this.IKController.BodyIK.solvers.leftHand.target = board.Animation.LeftHandAlignment.Transform;
			this.IKController.BodyIK.solvers.rightHand.target = board.Animation.RightHandAlignment.Transform;
			this.IKController.BodyIK.solvers.rightFoot.SetBendPlaneToCurrent();
			this.IKController.BodyIK.solvers.leftFoot.SetBendPlaneToCurrent();
			this.IKController.OverrideLegBendTargets(board.Animation.LeftLegBendTarget.Transform, board.Animation.RightLegBendTarget.Transform);
			this.IKController.SetIKActive(true);
			this.avatar.SetEquippable(string.Empty);
			this.avatar.LookController.ForceLookTarget = board.Animation.AvatarFaceTarget;
			this.avatar.LookController.ForceLookRotateBody = true;
			this.SetBool("SkateIdle", true);
			this.activeSkateboard = board;
			this.activeSkateboard.OnPushStart.AddListener(new UnityAction(this.SkateboardPush));
		}

		// Token: 0x06004404 RID: 17412 RVA: 0x0011D9A4 File Offset: 0x0011BBA4
		public void SkateboardDismounted()
		{
			this.IKController.ResetLegBendTargets();
			this.IKController.SetIKActive(false);
			this.avatar.LookController.ForceLookTarget = null;
			this.avatar.LookController.ForceLookRotateBody = false;
			this.SetBool("SkateIdle", false);
			this.activeSkateboard.OnPushStart.RemoveListener(new UnityAction(this.SkateboardPush));
			this.activeSkateboard = null;
		}

		// Token: 0x06004405 RID: 17413 RVA: 0x0011DA19 File Offset: 0x0011BC19
		private void SkateboardPush()
		{
			this.SetTrigger("SkatePush");
		}

		// Token: 0x040030B0 RID: 12464
		public const float AnimationRangeSqr = 1600f;

		// Token: 0x040030B1 RID: 12465
		public const float FrustrumCullMinDist = 225f;

		// Token: 0x040030B2 RID: 12466
		public const float RunningAnimationSpeed = 8f;

		// Token: 0x040030B3 RID: 12467
		public const float MaxBoneOffset = 0.01f;

		// Token: 0x040030B4 RID: 12468
		public const float MaxBoneOffsetSqr = 0.0001f;

		// Token: 0x040030B5 RID: 12469
		public static Vector3 SITTING_OFFSET = new Vector3(0f, -0.825f, 0f);

		// Token: 0x040030B6 RID: 12470
		public const float SEAT_TIME = 0.5f;

		// Token: 0x040030BC RID: 12476
		public bool DEBUG_MODE;

		// Token: 0x040030BD RID: 12477
		private int framesActive;

		// Token: 0x040030BE RID: 12478
		[Header("References")]
		public Animator animator;

		// Token: 0x040030BF RID: 12479
		public Transform HipBone;

		// Token: 0x040030C0 RID: 12480
		public Transform[] Bones;

		// Token: 0x040030C1 RID: 12481
		protected Avatar avatar;

		// Token: 0x040030C2 RID: 12482
		public Transform LeftHandContainer;

		// Token: 0x040030C3 RID: 12483
		public Transform RightHandContainer;

		// Token: 0x040030C4 RID: 12484
		public Transform RightHandAlignmentPoint;

		// Token: 0x040030C5 RID: 12485
		public Transform LeftHandAlignmentPoint;

		// Token: 0x040030C6 RID: 12486
		public AvatarIKController IKController;

		// Token: 0x040030C7 RID: 12487
		[Header("Settings")]
		public LayerMask GroundingMask;

		// Token: 0x040030C8 RID: 12488
		public string StandUpFromBackClipName;

		// Token: 0x040030C9 RID: 12489
		public string StandUpFromFrontClipName;

		// Token: 0x040030CA RID: 12490
		public bool UseImpostor = true;

		// Token: 0x040030CB RID: 12491
		public bool AllowCulling = true;

		// Token: 0x040030CC RID: 12492
		public UnityEvent onStandupStart;

		// Token: 0x040030CD RID: 12493
		public UnityEvent onStandupDone;

		// Token: 0x040030CE RID: 12494
		public UnityEvent onHeavyFlinch;

		// Token: 0x040030CF RID: 12495
		private BoneTransform[] standingBoneTransforms;

		// Token: 0x040030D0 RID: 12496
		private BoneTransform[] standUpFromBackBoneTransforms;

		// Token: 0x040030D1 RID: 12497
		private BoneTransform[] standUpFromFrontBoneTransforms;

		// Token: 0x040030D2 RID: 12498
		private BoneTransform[] ragdollBoneTransforms;

		// Token: 0x040030D3 RID: 12499
		private Coroutine standUpRoutine;

		// Token: 0x040030D4 RID: 12500
		private Coroutine seatRoutine;

		// Token: 0x040030D5 RID: 12501
		private Skateboard activeSkateboard;

		// Token: 0x040030D6 RID: 12502
		private bool animationEnabled = true;

		// Token: 0x040030D7 RID: 12503
		private AnimatorCullingMode initialCullingMode;

		// Token: 0x020009D1 RID: 2513
		public enum EFlinchType
		{
			// Token: 0x040030D9 RID: 12505
			Light,
			// Token: 0x040030DA RID: 12506
			Heavy
		}

		// Token: 0x020009D2 RID: 2514
		public enum EFlinchDirection
		{
			// Token: 0x040030DC RID: 12508
			Forward,
			// Token: 0x040030DD RID: 12509
			Backward,
			// Token: 0x040030DE RID: 12510
			Left,
			// Token: 0x040030DF RID: 12511
			Right
		}
	}
}
