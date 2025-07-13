using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200046D RID: 1133
	[Serializable]
	public class TextResponseData
	{
		// Token: 0x060016AA RID: 5802 RVA: 0x00064789 File Offset: 0x00062989
		public TextResponseData(string text, string label)
		{
			this.Text = text;
			this.Label = label;
		}

		// Token: 0x060016AB RID: 5803 RVA: 0x0006479F File Offset: 0x0006299F
		public TextResponseData()
		{
			this.Text = "";
			this.Label = "";
		}

		// Token: 0x040014F2 RID: 5362
		public string Text;

		// Token: 0x040014F3 RID: 5363
		public string Label;
	}
}
