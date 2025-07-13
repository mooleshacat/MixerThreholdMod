using System;
using UnityEngine;

namespace ScheduleOne.Storage
{
	// Token: 0x020008E0 RID: 2272
	public class PalletSlot : MonoBehaviour, IGUIDRegisterable
	{
		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x06003D56 RID: 15702 RVA: 0x00102BBD File Offset: 0x00100DBD
		// (set) Token: 0x06003D57 RID: 15703 RVA: 0x00102BC5 File Offset: 0x00100DC5
		public Guid GUID { get; protected set; }

		// Token: 0x06003D58 RID: 15704 RVA: 0x00102BCE File Offset: 0x00100DCE
		public void SetGUID(Guid guid)
		{
			this.GUID = guid;
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x06003D59 RID: 15705 RVA: 0x00102BDD File Offset: 0x00100DDD
		// (set) Token: 0x06003D5A RID: 15706 RVA: 0x00102BE5 File Offset: 0x00100DE5
		public Pallet occupant { get; protected set; }

		// Token: 0x06003D5B RID: 15707 RVA: 0x00102BEE File Offset: 0x00100DEE
		public void SetOccupant(Pallet _occupant)
		{
			this.occupant = _occupant;
			if (this.occupant != null)
			{
				if (this.onPalletAdded != null)
				{
					this.onPalletAdded();
					return;
				}
			}
			else if (this.onPalletRemoved != null)
			{
				this.onPalletRemoved();
			}
		}

		// Token: 0x04002BF8 RID: 11256
		public Action onPalletAdded;

		// Token: 0x04002BF9 RID: 11257
		public Action onPalletRemoved;
	}
}
