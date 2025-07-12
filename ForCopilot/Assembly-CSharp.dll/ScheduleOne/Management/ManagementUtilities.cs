using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;

namespace ScheduleOne.Management
{
	// Token: 0x020005BD RID: 1469
	public class ManagementUtilities : Singleton<ManagementUtilities>
	{
		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x06002437 RID: 9271 RVA: 0x00094B7C File Offset: 0x00092D7C
		public static List<string> WeedSeedAssetPaths
		{
			get
			{
				return Singleton<ManagementUtilities>.Instance.weedSeedAssetPaths;
			}
		}

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x06002438 RID: 9272 RVA: 0x00094B88 File Offset: 0x00092D88
		public static List<string> AdditiveAssetPaths
		{
			get
			{
				return Singleton<ManagementUtilities>.Instance.additiveAssetPaths;
			}
		}

		// Token: 0x04001ADD RID: 6877
		public List<string> weedSeedAssetPaths = new List<string>();

		// Token: 0x04001ADE RID: 6878
		public List<string> additiveAssetPaths = new List<string>();

		// Token: 0x04001ADF RID: 6879
		public List<AdditiveDefinition> AdditiveDefinitions = new List<AdditiveDefinition>();
	}
}
