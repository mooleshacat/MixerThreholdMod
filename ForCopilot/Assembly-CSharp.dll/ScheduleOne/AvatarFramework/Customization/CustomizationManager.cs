using System;
using ScheduleOne.DevUtilities;
using TMPro;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Customization
{
	// Token: 0x020009ED RID: 2541
	public class CustomizationManager : Singleton<CustomizationManager>
	{
		// Token: 0x06004480 RID: 17536 RVA: 0x0011FD4A File Offset: 0x0011DF4A
		protected override void Start()
		{
			base.Start();
			this.LoadSettings(UnityEngine.Object.Instantiate<AvatarSettings>(this.DefaultSettings));
		}

		// Token: 0x06004481 RID: 17537 RVA: 0x000045B1 File Offset: 0x000027B1
		public void CreateSettings(string name)
		{
		}

		// Token: 0x06004482 RID: 17538 RVA: 0x0011FD63 File Offset: 0x0011DF63
		public void CreateSettings()
		{
			if (this.SaveInputField.text == "")
			{
				Console.LogWarning("No name entered for settings file.", null);
				return;
			}
			this.CreateSettings(this.SaveInputField.text);
		}

		// Token: 0x06004483 RID: 17539 RVA: 0x0011FD9C File Offset: 0x0011DF9C
		public void LoadSettings(AvatarSettings loadedSettings)
		{
			if (loadedSettings == null)
			{
				Console.LogWarning("Settings are null!", null);
				return;
			}
			this.ActiveSettings = loadedSettings;
			Debug.Log("Settings loaded: " + this.ActiveSettings.name);
			this.TemplateAvatar.LoadAvatarSettings(this.ActiveSettings);
			if (this.OnAvatarSettingsChanged != null)
			{
				this.OnAvatarSettingsChanged(this.ActiveSettings);
			}
		}

		// Token: 0x06004484 RID: 17540 RVA: 0x0011FE0C File Offset: 0x0011E00C
		public void LoadSettings(string settingsName, bool editOriginal = false)
		{
			this.isEditingOriginal = editOriginal;
			AvatarSettings loadedSettings;
			if (editOriginal)
			{
				loadedSettings = Resources.Load<AvatarSettings>("CharacterSettings/" + settingsName);
				this.SaveInputField.SetTextWithoutNotify(settingsName);
			}
			else
			{
				loadedSettings = UnityEngine.Object.Instantiate<AvatarSettings>(Resources.Load<AvatarSettings>("CharacterSettings/" + settingsName));
			}
			this.LoadSettings(loadedSettings);
		}

		// Token: 0x06004485 RID: 17541 RVA: 0x0011FE64 File Offset: 0x0011E064
		private void ApplyDefaultSettings(AvatarSettings settings)
		{
			settings.SkinColor = new Color32(150, 120, 95, byte.MaxValue);
			settings.Height = 0.98f;
			settings.Gender = 0f;
			settings.Weight = 0.4f;
			settings.EyebrowScale = 1f;
			settings.EyebrowThickness = 1f;
			settings.EyebrowRestingHeight = 0f;
			settings.EyebrowRestingAngle = 0f;
			settings.LeftEyeLidColor = new Color32(150, 120, 95, byte.MaxValue);
			settings.RightEyeLidColor = new Color32(150, 120, 95, byte.MaxValue);
			settings.LeftEyeRestingState = new Eye.EyeLidConfiguration
			{
				bottomLidOpen = 0.5f,
				topLidOpen = 0.5f
			};
			settings.RightEyeRestingState = new Eye.EyeLidConfiguration
			{
				bottomLidOpen = 0.5f,
				topLidOpen = 0.5f
			};
			settings.EyeballMaterialIdentifier = "Default";
			settings.EyeBallTint = Color.white;
			settings.PupilDilation = 1f;
			settings.HairPath = string.Empty;
			settings.HairColor = Color.black;
		}

		// Token: 0x06004486 RID: 17542 RVA: 0x0011FFA0 File Offset: 0x0011E1A0
		public void LoadSettings()
		{
			this.isEditingOriginal = true;
			Debug.Log("Loading!: " + this.LoadInputField.text);
			this.LoadSettings(this.LoadInputField.text, this.LoadInputField.text != "Default");
		}

		// Token: 0x06004487 RID: 17543 RVA: 0x0011FFF4 File Offset: 0x0011E1F4
		public void GenderChanged(float genderScale)
		{
			this.ActiveSettings.Gender = genderScale;
			this.TemplateAvatar.ApplyBodySettings(this.ActiveSettings);
		}

		// Token: 0x06004488 RID: 17544 RVA: 0x00120013 File Offset: 0x0011E213
		public void WeightChanged(float weightScale)
		{
			this.ActiveSettings.Weight = weightScale;
			this.TemplateAvatar.ApplyBodySettings(this.ActiveSettings);
		}

		// Token: 0x06004489 RID: 17545 RVA: 0x00120032 File Offset: 0x0011E232
		public void HeightChanged(float height)
		{
			this.ActiveSettings.Height = height;
			this.TemplateAvatar.ApplyBodySettings(this.ActiveSettings);
		}

		// Token: 0x0600448A RID: 17546 RVA: 0x00120054 File Offset: 0x0011E254
		public void SkinColorChanged(Color col)
		{
			this.ActiveSettings.SkinColor = col;
			this.TemplateAvatar.ApplyBodySettings(this.ActiveSettings);
			if (Input.GetKey(KeyCode.LeftControl))
			{
				this.ActiveSettings.LeftEyeLidColor = col;
				this.ActiveSettings.RightEyeLidColor = col;
			}
			this.TemplateAvatar.ApplyEyeLidColorSettings(this.ActiveSettings);
		}

		// Token: 0x0600448B RID: 17547 RVA: 0x001200B3 File Offset: 0x0011E2B3
		public void HairChanged(Accessory newHair)
		{
			this.ActiveSettings.HairPath = ((newHair != null) ? newHair.AssetPath : string.Empty);
			this.TemplateAvatar.ApplyHairSettings(this.ActiveSettings);
		}

		// Token: 0x0600448C RID: 17548 RVA: 0x001200E7 File Offset: 0x0011E2E7
		public void HairColorChanged(Color newCol)
		{
			this.ActiveSettings.HairColor = newCol;
			this.TemplateAvatar.ApplyHairColorSettings(this.ActiveSettings);
		}

		// Token: 0x0600448D RID: 17549 RVA: 0x00120106 File Offset: 0x0011E306
		public void EyeBallTintChanged(Color col)
		{
			this.ActiveSettings.EyeBallTint = col;
			this.TemplateAvatar.ApplyEyeBallSettings(this.ActiveSettings);
		}

		// Token: 0x0600448E RID: 17550 RVA: 0x00120125 File Offset: 0x0011E325
		public void UpperEyeLidRestingPositionChanged(float newVal)
		{
			this.ActiveSettings.LeftEyeRestingState.topLidOpen = newVal;
			this.ActiveSettings.RightEyeRestingState.topLidOpen = newVal;
			this.TemplateAvatar.ApplyEyeLidSettings(this.ActiveSettings);
		}

		// Token: 0x0600448F RID: 17551 RVA: 0x0012015A File Offset: 0x0011E35A
		public void LowerEyeLidRestingPositionChanged(float newVal)
		{
			this.ActiveSettings.LeftEyeRestingState.bottomLidOpen = newVal;
			this.ActiveSettings.RightEyeRestingState.bottomLidOpen = newVal;
			this.TemplateAvatar.ApplyEyeLidSettings(this.ActiveSettings);
		}

		// Token: 0x06004490 RID: 17552 RVA: 0x0012018F File Offset: 0x0011E38F
		public void EyebrowScaleChanged(float newVal)
		{
			this.ActiveSettings.EyebrowScale = newVal;
			this.TemplateAvatar.ApplyEyebrowSettings(this.ActiveSettings);
		}

		// Token: 0x06004491 RID: 17553 RVA: 0x001201AE File Offset: 0x0011E3AE
		public void EyebrowThicknessChanged(float newVal)
		{
			this.ActiveSettings.EyebrowThickness = newVal;
			this.TemplateAvatar.ApplyEyebrowSettings(this.ActiveSettings);
		}

		// Token: 0x06004492 RID: 17554 RVA: 0x001201CD File Offset: 0x0011E3CD
		public void EyebrowRestingHeightChanged(float newVal)
		{
			this.ActiveSettings.EyebrowRestingHeight = newVal;
			this.TemplateAvatar.ApplyEyebrowSettings(this.ActiveSettings);
		}

		// Token: 0x06004493 RID: 17555 RVA: 0x001201EC File Offset: 0x0011E3EC
		public void EyebrowRestingAngleChanged(float newVal)
		{
			this.ActiveSettings.EyebrowRestingAngle = newVal;
			this.TemplateAvatar.ApplyEyebrowSettings(this.ActiveSettings);
		}

		// Token: 0x06004494 RID: 17556 RVA: 0x0012020B File Offset: 0x0011E40B
		public void PupilDilationChanged(float dilation)
		{
			this.ActiveSettings.PupilDilation = dilation;
			this.TemplateAvatar.ApplyEyeBallSettings(this.ActiveSettings);
		}

		// Token: 0x06004495 RID: 17557 RVA: 0x0012022C File Offset: 0x0011E42C
		public void FaceLayerChanged(FaceLayer layer, int index)
		{
			string layerPath = (layer != null) ? layer.AssetPath : string.Empty;
			Color layerTint = this.ActiveSettings.FaceLayerSettings[index].layerTint;
			this.ActiveSettings.FaceLayerSettings[index] = new AvatarSettings.LayerSetting
			{
				layerPath = layerPath,
				layerTint = layerTint
			};
			this.TemplateAvatar.ApplyFaceLayerSettings(this.ActiveSettings);
		}

		// Token: 0x06004496 RID: 17558 RVA: 0x001202A4 File Offset: 0x0011E4A4
		public void FaceLayerColorChanged(Color col, int index)
		{
			string layerPath = this.ActiveSettings.FaceLayerSettings[index].layerPath;
			this.ActiveSettings.FaceLayerSettings[index] = new AvatarSettings.LayerSetting
			{
				layerPath = layerPath,
				layerTint = col
			};
			this.TemplateAvatar.ApplyFaceLayerSettings(this.ActiveSettings);
		}

		// Token: 0x06004497 RID: 17559 RVA: 0x00120304 File Offset: 0x0011E504
		public void BodyLayerChanged(AvatarLayer layer, int index)
		{
			string layerPath = (layer != null) ? layer.AssetPath : string.Empty;
			Color layerTint = this.ActiveSettings.BodyLayerSettings[index].layerTint;
			this.ActiveSettings.BodyLayerSettings[index] = new AvatarSettings.LayerSetting
			{
				layerPath = layerPath,
				layerTint = layerTint
			};
			this.TemplateAvatar.ApplyBodyLayerSettings(this.ActiveSettings, -1);
		}

		// Token: 0x06004498 RID: 17560 RVA: 0x0012037C File Offset: 0x0011E57C
		public void BodyLayerColorChanged(Color col, int index)
		{
			string layerPath = this.ActiveSettings.BodyLayerSettings[index].layerPath;
			this.ActiveSettings.BodyLayerSettings[index] = new AvatarSettings.LayerSetting
			{
				layerPath = layerPath,
				layerTint = col
			};
			this.TemplateAvatar.ApplyBodyLayerSettings(this.ActiveSettings, -1);
		}

		// Token: 0x06004499 RID: 17561 RVA: 0x001203DC File Offset: 0x0011E5DC
		public void AccessoryChanged(Accessory acc, int index)
		{
			Debug.Log("Accessory changed: " + ((acc != null) ? acc.AssetPath : null));
			string path = (acc != null) ? acc.AssetPath : string.Empty;
			while (this.ActiveSettings.AccessorySettings.Count <= index)
			{
				this.ActiveSettings.AccessorySettings.Add(new AvatarSettings.AccessorySetting());
			}
			Color color = this.ActiveSettings.AccessorySettings[index].color;
			this.ActiveSettings.AccessorySettings[index] = new AvatarSettings.AccessorySetting
			{
				path = path,
				color = color
			};
			this.TemplateAvatar.ApplyAccessorySettings(this.ActiveSettings);
		}

		// Token: 0x0600449A RID: 17562 RVA: 0x00120494 File Offset: 0x0011E694
		public void AccessoryColorChanged(Color col, int index)
		{
			string path = this.ActiveSettings.AccessorySettings[index].path;
			this.ActiveSettings.AccessorySettings[index] = new AvatarSettings.AccessorySetting
			{
				path = path,
				color = col
			};
			this.TemplateAvatar.ApplyAccessorySettings(this.ActiveSettings);
		}

		// Token: 0x04003176 RID: 12662
		[SerializeField]
		private AvatarSettings ActiveSettings;

		// Token: 0x04003177 RID: 12663
		public Avatar TemplateAvatar;

		// Token: 0x04003178 RID: 12664
		public TMP_InputField SaveInputField;

		// Token: 0x04003179 RID: 12665
		public TMP_InputField LoadInputField;

		// Token: 0x0400317A RID: 12666
		public CustomizationManager.AvatarSettingsChanged OnAvatarSettingsChanged;

		// Token: 0x0400317B RID: 12667
		public AvatarSettings DefaultSettings;

		// Token: 0x0400317C RID: 12668
		private bool isEditingOriginal;

		// Token: 0x020009EE RID: 2542
		// (Invoke) Token: 0x0600449D RID: 17565
		public delegate void AvatarSettingsChanged(AvatarSettings settings);
	}
}
