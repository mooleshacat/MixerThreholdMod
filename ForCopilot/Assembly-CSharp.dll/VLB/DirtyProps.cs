using System;

namespace VLB
{
	// Token: 0x02000114 RID: 276
	[Flags]
	public enum DirtyProps
	{
		// Token: 0x040005EA RID: 1514
		None = 0,
		// Token: 0x040005EB RID: 1515
		Intensity = 2,
		// Token: 0x040005EC RID: 1516
		HDRPExposureWeight = 4,
		// Token: 0x040005ED RID: 1517
		ColorMode = 8,
		// Token: 0x040005EE RID: 1518
		Color = 16,
		// Token: 0x040005EF RID: 1519
		BlendingMode = 32,
		// Token: 0x040005F0 RID: 1520
		Cone = 64,
		// Token: 0x040005F1 RID: 1521
		SideSoftness = 128,
		// Token: 0x040005F2 RID: 1522
		Attenuation = 256,
		// Token: 0x040005F3 RID: 1523
		Dimensions = 512,
		// Token: 0x040005F4 RID: 1524
		RaymarchingQuality = 1024,
		// Token: 0x040005F5 RID: 1525
		Jittering = 2048,
		// Token: 0x040005F6 RID: 1526
		NoiseMode = 4096,
		// Token: 0x040005F7 RID: 1527
		NoiseIntensity = 8192,
		// Token: 0x040005F8 RID: 1528
		NoiseVelocityAndScale = 16384,
		// Token: 0x040005F9 RID: 1529
		CookieProps = 32768,
		// Token: 0x040005FA RID: 1530
		ShadowProps = 65536,
		// Token: 0x040005FB RID: 1531
		AllWithoutMaterialChange = 125142,
		// Token: 0x040005FC RID: 1532
		OnlyMaterialChangeOnly = 5928,
		// Token: 0x040005FD RID: 1533
		All = 131070
	}
}
