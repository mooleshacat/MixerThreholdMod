using System;
using ScheduleOne.Management;
using ScheduleOne.ObjectScripts;
using UnityEngine.Events;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B55 RID: 2901
	public class PackagingStationUIElement : WorldspaceUIElement
	{
		// Token: 0x17000AA1 RID: 2721
		// (get) Token: 0x06004D45 RID: 19781 RVA: 0x001457F9 File Offset: 0x001439F9
		// (set) Token: 0x06004D46 RID: 19782 RVA: 0x00145801 File Offset: 0x00143A01
		public PackagingStation AssignedStation { get; protected set; }

		// Token: 0x06004D47 RID: 19783 RVA: 0x0014580A File Offset: 0x00143A0A
		public void Initialize(PackagingStation pack)
		{
			this.AssignedStation = pack;
			this.AssignedStation.Configuration.onChanged.AddListener(new UnityAction(this.RefreshUI));
			this.RefreshUI();
			base.gameObject.SetActive(false);
		}

		// Token: 0x06004D48 RID: 19784 RVA: 0x00145848 File Offset: 0x00143A48
		protected virtual void RefreshUI()
		{
			PackagingStationConfiguration packagingStationConfiguration = this.AssignedStation.Configuration as PackagingStationConfiguration;
			base.SetAssignedNPC(packagingStationConfiguration.AssignedPackager.SelectedNPC);
		}
	}
}
