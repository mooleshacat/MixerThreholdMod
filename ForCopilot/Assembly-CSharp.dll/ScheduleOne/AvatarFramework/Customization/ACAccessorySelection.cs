using System;

namespace ScheduleOne.AvatarFramework.Customization
{
	// Token: 0x020009DE RID: 2526
	public class ACAccessorySelection : ACSelection<Accessory>
	{
		// Token: 0x06004440 RID: 17472 RVA: 0x0011ED8F File Offset: 0x0011CF8F
		public override string GetOptionLabel(int index)
		{
			return this.Options[index].Name;
		}

		// Token: 0x06004441 RID: 17473 RVA: 0x0011EDA4 File Offset: 0x0011CFA4
		public override void CallValueChange()
		{
			if (this.onValueChange != null)
			{
				this.onValueChange.Invoke((this.SelectedOptionIndex == -1) ? null : this.Options[this.SelectedOptionIndex]);
			}
			if (this.onValueChangeWithIndex != null)
			{
				this.onValueChangeWithIndex.Invoke((this.SelectedOptionIndex == -1) ? null : this.Options[this.SelectedOptionIndex], this.PropertyIndex);
			}
		}

		// Token: 0x06004442 RID: 17474 RVA: 0x0011EE18 File Offset: 0x0011D018
		public override int GetAssetPathIndex(string path)
		{
			Accessory accessory = this.Options.Find((Accessory x) => x.AssetPath == path);
			if (!(accessory != null))
			{
				return -1;
			}
			return this.Options.IndexOf(accessory);
		}
	}
}
