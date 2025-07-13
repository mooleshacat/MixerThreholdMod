using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.PlayerScripts
{
	// Token: 0x0200060F RID: 1551
	public class CursorManager : Singleton<CursorManager>
	{
		// Token: 0x060025FE RID: 9726 RVA: 0x000998DE File Offset: 0x00097ADE
		protected override void Awake()
		{
			base.Awake();
			this.SetCursorAppearance(CursorManager.ECursorType.Default);
		}

		// Token: 0x060025FF RID: 9727 RVA: 0x000998F0 File Offset: 0x00097AF0
		public void SetCursorAppearance(CursorManager.ECursorType type)
		{
			CursorManager.CursorConfig cursorConfig = this.Cursors.Find((CursorManager.CursorConfig x) => x.CursorType == type);
			Cursor.SetCursor(cursorConfig.Texture, cursorConfig.HotSpot, CursorMode.Auto);
		}

		// Token: 0x04001C0C RID: 7180
		[Header("References")]
		public List<CursorManager.CursorConfig> Cursors = new List<CursorManager.CursorConfig>();

		// Token: 0x02000610 RID: 1552
		public enum ECursorType
		{
			// Token: 0x04001C0E RID: 7182
			Default,
			// Token: 0x04001C0F RID: 7183
			Finger,
			// Token: 0x04001C10 RID: 7184
			OpenHand,
			// Token: 0x04001C11 RID: 7185
			Grab,
			// Token: 0x04001C12 RID: 7186
			Scissors
		}

		// Token: 0x02000611 RID: 1553
		[Serializable]
		public class CursorConfig
		{
			// Token: 0x04001C13 RID: 7187
			public CursorManager.ECursorType CursorType;

			// Token: 0x04001C14 RID: 7188
			public Texture2D Texture;

			// Token: 0x04001C15 RID: 7189
			public Vector2 HotSpot;
		}
	}
}
