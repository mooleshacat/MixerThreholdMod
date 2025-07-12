using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000118 RID: 280
	[Serializable]
	public class RaymarchingQuality
	{
		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x0600045E RID: 1118 RVA: 0x000178FE File Offset: 0x00015AFE
		public int uniqueID
		{
			get
			{
				return this._UniqueID;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x0600045F RID: 1119 RVA: 0x00017906 File Offset: 0x00015B06
		public bool hasValidUniqueID
		{
			get
			{
				return this._UniqueID >= 0;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000460 RID: 1120 RVA: 0x00017914 File Offset: 0x00015B14
		public static RaymarchingQuality defaultInstance
		{
			get
			{
				return RaymarchingQuality.ms_DefaultInstance;
			}
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x0001791B File Offset: 0x00015B1B
		private RaymarchingQuality(int uniqueID)
		{
			this._UniqueID = uniqueID;
			this.name = "New quality";
			this.stepCount = 10;
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0001793D File Offset: 0x00015B3D
		public static RaymarchingQuality New()
		{
			return new RaymarchingQuality(UnityEngine.Random.Range(4, int.MaxValue));
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x0001794F File Offset: 0x00015B4F
		public static RaymarchingQuality New(string name, int forcedUniqueID, int stepCount)
		{
			return new RaymarchingQuality(forcedUniqueID)
			{
				name = name,
				stepCount = stepCount
			};
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x00017968 File Offset: 0x00015B68
		private static bool HasRaymarchingQualityWithSameUniqueID(RaymarchingQuality[] values, int id)
		{
			foreach (RaymarchingQuality raymarchingQuality in values)
			{
				if (raymarchingQuality != null && raymarchingQuality.uniqueID == id)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000607 RID: 1543
		public string name;

		// Token: 0x04000608 RID: 1544
		public int stepCount;

		// Token: 0x04000609 RID: 1545
		[SerializeField]
		private int _UniqueID;

		// Token: 0x0400060A RID: 1546
		private static RaymarchingQuality ms_DefaultInstance = new RaymarchingQuality(-1);

		// Token: 0x0400060B RID: 1547
		private const int kRandomUniqueIdMinRange = 4;
	}
}
