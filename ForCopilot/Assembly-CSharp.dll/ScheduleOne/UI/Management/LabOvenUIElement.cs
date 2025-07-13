using System;
using ScheduleOne.Management;
using ScheduleOne.ObjectScripts;
using UnityEngine.Events;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B52 RID: 2898
	public class LabOvenUIElement : WorldspaceUIElement
	{
		// Token: 0x17000A9E RID: 2718
		// (get) Token: 0x06004D36 RID: 19766 RVA: 0x001455D0 File Offset: 0x001437D0
		// (set) Token: 0x06004D37 RID: 19767 RVA: 0x001455D8 File Offset: 0x001437D8
		public LabOven AssignedOven { get; protected set; }

		// Token: 0x06004D38 RID: 19768 RVA: 0x001455E1 File Offset: 0x001437E1
		public void Initialize(LabOven oven)
		{
			this.AssignedOven = oven;
			this.AssignedOven.Configuration.onChanged.AddListener(new UnityAction(this.RefreshUI));
			this.RefreshUI();
			base.gameObject.SetActive(false);
		}

		// Token: 0x06004D39 RID: 19769 RVA: 0x00145620 File Offset: 0x00143820
		protected virtual void RefreshUI()
		{
			LabOvenConfiguration labOvenConfiguration = this.AssignedOven.Configuration as LabOvenConfiguration;
			base.SetAssignedNPC(labOvenConfiguration.AssignedChemist.SelectedNPC);
		}
	}
}
