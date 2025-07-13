using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.UI.Input;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x02000A94 RID: 2708
	public class TrashBagCanvas : Singleton<TrashBagCanvas>
	{
		// Token: 0x17000A2F RID: 2607
		// (get) Token: 0x060048BB RID: 18619 RVA: 0x001312F5 File Offset: 0x0012F4F5
		// (set) Token: 0x060048BC RID: 18620 RVA: 0x001312FD File Offset: 0x0012F4FD
		public bool IsOpen { get; private set; }

		// Token: 0x060048BD RID: 18621 RVA: 0x00131306 File Offset: 0x0012F506
		public void Open()
		{
			this.IsOpen = true;
			this.Canvas.enabled = true;
		}

		// Token: 0x060048BE RID: 18622 RVA: 0x0013131B File Offset: 0x0012F51B
		public void Close()
		{
			this.IsOpen = false;
			this.Canvas.enabled = false;
		}

		// Token: 0x04003566 RID: 13670
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x04003567 RID: 13671
		public InputPrompt InputPrompt;
	}
}
