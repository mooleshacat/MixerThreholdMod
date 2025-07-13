using System;
using ScheduleOne.AvatarFramework.Customization;
using ScheduleOne.Clothing;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.UI.CharacterCreator
{
	// Token: 0x02000B8B RID: 2955
	public class CharacterCreatorField<T> : BaseCharacterCreatorField
	{
		// Token: 0x17000ABF RID: 2751
		// (get) Token: 0x06004E65 RID: 20069 RVA: 0x0014B5FC File Offset: 0x001497FC
		// (set) Token: 0x06004E66 RID: 20070 RVA: 0x0014B604 File Offset: 0x00149804
		public T value { get; protected set; }

		// Token: 0x06004E67 RID: 20071 RVA: 0x0014B60D File Offset: 0x0014980D
		public virtual T ReadValue()
		{
			return Singleton<CharacterCreator>.Instance.ActiveSettings.GetValue<T>(this.PropertyName);
		}

		// Token: 0x06004E68 RID: 20072 RVA: 0x0014B624 File Offset: 0x00149824
		public override void WriteValue(bool applyValue = true)
		{
			base.WriteValue(applyValue);
			Singleton<CharacterCreator>.Instance.SetValue<T>(this.PropertyName, this.value, this.selectedClothingDefinition);
			Singleton<CharacterCreator>.Instance.RefreshCategory(this.Category);
			if (applyValue)
			{
				this.ApplyValue();
			}
		}

		// Token: 0x06004E69 RID: 20073 RVA: 0x0014B663 File Offset: 0x00149863
		public override void ApplyValue()
		{
			base.ApplyValue();
			this.value = this.ReadValue();
		}

		// Token: 0x04003AB8 RID: 15032
		protected ClothingDefinition selectedClothingDefinition;
	}
}
