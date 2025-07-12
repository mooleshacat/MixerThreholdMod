using System;
using System.Collections;
using EasyButtons;
using ScheduleOne.DevUtilities;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.AvatarFramework
{
	// Token: 0x020009AC RID: 2476
	[ExecuteInEditMode]
	public class EyeController : MonoBehaviour
	{
		// Token: 0x1700096B RID: 2411
		// (get) Token: 0x06004322 RID: 17186 RVA: 0x0011A875 File Offset: 0x00118A75
		// (set) Token: 0x06004323 RID: 17187 RVA: 0x0011A87D File Offset: 0x00118A7D
		public bool EyesOpen { get; protected set; } = true;

		// Token: 0x06004324 RID: 17188 RVA: 0x0011A886 File Offset: 0x00118A86
		protected virtual void Awake()
		{
			this.avatar = base.GetComponentInParent<Avatar>();
			this.avatar.onRagdollChange.AddListener(new UnityAction<bool, bool, bool>(this.RagdollChange));
			this.SetEyesOpen(true);
			this.ApplyDilation();
		}

		// Token: 0x06004325 RID: 17189 RVA: 0x0011A8C0 File Offset: 0x00118AC0
		protected void Update()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if (this.BlinkingEnabled && this.blinkRoutine == null)
			{
				this.blinkRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(this.BlinkRoutine());
			}
			if (this.BlinkingEnabled)
			{
				this.timeUntilNextBlink -= Time.deltaTime;
			}
		}

		// Token: 0x06004326 RID: 17190 RVA: 0x0011A915 File Offset: 0x00118B15
		private void OnEnable()
		{
			this.ApplyRestingEyeLidState();
		}

		// Token: 0x06004327 RID: 17191 RVA: 0x0011A920 File Offset: 0x00118B20
		[Button]
		public void ApplySettings()
		{
			this.leftEye.transform.localEulerAngles = new Vector3(0f, -this.eyeSpacing, 0f);
			this.rightEye.transform.localEulerAngles = new Vector3(0f, this.eyeSpacing, 0f);
			this.rightEye.transform.localPosition = new Vector3(0f, this.eyeHeight * EyeController.eyeHeightMultiplier, 0f);
			this.leftEye.transform.localPosition = new Vector3(0f, this.eyeHeight * EyeController.eyeHeightMultiplier, 0f);
			this.leftEye.SetSize(this.eyeSize);
			this.rightEye.SetSize(this.eyeSize);
			this.leftEye.SetLidColor(this.leftEyeLidColor);
			this.rightEye.SetLidColor(this.rightEyeLidColor);
			this.leftEye.SetEyeballMaterial(this.eyeBallMaterial, this.eyeBallColor);
			this.rightEye.SetEyeballMaterial(this.eyeBallMaterial, this.eyeBallColor);
			this.ApplyDilation();
			this.ApplyRestingEyeLidState();
		}

		// Token: 0x06004328 RID: 17192 RVA: 0x0011AA4C File Offset: 0x00118C4C
		public void SetEyeballTint(Color col)
		{
			this.leftEye.SetEyeballColor(col, 0.115f, true);
			this.rightEye.SetEyeballColor(col, 0.115f, true);
		}

		// Token: 0x06004329 RID: 17193 RVA: 0x0011AA72 File Offset: 0x00118C72
		public void OverrideEyeballTint(Color col)
		{
			this.leftEye.SetEyeballColor(col, 0.115f, true);
			this.rightEye.SetEyeballColor(col, 0.115f, true);
			this.eyeBallTintOverridden = true;
		}

		// Token: 0x0600432A RID: 17194 RVA: 0x0011AA9F File Offset: 0x00118C9F
		public void ResetEyeballTint()
		{
			this.leftEye.SetEyeballColor(this.eyeBallColor, 0.115f, true);
			this.rightEye.SetEyeballColor(this.eyeBallColor, 0.115f, true);
			this.eyeBallTintOverridden = false;
		}

		// Token: 0x0600432B RID: 17195 RVA: 0x0011AAD6 File Offset: 0x00118CD6
		public void OverrideEyeLids(Eye.EyeLidConfiguration eyeLidConfiguration)
		{
			if (!this.eyeLidOverridden)
			{
				this.defaultLeftEyeRestingState = this.LeftRestingEyeState;
				this.defaultRightEyeRestingState = this.RightRestingEyeState;
			}
			this.LeftRestingEyeState = eyeLidConfiguration;
			this.RightRestingEyeState = eyeLidConfiguration;
			this.eyeLidOverridden = true;
		}

		// Token: 0x0600432C RID: 17196 RVA: 0x0011AB0D File Offset: 0x00118D0D
		public void ResetEyeLids()
		{
			this.LeftRestingEyeState = this.defaultLeftEyeRestingState;
			this.RightRestingEyeState = this.defaultRightEyeRestingState;
			this.eyeLidOverridden = false;
		}

		// Token: 0x0600432D RID: 17197 RVA: 0x0011AB2E File Offset: 0x00118D2E
		private void RagdollChange(bool oldValue, bool newValue, bool playStandUpAnim)
		{
			if (newValue)
			{
				this.ForceBlink();
			}
		}

		// Token: 0x0600432E RID: 17198 RVA: 0x0011AB3C File Offset: 0x00118D3C
		public void SetEyesOpen(bool open)
		{
			if (this.DEBUG)
			{
				Debug.Log("Setting eyes open: " + open.ToString());
			}
			this.EyesOpen = open;
			this.leftEye.SetEyeLidState(open ? this.LeftRestingEyeState : new Eye.EyeLidConfiguration
			{
				bottomLidOpen = 0f,
				topLidOpen = 0f
			}, 0.1f);
			this.rightEye.SetEyeLidState(open ? this.RightRestingEyeState : new Eye.EyeLidConfiguration
			{
				bottomLidOpen = 0f,
				topLidOpen = 0f
			}, 0.1f);
		}

		// Token: 0x0600432F RID: 17199 RVA: 0x0011ABE6 File Offset: 0x00118DE6
		private void ApplyDilation()
		{
			this.leftEye.SetDilation(this.PupilDilation);
			this.rightEye.SetDilation(this.PupilDilation);
		}

		// Token: 0x06004330 RID: 17200 RVA: 0x0011AC0A File Offset: 0x00118E0A
		public void SetPupilDilation(float dilation, bool writeDefault = true)
		{
			this.PupilDilation = dilation;
			this.ApplyDilation();
			this.defaultDilation = this.PupilDilation;
		}

		// Token: 0x06004331 RID: 17201 RVA: 0x0011AC25 File Offset: 0x00118E25
		public void ResetPupilDilation()
		{
			this.SetPupilDilation(this.defaultDilation, true);
		}

		// Token: 0x06004332 RID: 17202 RVA: 0x0011AC34 File Offset: 0x00118E34
		private void ApplyRestingEyeLidState()
		{
			this.leftEye.SetEyeLidState(this.LeftRestingEyeState, false);
			this.rightEye.SetEyeLidState(this.RightRestingEyeState, false);
		}

		// Token: 0x06004333 RID: 17203 RVA: 0x0011AC5A File Offset: 0x00118E5A
		public void ForceBlink()
		{
			this.leftEye.Blink(this.blinkDuration, this.LeftRestingEyeState, false);
			this.rightEye.Blink(this.blinkDuration, this.RightRestingEyeState, false);
			this.ResetBlinkCounter();
		}

		// Token: 0x06004334 RID: 17204 RVA: 0x0011AC92 File Offset: 0x00118E92
		public void SetLeftEyeRestingLidState(Eye.EyeLidConfiguration config)
		{
			this.LeftRestingEyeState = config;
			if (!this.leftEye.IsBlinking)
			{
				this.leftEye.SetEyeLidState(config, false);
			}
		}

		// Token: 0x06004335 RID: 17205 RVA: 0x0011ACB5 File Offset: 0x00118EB5
		public void SetRightEyeRestingLidState(Eye.EyeLidConfiguration config)
		{
			this.RightRestingEyeState = config;
			if (!this.rightEye.IsBlinking)
			{
				this.rightEye.SetEyeLidState(config, false);
			}
		}

		// Token: 0x06004336 RID: 17206 RVA: 0x0011ACD8 File Offset: 0x00118ED8
		private IEnumerator BlinkRoutine()
		{
			while (this.BlinkingEnabled)
			{
				if (this.EyesOpen)
				{
					if (this.DEBUG)
					{
						Debug.Log("Blinking");
					}
					this.leftEye.Blink(this.blinkDuration, this.LeftRestingEyeState, this.DEBUG);
					this.rightEye.Blink(this.blinkDuration, this.RightRestingEyeState, this.DEBUG);
				}
				this.ResetBlinkCounter();
				yield return new WaitUntil(() => this.timeUntilNextBlink <= 0f);
			}
			this.blinkRoutine = null;
			yield break;
		}

		// Token: 0x06004337 RID: 17207 RVA: 0x0011ACE7 File Offset: 0x00118EE7
		private void ResetBlinkCounter()
		{
			this.timeUntilNextBlink = UnityEngine.Random.Range(Mathf.Clamp(this.blinkInterval - this.blinkIntervalSpread, this.blinkDuration, float.MaxValue), this.blinkInterval + this.blinkIntervalSpread);
		}

		// Token: 0x06004338 RID: 17208 RVA: 0x0011AD1E File Offset: 0x00118F1E
		public void LookAt(Vector3 position, bool instant = false)
		{
			bool debug = this.DEBUG;
			this.leftEye.LookAt(position, instant);
			this.rightEye.LookAt(position, instant);
		}

		// Token: 0x04002FE8 RID: 12264
		private static float eyeHeightMultiplier = 0.03f;

		// Token: 0x04002FE9 RID: 12265
		public bool DEBUG;

		// Token: 0x04002FEB RID: 12267
		[Header("References")]
		[SerializeField]
		public Eye leftEye;

		// Token: 0x04002FEC RID: 12268
		[SerializeField]
		public Eye rightEye;

		// Token: 0x04002FED RID: 12269
		[Header("Location Settings")]
		[Range(0f, 45f)]
		[SerializeField]
		protected float eyeSpacing = 20f;

		// Token: 0x04002FEE RID: 12270
		[Range(-1f, 1f)]
		[SerializeField]
		protected float eyeHeight;

		// Token: 0x04002FEF RID: 12271
		[Range(0.5f, 1.5f)]
		[SerializeField]
		protected float eyeSize = 1f;

		// Token: 0x04002FF0 RID: 12272
		[Header("Eyelid Settings")]
		[SerializeField]
		protected Color leftEyeLidColor = Color.white;

		// Token: 0x04002FF1 RID: 12273
		[SerializeField]
		protected Color rightEyeLidColor = Color.white;

		// Token: 0x04002FF2 RID: 12274
		public Eye.EyeLidConfiguration LeftRestingEyeState;

		// Token: 0x04002FF3 RID: 12275
		public Eye.EyeLidConfiguration RightRestingEyeState;

		// Token: 0x04002FF4 RID: 12276
		[Header("Eyeball Settings")]
		[SerializeField]
		protected Material eyeBallMaterial;

		// Token: 0x04002FF5 RID: 12277
		[SerializeField]
		protected Color eyeBallColor;

		// Token: 0x04002FF6 RID: 12278
		[Header("Pupil State")]
		[Range(0f, 1f)]
		public float PupilDilation = 0.5f;

		// Token: 0x04002FF7 RID: 12279
		[Header("Blinking Settings")]
		public bool BlinkingEnabled = true;

		// Token: 0x04002FF8 RID: 12280
		[SerializeField]
		[Range(0f, 10f)]
		protected float blinkInterval = 3.5f;

		// Token: 0x04002FF9 RID: 12281
		[SerializeField]
		[Range(0f, 2f)]
		protected float blinkIntervalSpread = 0.5f;

		// Token: 0x04002FFA RID: 12282
		[SerializeField]
		[Range(0f, 1f)]
		protected float blinkDuration = 0.2f;

		// Token: 0x04002FFB RID: 12283
		private Avatar avatar;

		// Token: 0x04002FFC RID: 12284
		private Coroutine blinkRoutine;

		// Token: 0x04002FFD RID: 12285
		private float timeUntilNextBlink;

		// Token: 0x04002FFE RID: 12286
		private bool eyeBallTintOverridden;

		// Token: 0x04002FFF RID: 12287
		private bool eyeLidOverridden;

		// Token: 0x04003000 RID: 12288
		private Eye.EyeLidConfiguration defaultLeftEyeRestingState;

		// Token: 0x04003001 RID: 12289
		private Eye.EyeLidConfiguration defaultRightEyeRestingState;

		// Token: 0x04003002 RID: 12290
		private float defaultDilation = 0.5f;
	}
}
