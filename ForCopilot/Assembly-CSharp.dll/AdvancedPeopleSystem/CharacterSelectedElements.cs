using System;

namespace AdvancedPeopleSystem
{
	// Token: 0x02000227 RID: 551
	[Serializable]
	public class CharacterSelectedElements : ICloneable
	{
		// Token: 0x06000BD0 RID: 3024 RVA: 0x00036764 File Offset: 0x00034964
		public object Clone()
		{
			return new CharacterSelectedElements
			{
				Hair = this.Hair,
				Beard = this.Beard,
				Hat = this.Hat,
				Shirt = this.Shirt,
				Pants = this.Pants,
				Shoes = this.Shoes,
				Accessory = this.Accessory,
				Item1 = this.Item1
			};
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x000367D8 File Offset: 0x000349D8
		public int GetSelectedIndex(CharacterElementType type)
		{
			switch (type)
			{
			case CharacterElementType.Hat:
				return this.Hat;
			case CharacterElementType.Shirt:
				return this.Shirt;
			case CharacterElementType.Pants:
				return this.Pants;
			case CharacterElementType.Shoes:
				return this.Shoes;
			case CharacterElementType.Accessory:
				return this.Accessory;
			case CharacterElementType.Hair:
				return this.Hair;
			case CharacterElementType.Beard:
				return this.Beard;
			case CharacterElementType.Item1:
				return this.Item1;
			default:
				return -1;
			}
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x00036848 File Offset: 0x00034A48
		public void SetSelectedIndex(CharacterElementType type, int newIndex)
		{
			switch (type)
			{
			case CharacterElementType.Hat:
				this.Hat = newIndex;
				return;
			case CharacterElementType.Shirt:
				this.Shirt = newIndex;
				return;
			case CharacterElementType.Pants:
				this.Pants = newIndex;
				return;
			case CharacterElementType.Shoes:
				this.Shoes = newIndex;
				return;
			case CharacterElementType.Accessory:
				this.Accessory = newIndex;
				return;
			case CharacterElementType.Hair:
				this.Hair = newIndex;
				return;
			case CharacterElementType.Beard:
				this.Beard = newIndex;
				return;
			case CharacterElementType.Item1:
				this.Item1 = newIndex;
				return;
			default:
				return;
			}
		}

		// Token: 0x04000CC9 RID: 3273
		public int Hair = -1;

		// Token: 0x04000CCA RID: 3274
		public int Beard = -1;

		// Token: 0x04000CCB RID: 3275
		public int Hat = -1;

		// Token: 0x04000CCC RID: 3276
		public int Shirt = -1;

		// Token: 0x04000CCD RID: 3277
		public int Pants = -1;

		// Token: 0x04000CCE RID: 3278
		public int Shoes = -1;

		// Token: 0x04000CCF RID: 3279
		public int Accessory = -1;

		// Token: 0x04000CD0 RID: 3280
		public int Item1 = -1;
	}
}
