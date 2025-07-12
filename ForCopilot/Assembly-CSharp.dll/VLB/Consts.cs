using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x020000E9 RID: 233
	public static class Consts
	{
		// Token: 0x040004D1 RID: 1233
		public const string PluginFolder = "VolumetricLightBeam";

		// Token: 0x020000EA RID: 234
		public static class Help
		{
			// Token: 0x040004D2 RID: 1234
			private const string UrlBase = "http://saladgamer.com/vlb-doc/";

			// Token: 0x040004D3 RID: 1235
			private const string UrlSuffix = "/";

			// Token: 0x040004D4 RID: 1236
			public const string UrlDustParticles = "http://saladgamer.com/vlb-doc/comp-dustparticles/";

			// Token: 0x040004D5 RID: 1237
			public const string UrlTriggerZone = "http://saladgamer.com/vlb-doc/comp-triggerzone/";

			// Token: 0x040004D6 RID: 1238
			public const string UrlEffectFlicker = "http://saladgamer.com/vlb-doc/comp-effect-flicker/";

			// Token: 0x040004D7 RID: 1239
			public const string UrlEffectPulse = "http://saladgamer.com/vlb-doc/comp-effect-pulse/";

			// Token: 0x040004D8 RID: 1240
			public const string UrlEffectFromProfile = "http://saladgamer.com/vlb-doc/comp-effect-from-profile/";

			// Token: 0x040004D9 RID: 1241
			public const string UrlConfig = "http://saladgamer.com/vlb-doc/config/";

			// Token: 0x020000EB RID: 235
			public static class SD
			{
				// Token: 0x040004DA RID: 1242
				public const string UrlBeam = "http://saladgamer.com/vlb-doc/comp-lightbeam-sd/";

				// Token: 0x040004DB RID: 1243
				public const string UrlDynamicOcclusionRaycasting = "http://saladgamer.com/vlb-doc/comp-dynocclusion-sd-raycasting/";

				// Token: 0x040004DC RID: 1244
				public const string UrlDynamicOcclusionDepthBuffer = "http://saladgamer.com/vlb-doc/comp-dynocclusion-sd-depthbuffer/";

				// Token: 0x040004DD RID: 1245
				public const string UrlSkewingHandle = "http://saladgamer.com/vlb-doc/comp-skewinghandle-sd/";
			}

			// Token: 0x020000EC RID: 236
			public static class HD
			{
				// Token: 0x040004DE RID: 1246
				public const string UrlBeam = "http://saladgamer.com/vlb-doc/comp-lightbeam-hd/";

				// Token: 0x040004DF RID: 1247
				public const string UrlShadow = "http://saladgamer.com/vlb-doc/comp-shadow-hd/";

				// Token: 0x040004E0 RID: 1248
				public const string UrlCookie = "http://saladgamer.com/vlb-doc/comp-cookie-hd/";

				// Token: 0x040004E1 RID: 1249
				public const string UrlTrackRealtimeChangesOnLight = "http://saladgamer.com/vlb-doc/comp-trackrealtimechanges-hd/";
			}
		}

		// Token: 0x020000ED RID: 237
		public static class Internal
		{
			// Token: 0x17000098 RID: 152
			// (get) Token: 0x060003FE RID: 1022 RVA: 0x0001646D File Offset: 0x0001466D
			public static HideFlags ProceduralObjectsHideFlags
			{
				get
				{
					if (!Consts.Internal.ProceduralObjectsVisibleInEditor)
					{
						return HideFlags.HideAndDontSave;
					}
					return HideFlags.DontSaveInEditor | HideFlags.NotEditable | HideFlags.DontSaveInBuild | HideFlags.DontUnloadUnusedAsset;
				}
			}

			// Token: 0x040004E2 RID: 1250
			public static readonly bool ProceduralObjectsVisibleInEditor = true;
		}

		// Token: 0x020000EE RID: 238
		public static class Beam
		{
			// Token: 0x040004E3 RID: 1251
			public static readonly Color FlatColor = Color.white;

			// Token: 0x040004E4 RID: 1252
			public const ColorMode ColorModeDefault = ColorMode.Flat;

			// Token: 0x040004E5 RID: 1253
			public const float MultiplierDefault = 1f;

			// Token: 0x040004E6 RID: 1254
			public const float MultiplierMin = 0f;

			// Token: 0x040004E7 RID: 1255
			public const float IntensityDefault = 1f;

			// Token: 0x040004E8 RID: 1256
			public const float IntensityMin = 0f;

			// Token: 0x040004E9 RID: 1257
			public const float HDRPExposureWeightDefault = 0f;

			// Token: 0x040004EA RID: 1258
			public const float HDRPExposureWeightMin = 0f;

			// Token: 0x040004EB RID: 1259
			public const float HDRPExposureWeightMax = 1f;

			// Token: 0x040004EC RID: 1260
			public const float SpotAngleDefault = 35f;

			// Token: 0x040004ED RID: 1261
			public const float SpotAngleMin = 0.1f;

			// Token: 0x040004EE RID: 1262
			public const float SpotAngleMax = 179.9f;

			// Token: 0x040004EF RID: 1263
			public const float ConeRadiusStart = 0.1f;

			// Token: 0x040004F0 RID: 1264
			public const MeshType GeomMeshType = MeshType.Shared;

			// Token: 0x040004F1 RID: 1265
			public const int GeomSidesDefault = 18;

			// Token: 0x040004F2 RID: 1266
			public const int GeomSidesMin = 3;

			// Token: 0x040004F3 RID: 1267
			public const int GeomSidesMax = 256;

			// Token: 0x040004F4 RID: 1268
			public const int GeomSegmentsDefault = 5;

			// Token: 0x040004F5 RID: 1269
			public const int GeomSegmentsMin = 0;

			// Token: 0x040004F6 RID: 1270
			public const int GeomSegmentsMax = 64;

			// Token: 0x040004F7 RID: 1271
			public const bool GeomCap = false;

			// Token: 0x040004F8 RID: 1272
			public const bool ScalableDefault = true;

			// Token: 0x040004F9 RID: 1273
			public const AttenuationEquation AttenuationEquationDefault = AttenuationEquation.Quadratic;

			// Token: 0x040004FA RID: 1274
			public const float AttenuationCustomBlendingDefault = 0.5f;

			// Token: 0x040004FB RID: 1275
			public const float AttenuationCustomBlendingMin = 0f;

			// Token: 0x040004FC RID: 1276
			public const float AttenuationCustomBlendingMax = 1f;

			// Token: 0x040004FD RID: 1277
			public const float FallOffStart = 0f;

			// Token: 0x040004FE RID: 1278
			public const float FallOffEnd = 3f;

			// Token: 0x040004FF RID: 1279
			public const float FallOffDistancesMinThreshold = 0.01f;

			// Token: 0x04000500 RID: 1280
			public const float DepthBlendDistance = 2f;

			// Token: 0x04000501 RID: 1281
			public const float CameraClippingDistance = 0.5f;

			// Token: 0x04000502 RID: 1282
			public const NoiseMode NoiseModeDefault = NoiseMode.Disabled;

			// Token: 0x04000503 RID: 1283
			public const float NoiseIntensityMin = 0f;

			// Token: 0x04000504 RID: 1284
			public const float NoiseIntensityMax = 1f;

			// Token: 0x04000505 RID: 1285
			public const float NoiseIntensityDefault = 0.5f;

			// Token: 0x04000506 RID: 1286
			public const float NoiseScaleMin = 0.01f;

			// Token: 0x04000507 RID: 1287
			public const float NoiseScaleMax = 2f;

			// Token: 0x04000508 RID: 1288
			public const float NoiseScaleDefault = 0.5f;

			// Token: 0x04000509 RID: 1289
			public static readonly Vector3 NoiseVelocityDefault = new Vector3(0.07f, 0.18f, 0.05f);

			// Token: 0x0400050A RID: 1290
			public const BlendingMode BlendingModeDefault = BlendingMode.Additive;

			// Token: 0x0400050B RID: 1291
			public const ShaderAccuracy ShaderAccuracyDefault = ShaderAccuracy.Fast;

			// Token: 0x0400050C RID: 1292
			public const float FadeOutBeginDefault = -150f;

			// Token: 0x0400050D RID: 1293
			public const float FadeOutEndDefault = -200f;

			// Token: 0x0400050E RID: 1294
			public const Dimensions DimensionsDefault = Dimensions.Dim3D;

			// Token: 0x020000EF RID: 239
			public static class SD
			{
				// Token: 0x0400050F RID: 1295
				public const float FresnelPowMaxValue = 10f;

				// Token: 0x04000510 RID: 1296
				public const float FresnelPow = 8f;

				// Token: 0x04000511 RID: 1297
				public const float GlareFrontalDefault = 0.5f;

				// Token: 0x04000512 RID: 1298
				public const float GlareBehindDefault = 0.5f;

				// Token: 0x04000513 RID: 1299
				public const float GlareMin = 0f;

				// Token: 0x04000514 RID: 1300
				public const float GlareMax = 1f;

				// Token: 0x04000515 RID: 1301
				public static readonly Vector2 TiltDefault = Vector2.zero;

				// Token: 0x04000516 RID: 1302
				public static readonly Vector3 SkewingLocalForwardDirectionDefault = Vector3.forward;

				// Token: 0x04000517 RID: 1303
				public const Transform ClippingPlaneTransformDefault = null;
			}

			// Token: 0x020000F0 RID: 240
			public static class HD
			{
				// Token: 0x04000518 RID: 1304
				public const AttenuationEquationHD AttenuationEquationDefault = AttenuationEquationHD.Quadratic;

				// Token: 0x04000519 RID: 1305
				public const float SideSoftnessDefault = 1f;

				// Token: 0x0400051A RID: 1306
				public const float SideSoftnessMin = 0.0001f;

				// Token: 0x0400051B RID: 1307
				public const float SideSoftnessMax = 10f;

				// Token: 0x0400051C RID: 1308
				public const float JitteringFactorDefault = 0f;

				// Token: 0x0400051D RID: 1309
				public const float JitteringFactorMin = 0f;

				// Token: 0x0400051E RID: 1310
				public const int JitteringFrameRateDefault = 60;

				// Token: 0x0400051F RID: 1311
				public const int JitteringFrameRateMin = 0;

				// Token: 0x04000520 RID: 1312
				public const int JitteringFrameRateMax = 120;

				// Token: 0x04000521 RID: 1313
				public static readonly MinMaxRangeFloat JitteringLerpRange = new MinMaxRangeFloat(0f, 0.33f);
			}
		}

		// Token: 0x020000F1 RID: 241
		public static class DustParticles
		{
			// Token: 0x04000522 RID: 1314
			public const float AlphaDefault = 0.5f;

			// Token: 0x04000523 RID: 1315
			public const float SizeDefault = 0.01f;

			// Token: 0x04000524 RID: 1316
			public const ParticlesDirection DirectionDefault = ParticlesDirection.Random;

			// Token: 0x04000525 RID: 1317
			public static readonly Vector3 VelocityDefault = new Vector3(0f, 0f, 0.03f);

			// Token: 0x04000526 RID: 1318
			public const float DensityDefault = 5f;

			// Token: 0x04000527 RID: 1319
			public const float DensityMin = 0f;

			// Token: 0x04000528 RID: 1320
			public const float DensityMax = 1000f;

			// Token: 0x04000529 RID: 1321
			public static readonly MinMaxRangeFloat SpawnDistanceRangeDefault = new MinMaxRangeFloat(0f, 0.7f);

			// Token: 0x0400052A RID: 1322
			public const bool CullingEnabledDefault = false;

			// Token: 0x0400052B RID: 1323
			public const float CullingMaxDistanceDefault = 10f;

			// Token: 0x0400052C RID: 1324
			public const float CullingMaxDistanceMin = 1f;
		}

		// Token: 0x020000F2 RID: 242
		public static class DynOcclusion
		{
			// Token: 0x0400052D RID: 1325
			public static readonly LayerMask LayerMaskDefault = 1;

			// Token: 0x0400052E RID: 1326
			public const DynamicOcclusionUpdateRate UpdateRateDefault = DynamicOcclusionUpdateRate.EveryXFrames;

			// Token: 0x0400052F RID: 1327
			public const int WaitFramesCountDefault = 3;

			// Token: 0x04000530 RID: 1328
			public const Dimensions RaycastingDimensionsDefault = Dimensions.Dim3D;

			// Token: 0x04000531 RID: 1329
			public const bool RaycastingConsiderTriggersDefault = false;

			// Token: 0x04000532 RID: 1330
			public const float RaycastingMinOccluderAreaDefault = 0f;

			// Token: 0x04000533 RID: 1331
			public const float RaycastingMinSurfaceRatioDefault = 0.5f;

			// Token: 0x04000534 RID: 1332
			public const float RaycastingMinSurfaceRatioMin = 50f;

			// Token: 0x04000535 RID: 1333
			public const float RaycastingMinSurfaceRatioMax = 100f;

			// Token: 0x04000536 RID: 1334
			public const float RaycastingMaxSurfaceDotDefault = 0.25f;

			// Token: 0x04000537 RID: 1335
			public const float RaycastingMaxSurfaceAngleMin = 45f;

			// Token: 0x04000538 RID: 1336
			public const float RaycastingMaxSurfaceAngleMax = 90f;

			// Token: 0x04000539 RID: 1337
			public const PlaneAlignment RaycastingPlaneAlignmentDefault = PlaneAlignment.Surface;

			// Token: 0x0400053A RID: 1338
			public const float RaycastingPlaneOffsetDefault = 0.1f;

			// Token: 0x0400053B RID: 1339
			public const float RaycastingFadeDistanceToSurfaceDefault = 0.25f;

			// Token: 0x0400053C RID: 1340
			public const int DepthBufferDepthMapResolutionDefault = 128;

			// Token: 0x0400053D RID: 1341
			public const bool DepthBufferOcclusionCullingDefault = true;

			// Token: 0x0400053E RID: 1342
			public const float DepthBufferFadeDistanceToSurfaceDefault = 0f;
		}

		// Token: 0x020000F3 RID: 243
		public static class Effects
		{
			// Token: 0x0400053F RID: 1343
			public const EffectAbstractBase.ComponentsToChange ComponentsToChangeDefault = (EffectAbstractBase.ComponentsToChange)2147483647;

			// Token: 0x04000540 RID: 1344
			public const bool RestoreIntensityOnDisableDefault = true;

			// Token: 0x04000541 RID: 1345
			public const float FrequencyDefault = 10f;

			// Token: 0x04000542 RID: 1346
			public const bool PerformPausesDefault = false;

			// Token: 0x04000543 RID: 1347
			public const bool RestoreIntensityOnPauseDefault = false;

			// Token: 0x04000544 RID: 1348
			public static readonly MinMaxRangeFloat FlickeringDurationDefault = new MinMaxRangeFloat(1f, 4f);

			// Token: 0x04000545 RID: 1349
			public static readonly MinMaxRangeFloat PauseDurationDefault = new MinMaxRangeFloat(0f, 1f);

			// Token: 0x04000546 RID: 1350
			public static readonly MinMaxRangeFloat IntensityAmplitudeDefault = new MinMaxRangeFloat(-1f, 1f);

			// Token: 0x04000547 RID: 1351
			public const float SmoothingDefault = 0.05f;
		}

		// Token: 0x020000F4 RID: 244
		public static class Shadow
		{
			// Token: 0x06000406 RID: 1030 RVA: 0x0001654E File Offset: 0x0001474E
			public static string GetErrorChangeRuntimeDepthMapResolution(VolumetricShadowHD comp)
			{
				return string.Format("Can't change {0} Shadow.depthMapResolution property at runtime after DepthCamera initialization", comp.name);
			}

			// Token: 0x04000548 RID: 1352
			public const float StrengthDefault = 1f;

			// Token: 0x04000549 RID: 1353
			public const float StrengthMin = 0f;

			// Token: 0x0400054A RID: 1354
			public const float StrengthMax = 1f;

			// Token: 0x0400054B RID: 1355
			public static readonly LayerMask LayerMaskDefault = 1;

			// Token: 0x0400054C RID: 1356
			public const ShadowUpdateRate UpdateRateDefault = ShadowUpdateRate.EveryXFrames;

			// Token: 0x0400054D RID: 1357
			public const int WaitFramesCountDefault = 3;

			// Token: 0x0400054E RID: 1358
			public const int DepthMapResolutionDefault = 128;

			// Token: 0x0400054F RID: 1359
			public const bool OcclusionCullingDefault = true;
		}

		// Token: 0x020000F5 RID: 245
		public static class Cookie
		{
			// Token: 0x04000550 RID: 1360
			public const float ContributionDefault = 1f;

			// Token: 0x04000551 RID: 1361
			public const float ContributionMin = 0f;

			// Token: 0x04000552 RID: 1362
			public const float ContributionMax = 1f;

			// Token: 0x04000553 RID: 1363
			public const Texture CookieTextureDefault = null;

			// Token: 0x04000554 RID: 1364
			public const CookieChannel ChannelDefault = CookieChannel.Alpha;

			// Token: 0x04000555 RID: 1365
			public const bool NegativeDefault = false;

			// Token: 0x04000556 RID: 1366
			public static readonly Vector2 TranslationDefault = Vector2.zero;

			// Token: 0x04000557 RID: 1367
			public const float RotationDefault = 0f;

			// Token: 0x04000558 RID: 1368
			public static readonly Vector2 ScaleDefault = Vector2.one;
		}

		// Token: 0x020000F6 RID: 246
		public static class Config
		{
			// Token: 0x04000559 RID: 1369
			public const bool GeometryOverrideLayerDefault = true;

			// Token: 0x0400055A RID: 1370
			public const int GeometryLayerIDDefault = 1;

			// Token: 0x0400055B RID: 1371
			public const string GeometryTagDefault = "Untagged";

			// Token: 0x0400055C RID: 1372
			public const string FadeOutCameraTagDefault = "MainCamera";

			// Token: 0x0400055D RID: 1373
			public const RenderQueue GeometryRenderQueueDefault = RenderQueue.Transparent;

			// Token: 0x0400055E RID: 1374
			public const RenderPipeline GeometryRenderPipelineDefault = RenderPipeline.BuiltIn;

			// Token: 0x0400055F RID: 1375
			public const RenderingMode GeometryRenderingModeDefault = RenderingMode.Default;

			// Token: 0x04000560 RID: 1376
			public const int Noise3DSizeDefault = 64;

			// Token: 0x04000561 RID: 1377
			public const float DitheringFactor = 0f;

			// Token: 0x04000562 RID: 1378
			public const bool UseLightColorTemperatureDefault = true;

			// Token: 0x04000563 RID: 1379
			public const bool FeatureEnabledDefault = true;

			// Token: 0x04000564 RID: 1380
			public const FeatureEnabledColorGradient FeatureEnabledColorGradientDefault = FeatureEnabledColorGradient.HighOnly;

			// Token: 0x04000565 RID: 1381
			public const int SharedMeshSidesDefault = 24;

			// Token: 0x04000566 RID: 1382
			public const int SharedMeshSidesMin = 3;

			// Token: 0x04000567 RID: 1383
			public const int SharedMeshSidesMax = 256;

			// Token: 0x04000568 RID: 1384
			public const int SharedMeshSegmentsDefault = 5;

			// Token: 0x04000569 RID: 1385
			public const int SharedMeshSegmentsMin = 0;

			// Token: 0x0400056A RID: 1386
			public const int SharedMeshSegmentsMax = 64;

			// Token: 0x020000F7 RID: 247
			public static class HD
			{
				// Token: 0x0400056B RID: 1387
				public const RenderQueue GeometryRenderQueueDefault = (RenderQueue)3100;

				// Token: 0x0400056C RID: 1388
				public const float CameraBlendingDistance = 0.5f;

				// Token: 0x0400056D RID: 1389
				public const int RaymarchingQualitiesStepsMin = 2;
			}
		}
	}
}
