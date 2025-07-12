using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace VLB
{
	// Token: 0x02000146 RID: 326
	[ExecuteInEditMode]
	[HelpURL("http://saladgamer.com/vlb-doc/comp-dynocclusion-sd-raycasting/")]
	public class DynamicOcclusionRaycasting : DynamicOcclusionAbstractBase
	{
		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060005D0 RID: 1488 RVA: 0x0001B7D1 File Offset: 0x000199D1
		// (set) Token: 0x060005D1 RID: 1489 RVA: 0x0001B7D9 File Offset: 0x000199D9
		[Obsolete("Use 'fadeDistanceToSurface' instead")]
		public float fadeDistanceToPlane
		{
			get
			{
				return this.fadeDistanceToSurface;
			}
			set
			{
				this.fadeDistanceToSurface = value;
			}
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x0001B7E2 File Offset: 0x000199E2
		public bool IsColliderHiddenByDynamicOccluder(Collider collider)
		{
			return this.planeEquationWS.IsValid() && !GeometryUtility.TestPlanesAABB(new Plane[]
			{
				this.planeEquationWS
			}, collider.bounds);
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x0001B814 File Offset: 0x00019A14
		protected override string GetShaderKeyword()
		{
			return "VLB_OCCLUSION_CLIPPING_PLANE";
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x000022C9 File Offset: 0x000004C9
		protected override MaterialManager.SD.DynamicOcclusion GetDynamicOcclusionMode()
		{
			return MaterialManager.SD.DynamicOcclusion.ClippingPlane;
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060005D5 RID: 1493 RVA: 0x0001B81B File Offset: 0x00019A1B
		// (set) Token: 0x060005D6 RID: 1494 RVA: 0x0001B823 File Offset: 0x00019A23
		public Plane planeEquationWS { get; private set; }

		// Token: 0x060005D7 RID: 1495 RVA: 0x0001B82C File Offset: 0x00019A2C
		protected override void OnValidateProperties()
		{
			base.OnValidateProperties();
			this.minOccluderArea = Mathf.Max(this.minOccluderArea, 0f);
			this.fadeDistanceToSurface = Mathf.Max(this.fadeDistanceToSurface, 0f);
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x0001B860 File Offset: 0x00019A60
		protected override void OnEnablePostValidate()
		{
			this.m_CurrentHit.SetNull();
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x0001B86D File Offset: 0x00019A6D
		protected override void OnDisable()
		{
			base.OnDisable();
			this.SetHitNull();
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x0001B87C File Offset: 0x00019A7C
		private void Start()
		{
			if (Application.isPlaying)
			{
				TriggerZone component = base.GetComponent<TriggerZone>();
				if (component)
				{
					this.m_RangeMultiplier = Mathf.Max(1f, component.rangeMultiplier);
				}
			}
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0001B8B8 File Offset: 0x00019AB8
		private Vector3 GetRandomVectorAround(Vector3 direction, float angleDiff)
		{
			float num = angleDiff * 0.5f;
			return Quaternion.Euler(UnityEngine.Random.Range(-num, num), UnityEngine.Random.Range(-num, num), UnityEngine.Random.Range(-num, num)) * direction;
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060005DC RID: 1500 RVA: 0x0001B8F0 File Offset: 0x00019AF0
		private QueryTriggerInteraction queryTriggerInteraction
		{
			get
			{
				if (!this.considerTriggers)
				{
					return 1;
				}
				return 2;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060005DD RID: 1501 RVA: 0x0001B8FD File Offset: 0x00019AFD
		private float raycastMaxDistance
		{
			get
			{
				return this.m_Master.raycastDistance * this.m_RangeMultiplier * this.m_Master.GetLossyScale().z;
			}
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x0001B922 File Offset: 0x00019B22
		private DynamicOcclusionRaycasting.HitResult GetBestHit(Vector3 rayPos, Vector3 rayDir)
		{
			if (this.dimensions != Dimensions.Dim2D)
			{
				return this.GetBestHit3D(rayPos, rayDir);
			}
			return this.GetBestHit2D(rayPos, rayDir);
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x0001B940 File Offset: 0x00019B40
		private DynamicOcclusionRaycasting.HitResult GetBestHit3D(Vector3 rayPos, Vector3 rayDir)
		{
			RaycastHit[] array = Physics.RaycastAll(rayPos, rayDir, this.raycastMaxDistance, this.layerMask.value, this.queryTriggerInteraction);
			int num = -1;
			float num2 = float.MaxValue;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].collider.gameObject != this.m_Master.gameObject && array[i].collider.bounds.GetMaxArea2D() >= this.minOccluderArea && array[i].distance < num2)
				{
					num2 = array[i].distance;
					num = i;
				}
			}
			if (num != -1)
			{
				return new DynamicOcclusionRaycasting.HitResult(ref array[num]);
			}
			return default(DynamicOcclusionRaycasting.HitResult);
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x0001B9FC File Offset: 0x00019BFC
		private DynamicOcclusionRaycasting.HitResult GetBestHit2D(Vector3 rayPos, Vector3 rayDir)
		{
			RaycastHit2D[] array = Physics2D.RaycastAll(new Vector2(rayPos.x, rayPos.y), new Vector2(rayDir.x, rayDir.y), this.raycastMaxDistance, this.layerMask.value);
			int num = -1;
			float num2 = float.MaxValue;
			for (int i = 0; i < array.Length; i++)
			{
				if ((this.considerTriggers || !array[i].collider.isTrigger) && array[i].collider.gameObject != this.m_Master.gameObject && array[i].collider.bounds.GetMaxArea2D() >= this.minOccluderArea && array[i].distance < num2)
				{
					num2 = array[i].distance;
					num = i;
				}
			}
			if (num != -1)
			{
				return new DynamicOcclusionRaycasting.HitResult(ref array[num]);
			}
			return default(DynamicOcclusionRaycasting.HitResult);
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x0001BAF0 File Offset: 0x00019CF0
		private uint GetDirectionCount()
		{
			if (this.dimensions != Dimensions.Dim2D)
			{
				return 4U;
			}
			return 2U;
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x0001BB00 File Offset: 0x00019D00
		private Vector3 GetDirection(uint dirInt)
		{
			dirInt %= this.GetDirectionCount();
			switch (dirInt)
			{
			case 0U:
				return this.m_Master.raycastGlobalUp;
			case 1U:
				return -this.m_Master.raycastGlobalUp;
			case 2U:
				return -this.m_Master.raycastGlobalRight;
			case 3U:
				return this.m_Master.raycastGlobalRight;
			default:
				return Vector3.zero;
			}
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x0001BB6E File Offset: 0x00019D6E
		private bool IsHitValid(ref DynamicOcclusionRaycasting.HitResult hit, Vector3 forwardVec)
		{
			return hit.hasCollider && Vector3.Dot(hit.normal, -forwardVec) >= this.maxSurfaceDot;
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x0001BB98 File Offset: 0x00019D98
		protected override bool OnProcessOcclusion(DynamicOcclusionAbstractBase.ProcessOcclusionSource source)
		{
			Vector3 raycastGlobalForward = this.m_Master.raycastGlobalForward;
			DynamicOcclusionRaycasting.HitResult hitResult = this.GetBestHit(base.transform.position, raycastGlobalForward);
			if (this.IsHitValid(ref hitResult, raycastGlobalForward))
			{
				if (this.minSurfaceRatio > 0.5f)
				{
					float raycastDistance = this.m_Master.raycastDistance;
					for (uint num = 0U; num < this.GetDirectionCount(); num += 1U)
					{
						Vector3 a = this.GetDirection(num + this.m_PrevNonSubHitDirectionId) * (this.minSurfaceRatio * 2f - 1f);
						a.Scale(base.transform.localScale);
						Vector3 vector = base.transform.position + a * this.m_Master.coneRadiusStart;
						Vector3 a2 = base.transform.position + a * this.m_Master.coneRadiusEnd + raycastGlobalForward * raycastDistance;
						DynamicOcclusionRaycasting.HitResult bestHit = this.GetBestHit(vector, (a2 - vector).normalized);
						if (!this.IsHitValid(ref bestHit, raycastGlobalForward))
						{
							this.m_PrevNonSubHitDirectionId = num;
							hitResult.SetNull();
							break;
						}
						if (bestHit.distance > hitResult.distance)
						{
							hitResult = bestHit;
						}
					}
				}
			}
			else
			{
				hitResult.SetNull();
			}
			this.SetHit(ref hitResult);
			return hitResult.hasCollider;
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x0001BCF8 File Offset: 0x00019EF8
		private void SetHit(ref DynamicOcclusionRaycasting.HitResult hit)
		{
			if (!hit.hasCollider)
			{
				this.SetHitNull();
				return;
			}
			PlaneAlignment planeAlignment = this.planeAlignment;
			if (planeAlignment != PlaneAlignment.Surface && planeAlignment == PlaneAlignment.Beam)
			{
				this.SetClippingPlane(new Plane(-this.m_Master.raycastGlobalForward, hit.point));
			}
			else
			{
				this.SetClippingPlane(new Plane(hit.normal, hit.point));
			}
			this.m_CurrentHit = hit;
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x0001BD68 File Offset: 0x00019F68
		private void SetHitNull()
		{
			this.SetClippingPlaneOff();
			this.m_CurrentHit.SetNull();
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x0001BD7C File Offset: 0x00019F7C
		protected override void OnModifyMaterialCallback(MaterialModifier.Interface owner)
		{
			Plane planeEquationWS = this.planeEquationWS;
			owner.SetMaterialProp(ShaderProperties.SD.DynamicOcclusionClippingPlaneWS, new Vector4(planeEquationWS.normal.x, planeEquationWS.normal.y, planeEquationWS.normal.z, planeEquationWS.distance));
			owner.SetMaterialProp(ShaderProperties.SD.DynamicOcclusionClippingPlaneProps, this.fadeDistanceToSurface);
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x0001BDDC File Offset: 0x00019FDC
		private void SetClippingPlane(Plane planeWS)
		{
			planeWS = planeWS.TranslateCustom(planeWS.normal * this.planeOffset);
			this.SetPlaneWS(planeWS);
			this.m_Master._INTERNAL_SetDynamicOcclusionCallback(this.GetShaderKeyword(), this.m_MaterialModifierCallbackCached);
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x0001BE18 File Offset: 0x0001A018
		private void SetClippingPlaneOff()
		{
			this.SetPlaneWS(default(Plane));
			this.m_Master._INTERNAL_SetDynamicOcclusionCallback(this.GetShaderKeyword(), null);
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x0001BE46 File Offset: 0x0001A046
		private void SetPlaneWS(Plane planeWS)
		{
			this.planeEquationWS = planeWS;
		}

		// Token: 0x040006C6 RID: 1734
		public new const string ClassName = "DynamicOcclusionRaycasting";

		// Token: 0x040006C7 RID: 1735
		public Dimensions dimensions;

		// Token: 0x040006C8 RID: 1736
		public LayerMask layerMask = Consts.DynOcclusion.LayerMaskDefault;

		// Token: 0x040006C9 RID: 1737
		public bool considerTriggers;

		// Token: 0x040006CA RID: 1738
		public float minOccluderArea;

		// Token: 0x040006CB RID: 1739
		public float minSurfaceRatio = 0.5f;

		// Token: 0x040006CC RID: 1740
		public float maxSurfaceDot = 0.25f;

		// Token: 0x040006CD RID: 1741
		public PlaneAlignment planeAlignment;

		// Token: 0x040006CE RID: 1742
		public float planeOffset = 0.1f;

		// Token: 0x040006CF RID: 1743
		[FormerlySerializedAs("fadeDistanceToPlane")]
		public float fadeDistanceToSurface = 0.25f;

		// Token: 0x040006D0 RID: 1744
		private DynamicOcclusionRaycasting.HitResult m_CurrentHit;

		// Token: 0x040006D1 RID: 1745
		private float m_RangeMultiplier = 1f;

		// Token: 0x040006D3 RID: 1747
		private uint m_PrevNonSubHitDirectionId;

		// Token: 0x02000147 RID: 327
		public struct HitResult
		{
			// Token: 0x060005EC RID: 1516 RVA: 0x0001BEA5 File Offset: 0x0001A0A5
			public HitResult(ref RaycastHit hit3D)
			{
				this.point = hit3D.point;
				this.normal = hit3D.normal;
				this.distance = hit3D.distance;
				this.collider3D = hit3D.collider;
				this.collider2D = null;
			}

			// Token: 0x060005ED RID: 1517 RVA: 0x0001BEE0 File Offset: 0x0001A0E0
			public HitResult(ref RaycastHit2D hit2D)
			{
				this.point = hit2D.point;
				this.normal = hit2D.normal;
				this.distance = hit2D.distance;
				this.collider2D = hit2D.collider;
				this.collider3D = null;
			}

			// Token: 0x17000110 RID: 272
			// (get) Token: 0x060005EE RID: 1518 RVA: 0x0001BF2E File Offset: 0x0001A12E
			public bool hasCollider
			{
				get
				{
					return this.collider2D || this.collider3D;
				}
			}

			// Token: 0x17000111 RID: 273
			// (get) Token: 0x060005EF RID: 1519 RVA: 0x0001BF4A File Offset: 0x0001A14A
			public string name
			{
				get
				{
					if (this.collider3D)
					{
						return this.collider3D.name;
					}
					if (this.collider2D)
					{
						return this.collider2D.name;
					}
					return "null collider";
				}
			}

			// Token: 0x17000112 RID: 274
			// (get) Token: 0x060005F0 RID: 1520 RVA: 0x0001BF84 File Offset: 0x0001A184
			public Bounds bounds
			{
				get
				{
					if (this.collider3D)
					{
						return this.collider3D.bounds;
					}
					if (this.collider2D)
					{
						return this.collider2D.bounds;
					}
					return default(Bounds);
				}
			}

			// Token: 0x060005F1 RID: 1521 RVA: 0x0001BFCC File Offset: 0x0001A1CC
			public void SetNull()
			{
				this.collider2D = null;
				this.collider3D = null;
			}

			// Token: 0x040006D4 RID: 1748
			public Vector3 point;

			// Token: 0x040006D5 RID: 1749
			public Vector3 normal;

			// Token: 0x040006D6 RID: 1750
			public float distance;

			// Token: 0x040006D7 RID: 1751
			private Collider2D collider2D;

			// Token: 0x040006D8 RID: 1752
			private Collider collider3D;
		}

		// Token: 0x02000148 RID: 328
		private enum Direction
		{
			// Token: 0x040006DA RID: 1754
			Up,
			// Token: 0x040006DB RID: 1755
			Down,
			// Token: 0x040006DC RID: 1756
			Left,
			// Token: 0x040006DD RID: 1757
			Right,
			// Token: 0x040006DE RID: 1758
			Max2D = 1,
			// Token: 0x040006DF RID: 1759
			Max3D = 3
		}
	}
}
