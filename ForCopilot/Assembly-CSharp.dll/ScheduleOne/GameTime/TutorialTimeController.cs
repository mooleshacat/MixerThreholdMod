using System;
using EasyButtons;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.GameTime
{
	// Token: 0x020002C0 RID: 704
	public class TutorialTimeController : MonoBehaviour
	{
		// Token: 0x06000F27 RID: 3879 RVA: 0x00042A13 File Offset: 0x00040C13
		private void Awake()
		{
			TimeManager.onSleepStart = (Action)Delegate.Combine(TimeManager.onSleepStart, new Action(this.IncrementKeyframe));
		}

		// Token: 0x06000F28 RID: 3880 RVA: 0x00042A35 File Offset: 0x00040C35
		private void OnDestroy()
		{
			TimeManager.onSleepStart = (Action)Delegate.Remove(TimeManager.onSleepStart, new Action(this.IncrementKeyframe));
		}

		// Token: 0x06000F29 RID: 3881 RVA: 0x00042A58 File Offset: 0x00040C58
		private void Update()
		{
			if (this.disabled)
			{
				return;
			}
			TutorialTimeController.KeyFrame keyFrame = this.KeyFrames[this.currentKeyFrameIndex];
			float time = Mathf.Clamp01(Mathf.InverseLerp((float)this.GetCurrentKeyFrameStart(), (float)keyFrame.Time, (float)NetworkSingleton<TimeManager>.Instance.CurrentTime));
			float timeProgressionMultiplier = this.TimeProgressionCurve.Evaluate(time) * keyFrame.SpeedMultiplier;
			NetworkSingleton<TimeManager>.Instance.TimeProgressionMultiplier = timeProgressionMultiplier;
		}

		// Token: 0x06000F2A RID: 3882 RVA: 0x00042AC3 File Offset: 0x00040CC3
		private int GetCurrentKeyFrameStart()
		{
			if (this.currentKeyFrameIndex > 0)
			{
				return this.KeyFrames[this.currentKeyFrameIndex - 1].Time;
			}
			return NetworkSingleton<TimeManager>.Instance.DefaultTime;
		}

		// Token: 0x06000F2B RID: 3883 RVA: 0x00042AF4 File Offset: 0x00040CF4
		[Button]
		public void IncrementKeyframe()
		{
			Console.Log("Incrementing keyframe to " + (this.currentKeyFrameIndex + 1).ToString(), null);
			this.currentKeyFrameIndex = Mathf.Clamp(this.currentKeyFrameIndex + 1, 0, this.KeyFrames.Length - 1);
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x00042B3F File Offset: 0x00040D3F
		public void Disable()
		{
			NetworkSingleton<TimeManager>.Instance.TimeProgressionMultiplier = 1f;
			base.enabled = false;
			this.disabled = true;
		}

		// Token: 0x04000F65 RID: 3941
		public AnimationCurve TimeProgressionCurve;

		// Token: 0x04000F66 RID: 3942
		public TutorialTimeController.KeyFrame[] KeyFrames;

		// Token: 0x04000F67 RID: 3943
		[SerializeField]
		private int currentKeyFrameIndex;

		// Token: 0x04000F68 RID: 3944
		private bool disabled;

		// Token: 0x020002C1 RID: 705
		[Serializable]
		public struct KeyFrame
		{
			// Token: 0x04000F69 RID: 3945
			public int Time;

			// Token: 0x04000F6A RID: 3946
			public float SpeedMultiplier;

			// Token: 0x04000F6B RID: 3947
			public string Note;
		}
	}
}
