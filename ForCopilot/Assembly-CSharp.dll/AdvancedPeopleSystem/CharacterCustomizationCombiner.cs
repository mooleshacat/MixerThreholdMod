using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace AdvancedPeopleSystem
{
	// Token: 0x0200020D RID: 525
	public class CharacterCustomizationCombiner
	{
		// Token: 0x06000BA5 RID: 2981 RVA: 0x00034FB4 File Offset: 0x000331B4
		public static List<SkinnedMeshRenderer> MakeCombinedMeshes(CharacterCustomization character, GameObject exportInCustomObject = null, float blendshapeAddDelay = 0.001f, Action<List<SkinnedMeshRenderer>> callback = null)
		{
			CharacterCustomizationCombiner.returnSkinnedMeshes.Clear();
			if (character.IsBaked())
			{
				Debug.LogError("Character is already combined!");
				return null;
			}
			if (callback != null)
			{
				CharacterCustomizationCombiner._callback = callback;
			}
			CharacterCustomizationCombiner.BlendshapeTransferWork = false;
			CharacterCustomizationCombiner.useExportToAnotherObject = (exportInCustomObject != null);
			if (!CharacterCustomizationCombiner.useExportToAnotherObject)
			{
				character.CurrentCombinerState = CombinerState.InProgressCombineMesh;
			}
			CharacterCustomizationCombiner.currentCharacter = character;
			CharacterCustomizationCombiner.bindPoses = character.GetCharacterPart("Head").skinnedMesh[0].sharedMesh.bindposes;
			CharacterCustomizationCombiner.LODMeshInstances = new List<CharacterCustomizationCombiner.MeshInstance>();
			for (int i = 0; i < character.MaxLODLevels - character.MinLODLevels + 1; i++)
			{
				CharacterCustomizationCombiner.LODMeshInstances.Add(new CharacterCustomizationCombiner.MeshInstance());
				foreach (SkinnedMeshRenderer mesh in character.GetAllMeshesByLod(i))
				{
					CharacterCustomizationCombiner.<MakeCombinedMeshes>g__SelectMeshes|10_0(mesh, i);
				}
			}
			SkinnedMeshRenderer original = character.GetCharacterPart("Combined").skinnedMesh[0];
			List<SkinnedMeshRenderer> list = character.GetCharacterPart("Combined").skinnedMesh;
			if (exportInCustomObject != null)
			{
				List<SkinnedMeshRenderer> list2 = new List<SkinnedMeshRenderer>();
				for (int j = 0; j < character.MaxLODLevels - character.MinLODLevels + 1; j++)
				{
					SkinnedMeshRenderer item = UnityEngine.Object.Instantiate<SkinnedMeshRenderer>(original, exportInCustomObject.transform);
					list2.Add(item);
				}
				list = list2;
			}
			for (int k = 0; k < CharacterCustomizationCombiner.LODMeshInstances.Count; k++)
			{
				CharacterCustomizationCombiner.MeshInstance meshInstance = CharacterCustomizationCombiner.LODMeshInstances[k];
				for (int l = 0; l < meshInstance.unique_materials.Count; l++)
				{
					Material key = meshInstance.unique_materials[l];
					List<CharacterCustomizationCombiner.CombineInstanceWithSM> list3 = meshInstance.combine_instances[meshInstance.unique_materials[l]];
					for (int m = 0; m < list3.Count; m++)
					{
						CharacterCustomizationCombiner.CombineInstanceWithSM combineInstanceWithSM = list3[m];
						if (!meshInstance.vertex_offset_map.ContainsKey(combineInstanceWithSM.instance.mesh))
						{
							meshInstance.combined_vertices.AddRange(combineInstanceWithSM.instance.mesh.vertices);
							if (combineInstanceWithSM.instance.mesh.uv.Length == 0)
							{
								meshInstance.combined_uv.AddRange(new Vector2[combineInstanceWithSM.instance.mesh.vertexCount]);
							}
							else
							{
								meshInstance.combined_uv.AddRange(combineInstanceWithSM.instance.mesh.uv);
							}
							if (combineInstanceWithSM.instance.mesh.uv2.Length == 0)
							{
								meshInstance.combined_uv2.AddRange(new Vector2[combineInstanceWithSM.instance.mesh.vertexCount]);
							}
							else
							{
								meshInstance.combined_uv2.AddRange(combineInstanceWithSM.instance.mesh.uv2);
							}
							if (combineInstanceWithSM.instance.mesh.uv3.Length == 0)
							{
								meshInstance.combined_uv3.AddRange(new Vector2[combineInstanceWithSM.instance.mesh.vertexCount]);
							}
							else
							{
								meshInstance.combined_uv3.AddRange(combineInstanceWithSM.instance.mesh.uv3);
							}
							meshInstance.normals.AddRange(combineInstanceWithSM.instance.mesh.normals);
							meshInstance.combined_bone_weights.AddRange(combineInstanceWithSM.instance.mesh.boneWeights);
							meshInstance.vertex_offset_map[combineInstanceWithSM.instance.mesh] = meshInstance.vertex_index_offset;
							meshInstance.vertex_index_offset += combineInstanceWithSM.instance.mesh.vertexCount;
						}
						int num = meshInstance.vertex_offset_map[combineInstanceWithSM.instance.mesh];
						int[] triangles = combineInstanceWithSM.instance.mesh.GetTriangles(combineInstanceWithSM.instance.subMeshIndex);
						for (int n = 0; n < triangles.Length; n++)
						{
							triangles[n] += num;
						}
						if (!meshInstance.combined_submesh_indices.ContainsKey(key))
						{
							meshInstance.combined_submesh_indices.Add(key, triangles.ToList<int>());
						}
						else
						{
							meshInstance.combined_submesh_indices[key].AddRange(triangles);
						}
						for (int num2 = 0; num2 < combineInstanceWithSM.instance.mesh.blendShapeCount; num2++)
						{
							string blendShapeName = combineInstanceWithSM.instance.mesh.GetBlendShapeName(num2);
							if (!meshInstance.blendShapeNames.Contains(blendShapeName))
							{
								meshInstance.blendShapeNames.Add(blendShapeName);
								meshInstance.blendShapeValues.Add(combineInstanceWithSM.skinnedMesh.GetBlendShapeWeight(num2));
							}
						}
					}
				}
				meshInstance.combined_new_mesh.vertices = meshInstance.combined_vertices.ToArray();
				meshInstance.combined_new_mesh.uv = meshInstance.combined_uv.ToArray();
				if (meshInstance.combined_uv2.Count > 0)
				{
					meshInstance.combined_new_mesh.uv2 = meshInstance.combined_uv2.ToArray();
				}
				if (meshInstance.combined_uv3.Count > 0)
				{
					meshInstance.combined_new_mesh.uv3 = meshInstance.combined_uv3.ToArray();
				}
				if (meshInstance.combined_uv4.Count > 0)
				{
					meshInstance.combined_new_mesh.uv4 = meshInstance.combined_uv4.ToArray();
				}
				meshInstance.combined_new_mesh.boneWeights = meshInstance.combined_bone_weights.ToArray();
				meshInstance.combined_new_mesh.name = string.Format("APP_CombinedMesh_lod{0}", k);
				meshInstance.combined_new_mesh.subMeshCount = meshInstance.unique_materials.Count;
				for (int num3 = 0; num3 < meshInstance.unique_materials.Count; num3++)
				{
					meshInstance.combined_new_mesh.SetTriangles(meshInstance.combined_submesh_indices[meshInstance.unique_materials[num3]], num3);
				}
				meshInstance.combined_new_mesh.SetNormals(meshInstance.normals);
				meshInstance.combined_new_mesh.RecalculateTangents();
				if (!CharacterCustomizationCombiner.useExportToAnotherObject && character.CurrentCombinerState != CombinerState.InProgressBlendshapeTransfer)
				{
					character.CurrentCombinerState = CombinerState.InProgressBlendshapeTransfer;
				}
				character.StartCoroutine(CharacterCustomizationCombiner.BlendshapeTransfer(meshInstance, blendshapeAddDelay, list[k], k, exportInCustomObject == null));
			}
			for (int num4 = 0; num4 < list.Count; num4++)
			{
				list[num4].name = string.Format("combinemesh_lod{0}", num4);
				list[num4].sharedMesh = CharacterCustomizationCombiner.LODMeshInstances[num4].combined_new_mesh;
				list[num4].sharedMesh.bindposes = CharacterCustomizationCombiner.bindPoses;
				list[num4].sharedMaterials = CharacterCustomizationCombiner.LODMeshInstances[num4].unique_materials.ToArray();
				list[num4].updateWhenOffscreen = true;
			}
			CharacterCustomizationCombiner.returnSkinnedMeshes.AddRange(list);
			CharacterCustomizationCombiner.BlendshapeTransferWork = true;
			return CharacterCustomizationCombiner.returnSkinnedMeshes;
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x000356B4 File Offset: 0x000338B4
		private static IEnumerator BlendshapeTransfer(CharacterCustomizationCombiner.MeshInstance meshInstance, float waitTime, SkinnedMeshRenderer smr, int lod, bool yieldUse = true)
		{
			yield return new WaitWhile(() => !CharacterCustomizationCombiner.BlendshapeTransferWork);
			CharacterCustomization characterSystem = CharacterCustomizationCombiner.currentCharacter;
			int num2;
			for (int bs = 0; bs < meshInstance.blendShapeNames.Count; bs = num2 + 1)
			{
				int num = 0;
				CharacterCustomizationCombiner.BlendWeightData blendWeightData = default(CharacterCustomizationCombiner.BlendWeightData);
				blendWeightData.deltaNormals = new Vector3[meshInstance.combined_new_mesh.vertexCount];
				blendWeightData.deltaTangents = new Vector3[meshInstance.combined_new_mesh.vertexCount];
				blendWeightData.deltaVerts = new Vector3[meshInstance.combined_new_mesh.vertexCount];
				foreach (KeyValuePair<Material, List<CharacterCustomizationCombiner.CombineInstanceWithSM>> keyValuePair in meshInstance.combine_instances)
				{
					foreach (CharacterCustomizationCombiner.CombineInstanceWithSM combineInstanceWithSM in keyValuePair.Value)
					{
						CombineInstance instance = combineInstanceWithSM.instance;
						if (instance.subMeshIndex <= 0)
						{
							instance = combineInstanceWithSM.instance;
							int vertexCount = instance.mesh.vertexCount;
							Vector3[] array = new Vector3[vertexCount];
							Vector3[] array2 = new Vector3[vertexCount];
							Vector3[] array3 = new Vector3[vertexCount];
							instance = combineInstanceWithSM.instance;
							if (instance.mesh.GetBlendShapeIndex(meshInstance.blendShapeNames[bs]) != -1)
							{
								instance = combineInstanceWithSM.instance;
								int blendShapeIndex = instance.mesh.GetBlendShapeIndex(meshInstance.blendShapeNames[bs]);
								instance = combineInstanceWithSM.instance;
								Mesh mesh = instance.mesh;
								int shapeIndex = blendShapeIndex;
								instance = combineInstanceWithSM.instance;
								mesh.GetBlendShapeFrameVertices(shapeIndex, instance.mesh.GetBlendShapeFrameCount(blendShapeIndex) - 1, array, array2, array3);
								Array.Copy(array, 0, blendWeightData.deltaVerts, num, vertexCount);
								Array.Copy(array2, 0, blendWeightData.deltaNormals, num, vertexCount);
								Array.Copy(array3, 0, blendWeightData.deltaTangents, num, vertexCount);
							}
							num += vertexCount;
						}
					}
				}
				smr.sharedMesh.AddBlendShapeFrame(meshInstance.blendShapeNames[bs], 100f, blendWeightData.deltaVerts, blendWeightData.deltaNormals, blendWeightData.deltaTangents);
				smr.SetBlendShapeWeight(bs, meshInstance.blendShapeValues[bs]);
				if (waitTime > 0f && yieldUse)
				{
					yield return new WaitForSecondsRealtime(waitTime);
				}
				num2 = bs;
			}
			if (lod == characterSystem.MaxLODLevels - characterSystem.MinLODLevels)
			{
				if (!CharacterCustomizationCombiner.useExportToAnotherObject)
				{
					characterSystem.CurrentCombinerState = CombinerState.Combined;
				}
				Action<List<SkinnedMeshRenderer>> callback = CharacterCustomizationCombiner._callback;
				if (callback != null)
				{
					callback(CharacterCustomizationCombiner.returnSkinnedMeshes);
				}
				CharacterCustomizationCombiner._callback = null;
			}
			yield break;
		}

		// Token: 0x06000BA9 RID: 2985 RVA: 0x000356F8 File Offset: 0x000338F8
		[CompilerGenerated]
		internal static void <MakeCombinedMeshes>g__SelectMeshes|10_0(SkinnedMeshRenderer mesh, int LOD)
		{
			if (mesh != null)
			{
				for (int i = 0; i < mesh.sharedMaterials.Length; i++)
				{
					Material material = mesh.sharedMaterials[i];
					Mesh sharedMesh = mesh.sharedMesh;
					if (!(sharedMesh == null) && mesh.gameObject.activeSelf && mesh.enabled && sharedMesh.vertexCount != 0 && sharedMesh.subMeshCount - 1 >= i)
					{
						if (!CharacterCustomizationCombiner.LODMeshInstances[LOD].combine_instances.ContainsKey(material))
						{
							CharacterCustomizationCombiner.LODMeshInstances[LOD].combine_instances.Add(material, new List<CharacterCustomizationCombiner.CombineInstanceWithSM>());
							CharacterCustomizationCombiner.LODMeshInstances[LOD].unique_materials.Add(material);
						}
						CharacterCustomizationCombiner.CombineInstanceWithSM item = default(CharacterCustomizationCombiner.CombineInstanceWithSM);
						item.instance = new CombineInstance
						{
							transform = Matrix4x4.identity,
							subMeshIndex = i,
							mesh = sharedMesh
						};
						item.skinnedMesh = mesh;
						CharacterCustomizationCombiner.LODMeshInstances[LOD].combine_instances[material].Add(item);
					}
				}
			}
		}

		// Token: 0x04000C3C RID: 3132
		private static Matrix4x4[] bindPoses;

		// Token: 0x04000C3D RID: 3133
		private static List<CharacterCustomizationCombiner.MeshInstance> LODMeshInstances;

		// Token: 0x04000C3E RID: 3134
		private static CharacterCustomization currentCharacter;

		// Token: 0x04000C3F RID: 3135
		private static bool useExportToAnotherObject = false;

		// Token: 0x04000C40 RID: 3136
		private static bool BlendshapeTransferWork = false;

		// Token: 0x04000C41 RID: 3137
		private static Action<List<SkinnedMeshRenderer>> _callback;

		// Token: 0x04000C42 RID: 3138
		private static List<SkinnedMeshRenderer> returnSkinnedMeshes = new List<SkinnedMeshRenderer>();

		// Token: 0x0200020E RID: 526
		private class MeshInstance
		{
			// Token: 0x04000C43 RID: 3139
			public Dictionary<Material, List<CharacterCustomizationCombiner.CombineInstanceWithSM>> combine_instances = new Dictionary<Material, List<CharacterCustomizationCombiner.CombineInstanceWithSM>>();

			// Token: 0x04000C44 RID: 3140
			public List<Material> unique_materials = new List<Material>();

			// Token: 0x04000C45 RID: 3141
			public Mesh combined_new_mesh = new Mesh();

			// Token: 0x04000C46 RID: 3142
			public List<Vector3> combined_vertices = new List<Vector3>();

			// Token: 0x04000C47 RID: 3143
			public List<Vector2> combined_uv = new List<Vector2>();

			// Token: 0x04000C48 RID: 3144
			public List<Vector2> combined_uv2 = new List<Vector2>();

			// Token: 0x04000C49 RID: 3145
			public List<Vector2> combined_uv3 = new List<Vector2>();

			// Token: 0x04000C4A RID: 3146
			public List<Vector2> combined_uv4 = new List<Vector2>();

			// Token: 0x04000C4B RID: 3147
			public List<Vector3> normals = new List<Vector3>();

			// Token: 0x04000C4C RID: 3148
			public List<Vector4> tangents = new List<Vector4>();

			// Token: 0x04000C4D RID: 3149
			public Dictionary<Material, List<int>> combined_submesh_indices = new Dictionary<Material, List<int>>();

			// Token: 0x04000C4E RID: 3150
			public List<BoneWeight> combined_bone_weights = new List<BoneWeight>();

			// Token: 0x04000C4F RID: 3151
			public List<string> blendShapeNames = new List<string>();

			// Token: 0x04000C50 RID: 3152
			public List<float> blendShapeValues = new List<float>();

			// Token: 0x04000C51 RID: 3153
			public Dictionary<Mesh, int> vertex_offset_map = new Dictionary<Mesh, int>();

			// Token: 0x04000C52 RID: 3154
			public int vertex_index_offset;

			// Token: 0x04000C53 RID: 3155
			public int current_material_index;
		}

		// Token: 0x0200020F RID: 527
		private struct CombineInstanceWithSM
		{
			// Token: 0x04000C54 RID: 3156
			public SkinnedMeshRenderer skinnedMesh;

			// Token: 0x04000C55 RID: 3157
			public CombineInstance instance;
		}

		// Token: 0x02000210 RID: 528
		private struct BlendWeightData
		{
			// Token: 0x04000C56 RID: 3158
			public Vector3[] deltaVerts;

			// Token: 0x04000C57 RID: 3159
			public Vector3[] deltaNormals;

			// Token: 0x04000C58 RID: 3160
			public Vector3[] deltaTangents;
		}
	}
}
