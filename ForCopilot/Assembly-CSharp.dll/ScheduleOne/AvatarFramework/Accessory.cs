using System;
using UnityEngine;

namespace ScheduleOne.AvatarFramework
{
	// Token: 0x02000999 RID: 2457
	public class Accessory : MonoBehaviour
	{
		// Token: 0x06004257 RID: 16983 RVA: 0x00116E40 File Offset: 0x00115040
		private void Awake()
		{
			for (int i = 0; i < this.skinnedMeshesToBind.Length; i++)
			{
				this.skinnedMeshesToBind[i].updateWhenOffscreen = true;
			}
		}

		// Token: 0x06004258 RID: 16984 RVA: 0x00116E70 File Offset: 0x00115070
		public void ApplyColor(Color col)
		{
			foreach (MeshRenderer meshRenderer in this.meshesToColor)
			{
				for (int j = 0; j < meshRenderer.materials.Length; j++)
				{
					meshRenderer.materials[j].color = col;
					if (!this.ColorAllMeshes)
					{
						break;
					}
				}
			}
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in this.skinnedMeshesToColor)
			{
				for (int k = 0; k < skinnedMeshRenderer.materials.Length; k++)
				{
					skinnedMeshRenderer.materials[k].color = col;
					if (!this.ColorAllMeshes)
					{
						break;
					}
				}
			}
		}

		// Token: 0x06004259 RID: 16985 RVA: 0x00116F0C File Offset: 0x0011510C
		public void ApplyShapeKeys(float gender, float weight)
		{
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in this.shapeKeyMeshRends)
			{
				if (skinnedMeshRenderer.sharedMesh.blendShapeCount >= 2)
				{
					skinnedMeshRenderer.SetBlendShapeWeight(0, gender);
					skinnedMeshRenderer.SetBlendShapeWeight(1, weight);
				}
			}
		}

		// Token: 0x0600425A RID: 16986 RVA: 0x00116F50 File Offset: 0x00115150
		public void BindBones(Transform[] bones)
		{
			SkinnedMeshRenderer[] array = this.skinnedMeshesToBind;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].bones = bones;
			}
		}

		// Token: 0x04002F2A RID: 12074
		[Header("Settings")]
		public string Name;

		// Token: 0x04002F2B RID: 12075
		public string AssetPath;

		// Token: 0x04002F2C RID: 12076
		public bool ReduceFootSize;

		// Token: 0x04002F2D RID: 12077
		[Range(0f, 1f)]
		public float FootSizeReduction = 1f;

		// Token: 0x04002F2E RID: 12078
		public bool ShouldBlockHair;

		// Token: 0x04002F2F RID: 12079
		public bool ColorAllMeshes = true;

		// Token: 0x04002F30 RID: 12080
		[Header("References")]
		public MeshRenderer[] meshesToColor;

		// Token: 0x04002F31 RID: 12081
		public SkinnedMeshRenderer[] skinnedMeshesToColor;

		// Token: 0x04002F32 RID: 12082
		public SkinnedMeshRenderer[] skinnedMeshesToBind;

		// Token: 0x04002F33 RID: 12083
		public SkinnedMeshRenderer[] shapeKeyMeshRends;
	}
}
