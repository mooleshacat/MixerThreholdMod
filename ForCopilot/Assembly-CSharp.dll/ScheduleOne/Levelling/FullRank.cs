using System;
using UnityEngine;

namespace ScheduleOne.Levelling
{
	// Token: 0x020005E7 RID: 1511
	[Serializable]
	public struct FullRank
	{
		// Token: 0x06002505 RID: 9477 RVA: 0x00096AC6 File Offset: 0x00094CC6
		public FullRank(ERank rank, int tier)
		{
			this.Rank = rank;
			this.Tier = tier;
		}

		// Token: 0x06002506 RID: 9478 RVA: 0x00096AD6 File Offset: 0x00094CD6
		public override string ToString()
		{
			return FullRank.GetString(this);
		}

		// Token: 0x06002507 RID: 9479 RVA: 0x00096AE4 File Offset: 0x00094CE4
		public FullRank NextRank()
		{
			if (this.Rank == ERank.Kingpin)
			{
				return new FullRank(ERank.Kingpin, this.Tier + 1);
			}
			if (this.Tier < 5)
			{
				return new FullRank(this.Rank, this.Tier + 1);
			}
			return new FullRank(this.Rank + 1, 1);
		}

		// Token: 0x06002508 RID: 9480 RVA: 0x00096B38 File Offset: 0x00094D38
		public static string GetString(FullRank rank)
		{
			string text = rank.Rank.ToString();
			text = text.Replace("_", " ");
			switch (rank.Tier)
			{
			case 1:
				text += " I";
				break;
			case 2:
				text += " II";
				break;
			case 3:
				text += " III";
				break;
			case 4:
				text += " IV";
				break;
			case 5:
				text += " V";
				break;
			default:
				text = text + " " + rank.Tier.ToString();
				break;
			}
			return text;
		}

		// Token: 0x06002509 RID: 9481 RVA: 0x00096BED File Offset: 0x00094DED
		public static bool operator >(FullRank a, FullRank b)
		{
			return a.Rank > b.Rank || (a.Rank == b.Rank && a.Tier > b.Tier);
		}

		// Token: 0x0600250A RID: 9482 RVA: 0x00096C1D File Offset: 0x00094E1D
		public static bool operator <(FullRank a, FullRank b)
		{
			return a.Rank < b.Rank || (a.Rank == b.Rank && a.Tier < b.Tier);
		}

		// Token: 0x0600250B RID: 9483 RVA: 0x00096C4D File Offset: 0x00094E4D
		public static bool operator <=(FullRank a, FullRank b)
		{
			return a < b || a == b;
		}

		// Token: 0x0600250C RID: 9484 RVA: 0x00096C61 File Offset: 0x00094E61
		public static bool operator >=(FullRank a, FullRank b)
		{
			return a > b || a == b;
		}

		// Token: 0x0600250D RID: 9485 RVA: 0x00096C75 File Offset: 0x00094E75
		public static bool operator ==(FullRank a, FullRank b)
		{
			return a.Rank == b.Rank && a.Tier == b.Tier;
		}

		// Token: 0x0600250E RID: 9486 RVA: 0x00096C95 File Offset: 0x00094E95
		public static bool operator !=(FullRank a, FullRank b)
		{
			return a.Rank != b.Rank || a.Tier != b.Tier;
		}

		// Token: 0x0600250F RID: 9487 RVA: 0x00096CB8 File Offset: 0x00094EB8
		public override bool Equals(object obj)
		{
			return obj is FullRank && this == (FullRank)obj;
		}

		// Token: 0x06002510 RID: 9488 RVA: 0x00096CD5 File Offset: 0x00094ED5
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06002511 RID: 9489 RVA: 0x00096CE7 File Offset: 0x00094EE7
		public int CompareTo(FullRank other)
		{
			if (this > other)
			{
				return 1;
			}
			if (this < other)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x04001B6A RID: 7018
		public ERank Rank;

		// Token: 0x04001B6B RID: 7019
		[Range(1f, 5f)]
		public int Tier;
	}
}
