using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Funly.SkyStudio
{
	// Token: 0x020001E4 RID: 484
	public abstract class BaseSpriteInstancedRenderer : MonoBehaviour
	{
		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000AA1 RID: 2721 RVA: 0x0002EF04 File Offset: 0x0002D104
		// (set) Token: 0x06000AA2 RID: 2722 RVA: 0x0002EF0C File Offset: 0x0002D10C
		public int maxSprites { get; protected set; }

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000AA3 RID: 2723 RVA: 0x0002EF15 File Offset: 0x0002D115
		// (set) Token: 0x06000AA4 RID: 2724 RVA: 0x0002EF1D File Offset: 0x0002D11D
		protected Camera m_ViewerCamera { get; set; }

		// Token: 0x06000AA5 RID: 2725 RVA: 0x0002EF26 File Offset: 0x0002D126
		private void Start()
		{
			if (!SystemInfo.supportsInstancing)
			{
				Debug.LogError("Can't render since GPU instancing isn't supported on this device");
				base.enabled = false;
				return;
			}
			this.m_ViewerCamera = Camera.main;
		}

		// Token: 0x06000AA6 RID: 2726
		protected abstract Bounds CalculateMeshBounds();

		// Token: 0x06000AA7 RID: 2727
		protected abstract BaseSpriteItemData CreateSpriteItemData();

		// Token: 0x06000AA8 RID: 2728
		protected abstract bool IsRenderingEnabled();

		// Token: 0x06000AA9 RID: 2729
		protected abstract int GetNextSpawnCount();

		// Token: 0x06000AAA RID: 2730
		protected abstract void CalculateSpriteTRS(BaseSpriteItemData data, out Vector3 spritePosition, out Quaternion spriteRotation, out Vector3 spriteScale);

		// Token: 0x06000AAB RID: 2731
		protected abstract void ConfigureSpriteItemData(BaseSpriteItemData data);

		// Token: 0x06000AAC RID: 2732
		protected abstract void PrepareDataArraysForRendering(int instanceId, BaseSpriteItemData data);

		// Token: 0x06000AAD RID: 2733
		protected abstract void PopulatePropertyBlockForRendering(ref MaterialPropertyBlock propertyBlock);

		// Token: 0x06000AAE RID: 2734 RVA: 0x0002EF4C File Offset: 0x0002D14C
		private BaseSpriteItemData DequeueNextSpriteItemData()
		{
			BaseSpriteItemData baseSpriteItemData;
			if (this.m_Available.Count == 0)
			{
				baseSpriteItemData = this.CreateSpriteItemData();
			}
			else
			{
				baseSpriteItemData = this.m_Available.Dequeue();
			}
			this.m_Active.Add(baseSpriteItemData);
			return baseSpriteItemData;
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x0002EF8B File Offset: 0x0002D18B
		private void ReturnSpriteItemData(BaseSpriteItemData splash)
		{
			splash.Reset();
			this.m_Active.Remove(splash);
			this.m_Available.Enqueue(splash);
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x0002EFAC File Offset: 0x0002D1AC
		protected virtual void LateUpdate()
		{
			this.m_ViewerCamera = Camera.main;
			if (!this.IsRenderingEnabled())
			{
				return;
			}
			this.GenerateNewSprites();
			this.AdvanceAllSprites();
			this.RenderAllSprites();
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x0002EFD4 File Offset: 0x0002D1D4
		private void GenerateNewSprites()
		{
			int nextSpawnCount = this.GetNextSpawnCount();
			for (int i = 0; i < nextSpawnCount; i++)
			{
				BaseSpriteItemData baseSpriteItemData = this.DequeueNextSpriteItemData();
				baseSpriteItemData.spriteSheetData = this.m_SpriteSheetLayout;
				this.ConfigureSpriteItemData(baseSpriteItemData);
				Vector3 worldPosition;
				Quaternion rotation;
				Vector3 scale;
				this.CalculateSpriteTRS(baseSpriteItemData, out worldPosition, out rotation, out scale);
				baseSpriteItemData.SetTRSMatrix(worldPosition, rotation, scale);
				baseSpriteItemData.Start();
			}
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x0002F034 File Offset: 0x0002D234
		private void AdvanceAllSprites()
		{
			foreach (BaseSpriteItemData baseSpriteItemData in new HashSet<BaseSpriteItemData>(this.m_Active))
			{
				baseSpriteItemData.Continue();
				if (baseSpriteItemData.state == BaseSpriteItemData.SpriteState.Complete)
				{
					this.ReturnSpriteItemData(baseSpriteItemData);
				}
			}
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x0002F09C File Offset: 0x0002D29C
		private void RenderAllSprites()
		{
			if (this.m_Active.Count == 0)
			{
				return;
			}
			if (this.renderMaterial == null)
			{
				Debug.LogError("Can't render sprite without a material.");
				return;
			}
			if (this.m_PropertyBlock == null)
			{
				this.m_PropertyBlock = new MaterialPropertyBlock();
			}
			int num = 0;
			foreach (BaseSpriteItemData baseSpriteItemData in this.m_Active)
			{
				if (num >= 1000)
				{
					Debug.LogError("Can't render any more sprites...");
					break;
				}
				if (baseSpriteItemData.state == BaseSpriteItemData.SpriteState.Animating && baseSpriteItemData.startTime <= Time.time)
				{
					this.m_ModelMatrices[num] = baseSpriteItemData.modelMatrix;
					this.m_StartTimes[num] = baseSpriteItemData.startTime;
					this.m_EndTimes[num] = baseSpriteItemData.endTime;
					this.PrepareDataArraysForRendering(num, baseSpriteItemData);
					num++;
				}
			}
			if (num == 0)
			{
				return;
			}
			this.m_PropertyBlock.Clear();
			this.m_PropertyBlock.SetFloatArray("_StartTime", this.m_StartTimes);
			this.m_PropertyBlock.SetFloatArray("_EndTime", this.m_EndTimes);
			this.m_PropertyBlock.SetFloat("_SpriteColumnCount", (float)this.m_SpriteSheetLayout.columns);
			this.m_PropertyBlock.SetFloat("_SpriteRowCount", (float)this.m_SpriteSheetLayout.rows);
			this.m_PropertyBlock.SetFloat("_SpriteItemCount", (float)this.m_SpriteSheetLayout.frameCount);
			this.m_PropertyBlock.SetFloat("_AnimationSpeed", (float)this.m_SpriteSheetLayout.frameRate);
			this.m_PropertyBlock.SetVector("_TintColor", this.m_TintColor);
			this.PopulatePropertyBlockForRendering(ref this.m_PropertyBlock);
			Mesh mesh = this.GetMesh();
			mesh.bounds = this.CalculateMeshBounds();
			Graphics.DrawMeshInstanced(mesh, 0, this.renderMaterial, this.m_ModelMatrices, num, this.m_PropertyBlock, ShadowCastingMode.Off, false, LayerMask.NameToLayer("TransparentFX"));
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x0002F294 File Offset: 0x0002D494
		protected Mesh GetMesh()
		{
			if (this.modelMesh)
			{
				return this.modelMesh;
			}
			if (this.m_DefaltModelMesh)
			{
				return this.m_DefaltModelMesh;
			}
			this.m_DefaltModelMesh = this.GenerateMesh();
			return this.m_DefaltModelMesh;
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x0002F2D0 File Offset: 0x0002D4D0
		protected virtual Mesh GenerateMesh()
		{
			Mesh mesh = new Mesh();
			Vector3[] vertices = new Vector3[]
			{
				new Vector3(-1f, -1f, 0f),
				new Vector3(-1f, 1f, 0f),
				new Vector3(1f, 1f, 0f),
				new Vector3(1f, -1f, 0f)
			};
			Vector2[] uv = new Vector2[]
			{
				new Vector2(0f, 0f),
				new Vector2(0f, 1f),
				new Vector2(1f, 1f),
				new Vector2(1f, 0f)
			};
			int[] triangles = new int[]
			{
				0,
				1,
				2,
				0,
				2,
				3
			};
			mesh.vertices = vertices;
			mesh.uv = uv;
			mesh.triangles = triangles;
			mesh.bounds = new Bounds(Vector3.zero, new Vector3(500f, 500f, 500f));
			return mesh;
		}

		// Token: 0x04000B77 RID: 2935
		public const int kArrayMaxSprites = 1000;

		// Token: 0x04000B79 RID: 2937
		[Tooltip("Mesh used to render the instances onto. If empty, a quad will be used.")]
		public Mesh modelMesh;

		// Token: 0x04000B7A RID: 2938
		[Tooltip("Sky Studio sprite sheet animated shader material.")]
		public Material renderMaterial;

		// Token: 0x04000B7B RID: 2939
		protected Queue<BaseSpriteItemData> m_Available = new Queue<BaseSpriteItemData>();

		// Token: 0x04000B7C RID: 2940
		protected HashSet<BaseSpriteItemData> m_Active = new HashSet<BaseSpriteItemData>();

		// Token: 0x04000B7D RID: 2941
		private MaterialPropertyBlock m_PropertyBlock;

		// Token: 0x04000B7E RID: 2942
		private Matrix4x4[] m_ModelMatrices = new Matrix4x4[1000];

		// Token: 0x04000B7F RID: 2943
		private float[] m_StartTimes = new float[1000];

		// Token: 0x04000B80 RID: 2944
		private float[] m_EndTimes = new float[1000];

		// Token: 0x04000B81 RID: 2945
		protected SpriteSheetData m_SpriteSheetLayout = new SpriteSheetData();

		// Token: 0x04000B82 RID: 2946
		protected Texture m_SpriteTexture;

		// Token: 0x04000B83 RID: 2947
		protected Color m_TintColor = Color.white;

		// Token: 0x04000B85 RID: 2949
		protected Mesh m_DefaltModelMesh;
	}
}
