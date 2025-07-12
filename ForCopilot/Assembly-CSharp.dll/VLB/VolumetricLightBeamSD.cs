using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace VLB
{
	// Token: 0x0200014C RID: 332
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[SelectionBase]
	[HelpURL("http://saladgamer.com/vlb-doc/comp-lightbeam-sd/")]
	public class VolumetricLightBeamSD : VolumetricLightBeamAbstractBase
	{
		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000603 RID: 1539 RVA: 0x0001C1C3 File Offset: 0x0001A3C3
		public ColorMode usedColorMode
		{
			get
			{
				if (Config.Instance.featureEnabledColorGradient == FeatureEnabledColorGradient.Off)
				{
					return ColorMode.Flat;
				}
				return this.colorMode;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000604 RID: 1540 RVA: 0x0001C1D9 File Offset: 0x0001A3D9
		private bool useColorFromAttachedLightSpot
		{
			get
			{
				return this.colorFromLight && base.lightSpotAttached != null;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000605 RID: 1541 RVA: 0x0001C1F1 File Offset: 0x0001A3F1
		private bool useColorTemperatureFromAttachedLightSpot
		{
			get
			{
				return this.useColorFromAttachedLightSpot && base.lightSpotAttached.useColorTemperature && Config.Instance.useLightColorTemperature;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000606 RID: 1542 RVA: 0x0001C214 File Offset: 0x0001A414
		// (set) Token: 0x06000607 RID: 1543 RVA: 0x0001C21C File Offset: 0x0001A41C
		[Obsolete("Use 'intensityGlobal' or 'intensityInside' instead")]
		public float alphaInside
		{
			get
			{
				return this.intensityInside;
			}
			set
			{
				this.intensityInside = value;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000608 RID: 1544 RVA: 0x0001C225 File Offset: 0x0001A425
		// (set) Token: 0x06000609 RID: 1545 RVA: 0x0001C22D File Offset: 0x0001A42D
		[Obsolete("Use 'intensityGlobal' or 'intensityOutside' instead")]
		public float alphaOutside
		{
			get
			{
				return this.intensityOutside;
			}
			set
			{
				this.intensityOutside = value;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x0600060A RID: 1546 RVA: 0x0001C225 File Offset: 0x0001A425
		// (set) Token: 0x0600060B RID: 1547 RVA: 0x0001C236 File Offset: 0x0001A436
		public float intensityGlobal
		{
			get
			{
				return this.intensityOutside;
			}
			set
			{
				this.intensityInside = value;
				this.intensityOutside = value;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x0600060C RID: 1548 RVA: 0x0001C246 File Offset: 0x0001A446
		public bool useIntensityFromAttachedLightSpot
		{
			get
			{
				return this.intensityFromLight && base.lightSpotAttached != null;
			}
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x0001C260 File Offset: 0x0001A460
		public void GetInsideAndOutsideIntensity(out float inside, out float outside)
		{
			if (this.intensityModeAdvanced)
			{
				inside = this.intensityInside;
				outside = this.intensityOutside;
				return;
			}
			inside = (outside = this.intensityOutside);
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x0600060E RID: 1550 RVA: 0x0001C293 File Offset: 0x0001A493
		public bool useSpotAngleFromAttachedLightSpot
		{
			get
			{
				return this.spotAngleFromLight && base.lightSpotAttached != null;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x0600060F RID: 1551 RVA: 0x0001C2AB File Offset: 0x0001A4AB
		public float coneAngle
		{
			get
			{
				return Mathf.Atan2(this.coneRadiusEnd - this.coneRadiusStart, this.maxGeometryDistance) * 57.29578f * 2f;
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000610 RID: 1552 RVA: 0x0001C2D1 File Offset: 0x0001A4D1
		// (set) Token: 0x06000611 RID: 1553 RVA: 0x0001C2E4 File Offset: 0x0001A4E4
		public float coneRadiusEnd
		{
			get
			{
				return Utils.ComputeConeRadiusEnd(this.maxGeometryDistance, this.spotAngle);
			}
			set
			{
				this.spotAngle = Utils.ComputeSpotAngle(this.maxGeometryDistance, value);
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000612 RID: 1554 RVA: 0x0001C2F8 File Offset: 0x0001A4F8
		public float coneVolume
		{
			get
			{
				float num = this.coneRadiusStart;
				float coneRadiusEnd = this.coneRadiusEnd;
				return 1.0471976f * (num * num + num * coneRadiusEnd + coneRadiusEnd * coneRadiusEnd) * this.fallOffEnd;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000613 RID: 1555 RVA: 0x0001C32C File Offset: 0x0001A52C
		public float coneApexOffsetZ
		{
			get
			{
				float num = this.coneRadiusStart / this.coneRadiusEnd;
				if (num != 1f)
				{
					return this.maxGeometryDistance * num / (1f - num);
				}
				return float.MaxValue;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000614 RID: 1556 RVA: 0x0001C365 File Offset: 0x0001A565
		public Vector3 coneApexPositionLocal
		{
			get
			{
				return new Vector3(0f, 0f, -this.coneApexOffsetZ);
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000615 RID: 1557 RVA: 0x0001C380 File Offset: 0x0001A580
		public Vector3 coneApexPositionGlobal
		{
			get
			{
				return base.transform.localToWorldMatrix.MultiplyPoint(this.coneApexPositionLocal);
			}
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x000022C9 File Offset: 0x000004C9
		public override bool IsScalable()
		{
			return true;
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000617 RID: 1559 RVA: 0x0001C3A6 File Offset: 0x0001A5A6
		// (set) Token: 0x06000618 RID: 1560 RVA: 0x0001C3C2 File Offset: 0x0001A5C2
		public int geomSides
		{
			get
			{
				if (this.geomMeshType != MeshType.Custom)
				{
					return Config.Instance.sharedMeshSides;
				}
				return this.geomCustomSides;
			}
			set
			{
				this.geomCustomSides = value;
				Debug.LogWarningFormat("The setter VLB.{0}.geomSides is OBSOLETE and has been renamed to geomCustomSides.", new object[]
				{
					"VolumetricLightBeamSD"
				});
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000619 RID: 1561 RVA: 0x0001C3E3 File Offset: 0x0001A5E3
		// (set) Token: 0x0600061A RID: 1562 RVA: 0x0001C3FF File Offset: 0x0001A5FF
		public int geomSegments
		{
			get
			{
				if (this.geomMeshType != MeshType.Custom)
				{
					return Config.Instance.sharedMeshSegments;
				}
				return this.geomCustomSegments;
			}
			set
			{
				this.geomCustomSegments = value;
				Debug.LogWarningFormat("The setter VLB.{0}.geomSegments is OBSOLETE and has been renamed to geomCustomSegments.", new object[]
				{
					"VolumetricLightBeamSD"
				});
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x0600061B RID: 1563 RVA: 0x0001C420 File Offset: 0x0001A620
		public Vector3 skewingLocalForwardDirectionNormalized
		{
			get
			{
				if (Mathf.Approximately(this.skewingLocalForwardDirection.z, 0f))
				{
					Debug.LogErrorFormat("Beam {0} has a skewingLocalForwardDirection with a null Z, which is forbidden", new object[]
					{
						base.name
					});
					return Vector3.forward;
				}
				return this.skewingLocalForwardDirection.normalized;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x0600061C RID: 1564 RVA: 0x0001C46E File Offset: 0x0001A66E
		public bool canHaveMeshSkewing
		{
			get
			{
				return this.geomMeshType == MeshType.Custom;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x0600061D RID: 1565 RVA: 0x0001C479 File Offset: 0x0001A679
		public bool hasMeshSkewing
		{
			get
			{
				return Config.Instance.featureEnabledMeshSkewing && this.canHaveMeshSkewing && !Mathf.Approximately(Vector3.Dot(this.skewingLocalForwardDirectionNormalized, Vector3.forward), 1f);
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x0600061E RID: 1566 RVA: 0x0001C4B2 File Offset: 0x0001A6B2
		public Vector4 additionalClippingPlane
		{
			get
			{
				if (!(this.clippingPlaneTransform == null))
				{
					return Utils.PlaneEquation(this.clippingPlaneTransform.forward, this.clippingPlaneTransform.position);
				}
				return Vector4.zero;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x0600061F RID: 1567 RVA: 0x0001C4E3 File Offset: 0x0001A6E3
		public float attenuationLerpLinearQuad
		{
			get
			{
				if (this.attenuationEquation == AttenuationEquation.Linear)
				{
					return 0f;
				}
				if (this.attenuationEquation == AttenuationEquation.Quadratic)
				{
					return 1f;
				}
				return this.attenuationCustomBlending;
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x0001C508 File Offset: 0x0001A708
		// (set) Token: 0x06000621 RID: 1569 RVA: 0x0001C510 File Offset: 0x0001A710
		[Obsolete("Use 'fallOffStart' instead")]
		public float fadeStart
		{
			get
			{
				return this.fallOffStart;
			}
			set
			{
				this.fallOffStart = value;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000622 RID: 1570 RVA: 0x0001C519 File Offset: 0x0001A719
		// (set) Token: 0x06000623 RID: 1571 RVA: 0x0001C521 File Offset: 0x0001A721
		[Obsolete("Use 'fallOffEnd' instead")]
		public float fadeEnd
		{
			get
			{
				return this.fallOffEnd;
			}
			set
			{
				this.fallOffEnd = value;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000624 RID: 1572 RVA: 0x0001C52A File Offset: 0x0001A72A
		// (set) Token: 0x06000625 RID: 1573 RVA: 0x0001C532 File Offset: 0x0001A732
		[Obsolete("Use 'fallOffEndFromLight' instead")]
		public bool fadeEndFromLight
		{
			get
			{
				return this.fallOffEndFromLight;
			}
			set
			{
				this.fallOffEndFromLight = value;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000626 RID: 1574 RVA: 0x0001C53B File Offset: 0x0001A73B
		public bool useFallOffEndFromAttachedLightSpot
		{
			get
			{
				return this.fallOffEndFromLight && base.lightSpotAttached != null;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000627 RID: 1575 RVA: 0x0001C553 File Offset: 0x0001A753
		public float maxGeometryDistance
		{
			get
			{
				return this.fallOffEnd + Mathf.Max(Mathf.Abs(this.tiltFactor.x), Mathf.Abs(this.tiltFactor.y));
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000628 RID: 1576 RVA: 0x0001C581 File Offset: 0x0001A781
		public bool isNoiseEnabled
		{
			get
			{
				return this.noiseMode > NoiseMode.Disabled;
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000629 RID: 1577 RVA: 0x0001C58C File Offset: 0x0001A78C
		// (set) Token: 0x0600062A RID: 1578 RVA: 0x0001C594 File Offset: 0x0001A794
		[Obsolete("Use 'noiseMode' instead")]
		public bool noiseEnabled
		{
			get
			{
				return this.isNoiseEnabled;
			}
			set
			{
				this.noiseMode = (value ? NoiseMode.WorldSpace : NoiseMode.Disabled);
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x0600062B RID: 1579 RVA: 0x0001C5A3 File Offset: 0x0001A7A3
		// (set) Token: 0x0600062C RID: 1580 RVA: 0x0001C5AB File Offset: 0x0001A7AB
		public float fadeOutBegin
		{
			get
			{
				return this._FadeOutBegin;
			}
			set
			{
				this.SetFadeOutValue(ref this._FadeOutBegin, value);
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x0600062D RID: 1581 RVA: 0x0001C5BA File Offset: 0x0001A7BA
		// (set) Token: 0x0600062E RID: 1582 RVA: 0x0001C5C2 File Offset: 0x0001A7C2
		public float fadeOutEnd
		{
			get
			{
				return this._FadeOutEnd;
			}
			set
			{
				this.SetFadeOutValue(ref this._FadeOutEnd, value);
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x0600062F RID: 1583 RVA: 0x0001C5D1 File Offset: 0x0001A7D1
		public bool isFadeOutEnabled
		{
			get
			{
				return this._FadeOutBegin >= 0f && this._FadeOutEnd >= 0f;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000630 RID: 1584 RVA: 0x0001C5F2 File Offset: 0x0001A7F2
		public bool isTilted
		{
			get
			{
				return !this.tiltFactor.Approximately(Vector2.zero, 1E-05f);
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000631 RID: 1585 RVA: 0x0001C60C File Offset: 0x0001A80C
		// (set) Token: 0x06000632 RID: 1586 RVA: 0x0001C614 File Offset: 0x0001A814
		public int sortingLayerID
		{
			get
			{
				return this._SortingLayerID;
			}
			set
			{
				this._SortingLayerID = value;
				if (this.m_BeamGeom)
				{
					this.m_BeamGeom.sortingLayerID = value;
				}
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000633 RID: 1587 RVA: 0x0001C636 File Offset: 0x0001A836
		// (set) Token: 0x06000634 RID: 1588 RVA: 0x0001C643 File Offset: 0x0001A843
		public string sortingLayerName
		{
			get
			{
				return SortingLayer.IDToName(this.sortingLayerID);
			}
			set
			{
				this.sortingLayerID = SortingLayer.NameToID(value);
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000635 RID: 1589 RVA: 0x0001C651 File Offset: 0x0001A851
		// (set) Token: 0x06000636 RID: 1590 RVA: 0x0001C659 File Offset: 0x0001A859
		public int sortingOrder
		{
			get
			{
				return this._SortingOrder;
			}
			set
			{
				this._SortingOrder = value;
				if (this.m_BeamGeom)
				{
					this.m_BeamGeom.sortingOrder = value;
				}
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000637 RID: 1591 RVA: 0x0001C67B File Offset: 0x0001A87B
		// (set) Token: 0x06000638 RID: 1592 RVA: 0x0001C683 File Offset: 0x0001A883
		public bool trackChangesDuringPlaytime
		{
			get
			{
				return this._TrackChangesDuringPlaytime;
			}
			set
			{
				this._TrackChangesDuringPlaytime = value;
				this.StartPlaytimeUpdateIfNeeded();
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000639 RID: 1593 RVA: 0x0001C692 File Offset: 0x0001A892
		public bool isCurrentlyTrackingChanges
		{
			get
			{
				return this.m_CoPlaytimeUpdate != null;
			}
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x0001C69D File Offset: 0x0001A89D
		public override BeamGeometryAbstractBase GetBeamGeometry()
		{
			return this.m_BeamGeom;
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x0001C6A5 File Offset: 0x0001A8A5
		protected override void SetBeamGeometryNull()
		{
			this.m_BeamGeom = null;
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x0600063C RID: 1596 RVA: 0x0001C6AE File Offset: 0x0001A8AE
		public int blendingModeAsInt
		{
			get
			{
				return Mathf.Clamp((int)this.blendingMode, 0, Enum.GetValues(typeof(BlendingMode)).Length);
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x0600063D RID: 1597 RVA: 0x0001C6D0 File Offset: 0x0001A8D0
		public Quaternion beamInternalLocalRotation
		{
			get
			{
				if (this.dimensions != Dimensions.Dim3D)
				{
					return Quaternion.LookRotation(Vector3.right, Vector3.up);
				}
				return Quaternion.identity;
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x0600063E RID: 1598 RVA: 0x0001C6EF File Offset: 0x0001A8EF
		public Vector3 beamLocalForward
		{
			get
			{
				if (this.dimensions != Dimensions.Dim3D)
				{
					return Vector3.right;
				}
				return Vector3.forward;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x0600063F RID: 1599 RVA: 0x0001C704 File Offset: 0x0001A904
		public Vector3 beamGlobalForward
		{
			get
			{
				return base.transform.TransformDirection(this.beamLocalForward);
			}
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x0001C718 File Offset: 0x0001A918
		public override Vector3 GetLossyScale()
		{
			if (this.dimensions != Dimensions.Dim3D)
			{
				return new Vector3(base.transform.lossyScale.z, base.transform.lossyScale.y, base.transform.lossyScale.x);
			}
			return base.transform.lossyScale;
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000641 RID: 1601 RVA: 0x0001C770 File Offset: 0x0001A970
		public float raycastDistance
		{
			get
			{
				if (!this.hasMeshSkewing)
				{
					return this.maxGeometryDistance;
				}
				float z = this.skewingLocalForwardDirectionNormalized.z;
				if (!Mathf.Approximately(z, 0f))
				{
					return this.maxGeometryDistance / z;
				}
				return this.maxGeometryDistance;
			}
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x0001C7B4 File Offset: 0x0001A9B4
		private Vector3 ComputeRaycastGlobalVector(Vector3 localVec)
		{
			return base.transform.rotation * this.beamInternalLocalRotation * localVec;
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000643 RID: 1603 RVA: 0x0001C7D2 File Offset: 0x0001A9D2
		public Vector3 raycastGlobalForward
		{
			get
			{
				return this.ComputeRaycastGlobalVector(this.hasMeshSkewing ? this.skewingLocalForwardDirectionNormalized : Vector3.forward);
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000644 RID: 1604 RVA: 0x0001C7EF File Offset: 0x0001A9EF
		public Vector3 raycastGlobalUp
		{
			get
			{
				return this.ComputeRaycastGlobalVector(Vector3.up);
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000645 RID: 1605 RVA: 0x0001C7FC File Offset: 0x0001A9FC
		public Vector3 raycastGlobalRight
		{
			get
			{
				return this.ComputeRaycastGlobalVector(Vector3.right);
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000646 RID: 1606 RVA: 0x0001C809 File Offset: 0x0001AA09
		// (set) Token: 0x06000647 RID: 1607 RVA: 0x0001C81F File Offset: 0x0001AA1F
		public MaterialManager.SD.DynamicOcclusion _INTERNAL_DynamicOcclusionMode
		{
			get
			{
				if (!Config.Instance.featureEnabledDynamicOcclusion)
				{
					return MaterialManager.SD.DynamicOcclusion.Off;
				}
				return this.m_INTERNAL_DynamicOcclusionMode;
			}
			set
			{
				this.m_INTERNAL_DynamicOcclusionMode = value;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000648 RID: 1608 RVA: 0x0001C828 File Offset: 0x0001AA28
		public MaterialManager.SD.DynamicOcclusion _INTERNAL_DynamicOcclusionMode_Runtime
		{
			get
			{
				if (!this.m_INTERNAL_DynamicOcclusionMode_Runtime)
				{
					return MaterialManager.SD.DynamicOcclusion.Off;
				}
				return this._INTERNAL_DynamicOcclusionMode;
			}
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x0001C83A File Offset: 0x0001AA3A
		public void _INTERNAL_SetDynamicOcclusionCallback(string shaderKeyword, MaterialModifier.Callback cb)
		{
			this.m_INTERNAL_DynamicOcclusionMode_Runtime = (cb != null);
			if (this.m_BeamGeom)
			{
				this.m_BeamGeom.SetDynamicOcclusionCallback(shaderKeyword, cb);
			}
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600064A RID: 1610 RVA: 0x0001C860 File Offset: 0x0001AA60
		// (remove) Token: 0x0600064B RID: 1611 RVA: 0x0001C898 File Offset: 0x0001AA98
		public event VolumetricLightBeamSD.OnWillCameraRenderCB onWillCameraRenderThisBeam;

		// Token: 0x0600064C RID: 1612 RVA: 0x0001C8CD File Offset: 0x0001AACD
		public void _INTERNAL_OnWillCameraRenderThisBeam(Camera cam)
		{
			if (this.onWillCameraRenderThisBeam != null)
			{
				this.onWillCameraRenderThisBeam(cam);
			}
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x0001C8E3 File Offset: 0x0001AAE3
		public void RegisterOnBeamGeometryInitializedCallback(VolumetricLightBeamSD.OnBeamGeometryInitialized cb)
		{
			this.m_OnBeamGeometryInitialized = (VolumetricLightBeamSD.OnBeamGeometryInitialized)Delegate.Combine(this.m_OnBeamGeometryInitialized, cb);
			if (this.m_BeamGeom)
			{
				this.CallOnBeamGeometryInitializedCallback();
			}
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x0001C90F File Offset: 0x0001AB0F
		private void CallOnBeamGeometryInitializedCallback()
		{
			if (this.m_OnBeamGeometryInitialized != null)
			{
				this.m_OnBeamGeometryInitialized();
				this.m_OnBeamGeometryInitialized = null;
			}
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x0001C92C File Offset: 0x0001AB2C
		private void SetFadeOutValue(ref float propToChange, float value)
		{
			bool isFadeOutEnabled = this.isFadeOutEnabled;
			propToChange = value;
			if (this.isFadeOutEnabled != isFadeOutEnabled)
			{
				this.OnFadeOutStateChanged();
			}
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x0001C952 File Offset: 0x0001AB52
		private void OnFadeOutStateChanged()
		{
			if (this.isFadeOutEnabled && this.m_BeamGeom)
			{
				this.m_BeamGeom.RestartFadeOutCoroutine();
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000651 RID: 1617 RVA: 0x0001C974 File Offset: 0x0001AB74
		// (set) Token: 0x06000652 RID: 1618 RVA: 0x0001C97C File Offset: 0x0001AB7C
		public uint _INTERNAL_InstancedMaterialGroupID { get; protected set; }

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000653 RID: 1619 RVA: 0x0001C988 File Offset: 0x0001AB88
		public string meshStats
		{
			get
			{
				Mesh mesh = this.m_BeamGeom ? this.m_BeamGeom.coneMesh : null;
				if (mesh)
				{
					return string.Format("Cone angle: {0:0.0} degrees\nMesh: {1} vertices, {2} triangles", this.coneAngle, mesh.vertexCount, mesh.triangles.Length / 3);
				}
				return "no mesh available";
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000654 RID: 1620 RVA: 0x0001C9EE File Offset: 0x0001ABEE
		public int meshVerticesCount
		{
			get
			{
				if (!this.m_BeamGeom || !this.m_BeamGeom.coneMesh)
				{
					return 0;
				}
				return this.m_BeamGeom.coneMesh.vertexCount;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000655 RID: 1621 RVA: 0x0001CA21 File Offset: 0x0001AC21
		public int meshTrianglesCount
		{
			get
			{
				if (!this.m_BeamGeom || !this.m_BeamGeom.coneMesh)
				{
					return 0;
				}
				return this.m_BeamGeom.coneMesh.triangles.Length / 3;
			}
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x0001CA58 File Offset: 0x0001AC58
		public float GetInsideBeamFactor(Vector3 posWS)
		{
			return this.GetInsideBeamFactorFromObjectSpacePos(base.transform.InverseTransformPoint(posWS));
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x0001CA6C File Offset: 0x0001AC6C
		public float GetInsideBeamFactorFromObjectSpacePos(Vector3 posOS)
		{
			if (this.dimensions == Dimensions.Dim2D)
			{
				posOS = new Vector3(posOS.z, posOS.y, posOS.x);
			}
			if (posOS.z < 0f)
			{
				return -1f;
			}
			Vector2 a = posOS.xy();
			if (this.hasMeshSkewing)
			{
				Vector3 skewingLocalForwardDirectionNormalized = this.skewingLocalForwardDirectionNormalized;
				a -= skewingLocalForwardDirectionNormalized.xy() * (posOS.z / skewingLocalForwardDirectionNormalized.z);
			}
			Vector2 normalized = new Vector2(a.magnitude, posOS.z + this.coneApexOffsetZ).normalized;
			return Mathf.Clamp((Mathf.Abs(Mathf.Sin(this.coneAngle * 0.017453292f / 2f)) - Mathf.Abs(normalized.x)) / 0.1f, -1f, 1f);
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x0001CB42 File Offset: 0x0001AD42
		[Obsolete("Use 'GenerateGeometry()' instead")]
		public void Generate()
		{
			this.GenerateGeometry();
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x0001CB4C File Offset: 0x0001AD4C
		public virtual void GenerateGeometry()
		{
			this.HandleBackwardCompatibility(this.pluginVersion, 20100);
			this.pluginVersion = 20100;
			this.ValidateProperties();
			if (this.m_BeamGeom == null)
			{
				this.m_BeamGeom = Utils.NewWithComponent<BeamGeometrySD>("Beam Geometry");
				this.m_BeamGeom.Initialize(this);
				this.CallOnBeamGeometryInitializedCallback();
			}
			this.m_BeamGeom.RegenerateMesh(base.enabled);
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x0001CBBC File Offset: 0x0001ADBC
		public virtual void UpdateAfterManualPropertyChange()
		{
			this.ValidateProperties();
			if (this.m_BeamGeom)
			{
				this.m_BeamGeom.UpdateMaterialAndBounds();
			}
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x0001CBDC File Offset: 0x0001ADDC
		private void Start()
		{
			base.InitLightSpotAttachedCached();
			this.GenerateGeometry();
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x0001CBEA File Offset: 0x0001ADEA
		private void OnEnable()
		{
			if (this.m_BeamGeom)
			{
				this.m_BeamGeom.OnMasterEnable();
			}
			this.StartPlaytimeUpdateIfNeeded();
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x0001CC0A File Offset: 0x0001AE0A
		private void OnDisable()
		{
			if (this.m_BeamGeom)
			{
				this.m_BeamGeom.OnMasterDisable();
			}
			this.m_CoPlaytimeUpdate = null;
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x0001CC2B File Offset: 0x0001AE2B
		private void StartPlaytimeUpdateIfNeeded()
		{
			if (Application.isPlaying && this.trackChangesDuringPlaytime && this.m_CoPlaytimeUpdate == null)
			{
				this.m_CoPlaytimeUpdate = base.StartCoroutine(this.CoPlaytimeUpdate());
			}
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x0001CC56 File Offset: 0x0001AE56
		private IEnumerator CoPlaytimeUpdate()
		{
			while (this.trackChangesDuringPlaytime && base.enabled)
			{
				this.UpdateAfterManualPropertyChange();
				yield return null;
			}
			this.m_CoPlaytimeUpdate = null;
			yield break;
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x0001CC68 File Offset: 0x0001AE68
		private void AssignPropertiesFromAttachedSpotLight()
		{
			Light lightSpotAttached = base.lightSpotAttached;
			if (lightSpotAttached)
			{
				if (this.intensityFromLight)
				{
					this.intensityModeAdvanced = false;
					this.intensityGlobal = SpotLightHelper.GetIntensity(lightSpotAttached) * this.intensityMultiplier;
				}
				if (this.fallOffEndFromLight)
				{
					this.fallOffEnd = SpotLightHelper.GetFallOffEnd(lightSpotAttached) * this.fallOffEndMultiplier;
				}
				if (this.spotAngleFromLight)
				{
					this.spotAngle = Mathf.Clamp(SpotLightHelper.GetSpotAngle(lightSpotAttached) * this.spotAngleMultiplier, 0.1f, 179.9f);
				}
				if (this.colorFromLight)
				{
					this.colorMode = ColorMode.Flat;
					if (this.useColorTemperatureFromAttachedLightSpot)
					{
						Color b = Mathf.CorrelatedColorTemperatureToRGB(lightSpotAttached.colorTemperature);
						this.color = (lightSpotAttached.color.linear * b).gamma;
						return;
					}
					this.color = lightSpotAttached.color;
				}
			}
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x0001CD40 File Offset: 0x0001AF40
		private void ClampProperties()
		{
			this.intensityInside = Mathf.Max(this.intensityInside, 0f);
			this.intensityOutside = Mathf.Max(this.intensityOutside, 0f);
			this.intensityMultiplier = Mathf.Max(this.intensityMultiplier, 0f);
			this.attenuationCustomBlending = Mathf.Clamp(this.attenuationCustomBlending, 0f, 1f);
			this.fallOffEnd = Mathf.Max(0.01f, this.fallOffEnd);
			this.fallOffStart = Mathf.Clamp(this.fallOffStart, 0f, this.fallOffEnd - 0.01f);
			this.fallOffEndMultiplier = Mathf.Max(this.fallOffEndMultiplier, 0f);
			this.spotAngle = Mathf.Clamp(this.spotAngle, 0.1f, 179.9f);
			this.spotAngleMultiplier = Mathf.Max(this.spotAngleMultiplier, 0f);
			this.coneRadiusStart = Mathf.Max(this.coneRadiusStart, 0f);
			this.depthBlendDistance = Mathf.Max(this.depthBlendDistance, 0f);
			this.cameraClippingDistance = Mathf.Max(this.cameraClippingDistance, 0f);
			this.geomCustomSides = Mathf.Clamp(this.geomCustomSides, 3, 256);
			this.geomCustomSegments = Mathf.Clamp(this.geomCustomSegments, 0, 64);
			this.fresnelPow = Mathf.Max(0f, this.fresnelPow);
			this.glareBehind = Mathf.Clamp(this.glareBehind, 0f, 1f);
			this.glareFrontal = Mathf.Clamp(this.glareFrontal, 0f, 1f);
			this.noiseIntensity = Mathf.Clamp(this.noiseIntensity, 0f, 1f);
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x0001CEFD File Offset: 0x0001B0FD
		private void ValidateProperties()
		{
			this.AssignPropertiesFromAttachedSpotLight();
			this.ClampProperties();
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x0001CF0C File Offset: 0x0001B10C
		private void HandleBackwardCompatibility(int serializedVersion, int newVersion)
		{
			if (serializedVersion == -1)
			{
				return;
			}
			if (serializedVersion == newVersion)
			{
				return;
			}
			if (serializedVersion < 1301)
			{
				this.attenuationEquation = AttenuationEquation.Linear;
			}
			if (serializedVersion < 1501)
			{
				this.geomMeshType = MeshType.Custom;
				this.geomCustomSegments = 5;
			}
			if (serializedVersion < 1610)
			{
				this.intensityFromLight = false;
				this.intensityModeAdvanced = !Mathf.Approximately(this.intensityInside, this.intensityOutside);
			}
			if (serializedVersion < 1910 && !this.intensityModeAdvanced && !Mathf.Approximately(this.intensityInside, this.intensityOutside))
			{
				this.intensityInside = this.intensityOutside;
			}
			Utils.MarkCurrentSceneDirty();
		}

		// Token: 0x040006E8 RID: 1768
		public new const string ClassName = "VolumetricLightBeamSD";

		// Token: 0x040006E9 RID: 1769
		public bool colorFromLight = true;

		// Token: 0x040006EA RID: 1770
		public ColorMode colorMode;

		// Token: 0x040006EB RID: 1771
		[ColorUsage(false, true)]
		[FormerlySerializedAs("colorValue")]
		public Color color = Consts.Beam.FlatColor;

		// Token: 0x040006EC RID: 1772
		public Gradient colorGradient;

		// Token: 0x040006ED RID: 1773
		public bool intensityFromLight = true;

		// Token: 0x040006EE RID: 1774
		public bool intensityModeAdvanced;

		// Token: 0x040006EF RID: 1775
		[FormerlySerializedAs("alphaInside")]
		[Min(0f)]
		public float intensityInside = 1f;

		// Token: 0x040006F0 RID: 1776
		[FormerlySerializedAs("alphaOutside")]
		[FormerlySerializedAs("alpha")]
		[Min(0f)]
		public float intensityOutside = 1f;

		// Token: 0x040006F1 RID: 1777
		[Min(0f)]
		public float intensityMultiplier = 1f;

		// Token: 0x040006F2 RID: 1778
		[Range(0f, 1f)]
		public float hdrpExposureWeight;

		// Token: 0x040006F3 RID: 1779
		public BlendingMode blendingMode;

		// Token: 0x040006F4 RID: 1780
		[FormerlySerializedAs("angleFromLight")]
		public bool spotAngleFromLight = true;

		// Token: 0x040006F5 RID: 1781
		[Range(0.1f, 179.9f)]
		public float spotAngle = 35f;

		// Token: 0x040006F6 RID: 1782
		[Min(0f)]
		public float spotAngleMultiplier = 1f;

		// Token: 0x040006F7 RID: 1783
		[FormerlySerializedAs("radiusStart")]
		public float coneRadiusStart = 0.1f;

		// Token: 0x040006F8 RID: 1784
		public ShaderAccuracy shaderAccuracy;

		// Token: 0x040006F9 RID: 1785
		public MeshType geomMeshType;

		// Token: 0x040006FA RID: 1786
		[FormerlySerializedAs("geomSides")]
		public int geomCustomSides = 18;

		// Token: 0x040006FB RID: 1787
		public int geomCustomSegments = 5;

		// Token: 0x040006FC RID: 1788
		public Vector3 skewingLocalForwardDirection = Consts.Beam.SD.SkewingLocalForwardDirectionDefault;

		// Token: 0x040006FD RID: 1789
		public Transform clippingPlaneTransform;

		// Token: 0x040006FE RID: 1790
		public bool geomCap;

		// Token: 0x040006FF RID: 1791
		public AttenuationEquation attenuationEquation = AttenuationEquation.Quadratic;

		// Token: 0x04000700 RID: 1792
		[Range(0f, 1f)]
		public float attenuationCustomBlending = 0.5f;

		// Token: 0x04000701 RID: 1793
		[FormerlySerializedAs("fadeStart")]
		public float fallOffStart;

		// Token: 0x04000702 RID: 1794
		[FormerlySerializedAs("fadeEnd")]
		public float fallOffEnd = 3f;

		// Token: 0x04000703 RID: 1795
		[FormerlySerializedAs("fadeEndFromLight")]
		public bool fallOffEndFromLight = true;

		// Token: 0x04000704 RID: 1796
		[Min(0f)]
		public float fallOffEndMultiplier = 1f;

		// Token: 0x04000705 RID: 1797
		public float depthBlendDistance = 2f;

		// Token: 0x04000706 RID: 1798
		public float cameraClippingDistance = 0.5f;

		// Token: 0x04000707 RID: 1799
		[Range(0f, 1f)]
		public float glareFrontal = 0.5f;

		// Token: 0x04000708 RID: 1800
		[Range(0f, 1f)]
		public float glareBehind = 0.5f;

		// Token: 0x04000709 RID: 1801
		[FormerlySerializedAs("fresnelPowOutside")]
		public float fresnelPow = 8f;

		// Token: 0x0400070A RID: 1802
		public NoiseMode noiseMode;

		// Token: 0x0400070B RID: 1803
		[Range(0f, 1f)]
		public float noiseIntensity = 0.5f;

		// Token: 0x0400070C RID: 1804
		public bool noiseScaleUseGlobal = true;

		// Token: 0x0400070D RID: 1805
		[Range(0.01f, 2f)]
		public float noiseScaleLocal = 0.5f;

		// Token: 0x0400070E RID: 1806
		public bool noiseVelocityUseGlobal = true;

		// Token: 0x0400070F RID: 1807
		public Vector3 noiseVelocityLocal = Consts.Beam.NoiseVelocityDefault;

		// Token: 0x04000710 RID: 1808
		public Dimensions dimensions;

		// Token: 0x04000711 RID: 1809
		public Vector2 tiltFactor = Consts.Beam.SD.TiltDefault;

		// Token: 0x04000712 RID: 1810
		private MaterialManager.SD.DynamicOcclusion m_INTERNAL_DynamicOcclusionMode;

		// Token: 0x04000713 RID: 1811
		private bool m_INTERNAL_DynamicOcclusionMode_Runtime;

		// Token: 0x04000715 RID: 1813
		private VolumetricLightBeamSD.OnBeamGeometryInitialized m_OnBeamGeometryInitialized;

		// Token: 0x04000716 RID: 1814
		[FormerlySerializedAs("trackChangesDuringPlaytime")]
		[SerializeField]
		private bool _TrackChangesDuringPlaytime;

		// Token: 0x04000717 RID: 1815
		[SerializeField]
		private int _SortingLayerID;

		// Token: 0x04000718 RID: 1816
		[SerializeField]
		private int _SortingOrder;

		// Token: 0x04000719 RID: 1817
		[FormerlySerializedAs("fadeOutBegin")]
		[SerializeField]
		private float _FadeOutBegin = -150f;

		// Token: 0x0400071A RID: 1818
		[FormerlySerializedAs("fadeOutEnd")]
		[SerializeField]
		private float _FadeOutEnd = -200f;

		// Token: 0x0400071C RID: 1820
		private BeamGeometrySD m_BeamGeom;

		// Token: 0x0400071D RID: 1821
		private Coroutine m_CoPlaytimeUpdate;

		// Token: 0x0200014D RID: 333
		// (Invoke) Token: 0x06000666 RID: 1638
		public delegate void OnWillCameraRenderCB(Camera cam);

		// Token: 0x0200014E RID: 334
		// (Invoke) Token: 0x0600066A RID: 1642
		public delegate void OnBeamGeometryInitialized();
	}
}
