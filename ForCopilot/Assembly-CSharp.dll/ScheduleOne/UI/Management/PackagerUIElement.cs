using System;
using ScheduleOne.Employees;
using ScheduleOne.Management;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B54 RID: 2900
	public class PackagerUIElement : WorldspaceUIElement
	{
		// Token: 0x17000AA0 RID: 2720
		// (get) Token: 0x06004D40 RID: 19776 RVA: 0x001456CF File Offset: 0x001438CF
		// (set) Token: 0x06004D41 RID: 19777 RVA: 0x001456D7 File Offset: 0x001438D7
		public Packager AssignedPackager { get; protected set; }

		// Token: 0x06004D42 RID: 19778 RVA: 0x001456E0 File Offset: 0x001438E0
		public void Initialize(Packager packager)
		{
			this.AssignedPackager = packager;
			this.AssignedPackager.Configuration.onChanged.AddListener(new UnityAction(this.RefreshUI));
			this.TitleLabel.text = packager.fullName;
			this.RefreshUI();
			base.gameObject.SetActive(false);
		}

		// Token: 0x06004D43 RID: 19779 RVA: 0x0014573C File Offset: 0x0014393C
		protected virtual void RefreshUI()
		{
			PackagerConfiguration packagerConfiguration = this.AssignedPackager.Configuration as PackagerConfiguration;
			for (int i = 0; i < this.StationRects.Length; i++)
			{
				if (packagerConfiguration.Stations.SelectedObjects.Count > i)
				{
					this.StationRects[i].Find("Icon").GetComponent<Image>().sprite = packagerConfiguration.Stations.SelectedObjects[i].ItemInstance.Icon;
					this.StationRects[i].Find("Icon").gameObject.SetActive(true);
				}
				else
				{
					this.StationRects[i].Find("Icon").gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x04003993 RID: 14739
		[Header("References")]
		public RectTransform[] StationRects;
	}
}
