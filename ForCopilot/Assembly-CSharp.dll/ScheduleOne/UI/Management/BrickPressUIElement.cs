using System;
using ScheduleOne.Management;
using ScheduleOne.ObjectScripts;
using UnityEngine.Events;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B4C RID: 2892
	public class BrickPressUIElement : WorldspaceUIElement
	{
		// Token: 0x17000A98 RID: 2712
		// (get) Token: 0x06004D18 RID: 19736 RVA: 0x00145165 File Offset: 0x00143365
		// (set) Token: 0x06004D19 RID: 19737 RVA: 0x0014516D File Offset: 0x0014336D
		public BrickPress AssignedPress { get; protected set; }

		// Token: 0x06004D1A RID: 19738 RVA: 0x00145176 File Offset: 0x00143376
		public void Initialize(BrickPress press)
		{
			this.AssignedPress = press;
			this.AssignedPress.Configuration.onChanged.AddListener(new UnityAction(this.RefreshUI));
			this.RefreshUI();
			base.gameObject.SetActive(false);
		}

		// Token: 0x06004D1B RID: 19739 RVA: 0x001451B4 File Offset: 0x001433B4
		protected virtual void RefreshUI()
		{
			BrickPressConfiguration brickPressConfiguration = this.AssignedPress.Configuration as BrickPressConfiguration;
			base.SetAssignedNPC(brickPressConfiguration.AssignedPackager.SelectedNPC);
		}
	}
}
