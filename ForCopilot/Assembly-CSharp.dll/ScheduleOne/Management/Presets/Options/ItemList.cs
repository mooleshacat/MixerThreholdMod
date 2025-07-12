using System;
using System.Collections.Generic;

namespace ScheduleOne.Management.Presets.Options
{
	// Token: 0x020005D7 RID: 1495
	public class ItemList : Option
	{
		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x060024B8 RID: 9400 RVA: 0x00095E87 File Offset: 0x00094087
		// (set) Token: 0x060024B9 RID: 9401 RVA: 0x00095E8F File Offset: 0x0009408F
		public bool CanBeAll { get; protected set; } = true;

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x060024BA RID: 9402 RVA: 0x00095E98 File Offset: 0x00094098
		// (set) Token: 0x060024BB RID: 9403 RVA: 0x00095EA0 File Offset: 0x000940A0
		public bool CanBeNone { get; protected set; } = true;

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x060024BC RID: 9404 RVA: 0x00095EA9 File Offset: 0x000940A9
		// (set) Token: 0x060024BD RID: 9405 RVA: 0x00095EB1 File Offset: 0x000940B1
		public List<string> OptionList { get; protected set; } = new List<string>();

		// Token: 0x060024BE RID: 9406 RVA: 0x00095EBC File Offset: 0x000940BC
		public ItemList(string name, List<string> optionList, bool canBeAll, bool canBeNone) : base(name)
		{
			this.OptionList.AddRange(optionList);
			this.CanBeAll = canBeAll;
			this.CanBeNone = canBeNone;
		}

		// Token: 0x060024BF RID: 9407 RVA: 0x00095F10 File Offset: 0x00094110
		public override void CopyTo(Option other)
		{
			base.CopyTo(other);
			ItemList itemList = other as ItemList;
			itemList.All = this.All;
			itemList.None = this.None;
			itemList.Selection = new List<string>(this.Selection);
			itemList.CanBeAll = this.CanBeAll;
			itemList.CanBeNone = this.CanBeNone;
			itemList.OptionList = new List<string>(this.OptionList);
		}

		// Token: 0x060024C0 RID: 9408 RVA: 0x00095F7C File Offset: 0x0009417C
		public override string GetDisplayString()
		{
			if (this.All)
			{
				return "All";
			}
			if (this.None || this.Selection.Count == 0)
			{
				return "None";
			}
			List<string> list = new List<string>();
			for (int i = 0; i < this.Selection.Count; i++)
			{
				list.Add(Registry.GetItem(this.Selection[i]).Name);
			}
			return string.Join(", ", list);
		}

		// Token: 0x04001B29 RID: 6953
		public bool All;

		// Token: 0x04001B2A RID: 6954
		public bool None;

		// Token: 0x04001B2B RID: 6955
		public List<string> Selection = new List<string>();
	}
}
