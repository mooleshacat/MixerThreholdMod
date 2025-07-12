using System;

namespace VLB
{
	// Token: 0x0200015F RID: 351
	public static class Version
	{
		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060006C1 RID: 1729 RVA: 0x0001E3DD File Offset: 0x0001C5DD
		public static string CurrentAsString
		{
			get
			{
				return Version.GetVersionAsString(20100);
			}
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x0001E3EC File Offset: 0x0001C5EC
		private static string GetVersionAsString(int version)
		{
			int num = version / 10000;
			int num2 = (version - num * 10000) / 100;
			int num3 = (version - num * 10000 - num2 * 100) / 1;
			return string.Format("{0}.{1}.{2}", num, num2, num3);
		}

		// Token: 0x04000779 RID: 1913
		public const int Current = 20100;
	}
}
