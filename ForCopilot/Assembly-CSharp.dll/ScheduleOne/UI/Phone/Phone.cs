using System;
using System.Collections;
using System.Collections.Generic;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.ScriptableObjects;
using ScheduleOne.Vision;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone
{
	// Token: 0x02000AE8 RID: 2792
	public class Phone : PlayerSingleton<Phone>
	{
		// Token: 0x17000A67 RID: 2663
		// (get) Token: 0x06004AE6 RID: 19174 RVA: 0x0013A988 File Offset: 0x00138B88
		// (set) Token: 0x06004AE7 RID: 19175 RVA: 0x0013A990 File Offset: 0x00138B90
		public bool IsOpen { get; protected set; }

		// Token: 0x17000A68 RID: 2664
		// (get) Token: 0x06004AE8 RID: 19176 RVA: 0x0013A999 File Offset: 0x00138B99
		// (set) Token: 0x06004AE9 RID: 19177 RVA: 0x0013A9A1 File Offset: 0x00138BA1
		public bool isHorizontal { get; protected set; }

		// Token: 0x17000A69 RID: 2665
		// (get) Token: 0x06004AEA RID: 19178 RVA: 0x0013A9AA File Offset: 0x00138BAA
		// (set) Token: 0x06004AEB RID: 19179 RVA: 0x0013A9B2 File Offset: 0x00138BB2
		public bool isOpenable { get; protected set; } = true;

		// Token: 0x17000A6A RID: 2666
		// (get) Token: 0x06004AEC RID: 19180 RVA: 0x0013A9BB File Offset: 0x00138BBB
		// (set) Token: 0x06004AED RID: 19181 RVA: 0x0013A9C3 File Offset: 0x00138BC3
		public bool FlashlightOn { get; protected set; }

		// Token: 0x17000A6B RID: 2667
		// (get) Token: 0x06004AEE RID: 19182 RVA: 0x0013A9CC File Offset: 0x00138BCC
		public float ScaledLookOffset
		{
			get
			{
				return Mathf.Lerp(this.LookOffsetMax, this.LookOffsetMin, CanvasScaler.NormalizedCanvasScaleFactor);
			}
		}

		// Token: 0x06004AEF RID: 19183 RVA: 0x0013A9E4 File Offset: 0x00138BE4
		protected override void Awake()
		{
			base.Awake();
			this.eventSystem = EventSystem.current;
		}

		// Token: 0x06004AF0 RID: 19184 RVA: 0x0013A9F7 File Offset: 0x00138BF7
		public override void OnStartClient(bool IsOwner)
		{
			base.OnStartClient(IsOwner);
			if (!IsOwner)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x06004AF1 RID: 19185 RVA: 0x0013AA10 File Offset: 0x00138C10
		protected override void Start()
		{
			base.Start();
			if (this.flashlightVisibility == null)
			{
				this.flashlightVisibility = new VisibilityAttribute("Flashlight", 0f, 1f, -1);
			}
			base.transform.localRotation = this.orientation_Vertical.localRotation;
		}

		// Token: 0x06004AF2 RID: 19186 RVA: 0x0013AA5C File Offset: 0x00138C5C
		protected virtual void Update()
		{
			if (this.IsOpen)
			{
				Singleton<HUD>.Instance.OnlineBalanceDisplay.Show();
			}
			if (!GameInput.IsTyping && !Singleton<PauseMenu>.Instance.IsPaused && (PlayerSingleton<PlayerCamera>.Instance.activeUIElementCount == 0 || this.IsOpen) && GameInput.GetButtonDown(GameInput.ButtonCode.ToggleFlashlight))
			{
				this.ToggleFlashlight();
			}
		}

		// Token: 0x06004AF3 RID: 19187 RVA: 0x0013AAB6 File Offset: 0x00138CB6
		protected override void OnDestroy()
		{
			base.OnDestroy();
			Phone.ActiveApp = null;
		}

		// Token: 0x06004AF4 RID: 19188 RVA: 0x0013AAC4 File Offset: 0x00138CC4
		private void ToggleFlashlight()
		{
			this.FlashlightOn = !this.FlashlightOn;
			this.PhoneFlashlight.SetActive(this.FlashlightOn);
			this.FlashlightToggleSound.PitchMultiplier = (this.FlashlightOn ? 1f : 0.9f);
			this.FlashlightToggleSound.Play();
			this.flashlightVisibility.pointsChange = (this.FlashlightOn ? 10f : 0f);
			this.flashlightVisibility.multiplier = (this.FlashlightOn ? 1.5f : 1f);
			Player.Local.SendFlashlightOn(this.FlashlightOn);
		}

		// Token: 0x06004AF5 RID: 19189 RVA: 0x0013AB69 File Offset: 0x00138D69
		public void SetOpenable(bool o)
		{
			this.isOpenable = o;
		}

		// Token: 0x06004AF6 RID: 19190 RVA: 0x0013AB74 File Offset: 0x00138D74
		public void SetIsOpen(bool o)
		{
			this.IsOpen = o;
			if (this.IsOpen)
			{
				if (this.onPhoneOpened != null)
				{
					this.onPhoneOpened();
				}
				if (Phone.ActiveApp == null)
				{
					this.SetLookOffsetMultiplier(1f);
					return;
				}
			}
			else if (this.onPhoneClosed != null)
			{
				this.onPhoneClosed();
			}
		}

		// Token: 0x06004AF7 RID: 19191 RVA: 0x0013ABCF File Offset: 0x00138DCF
		public void SetIsHorizontal(bool h)
		{
			this.isHorizontal = h;
			if (this.rotationCoroutine != null)
			{
				base.StopCoroutine(this.rotationCoroutine);
			}
			this.rotationCoroutine = base.StartCoroutine(this.SetIsHorizontal_Process(h));
		}

		// Token: 0x06004AF8 RID: 19192 RVA: 0x0013ABFF File Offset: 0x00138DFF
		protected IEnumerator SetIsHorizontal_Process(bool h)
		{
			float adjustedRotationTime = this.rotationTime;
			Quaternion startRotation = base.transform.localRotation;
			Quaternion endRotation = Quaternion.identity;
			if (h)
			{
				endRotation = this.orientation_Horizontal.localRotation;
				adjustedRotationTime *= Quaternion.Angle(base.transform.localRotation, this.orientation_Horizontal.localRotation) / 90f;
			}
			else
			{
				endRotation = this.orientation_Vertical.localRotation;
				adjustedRotationTime *= Quaternion.Angle(base.transform.localRotation, this.orientation_Vertical.localRotation) / 90f;
			}
			for (float i = 0f; i < adjustedRotationTime; i += Time.deltaTime)
			{
				base.transform.localRotation = Quaternion.Lerp(startRotation, endRotation, i / adjustedRotationTime);
				yield return new WaitForEndOfFrame();
			}
			base.transform.localRotation = endRotation;
			this.rotationCoroutine = null;
			yield break;
		}

		// Token: 0x06004AF9 RID: 19193 RVA: 0x0013AC18 File Offset: 0x00138E18
		public void SetLookOffsetMultiplier(float multiplier)
		{
			float lookOffset_Process = this.ScaledLookOffset * multiplier;
			if (this.lookOffsetCoroutine != null)
			{
				base.StopCoroutine(this.lookOffsetCoroutine);
			}
			this.lookOffsetCoroutine = base.StartCoroutine(this.SetLookOffset_Process(lookOffset_Process));
		}

		// Token: 0x06004AFA RID: 19194 RVA: 0x0013AC55 File Offset: 0x00138E55
		public void RequestCloseApp()
		{
			if (Phone.ActiveApp != null && this.closeApps != null)
			{
				this.closeApps();
			}
		}

		// Token: 0x06004AFB RID: 19195 RVA: 0x0013AC77 File Offset: 0x00138E77
		protected IEnumerator SetLookOffset_Process(float lookOffset)
		{
			float startOffset = base.transform.localPosition.z;
			float moveTime = 0.1f;
			for (float i = 0f; i < moveTime; i += Time.deltaTime)
			{
				base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, Mathf.Lerp(startOffset, lookOffset, i / moveTime));
				yield return new WaitForEndOfFrame();
			}
			base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, lookOffset);
			this.rotationCoroutine = null;
			yield break;
		}

		// Token: 0x06004AFC RID: 19196 RVA: 0x0013AC90 File Offset: 0x00138E90
		public bool MouseRaycast(out RaycastResult result)
		{
			PointerEventData pointerEventData = new PointerEventData(this.eventSystem);
			pointerEventData.position = Input.mousePosition;
			List<RaycastResult> list = new List<RaycastResult>();
			this.raycaster.Raycast(pointerEventData, list);
			if (list.Count > 0)
			{
				result = list[0];
			}
			else
			{
				result = default(RaycastResult);
			}
			return list.Count > 0;
		}

		// Token: 0x0400373D RID: 14141
		public static GameObject ActiveApp;

		// Token: 0x04003742 RID: 14146
		public PhoneCallData testData;

		// Token: 0x04003743 RID: 14147
		public CallerID testCalller;

		// Token: 0x04003744 RID: 14148
		[Header("References")]
		[SerializeField]
		protected GameObject phoneModel;

		// Token: 0x04003745 RID: 14149
		[SerializeField]
		protected Transform orientation_Vertical;

		// Token: 0x04003746 RID: 14150
		[SerializeField]
		protected Transform orientation_Horizontal;

		// Token: 0x04003747 RID: 14151
		[SerializeField]
		protected GraphicRaycaster raycaster;

		// Token: 0x04003748 RID: 14152
		[SerializeField]
		protected GameObject PhoneFlashlight;

		// Token: 0x04003749 RID: 14153
		[SerializeField]
		protected AudioSourceController FlashlightToggleSound;

		// Token: 0x0400374A RID: 14154
		[Header("Settings")]
		public float rotationTime = 0.1f;

		// Token: 0x0400374B RID: 14155
		public float LookOffsetMax = 0.45f;

		// Token: 0x0400374C RID: 14156
		public float LookOffsetMin = 0.29f;

		// Token: 0x0400374D RID: 14157
		public float OpenVerticalOffset = 0.1f;

		// Token: 0x0400374E RID: 14158
		public Action onPhoneOpened;

		// Token: 0x0400374F RID: 14159
		public Action onPhoneClosed;

		// Token: 0x04003750 RID: 14160
		public Action closeApps;

		// Token: 0x04003751 RID: 14161
		private EventSystem eventSystem;

		// Token: 0x04003752 RID: 14162
		private VisibilityAttribute flashlightVisibility;

		// Token: 0x04003753 RID: 14163
		private Coroutine rotationCoroutine;

		// Token: 0x04003754 RID: 14164
		private Coroutine lookOffsetCoroutine;
	}
}
