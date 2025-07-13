using System;
using ScheduleOne.Employees;
using ScheduleOne.Management;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B4F RID: 2895
	public class ChemistUIElement : WorldspaceUIElement
	{
		// Token: 0x17000A9B RID: 2715
		// (get) Token: 0x06004D27 RID: 19751 RVA: 0x00145347 File Offset: 0x00143547
		// (set) Token: 0x06004D28 RID: 19752 RVA: 0x0014534F File Offset: 0x0014354F
		public Chemist AssignedChemist { get; protected set; }

		// Token: 0x06004D29 RID: 19753 RVA: 0x00145358 File Offset: 0x00143558
		public void Initialize(Chemist chemist)
		{
			this.AssignedChemist = chemist;
			this.AssignedChemist.Configuration.onChanged.AddListener(new UnityAction(this.RefreshUI));
			this.TitleLabel.text = chemist.fullName;
			this.RefreshUI();
			base.gameObject.SetActive(false);
		}

		// Token: 0x06004D2A RID: 19754 RVA: 0x001453B4 File Offset: 0x001435B4
		protected virtual void RefreshUI()
		{
			ChemistConfiguration chemistConfiguration = this.AssignedChemist.Configuration as ChemistConfiguration;
			for (int i = 0; i < this.StationsIcons.Length; i++)
			{
				if (chemistConfiguration.Stations.SelectedObjects.Count > i)
				{
					this.StationsIcons[i].sprite = chemistConfiguration.Stations.SelectedObjects[i].ItemInstance.Icon;
					this.StationsIcons[i].enabled = true;
				}
				else
				{
					this.StationsIcons[i].enabled = false;
				}
			}
		}

		// Token: 0x0400398B RID: 14731
		[Header("References")]
		public Image[] StationsIcons;
	}
}
