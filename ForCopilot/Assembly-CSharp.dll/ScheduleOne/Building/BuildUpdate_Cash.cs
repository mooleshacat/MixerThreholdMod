using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Interaction;
using ScheduleOne.ItemFramework;
using ScheduleOne.Money;
using ScheduleOne.ObjectScripts.Cash;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Storage;
using UnityEngine;

namespace ScheduleOne.Building
{
	// Token: 0x020007C8 RID: 1992
	public class BuildUpdate_Cash : BuildUpdate_StoredItem
	{
		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x060035FA RID: 13818 RVA: 0x000E1DF1 File Offset: 0x000DFFF1
		private float placeAmount
		{
			get
			{
				return (float)Cash.amounts[this.amountIndex % Cash.amounts.Length];
			}
		}

		// Token: 0x060035FB RID: 13819 RVA: 0x000E1E08 File Offset: 0x000E0008
		private void Start()
		{
			Transform transform = this.ghostModel.transform.Find("Bills");
			for (int i = 0; i < transform.childCount; i++)
			{
				this.bills.Add(transform.GetChild(i));
			}
			this.RefreshGhostModelAppearance();
			this.amountLabel = new WorldSpaceLabel("Amount", Vector3.zero);
			this.amountLabel.scale = 1.25f;
		}

		// Token: 0x060035FC RID: 13820 RVA: 0x000E1E79 File Offset: 0x000E0079
		protected override void Update()
		{
			base.Update();
			if (GameInput.GetButtonDown(GameInput.ButtonCode.SecondaryClick))
			{
				this.amountIndex++;
				this.RefreshGhostModelAppearance();
			}
		}

		// Token: 0x060035FD RID: 13821 RVA: 0x000E1EA0 File Offset: 0x000E00A0
		protected override void LateUpdate()
		{
			base.LateUpdate();
			if (this.GetRelevantCashBalane() < this.placeAmount)
			{
				if (this.GetRelevantCashBalane() < (float)Cash.amounts[0])
				{
					this.amountIndex = 0;
					this.RefreshGhostModelAppearance();
					this.validPosition = false;
					base.UpdateMaterials();
					this.amountLabel.text = "Insufficient cash";
					this.UpdateLabel();
					return;
				}
				while (this.GetRelevantCashBalane() < this.placeAmount)
				{
					this.amountIndex++;
					this.RefreshGhostModelAppearance();
				}
			}
			this.amountLabel.text = MoneyManager.FormatAmount(this.placeAmount, false, false);
			this.UpdateLabel();
		}

		// Token: 0x060035FE RID: 13822 RVA: 0x000E1F44 File Offset: 0x000E0144
		private void UpdateLabel()
		{
			this.amountLabel.position = this.ghostModel.transform.position;
			Vector3 a = PlayerSingleton<PlayerCamera>.Instance.transform.position - this.ghostModel.transform.position;
			a.y = 0f;
			a.Normalize();
			this.amountLabel.position += a * 0.2f;
			if (this.validPosition)
			{
				this.amountLabel.color = Color.white;
				return;
			}
			this.amountLabel.color = new Color32(byte.MaxValue, 50, 50, byte.MaxValue);
		}

		// Token: 0x060035FF RID: 13823 RVA: 0x000E2004 File Offset: 0x000E0204
		private void RefreshGhostModelAppearance()
		{
			int billStacksToDisplay = Cash.GetBillStacksToDisplay(this.placeAmount);
			for (int i = 0; i < this.bills.Count; i++)
			{
				if (i < billStacksToDisplay)
				{
					this.bills[i].gameObject.SetActive(true);
				}
				else
				{
					this.bills[i].gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x06003600 RID: 13824 RVA: 0x000E2068 File Offset: 0x000E0268
		protected override void Place()
		{
			float rotation = Vector3.SignedAngle(this.bestIntersection.storageTile.ownerGrid.transform.forward, this.storedItemClass.buildPoint.forward, this.bestIntersection.storageTile.ownerGrid.transform.up);
			CashInstance cashInstance = new CashInstance(this.itemInstance.Definition, 1);
			cashInstance.SetBalance(this.placeAmount, false);
			Singleton<BuildManager>.Instance.CreateStoredItem(cashInstance, this.bestIntersection.storageTile.ownerGrid.GetComponentInParent<IStorageEntity>(), this.bestIntersection.storageTile.ownerGrid, base.GetOriginCoordinate(), rotation);
			this.mouseUpSincePlace = false;
			this.PostPlace();
		}

		// Token: 0x06003601 RID: 13825 RVA: 0x000E2122 File Offset: 0x000E0322
		protected override void PostPlace()
		{
			NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(-this.placeAmount, true, false);
		}

		// Token: 0x06003602 RID: 13826 RVA: 0x000E2137 File Offset: 0x000E0337
		public override void Stop()
		{
			base.Stop();
			this.amountLabel.Destroy();
		}

		// Token: 0x06003603 RID: 13827 RVA: 0x000E214A File Offset: 0x000E034A
		public float GetRelevantCashBalane()
		{
			return NetworkSingleton<MoneyManager>.Instance.cashBalance;
		}

		// Token: 0x04002631 RID: 9777
		public int amountIndex;

		// Token: 0x04002632 RID: 9778
		protected List<Transform> bills = new List<Transform>();

		// Token: 0x04002633 RID: 9779
		private WorldSpaceLabel amountLabel;
	}
}
