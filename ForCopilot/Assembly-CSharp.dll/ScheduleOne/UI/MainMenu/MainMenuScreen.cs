using System;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.UI.MainMenu
{
	// Token: 0x02000B65 RID: 2917
	public class MainMenuScreen : MonoBehaviour
	{
		// Token: 0x17000AA9 RID: 2729
		// (get) Token: 0x06004D89 RID: 19849 RVA: 0x001465FF File Offset: 0x001447FF
		// (set) Token: 0x06004D8A RID: 19850 RVA: 0x00146607 File Offset: 0x00144807
		public bool IsOpen { get; protected set; }

		// Token: 0x06004D8B RID: 19851 RVA: 0x00146610 File Offset: 0x00144810
		protected virtual void Awake()
		{
			this.Rect = base.GetComponent<RectTransform>();
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), this.ExitInputPriority);
			if (this.OpenOnStart)
			{
				this.Group.alpha = 1f;
				this.Rect.localScale = new Vector3(1f, 1f, 1f);
				base.gameObject.SetActive(true);
				this.IsOpen = true;
			}
			else
			{
				this.Group.alpha = 0f;
				this.Rect.localScale = new Vector3(1.25f, 1.25f, 1.25f);
				base.gameObject.SetActive(false);
				this.IsOpen = false;
			}
			if (this.OpenOnStart)
			{
				Singleton<MusicPlayer>.Instance.SetTrackEnabled("Main Menu", true);
			}
		}

		// Token: 0x06004D8C RID: 19852 RVA: 0x001466E7 File Offset: 0x001448E7
		private void OnDestroy()
		{
			if (Singleton<MusicPlayer>.Instance != null)
			{
				Singleton<MusicPlayer>.Instance.SetTrackEnabled("Main Menu", false);
			}
		}

		// Token: 0x06004D8D RID: 19853 RVA: 0x00146706 File Offset: 0x00144906
		protected virtual void Exit(ExitAction action)
		{
			if (action.Used)
			{
				return;
			}
			if (action.exitType == ExitType.RightClick)
			{
				return;
			}
			if (this.PreviousScreen == null)
			{
				return;
			}
			if (this.IsOpen)
			{
				this.Close(true);
				action.Used = true;
			}
		}

		// Token: 0x06004D8E RID: 19854 RVA: 0x0014673F File Offset: 0x0014493F
		public virtual void Open(bool closePrevious)
		{
			this.IsOpen = true;
			this.Lerp(true);
			if (closePrevious && this.PreviousScreen != null)
			{
				this.PreviousScreen.Close(false);
			}
		}

		// Token: 0x06004D8F RID: 19855 RVA: 0x0014676C File Offset: 0x0014496C
		public virtual void Close(bool openPrevious)
		{
			this.IsOpen = false;
			this.Lerp(false);
			if (openPrevious && this.PreviousScreen != null)
			{
				this.PreviousScreen.Open(false);
			}
		}

		// Token: 0x06004D90 RID: 19856 RVA: 0x0014679C File Offset: 0x0014499C
		private void Lerp(bool open)
		{
			MainMenuScreen.<>c__DisplayClass17_0 CS$<>8__locals1 = new MainMenuScreen.<>c__DisplayClass17_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.open = open;
			if (this.lerpRoutine != null)
			{
				base.StopCoroutine(this.lerpRoutine);
			}
			if (CS$<>8__locals1.open)
			{
				base.gameObject.SetActive(true);
			}
			if (this.Rect == null)
			{
				this.Rect = base.GetComponent<RectTransform>();
			}
			this.lerpRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<Lerp>g__Routine|0());
		}

		// Token: 0x040039CC RID: 14796
		public const float LERP_TIME = 0.075f;

		// Token: 0x040039CD RID: 14797
		public const float LERP_SCALE = 1.25f;

		// Token: 0x040039CF RID: 14799
		[Header("Settings")]
		public int ExitInputPriority;

		// Token: 0x040039D0 RID: 14800
		public bool OpenOnStart;

		// Token: 0x040039D1 RID: 14801
		[Header("References")]
		public MainMenuScreen PreviousScreen;

		// Token: 0x040039D2 RID: 14802
		public CanvasGroup Group;

		// Token: 0x040039D3 RID: 14803
		private RectTransform Rect;

		// Token: 0x040039D4 RID: 14804
		private Coroutine lerpRoutine;
	}
}
