using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Object.Synchronizing;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.UI.Construction.Features;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Construction.Features
{
	// Token: 0x02000761 RID: 1889
	public class ColorFeature : Feature
	{
		// Token: 0x060032DF RID: 13023 RVA: 0x000D37A4 File Offset: 0x000D19A4
		public override FI_Base CreateInterface(Transform parent)
		{
			FI_ColorPicker fi_ColorPicker = base.CreateInterface(parent) as FI_ColorPicker;
			fi_ColorPicker.onSelectionChanged.AddListener(new UnityAction<ColorFeature.NamedColor>(this.ApplyColor));
			fi_ColorPicker.onSelectionPurchased.AddListener(new UnityAction<ColorFeature.NamedColor>(this.BuyColor));
			return fi_ColorPicker;
		}

		// Token: 0x060032E0 RID: 13024 RVA: 0x000D37E0 File Offset: 0x000D19E0
		public override void Default()
		{
			this.BuyColor(this.colors[this.defaultColorIndex]);
		}

		// Token: 0x060032E1 RID: 13025 RVA: 0x000D37FC File Offset: 0x000D19FC
		private void ApplyColor(ColorFeature.NamedColor color)
		{
			for (int i = 0; i < this.colorTargets.Count; i++)
			{
				this.colorTargets[i].material.color = color.color;
			}
			foreach (ColorFeature.SecondaryPaintTarget secondaryPaintTarget in this.secondaryTargets)
			{
				for (int j = 0; j < secondaryPaintTarget.colorTargets.Count; j++)
				{
					secondaryPaintTarget.colorTargets[j].material.color = ColorFeature.ModifyColor(color.color, secondaryPaintTarget.sChange, secondaryPaintTarget.vChange);
				}
			}
		}

		// Token: 0x060032E2 RID: 13026 RVA: 0x000D38C0 File Offset: 0x000D1AC0
		public static Color ModifyColor(Color original, float sChange, float vChange)
		{
			float h;
			float num;
			float num2;
			Color.RGBToHSV(original, out h, out num, out num2);
			num = Mathf.Clamp(num + sChange / 100f, 0f, 1f);
			num2 = Mathf.Clamp(num2 + vChange / 100f, 0f, 1f);
			return Color.HSVToRGB(h, num, num2);
		}

		// Token: 0x060032E3 RID: 13027 RVA: 0x000D3913 File Offset: 0x000D1B13
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		protected virtual void SetData(int colorIndex)
		{
			this.RpcWriter___Server_SetData_3316948804(colorIndex);
			this.RpcLogic___SetData_3316948804(colorIndex);
		}

		// Token: 0x060032E4 RID: 13028 RVA: 0x000D3929 File Offset: 0x000D1B29
		private void ReceiveData()
		{
			this.ApplyColor(this.colors[this.SyncAccessor_ownedColorIndex]);
		}

		// Token: 0x060032E5 RID: 13029 RVA: 0x000D3942 File Offset: 0x000D1B42
		private void BuyColor(ColorFeature.NamedColor color)
		{
			this.SetData(this.colors.IndexOf(color));
		}

		// Token: 0x060032E7 RID: 13031 RVA: 0x000D3980 File Offset: 0x000D1B80
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Construction.Features.ColorFeatureAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Construction.Features.ColorFeatureAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___ownedColorIndex = new SyncVar<int>(this, 0U, 0, 0, -1f, 0, this.ownedColorIndex);
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SetData_3316948804));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.Construction.Features.ColorFeature));
		}

		// Token: 0x060032E8 RID: 13032 RVA: 0x000D39F8 File Offset: 0x000D1BF8
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Construction.Features.ColorFeatureAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Construction.Features.ColorFeatureAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___ownedColorIndex.SetRegistered();
		}

		// Token: 0x060032E9 RID: 13033 RVA: 0x000D3A1C File Offset: 0x000D1C1C
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060032EA RID: 13034 RVA: 0x000D3A2C File Offset: 0x000D1C2C
		private void RpcWriter___Server_SetData_3316948804(int colorIndex)
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
			writer.WriteInt32(colorIndex, 1);
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060032EB RID: 13035 RVA: 0x000D3AD8 File Offset: 0x000D1CD8
		protected virtual void RpcLogic___SetData_3316948804(int colorIndex)
		{
			if (!base.IsSpawned)
			{
				this.ApplyColor(this.colors[colorIndex]);
				return;
			}
			this.sync___set_value_ownedColorIndex(colorIndex, true);
		}

		// Token: 0x060032EC RID: 13036 RVA: 0x000D3B00 File Offset: 0x000D1D00
		private void RpcReader___Server_SetData_3316948804(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int colorIndex = PooledReader0.ReadInt32(1);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetData_3316948804(colorIndex);
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x060032ED RID: 13037 RVA: 0x000D3B43 File Offset: 0x000D1D43
		// (set) Token: 0x060032EE RID: 13038 RVA: 0x000D3B4B File Offset: 0x000D1D4B
		public int SyncAccessor_ownedColorIndex
		{
			get
			{
				return this.ownedColorIndex;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.ownedColorIndex = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___ownedColorIndex.SetValue(value, value);
				}
			}
		}

		// Token: 0x060032EF RID: 13039 RVA: 0x000D3B88 File Offset: 0x000D1D88
		public override bool ColorFeature(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 != 0U)
			{
				return false;
			}
			if (PooledReader0 == null)
			{
				this.sync___set_value_ownedColorIndex(this.syncVar___ownedColorIndex.GetValue(true), true);
				return true;
			}
			int value = PooledReader0.ReadInt32(1);
			this.sync___set_value_ownedColorIndex(value, Boolean2);
			return true;
		}

		// Token: 0x060032F0 RID: 13040 RVA: 0x000D3BDF File Offset: 0x000D1DDF
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040023E8 RID: 9192
		[Header("References")]
		[SerializeField]
		protected List<MeshRenderer> colorTargets = new List<MeshRenderer>();

		// Token: 0x040023E9 RID: 9193
		[SerializeField]
		protected List<ColorFeature.SecondaryPaintTarget> secondaryTargets = new List<ColorFeature.SecondaryPaintTarget>();

		// Token: 0x040023EA RID: 9194
		[Header("Color settings")]
		public List<ColorFeature.NamedColor> colors = new List<ColorFeature.NamedColor>();

		// Token: 0x040023EB RID: 9195
		public int defaultColorIndex;

		// Token: 0x040023EC RID: 9196
		[SyncVar]
		public int ownedColorIndex;

		// Token: 0x040023ED RID: 9197
		public SyncVar<int> syncVar___ownedColorIndex;

		// Token: 0x040023EE RID: 9198
		private bool dll_Excuted;

		// Token: 0x040023EF RID: 9199
		private bool dll_Excuted;

		// Token: 0x02000762 RID: 1890
		[Serializable]
		public class NamedColor
		{
			// Token: 0x040023F0 RID: 9200
			public string colorName;

			// Token: 0x040023F1 RID: 9201
			public Color color;

			// Token: 0x040023F2 RID: 9202
			public float price = 100f;
		}

		// Token: 0x02000763 RID: 1891
		[Serializable]
		public class SecondaryPaintTarget
		{
			// Token: 0x040023F3 RID: 9203
			public List<MeshRenderer> colorTargets = new List<MeshRenderer>();

			// Token: 0x040023F4 RID: 9204
			public float sChange;

			// Token: 0x040023F5 RID: 9205
			public float vChange;
		}
	}
}
