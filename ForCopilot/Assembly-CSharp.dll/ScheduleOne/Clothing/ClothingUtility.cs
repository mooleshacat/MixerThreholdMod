using System;
using System.Collections;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.Clothing
{
	// Token: 0x02000780 RID: 1920
	public class ClothingUtility : Singleton<ClothingUtility>
	{
		// Token: 0x060033D5 RID: 13269 RVA: 0x000D7D4C File Offset: 0x000D5F4C
		protected override void Awake()
		{
			base.Awake();
			using (IEnumerator enumerator = Enum.GetValues(typeof(EClothingColor)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					EClothingColor color = (EClothingColor)enumerator.Current;
					if (this.ColorDataList.Find((ClothingUtility.ColorData x) => x.ColorType == color) == null)
					{
						Debug.LogError("Color " + color.ToString() + " is missing from the ColorDataList");
					}
				}
			}
		}

		// Token: 0x060033D6 RID: 13270 RVA: 0x000D7DF8 File Offset: 0x000D5FF8
		private void OnValidate()
		{
			using (IEnumerator enumerator = Enum.GetValues(typeof(EClothingColor)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					EClothingColor color = (EClothingColor)enumerator.Current;
					if (this.ColorDataList.Find((ClothingUtility.ColorData x) => x.ColorType == color) == null)
					{
						this.ColorDataList.Add(new ClothingUtility.ColorData
						{
							ColorType = color,
							ActualColor = Color.white,
							LabelColor = Color.white
						});
					}
				}
			}
			using (IEnumerator enumerator = Enum.GetValues(typeof(EClothingSlot)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					EClothingSlot slot = (EClothingSlot)enumerator.Current;
					if (this.ClothingSlotDataList.Find((ClothingUtility.ClothingSlotData x) => x.Slot == slot) == null)
					{
						this.ClothingSlotDataList.Add(new ClothingUtility.ClothingSlotData
						{
							Slot = slot,
							Name = slot.ToString(),
							Icon = null
						});
					}
				}
			}
		}

		// Token: 0x060033D7 RID: 13271 RVA: 0x000D7F54 File Offset: 0x000D6154
		public ClothingUtility.ColorData GetColorData(EClothingColor color)
		{
			return this.ColorDataList.Find((ClothingUtility.ColorData x) => x.ColorType == color);
		}

		// Token: 0x060033D8 RID: 13272 RVA: 0x000D7F88 File Offset: 0x000D6188
		public ClothingUtility.ClothingSlotData GetSlotData(EClothingSlot slot)
		{
			return this.ClothingSlotDataList.Find((ClothingUtility.ClothingSlotData x) => x.Slot == slot);
		}

		// Token: 0x0400249B RID: 9371
		public List<ClothingUtility.ColorData> ColorDataList = new List<ClothingUtility.ColorData>();

		// Token: 0x0400249C RID: 9372
		public List<ClothingUtility.ClothingSlotData> ClothingSlotDataList = new List<ClothingUtility.ClothingSlotData>();

		// Token: 0x02000781 RID: 1921
		[Serializable]
		public class ColorData
		{
			// Token: 0x0400249D RID: 9373
			public EClothingColor ColorType;

			// Token: 0x0400249E RID: 9374
			public Color ActualColor;

			// Token: 0x0400249F RID: 9375
			public Color LabelColor;
		}

		// Token: 0x02000782 RID: 1922
		[Serializable]
		public class ClothingSlotData
		{
			// Token: 0x040024A0 RID: 9376
			public EClothingSlot Slot;

			// Token: 0x040024A1 RID: 9377
			public string Name;

			// Token: 0x040024A2 RID: 9378
			public Sprite Icon;
		}
	}
}
