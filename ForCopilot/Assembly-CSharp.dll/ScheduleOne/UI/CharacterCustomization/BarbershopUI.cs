using System;
using HSVPicker;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.CharacterCustomization
{
	// Token: 0x02000B7F RID: 2943
	public class BarbershopUI : CharacterCustomizationUI
	{
		// Token: 0x06004E10 RID: 19984 RVA: 0x0014A3B3 File Offset: 0x001485B3
		public override bool IsOptionCurrentlyApplied(CharacterCustomizationOption option)
		{
			return this.currentSettings.HairStyle == option.Label;
		}

		// Token: 0x06004E11 RID: 19985 RVA: 0x0014A3CB File Offset: 0x001485CB
		public override void OptionSelected(CharacterCustomizationOption option)
		{
			base.OptionSelected(option);
			this.currentSettings.HairStyle = option.Label;
			this.AvatarRig.ApplyHairSettings(this.currentSettings.GetAvatarSettings());
		}

		// Token: 0x06004E12 RID: 19986 RVA: 0x0014A3FB File Offset: 0x001485FB
		protected override void Update()
		{
			base.Update();
			if (!base.IsOpen)
			{
				return;
			}
			this.currentSettings == null;
		}

		// Token: 0x06004E13 RID: 19987 RVA: 0x0014A419 File Offset: 0x00148619
		public override void Open()
		{
			base.Open();
			this.ColorPicker.CurrentColor = this.currentSettings.HairColor;
			this.appliedColor = this.currentSettings.HairColor;
			this.ApplyColorButton.interactable = false;
		}

		// Token: 0x06004E14 RID: 19988 RVA: 0x0014A454 File Offset: 0x00148654
		public void ColorFieldChanged(Color color)
		{
			this.currentSettings.HairColor = color;
			this.AvatarRig.ApplyHairColorSettings(this.currentSettings.GetAvatarSettings());
			this.ApplyColorButton.interactable = true;
		}

		// Token: 0x06004E15 RID: 19989 RVA: 0x0014A484 File Offset: 0x00148684
		public void ApplyColorChange()
		{
			this.appliedColor = this.ColorPicker.CurrentColor;
			this.currentSettings.HairColor = this.appliedColor;
			this.AvatarRig.ApplyHairSettings(this.currentSettings.GetAvatarSettings());
			this.ApplyColorButton.interactable = false;
		}

		// Token: 0x06004E16 RID: 19990 RVA: 0x0014A4D8 File Offset: 0x001486D8
		public void RevertColorChange()
		{
			this.ColorPicker.CurrentColor = this.currentSettings.HairColor;
			this.currentSettings.HairColor = this.appliedColor;
			this.AvatarRig.ApplyHairSettings(this.currentSettings.GetAvatarSettings());
			this.ApplyColorButton.interactable = false;
		}

		// Token: 0x04003A71 RID: 14961
		public ColorPicker ColorPicker;

		// Token: 0x04003A72 RID: 14962
		public Button ApplyColorButton;

		// Token: 0x04003A73 RID: 14963
		private Color appliedColor = Color.black;
	}
}
