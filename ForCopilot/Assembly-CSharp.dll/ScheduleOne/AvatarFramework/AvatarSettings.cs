using System;
using System.Collections.Generic;
using System.Reflection;
using FishNet.Serializing.Helping;
using UnityEngine;

namespace ScheduleOne.AvatarFramework
{
	// Token: 0x020009A0 RID: 2464
	[CreateAssetMenu(fileName = "Avatar Settings", menuName = "ScriptableObjects/Avatar Settings", order = 1)]
	[Serializable]
	public class AvatarSettings : ScriptableObject
	{
		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x060042C4 RID: 17092 RVA: 0x001195B3 File Offset: 0x001177B3
		public float UpperEyelidRestingPosition
		{
			get
			{
				return this.LeftEyeRestingState.topLidOpen;
			}
		}

		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x060042C5 RID: 17093 RVA: 0x001195C0 File Offset: 0x001177C0
		public float LowerEyelidRestingPosition
		{
			get
			{
				return this.LeftEyeRestingState.bottomLidOpen;
			}
		}

		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x060042C6 RID: 17094 RVA: 0x001195CD File Offset: 0x001177CD
		public string FaceLayer1Path
		{
			get
			{
				if (this.FaceLayerSettings.Count <= 0)
				{
					return null;
				}
				return this.FaceLayerSettings[0].layerPath;
			}
		}

		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x060042C7 RID: 17095 RVA: 0x001195F0 File Offset: 0x001177F0
		public Color FaceLayer1Color
		{
			get
			{
				if (this.FaceLayerSettings.Count <= 0)
				{
					return Color.white;
				}
				return this.FaceLayerSettings[0].layerTint;
			}
		}

		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x060042C8 RID: 17096 RVA: 0x00119617 File Offset: 0x00117817
		public string FaceLayer2Path
		{
			get
			{
				if (this.FaceLayerSettings.Count <= 1)
				{
					return null;
				}
				return this.FaceLayerSettings[1].layerPath;
			}
		}

		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x060042C9 RID: 17097 RVA: 0x0011963A File Offset: 0x0011783A
		public Color FaceLayer2Color
		{
			get
			{
				if (this.FaceLayerSettings.Count <= 1)
				{
					return Color.white;
				}
				return this.FaceLayerSettings[1].layerTint;
			}
		}

		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x060042CA RID: 17098 RVA: 0x00119661 File Offset: 0x00117861
		public string FaceLayer3Path
		{
			get
			{
				if (this.FaceLayerSettings.Count <= 2)
				{
					return null;
				}
				return this.FaceLayerSettings[2].layerPath;
			}
		}

		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x060042CB RID: 17099 RVA: 0x00119684 File Offset: 0x00117884
		public Color FaceLayer3Color
		{
			get
			{
				if (this.FaceLayerSettings.Count <= 2)
				{
					return Color.white;
				}
				return this.FaceLayerSettings[2].layerTint;
			}
		}

		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x060042CC RID: 17100 RVA: 0x001196AB File Offset: 0x001178AB
		public string FaceLayer4Path
		{
			get
			{
				if (this.FaceLayerSettings.Count <= 3)
				{
					return null;
				}
				return this.FaceLayerSettings[3].layerPath;
			}
		}

		// Token: 0x17000941 RID: 2369
		// (get) Token: 0x060042CD RID: 17101 RVA: 0x001196CE File Offset: 0x001178CE
		public Color FaceLayer4Color
		{
			get
			{
				if (this.FaceLayerSettings.Count <= 3)
				{
					return Color.white;
				}
				return this.FaceLayerSettings[3].layerTint;
			}
		}

		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x060042CE RID: 17102 RVA: 0x001196F5 File Offset: 0x001178F5
		public string FaceLayer5Path
		{
			get
			{
				if (this.FaceLayerSettings.Count <= 4)
				{
					return null;
				}
				return this.FaceLayerSettings[4].layerPath;
			}
		}

		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x060042CF RID: 17103 RVA: 0x00119718 File Offset: 0x00117918
		public Color FaceLayer5Color
		{
			get
			{
				if (this.FaceLayerSettings.Count <= 4)
				{
					return Color.white;
				}
				return this.FaceLayerSettings[4].layerTint;
			}
		}

		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x060042D0 RID: 17104 RVA: 0x0011973F File Offset: 0x0011793F
		public string FaceLayer6Path
		{
			get
			{
				if (this.FaceLayerSettings.Count <= 5)
				{
					return null;
				}
				return this.FaceLayerSettings[5].layerPath;
			}
		}

		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x060042D1 RID: 17105 RVA: 0x00119762 File Offset: 0x00117962
		public Color FaceLayer6Color
		{
			get
			{
				if (this.FaceLayerSettings.Count <= 5)
				{
					return Color.white;
				}
				return this.FaceLayerSettings[5].layerTint;
			}
		}

		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x060042D2 RID: 17106 RVA: 0x00119789 File Offset: 0x00117989
		public string BodyLayer1Path
		{
			get
			{
				if (this.BodyLayerSettings.Count <= 0)
				{
					return null;
				}
				return this.BodyLayerSettings[0].layerPath;
			}
		}

		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x060042D3 RID: 17107 RVA: 0x001197AC File Offset: 0x001179AC
		public Color BodyLayer1Color
		{
			get
			{
				if (this.BodyLayerSettings.Count <= 0)
				{
					return Color.white;
				}
				return this.BodyLayerSettings[0].layerTint;
			}
		}

		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x060042D4 RID: 17108 RVA: 0x001197D3 File Offset: 0x001179D3
		public string BodyLayer2Path
		{
			get
			{
				if (this.BodyLayerSettings.Count <= 1)
				{
					return null;
				}
				return this.BodyLayerSettings[1].layerPath;
			}
		}

		// Token: 0x17000949 RID: 2377
		// (get) Token: 0x060042D5 RID: 17109 RVA: 0x001197F6 File Offset: 0x001179F6
		public Color BodyLayer2Color
		{
			get
			{
				if (this.BodyLayerSettings.Count <= 1)
				{
					return Color.white;
				}
				return this.BodyLayerSettings[1].layerTint;
			}
		}

		// Token: 0x1700094A RID: 2378
		// (get) Token: 0x060042D6 RID: 17110 RVA: 0x0011981D File Offset: 0x00117A1D
		public string BodyLayer3Path
		{
			get
			{
				if (this.BodyLayerSettings.Count <= 2)
				{
					return null;
				}
				return this.BodyLayerSettings[2].layerPath;
			}
		}

		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x060042D7 RID: 17111 RVA: 0x00119840 File Offset: 0x00117A40
		public Color BodyLayer3Color
		{
			get
			{
				if (this.BodyLayerSettings.Count <= 2)
				{
					return Color.white;
				}
				return this.BodyLayerSettings[2].layerTint;
			}
		}

		// Token: 0x1700094C RID: 2380
		// (get) Token: 0x060042D8 RID: 17112 RVA: 0x00119867 File Offset: 0x00117A67
		public string BodyLayer4Path
		{
			get
			{
				if (this.BodyLayerSettings.Count <= 3)
				{
					return null;
				}
				return this.BodyLayerSettings[3].layerPath;
			}
		}

		// Token: 0x1700094D RID: 2381
		// (get) Token: 0x060042D9 RID: 17113 RVA: 0x0011988A File Offset: 0x00117A8A
		public Color BodyLayer4Color
		{
			get
			{
				if (this.BodyLayerSettings.Count <= 3)
				{
					return Color.white;
				}
				return this.BodyLayerSettings[3].layerTint;
			}
		}

		// Token: 0x1700094E RID: 2382
		// (get) Token: 0x060042DA RID: 17114 RVA: 0x001198B1 File Offset: 0x00117AB1
		public string BodyLayer5Path
		{
			get
			{
				if (this.BodyLayerSettings.Count <= 4)
				{
					return null;
				}
				return this.BodyLayerSettings[4].layerPath;
			}
		}

		// Token: 0x1700094F RID: 2383
		// (get) Token: 0x060042DB RID: 17115 RVA: 0x001198D4 File Offset: 0x00117AD4
		public Color BodyLayer5Color
		{
			get
			{
				if (this.BodyLayerSettings.Count <= 4)
				{
					return Color.white;
				}
				return this.BodyLayerSettings[4].layerTint;
			}
		}

		// Token: 0x17000950 RID: 2384
		// (get) Token: 0x060042DC RID: 17116 RVA: 0x001198FB File Offset: 0x00117AFB
		public string BodyLayer6Path
		{
			get
			{
				if (this.BodyLayerSettings.Count <= 5)
				{
					return null;
				}
				return this.BodyLayerSettings[5].layerPath;
			}
		}

		// Token: 0x17000951 RID: 2385
		// (get) Token: 0x060042DD RID: 17117 RVA: 0x0011991E File Offset: 0x00117B1E
		public Color BodyLayer6Color
		{
			get
			{
				if (this.BodyLayerSettings.Count <= 5)
				{
					return Color.white;
				}
				return this.BodyLayerSettings[5].layerTint;
			}
		}

		// Token: 0x17000952 RID: 2386
		// (get) Token: 0x060042DE RID: 17118 RVA: 0x00119945 File Offset: 0x00117B45
		public string Accessory1Path
		{
			get
			{
				if (this.AccessorySettings.Count <= 0)
				{
					return null;
				}
				return this.AccessorySettings[0].path;
			}
		}

		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x060042DF RID: 17119 RVA: 0x00119968 File Offset: 0x00117B68
		public Color Accessory1Color
		{
			get
			{
				if (this.AccessorySettings.Count <= 0)
				{
					return Color.white;
				}
				return this.AccessorySettings[0].color;
			}
		}

		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x060042E0 RID: 17120 RVA: 0x0011998F File Offset: 0x00117B8F
		public string Accessory2Path
		{
			get
			{
				if (this.AccessorySettings.Count <= 1)
				{
					return null;
				}
				return this.AccessorySettings[1].path;
			}
		}

		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x060042E1 RID: 17121 RVA: 0x001199B2 File Offset: 0x00117BB2
		public Color Accessory2Color
		{
			get
			{
				if (this.AccessorySettings.Count <= 1)
				{
					return Color.white;
				}
				return this.AccessorySettings[1].color;
			}
		}

		// Token: 0x17000956 RID: 2390
		// (get) Token: 0x060042E2 RID: 17122 RVA: 0x001199D9 File Offset: 0x00117BD9
		public string Accessory3Path
		{
			get
			{
				if (this.AccessorySettings.Count <= 2)
				{
					return null;
				}
				return this.AccessorySettings[2].path;
			}
		}

		// Token: 0x17000957 RID: 2391
		// (get) Token: 0x060042E3 RID: 17123 RVA: 0x001199FC File Offset: 0x00117BFC
		public Color Accessory3Color
		{
			get
			{
				if (this.AccessorySettings.Count <= 2)
				{
					return Color.white;
				}
				return this.AccessorySettings[2].color;
			}
		}

		// Token: 0x17000958 RID: 2392
		// (get) Token: 0x060042E4 RID: 17124 RVA: 0x00119A23 File Offset: 0x00117C23
		public string Accessory4Path
		{
			get
			{
				if (this.AccessorySettings.Count <= 3)
				{
					return null;
				}
				return this.AccessorySettings[3].path;
			}
		}

		// Token: 0x17000959 RID: 2393
		// (get) Token: 0x060042E5 RID: 17125 RVA: 0x00119A46 File Offset: 0x00117C46
		public Color Accessory4Color
		{
			get
			{
				if (this.AccessorySettings.Count <= 3)
				{
					return Color.white;
				}
				return this.AccessorySettings[3].color;
			}
		}

		// Token: 0x1700095A RID: 2394
		// (get) Token: 0x060042E6 RID: 17126 RVA: 0x00119A6D File Offset: 0x00117C6D
		public string Accessory5Path
		{
			get
			{
				if (this.AccessorySettings.Count <= 4)
				{
					return null;
				}
				return this.AccessorySettings[4].path;
			}
		}

		// Token: 0x1700095B RID: 2395
		// (get) Token: 0x060042E7 RID: 17127 RVA: 0x00119A90 File Offset: 0x00117C90
		public Color Accessory5Color
		{
			get
			{
				if (this.AccessorySettings.Count <= 4)
				{
					return Color.white;
				}
				return this.AccessorySettings[4].color;
			}
		}

		// Token: 0x1700095C RID: 2396
		// (get) Token: 0x060042E8 RID: 17128 RVA: 0x00119AB7 File Offset: 0x00117CB7
		public string Accessory6Path
		{
			get
			{
				if (this.AccessorySettings.Count <= 5)
				{
					return null;
				}
				return this.AccessorySettings[5].path;
			}
		}

		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x060042E9 RID: 17129 RVA: 0x00119ADA File Offset: 0x00117CDA
		public Color Accessory6Color
		{
			get
			{
				if (this.AccessorySettings.Count <= 5)
				{
					return Color.white;
				}
				return this.AccessorySettings[5].color;
			}
		}

		// Token: 0x1700095E RID: 2398
		// (get) Token: 0x060042EA RID: 17130 RVA: 0x00119B01 File Offset: 0x00117D01
		public string Accessory7Path
		{
			get
			{
				if (this.AccessorySettings.Count <= 6)
				{
					return null;
				}
				return this.AccessorySettings[6].path;
			}
		}

		// Token: 0x1700095F RID: 2399
		// (get) Token: 0x060042EB RID: 17131 RVA: 0x00119B24 File Offset: 0x00117D24
		public Color Accessory7Color
		{
			get
			{
				if (this.AccessorySettings.Count <= 6)
				{
					return Color.white;
				}
				return this.AccessorySettings[6].color;
			}
		}

		// Token: 0x17000960 RID: 2400
		// (get) Token: 0x060042EC RID: 17132 RVA: 0x00119B4B File Offset: 0x00117D4B
		public string Accessory8Path
		{
			get
			{
				if (this.AccessorySettings.Count <= 7)
				{
					return null;
				}
				return this.AccessorySettings[7].path;
			}
		}

		// Token: 0x17000961 RID: 2401
		// (get) Token: 0x060042ED RID: 17133 RVA: 0x00119B6E File Offset: 0x00117D6E
		public Color Accessory8Color
		{
			get
			{
				if (this.AccessorySettings.Count <= 7)
				{
					return Color.white;
				}
				return this.AccessorySettings[7].color;
			}
		}

		// Token: 0x17000962 RID: 2402
		// (get) Token: 0x060042EE RID: 17134 RVA: 0x00119B95 File Offset: 0x00117D95
		public string Accessory9Path
		{
			get
			{
				if (this.AccessorySettings.Count <= 8)
				{
					return null;
				}
				return this.AccessorySettings[8].path;
			}
		}

		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x060042EF RID: 17135 RVA: 0x00119BB8 File Offset: 0x00117DB8
		public Color Accessory9Color
		{
			get
			{
				if (this.AccessorySettings.Count <= 8)
				{
					return Color.white;
				}
				return this.AccessorySettings[8].color;
			}
		}

		// Token: 0x17000964 RID: 2404
		public object this[string propertyName]
		{
			get
			{
				FieldInfo field = base.GetType().GetField(propertyName);
				PropertyInfo property = base.GetType().GetProperty(propertyName);
				if (field != null)
				{
					return field.GetValue(this);
				}
				if (property != null)
				{
					return property.GetValue(this, null);
				}
				return null;
			}
		}

		// Token: 0x060042F1 RID: 17137 RVA: 0x00119C2B File Offset: 0x00117E2B
		public virtual string GetJson(bool prettyPrint = true)
		{
			return JsonUtility.ToJson(this, prettyPrint);
		}

		// Token: 0x04002F94 RID: 12180
		public Color SkinColor;

		// Token: 0x04002F95 RID: 12181
		public float Height;

		// Token: 0x04002F96 RID: 12182
		public float Gender;

		// Token: 0x04002F97 RID: 12183
		public float Weight;

		// Token: 0x04002F98 RID: 12184
		public string HairPath;

		// Token: 0x04002F99 RID: 12185
		public Color HairColor;

		// Token: 0x04002F9A RID: 12186
		public float EyebrowScale;

		// Token: 0x04002F9B RID: 12187
		public float EyebrowThickness;

		// Token: 0x04002F9C RID: 12188
		public float EyebrowRestingHeight;

		// Token: 0x04002F9D RID: 12189
		public float EyebrowRestingAngle;

		// Token: 0x04002F9E RID: 12190
		public Color LeftEyeLidColor;

		// Token: 0x04002F9F RID: 12191
		public Color RightEyeLidColor;

		// Token: 0x04002FA0 RID: 12192
		public Eye.EyeLidConfiguration LeftEyeRestingState;

		// Token: 0x04002FA1 RID: 12193
		public Eye.EyeLidConfiguration RightEyeRestingState;

		// Token: 0x04002FA2 RID: 12194
		public string EyeballMaterialIdentifier;

		// Token: 0x04002FA3 RID: 12195
		public Color EyeBallTint;

		// Token: 0x04002FA4 RID: 12196
		public float PupilDilation;

		// Token: 0x04002FA5 RID: 12197
		public List<AvatarSettings.LayerSetting> FaceLayerSettings = new List<AvatarSettings.LayerSetting>();

		// Token: 0x04002FA6 RID: 12198
		public List<AvatarSettings.LayerSetting> BodyLayerSettings = new List<AvatarSettings.LayerSetting>();

		// Token: 0x04002FA7 RID: 12199
		public List<AvatarSettings.AccessorySetting> AccessorySettings = new List<AvatarSettings.AccessorySetting>();

		// Token: 0x04002FA8 RID: 12200
		public bool UseCombinedLayer;

		// Token: 0x04002FA9 RID: 12201
		public string CombinedLayerPath;

		// Token: 0x04002FAA RID: 12202
		[CodegenExclude]
		public Texture2D ImpostorTexture;

		// Token: 0x020009A1 RID: 2465
		[Serializable]
		public struct LayerSetting
		{
			// Token: 0x04002FAB RID: 12203
			public string layerPath;

			// Token: 0x04002FAC RID: 12204
			public Color layerTint;
		}

		// Token: 0x020009A2 RID: 2466
		[Serializable]
		public class AccessorySetting
		{
			// Token: 0x04002FAD RID: 12205
			public string path;

			// Token: 0x04002FAE RID: 12206
			public Color color;
		}
	}
}
