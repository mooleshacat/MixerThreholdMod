using System;
using System.Collections.Generic;
using EasyButtons;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Variables
{
	// Token: 0x020002A1 RID: 673
	public class VariableDatabase : NetworkSingleton<VariableDatabase>, IBaseSaveable, ISaveable
	{
		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000DEB RID: 3563 RVA: 0x0003DA11 File Offset: 0x0003BC11
		public string SaveFolderName
		{
			get
			{
				return "Variables";
			}
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000DEC RID: 3564 RVA: 0x0003DA11 File Offset: 0x0003BC11
		public string SaveFileName
		{
			get
			{
				return "Variables";
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000DED RID: 3565 RVA: 0x0003DA18 File Offset: 0x0003BC18
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000DEE RID: 3566 RVA: 0x00014B5A File Offset: 0x00012D5A
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000DEF RID: 3567 RVA: 0x0003DA20 File Offset: 0x0003BC20
		// (set) Token: 0x06000DF0 RID: 3568 RVA: 0x0003DA28 File Offset: 0x0003BC28
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000DF1 RID: 3569 RVA: 0x0003DA31 File Offset: 0x0003BC31
		// (set) Token: 0x06000DF2 RID: 3570 RVA: 0x0003DA39 File Offset: 0x0003BC39
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000DF3 RID: 3571 RVA: 0x0003DA42 File Offset: 0x0003BC42
		// (set) Token: 0x06000DF4 RID: 3572 RVA: 0x0003DA4A File Offset: 0x0003BC4A
		public bool HasChanged { get; set; } = true;

		// Token: 0x06000DF5 RID: 3573 RVA: 0x0003DA54 File Offset: 0x0003BC54
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Variables.VariableDatabase_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x0003DA73 File Offset: 0x0003BC73
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x0003DA80 File Offset: 0x0003BC80
		private void CreateVariables()
		{
			for (int i = 0; i < this.Creators.Length; i++)
			{
				if (this.Creators[i].Mode == EVariableMode.Player)
				{
					this.playerVariables.Add(this.Creators[i].Name.ToLower());
				}
				else
				{
					this.CreateVariable(this.Creators[i].Name, this.Creators[i].Type, this.Creators[i].InitialValue, this.Creators[i].Persistent, EVariableMode.Global, null, EVariableReplicationMode.Networked);
				}
			}
			this.SetVariableValue("IsDemo", false.ToString(), true);
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x0003DB24 File Offset: 0x0003BD24
		public void CreatePlayerVariables(Player owner)
		{
			for (int i = 0; i < this.Creators.Length; i++)
			{
				if (this.Creators[i].Mode == EVariableMode.Player)
				{
					this.CreateVariable(this.Creators[i].Name, this.Creators[i].Type, this.Creators[i].InitialValue, this.Creators[i].Persistent, EVariableMode.Player, owner, EVariableReplicationMode.Local);
				}
			}
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x0003DB94 File Offset: 0x0003BD94
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (connection.IsHost)
			{
				return;
			}
			for (int i = 0; i < this.VariableList.Count; i++)
			{
				if (this.VariableList[i].ReplicationMode != EVariableReplicationMode.Local)
				{
					this.VariableList[i].ReplicateValue(connection);
				}
			}
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x0003DBEC File Offset: 0x0003BDEC
		public void CreateVariable(string name, VariableDatabase.EVariableType type, string initialValue, bool persistent, EVariableMode mode, Player owner, EVariableReplicationMode replicationMode = EVariableReplicationMode.Networked)
		{
			if (type == VariableDatabase.EVariableType.Bool)
			{
				new BoolVariable(name, replicationMode, persistent, mode, owner, initialValue == "true");
				return;
			}
			if (type != VariableDatabase.EVariableType.Number)
			{
				return;
			}
			float num;
			float value = float.TryParse(initialValue, out num) ? num : 0f;
			new NumberVariable(name, replicationMode, persistent, mode, owner, value);
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x0003DC48 File Offset: 0x0003BE48
		public void AddVariable(BaseVariable variable)
		{
			if (this.VariableDict.ContainsKey(variable.Name))
			{
				Console.LogError("Variable with name " + variable.Name + " already exists in the database.", null);
				return;
			}
			this.VariableList.Add(variable);
			this.VariableDict.Add(variable.Name.ToLower(), variable);
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x0003DCA7 File Offset: 0x0003BEA7
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendValue(NetworkConnection conn, string variableName, string value)
		{
			this.RpcWriter___Server_SendValue_3895153758(conn, variableName, value);
			this.RpcLogic___SendValue_3895153758(conn, variableName, value);
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x0003DCCD File Offset: 0x0003BECD
		[ObserversRpc]
		[TargetRpc]
		public void ReceiveValue(NetworkConnection conn, string variableName, string value)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_ReceiveValue_3895153758(conn, variableName, value);
			}
			else
			{
				this.RpcWriter___Target_ReceiveValue_3895153758(conn, variableName, value);
			}
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x0003DD00 File Offset: 0x0003BF00
		public void SetVariableValue(string variableName, string value, bool network = true)
		{
			variableName = variableName.ToLower();
			if (this.playerVariables.Contains(variableName))
			{
				Player.Local.SetVariableValue(variableName, value, network);
				return;
			}
			if (this.VariableDict.ContainsKey(variableName))
			{
				this.VariableDict[variableName].SetValue(value, network);
				return;
			}
			Console.LogWarning("Failed to find variable with name: " + variableName, null);
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x0003DD64 File Offset: 0x0003BF64
		public BaseVariable GetVariable(string variableName)
		{
			variableName = variableName.ToLower();
			if (this.playerVariables.Contains(variableName))
			{
				return Player.Local.GetVariable(variableName);
			}
			if (this.VariableDict.ContainsKey(variableName))
			{
				return this.VariableDict[variableName];
			}
			Console.LogWarning("Failed to find variable with name: " + variableName, null);
			return null;
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x0003DDC0 File Offset: 0x0003BFC0
		public T GetValue<T>(string variableName)
		{
			variableName = variableName.ToLower();
			if (this.playerVariables.Contains(variableName))
			{
				return Player.Local.GetValue<T>(variableName);
			}
			if (this.VariableDict.ContainsKey(variableName))
			{
				return (T)((object)this.VariableDict[variableName].GetValue());
			}
			Console.LogError("Variable with name " + variableName + " does not exist in the database.", null);
			return default(T);
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x0003DE34 File Offset: 0x0003C034
		[Button]
		public void PrintAllVariables()
		{
			for (int i = 0; i < this.VariableList.Count; i++)
			{
				this.PrintVariableValue(this.VariableList[i].Name);
			}
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x0003DE70 File Offset: 0x0003C070
		public void PrintVariableValue(string variableName)
		{
			variableName = variableName.ToLower();
			if (this.VariableDict.ContainsKey(variableName))
			{
				string str = "Value of ";
				string str2 = variableName;
				string str3 = ": ";
				object value = this.VariableDict[variableName].GetValue();
				Console.Log(str + str2 + str3 + ((value != null) ? value.ToString() : null), null);
				return;
			}
			Console.LogError("Variable with name " + variableName + " does not exist in the database.", null);
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x0003DEE0 File Offset: 0x0003C0E0
		public void NotifyItemAcquired(string id, int quantity)
		{
			if (this.VariableDict.ContainsKey(id + "_acquired"))
			{
				float value = this.GetValue<float>(id + "_acquired");
				this.SetVariableValue(id + "_acquired", (value + (float)quantity).ToString(), true);
			}
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x0003DF38 File Offset: 0x0003C138
		public virtual string GetSaveString()
		{
			List<VariableData> list = new List<VariableData>();
			for (int i = 0; i < this.VariableList.Count; i++)
			{
				if (this.VariableList[i] != null && this.VariableList[i].Persistent && this.VariableList[i].VariableMode != EVariableMode.Player)
				{
					list.Add(new VariableData(this.VariableList[i].Name, this.VariableList[i].GetValue().ToString()));
				}
			}
			return new VariableCollectionData(list.ToArray()).GetJson(true);
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x0003DFDC File Offset: 0x0003C1DC
		public void LoadVariable(VariableData data)
		{
			if (this.playerVariables.Contains(data.Name.ToLower()))
			{
				Console.Log("Player variable: " + data.Name + " loaded from database. Redirecting to player.", null);
				Player.Local.SetVariableValue(data.Name, data.Value, false);
				return;
			}
			BaseVariable variable = this.GetVariable(data.Name);
			if (variable == null)
			{
				Console.LogWarning("Failed to find variable with name: " + data.Name, null);
				return;
			}
			variable.SetValue(data.Value, true);
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x0003E0C4 File Offset: 0x0003C2C4
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Variables.VariableDatabaseAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Variables.VariableDatabaseAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendValue_3895153758));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveValue_3895153758));
			base.RegisterTargetRpc(2U, new ClientRpcDelegate(this.RpcReader___Target_ReceiveValue_3895153758));
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x0003E12D File Offset: 0x0003C32D
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Variables.VariableDatabaseAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Variables.VariableDatabaseAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x0003E146 File Offset: 0x0003C346
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x0003E154 File Offset: 0x0003C354
		private void RpcWriter___Server_SendValue_3895153758(NetworkConnection conn, string variableName, string value)
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
			writer.WriteNetworkConnection(conn);
			writer.WriteString(variableName);
			writer.WriteString(value);
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x0003E215 File Offset: 0x0003C415
		public void RpcLogic___SendValue_3895153758(NetworkConnection conn, string variableName, string value)
		{
			this.ReceiveValue(conn, variableName, value);
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x0003E220 File Offset: 0x0003C420
		private void RpcReader___Server_SendValue_3895153758(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkConnection conn2 = PooledReader0.ReadNetworkConnection();
			string variableName = PooledReader0.ReadString();
			string value = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendValue_3895153758(conn2, variableName, value);
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x0003E280 File Offset: 0x0003C480
		private void RpcWriter___Observers_ReceiveValue_3895153758(NetworkConnection conn, string variableName, string value)
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
			writer.WriteString(variableName);
			writer.WriteString(value);
			base.SendObserversRpc(1U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x0003E343 File Offset: 0x0003C543
		public void RpcLogic___ReceiveValue_3895153758(NetworkConnection conn, string variableName, string value)
		{
			variableName = variableName.ToLower();
			if (this.VariableDict.ContainsKey(variableName))
			{
				this.VariableDict[variableName].SetValue(value, false);
			}
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x0003E370 File Offset: 0x0003C570
		private void RpcReader___Observers_ReceiveValue_3895153758(PooledReader PooledReader0, Channel channel)
		{
			string variableName = PooledReader0.ReadString();
			string value = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveValue_3895153758(null, variableName, value);
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x0003E3B4 File Offset: 0x0003C5B4
		private void RpcWriter___Target_ReceiveValue_3895153758(NetworkConnection conn, string variableName, string value)
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
			writer.WriteString(variableName);
			writer.WriteString(value);
			base.SendTargetRpc(2U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x0003E478 File Offset: 0x0003C678
		private void RpcReader___Target_ReceiveValue_3895153758(PooledReader PooledReader0, Channel channel)
		{
			string variableName = PooledReader0.ReadString();
			string value = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveValue_3895153758(base.LocalConnection, variableName, value);
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x0003E4C0 File Offset: 0x0003C6C0
		protected override void dll()
		{
			base.Awake();
			List<VariableCreator> list = new List<VariableCreator>(this.Creators);
			for (int i = 0; i < this.ItemsToTrackAcquire.Length; i++)
			{
				list.Add(new VariableCreator
				{
					InitialValue = "0",
					Mode = EVariableMode.Global,
					Type = VariableDatabase.EVariableType.Number,
					Persistent = true,
					Name = this.ItemsToTrackAcquire[i].ID + "_acquired"
				});
			}
			this.Creators = list.ToArray();
			this.CreateVariables();
			this.InitializeSaveable();
		}

		// Token: 0x04000E61 RID: 3681
		public List<BaseVariable> VariableList = new List<BaseVariable>();

		// Token: 0x04000E62 RID: 3682
		public Dictionary<string, BaseVariable> VariableDict = new Dictionary<string, BaseVariable>();

		// Token: 0x04000E63 RID: 3683
		private List<string> playerVariables = new List<string>();

		// Token: 0x04000E64 RID: 3684
		public VariableCreator[] Creators;

		// Token: 0x04000E65 RID: 3685
		public StorableItemDefinition[] ItemsToTrackAcquire;

		// Token: 0x04000E66 RID: 3686
		private VariablesLoader loader = new VariablesLoader();

		// Token: 0x04000E6A RID: 3690
		private bool dll_Excuted;

		// Token: 0x04000E6B RID: 3691
		private bool dll_Excuted;

		// Token: 0x020002A2 RID: 674
		public enum EVariableType
		{
			// Token: 0x04000E6D RID: 3693
			Bool,
			// Token: 0x04000E6E RID: 3694
			Number
		}
	}
}
