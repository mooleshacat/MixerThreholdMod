using System;
using ScheduleOne.GameTime;
using ScheduleOne.Quests;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000402 RID: 1026
	public class CustomerData : SaveData
	{
		// Token: 0x0600161F RID: 5663 RVA: 0x000634E4 File Offset: 0x000616E4
		public CustomerData(float dependence, string[] purchaseableProducts, float[] productAffinities, int timeSinceLastDealCompleted, int timeSinceLastDealOffered, int offeredDeals, int completedDeals, bool isContractOffered, ContractInfo offeredContract, GameDateTime offeredTime, int timeSincePlayerApproached, int timeSinceInstantDealOffered, bool hasBeenRecommended)
		{
			this.Dependence = dependence;
			this.PurchaseableProducts = purchaseableProducts;
			this.ProductAffinities = productAffinities;
			this.TimeSinceLastDealCompleted = timeSinceLastDealCompleted;
			this.TimeSinceLastDealOffered = timeSinceLastDealOffered;
			this.OfferedDeals = offeredDeals;
			this.CompletedDeals = completedDeals;
			this.IsContractOffered = isContractOffered;
			this.OfferedContract = offeredContract;
			this.OfferedContractTime = offeredTime;
			this.TimeSincePlayerApproached = timeSincePlayerApproached;
			this.TimeSinceInstantDealOffered = timeSinceInstantDealOffered;
			this.HasBeenRecommended = hasBeenRecommended;
		}

		// Token: 0x06001620 RID: 5664 RVA: 0x0006355C File Offset: 0x0006175C
		public CustomerData()
		{
		}

		// Token: 0x040013E0 RID: 5088
		public float Dependence;

		// Token: 0x040013E1 RID: 5089
		public string[] PurchaseableProducts;

		// Token: 0x040013E2 RID: 5090
		public float[] ProductAffinities;

		// Token: 0x040013E3 RID: 5091
		public int TimeSinceLastDealCompleted;

		// Token: 0x040013E4 RID: 5092
		public int TimeSinceLastDealOffered;

		// Token: 0x040013E5 RID: 5093
		public int OfferedDeals;

		// Token: 0x040013E6 RID: 5094
		public int CompletedDeals;

		// Token: 0x040013E7 RID: 5095
		public bool IsContractOffered;

		// Token: 0x040013E8 RID: 5096
		public ContractInfo OfferedContract;

		// Token: 0x040013E9 RID: 5097
		public GameDateTime OfferedContractTime;

		// Token: 0x040013EA RID: 5098
		public int TimeSincePlayerApproached;

		// Token: 0x040013EB RID: 5099
		public int TimeSinceInstantDealOffered;

		// Token: 0x040013EC RID: 5100
		public bool HasBeenRecommended;
	}
}
