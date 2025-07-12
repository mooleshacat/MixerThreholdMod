using System;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Customization
{
	// Token: 0x020009F0 RID: 2544
	public class ACAssetPathReplicator<T> : ACReplicator where T : UnityEngine.Object
	{
		// Token: 0x060044A1 RID: 17569 RVA: 0x001204FD File Offset: 0x0011E6FD
		protected virtual void Awake()
		{
			this.selection = base.GetComponent<ACSelection<T>>();
		}

		// Token: 0x060044A2 RID: 17570 RVA: 0x0012050B File Offset: 0x0011E70B
		protected override void AvatarSettingsChanged(AvatarSettings newSettings)
		{
			base.AvatarSettingsChanged(newSettings);
			this.selection.SelectOption(this.selection.GetAssetPathIndex((string)newSettings[this.propertyName]), false);
		}

		// Token: 0x0400317D RID: 12669
		private ACSelection<T> selection;
	}
}
