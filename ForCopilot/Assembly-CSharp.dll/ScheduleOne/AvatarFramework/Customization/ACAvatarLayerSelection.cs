using System;

namespace ScheduleOne.AvatarFramework.Customization
{
	// Token: 0x020009E0 RID: 2528
	public class ACAvatarLayerSelection : ACSelection<AvatarLayer>
	{
		// Token: 0x06004446 RID: 17478 RVA: 0x0011EE7C File Offset: 0x0011D07C
		public override string GetOptionLabel(int index)
		{
			return this.Options[index].Name;
		}

		// Token: 0x06004447 RID: 17479 RVA: 0x0011EE90 File Offset: 0x0011D090
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

		// Token: 0x06004448 RID: 17480 RVA: 0x0011EF04 File Offset: 0x0011D104
		public override int GetAssetPathIndex(string path)
		{
			AvatarLayer avatarLayer = this.Options.Find((AvatarLayer x) => x.AssetPath == path);
			if (!(avatarLayer != null))
			{
				return -1;
			}
			return this.Options.IndexOf(avatarLayer);
		}
	}
}
