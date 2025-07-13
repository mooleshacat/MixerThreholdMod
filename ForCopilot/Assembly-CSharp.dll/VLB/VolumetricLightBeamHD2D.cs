using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x0200011C RID: 284
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[SelectionBase]
	[HelpURL("http://saladgamer.com/vlb-doc/comp-lightbeam-hd/")]
	public class VolumetricLightBeamHD2D : VolumetricLightBeamHD
	{
		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060004E2 RID: 1250 RVA: 0x00018800 File Offset: 0x00016A00
		// (set) Token: 0x060004E3 RID: 1251 RVA: 0x00018808 File Offset: 0x00016A08
		public int sortingLayerID
		{
			get
			{
				return this.m_SortingLayerID;
			}
			set
			{
				this.m_SortingLayerID = value;
				if (this.m_BeamGeom)
				{
					this.m_BeamGeom.sortingLayerID = value;
				}
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060004E4 RID: 1252 RVA: 0x0001882A File Offset: 0x00016A2A
		// (set) Token: 0x060004E5 RID: 1253 RVA: 0x00018837 File Offset: 0x00016A37
		public string sortingLayerName
		{
			get
			{
				return SortingLayer.IDToName(this.sortingLayerID);
			}
			set
			{
				this.sortingLayerID = SortingLayer.NameToID(value);
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060004E6 RID: 1254 RVA: 0x00018845 File Offset: 0x00016A45
		// (set) Token: 0x060004E7 RID: 1255 RVA: 0x0001884D File Offset: 0x00016A4D
		public int sortingOrder
		{
			get
			{
				return this.m_SortingOrder;
			}
			set
			{
				this.m_SortingOrder = value;
				if (this.m_BeamGeom)
				{
					this.m_BeamGeom.sortingOrder = value;
				}
			}
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x000022C9 File Offset: 0x000004C9
		public override Dimensions GetDimensions()
		{
			return Dimensions.Dim2D;
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x000022C9 File Offset: 0x000004C9
		public override bool DoesSupportSorting2D()
		{
			return true;
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x0001886F File Offset: 0x00016A6F
		public override int GetSortingLayerID()
		{
			return this.sortingLayerID;
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x00018877 File Offset: 0x00016A77
		public override int GetSortingOrder()
		{
			return this.sortingOrder;
		}

		// Token: 0x04000635 RID: 1589
		[SerializeField]
		private int m_SortingLayerID;

		// Token: 0x04000636 RID: 1590
		[SerializeField]
		private int m_SortingOrder;
	}
}
