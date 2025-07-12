using System;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B1F RID: 2847
	public class ClipboardScreen : MonoBehaviour
	{
		// Token: 0x17000A86 RID: 2694
		// (get) Token: 0x06004C23 RID: 19491 RVA: 0x001405C3 File Offset: 0x0013E7C3
		// (set) Token: 0x06004C24 RID: 19492 RVA: 0x001405CB File Offset: 0x0013E7CB
		public bool IsOpen { get; protected set; }

		// Token: 0x06004C25 RID: 19493 RVA: 0x001405D4 File Offset: 0x0013E7D4
		protected virtual void Start()
		{
			if (this.OpenOnStart)
			{
				this.IsOpen = true;
				this.Container.anchoredPosition = new Vector2(0f, this.Container.anchoredPosition.y);
			}
			else
			{
				this.IsOpen = false;
				this.Container.anchoredPosition = new Vector2(this.ClosedOffset, this.Container.anchoredPosition.y);
				this.Container.gameObject.SetActive(false);
			}
			if (this.UseExitListener)
			{
				GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), this.ExitActionPriority);
			}
		}

		// Token: 0x06004C26 RID: 19494 RVA: 0x00140674 File Offset: 0x0013E874
		private void Exit(ExitAction exitAction)
		{
			if (!this.IsOpen)
			{
				return;
			}
			if (exitAction.Used)
			{
				return;
			}
			exitAction.Used = true;
			this.Close();
		}

		// Token: 0x06004C27 RID: 19495 RVA: 0x00140695 File Offset: 0x0013E895
		public virtual void Open()
		{
			this.Container.gameObject.SetActive(true);
			this.IsOpen = true;
			this.Lerp(true, null);
		}

		// Token: 0x06004C28 RID: 19496 RVA: 0x001406B7 File Offset: 0x0013E8B7
		public virtual void Close()
		{
			this.IsOpen = false;
			this.Lerp(false, delegate
			{
				this.Container.gameObject.SetActive(false);
			});
		}

		// Token: 0x06004C29 RID: 19497 RVA: 0x001406D4 File Offset: 0x0013E8D4
		private void Lerp(bool open, Action callback)
		{
			ClipboardScreen.<>c__DisplayClass14_0 CS$<>8__locals1 = new ClipboardScreen.<>c__DisplayClass14_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.open = open;
			CS$<>8__locals1.callback = callback;
			if (this.lerpRoutine != null)
			{
				Singleton<CoroutineService>.Instance.StopCoroutine(this.lerpRoutine);
			}
			this.lerpRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<Lerp>g__Routine|0());
		}

		// Token: 0x040038B6 RID: 14518
		[Header("References")]
		public RectTransform Container;

		// Token: 0x040038B7 RID: 14519
		[Header("Settings")]
		public float ClosedOffset = 420f;

		// Token: 0x040038B8 RID: 14520
		public bool OpenOnStart;

		// Token: 0x040038B9 RID: 14521
		public bool UseExitListener = true;

		// Token: 0x040038BA RID: 14522
		public int ExitActionPriority = 10;

		// Token: 0x040038BB RID: 14523
		private Coroutine lerpRoutine;
	}
}
