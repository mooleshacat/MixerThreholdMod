using System;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Customization
{
	// Token: 0x020009F4 RID: 2548
	public class ACReplicator : MonoBehaviour
	{
		// Token: 0x060044A8 RID: 17576 RVA: 0x00120579 File Offset: 0x0011E779
		private void Start()
		{
			CustomizationManager instance = Singleton<CustomizationManager>.Instance;
			instance.OnAvatarSettingsChanged = (CustomizationManager.AvatarSettingsChanged)Delegate.Combine(instance.OnAvatarSettingsChanged, new CustomizationManager.AvatarSettingsChanged(this.AvatarSettingsChanged));
		}

		// Token: 0x060044A9 RID: 17577 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void AvatarSettingsChanged(AvatarSettings newSettings)
		{
		}

		// Token: 0x0400317F RID: 12671
		public string propertyName = string.Empty;
	}
}
