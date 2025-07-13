using System;
using System.Collections.Generic;

namespace ScheduleOne.Police
{
	// Token: 0x0200034A RID: 842
	public class Offense
	{
		// Token: 0x06001286 RID: 4742 RVA: 0x0005001C File Offset: 0x0004E21C
		public Offense(List<Offense.Charge> _charges)
		{
			this.charges.AddRange(_charges);
		}

		// Token: 0x040011C6 RID: 4550
		public List<Offense.Charge> charges = new List<Offense.Charge>();

		// Token: 0x040011C7 RID: 4551
		public List<string> penalties = new List<string>();

		// Token: 0x0200034B RID: 843
		public class Charge
		{
			// Token: 0x06001287 RID: 4743 RVA: 0x00050046 File Offset: 0x0004E246
			public Charge(string _chargeName, int _crimeIndex, int _quantity)
			{
				this.chargeName = _chargeName;
				this.crimeIndex = _crimeIndex;
				this.quantity = _quantity;
			}

			// Token: 0x040011C8 RID: 4552
			public string chargeName = "<ChargeName>";

			// Token: 0x040011C9 RID: 4553
			public int crimeIndex = 1;

			// Token: 0x040011CA RID: 4554
			public int quantity = 1;
		}
	}
}
