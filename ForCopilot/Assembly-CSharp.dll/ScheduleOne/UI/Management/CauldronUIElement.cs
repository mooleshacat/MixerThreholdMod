using System;
using ScheduleOne.Management;
using ScheduleOne.ObjectScripts;
using UnityEngine.Events;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B4D RID: 2893
	public class CauldronUIElement : WorldspaceUIElement
	{
		// Token: 0x17000A99 RID: 2713
		// (get) Token: 0x06004D1D RID: 19741 RVA: 0x001451E3 File Offset: 0x001433E3
		// (set) Token: 0x06004D1E RID: 19742 RVA: 0x001451EB File Offset: 0x001433EB
		public Cauldron AssignedCauldron { get; protected set; }

		// Token: 0x06004D1F RID: 19743 RVA: 0x001451F4 File Offset: 0x001433F4
		public void Initialize(Cauldron cauldron)
		{
			this.AssignedCauldron = cauldron;
			this.AssignedCauldron.Configuration.onChanged.AddListener(new UnityAction(this.RefreshUI));
			this.RefreshUI();
			base.gameObject.SetActive(false);
		}

		// Token: 0x06004D20 RID: 19744 RVA: 0x00145234 File Offset: 0x00143434
		protected virtual void RefreshUI()
		{
			CauldronConfiguration cauldronConfiguration = this.AssignedCauldron.Configuration as CauldronConfiguration;
			base.SetAssignedNPC(cauldronConfiguration.AssignedChemist.SelectedNPC);
		}
	}
}
