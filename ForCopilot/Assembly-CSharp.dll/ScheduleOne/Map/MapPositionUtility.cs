using System;
using EasyButtons;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.Map
{
	// Token: 0x02000C79 RID: 3193
	public class MapPositionUtility : Singleton<MapPositionUtility>
	{
		// Token: 0x17000C73 RID: 3187
		// (get) Token: 0x060059B8 RID: 22968 RVA: 0x0017ADD8 File Offset: 0x00178FD8
		// (set) Token: 0x060059B9 RID: 22969 RVA: 0x0017ADE0 File Offset: 0x00178FE0
		private float conversionFactor { get; set; }

		// Token: 0x060059BA RID: 22970 RVA: 0x0017ADE9 File Offset: 0x00178FE9
		protected override void Awake()
		{
			base.Awake();
			this.Recalculate();
		}

		// Token: 0x060059BB RID: 22971 RVA: 0x0017ADF7 File Offset: 0x00178FF7
		public Vector2 GetMapPosition(Vector3 worldPosition)
		{
			return new Vector2(worldPosition.x - this.OriginPoint.position.x, worldPosition.z - this.OriginPoint.position.z) * this.conversionFactor;
		}

		// Token: 0x060059BC RID: 22972 RVA: 0x0017AE37 File Offset: 0x00179037
		[Button]
		public void Recalculate()
		{
			this.conversionFactor = this.MapDimensions * 0.5f / Vector3.Distance(this.OriginPoint.position, this.EdgePoint.position);
		}

		// Token: 0x040041D8 RID: 16856
		public Transform OriginPoint;

		// Token: 0x040041D9 RID: 16857
		public Transform EdgePoint;

		// Token: 0x040041DA RID: 16858
		public float MapDimensions = 2048f;
	}
}
