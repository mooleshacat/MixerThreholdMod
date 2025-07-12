using System;

namespace Funly.SkyStudio
{
	// Token: 0x020001AD RID: 429
	public struct ProfileBlendingState
	{
		// Token: 0x060008BE RID: 2238 RVA: 0x00027A3D File Offset: 0x00025C3D
		public ProfileBlendingState(SkyProfile blendedProfile, SkyProfile fromProfile, SkyProfile toProfile, float progress, float outProgress, float inProgress, float timeOfDay)
		{
			this.blendedProfile = blendedProfile;
			this.fromProfile = fromProfile;
			this.toProfile = toProfile;
			this.progress = progress;
			this.inProgress = inProgress;
			this.outProgress = outProgress;
			this.timeOfDay = timeOfDay;
		}

		// Token: 0x0400096E RID: 2414
		public SkyProfile blendedProfile;

		// Token: 0x0400096F RID: 2415
		public SkyProfile fromProfile;

		// Token: 0x04000970 RID: 2416
		public SkyProfile toProfile;

		// Token: 0x04000971 RID: 2417
		public float progress;

		// Token: 0x04000972 RID: 2418
		public float outProgress;

		// Token: 0x04000973 RID: 2419
		public float inProgress;

		// Token: 0x04000974 RID: 2420
		public float timeOfDay;
	}
}
