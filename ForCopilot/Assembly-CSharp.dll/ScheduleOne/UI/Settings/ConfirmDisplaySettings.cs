using System;
using ScheduleOne.DevUtilities;
using TMPro;
using UnityEngine;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000AB6 RID: 2742
	public class ConfirmDisplaySettings : MonoBehaviour
	{
		// Token: 0x17000A4E RID: 2638
		// (get) Token: 0x060049BB RID: 18875 RVA: 0x001365C0 File Offset: 0x001347C0
		public bool IsOpen
		{
			get
			{
				return this != null && base.gameObject != null && base.gameObject.activeSelf;
			}
		}

		// Token: 0x060049BC RID: 18876 RVA: 0x001365E6 File Offset: 0x001347E6
		public void Awake()
		{
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 6);
			base.gameObject.SetActive(false);
		}

		// Token: 0x060049BD RID: 18877 RVA: 0x00136606 File Offset: 0x00134806
		public void Open(DisplaySettings _oldSettings, DisplaySettings _newSettings)
		{
			base.gameObject.SetActive(true);
			this.oldSettings = _oldSettings;
			this.newSettings = _newSettings;
			this.timeUntilRevert = 15f;
			this.Update();
		}

		// Token: 0x060049BE RID: 18878 RVA: 0x00136633 File Offset: 0x00134833
		public void Exit(ExitAction action)
		{
			if (action.Used)
			{
				return;
			}
			if (!this.IsOpen)
			{
				return;
			}
			if (action.exitType == ExitType.Escape)
			{
				action.Used = true;
				this.Close(true);
			}
		}

		// Token: 0x060049BF RID: 18879 RVA: 0x00136660 File Offset: 0x00134860
		public void Update()
		{
			this.timeUntilRevert -= Time.unscaledDeltaTime;
			this.SubtitleLabel.text = string.Format("Reverting in {0:0.0} seconds", this.timeUntilRevert);
			if (this.timeUntilRevert <= 0f)
			{
				this.Close(true);
			}
		}

		// Token: 0x060049C0 RID: 18880 RVA: 0x001366B4 File Offset: 0x001348B4
		public void Close(bool revert)
		{
			if (revert)
			{
				Singleton<Settings>.Instance.ApplyDisplaySettings(this.oldSettings);
				Singleton<Settings>.Instance.DisplaySettings = this.oldSettings;
				Singleton<Settings>.Instance.UnappliedDisplaySettings = this.oldSettings;
			}
			else
			{
				Singleton<Settings>.Instance.WriteDisplaySettings(this.newSettings);
			}
			base.transform.parent.gameObject.SetActive(false);
			base.transform.parent.gameObject.SetActive(true);
			base.gameObject.SetActive(false);
		}

		// Token: 0x0400364E RID: 13902
		public const float RevertTime = 15f;

		// Token: 0x0400364F RID: 13903
		public TextMeshProUGUI SubtitleLabel;

		// Token: 0x04003650 RID: 13904
		private float timeUntilRevert;

		// Token: 0x04003651 RID: 13905
		private DisplaySettings oldSettings;

		// Token: 0x04003652 RID: 13906
		private DisplaySettings newSettings;
	}
}
