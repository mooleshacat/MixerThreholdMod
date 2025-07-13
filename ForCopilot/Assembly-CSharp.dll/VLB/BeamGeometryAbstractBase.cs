using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x020000E7 RID: 231
	public abstract class BeamGeometryAbstractBase : MonoBehaviour
	{
		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060003CF RID: 975 RVA: 0x00015D4D File Offset: 0x00013F4D
		// (set) Token: 0x060003D0 RID: 976 RVA: 0x00015D55 File Offset: 0x00013F55
		public MeshRenderer meshRenderer { get; protected set; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060003D1 RID: 977 RVA: 0x00015D5E File Offset: 0x00013F5E
		// (set) Token: 0x060003D2 RID: 978 RVA: 0x00015D66 File Offset: 0x00013F66
		public MeshFilter meshFilter { get; protected set; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060003D3 RID: 979 RVA: 0x00015D6F File Offset: 0x00013F6F
		// (set) Token: 0x060003D4 RID: 980 RVA: 0x00015D77 File Offset: 0x00013F77
		public Mesh coneMesh { get; protected set; }

		// Token: 0x060003D5 RID: 981
		protected abstract VolumetricLightBeamAbstractBase GetMaster();

		// Token: 0x060003D6 RID: 982 RVA: 0x00015D80 File Offset: 0x00013F80
		private void Start()
		{
			this.DestroyInvalidOwner();
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x00015D88 File Offset: 0x00013F88
		private void OnDestroy()
		{
			if (this.m_CustomMaterial)
			{
				UnityEngine.Object.DestroyImmediate(this.m_CustomMaterial);
				this.m_CustomMaterial = null;
			}
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x00015DA9 File Offset: 0x00013FA9
		private void DestroyInvalidOwner()
		{
			if (!this.GetMaster())
			{
				BeamGeometryAbstractBase.DestroyBeamGeometryGameObject(this);
			}
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x00015DBE File Offset: 0x00013FBE
		public static void DestroyBeamGeometryGameObject(BeamGeometryAbstractBase beamGeom)
		{
			if (beamGeom)
			{
				UnityEngine.Object.DestroyImmediate(beamGeom.gameObject);
			}
		}

		// Token: 0x040004A7 RID: 1191
		protected Matrix4x4 m_ColorGradientMatrix;

		// Token: 0x040004A8 RID: 1192
		protected Material m_CustomMaterial;
	}
}
