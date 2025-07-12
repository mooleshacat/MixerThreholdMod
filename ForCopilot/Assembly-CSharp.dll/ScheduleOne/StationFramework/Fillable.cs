using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ScheduleOne.StationFramework
{
	// Token: 0x020008FC RID: 2300
	public class Fillable : MonoBehaviour
	{
		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x06003E62 RID: 15970 RVA: 0x00106DF0 File Offset: 0x00104FF0
		// (set) Token: 0x06003E63 RID: 15971 RVA: 0x00106DF8 File Offset: 0x00104FF8
		public List<Fillable.Content> contents { get; protected set; } = new List<Fillable.Content>();

		// Token: 0x06003E64 RID: 15972 RVA: 0x00106E01 File Offset: 0x00105001
		private void Awake()
		{
			this.LiquidContainer.SetLiquidLevel(0f, false);
		}

		// Token: 0x06003E65 RID: 15973 RVA: 0x00106E14 File Offset: 0x00105014
		public void AddLiquid(string label, float volume, Color color)
		{
			Fillable.Content content = this.contents.Find((Fillable.Content c) => c.Label == label);
			if (content == null)
			{
				content = new Fillable.Content();
				content.Label = label;
				content.Volume_L = 0f;
				content.Color = color;
				this.contents.Add(content);
			}
			content.Volume_L += volume;
			this.UpdateLiquid();
		}

		// Token: 0x06003E66 RID: 15974 RVA: 0x00106E8D File Offset: 0x0010508D
		public void ResetContents()
		{
			this.contents.Clear();
			this.UpdateLiquid();
		}

		// Token: 0x06003E67 RID: 15975 RVA: 0x00106EA0 File Offset: 0x001050A0
		private void UpdateLiquid()
		{
			float totalVolume = this.contents.Sum((Fillable.Content x) => x.Volume_L);
			this.LiquidContainer.SetLiquidLevel(totalVolume / this.LiquidCapacity_L, false);
			if (totalVolume > 0f)
			{
				Color color = this.contents.Aggregate(Color.clear, (Color acc, Fillable.Content c) => acc + c.Color * c.Volume_L / totalVolume);
				this.LiquidContainer.SetLiquidColor(color, true, true);
			}
		}

		// Token: 0x06003E68 RID: 15976 RVA: 0x00106F34 File Offset: 0x00105134
		public float GetLiquidVolume(string label)
		{
			Fillable.Content content = this.contents.Find((Fillable.Content c) => c.Label == label);
			if (content == null)
			{
				return 0f;
			}
			return content.Volume_L;
		}

		// Token: 0x06003E69 RID: 15977 RVA: 0x00106F75 File Offset: 0x00105175
		public float GetTotalLiquidVolume()
		{
			return this.contents.Sum((Fillable.Content x) => x.Volume_L);
		}

		// Token: 0x04002C7C RID: 11388
		[Header("References")]
		public LiquidContainer LiquidContainer;

		// Token: 0x04002C7D RID: 11389
		[Header("Settings")]
		public bool FillableEnabled = true;

		// Token: 0x04002C7E RID: 11390
		public float LiquidCapacity_L = 1f;

		// Token: 0x020008FD RID: 2301
		public class Content
		{
			// Token: 0x04002C7F RID: 11391
			public string Label;

			// Token: 0x04002C80 RID: 11392
			public float Volume_L;

			// Token: 0x04002C81 RID: 11393
			public Color Color;
		}
	}
}
