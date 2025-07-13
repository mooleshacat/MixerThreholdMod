using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x0200015E RID: 350
	public static class UtilsBeamProps
	{
		// Token: 0x060006B1 RID: 1713 RVA: 0x0001E034 File Offset: 0x0001C234
		public static bool CanChangeDuringPlaytime(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			return !volumetricLightBeamSD || volumetricLightBeamSD.trackChangesDuringPlaytime;
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x0001E058 File Offset: 0x0001C258
		public static Quaternion GetInternalLocalRotation(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if (volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.beamInternalLocalRotation;
			}
			VolumetricLightBeamHD volumetricLightBeamHD = self as VolumetricLightBeamHD;
			if (volumetricLightBeamHD)
			{
				return volumetricLightBeamHD.beamInternalLocalRotation;
			}
			return Quaternion.identity;
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x0001E098 File Offset: 0x0001C298
		public static float GetThickness(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if (volumetricLightBeamSD)
			{
				return Mathf.Clamp01(1f - volumetricLightBeamSD.fresnelPow / 10f);
			}
			VolumetricLightBeamHD volumetricLightBeamHD = self as VolumetricLightBeamHD;
			if (volumetricLightBeamHD)
			{
				return Mathf.Clamp01(1f - volumetricLightBeamHD.sideSoftness / 10f);
			}
			return 0f;
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x0001E0F8 File Offset: 0x0001C2F8
		public static float GetFallOffEnd(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if (volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.fallOffEnd;
			}
			VolumetricLightBeamHD volumetricLightBeamHD = self as VolumetricLightBeamHD;
			if (volumetricLightBeamHD)
			{
				return volumetricLightBeamHD.fallOffEnd;
			}
			return 0f;
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x0001E138 File Offset: 0x0001C338
		public static ColorMode GetColorMode(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if (volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.usedColorMode;
			}
			VolumetricLightBeamHD volumetricLightBeamHD = self as VolumetricLightBeamHD;
			if (volumetricLightBeamHD)
			{
				return volumetricLightBeamHD.colorMode;
			}
			return ColorMode.Flat;
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x0001E174 File Offset: 0x0001C374
		public static Color GetColorFlat(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if (volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.color;
			}
			VolumetricLightBeamHD volumetricLightBeamHD = self as VolumetricLightBeamHD;
			if (volumetricLightBeamHD)
			{
				return volumetricLightBeamHD.colorFlat;
			}
			return Color.white;
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x0001E1B4 File Offset: 0x0001C3B4
		public static Gradient GetColorGradient(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if (volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.colorGradient;
			}
			VolumetricLightBeamHD volumetricLightBeamHD = self as VolumetricLightBeamHD;
			if (volumetricLightBeamHD)
			{
				return volumetricLightBeamHD.colorGradient;
			}
			return null;
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x0001E1F0 File Offset: 0x0001C3F0
		public static float GetConeAngle(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if (volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.coneAngle;
			}
			VolumetricLightBeamHD volumetricLightBeamHD = self as VolumetricLightBeamHD;
			if (volumetricLightBeamHD)
			{
				return volumetricLightBeamHD.coneAngle;
			}
			return 0f;
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x0001E230 File Offset: 0x0001C430
		public static float GetConeRadiusStart(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if (volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.coneRadiusStart;
			}
			VolumetricLightBeamHD volumetricLightBeamHD = self as VolumetricLightBeamHD;
			if (volumetricLightBeamHD)
			{
				return volumetricLightBeamHD.coneRadiusStart;
			}
			return 0f;
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x0001E270 File Offset: 0x0001C470
		public static float GetConeRadiusEnd(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if (volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.coneRadiusEnd;
			}
			VolumetricLightBeamHD volumetricLightBeamHD = self as VolumetricLightBeamHD;
			if (volumetricLightBeamHD)
			{
				return volumetricLightBeamHD.coneRadiusEnd;
			}
			return 0f;
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x0001E2B0 File Offset: 0x0001C4B0
		public static int GetSortingLayerID(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if (volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.sortingLayerID;
			}
			VolumetricLightBeamHD volumetricLightBeamHD = self as VolumetricLightBeamHD;
			if (volumetricLightBeamHD)
			{
				return volumetricLightBeamHD.GetSortingLayerID();
			}
			return 0;
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x0001E2EC File Offset: 0x0001C4EC
		public static int GetSortingOrder(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if (volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.sortingOrder;
			}
			VolumetricLightBeamHD volumetricLightBeamHD = self as VolumetricLightBeamHD;
			if (volumetricLightBeamHD)
			{
				return volumetricLightBeamHD.GetSortingOrder();
			}
			return 0;
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x0001E328 File Offset: 0x0001C528
		public static bool GetFadeOutEnabled(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			return volumetricLightBeamSD && volumetricLightBeamSD.isFadeOutEnabled;
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x0001E34C File Offset: 0x0001C54C
		public static float GetFadeOutEnd(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if (volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.fadeOutEnd;
			}
			return 0f;
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x0001E374 File Offset: 0x0001C574
		public static Dimensions GetDimensions(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if (volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.dimensions;
			}
			VolumetricLightBeamHD volumetricLightBeamHD = self as VolumetricLightBeamHD;
			if (volumetricLightBeamHD)
			{
				return volumetricLightBeamHD.GetDimensions();
			}
			return Dimensions.Dim3D;
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x0001E3B0 File Offset: 0x0001C5B0
		public static int GetGeomSides(VolumetricLightBeamAbstractBase self)
		{
			VolumetricLightBeamSD volumetricLightBeamSD = self as VolumetricLightBeamSD;
			if (volumetricLightBeamSD)
			{
				return volumetricLightBeamSD.geomSides;
			}
			return Config.Instance.sharedMeshSides;
		}
	}
}
