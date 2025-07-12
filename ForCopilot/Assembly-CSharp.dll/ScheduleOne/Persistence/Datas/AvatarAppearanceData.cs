using System;
using ScheduleOne.AvatarFramework;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000400 RID: 1024
	[Serializable]
	public class AvatarAppearanceData : SaveData
	{
		// Token: 0x0600161D RID: 5661 RVA: 0x000634B9 File Offset: 0x000616B9
		public AvatarAppearanceData(AvatarSettings avatarSettings)
		{
			this.AvatarSettings = avatarSettings;
		}

		// Token: 0x040013DE RID: 5086
		public AvatarSettings AvatarSettings;
	}
}
