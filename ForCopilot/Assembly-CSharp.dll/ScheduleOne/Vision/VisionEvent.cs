using System;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Vision
{
	// Token: 0x02000292 RID: 658
	public class VisionEvent
	{
		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000DC3 RID: 3523 RVA: 0x0003D1CE File Offset: 0x0003B3CE
		// (set) Token: 0x06000DC4 RID: 3524 RVA: 0x0003D1D6 File Offset: 0x0003B3D6
		public Player Target { get; protected set; }

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000DC5 RID: 3525 RVA: 0x0003D1DF File Offset: 0x0003B3DF
		// (set) Token: 0x06000DC6 RID: 3526 RVA: 0x0003D1E7 File Offset: 0x0003B3E7
		public PlayerVisualState.VisualState State { get; protected set; }

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000DC7 RID: 3527 RVA: 0x0003D1F0 File Offset: 0x0003B3F0
		// (set) Token: 0x06000DC8 RID: 3528 RVA: 0x0003D1F8 File Offset: 0x0003B3F8
		public VisionCone Owner { get; protected set; }

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000DC9 RID: 3529 RVA: 0x0003D201 File Offset: 0x0003B401
		// (set) Token: 0x06000DCA RID: 3530 RVA: 0x0003D209 File Offset: 0x0003B409
		public float FullNoticeTime { get; protected set; }

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000DCB RID: 3531 RVA: 0x0003D212 File Offset: 0x0003B412
		public float NormalizedNoticeLevel
		{
			get
			{
				return this.currentNoticeTime / this.FullNoticeTime;
			}
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x0003D224 File Offset: 0x0003B424
		public VisionEvent(VisionCone _owner, Player _target, PlayerVisualState.VisualState _state, float _noticeTime)
		{
			this.Owner = _owner;
			this.Target = _target;
			this.State = _state;
			this.FullNoticeTime = _noticeTime;
			PlayerVisualState.VisualState state = this.State;
			state.stateDestroyed = (Action)Delegate.Combine(state.stateDestroyed, new Action(this.EndEvent));
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x0003D27C File Offset: 0x0003B47C
		public void UpdateEvent(float visionDeltaThisFrame, float tickTime)
		{
			float normalizedNoticeLevel = this.NormalizedNoticeLevel;
			if (visionDeltaThisFrame > 0f)
			{
				this.timeSinceSighted = 0f;
			}
			else
			{
				this.timeSinceSighted += tickTime;
			}
			if (visionDeltaThisFrame > 0f)
			{
				this.currentNoticeTime += visionDeltaThisFrame * (this.Owner.Attentiveness * VisionCone.UniversalAttentivenessScale) * tickTime;
			}
			else if (this.timeSinceSighted > 1f * (this.Owner.Memory * VisionCone.UniversalMemoryScale))
			{
				this.currentNoticeTime -= tickTime / (this.Owner.Memory * VisionCone.UniversalMemoryScale);
			}
			this.currentNoticeTime = Mathf.Clamp(this.currentNoticeTime, 0f, this.FullNoticeTime);
			if (this.Target.Visibility.HighestVisionEvent == null || this.NormalizedNoticeLevel > this.Target.Visibility.HighestVisionEvent.NormalizedNoticeLevel)
			{
				this.Target.Visibility.HighestVisionEvent = this;
			}
			if (this.NormalizedNoticeLevel <= 0f && normalizedNoticeLevel > 0f)
			{
				this.EndEvent();
			}
			if (this.NormalizedNoticeLevel >= 0.5f && normalizedNoticeLevel < 0.5f)
			{
				if (this.Target.Visibility.HighestVisionEvent == this)
				{
					this.Target.Visibility.HighestVisionEvent = null;
				}
				this.Owner.EventHalfNoticed(this);
			}
			if (this.NormalizedNoticeLevel >= 1f && normalizedNoticeLevel < 1f)
			{
				if (this.Target.Visibility.HighestVisionEvent == this)
				{
					this.Target.Visibility.HighestVisionEvent = null;
				}
				this.Owner.EventFullyNoticed(this);
			}
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x0003D41F File Offset: 0x0003B61F
		public void EndEvent()
		{
			if (this.Target.Visibility.HighestVisionEvent == this)
			{
				this.Target.Visibility.HighestVisionEvent = null;
			}
			this.Owner.EventReachedZero(this);
		}

		// Token: 0x04000E2F RID: 3631
		private const float NOTICE_DROP_THRESHOLD = 1f;

		// Token: 0x04000E34 RID: 3636
		private float timeSinceSighted;

		// Token: 0x04000E35 RID: 3637
		private float currentNoticeTime;
	}
}
