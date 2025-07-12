using System;
using ScheduleOne.AvatarFramework;

namespace ScheduleOne.UI.CharacterCustomization
{
	// Token: 0x02000B87 RID: 2951
	public class TattooShopUI : CharacterCustomizationUI
	{
		// Token: 0x06004E55 RID: 20053 RVA: 0x0014B2D4 File Offset: 0x001494D4
		public override bool IsOptionCurrentlyApplied(CharacterCustomizationOption option)
		{
			Console.Log("Checking if tattoo is applied: " + option.Label, null);
			Console.Log((this.currentSettings.Tattoos != null) ? string.Join(", ", this.currentSettings.Tattoos.ToArray()) : "No tattoos applied", null);
			return this.currentSettings.Tattoos != null && this.currentSettings.Tattoos.Contains(option.Label);
		}

		// Token: 0x06004E56 RID: 20054 RVA: 0x0014B350 File Offset: 0x00149550
		public override void OptionSelected(CharacterCustomizationOption option)
		{
			base.OptionSelected(option);
			if (!this.currentSettings.Tattoos.Contains(option.Label))
			{
				this.currentSettings.Tattoos.Add(option.Label);
			}
			AvatarSettings avatarSettings = this.currentSettings.GetAvatarSettings();
			this.AvatarRig.ApplyBodyLayerSettings(avatarSettings, 19);
			this.AvatarRig.ApplyFaceLayerSettings(avatarSettings);
		}

		// Token: 0x06004E57 RID: 20055 RVA: 0x0014B3B8 File Offset: 0x001495B8
		public override void OptionDeselected(CharacterCustomizationOption option)
		{
			base.OptionDeselected(option);
			this.currentSettings.Tattoos.Remove(option.Label);
			AvatarSettings avatarSettings = this.currentSettings.GetAvatarSettings();
			this.AvatarRig.ApplyBodyLayerSettings(avatarSettings, 19);
			this.AvatarRig.ApplyFaceLayerSettings(avatarSettings);
		}
	}
}
