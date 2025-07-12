using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone
{
	// Token: 0x02000AE3 RID: 2787
	public class CallNotification : Singleton<CallNotification>
	{
		// Token: 0x17000A60 RID: 2656
		// (get) Token: 0x06004AC3 RID: 19139 RVA: 0x0013A2B8 File Offset: 0x001384B8
		// (set) Token: 0x06004AC4 RID: 19140 RVA: 0x0013A2C0 File Offset: 0x001384C0
		public PhoneCallData ActiveCallData { get; private set; }

		// Token: 0x17000A61 RID: 2657
		// (get) Token: 0x06004AC5 RID: 19141 RVA: 0x0013A2C9 File Offset: 0x001384C9
		// (set) Token: 0x06004AC6 RID: 19142 RVA: 0x0013A2D1 File Offset: 0x001384D1
		public bool IsOpen { get; protected set; }

		// Token: 0x06004AC7 RID: 19143 RVA: 0x0013A2DC File Offset: 0x001384DC
		protected override void Awake()
		{
			base.Awake();
			this.Group.alpha = 0f;
			this.Container.anchoredPosition = new Vector2(-600f, 0f);
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x06004AC8 RID: 19144 RVA: 0x0013A32C File Offset: 0x0013852C
		public void SetIsOpen(bool visible, CallerID caller)
		{
			CallNotification.<>c__DisplayClass14_0 CS$<>8__locals1 = new CallNotification.<>c__DisplayClass14_0();
			CS$<>8__locals1.visible = visible;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.caller = caller;
			this.IsOpen = CS$<>8__locals1.visible;
			if (this.slideRoutine != null)
			{
				base.StopCoroutine(this.slideRoutine);
			}
			this.slideRoutine = base.StartCoroutine(CS$<>8__locals1.<SetIsOpen>g__Routine|0());
		}

		// Token: 0x0400371E RID: 14110
		public const float TIME_PER_CHAR = 0.015f;

		// Token: 0x04003721 RID: 14113
		[Header("References")]
		public RectTransform Container;

		// Token: 0x04003722 RID: 14114
		public Image ProfilePicture;

		// Token: 0x04003723 RID: 14115
		public CanvasGroup Group;

		// Token: 0x04003724 RID: 14116
		private Coroutine slideRoutine;
	}
}
