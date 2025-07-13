using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.TV;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x020009FA RID: 2554
	public class TVPauseScreen : MonoBehaviour
	{
		// Token: 0x17000996 RID: 2454
		// (get) Token: 0x060044B5 RID: 17589 RVA: 0x001207B7 File Offset: 0x0011E9B7
		// (set) Token: 0x060044B6 RID: 17590 RVA: 0x001207BF File Offset: 0x0011E9BF
		public bool IsPaused { get; private set; }

		// Token: 0x060044B7 RID: 17591 RVA: 0x001207C8 File Offset: 0x0011E9C8
		private void Awake()
		{
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 4);
		}

		// Token: 0x060044B8 RID: 17592 RVA: 0x001207DC File Offset: 0x0011E9DC
		private void Exit(ExitAction action)
		{
			if (action.Used)
			{
				return;
			}
			if (!this.IsPaused)
			{
				return;
			}
			if (!this.App.IsOpen)
			{
				return;
			}
			action.Used = true;
			this.Back();
		}

		// Token: 0x060044B9 RID: 17593 RVA: 0x0012080B File Offset: 0x0011EA0B
		public void Pause()
		{
			this.IsPaused = true;
			base.gameObject.SetActive(true);
		}

		// Token: 0x060044BA RID: 17594 RVA: 0x00120820 File Offset: 0x0011EA20
		public void Resume()
		{
			this.IsPaused = false;
			base.gameObject.SetActive(false);
			this.App.Resume();
		}

		// Token: 0x060044BB RID: 17595 RVA: 0x00120840 File Offset: 0x0011EA40
		public void Back()
		{
			this.App.Close();
		}

		// Token: 0x04003191 RID: 12689
		public TVApp App;
	}
}
