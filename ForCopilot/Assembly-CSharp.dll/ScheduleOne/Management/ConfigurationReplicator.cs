using System;
using System.Collections.Generic;
using System.Linq;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.EntityFramework;
using ScheduleOne.ItemFramework;
using ScheduleOne.NPCs;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.StationFramework;
using UnityEngine;

namespace ScheduleOne.Management
{
	// Token: 0x0200059B RID: 1435
	public class ConfigurationReplicator : NetworkBehaviour
	{
		// Token: 0x060022A7 RID: 8871 RVA: 0x0008F100 File Offset: 0x0008D300
		public void ReplicateField(ConfigField field, NetworkConnection conn = null)
		{
			int num = this.Configuration.Fields.IndexOf(field);
			if (num == -1)
			{
				Console.LogError("Failed to find field in configuration", null);
				return;
			}
			if (field is ItemField)
			{
				ItemField itemField = (ItemField)field;
				this.SendItemField(num, (itemField.SelectedItem != null) ? itemField.SelectedItem.name : string.Empty);
				return;
			}
			if (field is NPCField)
			{
				NPCField npcfield = (NPCField)field;
				this.SendNPCField(num, (npcfield.SelectedNPC != null) ? npcfield.SelectedNPC.NetworkObject : null);
				return;
			}
			if (field is ObjectField)
			{
				ObjectField objectField = (ObjectField)field;
				NetworkObject obj = null;
				if (objectField.SelectedObject != null)
				{
					obj = objectField.SelectedObject.NetworkObject;
				}
				this.SendObjectField(num, obj);
				return;
			}
			if (field is ObjectListField)
			{
				ObjectListField objectListField = (ObjectListField)field;
				List<NetworkObject> list = new List<NetworkObject>();
				for (int i = 0; i < objectListField.SelectedObjects.Count; i++)
				{
					list.Add(objectListField.SelectedObjects[i].NetworkObject);
				}
				this.SendObjectListField(num, list);
				return;
			}
			if (field is StationRecipeField)
			{
				StationRecipeField stationRecipeField = (StationRecipeField)field;
				int recipeIndex = -1;
				if (stationRecipeField.SelectedRecipe != null)
				{
					recipeIndex = stationRecipeField.Options.IndexOf(stationRecipeField.SelectedRecipe);
				}
				this.SendRecipeField(num, recipeIndex);
				return;
			}
			if (field is NumberField)
			{
				NumberField numberField = (NumberField)field;
				this.SendNumberField(num, numberField.Value);
				return;
			}
			if (field is RouteListField)
			{
				RouteListField routeListField = (RouteListField)field;
				this.SendRouteListField(num, (from x in routeListField.Routes
				select x.GetData()).ToArray<AdvancedTransitRouteData>());
				return;
			}
			if (field is QualityField)
			{
				QualityField qualityField = (QualityField)field;
				this.SendQualityField(num, qualityField.Value);
				return;
			}
			string str = "Failed to find replication method for ";
			Type type = field.GetType();
			Console.LogError(str + ((type != null) ? type.ToString() : null), null);
		}

		// Token: 0x060022A8 RID: 8872 RVA: 0x0008F30F File Offset: 0x0008D50F
		[ServerRpc(RequireOwnership = false)]
		private void SendItemField(int fieldIndex, string value)
		{
			this.RpcWriter___Server_SendItemField_2801973956(fieldIndex, value);
		}

		// Token: 0x060022A9 RID: 8873 RVA: 0x0008F320 File Offset: 0x0008D520
		[ObserversRpc]
		private void ReceiveItemField(int fieldIndex, string value)
		{
			this.RpcWriter___Observers_ReceiveItemField_2801973956(fieldIndex, value);
		}

		// Token: 0x060022AA RID: 8874 RVA: 0x0008F33B File Offset: 0x0008D53B
		[ServerRpc(RequireOwnership = false)]
		private void SendNPCField(int fieldIndex, NetworkObject npcObject)
		{
			this.RpcWriter___Server_SendNPCField_1687693739(fieldIndex, npcObject);
		}

		// Token: 0x060022AB RID: 8875 RVA: 0x0008F34C File Offset: 0x0008D54C
		[ObserversRpc]
		private void ReceiveNPCField(int fieldIndex, NetworkObject npcObject)
		{
			this.RpcWriter___Observers_ReceiveNPCField_1687693739(fieldIndex, npcObject);
		}

		// Token: 0x060022AC RID: 8876 RVA: 0x0008F367 File Offset: 0x0008D567
		[ServerRpc(RequireOwnership = false)]
		private void SendObjectField(int fieldIndex, NetworkObject obj)
		{
			this.RpcWriter___Server_SendObjectField_1687693739(fieldIndex, obj);
		}

		// Token: 0x060022AD RID: 8877 RVA: 0x0008F378 File Offset: 0x0008D578
		[ObserversRpc]
		private void ReceiveObjectField(int fieldIndex, NetworkObject obj)
		{
			this.RpcWriter___Observers_ReceiveObjectField_1687693739(fieldIndex, obj);
		}

		// Token: 0x060022AE RID: 8878 RVA: 0x0008F393 File Offset: 0x0008D593
		[ServerRpc(RequireOwnership = false)]
		private void SendObjectListField(int fieldIndex, List<NetworkObject> objects)
		{
			this.RpcWriter___Server_SendObjectListField_690244341(fieldIndex, objects);
		}

		// Token: 0x060022AF RID: 8879 RVA: 0x0008F3A4 File Offset: 0x0008D5A4
		[ObserversRpc]
		private void ReceiveObjectListField(int fieldIndex, List<NetworkObject> objects)
		{
			this.RpcWriter___Observers_ReceiveObjectListField_690244341(fieldIndex, objects);
		}

		// Token: 0x060022B0 RID: 8880 RVA: 0x0008F3BF File Offset: 0x0008D5BF
		[ServerRpc(RequireOwnership = false)]
		private void SendRecipeField(int fieldIndex, int recipeIndex)
		{
			this.RpcWriter___Server_SendRecipeField_1692629761(fieldIndex, recipeIndex);
		}

		// Token: 0x060022B1 RID: 8881 RVA: 0x0008F3D0 File Offset: 0x0008D5D0
		[ObserversRpc]
		private void ReceiveRecipeField(int fieldIndex, int recipeIndex)
		{
			this.RpcWriter___Observers_ReceiveRecipeField_1692629761(fieldIndex, recipeIndex);
		}

		// Token: 0x060022B2 RID: 8882 RVA: 0x0008F3EB File Offset: 0x0008D5EB
		[ServerRpc(RequireOwnership = false)]
		private void SendNumberField(int fieldIndex, float value)
		{
			this.RpcWriter___Server_SendNumberField_1293284375(fieldIndex, value);
		}

		// Token: 0x060022B3 RID: 8883 RVA: 0x0008F3FB File Offset: 0x0008D5FB
		[ObserversRpc]
		private void ReceiveNumberField(int fieldIndex, float value)
		{
			this.RpcWriter___Observers_ReceiveNumberField_1293284375(fieldIndex, value);
		}

		// Token: 0x060022B4 RID: 8884 RVA: 0x0008F40B File Offset: 0x0008D60B
		[ServerRpc(RequireOwnership = false)]
		private void SendRouteListField(int fieldIndex, AdvancedTransitRouteData[] value)
		{
			this.RpcWriter___Server_SendRouteListField_3226448297(fieldIndex, value);
		}

		// Token: 0x060022B5 RID: 8885 RVA: 0x0008F41C File Offset: 0x0008D61C
		[ObserversRpc]
		private void ReceiveRouteListField(int fieldIndex, AdvancedTransitRouteData[] value)
		{
			this.RpcWriter___Observers_ReceiveRouteListField_3226448297(fieldIndex, value);
		}

		// Token: 0x060022B6 RID: 8886 RVA: 0x0008F437 File Offset: 0x0008D637
		[ServerRpc(RequireOwnership = false)]
		private void SendQualityField(int fieldIndex, EQuality quality)
		{
			this.RpcWriter___Server_SendQualityField_3536682170(fieldIndex, quality);
		}

		// Token: 0x060022B7 RID: 8887 RVA: 0x0008F447 File Offset: 0x0008D647
		[ObserversRpc]
		private void ReceiveQualityField(int fieldIndex, EQuality value)
		{
			this.RpcWriter___Observers_ReceiveQualityField_3536682170(fieldIndex, value);
		}

		// Token: 0x060022B9 RID: 8889 RVA: 0x0008F458 File Offset: 0x0008D658
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Management.ConfigurationReplicatorAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Management.ConfigurationReplicatorAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendItemField_2801973956));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveItemField_2801973956));
			base.RegisterServerRpc(2U, new ServerRpcDelegate(this.RpcReader___Server_SendNPCField_1687693739));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveNPCField_1687693739));
			base.RegisterServerRpc(4U, new ServerRpcDelegate(this.RpcReader___Server_SendObjectField_1687693739));
			base.RegisterObserversRpc(5U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveObjectField_1687693739));
			base.RegisterServerRpc(6U, new ServerRpcDelegate(this.RpcReader___Server_SendObjectListField_690244341));
			base.RegisterObserversRpc(7U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveObjectListField_690244341));
			base.RegisterServerRpc(8U, new ServerRpcDelegate(this.RpcReader___Server_SendRecipeField_1692629761));
			base.RegisterObserversRpc(9U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveRecipeField_1692629761));
			base.RegisterServerRpc(10U, new ServerRpcDelegate(this.RpcReader___Server_SendNumberField_1293284375));
			base.RegisterObserversRpc(11U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveNumberField_1293284375));
			base.RegisterServerRpc(12U, new ServerRpcDelegate(this.RpcReader___Server_SendRouteListField_3226448297));
			base.RegisterObserversRpc(13U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveRouteListField_3226448297));
			base.RegisterServerRpc(14U, new ServerRpcDelegate(this.RpcReader___Server_SendQualityField_3536682170));
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveQualityField_3536682170));
		}

		// Token: 0x060022BA RID: 8890 RVA: 0x0008F5E6 File Offset: 0x0008D7E6
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Management.ConfigurationReplicatorAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Management.ConfigurationReplicatorAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x060022BB RID: 8891 RVA: 0x0008F5F9 File Offset: 0x0008D7F9
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060022BC RID: 8892 RVA: 0x0008F608 File Offset: 0x0008D808
		private void RpcWriter___Server_SendItemField_2801973956(int fieldIndex, string value)
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
			writer.WriteInt32(fieldIndex, 1);
			writer.WriteString(value);
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060022BD RID: 8893 RVA: 0x0008F6C1 File Offset: 0x0008D8C1
		private void RpcLogic___SendItemField_2801973956(int fieldIndex, string value)
		{
			this.ReceiveItemField(fieldIndex, value);
		}

		// Token: 0x060022BE RID: 8894 RVA: 0x0008F6CC File Offset: 0x0008D8CC
		private void RpcReader___Server_SendItemField_2801973956(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int fieldIndex = PooledReader0.ReadInt32(1);
			string value = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendItemField_2801973956(fieldIndex, value);
		}

		// Token: 0x060022BF RID: 8895 RVA: 0x0008F714 File Offset: 0x0008D914
		private void RpcWriter___Observers_ReceiveItemField_2801973956(int fieldIndex, string value)
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
			writer.WriteInt32(fieldIndex, 1);
			writer.WriteString(value);
			base.SendObserversRpc(1U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060022C0 RID: 8896 RVA: 0x0008F7DC File Offset: 0x0008D9DC
		private void RpcLogic___ReceiveItemField_2801973956(int fieldIndex, string value)
		{
			ItemField itemField = this.Configuration.Fields[fieldIndex] as ItemField;
			ItemDefinition item = null;
			if (value != string.Empty)
			{
				item = Registry.GetItem(value);
			}
			itemField.SetItem(item, false);
		}

		// Token: 0x060022C1 RID: 8897 RVA: 0x0008F81C File Offset: 0x0008DA1C
		private void RpcReader___Observers_ReceiveItemField_2801973956(PooledReader PooledReader0, Channel channel)
		{
			int fieldIndex = PooledReader0.ReadInt32(1);
			string value = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveItemField_2801973956(fieldIndex, value);
		}

		// Token: 0x060022C2 RID: 8898 RVA: 0x0008F864 File Offset: 0x0008DA64
		private void RpcWriter___Server_SendNPCField_1687693739(int fieldIndex, NetworkObject npcObject)
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
			writer.WriteInt32(fieldIndex, 1);
			writer.WriteNetworkObject(npcObject);
			base.SendServerRpc(2U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060022C3 RID: 8899 RVA: 0x0008F91D File Offset: 0x0008DB1D
		private void RpcLogic___SendNPCField_1687693739(int fieldIndex, NetworkObject npcObject)
		{
			this.ReceiveNPCField(fieldIndex, npcObject);
		}

		// Token: 0x060022C4 RID: 8900 RVA: 0x0008F928 File Offset: 0x0008DB28
		private void RpcReader___Server_SendNPCField_1687693739(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int fieldIndex = PooledReader0.ReadInt32(1);
			NetworkObject npcObject = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendNPCField_1687693739(fieldIndex, npcObject);
		}

		// Token: 0x060022C5 RID: 8901 RVA: 0x0008F970 File Offset: 0x0008DB70
		private void RpcWriter___Observers_ReceiveNPCField_1687693739(int fieldIndex, NetworkObject npcObject)
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
			writer.WriteInt32(fieldIndex, 1);
			writer.WriteNetworkObject(npcObject);
			base.SendObserversRpc(3U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060022C6 RID: 8902 RVA: 0x0008FA38 File Offset: 0x0008DC38
		private void RpcLogic___ReceiveNPCField_1687693739(int fieldIndex, NetworkObject npcObject)
		{
			NPCField npcfield = this.Configuration.Fields[fieldIndex] as NPCField;
			NPC npc = null;
			if (npcObject != null)
			{
				npc = npcObject.GetComponent<NPC>();
			}
			npcfield.SetNPC(npc, false);
		}

		// Token: 0x060022C7 RID: 8903 RVA: 0x0008FA74 File Offset: 0x0008DC74
		private void RpcReader___Observers_ReceiveNPCField_1687693739(PooledReader PooledReader0, Channel channel)
		{
			int fieldIndex = PooledReader0.ReadInt32(1);
			NetworkObject npcObject = PooledReader0.ReadNetworkObject();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveNPCField_1687693739(fieldIndex, npcObject);
		}

		// Token: 0x060022C8 RID: 8904 RVA: 0x0008FABC File Offset: 0x0008DCBC
		private void RpcWriter___Server_SendObjectField_1687693739(int fieldIndex, NetworkObject obj)
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
			writer.WriteInt32(fieldIndex, 1);
			writer.WriteNetworkObject(obj);
			base.SendServerRpc(4U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060022C9 RID: 8905 RVA: 0x0008FB75 File Offset: 0x0008DD75
		private void RpcLogic___SendObjectField_1687693739(int fieldIndex, NetworkObject obj)
		{
			this.ReceiveObjectField(fieldIndex, obj);
		}

		// Token: 0x060022CA RID: 8906 RVA: 0x0008FB80 File Offset: 0x0008DD80
		private void RpcReader___Server_SendObjectField_1687693739(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int fieldIndex = PooledReader0.ReadInt32(1);
			NetworkObject obj = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendObjectField_1687693739(fieldIndex, obj);
		}

		// Token: 0x060022CB RID: 8907 RVA: 0x0008FBC8 File Offset: 0x0008DDC8
		private void RpcWriter___Observers_ReceiveObjectField_1687693739(int fieldIndex, NetworkObject obj)
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
			writer.WriteInt32(fieldIndex, 1);
			writer.WriteNetworkObject(obj);
			base.SendObserversRpc(5U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060022CC RID: 8908 RVA: 0x0008FC90 File Offset: 0x0008DE90
		private void RpcLogic___ReceiveObjectField_1687693739(int fieldIndex, NetworkObject obj)
		{
			ObjectField objectField = this.Configuration.Fields[fieldIndex] as ObjectField;
			BuildableItem obj2 = null;
			if (obj != null)
			{
				obj2 = obj.GetComponent<BuildableItem>();
			}
			objectField.SetObject(obj2, false);
		}

		// Token: 0x060022CD RID: 8909 RVA: 0x0008FCCC File Offset: 0x0008DECC
		private void RpcReader___Observers_ReceiveObjectField_1687693739(PooledReader PooledReader0, Channel channel)
		{
			int fieldIndex = PooledReader0.ReadInt32(1);
			NetworkObject obj = PooledReader0.ReadNetworkObject();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveObjectField_1687693739(fieldIndex, obj);
		}

		// Token: 0x060022CE RID: 8910 RVA: 0x0008FD14 File Offset: 0x0008DF14
		private void RpcWriter___Server_SendObjectListField_690244341(int fieldIndex, List<NetworkObject> objects)
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
			writer.WriteInt32(fieldIndex, 1);
			writer.Write___System.Collections.Generic.List`1<FishNet.Object.NetworkObject>FishNet.Serializing.Generated(objects);
			base.SendServerRpc(6U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060022CF RID: 8911 RVA: 0x0008FDCD File Offset: 0x0008DFCD
		private void RpcLogic___SendObjectListField_690244341(int fieldIndex, List<NetworkObject> objects)
		{
			this.ReceiveObjectListField(fieldIndex, objects);
		}

		// Token: 0x060022D0 RID: 8912 RVA: 0x0008FDD8 File Offset: 0x0008DFD8
		private void RpcReader___Server_SendObjectListField_690244341(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int fieldIndex = PooledReader0.ReadInt32(1);
			List<NetworkObject> objects = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<FishNet.Object.NetworkObject>FishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendObjectListField_690244341(fieldIndex, objects);
		}

		// Token: 0x060022D1 RID: 8913 RVA: 0x0008FE20 File Offset: 0x0008E020
		private void RpcWriter___Observers_ReceiveObjectListField_690244341(int fieldIndex, List<NetworkObject> objects)
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
			writer.WriteInt32(fieldIndex, 1);
			writer.Write___System.Collections.Generic.List`1<FishNet.Object.NetworkObject>FishNet.Serializing.Generated(objects);
			base.SendObserversRpc(7U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060022D2 RID: 8914 RVA: 0x0008FEE8 File Offset: 0x0008E0E8
		private void RpcLogic___ReceiveObjectListField_690244341(int fieldIndex, List<NetworkObject> objects)
		{
			ObjectListField objectListField = this.Configuration.Fields[fieldIndex] as ObjectListField;
			List<BuildableItem> list = new List<BuildableItem>();
			for (int i = 0; i < objects.Count; i++)
			{
				list.Add(objects[i].GetComponent<BuildableItem>());
			}
			objectListField.SetList(list, false);
		}

		// Token: 0x060022D3 RID: 8915 RVA: 0x0008FF40 File Offset: 0x0008E140
		private void RpcReader___Observers_ReceiveObjectListField_690244341(PooledReader PooledReader0, Channel channel)
		{
			int fieldIndex = PooledReader0.ReadInt32(1);
			List<NetworkObject> objects = GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<FishNet.Object.NetworkObject>FishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveObjectListField_690244341(fieldIndex, objects);
		}

		// Token: 0x060022D4 RID: 8916 RVA: 0x0008FF88 File Offset: 0x0008E188
		private void RpcWriter___Server_SendRecipeField_1692629761(int fieldIndex, int recipeIndex)
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
			writer.WriteInt32(fieldIndex, 1);
			writer.WriteInt32(recipeIndex, 1);
			base.SendServerRpc(8U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060022D5 RID: 8917 RVA: 0x00090046 File Offset: 0x0008E246
		private void RpcLogic___SendRecipeField_1692629761(int fieldIndex, int recipeIndex)
		{
			this.ReceiveRecipeField(fieldIndex, recipeIndex);
		}

		// Token: 0x060022D6 RID: 8918 RVA: 0x00090050 File Offset: 0x0008E250
		private void RpcReader___Server_SendRecipeField_1692629761(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int fieldIndex = PooledReader0.ReadInt32(1);
			int recipeIndex = PooledReader0.ReadInt32(1);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendRecipeField_1692629761(fieldIndex, recipeIndex);
		}

		// Token: 0x060022D7 RID: 8919 RVA: 0x0009009C File Offset: 0x0008E29C
		private void RpcWriter___Observers_ReceiveRecipeField_1692629761(int fieldIndex, int recipeIndex)
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
			writer.WriteInt32(fieldIndex, 1);
			writer.WriteInt32(recipeIndex, 1);
			base.SendObserversRpc(9U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060022D8 RID: 8920 RVA: 0x0009016C File Offset: 0x0008E36C
		private void RpcLogic___ReceiveRecipeField_1692629761(int fieldIndex, int recipeIndex)
		{
			StationRecipeField stationRecipeField = this.Configuration.Fields[fieldIndex] as StationRecipeField;
			StationRecipe recipe = null;
			if (recipeIndex != -1)
			{
				recipe = stationRecipeField.Options[recipeIndex];
			}
			stationRecipeField.SetRecipe(recipe, false);
		}

		// Token: 0x060022D9 RID: 8921 RVA: 0x000901AC File Offset: 0x0008E3AC
		private void RpcReader___Observers_ReceiveRecipeField_1692629761(PooledReader PooledReader0, Channel channel)
		{
			int fieldIndex = PooledReader0.ReadInt32(1);
			int recipeIndex = PooledReader0.ReadInt32(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveRecipeField_1692629761(fieldIndex, recipeIndex);
		}

		// Token: 0x060022DA RID: 8922 RVA: 0x000901F8 File Offset: 0x0008E3F8
		private void RpcWriter___Server_SendNumberField_1293284375(int fieldIndex, float value)
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
			writer.WriteInt32(fieldIndex, 1);
			writer.WriteSingle(value, 0);
			base.SendServerRpc(10U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060022DB RID: 8923 RVA: 0x000902B6 File Offset: 0x0008E4B6
		private void RpcLogic___SendNumberField_1293284375(int fieldIndex, float value)
		{
			this.ReceiveNumberField(fieldIndex, value);
		}

		// Token: 0x060022DC RID: 8924 RVA: 0x000902C0 File Offset: 0x0008E4C0
		private void RpcReader___Server_SendNumberField_1293284375(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int fieldIndex = PooledReader0.ReadInt32(1);
			float value = PooledReader0.ReadSingle(0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendNumberField_1293284375(fieldIndex, value);
		}

		// Token: 0x060022DD RID: 8925 RVA: 0x0009030C File Offset: 0x0008E50C
		private void RpcWriter___Observers_ReceiveNumberField_1293284375(int fieldIndex, float value)
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
			writer.WriteInt32(fieldIndex, 1);
			writer.WriteSingle(value, 0);
			base.SendObserversRpc(11U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060022DE RID: 8926 RVA: 0x000903D9 File Offset: 0x0008E5D9
		private void RpcLogic___ReceiveNumberField_1293284375(int fieldIndex, float value)
		{
			(this.Configuration.Fields[fieldIndex] as NumberField).SetValue(value, false);
		}

		// Token: 0x060022DF RID: 8927 RVA: 0x000903F8 File Offset: 0x0008E5F8
		private void RpcReader___Observers_ReceiveNumberField_1293284375(PooledReader PooledReader0, Channel channel)
		{
			int fieldIndex = PooledReader0.ReadInt32(1);
			float value = PooledReader0.ReadSingle(0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveNumberField_1293284375(fieldIndex, value);
		}

		// Token: 0x060022E0 RID: 8928 RVA: 0x00090444 File Offset: 0x0008E644
		private void RpcWriter___Server_SendRouteListField_3226448297(int fieldIndex, AdvancedTransitRouteData[] value)
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
			writer.WriteInt32(fieldIndex, 1);
			writer.Write___ScheduleOne.Persistence.Datas.AdvancedTransitRouteData[]FishNet.Serializing.Generated(value);
			base.SendServerRpc(12U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060022E1 RID: 8929 RVA: 0x000904FD File Offset: 0x0008E6FD
		private void RpcLogic___SendRouteListField_3226448297(int fieldIndex, AdvancedTransitRouteData[] value)
		{
			this.ReceiveRouteListField(fieldIndex, value);
		}

		// Token: 0x060022E2 RID: 8930 RVA: 0x00090508 File Offset: 0x0008E708
		private void RpcReader___Server_SendRouteListField_3226448297(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int fieldIndex = PooledReader0.ReadInt32(1);
			AdvancedTransitRouteData[] value = GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.AdvancedTransitRouteData[]FishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendRouteListField_3226448297(fieldIndex, value);
		}

		// Token: 0x060022E3 RID: 8931 RVA: 0x00090550 File Offset: 0x0008E750
		private void RpcWriter___Observers_ReceiveRouteListField_3226448297(int fieldIndex, AdvancedTransitRouteData[] value)
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
			writer.WriteInt32(fieldIndex, 1);
			writer.Write___ScheduleOne.Persistence.Datas.AdvancedTransitRouteData[]FishNet.Serializing.Generated(value);
			base.SendObserversRpc(13U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060022E4 RID: 8932 RVA: 0x00090618 File Offset: 0x0008E818
		private void RpcLogic___ReceiveRouteListField_3226448297(int fieldIndex, AdvancedTransitRouteData[] value)
		{
			(this.Configuration.Fields[fieldIndex] as RouteListField).SetList((from x in value
			select new AdvancedTransitRoute(x)).ToList<AdvancedTransitRoute>(), false, false);
		}

		// Token: 0x060022E5 RID: 8933 RVA: 0x0009066C File Offset: 0x0008E86C
		private void RpcReader___Observers_ReceiveRouteListField_3226448297(PooledReader PooledReader0, Channel channel)
		{
			int fieldIndex = PooledReader0.ReadInt32(1);
			AdvancedTransitRouteData[] value = GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.AdvancedTransitRouteData[]FishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveRouteListField_3226448297(fieldIndex, value);
		}

		// Token: 0x060022E6 RID: 8934 RVA: 0x000906B4 File Offset: 0x0008E8B4
		private void RpcWriter___Server_SendQualityField_3536682170(int fieldIndex, EQuality quality)
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
			writer.WriteInt32(fieldIndex, 1);
			writer.Write___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generated(quality);
			base.SendServerRpc(14U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060022E7 RID: 8935 RVA: 0x0009076D File Offset: 0x0008E96D
		private void RpcLogic___SendQualityField_3536682170(int fieldIndex, EQuality quality)
		{
			this.ReceiveQualityField(fieldIndex, quality);
		}

		// Token: 0x060022E8 RID: 8936 RVA: 0x00090778 File Offset: 0x0008E978
		private void RpcReader___Server_SendQualityField_3536682170(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int fieldIndex = PooledReader0.ReadInt32(1);
			EQuality quality = GeneratedReaders___Internal.Read___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendQualityField_3536682170(fieldIndex, quality);
		}

		// Token: 0x060022E9 RID: 8937 RVA: 0x000907C0 File Offset: 0x0008E9C0
		private void RpcWriter___Observers_ReceiveQualityField_3536682170(int fieldIndex, EQuality value)
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
			writer.WriteInt32(fieldIndex, 1);
			writer.Write___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generated(value);
			base.SendObserversRpc(15U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060022EA RID: 8938 RVA: 0x00090888 File Offset: 0x0008EA88
		private void RpcLogic___ReceiveQualityField_3536682170(int fieldIndex, EQuality value)
		{
			(this.Configuration.Fields[fieldIndex] as QualityField).SetValue(value, false);
		}

		// Token: 0x060022EB RID: 8939 RVA: 0x000908A8 File Offset: 0x0008EAA8
		private void RpcReader___Observers_ReceiveQualityField_3536682170(PooledReader PooledReader0, Channel channel)
		{
			int fieldIndex = PooledReader0.ReadInt32(1);
			EQuality value = GeneratedReaders___Internal.Read___ScheduleOne.ItemFramework.EQualityFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveQualityField_3536682170(fieldIndex, value);
		}

		// Token: 0x060022EC RID: 8940 RVA: 0x0008F5F9 File Offset: 0x0008D7F9
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001A57 RID: 6743
		public EntityConfiguration Configuration;

		// Token: 0x04001A58 RID: 6744
		private bool dll_Excuted;

		// Token: 0x04001A59 RID: 6745
		private bool dll_Excuted;
	}
}
