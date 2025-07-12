using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Customization
{
	// Token: 0x020009E8 RID: 2536
	[CreateAssetMenu(fileName = "BasicAvatarSettings", menuName = "ScriptableObjects/BasicAvatarSettings", order = 1)]
	[Serializable]
	public class BasicAvatarSettings : ScriptableObject
	{
		// Token: 0x06004461 RID: 17505 RVA: 0x0011F29D File Offset: 0x0011D49D
		public T SetValue<T>(string fieldName, T value)
		{
			base.GetType().GetField(fieldName).SetValue(this, value);
			return value;
		}

		// Token: 0x06004462 RID: 17506 RVA: 0x0011F2B8 File Offset: 0x0011D4B8
		public T GetValue<T>(string fieldName)
		{
			FieldInfo field = base.GetType().GetField(fieldName);
			if (field == null)
			{
				return default(T);
			}
			return (T)((object)field.GetValue(this));
		}

		// Token: 0x06004463 RID: 17507 RVA: 0x0011F2F4 File Offset: 0x0011D4F4
		public AvatarSettings GetAvatarSettings()
		{
			AvatarSettings avatarSettings = ScriptableObject.CreateInstance<AvatarSettings>();
			avatarSettings.Gender = (float)this.Gender * 0.7f;
			avatarSettings.Weight = this.Weight;
			avatarSettings.Height = 1f;
			avatarSettings.SkinColor = this.SkinColor;
			avatarSettings.HairPath = this.HairStyle;
			avatarSettings.HairColor = this.HairColor;
			avatarSettings.FaceLayerSettings.Add(new AvatarSettings.LayerSetting
			{
				layerPath = this.Mouth,
				layerTint = Color.black
			});
			avatarSettings.FaceLayerSettings.Add(new AvatarSettings.LayerSetting
			{
				layerPath = this.FacialHair,
				layerTint = Color.white
			});
			avatarSettings.FaceLayerSettings.Add(new AvatarSettings.LayerSetting
			{
				layerPath = this.FacialDetails,
				layerTint = new Color(0f, 0f, 0f, this.FacialDetailsIntensity)
			});
			avatarSettings.FaceLayerSettings.Add(new AvatarSettings.LayerSetting
			{
				layerPath = "Avatar/Layers/Face/EyeShadow",
				layerTint = new Color(0f, 0f, 0f, 0.7f)
			});
			avatarSettings.EyeBallTint = this.EyeballColor;
			avatarSettings.LeftEyeLidColor = this.SkinColor;
			avatarSettings.RightEyeLidColor = this.SkinColor;
			avatarSettings.EyeballMaterialIdentifier = "Default";
			avatarSettings.PupilDilation = this.PupilDilation;
			Eye.EyeLidConfiguration eyeLidConfiguration = new Eye.EyeLidConfiguration
			{
				topLidOpen = this.UpperEyeLidRestingPosition,
				bottomLidOpen = this.LowerEyeLidRestingPosition
			};
			avatarSettings.LeftEyeRestingState = eyeLidConfiguration;
			avatarSettings.RightEyeRestingState = eyeLidConfiguration;
			avatarSettings.EyebrowScale = this.EyebrowScale;
			avatarSettings.EyebrowThickness = this.EyebrowThickness;
			avatarSettings.EyebrowRestingHeight = this.EyebrowRestingHeight;
			avatarSettings.EyebrowRestingAngle = this.EyebrowRestingAngle;
			avatarSettings.BodyLayerSettings.Add(new AvatarSettings.LayerSetting
			{
				layerPath = "Avatar/Layers/Top/Nipples",
				layerTint = new Color32(212, 181, 142, byte.MaxValue)
			});
			string layerPath = ((float)this.Gender <= 0.5f) ? "Avatar/Layers/Bottom/MaleUnderwear" : "Avatar/Layers/Bottom/FemaleUnderwear";
			avatarSettings.BodyLayerSettings.Add(new AvatarSettings.LayerSetting
			{
				layerPath = layerPath,
				layerTint = Color.white
			});
			if (!string.IsNullOrEmpty(this.Top))
			{
				avatarSettings.BodyLayerSettings.Add(new AvatarSettings.LayerSetting
				{
					layerPath = this.Top,
					layerTint = this.TopColor
				});
			}
			if (!string.IsNullOrEmpty(this.Bottom))
			{
				avatarSettings.BodyLayerSettings.Add(new AvatarSettings.LayerSetting
				{
					layerPath = this.Bottom,
					layerTint = this.BottomColor
				});
			}
			if (this.Tattoos != null)
			{
				for (int i = 0; i < this.Tattoos.Count; i++)
				{
					if (this.Tattoos[i].Contains("/Face/"))
					{
						avatarSettings.FaceLayerSettings.Add(new AvatarSettings.LayerSetting
						{
							layerPath = this.Tattoos[i],
							layerTint = Color.white
						});
					}
					else
					{
						avatarSettings.BodyLayerSettings.Add(new AvatarSettings.LayerSetting
						{
							layerPath = this.Tattoos[i],
							layerTint = Color.white
						});
					}
				}
			}
			if (!string.IsNullOrEmpty(this.Shoes))
			{
				avatarSettings.AccessorySettings.Add(new AvatarSettings.AccessorySetting
				{
					path = this.Shoes,
					color = this.ShoesColor
				});
			}
			if (!string.IsNullOrEmpty(this.Headwear))
			{
				avatarSettings.AccessorySettings.Add(new AvatarSettings.AccessorySetting
				{
					path = this.Headwear,
					color = this.HeadwearColor
				});
			}
			if (!string.IsNullOrEmpty(this.Eyewear))
			{
				avatarSettings.AccessorySettings.Add(new AvatarSettings.AccessorySetting
				{
					path = this.Eyewear,
					color = this.EyewearColor
				});
			}
			return avatarSettings;
		}

		// Token: 0x06004464 RID: 17508 RVA: 0x00119C2B File Offset: 0x00117E2B
		public virtual string GetJson(bool prettyPrint = true)
		{
			return JsonUtility.ToJson(this, prettyPrint);
		}

		// Token: 0x0400313B RID: 12603
		public const float GENDER_MULTIPLIER = 0.7f;

		// Token: 0x0400313C RID: 12604
		public const string MaleUnderwearPath = "Avatar/Layers/Bottom/MaleUnderwear";

		// Token: 0x0400313D RID: 12605
		public const string FemaleUnderwearPath = "Avatar/Layers/Bottom/FemaleUnderwear";

		// Token: 0x0400313E RID: 12606
		public int Gender;

		// Token: 0x0400313F RID: 12607
		public float Weight;

		// Token: 0x04003140 RID: 12608
		public Color SkinColor;

		// Token: 0x04003141 RID: 12609
		public string HairStyle;

		// Token: 0x04003142 RID: 12610
		public Color HairColor;

		// Token: 0x04003143 RID: 12611
		public string Mouth;

		// Token: 0x04003144 RID: 12612
		public string FacialHair;

		// Token: 0x04003145 RID: 12613
		public string FacialDetails;

		// Token: 0x04003146 RID: 12614
		public float FacialDetailsIntensity;

		// Token: 0x04003147 RID: 12615
		public Color EyeballColor;

		// Token: 0x04003148 RID: 12616
		public float UpperEyeLidRestingPosition;

		// Token: 0x04003149 RID: 12617
		public float LowerEyeLidRestingPosition;

		// Token: 0x0400314A RID: 12618
		public float PupilDilation = 1f;

		// Token: 0x0400314B RID: 12619
		public float EyebrowScale;

		// Token: 0x0400314C RID: 12620
		public float EyebrowThickness;

		// Token: 0x0400314D RID: 12621
		public float EyebrowRestingHeight;

		// Token: 0x0400314E RID: 12622
		public float EyebrowRestingAngle;

		// Token: 0x0400314F RID: 12623
		public string Top;

		// Token: 0x04003150 RID: 12624
		public Color TopColor;

		// Token: 0x04003151 RID: 12625
		public string Bottom;

		// Token: 0x04003152 RID: 12626
		public Color BottomColor;

		// Token: 0x04003153 RID: 12627
		public string Shoes;

		// Token: 0x04003154 RID: 12628
		public Color ShoesColor;

		// Token: 0x04003155 RID: 12629
		public string Headwear;

		// Token: 0x04003156 RID: 12630
		public Color HeadwearColor;

		// Token: 0x04003157 RID: 12631
		public string Eyewear;

		// Token: 0x04003158 RID: 12632
		public Color EyewearColor;

		// Token: 0x04003159 RID: 12633
		public List<string> Tattoos = new List<string>();
	}
}
