using System;
using System.Collections.Generic;
using System.Reflection;

namespace Funly.SkyStudio
{
	// Token: 0x020001C8 RID: 456
	public abstract class ProfilePropertyKeys
	{
		// Token: 0x06000921 RID: 2337 RVA: 0x00028870 File Offset: 0x00026A70
		public static HashSet<string> GetPropertyKeysSet()
		{
			FieldInfo[] fields = typeof(ProfilePropertyKeys).GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
			HashSet<string> hashSet = new HashSet<string>();
			foreach (FieldInfo fieldInfo in fields)
			{
				if (fieldInfo.IsLiteral)
				{
					string item = fieldInfo.GetValue(null) as string;
					hashSet.Add(item);
				}
			}
			return hashSet;
		}

		// Token: 0x040009F7 RID: 2551
		public const string SkyCubemapKey = "SkyCubemapKey";

		// Token: 0x040009F8 RID: 2552
		public const string SkyUpperColorKey = "SkyUpperColorKey";

		// Token: 0x040009F9 RID: 2553
		public const string SkyMiddleColorKey = "SkyMiddleColorKey";

		// Token: 0x040009FA RID: 2554
		public const string SkyLowerColorKey = "SkyLowerColorKey";

		// Token: 0x040009FB RID: 2555
		public const string SkyMiddleColorPositionKey = "SkyMiddleColorPosition";

		// Token: 0x040009FC RID: 2556
		public const string HorizonTrasitionStartKey = "HorizonTransitionStartKey";

		// Token: 0x040009FD RID: 2557
		public const string HorizonTransitionLengthKey = "HorizonTransitionLengthKey";

		// Token: 0x040009FE RID: 2558
		public const string HorizonStarScaleKey = "HorizonStarScaleKey";

		// Token: 0x040009FF RID: 2559
		public const string StarTransitionStartKey = "StarTransitionStartKey";

		// Token: 0x04000A00 RID: 2560
		public const string StarTransitionLengthKey = "StarTransitionLengthKey";

		// Token: 0x04000A01 RID: 2561
		public const string AmbientLightSkyColorKey = "AmbientLightSkyColorKey";

		// Token: 0x04000A02 RID: 2562
		public const string AmbientLightEquatorColorKey = "AmbientLightEquatorColorKey";

		// Token: 0x04000A03 RID: 2563
		public const string AmbientLightGroundColorKey = "AmbientLightGroundColorKey";

		// Token: 0x04000A04 RID: 2564
		public const string SunColorKey = "SunColorKey";

		// Token: 0x04000A05 RID: 2565
		public const string SunTextureKey = "SunTextureKey";

		// Token: 0x04000A06 RID: 2566
		public const string SunSizeKey = "SunSizeKey";

		// Token: 0x04000A07 RID: 2567
		public const string SunRotationSpeedKey = "SunRotationSpeedKey";

		// Token: 0x04000A08 RID: 2568
		public const string SunEdgeFeatheringKey = "SunEdgeFeatheringKey";

		// Token: 0x04000A09 RID: 2569
		public const string SunColorIntensityKey = "SunColorIntensityKey";

		// Token: 0x04000A0A RID: 2570
		public const string SunLightColorKey = "SunLightColorKey";

		// Token: 0x04000A0B RID: 2571
		public const string SunLightIntensityKey = "SunLightIntensityKey";

		// Token: 0x04000A0C RID: 2572
		public const string SunPositionKey = "SunPositionKey";

		// Token: 0x04000A0D RID: 2573
		public const string SunSpriteRowCountKey = "SunSpriteRowCountKey";

		// Token: 0x04000A0E RID: 2574
		public const string SunSpriteColumnCountKey = "SunSpriteColumnCountKey";

		// Token: 0x04000A0F RID: 2575
		public const string SunSpriteItemCountKey = "SunSpriteItemCount";

		// Token: 0x04000A10 RID: 2576
		public const string SunSpriteAnimationSpeedKey = "SunSpriteAnimationSpeed";

		// Token: 0x04000A11 RID: 2577
		public const string SunAlpha = "SunAlphaKey";

		// Token: 0x04000A12 RID: 2578
		public const string MoonColorKey = "MoonColorKey";

		// Token: 0x04000A13 RID: 2579
		public const string MoonTextureKey = "MoonTextureKey";

		// Token: 0x04000A14 RID: 2580
		public const string MoonSizeKey = "MoonSizeKey";

		// Token: 0x04000A15 RID: 2581
		public const string MoonRotationSpeedKey = "MoonRotationSpeedKey";

		// Token: 0x04000A16 RID: 2582
		public const string MoonEdgeFeatheringKey = "MoonEdgeFeatheringKey";

		// Token: 0x04000A17 RID: 2583
		public const string MoonColorIntensityKey = "MoonColorIntensityKey";

		// Token: 0x04000A18 RID: 2584
		public const string MoonLightColorKey = "MoonLightColorKey";

		// Token: 0x04000A19 RID: 2585
		public const string MoonLightIntensityKey = "MoonLightIntensityKey";

		// Token: 0x04000A1A RID: 2586
		public const string MoonPositionKey = "MoonPositionKey";

		// Token: 0x04000A1B RID: 2587
		public const string MoonSpriteRowCountKey = "MoonSpriteRowCountKey";

		// Token: 0x04000A1C RID: 2588
		public const string MoonSpriteColumnCountKey = "MoonSpriteColumnCountKey";

		// Token: 0x04000A1D RID: 2589
		public const string MoonSpriteItemCountKey = "MoonSpriteItemCount";

		// Token: 0x04000A1E RID: 2590
		public const string MoonSpriteAnimationSpeedKey = "MoonSpriteAnimationSpeed";

		// Token: 0x04000A1F RID: 2591
		public const string MoonAlpha = "MoonAlphaKey";

		// Token: 0x04000A20 RID: 2592
		public const string StarBasicCubemapKey = "StarBasicCubemapKey";

		// Token: 0x04000A21 RID: 2593
		public const string StarBasicTwinkleSpeedKey = "StarBasicTwinkleSpeedKey";

		// Token: 0x04000A22 RID: 2594
		public const string StarBasicTwinkleAmountKey = "StarBasicTwinkleAmountKey";

		// Token: 0x04000A23 RID: 2595
		public const string StarBasicOpacityKey = "StarBasicOpacityKey";

		// Token: 0x04000A24 RID: 2596
		public const string StarBasicTintColorKey = "StarBasicTintColorKey";

		// Token: 0x04000A25 RID: 2597
		public const string StarBasicIntensityKey = "StarBasicIntensityKey";

		// Token: 0x04000A26 RID: 2598
		public const string StarBasicExponentKey = "StarBasicExponentKey";

		// Token: 0x04000A27 RID: 2599
		public const string Star1SizeKey = "Star1SizeKey";

		// Token: 0x04000A28 RID: 2600
		public const string Star1DensityKey = "Star1DensityKey";

		// Token: 0x04000A29 RID: 2601
		public const string Star1TextureKey = "Star1TextureKey";

		// Token: 0x04000A2A RID: 2602
		public const string Star1ColorKey = "Star1ColorKey";

		// Token: 0x04000A2B RID: 2603
		public const string Star1TwinkleAmountKey = "Star1TwinkleAmountKey";

		// Token: 0x04000A2C RID: 2604
		public const string Star1TwinkleSpeedKey = "Star1TwinkleSpeedKey";

		// Token: 0x04000A2D RID: 2605
		public const string Star1RotationSpeedKey = "Star1RotationSpeed";

		// Token: 0x04000A2E RID: 2606
		public const string Star1EdgeFeatheringKey = "Star1EdgeFeathering";

		// Token: 0x04000A2F RID: 2607
		public const string Star1ColorIntensityKey = "Star1ColorIntensityKey";

		// Token: 0x04000A30 RID: 2608
		public const string Star1SpriteRowCountKey = "Star1SpriteRowCountKey";

		// Token: 0x04000A31 RID: 2609
		public const string Star1SpriteColumnCountKey = "Star1SpriteColumnCountKey";

		// Token: 0x04000A32 RID: 2610
		public const string Star1SpriteItemCountKey = "Star1SpriteItemCount";

		// Token: 0x04000A33 RID: 2611
		public const string Star1SpriteAnimationSpeedKey = "Star1SpriteAnimationSpeed";

		// Token: 0x04000A34 RID: 2612
		public const string Star2SizeKey = "Star2SizeKey";

		// Token: 0x04000A35 RID: 2613
		public const string Star2DensityKey = "Star2DensityKey";

		// Token: 0x04000A36 RID: 2614
		public const string Star2TextureKey = "Star2TextureKey";

		// Token: 0x04000A37 RID: 2615
		public const string Star2ColorKey = "Star2ColorKey";

		// Token: 0x04000A38 RID: 2616
		public const string Star2TwinkleAmountKey = "Star2TwinkleAmountKey";

		// Token: 0x04000A39 RID: 2617
		public const string Star2TwinkleSpeedKey = "Star2TwinkleSpeedKey";

		// Token: 0x04000A3A RID: 2618
		public const string Star2RotationSpeedKey = "Star2RotationSpeed";

		// Token: 0x04000A3B RID: 2619
		public const string Star2EdgeFeatheringKey = "Star2EdgeFeathering";

		// Token: 0x04000A3C RID: 2620
		public const string Star2ColorIntensityKey = "Star2ColorIntensityKey";

		// Token: 0x04000A3D RID: 2621
		public const string Star2SpriteRowCountKey = "Star2SpriteRowCountKey";

		// Token: 0x04000A3E RID: 2622
		public const string Star2SpriteColumnCountKey = "Star2SpriteColumnCountKey";

		// Token: 0x04000A3F RID: 2623
		public const string Star2SpriteItemCountKey = "Star2SpriteItemCount";

		// Token: 0x04000A40 RID: 2624
		public const string Star2SpriteAnimationSpeedKey = "Star2SpriteAnimationSpeed";

		// Token: 0x04000A41 RID: 2625
		public const string Star3SizeKey = "Star3SizeKey";

		// Token: 0x04000A42 RID: 2626
		public const string Star3DensityKey = "Star3DensityKey";

		// Token: 0x04000A43 RID: 2627
		public const string Star3TextureKey = "Star3TextureKey";

		// Token: 0x04000A44 RID: 2628
		public const string Star3ColorKey = "Star3ColorKey";

		// Token: 0x04000A45 RID: 2629
		public const string Star3TwinkleAmountKey = "Star3TwinkleAmountKey";

		// Token: 0x04000A46 RID: 2630
		public const string Star3TwinkleSpeedKey = "Star3TwinkleSpeedKey";

		// Token: 0x04000A47 RID: 2631
		public const string Star3RotationSpeedKey = "Star3RotationSpeed";

		// Token: 0x04000A48 RID: 2632
		public const string Star3EdgeFeatheringKey = "Star3EdgeFeathering";

		// Token: 0x04000A49 RID: 2633
		public const string Star3ColorIntensityKey = "Star3ColorIntensityKey";

		// Token: 0x04000A4A RID: 2634
		public const string Star3SpriteRowCountKey = "Star3SpriteRowCountKey";

		// Token: 0x04000A4B RID: 2635
		public const string Star3SpriteColumnCountKey = "Star3SpriteColumnCountKey";

		// Token: 0x04000A4C RID: 2636
		public const string Star3SpriteItemCountKey = "Star3SpriteItemCount";

		// Token: 0x04000A4D RID: 2637
		public const string Star3SpriteAnimationSpeedKey = "Star3SpriteAnimationSpeed";

		// Token: 0x04000A4E RID: 2638
		public const string CloudNoiseTextureKey = "CloudNoiseTextureKey";

		// Token: 0x04000A4F RID: 2639
		public const string CloudDensityKey = "CloudDensityKey";

		// Token: 0x04000A50 RID: 2640
		public const string CloudSpeedKey = "CloudSpeedKey";

		// Token: 0x04000A51 RID: 2641
		public const string CloudDirectionKey = "CloudDirectionKey";

		// Token: 0x04000A52 RID: 2642
		public const string CloudHeightKey = "CloudHeightKey";

		// Token: 0x04000A53 RID: 2643
		public const string CloudColor1Key = "CloudColor1Key";

		// Token: 0x04000A54 RID: 2644
		public const string CloudColor2Key = "CloudColor2Key";

		// Token: 0x04000A55 RID: 2645
		public const string CloudFadePositionKey = "CloudFadePositionKey";

		// Token: 0x04000A56 RID: 2646
		public const string CloudFadeAmountKey = "CloudFadeAmountKey";

		// Token: 0x04000A57 RID: 2647
		public const string CloudTextureTiling = "CloudTextureTiling";

		// Token: 0x04000A58 RID: 2648
		public const string CloudAlpha = "CloudAlphaKey";

		// Token: 0x04000A59 RID: 2649
		public const string CloudCubemapNormalTextureKey = "CloudCubemapNormalTextureKey";

		// Token: 0x04000A5A RID: 2650
		public const string CloudCubemapNormalLitColorKey = "CloudCubemapNormalLitColorKey";

		// Token: 0x04000A5B RID: 2651
		public const string CloudCubemapNormalShadowKey = "CloudCubemapNormalShadowColorKey";

		// Token: 0x04000A5C RID: 2652
		public const string CloudCubemapNormalRotationSpeedKey = "CloudCubemapNormalRotationSpeedKey";

		// Token: 0x04000A5D RID: 2653
		public const string CloudCubemapNormalAmbientIntensity = "CloudCubemapNormalAmbientIntensityKey";

		// Token: 0x04000A5E RID: 2654
		public const string CloudCubemapNormalHeightKey = "CloudCubemapNormalHeightKey";

		// Token: 0x04000A5F RID: 2655
		public const string CloudCubemapNormalDoubleLayerRotationSpeedKey = "CloudCubemapNormalDoubleLayerRotationSpeedKey";

		// Token: 0x04000A60 RID: 2656
		public const string CloudCubemapNormalDoubleLayerHeightKey = "CloudCubemapNormalDoubleLayerHeightKey";

		// Token: 0x04000A61 RID: 2657
		public const string CloudCubemapNormalDoubleLayerCustomTextureKey = "CloudCubemapNormalDoubleLayerCustomTextureKey";

		// Token: 0x04000A62 RID: 2658
		public const string CloudCubemapNormalDoubleLayerLitColorKey = "CloudCubemapNormalDoubleLayerLitColorKey";

		// Token: 0x04000A63 RID: 2659
		public const string CloudCubemapNormalDoubleLayerShadowKey = "CloudCubemapNormalDoubleLayerShadowKey";

		// Token: 0x04000A64 RID: 2660
		public const string CloudCubemapTextureKey = "CloudCubemapTextureKey";

		// Token: 0x04000A65 RID: 2661
		public const string CloudCubemapRotationSpeedKey = "CloudCubemapRotationSpeedKey";

		// Token: 0x04000A66 RID: 2662
		public const string CloudCubemapTintColorKey = "CloudCubemapTintColorKey";

		// Token: 0x04000A67 RID: 2663
		public const string CloudCubemapHeightKey = "CloudCubemapHeightKey";

		// Token: 0x04000A68 RID: 2664
		public const string CloudCubemapDoubleLayerRotationSpeedKey = "CloudCubemapDoubleLayerRotationSpeedKey";

		// Token: 0x04000A69 RID: 2665
		public const string CloudCubemapDoubleLayerHeightKey = "CloudCubemapDoubleLayerHeightKey";

		// Token: 0x04000A6A RID: 2666
		public const string CloudCubemapDoubleLayerCustomTextureKey = "CloudCubemapDoubleLayerCustomTextureKey";

		// Token: 0x04000A6B RID: 2667
		public const string CloudCubemapDoubleLayerTintColorKey = "CloudCubemapDoubleLayerTintColorKey";

		// Token: 0x04000A6C RID: 2668
		public const string FogDensityKey = "FogDensityKey";

		// Token: 0x04000A6D RID: 2669
		public const string FogColorKey = "FogColorKey";

		// Token: 0x04000A6E RID: 2670
		public const string FogLengthKey = "FogLengthKey";

		// Token: 0x04000A6F RID: 2671
		public const string FogSyncWithGlobal = "FogSyncWithGlobal";

		// Token: 0x04000A70 RID: 2672
		public const string RainNearIntensityKey = "RainNearIntensityKey";

		// Token: 0x04000A71 RID: 2673
		public const string RainFarIntensityKey = "RainFarIntensityKey";

		// Token: 0x04000A72 RID: 2674
		public const string RainNearSpeedKey = "RainNearSpeedKey";

		// Token: 0x04000A73 RID: 2675
		public const string RainFarSpeedKey = "RainFarSpeedKey";

		// Token: 0x04000A74 RID: 2676
		public const string RainSoundVolumeKey = "RainSoundVolume";

		// Token: 0x04000A75 RID: 2677
		public const string RainSoundKey = "RainSoundKey";

		// Token: 0x04000A76 RID: 2678
		public const string RainTintColorKey = "RainTintColorKey";

		// Token: 0x04000A77 RID: 2679
		public const string RainWindTurbulence = "RainWindTurbulenceKey";

		// Token: 0x04000A78 RID: 2680
		public const string RainWindTurbulenceSpeed = "RainWindTurbulenceSpeedKey";

		// Token: 0x04000A79 RID: 2681
		public const string RainNearTextureKey = "RainNearTextureKey";

		// Token: 0x04000A7A RID: 2682
		public const string RainFarTextureKey = "RainFarTextureKey";

		// Token: 0x04000A7B RID: 2683
		public const string RainNearTextureTiling = "RainNearTextureTiling";

		// Token: 0x04000A7C RID: 2684
		public const string RainFarTextureTiling = "RainFarTextureTiling";

		// Token: 0x04000A7D RID: 2685
		public const string RainSplashMaxConcurrentKey = "RainSplashMaxConcurrentKey";

		// Token: 0x04000A7E RID: 2686
		public const string RainSplashAreaStartKey = "RainSplashAreaStartKey";

		// Token: 0x04000A7F RID: 2687
		public const string RainSplashAreaLengthKey = "RainSplashAreaLengthKey";

		// Token: 0x04000A80 RID: 2688
		public const string RainSplashScaleKey = "RainSplashScaleKey";

		// Token: 0x04000A81 RID: 2689
		public const string RainSplashScaleVarienceKey = "RainSplashScaleVarienceKey";

		// Token: 0x04000A82 RID: 2690
		public const string RainSplashIntensityKey = "RainSplashIntensityKey";

		// Token: 0x04000A83 RID: 2691
		public const string RainSplashSurfaceOffsetKey = "RainSplashSurfaceOffsetKey";

		// Token: 0x04000A84 RID: 2692
		public const string RainSplashTintColorKey = "RainSplashTintColorKey";

		// Token: 0x04000A85 RID: 2693
		public const string LightningProbabilityKey = "LightningProbabilityKey";

		// Token: 0x04000A86 RID: 2694
		public const string LightningStrikeCoolDown = "LightningStrikeCoolDown";

		// Token: 0x04000A87 RID: 2695
		public const string LightningIntensityKey = "LightningIntensityKey";

		// Token: 0x04000A88 RID: 2696
		public const string LightningTintColorKey = "LightningTintColorKey";

		// Token: 0x04000A89 RID: 2697
		public const string ThunderSoundVolumeKey = "ThunderSoundVolumeKey";

		// Token: 0x04000A8A RID: 2698
		public const string ThunderSoundDelayKey = "ThunderSoundDelayKey";
	}
}
