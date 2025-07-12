using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;

namespace AdvancedPeopleSystem
{
	// Token: 0x02000213 RID: 531
	[Serializable]
	public class CharacterCustomizationSetup
	{
		// Token: 0x06000BB5 RID: 2997 RVA: 0x00035CD4 File Offset: 0x00033ED4
		public void ApplyToCharacter(CharacterCustomization cc)
		{
			if (cc.Settings == null && this.settingsName != cc.Settings.name)
			{
				Debug.LogError("Character settings are not compatible with saved data");
				return;
			}
			cc.SetBodyColor(BodyColorPart.Skin, new Color(this.SkinColor[0], this.SkinColor[1], this.SkinColor[2], this.SkinColor[3]));
			cc.SetBodyColor(BodyColorPart.Eye, new Color(this.EyeColor[0], this.EyeColor[1], this.EyeColor[2], this.EyeColor[3]));
			cc.SetBodyColor(BodyColorPart.Hair, new Color(this.HairColor[0], this.HairColor[1], this.HairColor[2], this.HairColor[3]));
			cc.SetBodyColor(BodyColorPart.Underpants, new Color(this.UnderpantsColor[0], this.UnderpantsColor[1], this.UnderpantsColor[2], this.UnderpantsColor[3]));
			cc.SetBodyColor(BodyColorPart.Teeth, new Color(this.TeethColor[0], this.TeethColor[1], this.TeethColor[2], this.TeethColor[3]));
			cc.SetBodyColor(BodyColorPart.OralCavity, new Color(this.OralCavityColor[0], this.OralCavityColor[1], this.OralCavityColor[2], this.OralCavityColor[3]));
			cc.SetElementByIndex(CharacterElementType.Hair, this.selectedElements.Hair);
			cc.SetElementByIndex(CharacterElementType.Accessory, this.selectedElements.Accessory);
			cc.SetElementByIndex(CharacterElementType.Hat, this.selectedElements.Hat);
			cc.SetElementByIndex(CharacterElementType.Pants, this.selectedElements.Pants);
			cc.SetElementByIndex(CharacterElementType.Shoes, this.selectedElements.Shoes);
			cc.SetElementByIndex(CharacterElementType.Shirt, this.selectedElements.Shirt);
			cc.SetElementByIndex(CharacterElementType.Beard, this.selectedElements.Beard);
			cc.SetElementByIndex(CharacterElementType.Item1, this.selectedElements.Item1);
			cc.SetHeight(this.Height);
			cc.SetHeadSize(this.HeadSize);
			foreach (CharacterBlendshapeData characterBlendshapeData in this.blendshapes)
			{
				cc.SetBlendshapeValue(characterBlendshapeData.type, characterBlendshapeData.value, null, null);
			}
			cc.ApplyPrefab();
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x00035F1C File Offset: 0x0003411C
		public string Serialize(CharacterCustomizationSetup.CharacterFileSaveFormat format)
		{
			string result = string.Empty;
			switch (format)
			{
			case CharacterCustomizationSetup.CharacterFileSaveFormat.Json:
				return JsonUtility.ToJson(this, true);
			case CharacterCustomizationSetup.CharacterFileSaveFormat.Xml:
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(CharacterCustomizationSetup));
				using (StringWriter stringWriter = new StringWriter())
				{
					xmlSerializer.Serialize(stringWriter, this);
					return stringWriter.ToString();
				}
				break;
			}
			case CharacterCustomizationSetup.CharacterFileSaveFormat.Binary:
				break;
			default:
				return result;
			}
			using (MemoryStream memoryStream = new MemoryStream())
			{
				new BinaryFormatter().Serialize(memoryStream, this);
				result = Convert.ToBase64String(memoryStream.ToArray());
			}
			return result;
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x00035FC8 File Offset: 0x000341C8
		public static CharacterCustomizationSetup Deserialize(string data, CharacterCustomizationSetup.CharacterFileSaveFormat format)
		{
			CharacterCustomizationSetup result = null;
			switch (format)
			{
			case CharacterCustomizationSetup.CharacterFileSaveFormat.Json:
				return JsonUtility.FromJson<CharacterCustomizationSetup>(data);
			case CharacterCustomizationSetup.CharacterFileSaveFormat.Xml:
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(CharacterCustomizationSetup));
				using (StringReader stringReader = new StringReader(data))
				{
					return (CharacterCustomizationSetup)xmlSerializer.Deserialize(stringReader);
				}
				break;
			}
			case CharacterCustomizationSetup.CharacterFileSaveFormat.Binary:
				break;
			default:
				return result;
			}
			using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(data)))
			{
				result = (CharacterCustomizationSetup)new BinaryFormatter().Deserialize(memoryStream);
			}
			return result;
		}

		// Token: 0x04000C64 RID: 3172
		public string settingsName;

		// Token: 0x04000C65 RID: 3173
		public CharacterSelectedElements selectedElements = new CharacterSelectedElements();

		// Token: 0x04000C66 RID: 3174
		public List<CharacterBlendshapeData> blendshapes = new List<CharacterBlendshapeData>();

		// Token: 0x04000C67 RID: 3175
		public int MinLod;

		// Token: 0x04000C68 RID: 3176
		public int MaxLod;

		// Token: 0x04000C69 RID: 3177
		public float[] SkinColor;

		// Token: 0x04000C6A RID: 3178
		public float[] EyeColor;

		// Token: 0x04000C6B RID: 3179
		public float[] HairColor;

		// Token: 0x04000C6C RID: 3180
		public float[] UnderpantsColor;

		// Token: 0x04000C6D RID: 3181
		public float[] TeethColor;

		// Token: 0x04000C6E RID: 3182
		public float[] OralCavityColor;

		// Token: 0x04000C6F RID: 3183
		public float Height;

		// Token: 0x04000C70 RID: 3184
		public float HeadSize;

		// Token: 0x02000214 RID: 532
		public enum CharacterFileSaveFormat
		{
			// Token: 0x04000C72 RID: 3186
			Json,
			// Token: 0x04000C73 RID: 3187
			Xml,
			// Token: 0x04000C74 RID: 3188
			Binary
		}
	}
}
