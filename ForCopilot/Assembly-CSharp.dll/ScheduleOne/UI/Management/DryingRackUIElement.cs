using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Management;
using ScheduleOne.ObjectScripts;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B51 RID: 2897
	public class DryingRackUIElement : WorldspaceUIElement
	{
		// Token: 0x17000A9D RID: 2717
		// (get) Token: 0x06004D31 RID: 19761 RVA: 0x00145536 File Offset: 0x00143736
		// (set) Token: 0x06004D32 RID: 19762 RVA: 0x0014553E File Offset: 0x0014373E
		public DryingRack AssignedRack { get; protected set; }

		// Token: 0x06004D33 RID: 19763 RVA: 0x00145547 File Offset: 0x00143747
		public void Initialize(DryingRack rack)
		{
			this.AssignedRack = rack;
			this.AssignedRack.Configuration.onChanged.AddListener(new UnityAction(this.RefreshUI));
			this.RefreshUI();
			base.gameObject.SetActive(false);
		}

		// Token: 0x06004D34 RID: 19764 RVA: 0x00145584 File Offset: 0x00143784
		protected virtual void RefreshUI()
		{
			DryingRackConfiguration dryingRackConfiguration = this.AssignedRack.Configuration as DryingRackConfiguration;
			EQuality value = dryingRackConfiguration.TargetQuality.Value;
			this.TargetQualityIcon.color = ItemQuality.GetColor(value);
			base.SetAssignedNPC(dryingRackConfiguration.AssignedBotanist.SelectedNPC);
		}

		// Token: 0x04003990 RID: 14736
		public Image TargetQualityIcon;
	}
}
