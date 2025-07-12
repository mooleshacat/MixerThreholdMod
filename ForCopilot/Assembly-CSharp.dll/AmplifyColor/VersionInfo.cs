using System;
using UnityEngine;

namespace AmplifyColor
{
	// Token: 0x02000CA0 RID: 3232
	[Serializable]
	public class VersionInfo
	{
		// Token: 0x06005AC0 RID: 23232 RVA: 0x0017E630 File Offset: 0x0017C830
		public static string StaticToString()
		{
			return string.Format("{0}.{1}.{2}", 1, 9, 0) + VersionInfo.StageSuffix + VersionInfo.TrialSuffix;
		}

		// Token: 0x06005AC1 RID: 23233 RVA: 0x0017E65E File Offset: 0x0017C85E
		public override string ToString()
		{
			return string.Format("{0}.{1}.{2}", this.m_major, this.m_minor, this.m_release) + VersionInfo.StageSuffix + VersionInfo.TrialSuffix;
		}

		// Token: 0x17000C8F RID: 3215
		// (get) Token: 0x06005AC2 RID: 23234 RVA: 0x0017E69A File Offset: 0x0017C89A
		public static int FullNumber
		{
			get
			{
				return 190;
			}
		}

		// Token: 0x17000C90 RID: 3216
		// (get) Token: 0x06005AC3 RID: 23235 RVA: 0x0017E6A1 File Offset: 0x0017C8A1
		public int Number
		{
			get
			{
				return this.m_major * 100 + this.m_minor * 10 + this.m_release;
			}
		}

		// Token: 0x06005AC4 RID: 23236 RVA: 0x0017E6BD File Offset: 0x0017C8BD
		private VersionInfo()
		{
			this.m_major = 1;
			this.m_minor = 9;
			this.m_release = 0;
		}

		// Token: 0x06005AC5 RID: 23237 RVA: 0x0017E6DB File Offset: 0x0017C8DB
		private VersionInfo(byte major, byte minor, byte release)
		{
			this.m_major = (int)major;
			this.m_minor = (int)minor;
			this.m_release = (int)release;
		}

		// Token: 0x06005AC6 RID: 23238 RVA: 0x0017E6F8 File Offset: 0x0017C8F8
		public static VersionInfo Current()
		{
			return new VersionInfo(1, 9, 0);
		}

		// Token: 0x06005AC7 RID: 23239 RVA: 0x0017E703 File Offset: 0x0017C903
		public static bool Matches(VersionInfo version)
		{
			return 1 == version.m_major && 9 == version.m_minor && version.m_release == 0;
		}

		// Token: 0x040042A5 RID: 17061
		public const byte Major = 1;

		// Token: 0x040042A6 RID: 17062
		public const byte Minor = 9;

		// Token: 0x040042A7 RID: 17063
		public const byte Release = 0;

		// Token: 0x040042A8 RID: 17064
		private static string StageSuffix = "";

		// Token: 0x040042A9 RID: 17065
		private static string TrialSuffix = "";

		// Token: 0x040042AA RID: 17066
		[SerializeField]
		private int m_major;

		// Token: 0x040042AB RID: 17067
		[SerializeField]
		private int m_minor;

		// Token: 0x040042AC RID: 17068
		[SerializeField]
		private int m_release;
	}
}
