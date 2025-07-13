using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.TV
{
	// Token: 0x020002B0 RID: 688
	public class TVApp : MonoBehaviour
	{
		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000E63 RID: 3683 RVA: 0x0003FEB4 File Offset: 0x0003E0B4
		// (set) Token: 0x06000E64 RID: 3684 RVA: 0x0003FEBC File Offset: 0x0003E0BC
		public bool IsOpen { get; private set; }

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000E65 RID: 3685 RVA: 0x0003FEC5 File Offset: 0x0003E0C5
		public bool IsPaused
		{
			get
			{
				return this.PauseScreen != null && this.PauseScreen.IsPaused;
			}
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x0003FEE2 File Offset: 0x0003E0E2
		protected virtual void Awake()
		{
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 3);
			this.CanvasGroup.alpha = 0f;
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x0003FF06 File Offset: 0x0003E106
		private void OnDestroy()
		{
			GameInput.DeregisterExitListener(new GameInput.ExitDelegate(this.Exit));
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x0003FF1C File Offset: 0x0003E11C
		public virtual void Open()
		{
			this.IsOpen = true;
			this.Canvas.gameObject.SetActive(true);
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.ActiveMinPass));
			TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
			instance2.onMinutePass = (Action)Delegate.Combine(instance2.onMinutePass, new Action(this.ActiveMinPass));
			this.Lerp(1f, 1f);
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x0003FFA0 File Offset: 0x0003E1A0
		public virtual void Close()
		{
			this.IsOpen = false;
			this.Canvas.gameObject.SetActive(false);
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.ActiveMinPass));
			if (this.PreviousScreen != null)
			{
				this.Lerp(0.67f, 0f);
			}
			else
			{
				this.Lerp(1.5f, 0f);
			}
			if (this.PreviousScreen != null)
			{
				this.PreviousScreen.Open();
			}
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void Resume()
		{
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x00040035 File Offset: 0x0003E235
		private void Lerp(float endScale, float endAlpha)
		{
			if (this.lerpCoroutine != null)
			{
				Singleton<CoroutineService>.Instance.StopCoroutine(this.lerpCoroutine);
			}
			this.lerpCoroutine = Singleton<CoroutineService>.Instance.StartCoroutine(this.<Lerp>g__Lerp|23_0(endScale, endAlpha));
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void ActiveMinPass()
		{
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x00040068 File Offset: 0x0003E268
		private void Exit(ExitAction action)
		{
			if (action.Used)
			{
				return;
			}
			if (!this.IsOpen)
			{
				return;
			}
			if (!this.CanClose && !this.Pauseable)
			{
				this.PreviousScreen.Open();
				return;
			}
			action.Used = true;
			if (this.Pauseable && this.PauseScreen != null)
			{
				this.TryPause();
				return;
			}
			this.Close();
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x000400CD File Offset: 0x0003E2CD
		protected virtual void TryPause()
		{
			this.PauseScreen.Pause();
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x000400F0 File Offset: 0x0003E2F0
		[CompilerGenerated]
		private IEnumerator <Lerp>g__Lerp|23_0(float endScale, float endAlpha)
		{
			if (this.Canvas == null)
			{
				yield break;
			}
			this.Canvas.gameObject.SetActive(true);
			float startScale = this.Canvas.transform.localScale.x;
			float startAlpha = this.CanvasGroup.alpha;
			float lerpTime = Mathf.Abs(endScale - startScale) / 0.5f * 0.12f;
			for (float i = 0f; i < lerpTime; i += Time.deltaTime)
			{
				if (this.Canvas == null)
				{
					yield break;
				}
				this.Canvas.transform.localScale = Vector3.one * Mathf.Lerp(startScale, endScale, i / lerpTime);
				this.CanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, i / lerpTime);
				yield return new WaitForEndOfFrame();
			}
			if (this.Canvas != null)
			{
				this.Canvas.transform.localScale = Vector3.one * endScale;
				this.CanvasGroup.alpha = endAlpha;
				if (endAlpha == 0f)
				{
					this.Canvas.gameObject.SetActive(false);
				}
			}
			this.lerpCoroutine = null;
			yield break;
		}

		// Token: 0x04000EE3 RID: 3811
		public const float SCALE_MIN = 0.67f;

		// Token: 0x04000EE4 RID: 3812
		public const float SCALE_MAX = 1.5f;

		// Token: 0x04000EE5 RID: 3813
		public const float LERP_TIME = 0.12f;

		// Token: 0x04000EE7 RID: 3815
		[Header("Settings")]
		public bool CanClose = true;

		// Token: 0x04000EE8 RID: 3816
		public string AppName;

		// Token: 0x04000EE9 RID: 3817
		public Sprite Icon;

		// Token: 0x04000EEA RID: 3818
		public bool Pauseable = true;

		// Token: 0x04000EEB RID: 3819
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x04000EEC RID: 3820
		[HideInInspector]
		public TVApp PreviousScreen;

		// Token: 0x04000EED RID: 3821
		public CanvasGroup CanvasGroup;

		// Token: 0x04000EEE RID: 3822
		public TVPauseScreen PauseScreen;

		// Token: 0x04000EEF RID: 3823
		private Coroutine lerpCoroutine;
	}
}
