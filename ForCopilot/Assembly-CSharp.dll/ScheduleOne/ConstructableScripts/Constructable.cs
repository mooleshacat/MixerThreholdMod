using System;
using System.Collections.Generic;
using EPOOutline;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Construction.Features;
using ScheduleOne.EntityFramework;
using UnityEngine;

namespace ScheduleOne.ConstructableScripts
{
	// Token: 0x0200096B RID: 2411
	public class Constructable : NetworkBehaviour
	{
		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x06004111 RID: 16657 RVA: 0x001131D4 File Offset: 0x001113D4
		public bool IsStatic
		{
			get
			{
				return this.isStatic;
			}
		}

		// Token: 0x170008FF RID: 2303
		// (get) Token: 0x06004112 RID: 16658 RVA: 0x001131DC File Offset: 0x001113DC
		public string ConstructableName
		{
			get
			{
				return this.constructableName;
			}
		}

		// Token: 0x17000900 RID: 2304
		// (get) Token: 0x06004113 RID: 16659 RVA: 0x001131E4 File Offset: 0x001113E4
		public string ConstructableDescription
		{
			get
			{
				return this.constructableDescription;
			}
		}

		// Token: 0x17000901 RID: 2305
		// (get) Token: 0x06004114 RID: 16660 RVA: 0x001131EC File Offset: 0x001113EC
		public string ConstructableAssetPath
		{
			get
			{
				return this.constructableAssetPath;
			}
		}

		// Token: 0x17000902 RID: 2306
		// (get) Token: 0x06004115 RID: 16661 RVA: 0x001131F4 File Offset: 0x001113F4
		public string PrefabID
		{
			get
			{
				return this.ID;
			}
		}

		// Token: 0x17000903 RID: 2307
		// (get) Token: 0x06004116 RID: 16662 RVA: 0x001131FC File Offset: 0x001113FC
		public Sprite ConstructableIcon
		{
			get
			{
				return this.constructableIcon;
			}
		}

		// Token: 0x17000904 RID: 2308
		// (get) Token: 0x06004117 RID: 16663 RVA: 0x00113204 File Offset: 0x00111404
		public GameObject _constructionHandler_Asset
		{
			get
			{
				return this.constructionHandler_Asset;
			}
		}

		// Token: 0x17000905 RID: 2309
		// (get) Token: 0x06004118 RID: 16664 RVA: 0x0011320C File Offset: 0x0011140C
		// (set) Token: 0x06004119 RID: 16665 RVA: 0x00113214 File Offset: 0x00111414
		public bool isVisible { get; protected set; } = true;

		// Token: 0x0600411A RID: 16666 RVA: 0x00113220 File Offset: 0x00111420
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.ConstructableScripts.Constructable_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600411B RID: 16667 RVA: 0x00065656 File Offset: 0x00063856
		public override void OnStartClient()
		{
			base.OnStartClient();
		}

		// Token: 0x0600411C RID: 16668 RVA: 0x0011323F File Offset: 0x0011143F
		public virtual bool CanBeDestroyed(out string reason)
		{
			reason = string.Empty;
			return !this.isStatic;
		}

		// Token: 0x0600411D RID: 16669 RVA: 0x00113254 File Offset: 0x00111454
		public virtual bool CanBeDestroyed()
		{
			string text;
			return this.CanBeDestroyed(out text);
		}

		// Token: 0x0600411E RID: 16670 RVA: 0x00113269 File Offset: 0x00111469
		public virtual void DestroyConstructable(bool callOnServer = true)
		{
			if (this.isDestroyed)
			{
				return;
			}
			this.isDestroyed = true;
			Console.Log("Destroying constructable", null);
			if (callOnServer)
			{
				this.Destroy_Networked();
			}
			base.gameObject.SetActive(false);
		}

		// Token: 0x0600411F RID: 16671 RVA: 0x0011329B File Offset: 0x0011149B
		[ServerRpc(RequireOwnership = false)]
		private void Destroy_Networked()
		{
			this.RpcWriter___Server_Destroy_Networked_2166136261();
		}

		// Token: 0x06004120 RID: 16672 RVA: 0x001132A3 File Offset: 0x001114A3
		[ObserversRpc]
		private void DestroyConstructableWrapper()
		{
			this.RpcWriter___Observers_DestroyConstructableWrapper_2166136261();
		}

		// Token: 0x06004121 RID: 16673 RVA: 0x000022C9 File Offset: 0x000004C9
		public virtual bool CanBeModified()
		{
			return true;
		}

		// Token: 0x06004122 RID: 16674 RVA: 0x00014B5A File Offset: 0x00012D5A
		public virtual bool CanBePickedUpByHand()
		{
			return false;
		}

		// Token: 0x06004123 RID: 16675 RVA: 0x001132AB File Offset: 0x001114AB
		public virtual bool CanBeSelected()
		{
			return !this.isStatic;
		}

		// Token: 0x06004124 RID: 16676 RVA: 0x00092542 File Offset: 0x00090742
		public virtual string GetBuildableVersionAssetPath()
		{
			return string.Empty;
		}

		// Token: 0x06004125 RID: 16677 RVA: 0x001132B8 File Offset: 0x001114B8
		public void ShowOutline(BuildableItem.EOutlineColor color)
		{
			if (this.outlineEffect == null)
			{
				this.outlineEffect = base.gameObject.AddComponent<Outlinable>();
				this.outlineEffect.OutlineParameters.BlurShift = 0f;
				this.outlineEffect.OutlineParameters.DilateShift = 0.5f;
				this.outlineEffect.OutlineParameters.FillPass.Shader = Resources.Load<Shader>("Easy performant outline/Shaders/Fills/ColorFill");
				foreach (GameObject gameObject in this.outlineRenderers)
				{
					MeshRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<MeshRenderer>();
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						OutlineTarget outlineTarget = new OutlineTarget(componentsInChildren[i], 0);
						this.outlineEffect.TryAddTarget(outlineTarget);
					}
				}
			}
			this.outlineEffect.OutlineParameters.Color = BuildableItem.GetColorFromOutlineColorEnum(color);
			Color32 colorFromOutlineColorEnum = BuildableItem.GetColorFromOutlineColorEnum(color);
			colorFromOutlineColorEnum.a = 9;
			this.outlineEffect.OutlineParameters.FillPass.SetColor("_PublicColor", colorFromOutlineColorEnum);
			this.outlineEffect.enabled = true;
		}

		// Token: 0x06004126 RID: 16678 RVA: 0x001133F4 File Offset: 0x001115F4
		public void HideOutline()
		{
			if (this.outlineEffect != null)
			{
				this.outlineEffect.enabled = false;
			}
		}

		// Token: 0x06004127 RID: 16679 RVA: 0x0006BF1E File Offset: 0x0006A11E
		public virtual Vector3 GetCosmeticCenter()
		{
			return base.transform.position;
		}

		// Token: 0x06004128 RID: 16680 RVA: 0x00113410 File Offset: 0x00111610
		public float GetBoundingBoxLongestSide()
		{
			return Mathf.Max(Mathf.Max(this.boundingBox.size.x, this.boundingBox.size.y), this.boundingBox.size.z);
		}

		// Token: 0x06004129 RID: 16681 RVA: 0x0011344C File Offset: 0x0011164C
		public virtual void SetInvisible()
		{
			this.isVisible = false;
			this.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("Invisible"));
		}

		// Token: 0x0600412A RID: 16682 RVA: 0x0011346C File Offset: 0x0011166C
		public virtual void RestoreVisibility()
		{
			this.isVisible = true;
			foreach (Transform transform in base.GetComponentsInChildren<Transform>(true))
			{
				if (transform.gameObject.layer != LayerMask.NameToLayer("Grid"))
				{
					if (this.originalLayers.ContainsKey(transform))
					{
						transform.gameObject.layer = this.originalLayers[transform];
					}
					else
					{
						transform.gameObject.layer = LayerMask.NameToLayer("Default");
					}
				}
			}
		}

		// Token: 0x0600412B RID: 16683 RVA: 0x001134F4 File Offset: 0x001116F4
		public void SetLayerRecursively(GameObject go, int layerNumber)
		{
			foreach (Transform transform in go.GetComponentsInChildren<Transform>(true))
			{
				if (transform.gameObject.layer != LayerMask.NameToLayer("Grid"))
				{
					if (transform.gameObject.layer != LayerMask.NameToLayer("Default"))
					{
						if (this.originalLayers.ContainsKey(transform))
						{
							this.originalLayers[transform] = transform.gameObject.layer;
						}
						else
						{
							this.originalLayers.Add(transform, transform.gameObject.layer);
						}
					}
					transform.gameObject.layer = layerNumber;
				}
			}
		}

		// Token: 0x0600412D RID: 16685 RVA: 0x0011360C File Offset: 0x0011180C
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ConstructableScripts.ConstructableAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ConstructableScripts.ConstructableAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_Destroy_Networked_2166136261));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_DestroyConstructableWrapper_2166136261));
		}

		// Token: 0x0600412E RID: 16686 RVA: 0x00113658 File Offset: 0x00111858
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ConstructableScripts.ConstructableAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ConstructableScripts.ConstructableAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x0600412F RID: 16687 RVA: 0x0011366B File Offset: 0x0011186B
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06004130 RID: 16688 RVA: 0x0011367C File Offset: 0x0011187C
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
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06004131 RID: 16689 RVA: 0x00113716 File Offset: 0x00111916
		private void RpcLogic___Destroy_Networked_2166136261()
		{
			Console.Log("Networked", null);
			this.DestroyConstructableWrapper();
			base.Despawn(new DespawnType?(0));
		}

		// Token: 0x06004132 RID: 16690 RVA: 0x00113738 File Offset: 0x00111938
		private void RpcReader___Server_Destroy_Networked_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___Destroy_Networked_2166136261();
		}

		// Token: 0x06004133 RID: 16691 RVA: 0x00113758 File Offset: 0x00111958
		private void RpcWriter___Observers_DestroyConstructableWrapper_2166136261()
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
			base.SendObserversRpc(1U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06004134 RID: 16692 RVA: 0x00113801 File Offset: 0x00111A01
		private void RpcLogic___DestroyConstructableWrapper_2166136261()
		{
			Console.Log("Wrapper", null);
			this.DestroyConstructable(false);
		}

		// Token: 0x06004135 RID: 16693 RVA: 0x00113818 File Offset: 0x00111A18
		private void RpcReader___Observers_DestroyConstructableWrapper_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___DestroyConstructableWrapper_2166136261();
		}

		// Token: 0x06004136 RID: 16694 RVA: 0x00113838 File Offset: 0x00111A38
		protected virtual void dll()
		{
			this.boundingBox.isTrigger = true;
			this.boundingBox.gameObject.layer = LayerMask.NameToLayer("Invisible");
			foreach (Feature feature in this.features)
			{
			}
		}

		// Token: 0x04002E78 RID: 11896
		[Header("Basic settings")]
		[SerializeField]
		protected bool isStatic;

		// Token: 0x04002E79 RID: 11897
		[SerializeField]
		protected string constructableName = "Constructable";

		// Token: 0x04002E7A RID: 11898
		[SerializeField]
		protected string constructableDescription = "Description";

		// Token: 0x04002E7B RID: 11899
		[SerializeField]
		protected string constructableAssetPath = string.Empty;

		// Token: 0x04002E7C RID: 11900
		[SerializeField]
		protected string ID = string.Empty;

		// Token: 0x04002E7D RID: 11901
		[SerializeField]
		protected Sprite constructableIcon;

		// Token: 0x04002E7E RID: 11902
		[Header("Bounds settings")]
		public BoxCollider boundingBox;

		// Token: 0x04002E7F RID: 11903
		[Header("Construction Handler")]
		[SerializeField]
		protected GameObject constructionHandler_Asset;

		// Token: 0x04002E80 RID: 11904
		[Header("Outline settings")]
		[SerializeField]
		protected List<GameObject> outlineRenderers = new List<GameObject>();

		// Token: 0x04002E81 RID: 11905
		protected Outlinable outlineEffect;

		// Token: 0x04002E82 RID: 11906
		[Header("Features")]
		public List<Feature> features = new List<Feature>();

		// Token: 0x04002E84 RID: 11908
		private bool isDestroyed;

		// Token: 0x04002E85 RID: 11909
		private Dictionary<Transform, LayerMask> originalLayers = new Dictionary<Transform, LayerMask>();

		// Token: 0x04002E86 RID: 11910
		private bool dll_Excuted;

		// Token: 0x04002E87 RID: 11911
		private bool dll_Excuted;
	}
}
