using System;
using ScheduleOne.Management;
using ScheduleOne.ObjectScripts;
using UnityEngine.Events;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B53 RID: 2899
	public class MixingStationUIElement : WorldspaceUIElement
	{
		// Token: 0x17000A9F RID: 2719
		// (get) Token: 0x06004D3B RID: 19771 RVA: 0x0014564F File Offset: 0x0014384F
		// (set) Token: 0x06004D3C RID: 19772 RVA: 0x00145657 File Offset: 0x00143857
		public MixingStation AssignedStation { get; protected set; }

		// Token: 0x06004D3D RID: 19773 RVA: 0x00145660 File Offset: 0x00143860
		public void Initialize(MixingStation station)
		{
			this.AssignedStation = station;
			this.AssignedStation.Configuration.onChanged.AddListener(new UnityAction(this.RefreshUI));
			this.RefreshUI();
			base.gameObject.SetActive(false);
		}

		// Token: 0x06004D3E RID: 19774 RVA: 0x001456A0 File Offset: 0x001438A0
		protected virtual void RefreshUI()
		{
			MixingStationConfiguration mixingStationConfiguration = this.AssignedStation.Configuration as MixingStationConfiguration;
			base.SetAssignedNPC(mixingStationConfiguration.AssignedChemist.SelectedNPC);
		}
	}
}
