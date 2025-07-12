using System;
using System.Collections.Generic;
using System.Linq;
using EasyButtons;
using EPOOutline;
using FishNet;
using FishNet.Component.Ownership;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Building;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.EntityFramework
{
	// Token: 0x02000660 RID: 1632
	[RequireComponent(typeof(PredictedSpawn))]
	public class BuildableItem : NetworkBehaviour, IGUIDRegisterable, ISaveable
	{
		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x06002A53 RID: 10835 RVA: 0x000AF0B7 File Offset: 0x000AD2B7
		// (set) Token: 0x06002A54 RID: 10836 RVA: 0x000AF0BF File Offset: 0x000AD2BF
		public ItemInstance ItemInstance { get; protected set; }

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x06002A55 RID: 10837 RVA: 0x000AF0C8 File Offset: 0x000AD2C8
		// (set) Token: 0x06002A56 RID: 10838 RVA: 0x000AF0D0 File Offset: 0x000AD2D0
		public Property ParentProperty { get; protected set; }

		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x06002A57 RID: 10839 RVA: 0x000AF0D9 File Offset: 0x000AD2D9
		// (set) Token: 0x06002A58 RID: 10840 RVA: 0x000AF0E1 File Offset: 0x000AD2E1
		public bool IsDestroyed { get; protected set; }

		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x06002A59 RID: 10841 RVA: 0x000AF0EA File Offset: 0x000AD2EA
		// (set) Token: 0x06002A5A RID: 10842 RVA: 0x000AF0F2 File Offset: 0x000AD2F2
		public bool Initialized { get; protected set; }

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x06002A5B RID: 10843 RVA: 0x000AF0FB File Offset: 0x000AD2FB
		// (set) Token: 0x06002A5C RID: 10844 RVA: 0x000AF103 File Offset: 0x000AD303
		public Guid GUID { get; protected set; }

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x06002A5D RID: 10845 RVA: 0x000AF10C File Offset: 0x000AD30C
		// (set) Token: 0x06002A5E RID: 10846 RVA: 0x000AF114 File Offset: 0x000AD314
		public bool IsCulled { get; protected set; }

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x06002A5F RID: 10847 RVA: 0x000AF11D File Offset: 0x000AD31D
		public GameObject BuildHandler
		{
			get
			{
				return this.buildHandler;
			}
		}

		// Token: 0x06002A60 RID: 10848 RVA: 0x000AF128 File Offset: 0x000AD328
		[Button]
		public void AddChildMeshes()
		{
			foreach (MeshRenderer meshRenderer in new List<MeshRenderer>(this.MeshesToCull))
			{
				foreach (MeshRenderer item in meshRenderer.GetComponentsInChildren<MeshRenderer>())
				{
					if (!this.MeshesToCull.Contains(item))
					{
						this.MeshesToCull.Add(item);
					}
				}
			}
		}

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x06002A61 RID: 10849 RVA: 0x000AF1AC File Offset: 0x000AD3AC
		// (set) Token: 0x06002A62 RID: 10850 RVA: 0x000AF1B4 File Offset: 0x000AD3B4
		public bool LocallyBuilt { get; protected set; }

		// Token: 0x06002A63 RID: 10851 RVA: 0x000AF1BD File Offset: 0x000AD3BD
		public void SetLocallyBuilt()
		{
			this.LocallyBuilt = true;
		}

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x06002A64 RID: 10852 RVA: 0x000AF1C8 File Offset: 0x000AD3C8
		public string SaveFolderName
		{
			get
			{
				return this.ItemInstance.ID + "_" + this.GUID.ToString().Substring(0, 6);
			}
		}

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x06002A65 RID: 10853 RVA: 0x000AF205 File Offset: 0x000AD405
		public string SaveFileName
		{
			get
			{
				return "Data";
			}
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x06002A66 RID: 10854 RVA: 0x00047DCE File Offset: 0x00045FCE
		public Loader Loader
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x06002A67 RID: 10855 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x06002A68 RID: 10856 RVA: 0x000AF20C File Offset: 0x000AD40C
		// (set) Token: 0x06002A69 RID: 10857 RVA: 0x000AF214 File Offset: 0x000AD414
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x06002A6A RID: 10858 RVA: 0x000AF21D File Offset: 0x000AD41D
		// (set) Token: 0x06002A6B RID: 10859 RVA: 0x000AF225 File Offset: 0x000AD425
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x06002A6C RID: 10860 RVA: 0x000AF22E File Offset: 0x000AD42E
		// (set) Token: 0x06002A6D RID: 10861 RVA: 0x000AF236 File Offset: 0x000AD436
		public bool HasChanged { get; set; }

		// Token: 0x06002A6E RID: 10862 RVA: 0x000AF23F File Offset: 0x000AD43F
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.EntityFramework.BuildableItem_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002A6F RID: 10863 RVA: 0x000AF254 File Offset: 0x000AD454
		protected virtual void Start()
		{
			if (!this.isGhost)
			{
				this.InitializeSaveable();
				if (this.GUID == Guid.Empty)
				{
					this.GUID = GUIDManager.GenerateUniqueGUID();
					GUIDManager.RegisterObject(this);
				}
				ActivateDuringBuild[] componentsInChildren = base.transform.GetComponentsInChildren<ActivateDuringBuild>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x06002A70 RID: 10864 RVA: 0x000AF2BC File Offset: 0x000AD4BC
		protected virtual Property GetProperty(Transform searchTransform = null)
		{
			if (searchTransform == null)
			{
				searchTransform = base.transform;
			}
			PropertyContentsContainer componentInParent = searchTransform.GetComponentInParent<PropertyContentsContainer>();
			if (componentInParent != null)
			{
				return componentInParent.Property;
			}
			return searchTransform.GetComponentInParent<Property>();
		}

		// Token: 0x06002A71 RID: 10865 RVA: 0x0003DA73 File Offset: 0x0003BC73
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x06002A72 RID: 10866 RVA: 0x000AF2F7 File Offset: 0x000AD4F7
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (!connection.IsLocalClient && this.Initialized)
			{
				this.SendInitToClient(connection);
			}
		}

		// Token: 0x06002A73 RID: 10867 RVA: 0x000AF318 File Offset: 0x000AD518
		protected virtual void SendInitToClient(NetworkConnection conn)
		{
			Console.Log("Sending BuildableItem init to client", null);
			this.ReceiveBuildableItemData(conn, this.ItemInstance, this.GUID.ToString(), this.ParentProperty.PropertyCode);
		}

		// Token: 0x06002A74 RID: 10868 RVA: 0x000AF35C File Offset: 0x000AD55C
		[ServerRpc(RequireOwnership = false)]
		public void SendBuildableItemData(ItemInstance instance, string GUID, string parentPropertyCode)
		{
			this.RpcWriter___Server_SendBuildableItemData_3537728543(instance, GUID, parentPropertyCode);
		}

		// Token: 0x06002A75 RID: 10869 RVA: 0x000AF370 File Offset: 0x000AD570
		[ObserversRpc]
		[TargetRpc]
		public void ReceiveBuildableItemData(NetworkConnection conn, ItemInstance instance, string GUID, string parentPropertyCode)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_ReceiveBuildableItemData_3859851844(conn, instance, GUID, parentPropertyCode);
			}
			else
			{
				this.RpcWriter___Target_ReceiveBuildableItemData_3859851844(conn, instance, GUID, parentPropertyCode);
			}
		}

		// Token: 0x06002A76 RID: 10870 RVA: 0x000AF3A8 File Offset: 0x000AD5A8
		public virtual void InitializeBuildableItem(ItemInstance instance, string GUID, string parentPropertyCode)
		{
			if (this.Initialized)
			{
				return;
			}
			if (instance == null)
			{
				Console.LogError("InitializeBuildItem: passed null instance", null);
			}
			if (instance.Quantity != 1)
			{
				Console.LogWarning("BuiltadlbeItem initialized with quantity '" + instance.Quantity.ToString() + "'! This should be 1.", null);
			}
			this.Initialized = true;
			this.ItemInstance = instance;
			this.SetGUID(new Guid(GUID));
			this.ParentProperty = Property.Properties.FirstOrDefault((Property p) => p.PropertyCode == parentPropertyCode);
			if (this.ParentProperty == null)
			{
				this.ParentProperty = Business.Businesses.FirstOrDefault((Business b) => b.PropertyCode == parentPropertyCode);
			}
			if (this.ParentProperty != null)
			{
				this.ParentProperty.BuildableItems.Add(this);
				if (this.ParentProperty.IsContentCulled)
				{
					this.SetCulled(true);
				}
			}
			else
			{
				Console.LogError("BuildableItem '" + base.gameObject.name + "' does not have a parent Property!", null);
			}
			ActivateDuringBuild[] componentsInChildren = base.transform.GetComponentsInChildren<ActivateDuringBuild>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.SetActive(false);
			}
			if (this.onInitialized != null)
			{
				this.onInitialized.Invoke();
			}
		}

		// Token: 0x06002A77 RID: 10871 RVA: 0x000AF4F2 File Offset: 0x000AD6F2
		public bool CanBePickedUp(out string reason)
		{
			if (PlayerSingleton<PlayerInventory>.Instance.CanItemFitInInventory(this.ItemInstance, 1))
			{
				return this.CanBeDestroyed(out reason);
			}
			reason = "Item won't fit in inventory";
			return false;
		}

		// Token: 0x06002A78 RID: 10872 RVA: 0x00075416 File Offset: 0x00073616
		public virtual bool CanBeDestroyed(out string reason)
		{
			reason = string.Empty;
			return true;
		}

		// Token: 0x06002A79 RID: 10873 RVA: 0x000AF518 File Offset: 0x000AD718
		public virtual void PickupItem()
		{
			string empty = string.Empty;
			if (!this.CanBePickedUp(out empty))
			{
				Console.LogWarning("Item can not be picked up!", null);
				return;
			}
			PlayerSingleton<PlayerInventory>.Instance.AddItemToInventory(this.ItemInstance);
			this.DestroyItem(true);
		}

		// Token: 0x06002A7A RID: 10874 RVA: 0x000AF558 File Offset: 0x000AD758
		public virtual void DestroyItem(bool callOnServer = true)
		{
			if (this.IsDestroyed)
			{
				return;
			}
			this.IsDestroyed = true;
			if (callOnServer)
			{
				this.Destroy_Networked();
			}
			if (this.ParentProperty != null)
			{
				this.ParentProperty.BuildableItems.Remove(this);
			}
			if (this.onDestroyed != null)
			{
				this.onDestroyed.Invoke();
			}
			if (this.onDestroyedWithParameter != null)
			{
				this.onDestroyedWithParameter(this);
			}
			base.gameObject.SetActive(false);
		}

		// Token: 0x06002A7B RID: 10875 RVA: 0x000AF5D1 File Offset: 0x000AD7D1
		[ServerRpc(RequireOwnership = false)]
		private void Destroy_Networked()
		{
			this.RpcWriter___Server_Destroy_Networked_2166136261();
		}

		// Token: 0x06002A7C RID: 10876 RVA: 0x000AF5D9 File Offset: 0x000AD7D9
		[ObserversRpc]
		private void DestroyItemWrapper()
		{
			this.RpcWriter___Observers_DestroyItemWrapper_2166136261();
		}

		// Token: 0x06002A7D RID: 10877 RVA: 0x000AF5E1 File Offset: 0x000AD7E1
		public void SetGUID(Guid guid)
		{
			this.GUID = guid;
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x06002A7E RID: 10878 RVA: 0x000AF5F0 File Offset: 0x000AD7F0
		public static Color32 GetColorFromOutlineColorEnum(BuildableItem.EOutlineColor col)
		{
			switch (col)
			{
			case BuildableItem.EOutlineColor.White:
				return Color.white;
			case BuildableItem.EOutlineColor.Blue:
				return new Color32(0, 200, byte.MaxValue, byte.MaxValue);
			case BuildableItem.EOutlineColor.LightBlue:
				return new Color32(120, 225, byte.MaxValue, byte.MaxValue);
			default:
				return Color.white;
			}
		}

		// Token: 0x06002A7F RID: 10879 RVA: 0x000AF654 File Offset: 0x000AD854
		public virtual void ShowOutline(Color color)
		{
			if (this.IsDestroyed || base.gameObject == null)
			{
				return;
			}
			if (this.OutlineEffect == null)
			{
				this.OutlineEffect = base.gameObject.AddComponent<Outlinable>();
				this.OutlineEffect.OutlineParameters.BlurShift = 0f;
				this.OutlineEffect.OutlineParameters.DilateShift = 0.5f;
				this.OutlineEffect.OutlineParameters.FillPass.Shader = Resources.Load<Shader>("Easy performant outline/Shaders/Fills/ColorFill");
				foreach (GameObject gameObject in this.OutlineRenderers)
				{
					MeshRenderer[] array = new MeshRenderer[0];
					if (this.IncludeOutlineRendererChildren)
					{
						array = gameObject.GetComponentsInChildren<MeshRenderer>();
					}
					else
					{
						array = new MeshRenderer[]
						{
							gameObject.GetComponent<MeshRenderer>()
						};
					}
					for (int i = 0; i < array.Length; i++)
					{
						OutlineTarget outlineTarget = new OutlineTarget(array[i], 0);
						this.OutlineEffect.TryAddTarget(outlineTarget);
					}
				}
			}
			this.OutlineEffect.OutlineParameters.Color = color;
			Color32 c = color;
			c.a = 9;
			this.OutlineEffect.OutlineParameters.FillPass.SetColor("_PublicColor", c);
			this.OutlineEffect.enabled = true;
		}

		// Token: 0x06002A80 RID: 10880 RVA: 0x000AF7C4 File Offset: 0x000AD9C4
		public void ShowOutline(BuildableItem.EOutlineColor color)
		{
			this.ShowOutline(BuildableItem.GetColorFromOutlineColorEnum(color));
		}

		// Token: 0x06002A81 RID: 10881 RVA: 0x000AF7D7 File Offset: 0x000AD9D7
		public virtual void HideOutline()
		{
			if (this.IsDestroyed || base.gameObject == null)
			{
				return;
			}
			if (this.OutlineEffect != null)
			{
				this.OutlineEffect.enabled = false;
			}
		}

		// Token: 0x06002A82 RID: 10882 RVA: 0x000AF80C File Offset: 0x000ADA0C
		public Vector3 GetFurthestPointFromBoundingCollider(Vector3 pos)
		{
			Vector3[] array = new Vector3[8];
			BoxCollider boundingCollider = this.BoundingCollider;
			array[0] = this.BoundingCollider.transform.TransformPoint(boundingCollider.center + new Vector3(boundingCollider.size.x, -boundingCollider.size.y, boundingCollider.size.z) * 0.5f);
			array[1] = this.BoundingCollider.transform.TransformPoint(boundingCollider.center + new Vector3(-boundingCollider.size.x, -boundingCollider.size.y, boundingCollider.size.z) * 0.5f);
			array[2] = this.BoundingCollider.transform.TransformPoint(boundingCollider.center + new Vector3(-boundingCollider.size.x, -boundingCollider.size.y, -boundingCollider.size.z) * 0.5f);
			array[3] = this.BoundingCollider.transform.TransformPoint(boundingCollider.center + new Vector3(boundingCollider.size.x, -boundingCollider.size.y, -boundingCollider.size.z) * 0.5f);
			array[4] = this.BoundingCollider.transform.TransformPoint(boundingCollider.center + new Vector3(boundingCollider.size.x, boundingCollider.size.y, boundingCollider.size.z) * 0.5f);
			array[5] = this.BoundingCollider.transform.TransformPoint(boundingCollider.center + new Vector3(-boundingCollider.size.x, boundingCollider.size.y, boundingCollider.size.z) * 0.5f);
			array[6] = this.BoundingCollider.transform.TransformPoint(boundingCollider.center + new Vector3(-boundingCollider.size.x, boundingCollider.size.y, -boundingCollider.size.z) * 0.5f);
			array[7] = this.BoundingCollider.transform.TransformPoint(boundingCollider.center + new Vector3(boundingCollider.size.x, boundingCollider.size.y, -boundingCollider.size.z) * 0.5f);
			List<Vector3> list = new List<Vector3>();
			foreach (Vector3 vector in array)
			{
				if (list.Count == 0)
				{
					list.Add(vector);
				}
				else if (Vector3.Distance(pos, vector) > Vector3.Distance(pos, list[0]))
				{
					list.Clear();
					list.Add(vector);
				}
				else if (Mathf.Abs(Vector3.Distance(pos, vector) - Vector3.Distance(pos, list[0])) < 1E-06f)
				{
					list.Add(vector);
				}
			}
			Vector3 a = Vector3.zero;
			for (int j = 0; j < list.Count; j++)
			{
				a += list[j];
			}
			return a / (float)list.Count;
		}

		// Token: 0x06002A83 RID: 10883 RVA: 0x000AFB84 File Offset: 0x000ADD84
		public bool GetPenetration(out float x, out float z, out float y)
		{
			Vector3 a = this.BoundingCollider.transform.TransformPoint(this.BoundingCollider.center);
			float num = this.BoundingCollider.size.x / 2f;
			float num2 = 0f;
			x = 0f;
			z = 0f;
			y = 0f;
			Vector3 vector = a - base.transform.right * num;
			RaycastHit raycastHit;
			if (this.HasLoS_IgnoreBuildables(vector) && PlayerSingleton<PlayerCamera>.Instance.Raycast_ExcludeBuildables(vector, base.transform.right, this.BoundingCollider.size.x / 2f + num - num2, out raycastHit, 1 << LayerMask.NameToLayer("Default"), false, num2, 45f) && Vector3.Angle(base.transform.right, -raycastHit.normal) < 5f)
			{
				x = this.BoundingCollider.size.x - Vector3.Distance(vector, raycastHit.point);
				Debug.DrawLine(a - base.transform.right * num, raycastHit.point, Color.green);
			}
			vector = a + base.transform.right * num;
			if (this.HasLoS_IgnoreBuildables(vector) && PlayerSingleton<PlayerCamera>.Instance.Raycast_ExcludeBuildables(vector, -base.transform.right, this.BoundingCollider.size.x / 2f + num - num2, out raycastHit, 1 << LayerMask.NameToLayer("Default"), false, num2, 45f) && Vector3.Angle(-base.transform.right, -raycastHit.normal) < 5f)
			{
				float num3 = -(this.BoundingCollider.size.x - Vector3.Distance(vector, raycastHit.point));
				x = num3;
				Debug.DrawLine(a + base.transform.right * num, raycastHit.point, Color.red);
			}
			num = this.BoundingCollider.size.z / 2f;
			vector = a - base.transform.forward * num;
			if (this.HasLoS_IgnoreBuildables(vector) && PlayerSingleton<PlayerCamera>.Instance.Raycast_ExcludeBuildables(vector, base.transform.forward, this.BoundingCollider.size.z / 2f + num - num2, out raycastHit, 1 << LayerMask.NameToLayer("Default"), false, num2, 45f) && Vector3.Angle(base.transform.forward, -raycastHit.normal) < 5f)
			{
				z = this.BoundingCollider.size.z - Vector3.Distance(vector, raycastHit.point);
				Debug.DrawLine(a - base.transform.forward * num, raycastHit.point, Color.cyan);
			}
			vector = a + base.transform.forward * num;
			if (this.HasLoS_IgnoreBuildables(vector) && PlayerSingleton<PlayerCamera>.Instance.Raycast_ExcludeBuildables(vector, -base.transform.forward, this.BoundingCollider.size.z / 2f + num - num2, out raycastHit, 1 << LayerMask.NameToLayer("Default"), false, num2, 45f) && Vector3.Angle(-base.transform.forward, -raycastHit.normal) < 5f)
			{
				float num4 = -(this.BoundingCollider.size.z - Vector3.Distance(vector, raycastHit.point));
				z = num4;
				Debug.DrawLine(a + base.transform.forward * num, raycastHit.point, Color.yellow);
			}
			num = this.BoundingCollider.size.y / 2f;
			vector = a - base.transform.up * num;
			if (this.HasLoS_IgnoreBuildables(vector) && PlayerSingleton<PlayerCamera>.Instance.Raycast_ExcludeBuildables(vector, base.transform.up, this.BoundingCollider.size.y / 2f + num - num2, out raycastHit, 1 << LayerMask.NameToLayer("Default"), false, num2, 45f) && Vector3.Angle(base.transform.forward, -raycastHit.normal) < 5f)
			{
				y = this.BoundingCollider.size.y - Vector3.Distance(vector, raycastHit.point);
				Debug.DrawLine(a - base.transform.up * num, raycastHit.point, Color.cyan);
			}
			vector = a + base.transform.up * num;
			if (this.HasLoS_IgnoreBuildables(vector) && PlayerSingleton<PlayerCamera>.Instance.Raycast_ExcludeBuildables(vector, -base.transform.up, this.BoundingCollider.size.y / 2f + num - num2, out raycastHit, 1 << LayerMask.NameToLayer("Default"), false, num2, 45f) && Vector3.Angle(-base.transform.up, -raycastHit.normal) < 5f)
			{
				float num5 = -(this.BoundingCollider.size.y - Vector3.Distance(vector, raycastHit.point));
				y = num5;
				Debug.DrawLine(a + base.transform.up * num, raycastHit.point, Color.yellow);
			}
			return x != 0f || z != 0f || y != 0f;
		}

		// Token: 0x06002A84 RID: 10884 RVA: 0x000B01A4 File Offset: 0x000AE3A4
		private bool HasLoS_IgnoreBuildables(Vector3 point)
		{
			RaycastHit raycastHit;
			return !PlayerSingleton<PlayerCamera>.Instance.Raycast_ExcludeBuildables(PlayerSingleton<PlayerCamera>.Instance.transform.position, point - PlayerSingleton<PlayerCamera>.Instance.transform.position, Vector3.Distance(point, PlayerSingleton<PlayerCamera>.Instance.transform.position) - 0.01f, out raycastHit, 1 << LayerMask.NameToLayer("Default"), false, 0f, 0f);
		}

		// Token: 0x06002A85 RID: 10885 RVA: 0x000B0220 File Offset: 0x000AE420
		public virtual void SetCulled(bool culled)
		{
			this.IsCulled = culled;
			foreach (MeshRenderer meshRenderer in this.MeshesToCull)
			{
				if (!(meshRenderer == null))
				{
					meshRenderer.enabled = !culled;
				}
			}
			foreach (GameObject gameObject in this.GameObjectsToCull)
			{
				if (!(gameObject == null))
				{
					gameObject.SetActive(!culled);
				}
			}
		}

		// Token: 0x06002A86 RID: 10886 RVA: 0x000B02B8 File Offset: 0x000AE4B8
		public virtual DynamicSaveData GetSaveData()
		{
			return new DynamicSaveData(this.GetBaseData());
		}

		// Token: 0x06002A87 RID: 10887 RVA: 0x000B02C5 File Offset: 0x000AE4C5
		public virtual BuildableItemData GetBaseData()
		{
			return new BuildableItemData(this.GUID, this.ItemInstance, 0);
		}

		// Token: 0x06002A88 RID: 10888 RVA: 0x000B02D9 File Offset: 0x000AE4D9
		public string GetSaveString()
		{
			return this.GetBaseData().GetJson(true);
		}

		// Token: 0x06002A89 RID: 10889 RVA: 0x000594B4 File Offset: 0x000576B4
		public virtual List<string> WriteData(string parentFolderPath)
		{
			return new List<string>();
		}

		// Token: 0x06002A8B RID: 10891 RVA: 0x000B0324 File Offset: 0x000AE524
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.EntityFramework.BuildableItemAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.EntityFramework.BuildableItemAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendBuildableItemData_3537728543));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveBuildableItemData_3859851844));
			base.RegisterTargetRpc(2U, new ClientRpcDelegate(this.RpcReader___Target_ReceiveBuildableItemData_3859851844));
			base.RegisterServerRpc(3U, new ServerRpcDelegate(this.RpcReader___Server_Destroy_Networked_2166136261));
			base.RegisterObserversRpc(4U, new ClientRpcDelegate(this.RpcReader___Observers_DestroyItemWrapper_2166136261));
		}

		// Token: 0x06002A8C RID: 10892 RVA: 0x000B03B5 File Offset: 0x000AE5B5
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.EntityFramework.BuildableItemAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.EntityFramework.BuildableItemAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06002A8D RID: 10893 RVA: 0x000B03C8 File Offset: 0x000AE5C8
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002A8E RID: 10894 RVA: 0x000B03D8 File Offset: 0x000AE5D8
		private void RpcWriter___Server_SendBuildableItemData_3537728543(ItemInstance instance, string GUID, string parentPropertyCode)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteItemInstance(instance);
			writer.WriteString(GUID);
			writer.WriteString(parentPropertyCode);
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002A8F RID: 10895 RVA: 0x000B0499 File Offset: 0x000AE699
		public void RpcLogic___SendBuildableItemData_3537728543(ItemInstance instance, string GUID, string parentPropertyCode)
		{
			this.ReceiveBuildableItemData(null, instance, GUID, parentPropertyCode);
		}

		// Token: 0x06002A90 RID: 10896 RVA: 0x000B04A8 File Offset: 0x000AE6A8
		private void RpcReader___Server_SendBuildableItemData_3537728543(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			ItemInstance instance = PooledReader0.ReadItemInstance();
			string guid = PooledReader0.ReadString();
			string parentPropertyCode = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendBuildableItemData_3537728543(instance, guid, parentPropertyCode);
		}

		// Token: 0x06002A91 RID: 10897 RVA: 0x000B04FC File Offset: 0x000AE6FC
		private void RpcWriter___Observers_ReceiveBuildableItemData_3859851844(NetworkConnection conn, ItemInstance instance, string GUID, string parentPropertyCode)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteItemInstance(instance);
			writer.WriteString(GUID);
			writer.WriteString(parentPropertyCode);
			base.SendObserversRpc(1U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002A92 RID: 10898 RVA: 0x000B05CC File Offset: 0x000AE7CC
		public void RpcLogic___ReceiveBuildableItemData_3859851844(NetworkConnection conn, ItemInstance instance, string GUID, string parentPropertyCode)
		{
			this.InitializeBuildableItem(instance, GUID, parentPropertyCode);
		}

		// Token: 0x06002A93 RID: 10899 RVA: 0x000B05D8 File Offset: 0x000AE7D8
		private void RpcReader___Observers_ReceiveBuildableItemData_3859851844(PooledReader PooledReader0, Channel channel)
		{
			ItemInstance instance = PooledReader0.ReadItemInstance();
			string guid = PooledReader0.ReadString();
			string parentPropertyCode = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveBuildableItemData_3859851844(null, instance, guid, parentPropertyCode);
		}

		// Token: 0x06002A94 RID: 10900 RVA: 0x000B062C File Offset: 0x000AE82C
		private void RpcWriter___Target_ReceiveBuildableItemData_3859851844(NetworkConnection conn, ItemInstance instance, string GUID, string parentPropertyCode)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteItemInstance(instance);
			writer.WriteString(GUID);
			writer.WriteString(parentPropertyCode);
			base.SendTargetRpc(2U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06002A95 RID: 10901 RVA: 0x000B06FC File Offset: 0x000AE8FC
		private void RpcReader___Target_ReceiveBuildableItemData_3859851844(PooledReader PooledReader0, Channel channel)
		{
			ItemInstance instance = PooledReader0.ReadItemInstance();
			string guid = PooledReader0.ReadString();
			string parentPropertyCode = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveBuildableItemData_3859851844(base.LocalConnection, instance, guid, parentPropertyCode);
		}

		// Token: 0x06002A96 RID: 10902 RVA: 0x000B0758 File Offset: 0x000AE958
		private void RpcWriter___Server_Destroy_Networked_2166136261()
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendServerRpc(3U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002A97 RID: 10903 RVA: 0x000B07F2 File Offset: 0x000AE9F2
		private void RpcLogic___Destroy_Networked_2166136261()
		{
			this.DestroyItemWrapper();
			base.Despawn(new DespawnType?(0));
		}

		// Token: 0x06002A98 RID: 10904 RVA: 0x000B0808 File Offset: 0x000AEA08
		private void RpcReader___Server_Destroy_Networked_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___Destroy_Networked_2166136261();
		}

		// Token: 0x06002A99 RID: 10905 RVA: 0x000B0828 File Offset: 0x000AEA28
		private void RpcWriter___Observers_DestroyItemWrapper_2166136261()
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendObserversRpc(4U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002A9A RID: 10906 RVA: 0x000B08D1 File Offset: 0x000AEAD1
		private void RpcLogic___DestroyItemWrapper_2166136261()
		{
			this.DestroyItem(false);
		}

		// Token: 0x06002A9B RID: 10907 RVA: 0x000B08DC File Offset: 0x000AEADC
		private void RpcReader___Observers_DestroyItemWrapper_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___DestroyItemWrapper_2166136261();
		}

		// Token: 0x06002A9C RID: 10908 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void dll()
		{
		}

		// Token: 0x04001F04 RID: 7940
		[HideInInspector]
		public bool isGhost;

		// Token: 0x04001F05 RID: 7941
		[Header("Build Settings")]
		[SerializeField]
		protected GameObject buildHandler;

		// Token: 0x04001F06 RID: 7942
		public float HoldDistance = 2.5f;

		// Token: 0x04001F07 RID: 7943
		public Transform BuildPoint;

		// Token: 0x04001F08 RID: 7944
		public Transform MidAirCenterPoint;

		// Token: 0x04001F09 RID: 7945
		public BoxCollider BoundingCollider;

		// Token: 0x04001F0A RID: 7946
		[Header("Outline settings")]
		[SerializeField]
		protected List<GameObject> OutlineRenderers = new List<GameObject>();

		// Token: 0x04001F0B RID: 7947
		[SerializeField]
		protected bool IncludeOutlineRendererChildren = true;

		// Token: 0x04001F0C RID: 7948
		protected Outlinable OutlineEffect;

		// Token: 0x04001F0D RID: 7949
		[Header("Culling Settings")]
		public GameObject[] GameObjectsToCull;

		// Token: 0x04001F0E RID: 7950
		public List<MeshRenderer> MeshesToCull;

		// Token: 0x04001F0F RID: 7951
		[Header("Buildable Events")]
		public UnityEvent onGhostModel;

		// Token: 0x04001F10 RID: 7952
		public UnityEvent onInitialized;

		// Token: 0x04001F11 RID: 7953
		public UnityEvent onDestroyed;

		// Token: 0x04001F12 RID: 7954
		public Action<BuildableItem> onDestroyedWithParameter;

		// Token: 0x04001F17 RID: 7959
		private bool dll_Excuted;

		// Token: 0x04001F18 RID: 7960
		private bool dll_Excuted;

		// Token: 0x02000661 RID: 1633
		public enum EOutlineColor
		{
			// Token: 0x04001F1A RID: 7962
			White,
			// Token: 0x04001F1B RID: 7963
			Blue,
			// Token: 0x04001F1C RID: 7964
			LightBlue
		}
	}
}
