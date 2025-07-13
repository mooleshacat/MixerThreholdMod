using System;

namespace Funly.SkyStudio
{
	// Token: 0x020001B1 RID: 433
	public interface IKeyframeGroup
	{
		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x060008C5 RID: 2245
		// (set) Token: 0x060008C6 RID: 2246
		string name { get; set; }

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x060008C7 RID: 2247
		string id { get; }

		// Token: 0x060008C8 RID: 2248
		void SortKeyframes();

		// Token: 0x060008C9 RID: 2249
		void TrimToSingleKeyframe();

		// Token: 0x060008CA RID: 2250
		void RemoveKeyFrame(IBaseKeyframe keyframe);

		// Token: 0x060008CB RID: 2251
		int GetKeyFrameCount();
	}
}
