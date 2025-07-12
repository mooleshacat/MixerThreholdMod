using System;
using ScheduleOne.Delivery;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000404 RID: 1028
	public class DeliveriesData : SaveData
	{
		// Token: 0x06001623 RID: 5667 RVA: 0x000635F0 File Offset: 0x000617F0
		public DeliveriesData(DeliveryInstance[] deliveries, VehicleData[] deliveryVehicles)
		{
			this.ActiveDeliveries = deliveries;
			this.DeliveryVehicles = deliveryVehicles;
		}

		// Token: 0x040013F3 RID: 5107
		public DeliveryInstance[] ActiveDeliveries;

		// Token: 0x040013F4 RID: 5108
		public VehicleData[] DeliveryVehicles;
	}
}
