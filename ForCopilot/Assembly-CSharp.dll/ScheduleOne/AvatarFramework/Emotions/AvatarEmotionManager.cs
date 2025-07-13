using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Emotions
{
	// Token: 0x020009C6 RID: 2502
	public class AvatarEmotionManager : MonoBehaviour
	{
		// Token: 0x1700097D RID: 2429
		// (get) Token: 0x060043A9 RID: 17321 RVA: 0x0011C242 File Offset: 0x0011A442
		// (set) Token: 0x060043AA RID: 17322 RVA: 0x0011C24A File Offset: 0x0011A44A
		public string CurrentEmotion { get; protected set; } = "Neutral";

		// Token: 0x1700097E RID: 2430
		// (get) Token: 0x060043AB RID: 17323 RVA: 0x0011C253 File Offset: 0x0011A453
		// (set) Token: 0x060043AC RID: 17324 RVA: 0x0011C25B File Offset: 0x0011A45B
		public AvatarEmotionPreset CurrentEmotionPreset { get; protected set; }

		// Token: 0x1700097F RID: 2431
		// (get) Token: 0x060043AD RID: 17325 RVA: 0x0011C264 File Offset: 0x0011A464
		public bool IsSwitchingEmotion
		{
			get
			{
				return this.emotionLerpRoutine != null;
			}
		}

		// Token: 0x060043AE RID: 17326 RVA: 0x0011C270 File Offset: 0x0011A470
		private void Start()
		{
			this.neutralPreset = this.EmotionPresetList.Find((AvatarEmotionPreset x) => x.PresetName == "Neutral");
			this.AddEmotionOverride("Neutral", "base_emotion", 0f, -1);
			base.InvokeRepeating("UpdateEmotion", 0f, 0.25f);
		}

		// Token: 0x060043AF RID: 17327 RVA: 0x000045B1 File Offset: 0x000027B1
		private void Update()
		{
		}

		// Token: 0x060043B0 RID: 17328 RVA: 0x0011C2D8 File Offset: 0x0011A4D8
		public void UpdateEmotion()
		{
			if (PlayerSingleton<PlayerCamera>.InstanceExists && Vector3.Distance(base.transform.position, PlayerSingleton<PlayerCamera>.Instance.transform.position) > 30f)
			{
				return;
			}
			EmotionOverride highestPriorityOverride = this.GetHighestPriorityOverride();
			if (highestPriorityOverride == null)
			{
				return;
			}
			if (highestPriorityOverride != this.activeEmotionOverride)
			{
				this.activeEmotionOverride = highestPriorityOverride;
				this.LerpEmotion(this.GetEmotion(highestPriorityOverride.Emotion), 0.2f);
			}
		}

		// Token: 0x060043B1 RID: 17329 RVA: 0x0011C348 File Offset: 0x0011A548
		public void ConfigureNeutralFace(Texture2D faceTex, float restingBrowHeight, float restingBrowAngle, Eye.EyeLidConfiguration leftEyelidConfig, Eye.EyeLidConfiguration rightEyelidConfig)
		{
			this.neutralPreset = this.EmotionPresetList.Find((AvatarEmotionPreset x) => x.PresetName == "Neutral");
			if (this.neutralPreset == null)
			{
				Debug.LogError("Could not find neutral preset");
				return;
			}
			this.neutralPreset.FaceTexture = faceTex;
			this.neutralPreset.BrowAngleChange_R = restingBrowAngle;
			this.neutralPreset.BrowAngleChange_L = restingBrowAngle;
			this.neutralPreset.BrowHeightChange_L = restingBrowHeight;
			this.neutralPreset.BrowHeightChange_R = restingBrowHeight;
			this.neutralPreset.LeftEyeRestingState = leftEyelidConfig;
			this.neutralPreset.RightEyeRestingState = rightEyelidConfig;
			if (this.CurrentEmotionPreset == this.neutralPreset)
			{
				this.SetEmotion(this.neutralPreset);
			}
		}

		// Token: 0x060043B2 RID: 17330 RVA: 0x0011C408 File Offset: 0x0011A608
		public virtual void AddEmotionOverride(string emotionName, string overrideLabel, float duration = 0f, int priority = 0)
		{
			AvatarEmotionManager.<>c__DisplayClass25_0 CS$<>8__locals1 = new AvatarEmotionManager.<>c__DisplayClass25_0();
			CS$<>8__locals1.overrideLabel = overrideLabel;
			CS$<>8__locals1.duration = duration;
			CS$<>8__locals1.<>4__this = this;
			EmotionOverride emotionOverride = this.overrideStack.Find((EmotionOverride x) => x.Label.ToLower() == CS$<>8__locals1.overrideLabel.ToLower());
			if (emotionOverride != null)
			{
				emotionOverride.Emotion = emotionName;
				emotionOverride.Priority = priority;
				if (emotionOverride == this.activeEmotionOverride)
				{
					this.activeEmotionOverride = null;
				}
			}
			else
			{
				emotionOverride = new EmotionOverride(emotionName, CS$<>8__locals1.overrideLabel, priority);
				this.overrideStack.Add(emotionOverride);
			}
			this.ClearRemovalRoutine(CS$<>8__locals1.overrideLabel);
			if (CS$<>8__locals1.duration > 0f)
			{
				Coroutine value = Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<AddEmotionOverride>g__RemoveEmotionAfterDuration|1());
				this.emotionRemovalRoutines.Add(CS$<>8__locals1.overrideLabel.ToLower(), value);
			}
		}

		// Token: 0x060043B3 RID: 17331 RVA: 0x0011C4C8 File Offset: 0x0011A6C8
		public void RemoveEmotionOverride(string label)
		{
			this.ClearRemovalRoutine(label);
			EmotionOverride emotionOverride = this.overrideStack.Find((EmotionOverride x) => x.Label.ToLower() == label.ToLower());
			if (emotionOverride == null)
			{
				return;
			}
			this.overrideStack.Remove(emotionOverride);
		}

		// Token: 0x060043B4 RID: 17332 RVA: 0x0011C518 File Offset: 0x0011A718
		public void ClearOverrides()
		{
			EmotionOverride[] array = this.overrideStack.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				if (!(array[i].Label == "base_emotion"))
				{
					this.RemoveEmotionOverride(array[i].Label);
				}
			}
		}

		// Token: 0x060043B5 RID: 17333 RVA: 0x0011C564 File Offset: 0x0011A764
		private void ClearRemovalRoutine(string label)
		{
			label = label.ToLower();
			if (this.emotionRemovalRoutines.ContainsKey(label))
			{
				if (this.emotionRemovalRoutines[label] != null)
				{
					base.StopCoroutine(this.emotionRemovalRoutines[label]);
				}
				this.emotionRemovalRoutines.Remove(label);
			}
		}

		// Token: 0x060043B6 RID: 17334 RVA: 0x0011C5B4 File Offset: 0x0011A7B4
		public EmotionOverride GetHighestPriorityOverride()
		{
			return (from x in this.overrideStack
			orderby x.Priority descending
			select x).ToList<EmotionOverride>().FirstOrDefault<EmotionOverride>();
		}

		// Token: 0x060043B7 RID: 17335 RVA: 0x0011C5EC File Offset: 0x0011A7EC
		private void LerpEmotion(AvatarEmotionPreset preset, float animationTime = 0.2f)
		{
			AvatarEmotionManager.<>c__DisplayClass30_0 CS$<>8__locals1 = new AvatarEmotionManager.<>c__DisplayClass30_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.preset = preset;
			CS$<>8__locals1.animationTime = animationTime;
			if (this.CurrentEmotionPreset == null)
			{
				this.SetEmotion(CS$<>8__locals1.preset);
				return;
			}
			if (this.emotionLerpRoutine != null)
			{
				base.StopCoroutine(this.emotionLerpRoutine);
			}
			this.emotionLerpRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<LerpEmotion>g__Routine|0());
		}

		// Token: 0x060043B8 RID: 17336 RVA: 0x0011C654 File Offset: 0x0011A854
		private void SetEmotion(AvatarEmotionPreset preset)
		{
			this.CurrentEmotionPreset = preset;
			this.Avatar.SetFaceTexture(preset.FaceTexture, Color.black);
			this.EyeController.SetLeftEyeRestingLidState(preset.LeftEyeRestingState);
			this.EyeController.SetRightEyeRestingLidState(preset.RightEyeRestingState);
			this.EyeController.LeftRestingEyeState = preset.LeftEyeRestingState;
			this.EyeController.RightRestingEyeState = preset.RightEyeRestingState;
			this.EyebrowController.SetLeftBrowRestingHeight(preset.BrowHeightChange_L);
			this.EyebrowController.SetRightBrowRestingHeight(preset.BrowHeightChange_R);
			this.EyebrowController.leftBrow.SetRestingAngle(preset.BrowAngleChange_L);
			this.EyebrowController.rightBrow.SetRestingAngle(preset.BrowAngleChange_R);
		}

		// Token: 0x060043B9 RID: 17337 RVA: 0x0011C710 File Offset: 0x0011A910
		public bool HasEmotion(string emotion)
		{
			return this.GetEmotion(emotion) != null;
		}

		// Token: 0x060043BA RID: 17338 RVA: 0x0011C71C File Offset: 0x0011A91C
		public AvatarEmotionPreset GetEmotion(string emotion)
		{
			return this.EmotionPresetList.Find((AvatarEmotionPreset x) => x.PresetName.ToLower() == emotion.ToLower());
		}

		// Token: 0x04003084 RID: 12420
		public const float MAX_UPDATE_DISTANCE = 30f;

		// Token: 0x04003087 RID: 12423
		[Header("Settings")]
		public List<AvatarEmotionPreset> EmotionPresetList = new List<AvatarEmotionPreset>();

		// Token: 0x04003088 RID: 12424
		[Header("References")]
		public Avatar Avatar;

		// Token: 0x04003089 RID: 12425
		public EyeController EyeController;

		// Token: 0x0400308A RID: 12426
		public EyebrowController EyebrowController;

		// Token: 0x0400308B RID: 12427
		private EmotionOverride activeEmotionOverride;

		// Token: 0x0400308C RID: 12428
		private List<EmotionOverride> overrideStack = new List<EmotionOverride>();

		// Token: 0x0400308D RID: 12429
		private AvatarEmotionPreset neutralPreset;

		// Token: 0x0400308E RID: 12430
		private Coroutine emotionLerpRoutine;

		// Token: 0x0400308F RID: 12431
		private Dictionary<string, Coroutine> emotionRemovalRoutines = new Dictionary<string, Coroutine>();

		// Token: 0x04003090 RID: 12432
		private int tempIndex;
	}
}
