using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001D5 RID: 469
	public class SkyMaterialController
	{
		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000967 RID: 2407 RVA: 0x0002B3C7 File Offset: 0x000295C7
		// (set) Token: 0x06000968 RID: 2408 RVA: 0x0002B3CF File Offset: 0x000295CF
		public Material SkyboxMaterial
		{
			get
			{
				return this._skyboxMaterial;
			}
			set
			{
				this._skyboxMaterial = value;
				RenderSettings.skybox = this._skyboxMaterial;
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000969 RID: 2409 RVA: 0x0002B3E3 File Offset: 0x000295E3
		// (set) Token: 0x0600096A RID: 2410 RVA: 0x0002B3EB File Offset: 0x000295EB
		public Color SkyColor
		{
			get
			{
				return this._skyColor;
			}
			set
			{
				this._skyColor = value;
				this.SkyboxMaterial.SetColor("_GradientSkyUpperColor", this._skyColor);
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x0600096B RID: 2411 RVA: 0x0002B40A File Offset: 0x0002960A
		// (set) Token: 0x0600096C RID: 2412 RVA: 0x0002B412 File Offset: 0x00029612
		public Color SkyMiddleColor
		{
			get
			{
				return this._skyMiddleColor;
			}
			set
			{
				this._skyMiddleColor = value;
				this.SkyboxMaterial.SetColor("_GradientSkyMiddleColor", this._skyMiddleColor);
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x0600096D RID: 2413 RVA: 0x0002B431 File Offset: 0x00029631
		// (set) Token: 0x0600096E RID: 2414 RVA: 0x0002B439 File Offset: 0x00029639
		public Color HorizonColor
		{
			get
			{
				return this._horizonColor;
			}
			set
			{
				this._horizonColor = value;
				this.SkyboxMaterial.SetColor("_GradientSkyLowerColor", this._horizonColor);
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x0600096F RID: 2415 RVA: 0x0002B458 File Offset: 0x00029658
		// (set) Token: 0x06000970 RID: 2416 RVA: 0x0002B460 File Offset: 0x00029660
		public float GradientFadeBegin
		{
			get
			{
				return this._gradientFadeBegin;
			}
			set
			{
				this._gradientFadeBegin = value;
				this.ApplyGradientValuesOnMaterial();
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000971 RID: 2417 RVA: 0x0002B46F File Offset: 0x0002966F
		// (set) Token: 0x06000972 RID: 2418 RVA: 0x0002B477 File Offset: 0x00029677
		public float GradientFadeLength
		{
			get
			{
				return this._gradientFadeLength;
			}
			set
			{
				this._gradientFadeLength = value;
				this.ApplyGradientValuesOnMaterial();
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000973 RID: 2419 RVA: 0x0002B486 File Offset: 0x00029686
		// (set) Token: 0x06000974 RID: 2420 RVA: 0x0002B48E File Offset: 0x0002968E
		public float SkyMiddlePosition
		{
			get
			{
				return this._skyMiddlePosition;
			}
			set
			{
				this._skyMiddlePosition = value;
				this.SkyboxMaterial.SetFloat("_GradientFadeMiddlePosition", this._skyMiddlePosition);
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000975 RID: 2421 RVA: 0x0002B4AD File Offset: 0x000296AD
		// (set) Token: 0x06000976 RID: 2422 RVA: 0x0002B4B5 File Offset: 0x000296B5
		public Cubemap BackgroundCubemap
		{
			get
			{
				return this._backgroundCubemap;
			}
			set
			{
				this._backgroundCubemap = value;
				this.SkyboxMaterial.SetTexture("_MainTex", this._backgroundCubemap);
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000977 RID: 2423 RVA: 0x0002B4D4 File Offset: 0x000296D4
		// (set) Token: 0x06000978 RID: 2424 RVA: 0x0002B4DC File Offset: 0x000296DC
		public float StarFadeBegin
		{
			get
			{
				return this._starFadeBegin;
			}
			set
			{
				this._starFadeBegin = value;
				this.ApplyStarFadeValuesOnMaterial();
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000979 RID: 2425 RVA: 0x0002B4EB File Offset: 0x000296EB
		// (set) Token: 0x0600097A RID: 2426 RVA: 0x0002B4F3 File Offset: 0x000296F3
		public float StarFadeLength
		{
			get
			{
				return this._starFadeLength;
			}
			set
			{
				this._starFadeLength = value;
				this.ApplyStarFadeValuesOnMaterial();
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x0600097B RID: 2427 RVA: 0x0002B502 File Offset: 0x00029702
		// (set) Token: 0x0600097C RID: 2428 RVA: 0x0002B50A File Offset: 0x0002970A
		public float HorizonDistanceScale
		{
			get
			{
				return this._horizonDistanceScale;
			}
			set
			{
				this._horizonDistanceScale = value;
				this.SkyboxMaterial.SetFloat("_HorizonScaleFactor", this._horizonDistanceScale);
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x0600097D RID: 2429 RVA: 0x0002B529 File Offset: 0x00029729
		// (set) Token: 0x0600097E RID: 2430 RVA: 0x0002B531 File Offset: 0x00029731
		public Texture StarBasicCubemap
		{
			get
			{
				return this._starBasicCubemap;
			}
			set
			{
				this._starBasicCubemap = value;
				this.SkyboxMaterial.SetTexture("_StarBasicCubemap", this._starBasicCubemap);
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x0600097F RID: 2431 RVA: 0x0002B550 File Offset: 0x00029750
		// (set) Token: 0x06000980 RID: 2432 RVA: 0x0002B558 File Offset: 0x00029758
		public float StarBasicTwinkleSpeed
		{
			get
			{
				return this._starBasicTwinkleSpeed;
			}
			set
			{
				this._starBasicTwinkleSpeed = value;
				this.SkyboxMaterial.SetFloat("_StarBasicTwinkleSpeed", this._starBasicTwinkleSpeed);
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000981 RID: 2433 RVA: 0x0002B577 File Offset: 0x00029777
		// (set) Token: 0x06000982 RID: 2434 RVA: 0x0002B57F File Offset: 0x0002977F
		public float StarBasicTwinkleAmount
		{
			get
			{
				return this._starBasicTwinkleAmount;
			}
			set
			{
				this._starBasicTwinkleAmount = value;
				this.SkyboxMaterial.SetFloat("_StarBasicTwinkleAmount", this._starBasicTwinkleAmount);
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000983 RID: 2435 RVA: 0x0002B59E File Offset: 0x0002979E
		// (set) Token: 0x06000984 RID: 2436 RVA: 0x0002B5A6 File Offset: 0x000297A6
		public float StarBasicOpacity
		{
			get
			{
				return this._starBasicOpacity;
			}
			set
			{
				this._starBasicOpacity = value;
				this.SkyboxMaterial.SetFloat("_StarBasicOpacity", this._starBasicOpacity);
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000985 RID: 2437 RVA: 0x0002B5C5 File Offset: 0x000297C5
		// (set) Token: 0x06000986 RID: 2438 RVA: 0x0002B5CD File Offset: 0x000297CD
		public Color StarBasicTintColor
		{
			get
			{
				return this._starBasicTintColor;
			}
			set
			{
				this._starBasicTintColor = value;
				this.SkyboxMaterial.SetColor("_StarBasicTintColor", this._starBasicTintColor);
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000987 RID: 2439 RVA: 0x0002B5EC File Offset: 0x000297EC
		// (set) Token: 0x06000988 RID: 2440 RVA: 0x0002B5F4 File Offset: 0x000297F4
		public float StarBasicExponent
		{
			get
			{
				return this._starBasicExponent;
			}
			set
			{
				this._starBasicExponent = value;
				this.SkyboxMaterial.SetFloat("_StarBasicExponent", this._starBasicExponent);
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000989 RID: 2441 RVA: 0x0002B613 File Offset: 0x00029813
		// (set) Token: 0x0600098A RID: 2442 RVA: 0x0002B61B File Offset: 0x0002981B
		public float StarBasicIntensity
		{
			get
			{
				return this._starBasicIntensity;
			}
			set
			{
				this._starBasicIntensity = value;
				this.SkyboxMaterial.SetFloat("_StarBasicHDRBoost", this._starBasicIntensity);
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x0600098B RID: 2443 RVA: 0x0002B63A File Offset: 0x0002983A
		// (set) Token: 0x0600098C RID: 2444 RVA: 0x0002B642 File Offset: 0x00029842
		public Texture StarLayer1Texture
		{
			get
			{
				return this._starLayer1Texture;
			}
			set
			{
				this._starLayer1Texture = value;
				this.SkyboxMaterial.SetTexture("_StarLayer1Tex", this._starLayer1Texture);
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x0600098D RID: 2445 RVA: 0x0002B661 File Offset: 0x00029861
		// (set) Token: 0x0600098E RID: 2446 RVA: 0x0002B669 File Offset: 0x00029869
		public Texture2D StarLayer1DataTexture
		{
			get
			{
				return this._starLayer1DataTexture;
			}
			set
			{
				this._starLayer1DataTexture = value;
				this.SkyboxMaterial.SetTexture("_StarLayer1DataTex", value);
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x0600098F RID: 2447 RVA: 0x0002B683 File Offset: 0x00029883
		// (set) Token: 0x06000990 RID: 2448 RVA: 0x0002B68B File Offset: 0x0002988B
		public Color StarLayer1Color
		{
			get
			{
				return this._starLayer1Color;
			}
			set
			{
				this._starLayer1Color = value;
				this.SkyboxMaterial.SetColor("_StarLayer1Color", this._starLayer1Color);
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000991 RID: 2449 RVA: 0x0002B6AA File Offset: 0x000298AA
		// (set) Token: 0x06000992 RID: 2450 RVA: 0x0002B6B2 File Offset: 0x000298B2
		public float StarLayer1MaxRadius
		{
			get
			{
				return this._starLayer1MaxRadius;
			}
			set
			{
				this._starLayer1MaxRadius = value;
				this.SkyboxMaterial.SetFloat("_StarLayer1MaxRadius", this._starLayer1MaxRadius);
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000993 RID: 2451 RVA: 0x0002B6D1 File Offset: 0x000298D1
		// (set) Token: 0x06000994 RID: 2452 RVA: 0x0002B6D9 File Offset: 0x000298D9
		public float StarLayer1TwinkleAmount
		{
			get
			{
				return this._starLayer1TwinkleAmount;
			}
			set
			{
				this._starLayer1TwinkleAmount = value;
				this.SkyboxMaterial.SetFloat("_StarLayer1TwinkleAmount", this._starLayer1TwinkleAmount);
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000995 RID: 2453 RVA: 0x0002B6F8 File Offset: 0x000298F8
		// (set) Token: 0x06000996 RID: 2454 RVA: 0x0002B700 File Offset: 0x00029900
		public float StarLayer1TwinkleSpeed
		{
			get
			{
				return this._starLayer1TwinkleSpeed;
			}
			set
			{
				this._starLayer1TwinkleSpeed = value;
				this.SkyboxMaterial.SetFloat("_StarLayer1TwinkleSpeed", this._starLayer1TwinkleSpeed);
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000997 RID: 2455 RVA: 0x0002B71F File Offset: 0x0002991F
		// (set) Token: 0x06000998 RID: 2456 RVA: 0x0002B727 File Offset: 0x00029927
		public float StarLayer1RotationSpeed
		{
			get
			{
				return this._starLayer1RotationSpeed;
			}
			set
			{
				this._starLayer1RotationSpeed = value;
				this.SkyboxMaterial.SetFloat("_StarLayer1RotationSpeed", this._starLayer1RotationSpeed);
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000999 RID: 2457 RVA: 0x0002B746 File Offset: 0x00029946
		// (set) Token: 0x0600099A RID: 2458 RVA: 0x0002B74E File Offset: 0x0002994E
		public float StarLayer1EdgeFeathering
		{
			get
			{
				return this._starLayer1EdgeFeathering;
			}
			set
			{
				this._starLayer1EdgeFeathering = value;
				this.SkyboxMaterial.SetFloat("_StarLayer1EdgeFade", this._starLayer1EdgeFeathering);
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x0600099B RID: 2459 RVA: 0x0002B76D File Offset: 0x0002996D
		// (set) Token: 0x0600099C RID: 2460 RVA: 0x0002B775 File Offset: 0x00029975
		public float StarLayer1BloomFilterBoost
		{
			get
			{
				return this._starLayer1BloomFilterBoost;
			}
			set
			{
				this._starLayer1BloomFilterBoost = value;
				this.SkyboxMaterial.SetFloat("_StarLayer1HDRBoost", this._starLayer1BloomFilterBoost);
			}
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x0002B794 File Offset: 0x00029994
		public void SetStarLayer1SpriteDimensions(int columns, int rows)
		{
			this._starLayer1SpriteDimensions.x = (float)columns;
			this._starLayer1SpriteDimensions.y = (float)rows;
			this.SkyboxMaterial.SetVector("_StarLayer1SpriteDimensions", this._starLayer1SpriteDimensions);
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x0002B7C6 File Offset: 0x000299C6
		public Vector2 GetStarLayer1SpriteDimensions()
		{
			return new Vector2(this._starLayer1SpriteDimensions.x, this._starLayer1SpriteDimensions.y);
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x0600099F RID: 2463 RVA: 0x0002B7E3 File Offset: 0x000299E3
		// (set) Token: 0x060009A0 RID: 2464 RVA: 0x0002B7EB File Offset: 0x000299EB
		public int StarLayer1SpriteItemCount
		{
			get
			{
				return this._starLayer1SpriteItemCount;
			}
			set
			{
				this._starLayer1SpriteItemCount = value;
				this.SkyboxMaterial.SetInt("_StarLayer1SpriteItemCount", this._starLayer1SpriteItemCount);
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x060009A1 RID: 2465 RVA: 0x0002B80A File Offset: 0x00029A0A
		// (set) Token: 0x060009A2 RID: 2466 RVA: 0x0002B812 File Offset: 0x00029A12
		public float StarLayer1SpriteAnimationSpeed
		{
			get
			{
				return this._starLayer1SpriteAnimationSpeed;
			}
			set
			{
				this._starLayer1SpriteAnimationSpeed = value;
				this.SkyboxMaterial.SetFloat("_StarLayer1SpriteAnimationSpeed", this._starLayer1SpriteAnimationSpeed);
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x060009A3 RID: 2467 RVA: 0x0002B831 File Offset: 0x00029A31
		// (set) Token: 0x060009A4 RID: 2468 RVA: 0x0002B839 File Offset: 0x00029A39
		public Texture StarLayer2Texture
		{
			get
			{
				return this._starLayer2Texture;
			}
			set
			{
				this._starLayer2Texture = value;
				this.SkyboxMaterial.SetTexture("_StarLayer2Tex", this._starLayer2Texture);
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x060009A5 RID: 2469 RVA: 0x0002B858 File Offset: 0x00029A58
		// (set) Token: 0x060009A6 RID: 2470 RVA: 0x0002B860 File Offset: 0x00029A60
		public Texture2D StarLayer2DataTexture
		{
			get
			{
				return this._starLayer2DataTexture;
			}
			set
			{
				this._starLayer2DataTexture = value;
				this.SkyboxMaterial.SetTexture("_StarLayer2DataTex", value);
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x060009A7 RID: 2471 RVA: 0x0002B87A File Offset: 0x00029A7A
		// (set) Token: 0x060009A8 RID: 2472 RVA: 0x0002B882 File Offset: 0x00029A82
		public Color StarLayer2Color
		{
			get
			{
				return this._starLayer2Color;
			}
			set
			{
				this._starLayer2Color = value;
				this.SkyboxMaterial.SetColor("_StarLayer2Color", this._starLayer2Color);
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x060009A9 RID: 2473 RVA: 0x0002B8A1 File Offset: 0x00029AA1
		// (set) Token: 0x060009AA RID: 2474 RVA: 0x0002B8A9 File Offset: 0x00029AA9
		public float StarLayer2MaxRadius
		{
			get
			{
				return this._starLayer2MaxRadius;
			}
			set
			{
				this._starLayer2MaxRadius = value;
				this.SkyboxMaterial.SetFloat("_StarLayer2MaxRadius", this._starLayer2MaxRadius);
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x060009AB RID: 2475 RVA: 0x0002B8C8 File Offset: 0x00029AC8
		// (set) Token: 0x060009AC RID: 2476 RVA: 0x0002B8D0 File Offset: 0x00029AD0
		public float StarLayer2TwinkleAmount
		{
			get
			{
				return this._starLayer2TwinkleAmount;
			}
			set
			{
				this._starLayer2TwinkleAmount = value;
				this.SkyboxMaterial.SetFloat("_StarLayer2TwinkleAmount", this._starLayer2TwinkleAmount);
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x060009AD RID: 2477 RVA: 0x0002B8EF File Offset: 0x00029AEF
		// (set) Token: 0x060009AE RID: 2478 RVA: 0x0002B8F7 File Offset: 0x00029AF7
		public float StarLayer2TwinkleSpeed
		{
			get
			{
				return this._starLayer2TwinkleSpeed;
			}
			set
			{
				this._starLayer2TwinkleSpeed = value;
				this.SkyboxMaterial.SetFloat("_StarLayer2TwinkleSpeed", this._starLayer2TwinkleSpeed);
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x060009AF RID: 2479 RVA: 0x0002B916 File Offset: 0x00029B16
		// (set) Token: 0x060009B0 RID: 2480 RVA: 0x0002B91E File Offset: 0x00029B1E
		public float StarLayer2RotationSpeed
		{
			get
			{
				return this._starLayer2RotationSpeed;
			}
			set
			{
				this._starLayer2RotationSpeed = value;
				this.SkyboxMaterial.SetFloat("_StarLayer2RotationSpeed", this._starLayer2RotationSpeed);
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x060009B1 RID: 2481 RVA: 0x0002B93D File Offset: 0x00029B3D
		// (set) Token: 0x060009B2 RID: 2482 RVA: 0x0002B945 File Offset: 0x00029B45
		public float StarLayer2EdgeFeathering
		{
			get
			{
				return this._starLayer2EdgeFeathering;
			}
			set
			{
				this._starLayer2EdgeFeathering = value;
				this.SkyboxMaterial.SetFloat("_StarLayer2EdgeFade", this._starLayer2EdgeFeathering);
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x060009B3 RID: 2483 RVA: 0x0002B964 File Offset: 0x00029B64
		// (set) Token: 0x060009B4 RID: 2484 RVA: 0x0002B96C File Offset: 0x00029B6C
		public float StarLayer2BloomFilterBoost
		{
			get
			{
				return this._starLayer2BloomFilterBoost;
			}
			set
			{
				this._starLayer2BloomFilterBoost = value;
				this.SkyboxMaterial.SetFloat("_StarLayer2HDRBoost", this._starLayer2BloomFilterBoost);
			}
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x0002B98B File Offset: 0x00029B8B
		public void SetStarLayer2SpriteDimensions(int columns, int rows)
		{
			this._starLayer2SpriteDimensions.x = (float)columns;
			this._starLayer2SpriteDimensions.y = (float)rows;
			this.SkyboxMaterial.SetVector("_StarLayer2SpriteDimensions", this._starLayer2SpriteDimensions);
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x0002B9BD File Offset: 0x00029BBD
		public Vector2 GetStarLayer2SpriteDimensions()
		{
			return new Vector2(this._starLayer2SpriteDimensions.x, this._starLayer2SpriteDimensions.y);
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x060009B7 RID: 2487 RVA: 0x0002B9DA File Offset: 0x00029BDA
		// (set) Token: 0x060009B8 RID: 2488 RVA: 0x0002B9E2 File Offset: 0x00029BE2
		public int StarLayer2SpriteItemCount
		{
			get
			{
				return this._starLayer2SpriteItemCount;
			}
			set
			{
				this._starLayer2SpriteItemCount = value;
				this.SkyboxMaterial.SetInt("_StarLayer2SpriteItemCount", this._starLayer2SpriteItemCount);
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x060009B9 RID: 2489 RVA: 0x0002BA01 File Offset: 0x00029C01
		// (set) Token: 0x060009BA RID: 2490 RVA: 0x0002BA09 File Offset: 0x00029C09
		public float StarLayer2SpriteAnimationSpeed
		{
			get
			{
				return this._starLayer2SpriteAnimationSpeed;
			}
			set
			{
				this._starLayer2SpriteAnimationSpeed = value;
				this.SkyboxMaterial.SetFloat("_StarLayer2SpriteAnimationSpeed", this._starLayer2SpriteAnimationSpeed);
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x060009BB RID: 2491 RVA: 0x0002BA28 File Offset: 0x00029C28
		// (set) Token: 0x060009BC RID: 2492 RVA: 0x0002BA30 File Offset: 0x00029C30
		public Texture StarLayer3Texture
		{
			get
			{
				return this._starLayer3Texture;
			}
			set
			{
				this._starLayer3Texture = value;
				this.SkyboxMaterial.SetTexture("_StarLayer3Tex", this._starLayer3Texture);
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x060009BD RID: 2493 RVA: 0x0002BA4F File Offset: 0x00029C4F
		// (set) Token: 0x060009BE RID: 2494 RVA: 0x0002BA57 File Offset: 0x00029C57
		public Texture2D StarLayer3DataTexture
		{
			get
			{
				return this._starLayer3DataTexture;
			}
			set
			{
				this._starLayer3DataTexture = value;
				this.SkyboxMaterial.SetTexture("_StarLayer3DataTex", value);
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x060009BF RID: 2495 RVA: 0x0002BA71 File Offset: 0x00029C71
		// (set) Token: 0x060009C0 RID: 2496 RVA: 0x0002BA79 File Offset: 0x00029C79
		public Color StarLayer3Color
		{
			get
			{
				return this._starLayer3Color;
			}
			set
			{
				this._starLayer3Color = value;
				this.SkyboxMaterial.SetColor("_StarLayer3Color", this._starLayer3Color);
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x060009C1 RID: 2497 RVA: 0x0002BA98 File Offset: 0x00029C98
		// (set) Token: 0x060009C2 RID: 2498 RVA: 0x0002BAA0 File Offset: 0x00029CA0
		public float StarLayer3MaxRadius
		{
			get
			{
				return this._starLayer3MaxRadius;
			}
			set
			{
				this._starLayer3MaxRadius = value;
				this.SkyboxMaterial.SetFloat("_StarLayer3MaxRadius", this._starLayer3MaxRadius);
			}
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x060009C3 RID: 2499 RVA: 0x0002BABF File Offset: 0x00029CBF
		// (set) Token: 0x060009C4 RID: 2500 RVA: 0x0002BAC7 File Offset: 0x00029CC7
		public float StarLayer3TwinkleAmount
		{
			get
			{
				return this._starLayer3TwinkleAmount;
			}
			set
			{
				this._starLayer3TwinkleAmount = value;
				this.SkyboxMaterial.SetFloat("_StarLayer3TwinkleAmount", this._starLayer3TwinkleAmount);
			}
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x060009C5 RID: 2501 RVA: 0x0002BAE6 File Offset: 0x00029CE6
		// (set) Token: 0x060009C6 RID: 2502 RVA: 0x0002BAEE File Offset: 0x00029CEE
		public float StarLayer3TwinkleSpeed
		{
			get
			{
				return this._starLayer3TwinkleSpeed;
			}
			set
			{
				this._starLayer3TwinkleSpeed = value;
				this.SkyboxMaterial.SetFloat("_StarLayer3TwinkleSpeed", this._starLayer3TwinkleSpeed);
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x060009C7 RID: 2503 RVA: 0x0002BB0D File Offset: 0x00029D0D
		// (set) Token: 0x060009C8 RID: 2504 RVA: 0x0002BB15 File Offset: 0x00029D15
		public float StarLayer3RotationSpeed
		{
			get
			{
				return this._starLayer3RotationSpeed;
			}
			set
			{
				this._starLayer3RotationSpeed = value;
				this.SkyboxMaterial.SetFloat("_StarLayer3RotationSpeed", this._starLayer3RotationSpeed);
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x060009C9 RID: 2505 RVA: 0x0002BB34 File Offset: 0x00029D34
		// (set) Token: 0x060009CA RID: 2506 RVA: 0x0002BB3C File Offset: 0x00029D3C
		public float StarLayer3EdgeFeathering
		{
			get
			{
				return this._starLayer3EdgeFeathering;
			}
			set
			{
				this._starLayer3EdgeFeathering = value;
				this.SkyboxMaterial.SetFloat("_StarLayer3EdgeFade", this._starLayer3EdgeFeathering);
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x060009CB RID: 2507 RVA: 0x0002BB5B File Offset: 0x00029D5B
		// (set) Token: 0x060009CC RID: 2508 RVA: 0x0002BB63 File Offset: 0x00029D63
		public float StarLayer3BloomFilterBoost
		{
			get
			{
				return this._starLayer3BloomFilterBoost;
			}
			set
			{
				this._starLayer3BloomFilterBoost = value;
				this.SkyboxMaterial.SetFloat("_StarLayer3HDRBoost", this._starLayer3BloomFilterBoost);
			}
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x0002BB82 File Offset: 0x00029D82
		public void SetStarLayer3SpriteDimensions(int columns, int rows)
		{
			this._starLayer3SpriteDimensions.x = (float)columns;
			this._starLayer3SpriteDimensions.y = (float)rows;
			this.SkyboxMaterial.SetVector("_StarLayer3SpriteDimensions", this._starLayer3SpriteDimensions);
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x0002BBB4 File Offset: 0x00029DB4
		public Vector2 GetStarLayer3SpriteDimensions()
		{
			return new Vector2(this._starLayer3SpriteDimensions.x, this._starLayer3SpriteDimensions.y);
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x060009CF RID: 2511 RVA: 0x0002BBD1 File Offset: 0x00029DD1
		// (set) Token: 0x060009D0 RID: 2512 RVA: 0x0002BBD9 File Offset: 0x00029DD9
		public int StarLayer3SpriteItemCount
		{
			get
			{
				return this._starLayer3SpriteItemCount;
			}
			set
			{
				this._starLayer3SpriteItemCount = value;
				this.SkyboxMaterial.SetInt("_StarLayer3SpriteItemCount", this._starLayer3SpriteItemCount);
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x060009D1 RID: 2513 RVA: 0x0002BBF8 File Offset: 0x00029DF8
		// (set) Token: 0x060009D2 RID: 2514 RVA: 0x0002BC00 File Offset: 0x00029E00
		public float StarLayer3SpriteAnimationSpeed
		{
			get
			{
				return this._starLayer3SpriteAnimationSpeed;
			}
			set
			{
				this._starLayer3SpriteAnimationSpeed = value;
				this.SkyboxMaterial.SetFloat("_StarLayer3SpriteAnimationSpeed", this._starLayer3SpriteAnimationSpeed);
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x060009D3 RID: 2515 RVA: 0x0002BC1F File Offset: 0x00029E1F
		// (set) Token: 0x060009D4 RID: 2516 RVA: 0x0002BC27 File Offset: 0x00029E27
		public Texture MoonTexture
		{
			get
			{
				return this._moonTexture;
			}
			set
			{
				this._moonTexture = value;
				this.SkyboxMaterial.SetTexture("_MoonTex", this._moonTexture);
			}
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x060009D5 RID: 2517 RVA: 0x0002BC46 File Offset: 0x00029E46
		// (set) Token: 0x060009D6 RID: 2518 RVA: 0x0002BC4E File Offset: 0x00029E4E
		public float MoonRotationSpeed
		{
			get
			{
				return this._moonRotationSpeed;
			}
			set
			{
				this._moonRotationSpeed = value;
				this.SkyboxMaterial.SetFloat("_MoonRotationSpeed", this._moonRotationSpeed);
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x060009D7 RID: 2519 RVA: 0x0002BC6D File Offset: 0x00029E6D
		// (set) Token: 0x060009D8 RID: 2520 RVA: 0x0002BC75 File Offset: 0x00029E75
		public Color MoonColor
		{
			get
			{
				return this._moonColor;
			}
			set
			{
				this._moonColor = value;
				this.SkyboxMaterial.SetColor("_MoonColor", this._moonColor);
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x060009D9 RID: 2521 RVA: 0x0002BC94 File Offset: 0x00029E94
		// (set) Token: 0x060009DA RID: 2522 RVA: 0x0002BC9C File Offset: 0x00029E9C
		public Vector3 MoonDirection
		{
			get
			{
				return this._moonDirection;
			}
			set
			{
				this._moonDirection = value.normalized;
				this.SkyboxMaterial.SetVector("_MoonPosition", this._moonDirection);
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x060009DB RID: 2523 RVA: 0x0002BCC6 File Offset: 0x00029EC6
		// (set) Token: 0x060009DC RID: 2524 RVA: 0x0002BCCE File Offset: 0x00029ECE
		public Matrix4x4 MoonWorldToLocalMatrix
		{
			get
			{
				return this._moonWorldToLocalMatrix;
			}
			set
			{
				this._moonWorldToLocalMatrix = value;
				this.SkyboxMaterial.SetMatrix("_MoonWorldToLocalMat", this._moonWorldToLocalMatrix);
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x060009DD RID: 2525 RVA: 0x0002BCED File Offset: 0x00029EED
		// (set) Token: 0x060009DE RID: 2526 RVA: 0x0002BCF5 File Offset: 0x00029EF5
		public float MoonSize
		{
			get
			{
				return this._moonSize;
			}
			set
			{
				this._moonSize = value;
				this.SkyboxMaterial.SetFloat("_MoonRadius", this._moonSize);
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x060009DF RID: 2527 RVA: 0x0002BD14 File Offset: 0x00029F14
		// (set) Token: 0x060009E0 RID: 2528 RVA: 0x0002BD1C File Offset: 0x00029F1C
		public float MoonEdgeFeathering
		{
			get
			{
				return this._moonEdgeFeathering;
			}
			set
			{
				this._moonEdgeFeathering = value;
				this.SkyboxMaterial.SetFloat("_MoonEdgeFade", this._moonEdgeFeathering);
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x060009E1 RID: 2529 RVA: 0x0002BD3B File Offset: 0x00029F3B
		// (set) Token: 0x060009E2 RID: 2530 RVA: 0x0002BD43 File Offset: 0x00029F43
		public float MoonBloomFilterBoost
		{
			get
			{
				return this._moonBloomFilterBoost;
			}
			set
			{
				this._moonBloomFilterBoost = value;
				this.SkyboxMaterial.SetFloat("_MoonHDRBoost", this._moonBloomFilterBoost);
			}
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x0002BD62 File Offset: 0x00029F62
		public void SetMoonSpriteDimensions(int columns, int rows)
		{
			this._moonSpriteDimensions.x = (float)columns;
			this._moonSpriteDimensions.y = (float)rows;
			this.SkyboxMaterial.SetVector("_MoonSpriteDimensions", this._moonSpriteDimensions);
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x0002BD94 File Offset: 0x00029F94
		public Vector2 GetMoonSpriteDimensions()
		{
			return new Vector2(this._moonSpriteDimensions.x, this._moonSpriteDimensions.y);
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x060009E5 RID: 2533 RVA: 0x0002BDB1 File Offset: 0x00029FB1
		// (set) Token: 0x060009E6 RID: 2534 RVA: 0x0002BDB9 File Offset: 0x00029FB9
		public int MoonSpriteItemCount
		{
			get
			{
				return this._moonSpriteItemCount;
			}
			set
			{
				this._moonSpriteItemCount = value;
				this.SkyboxMaterial.SetInt("_MoonSpriteItemCount", this._moonSpriteItemCount);
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x060009E7 RID: 2535 RVA: 0x0002BDD8 File Offset: 0x00029FD8
		// (set) Token: 0x060009E8 RID: 2536 RVA: 0x0002BDE0 File Offset: 0x00029FE0
		public float MoonSpriteAnimationSpeed
		{
			get
			{
				return this._moonSpriteAnimationSpeed;
			}
			set
			{
				this._moonSpriteAnimationSpeed = value;
				this.SkyboxMaterial.SetFloat("_MoonSpriteAnimationSpeed", this._moonSpriteAnimationSpeed);
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x060009E9 RID: 2537 RVA: 0x0002BDFF File Offset: 0x00029FFF
		// (set) Token: 0x060009EA RID: 2538 RVA: 0x0002BE07 File Offset: 0x0002A007
		public float MoonAlpha
		{
			get
			{
				return this._moonAlpha;
			}
			set
			{
				this._moonAlpha = value;
				this.SkyboxMaterial.SetFloat("_MoonAlpha", this._moonAlpha);
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x060009EB RID: 2539 RVA: 0x0002BE26 File Offset: 0x0002A026
		// (set) Token: 0x060009EC RID: 2540 RVA: 0x0002BE2E File Offset: 0x0002A02E
		public Texture SunTexture
		{
			get
			{
				return this._sunTexture;
			}
			set
			{
				this._sunTexture = value;
				this.SkyboxMaterial.SetTexture("_SunTex", this._sunTexture);
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x060009ED RID: 2541 RVA: 0x0002BE4D File Offset: 0x0002A04D
		// (set) Token: 0x060009EE RID: 2542 RVA: 0x0002BE55 File Offset: 0x0002A055
		public Color SunColor
		{
			get
			{
				return this._sunColor;
			}
			set
			{
				this._sunColor = value;
				this.SkyboxMaterial.SetColor("_SunColor", this._sunColor);
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x060009EF RID: 2543 RVA: 0x0002BE74 File Offset: 0x0002A074
		// (set) Token: 0x060009F0 RID: 2544 RVA: 0x0002BE7C File Offset: 0x0002A07C
		public float SunRotationSpeed
		{
			get
			{
				return this._sunRotationSpeed;
			}
			set
			{
				this._sunRotationSpeed = value;
				this.SkyboxMaterial.SetFloat("_SunRotationSpeed", this._sunRotationSpeed);
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x060009F1 RID: 2545 RVA: 0x0002BE9B File Offset: 0x0002A09B
		// (set) Token: 0x060009F2 RID: 2546 RVA: 0x0002BEA3 File Offset: 0x0002A0A3
		public Vector3 SunDirection
		{
			get
			{
				return this._sunDirection;
			}
			set
			{
				this._sunDirection = value.normalized;
				this.SkyboxMaterial.SetVector("_SunPosition", this._sunDirection);
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x060009F3 RID: 2547 RVA: 0x0002BECD File Offset: 0x0002A0CD
		// (set) Token: 0x060009F4 RID: 2548 RVA: 0x0002BED5 File Offset: 0x0002A0D5
		public Matrix4x4 SunWorldToLocalMatrix
		{
			get
			{
				return this._sunWorldToLocalMatrix;
			}
			set
			{
				this._sunWorldToLocalMatrix = value;
				this.SkyboxMaterial.SetMatrix("_SunWorldToLocalMat", this._sunWorldToLocalMatrix);
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x060009F5 RID: 2549 RVA: 0x0002BEF4 File Offset: 0x0002A0F4
		// (set) Token: 0x060009F6 RID: 2550 RVA: 0x0002BEFC File Offset: 0x0002A0FC
		public float SunSize
		{
			get
			{
				return this._sunSize;
			}
			set
			{
				this._sunSize = value;
				this.SkyboxMaterial.SetFloat("_SunRadius", this._sunSize);
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x060009F7 RID: 2551 RVA: 0x0002BF1B File Offset: 0x0002A11B
		// (set) Token: 0x060009F8 RID: 2552 RVA: 0x0002BF23 File Offset: 0x0002A123
		public float SunEdgeFeathering
		{
			get
			{
				return this._sunEdgeFeathering;
			}
			set
			{
				this._sunEdgeFeathering = value;
				this.SkyboxMaterial.SetFloat("_SunEdgeFade", this._sunEdgeFeathering);
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x060009F9 RID: 2553 RVA: 0x0002BF42 File Offset: 0x0002A142
		// (set) Token: 0x060009FA RID: 2554 RVA: 0x0002BF4A File Offset: 0x0002A14A
		public float SunBloomFilterBoost
		{
			get
			{
				return this._sunBloomFilterBoost;
			}
			set
			{
				this._sunBloomFilterBoost = value;
				this.SkyboxMaterial.SetFloat("_SunHDRBoost", this._sunBloomFilterBoost);
			}
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x0002BF69 File Offset: 0x0002A169
		public void SetSunSpriteDimensions(int columns, int rows)
		{
			this._sunSpriteDimensions.x = (float)columns;
			this._sunSpriteDimensions.y = (float)rows;
			this.SkyboxMaterial.SetVector("_SunSpriteDimensions", this._sunSpriteDimensions);
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x0002BF9B File Offset: 0x0002A19B
		public Vector2 GetSunSpriteDimensions()
		{
			return new Vector2(this._sunSpriteDimensions.x, this._sunSpriteDimensions.y);
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x060009FD RID: 2557 RVA: 0x0002BFB8 File Offset: 0x0002A1B8
		// (set) Token: 0x060009FE RID: 2558 RVA: 0x0002BFC0 File Offset: 0x0002A1C0
		public int SunSpriteItemCount
		{
			get
			{
				return this._sunSpriteItemCount;
			}
			set
			{
				this._sunSpriteItemCount = value;
				this.SkyboxMaterial.SetInt("_SunSpriteItemCount", this._sunSpriteItemCount);
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x060009FF RID: 2559 RVA: 0x0002BFDF File Offset: 0x0002A1DF
		// (set) Token: 0x06000A00 RID: 2560 RVA: 0x0002BFE7 File Offset: 0x0002A1E7
		public float SunSpriteAnimationSpeed
		{
			get
			{
				return this._sunSpriteAnimationSpeed;
			}
			set
			{
				this._sunSpriteAnimationSpeed = value;
				this.SkyboxMaterial.SetFloat("_SunSpriteAnimationSpeed", this._sunSpriteAnimationSpeed);
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000A01 RID: 2561 RVA: 0x0002C006 File Offset: 0x0002A206
		// (set) Token: 0x06000A02 RID: 2562 RVA: 0x0002C00E File Offset: 0x0002A20E
		public float SunAlpha
		{
			get
			{
				return this._sunAlpha;
			}
			set
			{
				this._sunAlpha = value;
				this.SkyboxMaterial.SetFloat("_SunAlpha", this._sunAlpha);
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000A03 RID: 2563 RVA: 0x0002C02D File Offset: 0x0002A22D
		// (set) Token: 0x06000A04 RID: 2564 RVA: 0x0002C035 File Offset: 0x0002A235
		public float CloudBegin
		{
			get
			{
				return this._cloudBegin;
			}
			set
			{
				this._cloudBegin = value;
				this.SkyboxMaterial.SetFloat("_CloudBegin", this._cloudBegin);
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000A05 RID: 2565 RVA: 0x0002C054 File Offset: 0x0002A254
		// (set) Token: 0x06000A06 RID: 2566 RVA: 0x0002C05C File Offset: 0x0002A25C
		public float CloudTextureTiling
		{
			get
			{
				return this._cloudTextureTiling;
			}
			set
			{
				this._cloudTextureTiling = value;
				this.SkyboxMaterial.SetFloat("_CloudTextureTiling", this._cloudTextureTiling);
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000A07 RID: 2567 RVA: 0x0002C07B File Offset: 0x0002A27B
		// (set) Token: 0x06000A08 RID: 2568 RVA: 0x0002C083 File Offset: 0x0002A283
		public Color CloudColor
		{
			get
			{
				return this._cloudColor;
			}
			set
			{
				this._cloudColor = value;
				this.SkyboxMaterial.SetColor("_CloudColor", this._cloudColor);
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000A09 RID: 2569 RVA: 0x0002C0A2 File Offset: 0x0002A2A2
		// (set) Token: 0x06000A0A RID: 2570 RVA: 0x0002C0BE File Offset: 0x0002A2BE
		public Texture CloudTexture
		{
			get
			{
				if (!(this._cloudTexture != null))
				{
					return Texture2D.blackTexture;
				}
				return this._cloudTexture;
			}
			set
			{
				this._cloudTexture = value;
				this.SkyboxMaterial.SetTexture("_CloudNoiseTexture", this._cloudTexture);
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000A0B RID: 2571 RVA: 0x0002C0DD File Offset: 0x0002A2DD
		// (set) Token: 0x06000A0C RID: 2572 RVA: 0x0002C0F9 File Offset: 0x0002A2F9
		public Texture ArtCloudCustomTexture
		{
			get
			{
				if (!(this._artCloudCustomTexture != null))
				{
					return Texture2D.blackTexture;
				}
				return this._artCloudCustomTexture;
			}
			set
			{
				this._artCloudCustomTexture = value;
				this.SkyboxMaterial.SetTexture("_ArtCloudCustomTexture", this._artCloudCustomTexture);
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000A0D RID: 2573 RVA: 0x0002C118 File Offset: 0x0002A318
		// (set) Token: 0x06000A0E RID: 2574 RVA: 0x0002C120 File Offset: 0x0002A320
		public float CloudDensity
		{
			get
			{
				return this._cloudDensity;
			}
			set
			{
				this._cloudDensity = value;
				this.SkyboxMaterial.SetFloat("_CloudDensity", this._cloudDensity);
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000A0F RID: 2575 RVA: 0x0002C13F File Offset: 0x0002A33F
		// (set) Token: 0x06000A10 RID: 2576 RVA: 0x0002C147 File Offset: 0x0002A347
		public float CloudSpeed
		{
			get
			{
				return this._cloudSpeed;
			}
			set
			{
				this._cloudSpeed = value;
				this.SkyboxMaterial.SetFloat("_CloudSpeed", this._cloudSpeed);
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000A11 RID: 2577 RVA: 0x0002C166 File Offset: 0x0002A366
		// (set) Token: 0x06000A12 RID: 2578 RVA: 0x0002C16E File Offset: 0x0002A36E
		public float CloudDirection
		{
			get
			{
				return this._cloudDirection;
			}
			set
			{
				this._cloudDirection = value;
				this.SkyboxMaterial.SetFloat("_CloudDirection", this._cloudDirection);
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000A13 RID: 2579 RVA: 0x0002C18D File Offset: 0x0002A38D
		// (set) Token: 0x06000A14 RID: 2580 RVA: 0x0002C195 File Offset: 0x0002A395
		public float CloudHeight
		{
			get
			{
				return this._cloudHeight;
			}
			set
			{
				this._cloudHeight = value;
				this.SkyboxMaterial.SetFloat("_CloudHeight", this._cloudHeight);
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000A15 RID: 2581 RVA: 0x0002C1B4 File Offset: 0x0002A3B4
		// (set) Token: 0x06000A16 RID: 2582 RVA: 0x0002C1BC File Offset: 0x0002A3BC
		public Color CloudColor1
		{
			get
			{
				return this._cloudColor1;
			}
			set
			{
				this._cloudColor1 = value;
				this.SkyboxMaterial.SetColor("_CloudColor1", this._cloudColor1);
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000A17 RID: 2583 RVA: 0x0002C1DB File Offset: 0x0002A3DB
		// (set) Token: 0x06000A18 RID: 2584 RVA: 0x0002C1E3 File Offset: 0x0002A3E3
		public Color CloudColor2
		{
			get
			{
				return this._cloudColor2;
			}
			set
			{
				this._cloudColor2 = value;
				this.SkyboxMaterial.SetColor("_CloudColor2", this._cloudColor2);
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000A19 RID: 2585 RVA: 0x0002C202 File Offset: 0x0002A402
		// (set) Token: 0x06000A1A RID: 2586 RVA: 0x0002C20A File Offset: 0x0002A40A
		public float CloudFadePosition
		{
			get
			{
				return this._cloudFadePosition;
			}
			set
			{
				this._cloudFadePosition = value;
				this.SkyboxMaterial.SetFloat("_CloudFadePosition", this._cloudFadePosition);
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000A1B RID: 2587 RVA: 0x0002C229 File Offset: 0x0002A429
		// (set) Token: 0x06000A1C RID: 2588 RVA: 0x0002C231 File Offset: 0x0002A431
		public float CloudFadeAmount
		{
			get
			{
				return this._cloudFadeAmount;
			}
			set
			{
				this._cloudFadeAmount = value;
				this.SkyboxMaterial.SetFloat("_CloudFadeAmount", this._cloudFadeAmount);
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000A1D RID: 2589 RVA: 0x0002C250 File Offset: 0x0002A450
		// (set) Token: 0x06000A1E RID: 2590 RVA: 0x0002C258 File Offset: 0x0002A458
		public float CloudAlpha
		{
			get
			{
				return this._cloudAlpha;
			}
			set
			{
				this._cloudAlpha = value;
				this.SkyboxMaterial.SetFloat("_CloudAlpha", this._cloudAlpha);
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000A1F RID: 2591 RVA: 0x0002C277 File Offset: 0x0002A477
		// (set) Token: 0x06000A20 RID: 2592 RVA: 0x0002C27F File Offset: 0x0002A47F
		public Texture CloudCubemap
		{
			get
			{
				return this._cloudCubemap;
			}
			set
			{
				this._cloudCubemap = value;
				this.SkyboxMaterial.SetTexture("_CloudCubemapTexture", this._cloudCubemap);
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000A21 RID: 2593 RVA: 0x0002C29E File Offset: 0x0002A49E
		// (set) Token: 0x06000A22 RID: 2594 RVA: 0x0002C2A6 File Offset: 0x0002A4A6
		public float CloudCubemapRotationSpeed
		{
			get
			{
				return this._cloudCubemapRotationSpeed;
			}
			set
			{
				this._cloudCubemapRotationSpeed = value;
				this.SkyboxMaterial.SetFloat("_CloudCubemapRotationSpeed", this._cloudCubemapRotationSpeed);
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000A23 RID: 2595 RVA: 0x0002C2C5 File Offset: 0x0002A4C5
		// (set) Token: 0x06000A24 RID: 2596 RVA: 0x0002C2CD File Offset: 0x0002A4CD
		public Texture CloudCubemapDoubleLayerCustomTexture
		{
			get
			{
				return this._cloudCubemapDoubleLayerCustomTexture;
			}
			set
			{
				this._cloudCubemapDoubleLayerCustomTexture = value;
				this.SkyboxMaterial.SetTexture("_CloudCubemapDoubleTexture", this._cloudCubemapDoubleLayerCustomTexture);
			}
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000A25 RID: 2597 RVA: 0x0002C2EC File Offset: 0x0002A4EC
		// (set) Token: 0x06000A26 RID: 2598 RVA: 0x0002C2F4 File Offset: 0x0002A4F4
		public float CloudCubemapDoubleLayerRotationSpeed
		{
			get
			{
				return this._cloudCubemapDoubleLayerRotationSpeed;
			}
			set
			{
				this._cloudCubemapDoubleLayerRotationSpeed = value;
				this.SkyboxMaterial.SetFloat("_CloudCubemapDoubleLayerRotationSpeed", this._cloudCubemapDoubleLayerRotationSpeed);
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000A27 RID: 2599 RVA: 0x0002C313 File Offset: 0x0002A513
		// (set) Token: 0x06000A28 RID: 2600 RVA: 0x0002C31B File Offset: 0x0002A51B
		public float CloudCubemapDoubleLayerHeight
		{
			get
			{
				return this._cloudCubemapDoubleLayerHeight;
			}
			set
			{
				this._cloudCubemapDoubleLayerHeight = value;
				this.SkyboxMaterial.SetFloat("_CloudCubemapDoubleLayerHeight", this._cloudCubemapDoubleLayerHeight);
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000A29 RID: 2601 RVA: 0x0002C33A File Offset: 0x0002A53A
		// (set) Token: 0x06000A2A RID: 2602 RVA: 0x0002C342 File Offset: 0x0002A542
		public Color CloudCubemapDoubleLayerTintColor
		{
			get
			{
				return this._cloudCubemapDoubleLayerTintColor;
			}
			set
			{
				this._cloudCubemapDoubleLayerTintColor = value;
				this.SkyboxMaterial.SetColor("_CloudCubemapDoubleLayerTintColor", this._cloudCubemapDoubleLayerTintColor);
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000A2B RID: 2603 RVA: 0x0002C361 File Offset: 0x0002A561
		// (set) Token: 0x06000A2C RID: 2604 RVA: 0x0002C369 File Offset: 0x0002A569
		public Color CloudCubemapTintColor
		{
			get
			{
				return this._cloudCubemapTintColor;
			}
			set
			{
				this._cloudCubemapTintColor = value;
				this.SkyboxMaterial.SetColor("_CloudCubemapTintColor", this._cloudCubemapTintColor);
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000A2D RID: 2605 RVA: 0x0002C388 File Offset: 0x0002A588
		// (set) Token: 0x06000A2E RID: 2606 RVA: 0x0002C390 File Offset: 0x0002A590
		public float CloudCubemapHeight
		{
			get
			{
				return this._cloudCubemapHeight;
			}
			set
			{
				this._cloudCubemapHeight = value;
				this.SkyboxMaterial.SetFloat("_CloudCubemapHeight", this._cloudCubemapHeight);
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000A2F RID: 2607 RVA: 0x0002C277 File Offset: 0x0002A477
		// (set) Token: 0x06000A30 RID: 2608 RVA: 0x0002C3AF File Offset: 0x0002A5AF
		public Texture CloudCubemapNormalTexture
		{
			get
			{
				return this._cloudCubemap;
			}
			set
			{
				this._cloudCubemapNormalTexture = value;
				this.SkyboxMaterial.SetTexture("_CloudCubemapNormalTexture", this._cloudCubemapNormalTexture);
			}
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000A31 RID: 2609 RVA: 0x0002C3CE File Offset: 0x0002A5CE
		// (set) Token: 0x06000A32 RID: 2610 RVA: 0x0002C3D6 File Offset: 0x0002A5D6
		public Color CloudCubemapNormalLitColor
		{
			get
			{
				return this._cloudCubemapNormalLitColor;
			}
			set
			{
				this._cloudCubemapNormalLitColor = value;
				this.SkyboxMaterial.SetColor("_CloudCubemapNormalLitColor", this._cloudCubemapNormalLitColor);
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000A33 RID: 2611 RVA: 0x0002C3F5 File Offset: 0x0002A5F5
		// (set) Token: 0x06000A34 RID: 2612 RVA: 0x0002C3FD File Offset: 0x0002A5FD
		public Color CloudCubemapNormalShadowColor
		{
			get
			{
				return this._cloudCubemapNormalShadowColor;
			}
			set
			{
				this._cloudCubemapNormalShadowColor = value;
				this.SkyboxMaterial.SetColor("_CloudCubemapNormalShadowColor", this._cloudCubemapNormalShadowColor);
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000A35 RID: 2613 RVA: 0x0002C41C File Offset: 0x0002A61C
		// (set) Token: 0x06000A36 RID: 2614 RVA: 0x0002C424 File Offset: 0x0002A624
		public float CloudCubemapNormalRotationSpeed
		{
			get
			{
				return this._cloudCubemapNormalRotationSpeed;
			}
			set
			{
				this._cloudCubemapNormalRotationSpeed = value;
				this.SkyboxMaterial.SetFloat("_CloudCubemapNormalRotationSpeed", this._cloudCubemapNormalRotationSpeed);
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000A37 RID: 2615 RVA: 0x0002C443 File Offset: 0x0002A643
		// (set) Token: 0x06000A38 RID: 2616 RVA: 0x0002C44B File Offset: 0x0002A64B
		public float CloudCubemapNormalHeight
		{
			get
			{
				return this._cloudCubemapNormalHeight;
			}
			set
			{
				this._cloudCubemapNormalHeight = value;
				this.SkyboxMaterial.SetFloat("_CloudCubemapNormalHeight", this._cloudCubemapNormalHeight);
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000A39 RID: 2617 RVA: 0x0002C46A File Offset: 0x0002A66A
		// (set) Token: 0x06000A3A RID: 2618 RVA: 0x0002C472 File Offset: 0x0002A672
		public float CloudCubemapNormalAmbientIntensity
		{
			get
			{
				return this._cloudCubemapNormalAmbientItensity;
			}
			set
			{
				this._cloudCubemapNormalAmbientItensity = value;
				this.SkyboxMaterial.SetFloat("_CloudCubemapNormalAmbientIntensity", this._cloudCubemapNormalAmbientItensity);
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000A3B RID: 2619 RVA: 0x0002C491 File Offset: 0x0002A691
		// (set) Token: 0x06000A3C RID: 2620 RVA: 0x0002C499 File Offset: 0x0002A699
		public Texture CloudCubemapNormalDoubleLayerCustomTexture
		{
			get
			{
				return this._cloudCubemapNormalDoubleLayerCustomTexture;
			}
			set
			{
				this._cloudCubemapNormalDoubleLayerCustomTexture = value;
				this.SkyboxMaterial.SetTexture("_CloudCubemapNormalDoubleTexture", this._cloudCubemapNormalDoubleLayerCustomTexture);
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000A3D RID: 2621 RVA: 0x0002C4B8 File Offset: 0x0002A6B8
		// (set) Token: 0x06000A3E RID: 2622 RVA: 0x0002C4C0 File Offset: 0x0002A6C0
		public float CloudCubemapNormalDoubleLayerRotationSpeed
		{
			get
			{
				return this._cloudCubemapNormalDoubleLayerRotationSpeed;
			}
			set
			{
				this._cloudCubemapNormalDoubleLayerRotationSpeed = value;
				this.SkyboxMaterial.SetFloat("_CloudCubemapNormalDoubleLayerRotationSpeed", this._cloudCubemapNormalDoubleLayerRotationSpeed);
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000A3F RID: 2623 RVA: 0x0002C313 File Offset: 0x0002A513
		// (set) Token: 0x06000A40 RID: 2624 RVA: 0x0002C4DF File Offset: 0x0002A6DF
		public float CloudCubemapNormalDoubleLayerHeight
		{
			get
			{
				return this._cloudCubemapDoubleLayerHeight;
			}
			set
			{
				this._cloudCubemapNormalDoubleLayerHeight = value;
				this.SkyboxMaterial.SetFloat("_CloudCubemapNormalDoubleLayerHeight", this._cloudCubemapNormalDoubleLayerHeight);
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000A41 RID: 2625 RVA: 0x0002C4FE File Offset: 0x0002A6FE
		// (set) Token: 0x06000A42 RID: 2626 RVA: 0x0002C506 File Offset: 0x0002A706
		public Color CloudCubemapNormalDoubleLayerLitColor
		{
			get
			{
				return this._cloudCubemapNormalDoubleLayerLitColor;
			}
			set
			{
				this._cloudCubemapNormalDoubleLayerLitColor = value;
				this.SkyboxMaterial.SetColor("_CloudCubemapNormalDoubleLitColor", this._cloudCubemapNormalDoubleLayerLitColor);
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000A43 RID: 2627 RVA: 0x0002C525 File Offset: 0x0002A725
		// (set) Token: 0x06000A44 RID: 2628 RVA: 0x0002C52D File Offset: 0x0002A72D
		public Color CloudCubemapNormalDoubleLayerShadowColor
		{
			get
			{
				return this._cloudCubemapNormalDoubleLayerShadowColor;
			}
			set
			{
				this._cloudCubemapNormalDoubleLayerShadowColor = value;
				this.SkyboxMaterial.SetColor("_CloudCubemapNormalDoubleShadowColor", this._cloudCubemapNormalDoubleLayerShadowColor);
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000A45 RID: 2629 RVA: 0x0002C54C File Offset: 0x0002A74C
		// (set) Token: 0x06000A46 RID: 2630 RVA: 0x0002C554 File Offset: 0x0002A754
		public Vector3 CloudCubemapNormalLightDirection
		{
			get
			{
				return this._cloudCubemapNormalLightDirection;
			}
			set
			{
				this._cloudCubemapNormalLightDirection = value;
				this.SkyboxMaterial.SetVector("_CloudCubemapNormalToLight", this._cloudCubemapNormalLightDirection);
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000A47 RID: 2631 RVA: 0x0002C578 File Offset: 0x0002A778
		// (set) Token: 0x06000A48 RID: 2632 RVA: 0x0002C580 File Offset: 0x0002A780
		public Color FogColor
		{
			get
			{
				return this._fogColor;
			}
			set
			{
				this._fogColor = value;
				this.SkyboxMaterial.SetColor("_HorizonFogColor", this._fogColor);
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000A49 RID: 2633 RVA: 0x0002C59F File Offset: 0x0002A79F
		// (set) Token: 0x06000A4A RID: 2634 RVA: 0x0002C5A7 File Offset: 0x0002A7A7
		public float FogDensity
		{
			get
			{
				return this._fogDensity;
			}
			set
			{
				this._fogDensity = value;
				this.SkyboxMaterial.SetFloat("_HorizonFogDensity", this._fogDensity);
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000A4B RID: 2635 RVA: 0x0002C5C6 File Offset: 0x0002A7C6
		// (set) Token: 0x06000A4C RID: 2636 RVA: 0x0002C5CE File Offset: 0x0002A7CE
		public float FogHeight
		{
			get
			{
				return this._fogHeight;
			}
			set
			{
				this._fogHeight = value;
				this.SkyboxMaterial.SetFloat("_HorizonFogLength", this._fogHeight);
			}
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x0002C5F0 File Offset: 0x0002A7F0
		private void ApplyGradientValuesOnMaterial()
		{
			float value = Mathf.Clamp(this._gradientFadeBegin + this._gradientFadeLength, -1f, 1f);
			this.SkyboxMaterial.SetFloat("_GradientFadeBegin", this._gradientFadeBegin);
			this.SkyboxMaterial.SetFloat("_GradientFadeEnd", value);
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x0002C644 File Offset: 0x0002A844
		private void ApplyStarFadeValuesOnMaterial()
		{
			float value = Mathf.Clamp(this._starFadeBegin + this._starFadeLength, -1f, 1f);
			this.SkyboxMaterial.SetFloat("_StarFadeBegin", this._starFadeBegin);
			this.SkyboxMaterial.SetFloat("_StarFadeEnd", value);
		}

		// Token: 0x04000AD6 RID: 2774
		[SerializeField]
		private Material _skyboxMaterial;

		// Token: 0x04000AD7 RID: 2775
		[SerializeField]
		private Color _skyColor = ColorHelper.ColorWithHex(2892384U);

		// Token: 0x04000AD8 RID: 2776
		[SerializeField]
		private Color _skyMiddleColor = Color.white;

		// Token: 0x04000AD9 RID: 2777
		[SerializeField]
		private Color _horizonColor = ColorHelper.ColorWithHex(14928002U);

		// Token: 0x04000ADA RID: 2778
		[SerializeField]
		[Range(-1f, 1f)]
		private float _gradientFadeBegin;

		// Token: 0x04000ADB RID: 2779
		[SerializeField]
		[Range(0f, 2f)]
		private float _gradientFadeLength = 1f;

		// Token: 0x04000ADC RID: 2780
		[SerializeField]
		[Range(0f, 1f)]
		private float _skyMiddlePosition = 0.5f;

		// Token: 0x04000ADD RID: 2781
		[SerializeField]
		private Cubemap _backgroundCubemap;

		// Token: 0x04000ADE RID: 2782
		[SerializeField]
		[Range(-1f, 1f)]
		private float _starFadeBegin = 0.067f;

		// Token: 0x04000ADF RID: 2783
		[SerializeField]
		[Range(0f, 2f)]
		private float _starFadeLength = 0.36f;

		// Token: 0x04000AE0 RID: 2784
		[SerializeField]
		[Range(0f, 1f)]
		private float _horizonDistanceScale = 0.7f;

		// Token: 0x04000AE1 RID: 2785
		[SerializeField]
		private Texture _starBasicCubemap;

		// Token: 0x04000AE2 RID: 2786
		[SerializeField]
		private float _starBasicTwinkleSpeed;

		// Token: 0x04000AE3 RID: 2787
		[SerializeField]
		private float _starBasicTwinkleAmount;

		// Token: 0x04000AE4 RID: 2788
		[SerializeField]
		private float _starBasicOpacity;

		// Token: 0x04000AE5 RID: 2789
		[SerializeField]
		private Color _starBasicTintColor;

		// Token: 0x04000AE6 RID: 2790
		[SerializeField]
		private float _starBasicExponent;

		// Token: 0x04000AE7 RID: 2791
		[SerializeField]
		private float _starBasicIntensity;

		// Token: 0x04000AE8 RID: 2792
		[SerializeField]
		private Texture _starLayer1Texture;

		// Token: 0x04000AE9 RID: 2793
		[SerializeField]
		private Texture2D _starLayer1DataTexture;

		// Token: 0x04000AEA RID: 2794
		[SerializeField]
		private Color _starLayer1Color;

		// Token: 0x04000AEB RID: 2795
		[SerializeField]
		[Range(0f, 0.1f)]
		private float _starLayer1MaxRadius = 0.007f;

		// Token: 0x04000AEC RID: 2796
		[SerializeField]
		[Range(0f, 1f)]
		private float _starLayer1TwinkleAmount = 0.7f;

		// Token: 0x04000AED RID: 2797
		[SerializeField]
		[Range(0f, 10f)]
		private float _starLayer1TwinkleSpeed = 0.7f;

		// Token: 0x04000AEE RID: 2798
		[SerializeField]
		[Range(0f, 10f)]
		private float _starLayer1RotationSpeed = 0.7f;

		// Token: 0x04000AEF RID: 2799
		[SerializeField]
		[Range(0.0001f, 0.9999f)]
		private float _starLayer1EdgeFeathering = 0.2f;

		// Token: 0x04000AF0 RID: 2800
		[SerializeField]
		[Range(1f, 10f)]
		private float _starLayer1BloomFilterBoost;

		// Token: 0x04000AF1 RID: 2801
		[SerializeField]
		private Vector4 _starLayer1SpriteDimensions = Vector4.zero;

		// Token: 0x04000AF2 RID: 2802
		[SerializeField]
		private int _starLayer1SpriteItemCount = 1;

		// Token: 0x04000AF3 RID: 2803
		[SerializeField]
		[Range(0f, 1f)]
		private float _starLayer1SpriteAnimationSpeed = 1f;

		// Token: 0x04000AF4 RID: 2804
		[SerializeField]
		private Texture _starLayer2Texture;

		// Token: 0x04000AF5 RID: 2805
		[SerializeField]
		private Texture2D _starLayer2DataTexture;

		// Token: 0x04000AF6 RID: 2806
		[SerializeField]
		private Color _starLayer2Color;

		// Token: 0x04000AF7 RID: 2807
		[SerializeField]
		[Range(0f, 0.1f)]
		private float _starLayer2MaxRadius = 0.007f;

		// Token: 0x04000AF8 RID: 2808
		[SerializeField]
		[Range(0f, 1f)]
		private float _starLayer2TwinkleAmount = 0.7f;

		// Token: 0x04000AF9 RID: 2809
		[SerializeField]
		[Range(0f, 10f)]
		private float _starLayer2TwinkleSpeed = 0.7f;

		// Token: 0x04000AFA RID: 2810
		[SerializeField]
		[Range(0f, 10f)]
		private float _starLayer2RotationSpeed = 0.7f;

		// Token: 0x04000AFB RID: 2811
		[SerializeField]
		[Range(0.0001f, 0.9999f)]
		private float _starLayer2EdgeFeathering = 0.2f;

		// Token: 0x04000AFC RID: 2812
		[SerializeField]
		[Range(1f, 10f)]
		private float _starLayer2BloomFilterBoost;

		// Token: 0x04000AFD RID: 2813
		[SerializeField]
		private Vector4 _starLayer2SpriteDimensions = Vector4.zero;

		// Token: 0x04000AFE RID: 2814
		[SerializeField]
		private int _starLayer2SpriteItemCount = 1;

		// Token: 0x04000AFF RID: 2815
		[SerializeField]
		[Range(0f, 1f)]
		private float _starLayer2SpriteAnimationSpeed = 1f;

		// Token: 0x04000B00 RID: 2816
		[SerializeField]
		private Texture _starLayer3Texture;

		// Token: 0x04000B01 RID: 2817
		[SerializeField]
		private Texture2D _starLayer3DataTexture;

		// Token: 0x04000B02 RID: 2818
		[SerializeField]
		private Color _starLayer3Color;

		// Token: 0x04000B03 RID: 2819
		[SerializeField]
		[Range(0f, 0.1f)]
		private float _starLayer3MaxRadius = 0.007f;

		// Token: 0x04000B04 RID: 2820
		[SerializeField]
		[Range(0f, 1f)]
		private float _starLayer3TwinkleAmount = 0.7f;

		// Token: 0x04000B05 RID: 2821
		[SerializeField]
		[Range(0f, 10f)]
		private float _starLayer3TwinkleSpeed = 0.7f;

		// Token: 0x04000B06 RID: 2822
		[SerializeField]
		[Range(0f, 10f)]
		private float _starLayer3RotationSpeed = 0.7f;

		// Token: 0x04000B07 RID: 2823
		[SerializeField]
		[Range(0.0001f, 0.9999f)]
		private float _starLayer3EdgeFeathering = 0.2f;

		// Token: 0x04000B08 RID: 2824
		[SerializeField]
		[Range(1f, 10f)]
		private float _starLayer3BloomFilterBoost;

		// Token: 0x04000B09 RID: 2825
		[SerializeField]
		private Vector4 _starLayer3SpriteDimensions = Vector4.zero;

		// Token: 0x04000B0A RID: 2826
		[SerializeField]
		private int _starLayer3SpriteItemCount = 1;

		// Token: 0x04000B0B RID: 2827
		[SerializeField]
		[Range(0f, 1f)]
		private float _starLayer3SpriteAnimationSpeed = 1f;

		// Token: 0x04000B0C RID: 2828
		[SerializeField]
		private Texture _moonTexture;

		// Token: 0x04000B0D RID: 2829
		[SerializeField]
		private float _moonRotationSpeed;

		// Token: 0x04000B0E RID: 2830
		[SerializeField]
		private Color _moonColor = Color.white;

		// Token: 0x04000B0F RID: 2831
		[SerializeField]
		private Vector3 _moonDirection = Vector3.right;

		// Token: 0x04000B10 RID: 2832
		[SerializeField]
		private Matrix4x4 _moonWorldToLocalMatrix = Matrix4x4.identity;

		// Token: 0x04000B11 RID: 2833
		[SerializeField]
		[Range(0f, 1f)]
		private float _moonSize = 0.1f;

		// Token: 0x04000B12 RID: 2834
		[SerializeField]
		[Range(0.0001f, 0.9999f)]
		private float _moonEdgeFeathering = 0.085f;

		// Token: 0x04000B13 RID: 2835
		[SerializeField]
		[Range(1f, 10f)]
		private float _moonBloomFilterBoost = 1f;

		// Token: 0x04000B14 RID: 2836
		[SerializeField]
		private Vector4 _moonSpriteDimensions = Vector4.zero;

		// Token: 0x04000B15 RID: 2837
		[SerializeField]
		private int _moonSpriteItemCount = 1;

		// Token: 0x04000B16 RID: 2838
		[SerializeField]
		[Range(0f, 1f)]
		private float _moonSpriteAnimationSpeed = 1f;

		// Token: 0x04000B17 RID: 2839
		[SerializeField]
		[Range(0f, 1f)]
		private float _moonAlpha = 1f;

		// Token: 0x04000B18 RID: 2840
		[SerializeField]
		private Texture _sunTexture;

		// Token: 0x04000B19 RID: 2841
		[SerializeField]
		private Color _sunColor = Color.white;

		// Token: 0x04000B1A RID: 2842
		[SerializeField]
		private float _sunRotationSpeed;

		// Token: 0x04000B1B RID: 2843
		[SerializeField]
		private Vector3 _sunDirection = Vector3.right;

		// Token: 0x04000B1C RID: 2844
		[SerializeField]
		private Matrix4x4 _sunWorldToLocalMatrix = Matrix4x4.identity;

		// Token: 0x04000B1D RID: 2845
		[SerializeField]
		[Range(0f, 1f)]
		private float _sunSize = 0.1f;

		// Token: 0x04000B1E RID: 2846
		[SerializeField]
		[Range(0.0001f, 0.9999f)]
		private float _sunEdgeFeathering = 0.085f;

		// Token: 0x04000B1F RID: 2847
		[SerializeField]
		[Range(1f, 10f)]
		private float _sunBloomFilterBoost = 1f;

		// Token: 0x04000B20 RID: 2848
		[SerializeField]
		private Vector4 _sunSpriteDimensions = Vector4.zero;

		// Token: 0x04000B21 RID: 2849
		[SerializeField]
		private int _sunSpriteItemCount = 1;

		// Token: 0x04000B22 RID: 2850
		[SerializeField]
		[Range(0f, 1f)]
		private float _sunSpriteAnimationSpeed = 1f;

		// Token: 0x04000B23 RID: 2851
		[SerializeField]
		[Range(0f, 1f)]
		private float _sunAlpha = 1f;

		// Token: 0x04000B24 RID: 2852
		[SerializeField]
		[Range(-1f, 1f)]
		private float _cloudBegin = 0.2f;

		// Token: 0x04000B25 RID: 2853
		private float _cloudTextureTiling;

		// Token: 0x04000B26 RID: 2854
		[SerializeField]
		private Color _cloudColor = Color.white;

		// Token: 0x04000B27 RID: 2855
		[SerializeField]
		private Texture _cloudTexture;

		// Token: 0x04000B28 RID: 2856
		[SerializeField]
		private Texture _artCloudCustomTexture;

		// Token: 0x04000B29 RID: 2857
		[SerializeField]
		private float _cloudDensity;

		// Token: 0x04000B2A RID: 2858
		[SerializeField]
		private float _cloudSpeed;

		// Token: 0x04000B2B RID: 2859
		[SerializeField]
		private float _cloudDirection;

		// Token: 0x04000B2C RID: 2860
		[SerializeField]
		private float _cloudHeight;

		// Token: 0x04000B2D RID: 2861
		[SerializeField]
		private Color _cloudColor1 = Color.white;

		// Token: 0x04000B2E RID: 2862
		[SerializeField]
		private Color _cloudColor2 = Color.white;

		// Token: 0x04000B2F RID: 2863
		[SerializeField]
		private float _cloudFadePosition;

		// Token: 0x04000B30 RID: 2864
		[SerializeField]
		private float _cloudFadeAmount = 0.5f;

		// Token: 0x04000B31 RID: 2865
		[SerializeField]
		private float _cloudAlpha = 1f;

		// Token: 0x04000B32 RID: 2866
		[SerializeField]
		private Texture _cloudCubemap;

		// Token: 0x04000B33 RID: 2867
		[SerializeField]
		private float _cloudCubemapRotationSpeed;

		// Token: 0x04000B34 RID: 2868
		[SerializeField]
		private Texture _cloudCubemapDoubleLayerCustomTexture;

		// Token: 0x04000B35 RID: 2869
		[SerializeField]
		private float _cloudCubemapDoubleLayerRotationSpeed;

		// Token: 0x04000B36 RID: 2870
		[SerializeField]
		private float _cloudCubemapDoubleLayerHeight;

		// Token: 0x04000B37 RID: 2871
		[SerializeField]
		private Color _cloudCubemapDoubleLayerTintColor = Color.white;

		// Token: 0x04000B38 RID: 2872
		[SerializeField]
		private Color _cloudCubemapTintColor = Color.white;

		// Token: 0x04000B39 RID: 2873
		[SerializeField]
		private float _cloudCubemapHeight;

		// Token: 0x04000B3A RID: 2874
		[SerializeField]
		private Texture _cloudCubemapNormalTexture;

		// Token: 0x04000B3B RID: 2875
		[SerializeField]
		private Color _cloudCubemapNormalLitColor = Color.white;

		// Token: 0x04000B3C RID: 2876
		[SerializeField]
		private Color _cloudCubemapNormalShadowColor = Color.gray;

		// Token: 0x04000B3D RID: 2877
		[SerializeField]
		private float _cloudCubemapNormalRotationSpeed;

		// Token: 0x04000B3E RID: 2878
		[SerializeField]
		private float _cloudCubemapNormalHeight;

		// Token: 0x04000B3F RID: 2879
		[SerializeField]
		private float _cloudCubemapNormalAmbientItensity;

		// Token: 0x04000B40 RID: 2880
		[SerializeField]
		private Texture _cloudCubemapNormalDoubleLayerCustomTexture;

		// Token: 0x04000B41 RID: 2881
		[SerializeField]
		private float _cloudCubemapNormalDoubleLayerRotationSpeed;

		// Token: 0x04000B42 RID: 2882
		[SerializeField]
		private float _cloudCubemapNormalDoubleLayerHeight;

		// Token: 0x04000B43 RID: 2883
		[SerializeField]
		private Color _cloudCubemapNormalDoubleLayerLitColor = Color.white;

		// Token: 0x04000B44 RID: 2884
		[SerializeField]
		private Color _cloudCubemapNormalDoubleLayerShadowColor = Color.gray;

		// Token: 0x04000B45 RID: 2885
		[SerializeField]
		private Vector3 _cloudCubemapNormalLightDirection = new Vector3(0f, 1f, 0f);

		// Token: 0x04000B46 RID: 2886
		[SerializeField]
		private Color _fogColor = Color.white;

		// Token: 0x04000B47 RID: 2887
		[SerializeField]
		private float _fogDensity = 0.12f;

		// Token: 0x04000B48 RID: 2888
		[SerializeField]
		private float _fogHeight = 0.12f;
	}
}
