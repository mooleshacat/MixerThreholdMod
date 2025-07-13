using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001C2 RID: 450
	public class ProfileGroupDefinition
	{
		// Token: 0x06000911 RID: 2321 RVA: 0x000285B0 File Offset: 0x000267B0
		public static ProfileGroupDefinition NumberGroupDefinition(string groupName, string propKey, float minimumValue, float maximumValue, float value, string tooltip)
		{
			return ProfileGroupDefinition.NumberGroupDefinition(groupName, propKey, minimumValue, maximumValue, value, ProfileGroupDefinition.RebuildType.None, null, false, tooltip);
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x000285D0 File Offset: 0x000267D0
		public static ProfileGroupDefinition NumberGroupDefinition(string groupName, string propKey, float minimumValue, float maximumValue, float value, string dependsOnKeyword, bool dependsOnValue, string tooltip)
		{
			return ProfileGroupDefinition.NumberGroupDefinition(groupName, propKey, minimumValue, maximumValue, value, ProfileGroupDefinition.RebuildType.None, ProfileGroupDefinition.FormatStyle.Float, dependsOnKeyword, dependsOnValue, tooltip);
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x000285F0 File Offset: 0x000267F0
		public static ProfileGroupDefinition NumberGroupDefinition(string groupName, string propKey, float minimumValue, float maximumValue, float value, ProfileGroupDefinition.RebuildType rebuildType, string dependsOnKeyword, bool dependsOnValue, string tooltip)
		{
			return ProfileGroupDefinition.NumberGroupDefinition(groupName, propKey, minimumValue, maximumValue, value, rebuildType, ProfileGroupDefinition.FormatStyle.Float, dependsOnKeyword, dependsOnValue, tooltip);
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x00028614 File Offset: 0x00026814
		public static ProfileGroupDefinition NumberGroupDefinition(string groupName, string propKey, float minimumValue, float maximumValue, float value, ProfileGroupDefinition.RebuildType rebuildType, ProfileGroupDefinition.FormatStyle formatStyle, string dependsOnKeyword, bool dependsOnValue, string tooltip)
		{
			return new ProfileGroupDefinition
			{
				type = ProfileGroupDefinition.GroupType.Number,
				formatStyle = formatStyle,
				groupName = groupName,
				propertyKey = propKey,
				value = value,
				minimumValue = minimumValue,
				maximumValue = maximumValue,
				tooltip = tooltip,
				rebuildType = rebuildType,
				dependsOnFeature = dependsOnKeyword,
				dependsOnValue = dependsOnValue
			};
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x00028679 File Offset: 0x00026879
		public static ProfileGroupDefinition ColorGroupDefinition(string groupName, string propKey, Color color, string tooltip)
		{
			return ProfileGroupDefinition.ColorGroupDefinition(groupName, propKey, color, ProfileGroupDefinition.RebuildType.None, null, false, tooltip);
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x00028687 File Offset: 0x00026887
		public static ProfileGroupDefinition ColorGroupDefinition(string groupName, string propKey, Color color, string dependsOnFeature, bool dependsOnValue, string tooltip)
		{
			return ProfileGroupDefinition.ColorGroupDefinition(groupName, propKey, color, ProfileGroupDefinition.RebuildType.None, dependsOnFeature, dependsOnValue, tooltip);
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x00028698 File Offset: 0x00026898
		public static ProfileGroupDefinition ColorGroupDefinition(string groupName, string propKey, Color color, ProfileGroupDefinition.RebuildType rebuildType, string dependsOnKeyword, bool dependsOnValue, string tooltip)
		{
			return new ProfileGroupDefinition
			{
				type = ProfileGroupDefinition.GroupType.Color,
				propertyKey = propKey,
				groupName = groupName,
				color = color,
				tooltip = tooltip,
				rebuildType = rebuildType,
				dependsOnFeature = dependsOnKeyword,
				dependsOnValue = dependsOnValue
			};
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x000286E5 File Offset: 0x000268E5
		public static ProfileGroupDefinition SpherePointGroupDefinition(string groupName, string propKey, float horizontalRotation, float verticalRotation, string tooltip)
		{
			return ProfileGroupDefinition.SpherePointGroupDefinition(groupName, propKey, horizontalRotation, verticalRotation, ProfileGroupDefinition.RebuildType.None, null, false, tooltip);
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x000286F8 File Offset: 0x000268F8
		public static ProfileGroupDefinition SpherePointGroupDefinition(string groupName, string propKey, float horizontalRotation, float verticalRotation, ProfileGroupDefinition.RebuildType rebuildType, string dependsOnKeyword, bool dependsOnValue, string tooltip)
		{
			return new ProfileGroupDefinition
			{
				type = ProfileGroupDefinition.GroupType.SpherePoint,
				propertyKey = propKey,
				groupName = groupName,
				tooltip = tooltip,
				rebuildType = rebuildType,
				dependsOnFeature = dependsOnKeyword,
				dependsOnValue = dependsOnValue,
				spherePoint = new SpherePoint(horizontalRotation, verticalRotation)
			};
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x0002874C File Offset: 0x0002694C
		public static ProfileGroupDefinition TextureGroupDefinition(string groupName, string propKey, Texture2D texture, string tooltip)
		{
			return ProfileGroupDefinition.TextureGroupDefinition(groupName, propKey, texture, ProfileGroupDefinition.RebuildType.None, null, false, tooltip);
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x0002875A File Offset: 0x0002695A
		public static ProfileGroupDefinition TextureGroupDefinition(string groupName, string propKey, Texture2D texture, string dependsOnKeyword, bool dependsOnValue, string tooltip)
		{
			return ProfileGroupDefinition.TextureGroupDefinition(groupName, propKey, texture, ProfileGroupDefinition.RebuildType.None, dependsOnKeyword, dependsOnValue, tooltip);
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x0002876C File Offset: 0x0002696C
		public static ProfileGroupDefinition TextureGroupDefinition(string groupName, string propKey, Texture2D texture, ProfileGroupDefinition.RebuildType rebuildType, string dependsOnKeyword, bool dependsOnValue, string tooltip)
		{
			return new ProfileGroupDefinition
			{
				type = ProfileGroupDefinition.GroupType.Texture,
				groupName = groupName,
				propertyKey = propKey,
				texture = texture,
				tooltip = tooltip,
				rebuildType = rebuildType,
				dependsOnFeature = dependsOnKeyword,
				dependsOnValue = dependsOnValue
			};
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x000287B9 File Offset: 0x000269B9
		public static ProfileGroupDefinition BoolGroupDefinition(string groupName, string propKey, bool value, string dependsOnKeyword, bool dependsOnValue, string tooltip)
		{
			return new ProfileGroupDefinition
			{
				type = ProfileGroupDefinition.GroupType.Boolean,
				groupName = groupName,
				propertyKey = propKey,
				dependsOnFeature = dependsOnKeyword,
				dependsOnValue = dependsOnValue,
				tooltip = tooltip,
				boolValue = value
			};
		}

		// Token: 0x040009D0 RID: 2512
		public ProfileGroupDefinition.GroupType type;

		// Token: 0x040009D1 RID: 2513
		public ProfileGroupDefinition.FormatStyle formatStyle;

		// Token: 0x040009D2 RID: 2514
		public ProfileGroupDefinition.RebuildType rebuildType;

		// Token: 0x040009D3 RID: 2515
		public string propertyKey;

		// Token: 0x040009D4 RID: 2516
		public string groupName;

		// Token: 0x040009D5 RID: 2517
		public Color color;

		// Token: 0x040009D6 RID: 2518
		public SpherePoint spherePoint;

		// Token: 0x040009D7 RID: 2519
		public float minimumValue = -1f;

		// Token: 0x040009D8 RID: 2520
		public float maximumValue = -1f;

		// Token: 0x040009D9 RID: 2521
		public float value = -1f;

		// Token: 0x040009DA RID: 2522
		public bool boolValue;

		// Token: 0x040009DB RID: 2523
		public Texture2D texture;

		// Token: 0x040009DC RID: 2524
		public string tooltip;

		// Token: 0x040009DD RID: 2525
		public string dependsOnFeature;

		// Token: 0x040009DE RID: 2526
		public bool dependsOnValue;

		// Token: 0x020001C3 RID: 451
		public enum GroupType
		{
			// Token: 0x040009E0 RID: 2528
			None,
			// Token: 0x040009E1 RID: 2529
			Color,
			// Token: 0x040009E2 RID: 2530
			Number,
			// Token: 0x040009E3 RID: 2531
			Texture,
			// Token: 0x040009E4 RID: 2532
			SpherePoint,
			// Token: 0x040009E5 RID: 2533
			Boolean
		}

		// Token: 0x020001C4 RID: 452
		public enum FormatStyle
		{
			// Token: 0x040009E7 RID: 2535
			None,
			// Token: 0x040009E8 RID: 2536
			Integer,
			// Token: 0x040009E9 RID: 2537
			Float
		}

		// Token: 0x020001C5 RID: 453
		public enum RebuildType
		{
			// Token: 0x040009EB RID: 2539
			None,
			// Token: 0x040009EC RID: 2540
			Stars
		}
	}
}
