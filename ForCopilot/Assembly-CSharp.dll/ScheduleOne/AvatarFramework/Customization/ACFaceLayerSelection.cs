using System;

namespace ScheduleOne.AvatarFramework.Customization
{
	// Token: 0x020009E3 RID: 2531
	public class ACFaceLayerSelection : ACSelection<FaceLayer>
	{
		// Token: 0x0600444E RID: 17486 RVA: 0x0011EF85 File Offset: 0x0011D185
		public override string GetOptionLabel(int index)
		{
			return this.Options[index].Name;
		}

		// Token: 0x0600444F RID: 17487 RVA: 0x0011EF98 File Offset: 0x0011D198
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

		// Token: 0x06004450 RID: 17488 RVA: 0x0011F00C File Offset: 0x0011D20C
		public override int GetAssetPathIndex(string path)
		{
			FaceLayer faceLayer = this.Options.Find((FaceLayer x) => x.AssetPath == path);
			if (!(faceLayer != null))
			{
				return -1;
			}
			return this.Options.IndexOf(faceLayer);
		}
	}
}
