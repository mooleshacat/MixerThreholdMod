using System;

namespace Funly.SkyStudio
{
	// Token: 0x020001BB RID: 443
	public interface IBaseKeyframe
	{
		// Token: 0x170001CB RID: 459
		// (get) Token: 0x060008FF RID: 2303
		string id { get; }

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000900 RID: 2304
		// (set) Token: 0x06000901 RID: 2305
		float time { get; set; }

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000902 RID: 2306
		// (set) Token: 0x06000903 RID: 2307
		InterpolationCurve interpolationCurve { get; set; }

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000904 RID: 2308
		// (set) Token: 0x06000905 RID: 2309
		InterpolationDirection interpolationDirection { get; set; }
	}
}
