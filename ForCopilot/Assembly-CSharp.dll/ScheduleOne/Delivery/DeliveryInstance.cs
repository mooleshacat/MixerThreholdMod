using System;
using FishNet.Serializing.Helping;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Property;
using ScheduleOne.UI.Phone.Delivery;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Delivery
{
	// Token: 0x02000752 RID: 1874
	[Serializable]
	public class DeliveryInstance
	{
		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x06003257 RID: 12887 RVA: 0x000D1F33 File Offset: 0x000D0133
		// (set) Token: 0x06003258 RID: 12888 RVA: 0x000D1F3B File Offset: 0x000D013B
		[CodegenExclude]
		public DeliveryVehicle ActiveVehicle { get; private set; }

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x06003259 RID: 12889 RVA: 0x000D1F44 File Offset: 0x000D0144
		[CodegenExclude]
		public Property Destination
		{
			get
			{
				return Singleton<PropertyManager>.Instance.GetProperty(this.DestinationCode);
			}
		}

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x0600325A RID: 12890 RVA: 0x000D1F56 File Offset: 0x000D0156
		[CodegenExclude]
		public LoadingDock LoadingDock
		{
			get
			{
				return this.Destination.LoadingDocks[this.LoadingDockIndex];
			}
		}

		// Token: 0x0600325B RID: 12891 RVA: 0x000D1F6A File Offset: 0x000D016A
		public DeliveryInstance(string deliveryID, string storeName, string destinationCode, int loadingDockIndex, StringIntPair[] items, EDeliveryStatus status, int timeUntilArrival)
		{
			this.DeliveryID = deliveryID;
			this.StoreName = storeName;
			this.DestinationCode = destinationCode;
			this.LoadingDockIndex = loadingDockIndex;
			this.Items = items;
			this.Status = status;
			this.TimeUntilArrival = timeUntilArrival;
		}

		// Token: 0x0600325C RID: 12892 RVA: 0x0000494F File Offset: 0x00002B4F
		public DeliveryInstance()
		{
		}

		// Token: 0x0600325D RID: 12893 RVA: 0x000D1FA7 File Offset: 0x000D01A7
		public int GetTimeStatus()
		{
			if (this.Status == EDeliveryStatus.Arrived)
			{
				return -1;
			}
			if (this.Status == EDeliveryStatus.Waiting)
			{
				return 0;
			}
			return this.TimeUntilArrival;
		}

		// Token: 0x0600325E RID: 12894 RVA: 0x000D1FC8 File Offset: 0x000D01C8
		public void SetStatus(EDeliveryStatus status)
		{
			Console.Log("Setting delivery status to " + status.ToString() + " for delivery " + this.DeliveryID, null);
			this.Status = status;
			if (this.Status == EDeliveryStatus.Arrived)
			{
				this.ActiveVehicle = NetworkSingleton<DeliveryManager>.Instance.GetShopInterface(this.StoreName).DeliveryVehicle;
				this.ActiveVehicle.Activate(this);
			}
			if (this.Status == EDeliveryStatus.Completed)
			{
				if (this.ActiveVehicle != null)
				{
					this.ActiveVehicle.Deactivate();
				}
				if (this.onDeliveryCompleted != null)
				{
					this.onDeliveryCompleted.Invoke();
				}
			}
		}

		// Token: 0x0600325F RID: 12895 RVA: 0x000D206C File Offset: 0x000D026C
		public void AddItemsToDeliveryVehicle()
		{
			DeliveryVehicle deliveryVehicle = PlayerSingleton<DeliveryApp>.Instance.GetShop(this.StoreName).MatchingShop.DeliveryVehicle;
			foreach (StringIntPair stringIntPair in this.Items)
			{
				ItemDefinition item = Registry.GetItem(stringIntPair.String);
				int j = stringIntPair.Int;
				while (j > 0)
				{
					int num = Mathf.Min(j, item.StackLimit);
					j -= num;
					ItemInstance defaultInstance = Registry.GetItem(stringIntPair.String).GetDefaultInstance(num);
					deliveryVehicle.Vehicle.Storage.InsertItem(defaultInstance, true);
				}
			}
		}

		// Token: 0x06003260 RID: 12896 RVA: 0x000D2107 File Offset: 0x000D0307
		public void OnMinPass()
		{
			this.TimeUntilArrival = Mathf.Max(0, this.TimeUntilArrival - 1);
		}

		// Token: 0x040023A0 RID: 9120
		public string DeliveryID;

		// Token: 0x040023A1 RID: 9121
		public string StoreName;

		// Token: 0x040023A2 RID: 9122
		public string DestinationCode;

		// Token: 0x040023A3 RID: 9123
		public int LoadingDockIndex;

		// Token: 0x040023A4 RID: 9124
		public StringIntPair[] Items;

		// Token: 0x040023A5 RID: 9125
		public EDeliveryStatus Status;

		// Token: 0x040023A6 RID: 9126
		public int TimeUntilArrival;

		// Token: 0x040023A8 RID: 9128
		[CodegenExclude]
		[NonSerialized]
		public UnityEvent onDeliveryCompleted;
	}
}
