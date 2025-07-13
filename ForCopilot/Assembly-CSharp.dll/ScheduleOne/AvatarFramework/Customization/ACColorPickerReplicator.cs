using System;
using HSVPicker;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Customization
{
	// Token: 0x020009F2 RID: 2546
	public class ACColorPickerReplicator : ACReplicator
	{
		// Token: 0x060044A5 RID: 17573 RVA: 0x0012054C File Offset: 0x0011E74C
		protected override void AvatarSettingsChanged(AvatarSettings newSettings)
		{
			base.AvatarSettingsChanged(newSettings);
			this.picker.CurrentColor = (Color)newSettings[this.propertyName];
		}

		// Token: 0x0400317E RID: 12670
		public ColorPicker picker;
	}
}
