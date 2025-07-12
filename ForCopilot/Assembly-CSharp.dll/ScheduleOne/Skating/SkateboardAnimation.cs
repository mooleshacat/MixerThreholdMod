using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Skating
{
	// Token: 0x020002DB RID: 731
	public class SkateboardAnimation : MonoBehaviour
	{
		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06000FCE RID: 4046 RVA: 0x00045905 File Offset: 0x00043B05
		public float CurrentCrouchShift
		{
			get
			{
				return this.currentCrouchShift;
			}
		}

		// Token: 0x06000FCF RID: 4047 RVA: 0x00045910 File Offset: 0x00043B10
		private void Awake()
		{
			this.board = base.GetComponent<Skateboard>();
			this.board.OnPushStart.AddListener(new UnityAction(this.OnPushStart));
			this.pelvisDefaultPosition = this.PelvisAlignment.Transform.localPosition;
			this.pelvisDefaultRotation = this.PelvisAlignment.Transform.localRotation;
			this.spineDefaultPosition = this.SpineAlignment.Transform.localPosition;
			this.alignmentSets.Add(this.PelvisContainerAlignment);
			this.alignmentSets.Add(this.PelvisAlignment);
			this.alignmentSets.Add(this.SpineContainerAlignment);
			this.alignmentSets.Add(this.SpineAlignment);
			this.alignmentSets.Add(this.LeftFootAlignment);
			this.alignmentSets.Add(this.RightFootAlignment);
			this.alignmentSets.Add(this.LeftLegBendTarget);
			this.alignmentSets.Add(this.RightLegBendTarget);
			this.alignmentSets.Add(this.LeftHandAlignment);
			this.alignmentSets.Add(this.RightHandAlignment);
			this.alignmentSets.Add(this.LeftHandLoweredAlignment);
			this.alignmentSets.Add(this.LeftHandRaisedAlignment);
			this.alignmentSets.Add(this.RightHandLoweredAlignment);
			this.alignmentSets.Add(this.RightHandRaisedAlignment);
		}

		// Token: 0x06000FD0 RID: 4048 RVA: 0x00045A75 File Offset: 0x00043C75
		private void Update()
		{
			this.UpdateIKBlend();
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x00045A7D File Offset: 0x00043C7D
		private void LateUpdate()
		{
			this.UpdateBodyAlignment();
			this.UpdateArmLift();
			this.UpdatePelvisRotation();
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x000045B1 File Offset: 0x000027B1
		private void FixedUpdate()
		{
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x00045A94 File Offset: 0x00043C94
		private void UpdateIKBlend()
		{
			if (this.board.IsPushing || (this.board.TimeSincePushStart < this.PushAnimationDuration && this.board.JumpBuildAmount < 0.1f))
			{
				this.ikBlend = Mathf.Lerp(this.ikBlend, 1f, Time.deltaTime * this.IKBlendChangeRate);
			}
			else
			{
				this.ikBlend = Mathf.Lerp(this.ikBlend, 0f, Time.deltaTime * this.IKBlendChangeRate);
			}
			foreach (SkateboardAnimation.AlignmentSet alignmentSet in this.alignmentSets)
			{
				alignmentSet.Transform.localPosition = Vector3.Lerp(alignmentSet.Default.localPosition, alignmentSet.Animated.localPosition, this.ikBlend);
				alignmentSet.Transform.localRotation = Quaternion.Lerp(alignmentSet.Default.localRotation, alignmentSet.Animated.localRotation, this.ikBlend);
			}
		}

		// Token: 0x06000FD4 RID: 4052 RVA: 0x00045BB0 File Offset: 0x00043DB0
		private void UpdateBodyAlignment()
		{
			Vector3 vector = this.PelvisAlignment.Transform.parent.TransformPoint(new Vector3(this.pelvisDefaultPosition.x, -0.01f, this.pelvisDefaultPosition.z));
			Vector3 a = new Vector3(0f, this.pelvisDefaultPosition.y, 0f);
			Vector3 b = base.transform.up * this.pelvisDefaultPosition.y;
			vector += Vector3.Lerp(a, b, this.PelvisOffsetBlend);
			float jumpBuildAmount = this.board.JumpBuildAmount;
			float b2 = Mathf.Clamp01(this.board.CurrentSpeed_Kmh / this.board.TopSpeed_Kmh) * 0.1f;
			float b3 = Mathf.Max(jumpBuildAmount, b2);
			this.currentCrouchShift = Mathf.Lerp(this.currentCrouchShift, b3, Time.deltaTime * this.CrouchSpeed);
			vector.y -= this.currentCrouchShift * this.JumpCrouchAmount;
			float num = Mathf.Clamp(-this.board.Accelerometer.Acceleration.y * this.VerticalMomentumMultiplier, -this.VerticalMomentumOffsetClamp, 0f);
			float num2 = 1f;
			if (num < this.currentMomentumOffset)
			{
				num2 = 0.3f;
			}
			this.currentMomentumOffset = Mathf.Lerp(this.currentMomentumOffset, num, Time.deltaTime * this.MomentumMoveSpeed * num2);
			vector.y += this.currentMomentumOffset;
			this.PelvisAlignment.Transform.position = vector;
			this.SpineAlignment.Transform.localPosition = Vector3.Lerp(this.spineDefaultPosition, this.SpineAlignment_Hunched.localPosition, this.currentCrouchShift);
		}

		// Token: 0x06000FD5 RID: 4053 RVA: 0x00045D64 File Offset: 0x00043F64
		private void UpdateArmLift()
		{
			float jumpBuildAmount = this.board.JumpBuildAmount;
			float num = Mathf.Clamp01(this.board.CurrentSpeed_Kmh / this.board.TopSpeed_Kmh) * 0f;
			float num2 = Mathf.Abs(this.board.CurrentSteerInput) * 0.2f;
			this.SetArmLift(Mathf.Max(new float[]
			{
				jumpBuildAmount,
				num,
				num2
			}));
			this.currentArmLift = Mathf.Lerp(this.currentArmLift, this.targetArmLift, Time.deltaTime * this.ArmLiftRate);
			this.RightHandAlignment.Transform.localPosition = Vector3.Lerp(this.RightHandLoweredAlignment.Transform.localPosition, this.RightHandRaisedAlignment.Transform.localPosition, this.currentArmLift);
			this.RightHandAlignment.Transform.localRotation = Quaternion.Lerp(this.RightHandLoweredAlignment.Transform.localRotation, this.RightHandRaisedAlignment.Transform.localRotation, this.currentArmLift);
			this.LeftHandAlignment.Transform.localPosition = Vector3.Lerp(this.LeftHandLoweredAlignment.Transform.localPosition, this.LeftHandRaisedAlignment.Transform.localPosition, this.currentArmLift);
			this.LeftHandAlignment.Transform.localRotation = Quaternion.Lerp(this.LeftHandLoweredAlignment.Transform.localRotation, this.LeftHandRaisedAlignment.Transform.localRotation, this.currentArmLift);
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x00045EE4 File Offset: 0x000440E4
		private void UpdatePelvisRotation()
		{
			float num = this.board.CurrentSteerInput * this.PelvisMaxRotation;
			Quaternion b = this.pelvisDefaultRotation * Quaternion.AngleAxis(num, Vector3.up);
			this.PelvisAlignment.Transform.localRotation = Quaternion.Lerp(this.PelvisAlignment.Transform.localRotation, b, Time.deltaTime * 5f);
			this.HandContainer.localRotation = Quaternion.Lerp(this.HandContainer.localRotation, Quaternion.Euler(num, 0f, 0f), Time.deltaTime * 5f);
		}

		// Token: 0x06000FD7 RID: 4055 RVA: 0x00045F82 File Offset: 0x00044182
		public void SetArmLift(float lift)
		{
			this.targetArmLift = lift;
		}

		// Token: 0x06000FD8 RID: 4056 RVA: 0x00045F8C File Offset: 0x0004418C
		private void OnPushStart()
		{
			this.IKAnimation.Stop();
			this.IKAnimation[this.PushAnim.name].speed = this.PushAnimationSpeed;
			this.IKAnimation.Play(this.PushAnim.name);
		}

		// Token: 0x0400101C RID: 4124
		[Header("Settings")]
		public float JumpCrouchAmount = 0.4f;

		// Token: 0x0400101D RID: 4125
		public float CrouchSpeed = 4f;

		// Token: 0x0400101E RID: 4126
		public float ArmLiftRate = 5f;

		// Token: 0x0400101F RID: 4127
		public float PelvisMaxRotation = 10f;

		// Token: 0x04001020 RID: 4128
		public float HandsMaxRotation = 10f;

		// Token: 0x04001021 RID: 4129
		public float PelvisOffsetBlend;

		// Token: 0x04001022 RID: 4130
		public float VerticalMomentumMultiplier = 0.5f;

		// Token: 0x04001023 RID: 4131
		public float VerticalMomentumOffsetClamp = 0.3f;

		// Token: 0x04001024 RID: 4132
		public float MomentumMoveSpeed = 5f;

		// Token: 0x04001025 RID: 4133
		public float IKBlendChangeRate = 3f;

		// Token: 0x04001026 RID: 4134
		public float PushAnimationDuration = 1.1f;

		// Token: 0x04001027 RID: 4135
		public float PushAnimationSpeed = 1.3f;

		// Token: 0x04001028 RID: 4136
		public AnimationClip PushAnim;

		// Token: 0x04001029 RID: 4137
		[Header("References")]
		public SkateboardAnimation.AlignmentSet PelvisContainerAlignment;

		// Token: 0x0400102A RID: 4138
		public SkateboardAnimation.AlignmentSet PelvisAlignment;

		// Token: 0x0400102B RID: 4139
		public SkateboardAnimation.AlignmentSet SpineContainerAlignment;

		// Token: 0x0400102C RID: 4140
		public SkateboardAnimation.AlignmentSet SpineAlignment;

		// Token: 0x0400102D RID: 4141
		public Transform SpineAlignment_Hunched;

		// Token: 0x0400102E RID: 4142
		public SkateboardAnimation.AlignmentSet LeftFootAlignment;

		// Token: 0x0400102F RID: 4143
		public SkateboardAnimation.AlignmentSet RightFootAlignment;

		// Token: 0x04001030 RID: 4144
		public SkateboardAnimation.AlignmentSet LeftLegBendTarget;

		// Token: 0x04001031 RID: 4145
		public SkateboardAnimation.AlignmentSet RightLegBendTarget;

		// Token: 0x04001032 RID: 4146
		public SkateboardAnimation.AlignmentSet LeftHandAlignment;

		// Token: 0x04001033 RID: 4147
		public SkateboardAnimation.AlignmentSet RightHandAlignment;

		// Token: 0x04001034 RID: 4148
		public Transform AvatarFaceTarget;

		// Token: 0x04001035 RID: 4149
		public Transform HandContainer;

		// Token: 0x04001036 RID: 4150
		public Animation IKAnimation;

		// Token: 0x04001037 RID: 4151
		[Header("Arm Lift")]
		public SkateboardAnimation.AlignmentSet LeftHandLoweredAlignment;

		// Token: 0x04001038 RID: 4152
		public SkateboardAnimation.AlignmentSet LeftHandRaisedAlignment;

		// Token: 0x04001039 RID: 4153
		public SkateboardAnimation.AlignmentSet RightHandLoweredAlignment;

		// Token: 0x0400103A RID: 4154
		public SkateboardAnimation.AlignmentSet RightHandRaisedAlignment;

		// Token: 0x0400103B RID: 4155
		private Skateboard board;

		// Token: 0x0400103C RID: 4156
		private float currentCrouchShift;

		// Token: 0x0400103D RID: 4157
		private float targetArmLift;

		// Token: 0x0400103E RID: 4158
		private float currentArmLift;

		// Token: 0x0400103F RID: 4159
		private Quaternion pelvisDefaultRotation;

		// Token: 0x04001040 RID: 4160
		private Vector3 pelvisDefaultPosition;

		// Token: 0x04001041 RID: 4161
		private Vector3 spineDefaultPosition;

		// Token: 0x04001042 RID: 4162
		private float currentMomentumOffset;

		// Token: 0x04001043 RID: 4163
		private float ikBlend;

		// Token: 0x04001044 RID: 4164
		private List<SkateboardAnimation.AlignmentSet> alignmentSets = new List<SkateboardAnimation.AlignmentSet>();

		// Token: 0x020002DC RID: 732
		[Serializable]
		public class AlignmentSet
		{
			// Token: 0x04001045 RID: 4165
			public Transform Transform;

			// Token: 0x04001046 RID: 4166
			public Transform Default;

			// Token: 0x04001047 RID: 4167
			public Transform Animated;
		}
	}
}
