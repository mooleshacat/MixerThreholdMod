using System;
using System.Collections.Generic;
using FishNet.Object;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.ItemFramework;
using ScheduleOne.Storage;
using ScheduleOne.Tiles;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace ScheduleOne.Building
{
	// Token: 0x020007BD RID: 1981
	public class BuildManager : Singleton<BuildManager>
	{
		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x060035CC RID: 13772 RVA: 0x000E11CE File Offset: 0x000DF3CE
		public Transform _tempContainer
		{
			get
			{
				return this.tempContainer;
			}
		}

		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x060035CD RID: 13773 RVA: 0x000E11D6 File Offset: 0x000DF3D6
		// (set) Token: 0x060035CE RID: 13774 RVA: 0x000E11DE File Offset: 0x000DF3DE
		public bool isBuilding { get; protected set; }

		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x060035CF RID: 13775 RVA: 0x000E11E7 File Offset: 0x000DF3E7
		// (set) Token: 0x060035D0 RID: 13776 RVA: 0x000E11EF File Offset: 0x000DF3EF
		public GameObject currentBuildHandler { get; protected set; }

		// Token: 0x060035D1 RID: 13777 RVA: 0x000E11F8 File Offset: 0x000DF3F8
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x060035D2 RID: 13778 RVA: 0x000E1200 File Offset: 0x000DF400
		public void StartBuilding(ItemInstance item)
		{
			if (!(item.Definition is BuildableItemDefinition))
			{
				Console.LogError("StartBuilding called but not passed BuildableItemDefinition", null);
				return;
			}
			if (this.isBuilding)
			{
				Console.LogWarning("StartBuilding called but building is already happening!", null);
				this.StopBuilding();
			}
			BuildableItem builtItem = (item.Definition as BuildableItemDefinition).BuiltItem;
			if (builtItem == null)
			{
				Console.LogWarning("itemToBuild is null!", null);
				return;
			}
			this.isBuilding = true;
			this.currentBuildHandler = UnityEngine.Object.Instantiate<GameObject>(builtItem.BuildHandler, this.tempContainer);
			this.currentBuildHandler.GetComponent<BuildStart_Base>().StartBuilding(item);
		}

		// Token: 0x060035D3 RID: 13779 RVA: 0x000E1294 File Offset: 0x000DF494
		public void StartBuildingStoredItem(ItemInstance item)
		{
			if (!(item.Definition is StorableItemDefinition))
			{
				Console.LogError("StartBuildingStoredItem called but not passed StorableItemDefinition", null);
				return;
			}
			if (this.isBuilding)
			{
				Console.LogWarning("StartBuildingStoredItem called but building is already happening!", null);
				this.StopBuilding();
			}
			this.isBuilding = true;
			this.currentBuildHandler = UnityEngine.Object.Instantiate<GameObject>(this.storedItemBuildHandler, this.tempContainer);
			this.currentBuildHandler.GetComponent<BuildStart_Base>().StartBuilding(item);
		}

		// Token: 0x060035D4 RID: 13780 RVA: 0x000E1304 File Offset: 0x000DF504
		public void StartPlacingCash(ItemInstance item)
		{
			if (this.isBuilding)
			{
				Console.LogWarning("StartPlacingCash called but building is already happening!", null);
				this.StopBuilding();
			}
			this.isBuilding = true;
			this.currentBuildHandler = UnityEngine.Object.Instantiate<GameObject>(this.cashBuildHandler, this.tempContainer);
			this.currentBuildHandler.GetComponent<BuildStart_Cash>().StartBuilding(item);
		}

		// Token: 0x060035D5 RID: 13781 RVA: 0x000E1359 File Offset: 0x000DF559
		public void StopBuilding()
		{
			this.isBuilding = false;
			this.currentBuildHandler.GetComponent<BuildStop_Base>().Stop_Building();
		}

		// Token: 0x060035D6 RID: 13782 RVA: 0x000E1374 File Offset: 0x000DF574
		public void PlayBuildSound(BuildableItemDefinition.EBuildSoundType type, Vector3 point)
		{
			BuildManager.BuildSound buildSound = this.PlaceSounds.Find((BuildManager.BuildSound s) => s.Type == type);
			if (buildSound != null)
			{
				buildSound.Sound.transform.position = point;
				buildSound.Sound.Play();
			}
		}

		// Token: 0x060035D7 RID: 13783 RVA: 0x000E13C8 File Offset: 0x000DF5C8
		public void DisableColliders(GameObject obj)
		{
			Collider[] componentsInChildren = obj.GetComponentsInChildren<Collider>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
		}

		// Token: 0x060035D8 RID: 13784 RVA: 0x000E13F4 File Offset: 0x000DF5F4
		public void DisableLights(GameObject obj)
		{
			OptimizedLight[] componentsInChildren = obj.GetComponentsInChildren<OptimizedLight>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Enabled = false;
			}
			Light[] componentsInChildren2 = obj.GetComponentsInChildren<Light>();
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				componentsInChildren2[j].enabled = false;
			}
		}

		// Token: 0x060035D9 RID: 13785 RVA: 0x000E1440 File Offset: 0x000DF640
		public void DisableNetworking(GameObject obj)
		{
			NetworkObject[] componentsInChildren = obj.GetComponentsInChildren<NetworkObject>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				UnityEngine.Object.Destroy(componentsInChildren[i]);
			}
		}

		// Token: 0x060035DA RID: 13786 RVA: 0x000E146C File Offset: 0x000DF66C
		public void DisableSpriteRenderers(GameObject obj)
		{
			SpriteRenderer[] componentsInChildren = obj.GetComponentsInChildren<SpriteRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
		}

		// Token: 0x060035DB RID: 13787 RVA: 0x000E1498 File Offset: 0x000DF698
		public void ApplyMaterial(GameObject obj, Material mat, bool allMaterials = true)
		{
			MeshRenderer[] componentsInChildren = obj.GetComponentsInChildren<MeshRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (!componentsInChildren[i].gameObject.GetComponentInParent<OverrideGhostMaterial>())
				{
					if (allMaterials)
					{
						Material[] materials = componentsInChildren[i].materials;
						for (int j = 0; j < materials.Length; j++)
						{
							materials[j] = mat;
						}
						componentsInChildren[i].materials = materials;
					}
					else
					{
						componentsInChildren[i].material = mat;
					}
				}
			}
		}

		// Token: 0x060035DC RID: 13788 RVA: 0x000E1500 File Offset: 0x000DF700
		public void DisableNavigation(GameObject obj)
		{
			NavMeshObstacle[] componentsInChildren = obj.GetComponentsInChildren<NavMeshObstacle>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
			NavMeshSurface[] componentsInChildren2 = obj.GetComponentsInChildren<NavMeshSurface>();
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				componentsInChildren2[j].enabled = false;
			}
			NavMeshLink[] componentsInChildren3 = obj.GetComponentsInChildren<NavMeshLink>();
			for (int k = 0; k < componentsInChildren3.Length; k++)
			{
				componentsInChildren3[k].enabled = false;
			}
		}

		// Token: 0x060035DD RID: 13789 RVA: 0x000E1574 File Offset: 0x000DF774
		public void DisableCanvases(GameObject obj)
		{
			Canvas[] componentsInChildren = obj.GetComponentsInChildren<Canvas>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
		}

		// Token: 0x060035DE RID: 13790 RVA: 0x000E15A0 File Offset: 0x000DF7A0
		public GridItem CreateGridItem(ItemInstance item, Grid grid, Vector2 originCoordinate, int rotation, string guid = "")
		{
			BuildableItemDefinition buildableItemDefinition = item.Definition as BuildableItemDefinition;
			if (buildableItemDefinition == null)
			{
				Console.LogError("BuildGridItem called but could not find BuildableItemDefinition", null);
				return null;
			}
			if (grid == null)
			{
				Console.LogError("BuildGridItem called and passed null grid", null);
				return null;
			}
			string guid2 = string.IsNullOrEmpty(guid) ? GUIDManager.GenerateUniqueGUID().ToString() : guid;
			GridItem component = UnityEngine.Object.Instantiate<GameObject>(buildableItemDefinition.BuiltItem.gameObject, null).GetComponent<GridItem>();
			component.SetLocallyBuilt();
			component.InitializeGridItem(item, grid, originCoordinate, rotation, guid2);
			this.networkObject.Spawn(component.gameObject, null, default(Scene));
			return component;
		}

		// Token: 0x060035DF RID: 13791 RVA: 0x000E164C File Offset: 0x000DF84C
		public ProceduralGridItem CreateProceduralGridItem(ItemInstance item, int rotationAngle, List<CoordinateProceduralTilePair> matches, string guid = "")
		{
			BuildableItemDefinition buildableItemDefinition = item.Definition as BuildableItemDefinition;
			if (buildableItemDefinition == null)
			{
				Console.LogError("BuildProceduralGridItem called but could not find BuildableItemDefinition", null);
				return null;
			}
			string guid2 = string.IsNullOrEmpty(guid) ? GUIDManager.GenerateUniqueGUID().ToString() : guid;
			ProceduralGridItem component = UnityEngine.Object.Instantiate<GameObject>(buildableItemDefinition.BuiltItem.gameObject, null).GetComponent<ProceduralGridItem>();
			component.SetLocallyBuilt();
			component.InitializeProceduralGridItem(item, rotationAngle, matches, guid2);
			this.networkObject.Spawn(component.gameObject, null, default(Scene));
			return component;
		}

		// Token: 0x060035E0 RID: 13792 RVA: 0x000E16E0 File Offset: 0x000DF8E0
		public SurfaceItem CreateSurfaceItem(ItemInstance item, Surface parentSurface, Vector3 relativePosition, Quaternion relativeRotation, string guid = "")
		{
			BuildableItemDefinition buildableItemDefinition = item.Definition as BuildableItemDefinition;
			if (buildableItemDefinition == null)
			{
				Console.LogError("CreateSurfaceItem called but could not find BuildableItemDefinition", null);
				return null;
			}
			string guid2 = string.IsNullOrEmpty(guid) ? GUIDManager.GenerateUniqueGUID().ToString() : guid;
			SurfaceItem component = UnityEngine.Object.Instantiate<GameObject>(buildableItemDefinition.BuiltItem.gameObject, null).GetComponent<SurfaceItem>();
			component.SetLocallyBuilt();
			component.InitializeSurfaceItem(item, guid2, parentSurface.GUID.ToString(), relativePosition, relativeRotation);
			this.networkObject.Spawn(component.gameObject, null, default(Scene));
			return component;
		}

		// Token: 0x060035E1 RID: 13793 RVA: 0x000E1789 File Offset: 0x000DF989
		public void CreateStoredItem(StorableItemInstance item, IStorageEntity parentStorageEntity, StorageGrid grid, Vector2 originCoord, float rotation)
		{
			if (parentStorageEntity == null)
			{
				Console.LogWarning("CreateStoredItem: parentStorageEntity is null", null);
				return;
			}
			if (item.Quantity != 1)
			{
				Console.LogWarning("CreateStoredItem: item quantity is '" + item.Quantity.ToString() + "'. It should be 1!", null);
			}
		}

		// Token: 0x04002625 RID: 9765
		public List<BuildManager.BuildSound> PlaceSounds = new List<BuildManager.BuildSound>();

		// Token: 0x04002626 RID: 9766
		[Header("References")]
		[SerializeField]
		protected Transform tempContainer;

		// Token: 0x04002627 RID: 9767
		public NetworkObject networkObject;

		// Token: 0x04002628 RID: 9768
		[Header("Prefabs")]
		[SerializeField]
		protected GameObject storedItemBuildHandler;

		// Token: 0x04002629 RID: 9769
		[SerializeField]
		protected GameObject cashBuildHandler;

		// Token: 0x0400262A RID: 9770
		[Header("Materials")]
		public Material ghostMaterial_White;

		// Token: 0x0400262B RID: 9771
		public Material ghostMaterial_Red;

		// Token: 0x020007BE RID: 1982
		[Serializable]
		public class BuildSound
		{
			// Token: 0x0400262E RID: 9774
			public BuildableItemDefinition.EBuildSoundType Type;

			// Token: 0x0400262F RID: 9775
			public AudioSourceController Sound;
		}
	}
}
