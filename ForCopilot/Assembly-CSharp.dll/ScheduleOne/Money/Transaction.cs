using System;

namespace ScheduleOne.Money
{
	// Token: 0x02000BE1 RID: 3041
	public class Transaction
	{
		// Token: 0x17000B12 RID: 2834
		// (get) Token: 0x060050FC RID: 20732 RVA: 0x00156604 File Offset: 0x00154804
		public float total_Amount
		{
			get
			{
				return this.unit_Amount * this.quantity;
			}
		}

		// Token: 0x060050FD RID: 20733 RVA: 0x00156614 File Offset: 0x00154814
		public Transaction(string _transaction_Name, float _unit_Amount, float _quantity, string _transaction_Note)
		{
			this.transaction_Name = _transaction_Name;
			this.unit_Amount = _unit_Amount;
			this.quantity = _quantity;
			this.transaction_Note = _transaction_Note;
		}

		// Token: 0x04003CBD RID: 15549
		public string transaction_Name = string.Empty;

		// Token: 0x04003CBE RID: 15550
		public float unit_Amount;

		// Token: 0x04003CBF RID: 15551
		public float quantity = 1f;

		// Token: 0x04003CC0 RID: 15552
		public string transaction_Note = string.Empty;
	}
}
