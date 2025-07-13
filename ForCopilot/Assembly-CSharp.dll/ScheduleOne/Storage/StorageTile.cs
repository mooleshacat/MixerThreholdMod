using System;
using UnityEngine;

namespace ScheduleOne.Storage
{
	// Token: 0x020008EC RID: 2284
	public class StorageTile : MonoBehaviour
	{
		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x06003DE6 RID: 15846 RVA: 0x001051E7 File Offset: 0x001033E7
		public StorageGrid _ownerGrid
		{
			get
			{
				return this.ownerGrid;
			}
		}

		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x06003DE7 RID: 15847 RVA: 0x001051EF File Offset: 0x001033EF
		// (set) Token: 0x06003DE8 RID: 15848 RVA: 0x001051F7 File Offset: 0x001033F7
		public StoredItem occupant { get; protected set; }

		// Token: 0x06003DE9 RID: 15849 RVA: 0x00105200 File Offset: 0x00103400
		public void InitializeStorageTile(int _x, int _y, float _available_Offset, StorageGrid _ownerGrid)
		{
			this.x = _x;
			this.y = _y;
			this.ownerGrid = _ownerGrid;
		}

		// Token: 0x06003DEA RID: 15850 RVA: 0x00105218 File Offset: 0x00103418
		public void SetOccupant(StoredItem occ)
		{
			if (occ != null && this.occupant != null)
			{
				Console.LogWarning("SetOccupant called by there is an existing occupant. Existing occupant should be dealt with before calling this.", null);
			}
			this.occupant = occ;
			if (this.onOccupantChanged != null)
			{
				this.onOccupantChanged();
			}
		}

		// Token: 0x04002C24 RID: 11300
		public int x;

		// Token: 0x04002C25 RID: 11301
		public int y;

		// Token: 0x04002C26 RID: 11302
		[SerializeField]
		public StorageGrid ownerGrid;

		// Token: 0x04002C27 RID: 11303
		public Action onOccupantChanged;
	}
}
