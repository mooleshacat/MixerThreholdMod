using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.Storage
{
	// Token: 0x020008E2 RID: 2274
	public class PalletZone : MonoBehaviour
	{
		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x06003D5F RID: 15711 RVA: 0x00102C3A File Offset: 0x00100E3A
		public bool isClear
		{
			get
			{
				return (this.pallets.Count == 0 || this.AreAllPalletsClear()) && !this.orderReceivedThisFrame;
			}
		}

		// Token: 0x06003D60 RID: 15712 RVA: 0x00102C5C File Offset: 0x00100E5C
		protected void OnTriggerStay(Collider other)
		{
			Pallet componentInParent = other.GetComponentInParent<Pallet>();
			if (componentInParent != null && !this.pallets.Contains(componentInParent))
			{
				this.pallets.Add(componentInParent);
			}
		}

		// Token: 0x06003D61 RID: 15713 RVA: 0x00102C93 File Offset: 0x00100E93
		protected void FixedUpdate()
		{
			this.pallets.Clear();
		}

		// Token: 0x06003D62 RID: 15714 RVA: 0x00102CA0 File Offset: 0x00100EA0
		protected void LateUpdate()
		{
			this.orderReceivedThisFrame = false;
		}

		// Token: 0x06003D63 RID: 15715 RVA: 0x00102CA9 File Offset: 0x00100EA9
		public Pallet GeneratePallet()
		{
			Pallet component = UnityEngine.Object.Instantiate<GameObject>(this.palletPrefab).GetComponent<Pallet>();
			component.transform.position = base.transform.position;
			component.transform.rotation = base.transform.rotation;
			return component;
		}

		// Token: 0x06003D64 RID: 15716 RVA: 0x00102CE8 File Offset: 0x00100EE8
		private bool AreAllPalletsClear()
		{
			for (int i = 0; i < this.pallets.Count; i++)
			{
				if (!this.pallets[i].isEmpty)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04002BFB RID: 11259
		private List<Pallet> pallets = new List<Pallet>();

		// Token: 0x04002BFC RID: 11260
		[Header("Prefabs")]
		[SerializeField]
		protected GameObject palletPrefab;

		// Token: 0x04002BFD RID: 11261
		private bool orderReceivedThisFrame;
	}
}
