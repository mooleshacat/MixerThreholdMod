using System;

namespace LiquidVolumeFX
{
	// Token: 0x02000188 RID: 392
	public static class DetailExtensions
	{
		// Token: 0x0600075D RID: 1885 RVA: 0x00021970 File Offset: 0x0001FB70
		public static bool allowsRefraction(this DETAIL detail)
		{
			return detail != DETAIL.DefaultNoFlask;
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x0002197A File Offset: 0x0001FB7A
		public static bool usesFlask(this DETAIL detail)
		{
			return detail == DETAIL.Simple || detail == DETAIL.Default;
		}
	}
}
