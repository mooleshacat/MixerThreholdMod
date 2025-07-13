using System;
using UnityEngine.UI;

namespace ScheduleOne.AvatarFramework.Customization
{
	// Token: 0x020009F5 RID: 2549
	public class ACSliderReplicator : ACReplicator
	{
		// Token: 0x060044AB RID: 17579 RVA: 0x001205B5 File Offset: 0x0011E7B5
		protected override void AvatarSettingsChanged(AvatarSettings newSettings)
		{
			base.AvatarSettingsChanged(newSettings);
			this.slider.SetValueWithoutNotify((float)newSettings[this.propertyName]);
		}

		// Token: 0x04003180 RID: 12672
		public Slider slider;
	}
}
