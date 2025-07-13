using System;
using System.Collections.Generic;
using EasyButtons;
using ScheduleOne.Audio;
using ScheduleOne.AvatarFramework.Animation;
using ScheduleOne.AvatarFramework.Emotions;
using ScheduleOne.AvatarFramework.Equipping;
using ScheduleOne.AvatarFramework.Impostors;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.AvatarFramework
{
	// Token: 0x0200099A RID: 2458
	public class Avatar : MonoBehaviour
	{
		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x0600425C RID: 16988 RVA: 0x00116F95 File Offset: 0x00115195
		// (set) Token: 0x0600425D RID: 16989 RVA: 0x00116F9D File Offset: 0x0011519D
		public bool Ragdolled { get; protected set; }

		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x0600425E RID: 16990 RVA: 0x00116FA6 File Offset: 0x001151A6
		// (set) Token: 0x0600425F RID: 16991 RVA: 0x00116FAE File Offset: 0x001151AE
		public AvatarEquippable CurrentEquippable { get; protected set; }

		// Token: 0x17000932 RID: 2354
		// (get) Token: 0x06004260 RID: 16992 RVA: 0x00116FB7 File Offset: 0x001151B7
		// (set) Token: 0x06004261 RID: 16993 RVA: 0x00116FBF File Offset: 0x001151BF
		public AvatarSettings CurrentSettings { get; protected set; }

		// Token: 0x06004262 RID: 16994 RVA: 0x00116FC8 File Offset: 0x001151C8
		[Button]
		public void Load()
		{
			this.LoadAvatarSettings(this.SettingsToLoad);
		}

		// Token: 0x06004263 RID: 16995 RVA: 0x00116FD6 File Offset: 0x001151D6
		[Button]
		public void LoadNaked()
		{
			this.LoadNakedSettings(this.SettingsToLoad, 19);
		}

		// Token: 0x17000933 RID: 2355
		// (get) Token: 0x06004264 RID: 16996 RVA: 0x00116FE6 File Offset: 0x001151E6
		public Vector3 CenterPoint
		{
			get
			{
				return this.MiddleSpine.transform.position;
			}
		}

		// Token: 0x06004265 RID: 16997 RVA: 0x00116FF8 File Offset: 0x001151F8
		protected virtual void Awake()
		{
			this.SetRagdollPhysicsEnabled(false, false);
			this.originalHipPos = this.HipBone.localPosition;
			if (this.InitialAvatarSettings != null)
			{
				this.LoadAvatarSettings(this.InitialAvatarSettings);
			}
		}

		// Token: 0x06004266 RID: 16998 RVA: 0x0011702D File Offset: 0x0011522D
		protected virtual void Update()
		{
			if (!this.Ragdolled && this.Anim != null && !this.Anim.StandUpAnimationPlaying)
			{
				this.HipBone.localPosition = this.originalHipPos;
			}
		}

		// Token: 0x06004267 RID: 16999 RVA: 0x00117064 File Offset: 0x00115264
		protected virtual void LateUpdate()
		{
			if (!this.BodyContainer.gameObject.activeInHierarchy)
			{
				return;
			}
			if (this.CurrentSettings != null && !this.Anim.IsAvatarCulled)
			{
				Vector3 centerPoint = this.CenterPoint;
				if (PlayerSingleton<PlayerCamera>.InstanceExists && Vector3.SqrMagnitude(PlayerSingleton<PlayerCamera>.Instance.transform.position - this.CenterPoint) < 1600f * QualitySettings.lodBias)
				{
					this.ApplyShapeKeys(Mathf.Clamp01(this.appliedGender + this.additionalGender) * 100f, Mathf.Clamp01(this.appliedWeight + this.additionalWeight) * 100f, false);
				}
			}
		}

		// Token: 0x06004268 RID: 17000 RVA: 0x00117110 File Offset: 0x00115310
		public void SetVisible(bool vis)
		{
			this.Eyes.SetEyesOpen(true);
			this.BodyContainer.gameObject.SetActive(vis);
		}

		// Token: 0x06004269 RID: 17001 RVA: 0x0011712F File Offset: 0x0011532F
		public void GetMugshot(Action<Texture2D> callback)
		{
			Singleton<MugshotGenerator>.Instance.GenerateMugshot(this.CurrentSettings, false, callback);
		}

		// Token: 0x0600426A RID: 17002 RVA: 0x00117144 File Offset: 0x00115344
		public void SetEmission(Color color)
		{
			if (this.usingCombinedLayer)
			{
				this.BodyMeshes[0].sharedMaterial.SetColor("_EmissionColor", color);
				return;
			}
			SkinnedMeshRenderer[] bodyMeshes = this.BodyMeshes;
			for (int i = 0; i < bodyMeshes.Length; i++)
			{
				bodyMeshes[i].material.SetColor("_EmissionColor", color);
			}
		}

		// Token: 0x0600426B RID: 17003 RVA: 0x0011719A File Offset: 0x0011539A
		public bool IsMale()
		{
			return this.CurrentSettings == null || this.CurrentSettings.Gender < 0.5f;
		}

		// Token: 0x0600426C RID: 17004 RVA: 0x001171C0 File Offset: 0x001153C0
		public bool IsWhite()
		{
			return this.CurrentSettings == null || this.CurrentSettings.SkinColor.r + this.CurrentSettings.SkinColor.g + this.CurrentSettings.SkinColor.b > 1.5f;
		}

		// Token: 0x0600426D RID: 17005 RVA: 0x00117216 File Offset: 0x00115416
		public string GetFormalAddress(bool capitalized = true)
		{
			if (this.IsMale())
			{
				if (!capitalized)
				{
					return "sir";
				}
				return "Sir";
			}
			else
			{
				if (!capitalized)
				{
					return "ma'am";
				}
				return "Ma'am";
			}
		}

		// Token: 0x0600426E RID: 17006 RVA: 0x0011723D File Offset: 0x0011543D
		public string GetThirdPersonAddress(bool capitalized = true)
		{
			if (this.IsMale())
			{
				if (!capitalized)
				{
					return "he";
				}
				return "He";
			}
			else
			{
				if (!capitalized)
				{
					return "she";
				}
				return "She";
			}
		}

		// Token: 0x0600426F RID: 17007 RVA: 0x00117264 File Offset: 0x00115464
		public string GetThirdPersonPronoun(bool capitalized = true)
		{
			if (this.IsMale())
			{
				if (!capitalized)
				{
					return "him";
				}
				return "Him";
			}
			else
			{
				if (!capitalized)
				{
					return "her";
				}
				return "Her";
			}
		}

		// Token: 0x06004270 RID: 17008 RVA: 0x0011728C File Offset: 0x0011548C
		private void ApplyShapeKeys(float gender, float weight, bool bodyOnly = false)
		{
			bool enabled = true;
			if (this.Anim.animator != null)
			{
				enabled = this.Anim.animator.enabled;
				this.Anim.animator.enabled = false;
			}
			for (int i = 0; i < this.ShapeKeyMeshes.Length; i++)
			{
				if (this.ShapeKeyMeshes[i].sharedMesh.blendShapeCount >= 2)
				{
					this.ShapeKeyMeshes[i].SetBlendShapeWeight(0, gender);
					this.ShapeKeyMeshes[i].SetBlendShapeWeight(1, weight);
				}
			}
			float num = Mathf.Lerp(Avatar.maleShoulderScale, Avatar.femaleShoulderScale, gender / 100f);
			this.LeftShoulder.localScale = new Vector3(num, num, num);
			this.RightShoulder.localScale = new Vector3(num, num, num);
			if (this.Anim.animator != null)
			{
				this.Anim.animator.enabled = enabled;
			}
			if (bodyOnly)
			{
				return;
			}
			for (int j = 0; j < this.appliedAccessories.Length; j++)
			{
				if (this.appliedAccessories[j] != null)
				{
					this.appliedAccessories[j].ApplyShapeKeys(gender, weight);
				}
			}
		}

		// Token: 0x06004271 RID: 17009 RVA: 0x001173B0 File Offset: 0x001155B0
		private void SetFeetShrunk(bool shrink, float reduction)
		{
			if (shrink)
			{
				for (int i = 0; i < this.BodyMeshes.Length; i++)
				{
					this.BodyMeshes[i].SetBlendShapeWeight(2, reduction * 100f);
				}
				return;
			}
			for (int j = 0; j < this.BodyMeshes.Length; j++)
			{
				this.BodyMeshes[j].SetBlendShapeWeight(2, 0f);
			}
		}

		// Token: 0x06004272 RID: 17010 RVA: 0x0011740F File Offset: 0x0011560F
		private void SetWearingHairBlockingAccessory(bool blocked)
		{
			this.wearingHairBlockingAccessory = blocked;
			if (this.appliedHair != null)
			{
				this.appliedHair.SetBlockedByHat(blocked);
			}
		}

		// Token: 0x06004273 RID: 17011 RVA: 0x00117434 File Offset: 0x00115634
		public void LoadAvatarSettings(AvatarSettings settings)
		{
			if (settings == null)
			{
				Console.LogWarning("LoadAvatarSettings: given settings are null", null);
				return;
			}
			this.CurrentSettings = settings;
			this.ApplyBodySettings(this.CurrentSettings);
			this.ApplyHairSettings(this.CurrentSettings);
			this.ApplyHairColorSettings(this.CurrentSettings);
			this.ApplyEyeLidSettings(this.CurrentSettings);
			this.ApplyEyeLidColorSettings(this.CurrentSettings);
			this.ApplyEyebrowSettings(this.CurrentSettings);
			this.ApplyEyeBallSettings(this.CurrentSettings);
			this.ApplyFaceLayerSettings(this.CurrentSettings);
			this.ApplyBodyLayerSettings(this.CurrentSettings, -1);
			this.ApplyAccessorySettings(this.CurrentSettings);
			FaceLayer faceLayer = Resources.Load(this.CurrentSettings.FaceLayer1Path) as FaceLayer;
			Texture2D faceTex = (faceLayer != null) ? faceLayer.Texture : null;
			this.EmotionManager.ConfigureNeutralFace(faceTex, this.CurrentSettings.EyebrowRestingHeight, this.CurrentSettings.EyebrowRestingAngle, this.CurrentSettings.LeftEyeRestingState, this.CurrentSettings.RightEyeRestingState);
			if (this.UseImpostor)
			{
				this.Impostor.SetAvatarSettings(this.CurrentSettings);
			}
			if (this.onSettingsLoaded != null)
			{
				this.onSettingsLoaded.Invoke();
			}
		}

		// Token: 0x06004274 RID: 17012 RVA: 0x00117564 File Offset: 0x00115764
		public void LoadNakedSettings(AvatarSettings settings, int maxLayerOrder = 19)
		{
			if (settings == null)
			{
				Console.LogWarning("LoadAvatarSettings: given settings are null", null);
				return;
			}
			AvatarSettings currentSettings = this.CurrentSettings;
			this.CurrentSettings = settings;
			if (this.CurrentSettings == null)
			{
				this.CurrentSettings = new AvatarSettings();
			}
			this.CurrentSettings = UnityEngine.Object.Instantiate<AvatarSettings>(this.CurrentSettings);
			if (currentSettings != null)
			{
				this.CurrentSettings.BodyLayerSettings.AddRange(currentSettings.BodyLayerSettings);
			}
			this.ApplyBodySettings(this.CurrentSettings);
			this.ApplyHairSettings(this.CurrentSettings);
			this.ApplyHairColorSettings(this.CurrentSettings);
			this.ApplyEyeLidSettings(this.CurrentSettings);
			this.ApplyEyeLidColorSettings(this.CurrentSettings);
			this.ApplyEyebrowSettings(this.CurrentSettings);
			this.ApplyEyeBallSettings(this.CurrentSettings);
			this.ApplyFaceLayerSettings(this.CurrentSettings);
			this.ApplyBodyLayerSettings(this.CurrentSettings, maxLayerOrder);
			FaceLayer faceLayer = Resources.Load(this.CurrentSettings.FaceLayer1Path) as FaceLayer;
			Texture2D faceTex = (faceLayer != null) ? faceLayer.Texture : null;
			this.EmotionManager.ConfigureNeutralFace(faceTex, this.CurrentSettings.EyebrowRestingHeight, this.CurrentSettings.EyebrowRestingAngle, this.CurrentSettings.LeftEyeRestingState, this.CurrentSettings.RightEyeRestingState);
			if (this.onSettingsLoaded != null)
			{
				this.onSettingsLoaded.Invoke();
			}
		}

		// Token: 0x06004275 RID: 17013 RVA: 0x001176C0 File Offset: 0x001158C0
		public void ApplyBodySettings(AvatarSettings settings)
		{
			this.appliedGender = settings.Gender;
			this.appliedWeight = settings.Weight;
			this.CurrentSettings.SkinColor = settings.SkinColor;
			this.ApplyShapeKeys(settings.Gender * 100f, settings.Weight * 100f, false);
			base.transform.localScale = new Vector3(settings.Height, settings.Height, settings.Height);
			if (this.onSettingsLoaded != null)
			{
				this.onSettingsLoaded.Invoke();
			}
		}

		// Token: 0x06004276 RID: 17014 RVA: 0x0011774A File Offset: 0x0011594A
		public void SetAdditionalWeight(float weight)
		{
			this.additionalWeight = weight;
		}

		// Token: 0x06004277 RID: 17015 RVA: 0x00117753 File Offset: 0x00115953
		public void SetAdditionalGender(float gender)
		{
			this.additionalGender = gender;
		}

		// Token: 0x06004278 RID: 17016 RVA: 0x0011775C File Offset: 0x0011595C
		public void SetSkinColor(Color color)
		{
			if (this.usingCombinedLayer)
			{
				if (this.BodyMeshes[0].sharedMaterial.GetColor("_SkinColor") == color)
				{
					return;
				}
				this.BodyMeshes[0].sharedMaterial.SetColor("_SkinColor", color);
			}
			else
			{
				if (this.BodyMeshes[0].material.GetColor("_SkinColor") == color)
				{
					return;
				}
				SkinnedMeshRenderer[] bodyMeshes = this.BodyMeshes;
				for (int i = 0; i < bodyMeshes.Length; i++)
				{
					bodyMeshes[i].material.SetColor("_SkinColor", color);
				}
			}
			this.Eyes.leftEye.SetLidColor(color);
			this.Eyes.rightEye.SetLidColor(color);
		}

		// Token: 0x06004279 RID: 17017 RVA: 0x00117818 File Offset: 0x00115A18
		public void ApplyHairSettings(AvatarSettings settings)
		{
			if (this.appliedHair != null)
			{
				UnityEngine.Object.Destroy(this.appliedHair.gameObject);
			}
			UnityEngine.Object @object = (settings.HairPath != null) ? Resources.Load(settings.HairPath) : null;
			if (@object != null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(@object, this.HeadBone) as GameObject;
				this.appliedHair = gameObject.GetComponent<Hair>();
			}
			this.ApplyHairColorSettings(settings);
			if (this.appliedHair != null)
			{
				this.appliedHair.SetBlockedByHat(this.wearingHairBlockingAccessory);
			}
		}

		// Token: 0x0600427A RID: 17018 RVA: 0x001178A7 File Offset: 0x00115AA7
		public void SetHairVisible(bool visible)
		{
			if (this.appliedHair != null)
			{
				this.appliedHair.gameObject.SetActive(visible);
			}
		}

		// Token: 0x0600427B RID: 17019 RVA: 0x001178C8 File Offset: 0x00115AC8
		public void ApplyHairColorSettings(AvatarSettings settings)
		{
			this.appliedHairColor = settings.HairColor;
			if (this.appliedHair != null)
			{
				this.appliedHair.ApplyColor(this.appliedHairColor);
			}
			this.EyeBrows.ApplySettings(settings);
			this.SetFaceLayer(2, settings.FaceLayer2Path, settings.HairColor);
		}

		// Token: 0x0600427C RID: 17020 RVA: 0x00117920 File Offset: 0x00115B20
		public void OverrideHairColor(Color color)
		{
			if (this.appliedHair != null)
			{
				this.appliedHair.ApplyColor(color);
			}
			this.EyeBrows.leftBrow.SetColor(color);
			this.EyeBrows.rightBrow.SetColor(color);
			if (this.CurrentSettings != null)
			{
				this.SetFaceLayer(2, this.CurrentSettings.FaceLayer2Path, color);
			}
		}

		// Token: 0x0600427D RID: 17021 RVA: 0x0011798C File Offset: 0x00115B8C
		public void ResetHairColor()
		{
			if (this.CurrentSettings == null)
			{
				return;
			}
			if (this.appliedHair != null)
			{
				this.appliedHair.ApplyColor(this.CurrentSettings.HairColor);
			}
			this.EyeBrows.leftBrow.SetColor(this.CurrentSettings.HairColor);
			this.EyeBrows.rightBrow.SetColor(this.CurrentSettings.HairColor);
			this.SetFaceLayer(2, this.CurrentSettings.FaceLayer2Path, this.CurrentSettings.HairColor);
		}

		// Token: 0x0600427E RID: 17022 RVA: 0x00117A1F File Offset: 0x00115C1F
		public void ApplyEyeBallSettings(AvatarSettings settings)
		{
			this.Eyes.SetEyeballTint(settings.EyeBallTint);
			this.Eyes.SetPupilDilation(settings.PupilDilation, true);
		}

		// Token: 0x0600427F RID: 17023 RVA: 0x00117A44 File Offset: 0x00115C44
		public void ApplyEyeLidSettings(AvatarSettings settings)
		{
			this.Eyes.SetLeftEyeRestingLidState(settings.LeftEyeRestingState);
			this.Eyes.SetRightEyeRestingLidState(settings.RightEyeRestingState);
		}

		// Token: 0x06004280 RID: 17024 RVA: 0x00117A68 File Offset: 0x00115C68
		public void ApplyEyeLidColorSettings(AvatarSettings settings)
		{
			this.Eyes.leftEye.SetLidColor(settings.LeftEyeLidColor);
			this.Eyes.rightEye.SetLidColor(settings.RightEyeLidColor);
		}

		// Token: 0x06004281 RID: 17025 RVA: 0x00117A96 File Offset: 0x00115C96
		public void ApplyEyebrowSettings(AvatarSettings settings)
		{
			this.EyeBrows.ApplySettings(settings);
		}

		// Token: 0x06004282 RID: 17026 RVA: 0x00117AA4 File Offset: 0x00115CA4
		public void SetBlockEyeFaceLayers(bool block)
		{
			this.blockEyeFaceLayers = block;
			if (this.CurrentSettings != null)
			{
				this.ApplyFaceLayerSettings(this.CurrentSettings);
			}
		}

		// Token: 0x06004283 RID: 17027 RVA: 0x00117AC8 File Offset: 0x00115CC8
		public void ApplyFaceLayerSettings(AvatarSettings settings)
		{
			for (int i = 1; i <= 6; i++)
			{
				this.SetFaceLayer(i, string.Empty, Color.white);
			}
			this.SetFaceLayer(1, settings.FaceLayer1Path, settings.FaceLayer1Color);
			this.SetFaceLayer(6, settings.FaceLayer2Path, settings.HairColor);
			List<Tuple<FaceLayer, Color>> list = new List<Tuple<FaceLayer, Color>>();
			for (int j = 2; j < settings.FaceLayerSettings.Count; j++)
			{
				if (!string.IsNullOrEmpty(settings.FaceLayerSettings[j].layerPath))
				{
					FaceLayer faceLayer = Resources.Load(settings.FaceLayerSettings[j].layerPath) as FaceLayer;
					if (!this.blockEyeFaceLayers || !faceLayer.Name.ToLower().Contains("eye"))
					{
						if (faceLayer != null)
						{
							list.Add(new Tuple<FaceLayer, Color>(faceLayer, settings.FaceLayerSettings[j].layerTint));
						}
						else
						{
							Console.LogWarning("Face layer not found at path " + settings.FaceLayerSettings[j].layerPath, null);
						}
					}
				}
			}
			list.Sort((Tuple<FaceLayer, Color> x, Tuple<FaceLayer, Color> y) => x.Item1.Order.CompareTo(y.Item1.Order));
			for (int k = 0; k < list.Count; k++)
			{
				this.SetFaceLayer(3 + k, list[k].Item1.AssetPath, list[k].Item2);
			}
		}

		// Token: 0x06004284 RID: 17028 RVA: 0x00117C3C File Offset: 0x00115E3C
		private void SetFaceLayer(int index, string assetPath, Color color)
		{
			FaceLayer faceLayer = Resources.Load(assetPath) as FaceLayer;
			Texture2D texture2D = (faceLayer != null) ? faceLayer.Texture : null;
			if (texture2D == null)
			{
				color.a = 0f;
			}
			this.FaceMesh.material.SetTexture("_Layer_" + index.ToString() + "_Texture", texture2D);
			this.FaceMesh.material.SetColor("_Layer_" + index.ToString() + "_Color", color);
		}

		// Token: 0x06004285 RID: 17029 RVA: 0x00117CCC File Offset: 0x00115ECC
		public void SetFaceTexture(Texture2D tex, Color color)
		{
			this.FaceMesh.material.SetTexture("_Layer_" + 1.ToString() + "_Texture", tex);
			this.FaceMesh.material.SetColor("_Layer_" + 1.ToString() + "_Color", color);
		}

		// Token: 0x06004286 RID: 17030 RVA: 0x00117D2C File Offset: 0x00115F2C
		public void ApplyBodyLayerSettings(AvatarSettings settings, int maxOrder = -1)
		{
			for (int i = 1; i <= 6; i++)
			{
				this.SetBodyLayer(i, string.Empty, Color.white);
			}
			AvatarLayer avatarLayer = null;
			if (settings.UseCombinedLayer && settings.CombinedLayerPath != string.Empty)
			{
				avatarLayer = (Resources.Load(settings.CombinedLayerPath) as AvatarLayer);
			}
			if (avatarLayer != null)
			{
				this.usingCombinedLayer = true;
				SkinnedMeshRenderer[] bodyMeshes = this.BodyMeshes;
				for (int j = 0; j < bodyMeshes.Length; j++)
				{
					bodyMeshes[j].material = avatarLayer.CombinedMaterial;
				}
				return;
			}
			this.usingCombinedLayer = false;
			List<Tuple<AvatarLayer, Color>> list = new List<Tuple<AvatarLayer, Color>>();
			for (int k = 0; k < settings.BodyLayerSettings.Count; k++)
			{
				if (!string.IsNullOrEmpty(settings.BodyLayerSettings[k].layerPath))
				{
					AvatarLayer avatarLayer2 = Resources.Load(settings.BodyLayerSettings[k].layerPath) as AvatarLayer;
					if (maxOrder <= -1 || avatarLayer2.Order <= maxOrder)
					{
						if (avatarLayer2 != null)
						{
							list.Add(new Tuple<AvatarLayer, Color>(avatarLayer2, settings.BodyLayerSettings[k].layerTint));
						}
						else
						{
							Console.LogWarning("Body layer not found at path " + settings.BodyLayerSettings[k].layerPath, null);
						}
					}
				}
			}
			list.Sort((Tuple<AvatarLayer, Color> x, Tuple<AvatarLayer, Color> y) => x.Item1.Order.CompareTo(y.Item1.Order));
			for (int l = 0; l < list.Count; l++)
			{
				this.SetBodyLayer(l + 1, list[l].Item1.AssetPath, list[l].Item2);
			}
		}

		// Token: 0x06004287 RID: 17031 RVA: 0x00117EE0 File Offset: 0x001160E0
		private void SetBodyLayer(int index, string assetPath, Color color)
		{
			AvatarLayer avatarLayer = Resources.Load(assetPath) as AvatarLayer;
			Texture2D texture2D = (avatarLayer != null) ? avatarLayer.Texture : null;
			if (texture2D == null)
			{
				color.a = 0f;
			}
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in this.BodyMeshes)
			{
				if (skinnedMeshRenderer.material.shader != this.DefaultAvatarMaterial.shader)
				{
					skinnedMeshRenderer.material = new Material(this.DefaultAvatarMaterial);
				}
				skinnedMeshRenderer.material.SetTexture("_Layer_" + index.ToString() + "_Texture", texture2D);
				skinnedMeshRenderer.material.SetColor("_Layer_" + index.ToString() + "_Color", color);
				if (avatarLayer != null)
				{
					skinnedMeshRenderer.material.SetTexture("_Layer_" + index.ToString() + "_Normal", avatarLayer.Normal);
				}
			}
		}

		// Token: 0x06004288 RID: 17032 RVA: 0x00117FE8 File Offset: 0x001161E8
		public void ApplyAccessorySettings(AvatarSettings settings)
		{
			if (this.appliedAccessories.Length != 9)
			{
				this.DestroyAccessories();
				this.appliedAccessories = new Accessory[9];
			}
			bool shrink = false;
			float num = 0f;
			bool flag = false;
			for (int i = 0; i < 9; i++)
			{
				if (settings.AccessorySettings.Count > i && settings.AccessorySettings[i].path != string.Empty)
				{
					if (this.appliedAccessories[i] != null && this.appliedAccessories[i].AssetPath != settings.AccessorySettings[i].path)
					{
						UnityEngine.Object.Destroy(this.appliedAccessories[i].gameObject);
						this.appliedAccessories[i] = null;
					}
					if (this.appliedAccessories[i] == null)
					{
						GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load(settings.AccessorySettings[i].path), this.BodyContainer) as GameObject;
						this.appliedAccessories[i] = gameObject.GetComponent<Accessory>();
						this.appliedAccessories[i].BindBones(this.BodyMeshes[0].bones);
						this.appliedAccessories[i].ApplyShapeKeys(this.appliedGender * 100f, this.appliedWeight * 100f);
					}
					if (this.appliedAccessories[i].ReduceFootSize)
					{
						shrink = true;
						num = Mathf.Max(num, this.appliedAccessories[i].FootSizeReduction);
					}
					if (this.appliedAccessories[i].ShouldBlockHair)
					{
						flag = true;
					}
				}
				else if (this.appliedAccessories[i] != null)
				{
					UnityEngine.Object.Destroy(this.appliedAccessories[i].gameObject);
					this.appliedAccessories[i] = null;
				}
			}
			this.SetFeetShrunk(shrink, num);
			this.SetWearingHairBlockingAccessory(flag);
			for (int j = 0; j < this.appliedAccessories.Length; j++)
			{
				if (this.appliedAccessories[j] != null)
				{
					this.appliedAccessories[j].ApplyColor(settings.AccessorySettings[j].color);
				}
			}
		}

		// Token: 0x06004289 RID: 17033 RVA: 0x001181F4 File Offset: 0x001163F4
		private void DestroyAccessories()
		{
			for (int i = 0; i < this.appliedAccessories.Length; i++)
			{
				if (this.appliedAccessories[i] != null)
				{
					UnityEngine.Object.Destroy(this.appliedAccessories[i].gameObject);
				}
			}
		}

		// Token: 0x0600428A RID: 17034 RVA: 0x00118238 File Offset: 0x00116438
		public virtual void SetRagdollPhysicsEnabled(bool ragdollEnabled, bool playStandUpAnim = true)
		{
			bool ragdolled = this.Ragdolled;
			this.Ragdolled = ragdollEnabled;
			if (this.onRagdollChange != null)
			{
				this.onRagdollChange.Invoke(ragdolled, ragdollEnabled, playStandUpAnim);
			}
			foreach (Rigidbody rigidbody in this.RagdollRBs)
			{
				if (!(rigidbody == null))
				{
					rigidbody.isKinematic = !ragdollEnabled;
					if (!rigidbody.isKinematic)
					{
						rigidbody.velocity = Vector3.zero;
						rigidbody.angularVelocity = Vector3.zero;
					}
				}
			}
			foreach (Collider collider in this.RagdollColliders)
			{
				if (!(collider == null))
				{
					collider.enabled = ragdollEnabled;
				}
			}
		}

		// Token: 0x0600428B RID: 17035 RVA: 0x001182E4 File Offset: 0x001164E4
		public virtual AvatarEquippable SetEquippable(string assetPath)
		{
			if (this.CurrentEquippable != null)
			{
				this.CurrentEquippable.Unequip();
			}
			if (!(assetPath != string.Empty))
			{
				return null;
			}
			GameObject gameObject = Resources.Load(assetPath) as GameObject;
			if (gameObject == null)
			{
				Console.LogError("Couldn't find equippable at path " + assetPath, null);
				return null;
			}
			this.CurrentEquippable = UnityEngine.Object.Instantiate<GameObject>(gameObject, null).GetComponent<AvatarEquippable>();
			this.CurrentEquippable.Equip(this);
			return this.CurrentEquippable;
		}

		// Token: 0x0600428C RID: 17036 RVA: 0x00118365 File Offset: 0x00116565
		public virtual void ReceiveEquippableMessage(string message, object data)
		{
			if (this.CurrentEquippable != null)
			{
				this.CurrentEquippable.ReceiveMessage(message, data);
				return;
			}
			Console.LogWarning("Received equippable message but no equippable is equipped!", null);
		}

		// Token: 0x04002F34 RID: 12084
		public const int MAX_ACCESSORIES = 9;

		// Token: 0x04002F35 RID: 12085
		public const bool USE_COMBINED_LAYERS = true;

		// Token: 0x04002F36 RID: 12086
		public const float DEFAULT_SMOOTHNESS = 0.25f;

		// Token: 0x04002F37 RID: 12087
		private static float maleShoulderScale = 0.93f;

		// Token: 0x04002F38 RID: 12088
		private static float femaleShoulderScale = 0.875f;

		// Token: 0x04002F39 RID: 12089
		[Header("References")]
		public AvatarAnimation Anim;

		// Token: 0x04002F3A RID: 12090
		public AvatarLookController LookController;

		// Token: 0x04002F3B RID: 12091
		public SkinnedMeshRenderer[] BodyMeshes;

		// Token: 0x04002F3C RID: 12092
		public SkinnedMeshRenderer[] ShapeKeyMeshes;

		// Token: 0x04002F3D RID: 12093
		public SkinnedMeshRenderer FaceMesh;

		// Token: 0x04002F3E RID: 12094
		public EyeController Eyes;

		// Token: 0x04002F3F RID: 12095
		public EyebrowController EyeBrows;

		// Token: 0x04002F40 RID: 12096
		public Transform BodyContainer;

		// Token: 0x04002F41 RID: 12097
		public Transform Armature;

		// Token: 0x04002F42 RID: 12098
		public Transform LeftShoulder;

		// Token: 0x04002F43 RID: 12099
		public Transform RightShoulder;

		// Token: 0x04002F44 RID: 12100
		public Transform HeadBone;

		// Token: 0x04002F45 RID: 12101
		public Transform HipBone;

		// Token: 0x04002F46 RID: 12102
		public Rigidbody[] RagdollRBs;

		// Token: 0x04002F47 RID: 12103
		public Collider[] RagdollColliders;

		// Token: 0x04002F48 RID: 12104
		public Rigidbody MiddleSpineRB;

		// Token: 0x04002F49 RID: 12105
		public AvatarEmotionManager EmotionManager;

		// Token: 0x04002F4A RID: 12106
		public AvatarEffects Effects;

		// Token: 0x04002F4B RID: 12107
		public Transform MiddleSpine;

		// Token: 0x04002F4C RID: 12108
		public Transform LowerSpine;

		// Token: 0x04002F4D RID: 12109
		public Transform LowestSpine;

		// Token: 0x04002F4E RID: 12110
		public AvatarImpostor Impostor;

		// Token: 0x04002F4F RID: 12111
		public FootstepSounds FootstepSounds;

		// Token: 0x04002F50 RID: 12112
		[Header("Settings")]
		public AvatarSettings InitialAvatarSettings;

		// Token: 0x04002F51 RID: 12113
		public Material DefaultAvatarMaterial;

		// Token: 0x04002F52 RID: 12114
		public bool UseImpostor;

		// Token: 0x04002F53 RID: 12115
		public UnityEvent<bool, bool, bool> onRagdollChange;

		// Token: 0x04002F56 RID: 12118
		[Header("Data - readonly")]
		[SerializeField]
		protected float appliedGender;

		// Token: 0x04002F57 RID: 12119
		[SerializeField]
		protected float appliedWeight;

		// Token: 0x04002F58 RID: 12120
		[SerializeField]
		protected Hair appliedHair;

		// Token: 0x04002F59 RID: 12121
		[SerializeField]
		protected Color appliedHairColor;

		// Token: 0x04002F5A RID: 12122
		[SerializeField]
		protected Accessory[] appliedAccessories = new Accessory[9];

		// Token: 0x04002F5B RID: 12123
		[SerializeField]
		protected bool wearingHairBlockingAccessory;

		// Token: 0x04002F5C RID: 12124
		private float additionalWeight;

		// Token: 0x04002F5D RID: 12125
		private float additionalGender;

		// Token: 0x04002F5F RID: 12127
		[Header("Runtime loading")]
		public AvatarSettings SettingsToLoad;

		// Token: 0x04002F60 RID: 12128
		public UnityEvent onSettingsLoaded;

		// Token: 0x04002F61 RID: 12129
		private Vector3 originalHipPos = Vector3.zero;

		// Token: 0x04002F62 RID: 12130
		private bool usingCombinedLayer;

		// Token: 0x04002F63 RID: 12131
		private bool blockEyeFaceLayers;
	}
}
