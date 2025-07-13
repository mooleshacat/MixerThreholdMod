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
using ScheduleOne.AvatarFramework;
using ScheduleOne.DevUtilities;
using ScheduleOne.Property;
using ScheduleOne.Quests;
using ScheduleOne.Variables;
using ScheduleOne.VoiceOver;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScheduleOne.Employees
{
	// Token: 0x02000685 RID: 1669
	public class EmployeeManager : NetworkSingleton<EmployeeManager>
	{
		// Token: 0x06002CAA RID: 11434 RVA: 0x000B8454 File Offset: 0x000B6654
		public void CreateNewEmployee(Property property, EEmployeeType type)
		{
			bool male = 0.67f > UnityEngine.Random.Range(0f, 1f);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("LifetimeEmployeesRecruited", (NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("LifetimeEmployeesRecruited") + 1f).ToString(), true);
			string text;
			string text2;
			this.GenerateRandomName(male, out text, out text2);
			string id = text.ToLower() + "_" + text2.ToLower();
			int appearanceIndex;
			AvatarSettings avatarSettings;
			this.GetRandomAppearance(male, out appearanceIndex, out avatarSettings);
			string guid = GUIDManager.GenerateUniqueGUID().ToString();
			this.CreateEmployee(property, type, text, text2, id, male, appearanceIndex, property.NPCSpawnPoint.position, property.NPCSpawnPoint.rotation, guid);
		}

		// Token: 0x06002CAB RID: 11435 RVA: 0x000B8510 File Offset: 0x000B6710
		[ServerRpc(RequireOwnership = false)]
		public void CreateEmployee(Property property, EEmployeeType type, string firstName, string lastName, string id, bool male, int appearanceIndex, Vector3 position, Quaternion rotation, string guid = "")
		{
			this.RpcWriter___Server_CreateEmployee_311954683(property, type, firstName, lastName, id, male, appearanceIndex, position, rotation, guid);
		}

		// Token: 0x06002CAC RID: 11436 RVA: 0x000B854C File Offset: 0x000B674C
		public Employee CreateEmployee_Server(Property property, EEmployeeType type, string firstName, string lastName, string id, bool male, int appearanceIndex, Vector3 position, Quaternion rotation, string guid)
		{
			if (property.Employees.Count >= property.EmployeeCapacity)
			{
				Console.LogError("Property " + property.PropertyCode + " is at capacity.", null);
				return null;
			}
			Employee employeePrefab = this.GetEmployeePrefab(type);
			if (employeePrefab == null)
			{
				Console.LogError("Failed to find employee prefab for " + type.ToString(), null);
				return null;
			}
			guid = ((guid == "") ? Guid.NewGuid().ToString() : guid);
			if (!this.IsPositionValid(position))
			{
				position = property.NPCSpawnPoint.position;
			}
			if (!this.IsRotationValid(rotation))
			{
				rotation = property.NPCSpawnPoint.rotation;
			}
			Employee component = UnityEngine.Object.Instantiate<Employee>(employeePrefab, position, rotation).GetComponent<Employee>();
			component.Initialize(null, firstName, lastName, id, guid, property.PropertyCode, male, appearanceIndex);
			base.NetworkObject.Spawn(component.gameObject, null, default(Scene));
			component.Movement.Warp(position);
			component.Movement.transform.rotation = rotation;
			Quest quest = this.EmployeeQuests.FirstOrDefault((Quest_Employees x) => x.EmployeeType == type);
			if (quest != null && quest.QuestState == EQuestState.Inactive)
			{
				quest.Begin(true);
			}
			return component;
		}

		// Token: 0x06002CAD RID: 11437 RVA: 0x000B86B9 File Offset: 0x000B68B9
		private bool IsPositionValid(Vector3 position)
		{
			return this.IsFloatValid(position.x) && this.IsFloatValid(position.y) && this.IsFloatValid(position.z);
		}

		// Token: 0x06002CAE RID: 11438 RVA: 0x000B86E5 File Offset: 0x000B68E5
		private bool IsRotationValid(Quaternion rotation)
		{
			return this.IsFloatValid(rotation.x) && this.IsFloatValid(rotation.y) && this.IsFloatValid(rotation.z) && this.IsFloatValid(rotation.w);
		}

		// Token: 0x06002CAF RID: 11439 RVA: 0x000B871F File Offset: 0x000B691F
		private bool IsFloatValid(float value)
		{
			return !float.IsNaN(value) && !float.IsInfinity(value);
		}

		// Token: 0x06002CB0 RID: 11440 RVA: 0x000B8734 File Offset: 0x000B6934
		public void RegisterName(string name)
		{
			this.takenNames.Add(name);
		}

		// Token: 0x06002CB1 RID: 11441 RVA: 0x000B8742 File Offset: 0x000B6942
		public void RegisterAppearance(bool male, int index)
		{
			if (male)
			{
				this.takenMaleAppearances.Add(index);
				return;
			}
			this.takenFemaleAppearances.Add(index);
		}

		// Token: 0x06002CB2 RID: 11442 RVA: 0x000B8760 File Offset: 0x000B6960
		public void GenerateRandomName(bool male, out string firstName, out string lastName)
		{
			do
			{
				if (male)
				{
					firstName = this.MaleFirstNames[UnityEngine.Random.Range(0, this.MaleFirstNames.Length)].ToString();
				}
				else
				{
					firstName = this.FemaleFirstNames[UnityEngine.Random.Range(0, this.FemaleFirstNames.Length)].ToString();
				}
				lastName = this.LastNames[UnityEngine.Random.Range(0, this.LastNames.Length)].ToString();
			}
			while (this.takenNames.Contains(firstName + " " + lastName));
		}

		// Token: 0x06002CB3 RID: 11443 RVA: 0x000B87E1 File Offset: 0x000B69E1
		public EmployeeManager.EmployeeAppearance GetAppearance(bool male, int index)
		{
			if (!male)
			{
				return this.FemaleAppearances[index];
			}
			return this.MaleAppearances[index];
		}

		// Token: 0x06002CB4 RID: 11444 RVA: 0x000B87FF File Offset: 0x000B69FF
		public VODatabase GetVoice(bool male, int index)
		{
			if (!male)
			{
				return this.FemaleVoices[index % this.FemaleVoices.Length];
			}
			return this.MaleVoices[index % this.MaleVoices.Length];
		}

		// Token: 0x06002CB5 RID: 11445 RVA: 0x000B8828 File Offset: 0x000B6A28
		public void GetRandomAppearance(bool male, out int index, out AvatarSettings settings)
		{
			List<EmployeeManager.EmployeeAppearance> list = male ? this.MaleAppearances : this.FemaleAppearances;
			List<int> list2 = male ? this.takenMaleAppearances : this.takenFemaleAppearances;
			index = UnityEngine.Random.Range(0, list.Count);
			settings = list[index].Settings;
			if (list2.Count >= list.Count)
			{
				return;
			}
			int num = 0;
			while (list2.Contains(index))
			{
				index++;
				if (index >= list.Count)
				{
					index = 0;
				}
				num++;
				if (num >= list.Count)
				{
					settings = list[index].Settings;
					return;
				}
			}
			settings = list[index].Settings;
		}

		// Token: 0x06002CB6 RID: 11446 RVA: 0x000B88D4 File Offset: 0x000B6AD4
		public Employee GetEmployeePrefab(EEmployeeType type)
		{
			switch (type)
			{
			case EEmployeeType.Botanist:
				return this.BotanistPrefab;
			case EEmployeeType.Handler:
				return this.PackagerPrefab;
			case EEmployeeType.Chemist:
				return this.ChemistPrefab;
			case EEmployeeType.Cleaner:
				return this.CleanerPrefab;
			default:
				Console.LogError("Employee type not found: " + type.ToString(), null);
				return null;
			}
		}

		// Token: 0x06002CB7 RID: 11447 RVA: 0x000B8934 File Offset: 0x000B6B34
		public List<Employee> GetEmployeesByType(EEmployeeType type)
		{
			List<Employee> list = new List<Employee>();
			foreach (Employee employee in this.AllEmployees)
			{
				if (employee.EmployeeType == type)
				{
					list.Add(employee);
				}
			}
			return list;
		}

		// Token: 0x06002CB9 RID: 11449 RVA: 0x000B89CC File Offset: 0x000B6BCC
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Employees.EmployeeManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Employees.EmployeeManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_CreateEmployee_311954683));
		}

		// Token: 0x06002CBA RID: 11450 RVA: 0x000B89FC File Offset: 0x000B6BFC
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Employees.EmployeeManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Employees.EmployeeManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06002CBB RID: 11451 RVA: 0x000B8A15 File Offset: 0x000B6C15
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002CBC RID: 11452 RVA: 0x000B8A24 File Offset: 0x000B6C24
		private void RpcWriter___Server_CreateEmployee_311954683(Property property, EEmployeeType type, string firstName, string lastName, string id, bool male, int appearanceIndex, Vector3 position, Quaternion rotation, string guid = "")
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
			writer.Write___ScheduleOne.Property.PropertyFishNet.Serializing.Generated(property);
			writer.Write___ScheduleOne.Employees.EEmployeeTypeFishNet.Serializing.Generated(type);
			writer.WriteString(firstName);
			writer.WriteString(lastName);
			writer.WriteString(id);
			writer.WriteBoolean(male);
			writer.WriteInt32(appearanceIndex, 1);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation, 1);
			writer.WriteString(guid);
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002CBD RID: 11453 RVA: 0x000B8B4C File Offset: 0x000B6D4C
		public void RpcLogic___CreateEmployee_311954683(Property property, EEmployeeType type, string firstName, string lastName, string id, bool male, int appearanceIndex, Vector3 position, Quaternion rotation, string guid = "")
		{
			this.CreateEmployee_Server(property, type, firstName, lastName, id, male, appearanceIndex, position, rotation, guid);
		}

		// Token: 0x06002CBE RID: 11454 RVA: 0x000B8B74 File Offset: 0x000B6D74
		private void RpcReader___Server_CreateEmployee_311954683(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			Property property = GeneratedReaders___Internal.Read___ScheduleOne.Property.PropertyFishNet.Serializing.Generateds(PooledReader0);
			EEmployeeType type = GeneratedReaders___Internal.Read___ScheduleOne.Employees.EEmployeeTypeFishNet.Serializing.Generateds(PooledReader0);
			string firstName = PooledReader0.ReadString();
			string lastName = PooledReader0.ReadString();
			string id = PooledReader0.ReadString();
			bool male = PooledReader0.ReadBoolean();
			int appearanceIndex = PooledReader0.ReadInt32(1);
			Vector3 position = PooledReader0.ReadVector3();
			Quaternion rotation = PooledReader0.ReadQuaternion(1);
			string guid = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___CreateEmployee_311954683(property, type, firstName, lastName, id, male, appearanceIndex, position, rotation, guid);
		}

		// Token: 0x06002CBF RID: 11455 RVA: 0x000B8C48 File Offset: 0x000B6E48
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001FF1 RID: 8177
		public const float MALE_EMPLOYEE_CHANCE = 0.67f;

		// Token: 0x04001FF2 RID: 8178
		public List<Employee> AllEmployees = new List<Employee>();

		// Token: 0x04001FF3 RID: 8179
		public Quest_Employees[] EmployeeQuests;

		// Token: 0x04001FF4 RID: 8180
		[Header("Prefabs")]
		public Botanist BotanistPrefab;

		// Token: 0x04001FF5 RID: 8181
		public Packager PackagerPrefab;

		// Token: 0x04001FF6 RID: 8182
		public Chemist ChemistPrefab;

		// Token: 0x04001FF7 RID: 8183
		public Cleaner CleanerPrefab;

		// Token: 0x04001FF8 RID: 8184
		[Header("Appearances")]
		public List<EmployeeManager.EmployeeAppearance> MaleAppearances;

		// Token: 0x04001FF9 RID: 8185
		public List<EmployeeManager.EmployeeAppearance> FemaleAppearances;

		// Token: 0x04001FFA RID: 8186
		[Header("Voices")]
		public VODatabase[] MaleVoices;

		// Token: 0x04001FFB RID: 8187
		public VODatabase[] FemaleVoices;

		// Token: 0x04001FFC RID: 8188
		[Header("Names")]
		public string[] MaleFirstNames;

		// Token: 0x04001FFD RID: 8189
		public string[] FemaleFirstNames;

		// Token: 0x04001FFE RID: 8190
		public string[] LastNames;

		// Token: 0x04001FFF RID: 8191
		private List<string> takenNames = new List<string>();

		// Token: 0x04002000 RID: 8192
		private List<int> takenMaleAppearances = new List<int>();

		// Token: 0x04002001 RID: 8193
		private List<int> takenFemaleAppearances = new List<int>();

		// Token: 0x04002002 RID: 8194
		private bool dll_Excuted;

		// Token: 0x04002003 RID: 8195
		private bool dll_Excuted;

		// Token: 0x02000686 RID: 1670
		[Serializable]
		public class EmployeeAppearance
		{
			// Token: 0x04002004 RID: 8196
			public AvatarSettings Settings;

			// Token: 0x04002005 RID: 8197
			public Sprite Mugshot;
		}
	}
}
