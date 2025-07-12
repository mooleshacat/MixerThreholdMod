using System;
using ScheduleOne.Employees;
using ScheduleOne.Management;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B50 RID: 2896
	public class CleanerUIElement : WorldspaceUIElement
	{
		// Token: 0x17000A9C RID: 2716
		// (get) Token: 0x06004D2C RID: 19756 RVA: 0x0014543E File Offset: 0x0014363E
		// (set) Token: 0x06004D2D RID: 19757 RVA: 0x00145446 File Offset: 0x00143646
		public Cleaner AssignedCleaner { get; protected set; }

		// Token: 0x06004D2E RID: 19758 RVA: 0x00145450 File Offset: 0x00143650
		public void Initialize(Cleaner cleaner)
		{
			this.AssignedCleaner = cleaner;
			this.AssignedCleaner.Configuration.onChanged.AddListener(new UnityAction(this.RefreshUI));
			this.TitleLabel.text = cleaner.fullName;
			this.RefreshUI();
			base.gameObject.SetActive(false);
		}

		// Token: 0x06004D2F RID: 19759 RVA: 0x001454AC File Offset: 0x001436AC
		protected virtual void RefreshUI()
		{
			CleanerConfiguration cleanerConfiguration = this.AssignedCleaner.Configuration as CleanerConfiguration;
			for (int i = 0; i < this.StationsIcons.Length; i++)
			{
				if (cleanerConfiguration.Bins.SelectedObjects.Count > i)
				{
					this.StationsIcons[i].sprite = cleanerConfiguration.Bins.SelectedObjects[i].ItemInstance.Icon;
					this.StationsIcons[i].enabled = true;
				}
				else
				{
					this.StationsIcons[i].enabled = false;
				}
			}
		}

		// Token: 0x0400398D RID: 14733
		[Header("References")]
		public Image[] StationsIcons;
	}
}
