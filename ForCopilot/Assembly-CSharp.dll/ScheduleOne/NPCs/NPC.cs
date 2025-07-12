using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using EPOOutline;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Object.Synchronizing;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using FluffyUnderware.DevTools.Extensions;
using ScheduleOne.AvatarFramework;
using ScheduleOne.AvatarFramework.Animation;
using ScheduleOne.AvatarFramework.Equipping;
using ScheduleOne.Combat;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dialogue;
using ScheduleOne.Doors;
using ScheduleOne.Economy;
using ScheduleOne.EntityFramework;
using ScheduleOne.GameTime;
using ScheduleOne.Interaction;
using ScheduleOne.ItemFramework;
using ScheduleOne.Map;
using ScheduleOne.Messaging;
using ScheduleOne.NPCs.Actions;
using ScheduleOne.NPCs.Behaviour;
using ScheduleOne.NPCs.Relation;
using ScheduleOne.NPCs.Responses;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Variables;
using ScheduleOne.Vehicles;
using ScheduleOne.Vehicles.AI;
using ScheduleOne.VoiceOver;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace ScheduleOne.NPCs
{
	// Token: 0x0200048C RID: 1164
	[RequireComponent(typeof(NPCHealth))]
	public class NPC : NetworkBehaviour, IGUIDRegisterable, ISaveable, IDamageable
	{
		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06001702 RID: 5890 RVA: 0x0006549F File Offset: 0x0006369F
		public string fullName
		{
			get
			{
				if (this.hasLastName)
				{
					return this.FirstName + " " + this.LastName;
				}
				return this.FirstName;
			}
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06001703 RID: 5891 RVA: 0x000654C6 File Offset: 0x000636C6
		// (set) Token: 0x06001704 RID: 5892 RVA: 0x000654CE File Offset: 0x000636CE
		public float Scale { get; private set; } = 1f;

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06001705 RID: 5893 RVA: 0x000654D7 File Offset: 0x000636D7
		public bool IsConscious
		{
			get
			{
				return this.Health.Health > 0f && !this.behaviour.UnconsciousBehaviour.Active && !this.behaviour.DeadBehaviour.Active;
			}
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06001706 RID: 5894 RVA: 0x00065512 File Offset: 0x00063712
		public NPCMovement Movement
		{
			get
			{
				return this.movement;
			}
		}

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06001707 RID: 5895 RVA: 0x0006551A File Offset: 0x0006371A
		// (set) Token: 0x06001708 RID: 5896 RVA: 0x00065522 File Offset: 0x00063722
		public NPCInventory Inventory { get; protected set; }

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06001709 RID: 5897 RVA: 0x0006552B File Offset: 0x0006372B
		// (set) Token: 0x0600170A RID: 5898 RVA: 0x00065533 File Offset: 0x00063733
		public LandVehicle CurrentVehicle { get; protected set; }

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x0600170B RID: 5899 RVA: 0x0006553C File Offset: 0x0006373C
		public bool IsInVehicle
		{
			get
			{
				return this.CurrentVehicle != null;
			}
		}

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x0600170C RID: 5900 RVA: 0x0006554A File Offset: 0x0006374A
		public bool isInBuilding
		{
			get
			{
				return this.CurrentBuilding != null;
			}
		}

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x0600170D RID: 5901 RVA: 0x00065558 File Offset: 0x00063758
		// (set) Token: 0x0600170E RID: 5902 RVA: 0x00065560 File Offset: 0x00063760
		public NPCEnterableBuilding CurrentBuilding { get; protected set; }

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x0600170F RID: 5903 RVA: 0x00065569 File Offset: 0x00063769
		// (set) Token: 0x06001710 RID: 5904 RVA: 0x00065571 File Offset: 0x00063771
		public StaticDoor LastEnteredDoor { get; set; }

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x06001711 RID: 5905 RVA: 0x0006557A File Offset: 0x0006377A
		// (set) Token: 0x06001712 RID: 5906 RVA: 0x00065582 File Offset: 0x00063782
		public MSGConversation MSGConversation { get; protected set; }

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06001713 RID: 5907 RVA: 0x0006558B File Offset: 0x0006378B
		public string SaveFolderName
		{
			get
			{
				return this.fullName;
			}
		}

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x06001714 RID: 5908 RVA: 0x00065593 File Offset: 0x00063793
		public string SaveFileName
		{
			get
			{
				return "NPC";
			}
		}

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x06001715 RID: 5909 RVA: 0x00047DCE File Offset: 0x00045FCE
		public Loader Loader
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06001716 RID: 5910 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06001717 RID: 5911 RVA: 0x0006559A File Offset: 0x0006379A
		// (set) Token: 0x06001718 RID: 5912 RVA: 0x000655A2 File Offset: 0x000637A2
		public List<string> LocalExtraFiles { get; set; } = new List<string>
		{
			"Relationship",
			"MessageConversation"
		};

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x06001719 RID: 5913 RVA: 0x000655AB File Offset: 0x000637AB
		// (set) Token: 0x0600171A RID: 5914 RVA: 0x000655B3 File Offset: 0x000637B3
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x0600171B RID: 5915 RVA: 0x000655BC File Offset: 0x000637BC
		// (set) Token: 0x0600171C RID: 5916 RVA: 0x000655C4 File Offset: 0x000637C4
		public bool HasChanged { get; set; }

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x0600171D RID: 5917 RVA: 0x000655CD File Offset: 0x000637CD
		// (set) Token: 0x0600171E RID: 5918 RVA: 0x000655D5 File Offset: 0x000637D5
		public Guid GUID { get; protected set; }

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x0600171F RID: 5919 RVA: 0x000655DE File Offset: 0x000637DE
		// (set) Token: 0x06001720 RID: 5920 RVA: 0x000655E6 File Offset: 0x000637E6
		public bool isVisible { get; protected set; } = true;

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x06001721 RID: 5921 RVA: 0x000655EF File Offset: 0x000637EF
		// (set) Token: 0x06001722 RID: 5922 RVA: 0x000655F7 File Offset: 0x000637F7
		public bool isUnsettled { get; protected set; }

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06001723 RID: 5923 RVA: 0x00065600 File Offset: 0x00063800
		public bool IsPanicked
		{
			get
			{
				return this.timeSincePanicked < 20f;
			}
		}

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x06001724 RID: 5924 RVA: 0x0006560F File Offset: 0x0006380F
		// (set) Token: 0x06001725 RID: 5925 RVA: 0x00065617 File Offset: 0x00063817
		public float timeSincePanicked { get; protected set; } = 1000f;

		// Token: 0x06001726 RID: 5926 RVA: 0x00065620 File Offset: 0x00063820
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.NPC_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001727 RID: 5927 RVA: 0x0003DA73 File Offset: 0x0003BC73
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x06001728 RID: 5928 RVA: 0x0006563F File Offset: 0x0006383F
		public void SetGUID(Guid guid)
		{
			this.GUID = guid;
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x06001729 RID: 5929 RVA: 0x0006564E File Offset: 0x0006384E
		private void PlayerSpawned()
		{
			this.CreateMessageConversation();
		}

		// Token: 0x0600172A RID: 5930 RVA: 0x00065656 File Offset: 0x00063856
		public override void OnStartClient()
		{
			base.OnStartClient();
		}

		// Token: 0x0600172B RID: 5931 RVA: 0x00065660 File Offset: 0x00063860
		protected virtual void CreateMessageConversation()
		{
			if (this.MSGConversation != null)
			{
				Console.LogWarning("Message conversation already exists for " + this.fullName, null);
				return;
			}
			this.MSGConversation = new MSGConversation(this, this.fullName);
			this.MSGConversation.SetCategories(this.ConversationCategories);
			if (this.onConversationCreated != null)
			{
				this.onConversationCreated();
			}
		}

		// Token: 0x0600172C RID: 5932 RVA: 0x000656C2 File Offset: 0x000638C2
		public void SendTextMessage(string message)
		{
			this.MSGConversation.SendMessage(new Message(message, Message.ESenderType.Other, true, UnityEngine.Random.Range(int.MinValue, int.MaxValue)), true, true);
		}

		// Token: 0x0600172D RID: 5933 RVA: 0x000656E8 File Offset: 0x000638E8
		protected override void OnValidate()
		{
			base.OnValidate();
			if (base.gameObject.scene.name == null || base.gameObject.scene.name == base.gameObject.name)
			{
				return;
			}
			if (this.ID == string.Empty)
			{
				Console.LogWarning("NPC ID is empty (" + base.gameObject.name + ")", null);
			}
			this.GetHealth();
			if (this.VoiceOverEmitter == null)
			{
				this.VoiceOverEmitter = this.Avatar.HeadBone.GetComponentInChildren<VOEmitter>();
			}
		}

		// Token: 0x0600172E RID: 5934 RVA: 0x00065792 File Offset: 0x00063992
		private void GetHealth()
		{
			if (this.Health == null)
			{
				this.Health = base.GetComponent<NPCHealth>();
				if (this.Health == null)
				{
					this.Health = base.gameObject.AddComponent<NPCHealth>();
				}
			}
		}

		// Token: 0x0600172F RID: 5935 RVA: 0x000657D0 File Offset: 0x000639D0
		protected virtual void Start()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
			instance2.onMinutePass = (Action)Delegate.Combine(instance2.onMinutePass, new Action(this.MinPass));
			if (this.GUID == Guid.Empty)
			{
				if (!GUIDManager.IsGUIDValid(this.BakedGUID))
				{
					Console.LogWarning(base.gameObject.name + "'s baked GUID is not valid! Choosing random GUID", null);
					this.BakedGUID = GUIDManager.GenerateUniqueGUID().ToString();
				}
				this.GUID = new Guid(this.BakedGUID);
				GUIDManager.RegisterObject(this);
			}
			base.transform.SetParent(NetworkSingleton<NPCManager>.Instance.NPCContainer);
		}

		// Token: 0x06001730 RID: 5936 RVA: 0x000658AA File Offset: 0x00063AAA
		protected virtual void OnDestroy()
		{
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			}
		}

		// Token: 0x06001731 RID: 5937 RVA: 0x000658DC File Offset: 0x00063ADC
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (connection.IsLocalClient)
			{
				return;
			}
			if (this.RelationData.Unlocked)
			{
				this.ReceiveRelationshipData(connection, this.RelationData.RelationDelta, true);
			}
			if (this.IsInVehicle)
			{
				this.EnterVehicle(connection, this.CurrentVehicle);
			}
			if (this.isInBuilding)
			{
				this.EnterBuilding(connection, this.CurrentBuilding.GUID.ToString(), ArrayExt.IndexOf<StaticDoor>(this.CurrentBuilding.Doors, this.LastEnteredDoor));
			}
			this.SetTransform(connection, base.transform.position, base.transform.rotation);
			if (this.Avatar.CurrentEquippable != null)
			{
				this.SetEquippable_Networked(connection, this.Avatar.CurrentEquippable.AssetPath);
			}
		}

		// Token: 0x06001732 RID: 5938 RVA: 0x000659B3 File Offset: 0x00063BB3
		[ObserversRpc]
		private void SetTransform(NetworkConnection conn, Vector3 position, Quaternion rotation)
		{
			this.RpcWriter___Observers_SetTransform_4260003484(conn, position, rotation);
		}

		// Token: 0x06001733 RID: 5939 RVA: 0x000659C8 File Offset: 0x00063BC8
		protected virtual void MinPass()
		{
			for (int i = 0; i < Player.PlayerList.Count; i++)
			{
				this.awareness.VisionCone.SetGeneralCrimeResponseActive(Player.PlayerList[i], this.ShouldNoticeGeneralCrime(Player.PlayerList[i]));
			}
			if (InstanceFinder.IsServer)
			{
				float timeSincePanicked = this.timeSincePanicked;
				this.timeSincePanicked += 1f;
				if (this.timeSincePanicked > 20f && timeSincePanicked <= 20f)
				{
					this.RemovePanicked();
				}
			}
			if (this.CurrentVehicle != null)
			{
				VehicleLights component = this.CurrentVehicle.GetComponent<VehicleLights>();
				if (component != null)
				{
					if (NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(this.headlightStartTime, this.heaedLightsEndTime))
					{
						component.headLightsOn = true;
						return;
					}
					component.headLightsOn = false;
				}
			}
		}

		// Token: 0x06001734 RID: 5940 RVA: 0x00065A9B File Offset: 0x00063C9B
		protected virtual void Update()
		{
			this.awareness.VisionCone.DisableSightUpdates = this.Avatar.Anim.IsAvatarCulled;
		}

		// Token: 0x06001735 RID: 5941 RVA: 0x00065AC0 File Offset: 0x00063CC0
		public virtual void SetVisible(bool visible)
		{
			this.isVisible = visible;
			this.modelContainer.gameObject.SetActive(this.isVisible);
			if (InstanceFinder.IsServer)
			{
				this.movement.Agent.enabled = this.isVisible;
			}
			if (this.onVisibilityChanged != null)
			{
				this.onVisibilityChanged(visible);
			}
		}

		// Token: 0x06001736 RID: 5942 RVA: 0x00065B1B File Offset: 0x00063D1B
		public void SetScale(float scale)
		{
			this.Scale = scale;
			this.ApplyScale();
		}

		// Token: 0x06001737 RID: 5943 RVA: 0x00065B2C File Offset: 0x00063D2C
		public void SetScale(float scale, float lerpTime)
		{
			NPC.<>c__DisplayClass132_0 CS$<>8__locals1 = new NPC.<>c__DisplayClass132_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.scale = scale;
			CS$<>8__locals1.lerpTime = lerpTime;
			if (this.lerpScaleRoutine != null)
			{
				base.StopCoroutine(this.lerpScaleRoutine);
			}
			CS$<>8__locals1.startScale = this.Scale;
			this.lerpScaleRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SetScale>g__LerpScale|0());
		}

		// Token: 0x06001738 RID: 5944 RVA: 0x00065B8A File Offset: 0x00063D8A
		protected virtual void ApplyScale()
		{
			base.transform.localScale = new Vector3(this.Scale, this.Scale, this.Scale);
		}

		// Token: 0x06001739 RID: 5945 RVA: 0x00065BAE File Offset: 0x00063DAE
		[ServerRpc(RequireOwnership = false)]
		public virtual void AimedAtByPlayer(NetworkObject player)
		{
			this.RpcWriter___Server_AimedAtByPlayer_3323014238(player);
		}

		// Token: 0x0600173A RID: 5946 RVA: 0x00065BBA File Offset: 0x00063DBA
		public void OverrideAggression(float aggression)
		{
			this.Aggression = aggression;
		}

		// Token: 0x0600173B RID: 5947 RVA: 0x00065BC3 File Offset: 0x00063DC3
		public void ResetAggression()
		{
			this.Aggression = this.defaultAggression;
		}

		// Token: 0x0600173C RID: 5948 RVA: 0x00065BD1 File Offset: 0x00063DD1
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public virtual void SendImpact(Impact impact)
		{
			this.RpcWriter___Server_SendImpact_427288424(impact);
			this.RpcLogic___SendImpact_427288424(impact);
		}

		// Token: 0x0600173D RID: 5949 RVA: 0x00065BE8 File Offset: 0x00063DE8
		[ObserversRpc(RunLocally = true)]
		public virtual void ReceiveImpact(Impact impact)
		{
			this.RpcWriter___Observers_ReceiveImpact_427288424(impact);
			this.RpcLogic___ReceiveImpact_427288424(impact);
		}

		// Token: 0x0600173E RID: 5950 RVA: 0x00065C0C File Offset: 0x00063E0C
		public virtual void ProcessImpactForce(Vector3 forcePoint, Vector3 forceDirection, float force)
		{
			if (force >= 150f)
			{
				if (!this.Avatar.Ragdolled)
				{
					this.movement.ActivateRagdoll(forcePoint, forceDirection, force);
					return;
				}
			}
			else
			{
				if (force >= 100f)
				{
					this.Avatar.Anim.Flinch(forceDirection, AvatarAnimation.EFlinchType.Heavy);
					return;
				}
				if (force >= 50f)
				{
					this.Avatar.Anim.Flinch(forceDirection, AvatarAnimation.EFlinchType.Light);
				}
			}
		}

		// Token: 0x0600173F RID: 5951 RVA: 0x00065C74 File Offset: 0x00063E74
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public virtual void EnterVehicle(NetworkConnection connection, LandVehicle veh)
		{
			if (connection == null)
			{
				this.RpcWriter___Observers_EnterVehicle_3321926803(connection, veh);
				this.RpcLogic___EnterVehicle_3321926803(connection, veh);
			}
			else
			{
				this.RpcWriter___Target_EnterVehicle_3321926803(connection, veh);
			}
		}

		// Token: 0x06001740 RID: 5952 RVA: 0x00065CB8 File Offset: 0x00063EB8
		[ObserversRpc(RunLocally = true)]
		public virtual void ExitVehicle()
		{
			this.RpcWriter___Observers_ExitVehicle_2166136261();
			this.RpcLogic___ExitVehicle_2166136261();
		}

		// Token: 0x06001741 RID: 5953 RVA: 0x00065CD1 File Offset: 0x00063ED1
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendWorldspaceDialogueKey(string key, float duration)
		{
			this.RpcWriter___Server_SendWorldspaceDialogueKey_606697822(key, duration);
			this.RpcLogic___SendWorldspaceDialogueKey_606697822(key, duration);
		}

		// Token: 0x06001742 RID: 5954 RVA: 0x00065CEF File Offset: 0x00063EEF
		[ObserversRpc(RunLocally = true)]
		private void PlayWorldspaceDialogue(string key, float duration)
		{
			this.RpcWriter___Observers_PlayWorldspaceDialogue_606697822(key, duration);
			this.RpcLogic___PlayWorldspaceDialogue_606697822(key, duration);
		}

		// Token: 0x06001743 RID: 5955 RVA: 0x00065D0D File Offset: 0x00063F0D
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetConversant(NetworkObject player)
		{
			this.RpcWriter___Server_SetConversant_3323014238(player);
			this.RpcLogic___SetConversant_3323014238(player);
		}

		// Token: 0x06001744 RID: 5956 RVA: 0x00065D23 File Offset: 0x00063F23
		private void Hovered_Internal()
		{
			this.Hovered();
		}

		// Token: 0x06001745 RID: 5957 RVA: 0x00065D2B File Offset: 0x00063F2B
		private void Interacted_Internal()
		{
			this.Interacted();
		}

		// Token: 0x06001746 RID: 5958 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void Hovered()
		{
		}

		// Token: 0x06001747 RID: 5959 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void Interacted()
		{
		}

		// Token: 0x06001748 RID: 5960 RVA: 0x00065D34 File Offset: 0x00063F34
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void EnterBuilding(NetworkConnection connection, string buildingGUID, int doorIndex)
		{
			if (connection == null)
			{
				this.RpcWriter___Observers_EnterBuilding_3905681115(connection, buildingGUID, doorIndex);
				this.RpcLogic___EnterBuilding_3905681115(connection, buildingGUID, doorIndex);
			}
			else
			{
				this.RpcWriter___Target_EnterBuilding_3905681115(connection, buildingGUID, doorIndex);
			}
		}

		// Token: 0x06001749 RID: 5961 RVA: 0x00065D84 File Offset: 0x00063F84
		[ObserversRpc(RunLocally = true)]
		public void ExitBuilding(string buildingID = "")
		{
			this.RpcWriter___Observers_ExitBuilding_3615296227(buildingID);
			this.RpcLogic___ExitBuilding_3615296227(buildingID);
		}

		// Token: 0x0600174A RID: 5962 RVA: 0x00065DA5 File Offset: 0x00063FA5
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SetEquippable_Networked(NetworkConnection conn, string assetPath)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetEquippable_Networked_2971853958(conn, assetPath);
				this.RpcLogic___SetEquippable_Networked_2971853958(conn, assetPath);
			}
			else
			{
				this.RpcWriter___Target_SetEquippable_Networked_2971853958(conn, assetPath);
			}
		}

		// Token: 0x0600174B RID: 5963 RVA: 0x00065DDB File Offset: 0x00063FDB
		public AvatarEquippable SetEquippable_Networked_Return(NetworkConnection conn, string assetPath)
		{
			this.SetEquippable_Networked_ExcludeServer(conn, assetPath);
			return this.Avatar.SetEquippable(assetPath);
		}

		// Token: 0x0600174C RID: 5964 RVA: 0x00065DF1 File Offset: 0x00063FF1
		public AvatarEquippable SetEquippable_Return(string assetPath)
		{
			return this.Avatar.SetEquippable(assetPath);
		}

		// Token: 0x0600174D RID: 5965 RVA: 0x00065DFF File Offset: 0x00063FFF
		[ObserversRpc(RunLocally = false, ExcludeServer = true)]
		private void SetEquippable_Networked_ExcludeServer(NetworkConnection conn, string assetPath)
		{
			this.RpcWriter___Observers_SetEquippable_Networked_ExcludeServer_2971853958(conn, assetPath);
		}

		// Token: 0x0600174E RID: 5966 RVA: 0x00065E0F File Offset: 0x0006400F
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SendEquippableMessage_Networked(NetworkConnection conn, string message)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SendEquippableMessage_Networked_2971853958(conn, message);
				this.RpcLogic___SendEquippableMessage_Networked_2971853958(conn, message);
			}
			else
			{
				this.RpcWriter___Target_SendEquippableMessage_Networked_2971853958(conn, message);
			}
		}

		// Token: 0x0600174F RID: 5967 RVA: 0x00065E48 File Offset: 0x00064048
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SendEquippableMessage_Networked_Vector(NetworkConnection conn, string message, Vector3 data)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SendEquippableMessage_Networked_Vector_4022222929(conn, message, data);
				this.RpcLogic___SendEquippableMessage_Networked_Vector_4022222929(conn, message, data);
			}
			else
			{
				this.RpcWriter___Target_SendEquippableMessage_Networked_Vector_4022222929(conn, message, data);
			}
		}

		// Token: 0x06001750 RID: 5968 RVA: 0x00065E95 File Offset: 0x00064095
		[ServerRpc(RequireOwnership = false)]
		public void SendAnimationTrigger(string trigger)
		{
			this.RpcWriter___Server_SendAnimationTrigger_3615296227(trigger);
		}

		// Token: 0x06001751 RID: 5969 RVA: 0x00065EA1 File Offset: 0x000640A1
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SetAnimationTrigger_Networked(NetworkConnection conn, string trigger)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetAnimationTrigger_Networked_2971853958(conn, trigger);
				this.RpcLogic___SetAnimationTrigger_Networked_2971853958(conn, trigger);
			}
			else
			{
				this.RpcWriter___Target_SetAnimationTrigger_Networked_2971853958(conn, trigger);
			}
		}

		// Token: 0x06001752 RID: 5970 RVA: 0x00065ED7 File Offset: 0x000640D7
		public void SetAnimationTrigger(string trigger)
		{
			this.Avatar.Anim.SetTrigger(trigger);
		}

		// Token: 0x06001753 RID: 5971 RVA: 0x00065EEA File Offset: 0x000640EA
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void ResetAnimationTrigger_Networked(NetworkConnection conn, string trigger)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_ResetAnimationTrigger_Networked_2971853958(conn, trigger);
				this.RpcLogic___ResetAnimationTrigger_Networked_2971853958(conn, trigger);
			}
			else
			{
				this.RpcWriter___Target_ResetAnimationTrigger_Networked_2971853958(conn, trigger);
			}
		}

		// Token: 0x06001754 RID: 5972 RVA: 0x00065F20 File Offset: 0x00064120
		public void ResetAnimationTrigger(string trigger)
		{
			this.Avatar.Anim.ResetTrigger(trigger);
		}

		// Token: 0x06001755 RID: 5973 RVA: 0x00065F33 File Offset: 0x00064133
		[ObserversRpc(RunLocally = true)]
		public void SetCrouched_Networked(bool crouched)
		{
			this.RpcWriter___Observers_SetCrouched_Networked_1140765316(crouched);
			this.RpcLogic___SetCrouched_Networked_1140765316(crouched);
		}

		// Token: 0x06001756 RID: 5974 RVA: 0x00065F4C File Offset: 0x0006414C
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SetAnimationBool_Networked(NetworkConnection conn, string id, bool value)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetAnimationBool_Networked_619441887(conn, id, value);
				this.RpcLogic___SetAnimationBool_Networked_619441887(conn, id, value);
			}
			else
			{
				this.RpcWriter___Target_SetAnimationBool_Networked_619441887(conn, id, value);
			}
		}

		// Token: 0x06001757 RID: 5975 RVA: 0x00065F99 File Offset: 0x00064199
		public void SetAnimationBool(string trigger, bool val)
		{
			this.Avatar.Anim.SetBool(trigger, val);
		}

		// Token: 0x06001758 RID: 5976 RVA: 0x00065FB0 File Offset: 0x000641B0
		protected virtual bool ShouldNoticeGeneralCrime(Player player)
		{
			return player.CrimeData.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.None && player.Health.IsAlive && !player.IsArrested && !player.IsUnconscious && !this.behaviour.CoweringBehaviour.Active && !this.behaviour.FleeBehaviour.Active && !this.isUnsettled;
		}

		// Token: 0x06001759 RID: 5977 RVA: 0x0006601E File Offset: 0x0006421E
		protected virtual void SetUnsettled_30s(Player player)
		{
			this.SetUnsettled(30f);
		}

		// Token: 0x0600175A RID: 5978 RVA: 0x0006602C File Offset: 0x0006422C
		protected void SetUnsettled(float duration)
		{
			NPC.<>c__DisplayClass167_0 CS$<>8__locals1 = new NPC.<>c__DisplayClass167_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.duration = duration;
			bool isUnsettled = this.isUnsettled;
			this.isUnsettled = true;
			if (!isUnsettled)
			{
				this.Avatar.EmotionManager.AddEmotionOverride("Concerned", "unsettled", 0f, 5);
			}
			if (this.resetUnsettledCoroutine != null)
			{
				base.StopCoroutine(this.resetUnsettledCoroutine);
			}
			this.resetUnsettledCoroutine = base.StartCoroutine(CS$<>8__locals1.<SetUnsettled>g__ResetUnsettled|0());
		}

		// Token: 0x0600175B RID: 5979 RVA: 0x000660A2 File Offset: 0x000642A2
		[ServerRpc(RequireOwnership = false)]
		public void SetPanicked()
		{
			this.RpcWriter___Server_SetPanicked_2166136261();
		}

		// Token: 0x0600175C RID: 5980 RVA: 0x000660AC File Offset: 0x000642AC
		[ObserversRpc]
		private void ReceivePanicked()
		{
			this.RpcWriter___Observers_ReceivePanicked_2166136261();
		}

		// Token: 0x0600175D RID: 5981 RVA: 0x000660C0 File Offset: 0x000642C0
		[ObserversRpc]
		private void RemovePanicked()
		{
			this.RpcWriter___Observers_RemovePanicked_2166136261();
		}

		// Token: 0x0600175E RID: 5982 RVA: 0x000660D3 File Offset: 0x000642D3
		public virtual string GetNameAddress()
		{
			return this.FirstName;
		}

		// Token: 0x0600175F RID: 5983 RVA: 0x000660DB File Offset: 0x000642DB
		public void PlayVO(EVOLineType lineType)
		{
			this.VoiceOverEmitter.Play(lineType);
		}

		// Token: 0x06001760 RID: 5984 RVA: 0x000660E9 File Offset: 0x000642E9
		[TargetRpc]
		public void ReceiveRelationshipData(NetworkConnection conn, float relationship, bool unlocked)
		{
			this.RpcWriter___Target_ReceiveRelationshipData_4052192084(conn, relationship, unlocked);
		}

		// Token: 0x06001761 RID: 5985 RVA: 0x000660FD File Offset: 0x000642FD
		[ServerRpc(RequireOwnership = false)]
		public void SetIsBeingPickPocketed(bool pickpocketed)
		{
			this.RpcWriter___Server_SetIsBeingPickPocketed_1140765316(pickpocketed);
		}

		// Token: 0x06001762 RID: 5986 RVA: 0x00066109 File Offset: 0x00064309
		[ServerRpc(RequireOwnership = false)]
		public void SendRelationship(float relationship)
		{
			this.RpcWriter___Server_SendRelationship_431000436(relationship);
		}

		// Token: 0x06001763 RID: 5987 RVA: 0x00066115 File Offset: 0x00064315
		[ObserversRpc]
		private void SetRelationship(float relationship)
		{
			this.RpcWriter___Observers_SetRelationship_431000436(relationship);
		}

		// Token: 0x06001764 RID: 5988 RVA: 0x00066124 File Offset: 0x00064324
		public void ShowOutline(Color color)
		{
			if (this.OutlineEffect == null)
			{
				this.OutlineEffect = base.gameObject.AddComponent<Outlinable>();
				this.OutlineEffect.OutlineParameters.BlurShift = 0f;
				this.OutlineEffect.OutlineParameters.DilateShift = 0.5f;
				this.OutlineEffect.OutlineParameters.FillPass.Shader = Resources.Load<Shader>("Easy performant outline/Shaders/Fills/ColorFill");
				foreach (GameObject gameObject in this.OutlineRenderers)
				{
					SkinnedMeshRenderer[] array = new SkinnedMeshRenderer[0];
					array = new SkinnedMeshRenderer[]
					{
						gameObject.GetComponent<SkinnedMeshRenderer>()
					};
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

		// Token: 0x06001765 RID: 5989 RVA: 0x0006626C File Offset: 0x0006446C
		public void ShowOutline(BuildableItem.EOutlineColor color)
		{
			this.ShowOutline(BuildableItem.GetColorFromOutlineColorEnum(color));
		}

		// Token: 0x06001766 RID: 5990 RVA: 0x0006627F File Offset: 0x0006447F
		public void HideOutline()
		{
			if (this.OutlineEffect != null)
			{
				this.OutlineEffect.enabled = false;
			}
		}

		// Token: 0x06001767 RID: 5991 RVA: 0x0006629B File Offset: 0x0006449B
		public virtual bool ShouldSave()
		{
			return this.ShouldSaveRelationshipData() || this.ShouldSaveMessages() || this.ShouldSaveInventory() || this.ShouldSaveHealth() || this.HasChanged;
		}

		// Token: 0x06001768 RID: 5992 RVA: 0x000662CB File Offset: 0x000644CB
		protected virtual bool ShouldSaveRelationshipData()
		{
			return this.RelationData.Unlocked || 2f != this.RelationData.RelationDelta;
		}

		// Token: 0x06001769 RID: 5993 RVA: 0x000662F1 File Offset: 0x000644F1
		protected bool ShouldSaveMessages()
		{
			return this.MSGConversation != null && this.MSGConversation.messageHistory.Count > 0;
		}

		// Token: 0x0600176A RID: 5994 RVA: 0x00066313 File Offset: 0x00064513
		protected virtual bool ShouldSaveInventory()
		{
			return ((IItemSlotOwner)this.Inventory).GetTotalItemCount() > 0;
		}

		// Token: 0x0600176B RID: 5995 RVA: 0x00066323 File Offset: 0x00064523
		protected virtual bool ShouldSaveHealth()
		{
			return this.Health.Health < this.Health.MaxHealth || this.Health.IsDead || this.Health.DaysPassedSinceDeath > 0;
		}

		// Token: 0x0600176C RID: 5996 RVA: 0x0006635A File Offset: 0x0006455A
		public string GetSaveString()
		{
			return this.GetSaveData().GetJson(true);
		}

		// Token: 0x0600176D RID: 5997 RVA: 0x00066368 File Offset: 0x00064568
		public virtual NPCData GetNPCData()
		{
			return new NPCData(this.ID);
		}

		// Token: 0x0600176E RID: 5998 RVA: 0x00066378 File Offset: 0x00064578
		public virtual DynamicSaveData GetSaveData()
		{
			DynamicSaveData dynamicSaveData = new DynamicSaveData(this.GetNPCData());
			if (this.ShouldSaveRelationshipData())
			{
				dynamicSaveData.AddData("Relationship", this.RelationData.GetSaveData().GetJson(true));
			}
			if (this.ShouldSaveMessages())
			{
				dynamicSaveData.AddData("MessageConversation", this.MSGConversation.GetSaveData().GetJson(true));
			}
			if (this.ShouldSaveInventory())
			{
				dynamicSaveData.AddData("Inventory", new ItemSet(this.Inventory.ItemSlots).GetJSON());
			}
			if (this.ShouldSaveHealth())
			{
				dynamicSaveData.AddData("Health", new NPCHealthData(this.Health.Health, this.Health.IsDead, this.Health.DaysPassedSinceDeath).GetJson(true));
			}
			Customer component = base.GetComponent<Customer>();
			if (component != null)
			{
				dynamicSaveData.AddData("CustomerData", component.GetSaveString());
			}
			return dynamicSaveData;
		}

		// Token: 0x0600176F RID: 5999 RVA: 0x000594B4 File Offset: 0x000576B4
		public virtual List<string> WriteData(string parentFolderPath)
		{
			return new List<string>();
		}

		// Token: 0x06001770 RID: 6000 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void Load(NPCData data, string containerPath)
		{
		}

		// Token: 0x06001771 RID: 6001 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void Load(DynamicSaveData dynamicData, NPCData npcData)
		{
		}

		// Token: 0x06001773 RID: 6003 RVA: 0x00066554 File Offset: 0x00064754
		[CompilerGenerated]
		private void <Awake>g__Unlocked|114_0(NPCRelationData.EUnlockType unlockType, bool notify)
		{
			if (this.NPCUnlockedVariable != string.Empty)
			{
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue(this.NPCUnlockedVariable, true.ToString(), true);
			}
		}

		// Token: 0x06001774 RID: 6004 RVA: 0x00066590 File Offset: 0x00064790
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.NPCAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.NPCAssembly-CSharp.dll_Excuted = true;
			this.syncVar___PlayerConversant = new SyncVar<NetworkObject>(this, 0U, 0, 0, -1f, 0, this.PlayerConversant);
			base.RegisterObserversRpc(0U, new ClientRpcDelegate(this.RpcReader___Observers_SetTransform_4260003484));
			base.RegisterServerRpc(1U, new ServerRpcDelegate(this.RpcReader___Server_AimedAtByPlayer_3323014238));
			base.RegisterServerRpc(2U, new ServerRpcDelegate(this.RpcReader___Server_SendImpact_427288424));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveImpact_427288424));
			base.RegisterObserversRpc(4U, new ClientRpcDelegate(this.RpcReader___Observers_EnterVehicle_3321926803));
			base.RegisterTargetRpc(5U, new ClientRpcDelegate(this.RpcReader___Target_EnterVehicle_3321926803));
			base.RegisterObserversRpc(6U, new ClientRpcDelegate(this.RpcReader___Observers_ExitVehicle_2166136261));
			base.RegisterServerRpc(7U, new ServerRpcDelegate(this.RpcReader___Server_SendWorldspaceDialogueKey_606697822));
			base.RegisterObserversRpc(8U, new ClientRpcDelegate(this.RpcReader___Observers_PlayWorldspaceDialogue_606697822));
			base.RegisterServerRpc(9U, new ServerRpcDelegate(this.RpcReader___Server_SetConversant_3323014238));
			base.RegisterObserversRpc(10U, new ClientRpcDelegate(this.RpcReader___Observers_EnterBuilding_3905681115));
			base.RegisterTargetRpc(11U, new ClientRpcDelegate(this.RpcReader___Target_EnterBuilding_3905681115));
			base.RegisterObserversRpc(12U, new ClientRpcDelegate(this.RpcReader___Observers_ExitBuilding_3615296227));
			base.RegisterObserversRpc(13U, new ClientRpcDelegate(this.RpcReader___Observers_SetEquippable_Networked_2971853958));
			base.RegisterTargetRpc(14U, new ClientRpcDelegate(this.RpcReader___Target_SetEquippable_Networked_2971853958));
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_SetEquippable_Networked_ExcludeServer_2971853958));
			base.RegisterObserversRpc(16U, new ClientRpcDelegate(this.RpcReader___Observers_SendEquippableMessage_Networked_2971853958));
			base.RegisterTargetRpc(17U, new ClientRpcDelegate(this.RpcReader___Target_SendEquippableMessage_Networked_2971853958));
			base.RegisterObserversRpc(18U, new ClientRpcDelegate(this.RpcReader___Observers_SendEquippableMessage_Networked_Vector_4022222929));
			base.RegisterTargetRpc(19U, new ClientRpcDelegate(this.RpcReader___Target_SendEquippableMessage_Networked_Vector_4022222929));
			base.RegisterServerRpc(20U, new ServerRpcDelegate(this.RpcReader___Server_SendAnimationTrigger_3615296227));
			base.RegisterObserversRpc(21U, new ClientRpcDelegate(this.RpcReader___Observers_SetAnimationTrigger_Networked_2971853958));
			base.RegisterTargetRpc(22U, new ClientRpcDelegate(this.RpcReader___Target_SetAnimationTrigger_Networked_2971853958));
			base.RegisterObserversRpc(23U, new ClientRpcDelegate(this.RpcReader___Observers_ResetAnimationTrigger_Networked_2971853958));
			base.RegisterTargetRpc(24U, new ClientRpcDelegate(this.RpcReader___Target_ResetAnimationTrigger_Networked_2971853958));
			base.RegisterObserversRpc(25U, new ClientRpcDelegate(this.RpcReader___Observers_SetCrouched_Networked_1140765316));
			base.RegisterObserversRpc(26U, new ClientRpcDelegate(this.RpcReader___Observers_SetAnimationBool_Networked_619441887));
			base.RegisterTargetRpc(27U, new ClientRpcDelegate(this.RpcReader___Target_SetAnimationBool_Networked_619441887));
			base.RegisterServerRpc(28U, new ServerRpcDelegate(this.RpcReader___Server_SetPanicked_2166136261));
			base.RegisterObserversRpc(29U, new ClientRpcDelegate(this.RpcReader___Observers_ReceivePanicked_2166136261));
			base.RegisterObserversRpc(30U, new ClientRpcDelegate(this.RpcReader___Observers_RemovePanicked_2166136261));
			base.RegisterTargetRpc(31U, new ClientRpcDelegate(this.RpcReader___Target_ReceiveRelationshipData_4052192084));
			base.RegisterServerRpc(32U, new ServerRpcDelegate(this.RpcReader___Server_SetIsBeingPickPocketed_1140765316));
			base.RegisterServerRpc(33U, new ServerRpcDelegate(this.RpcReader___Server_SendRelationship_431000436));
			base.RegisterObserversRpc(34U, new ClientRpcDelegate(this.RpcReader___Observers_SetRelationship_431000436));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.NPCs.NPC));
		}

		// Token: 0x06001775 RID: 6005 RVA: 0x00066910 File Offset: 0x00064B10
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.NPCAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.NPCAssembly-CSharp.dll_Excuted = true;
			this.syncVar___PlayerConversant.SetRegistered();
		}

		// Token: 0x06001776 RID: 6006 RVA: 0x0006692E File Offset: 0x00064B2E
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001777 RID: 6007 RVA: 0x0006693C File Offset: 0x00064B3C
		private void RpcWriter___Observers_SetTransform_4260003484(NetworkConnection conn, Vector3 position, Quaternion rotation)
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
			writer.WriteNetworkConnection(conn);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation, 1);
			base.SendObserversRpc(0U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06001778 RID: 6008 RVA: 0x00066A11 File Offset: 0x00064C11
		private void RpcLogic___SetTransform_4260003484(NetworkConnection conn, Vector3 position, Quaternion rotation)
		{
			base.transform.position = position;
			base.transform.rotation = rotation;
		}

		// Token: 0x06001779 RID: 6009 RVA: 0x00066A2C File Offset: 0x00064C2C
		private void RpcReader___Observers_SetTransform_4260003484(PooledReader PooledReader0, Channel channel)
		{
			NetworkConnection conn = PooledReader0.ReadNetworkConnection();
			Vector3 position = PooledReader0.ReadVector3();
			Quaternion rotation = PooledReader0.ReadQuaternion(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetTransform_4260003484(conn, position, rotation);
		}

		// Token: 0x0600177A RID: 6010 RVA: 0x00066A84 File Offset: 0x00064C84
		private void RpcWriter___Server_AimedAtByPlayer_3323014238(NetworkObject player)
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
			writer.WriteNetworkObject(player);
			base.SendServerRpc(1U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x0600177B RID: 6011 RVA: 0x00066B2B File Offset: 0x00064D2B
		public virtual void RpcLogic___AimedAtByPlayer_3323014238(NetworkObject player)
		{
			this.responses.RespondToAimedAt(player.GetComponent<Player>());
		}

		// Token: 0x0600177C RID: 6012 RVA: 0x00066B40 File Offset: 0x00064D40
		private void RpcReader___Server_AimedAtByPlayer_3323014238(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject player = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___AimedAtByPlayer_3323014238(player);
		}

		// Token: 0x0600177D RID: 6013 RVA: 0x00066B74 File Offset: 0x00064D74
		private void RpcWriter___Server_SendImpact_427288424(Impact impact)
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
			writer.Write___ScheduleOne.Combat.ImpactFishNet.Serializing.Generated(impact);
			base.SendServerRpc(2U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x0600177E RID: 6014 RVA: 0x00066C1B File Offset: 0x00064E1B
		public virtual void RpcLogic___SendImpact_427288424(Impact impact)
		{
			this.ReceiveImpact(impact);
		}

		// Token: 0x0600177F RID: 6015 RVA: 0x00066C24 File Offset: 0x00064E24
		private void RpcReader___Server_SendImpact_427288424(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			Impact impact = GeneratedReaders___Internal.Read___ScheduleOne.Combat.ImpactFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendImpact_427288424(impact);
		}

		// Token: 0x06001780 RID: 6016 RVA: 0x00066C64 File Offset: 0x00064E64
		private void RpcWriter___Observers_ReceiveImpact_427288424(Impact impact)
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
			writer.Write___ScheduleOne.Combat.ImpactFishNet.Serializing.Generated(impact);
			base.SendObserversRpc(3U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06001781 RID: 6017 RVA: 0x00066D1C File Offset: 0x00064F1C
		public virtual void RpcLogic___ReceiveImpact_427288424(Impact impact)
		{
			if (this.impactHistory.Contains(impact.ImpactID))
			{
				return;
			}
			this.impactHistory.Add(impact.ImpactID);
			float num = 1f;
			NPCMovement.EStance stance = this.movement.Stance;
			if (stance != NPCMovement.EStance.None)
			{
				if (stance == NPCMovement.EStance.Stanced)
				{
					num = 0.5f;
				}
			}
			else
			{
				num = 1f;
			}
			this.Health.TakeDamage(impact.ImpactDamage, Impact.IsLethal(impact.ImpactType));
			this.ProcessImpactForce(impact.HitPoint, impact.ImpactForceDirection, impact.ImpactForce * num);
			this.responses.ImpactReceived(impact);
		}

		// Token: 0x06001782 RID: 6018 RVA: 0x00066DBC File Offset: 0x00064FBC
		private void RpcReader___Observers_ReceiveImpact_427288424(PooledReader PooledReader0, Channel channel)
		{
			Impact impact = GeneratedReaders___Internal.Read___ScheduleOne.Combat.ImpactFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ReceiveImpact_427288424(impact);
		}

		// Token: 0x06001783 RID: 6019 RVA: 0x00066DF8 File Offset: 0x00064FF8
		private void RpcWriter___Observers_EnterVehicle_3321926803(NetworkConnection connection, LandVehicle veh)
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
			writer.Write___ScheduleOne.Vehicles.LandVehicleFishNet.Serializing.Generated(veh);
			base.SendObserversRpc(4U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06001784 RID: 6020 RVA: 0x00066EB0 File Offset: 0x000650B0
		public virtual void RpcLogic___EnterVehicle_3321926803(NetworkConnection connection, LandVehicle veh)
		{
			if (veh == this.CurrentVehicle)
			{
				return;
			}
			this.CurrentVehicle = veh;
			this.SetVisible(false);
			this.movement.Agent.enabled = false;
			base.transform.SetParent(veh.transform);
			veh.AddNPCOccupant(this);
			base.transform.position = this.CurrentVehicle.Seats[this.CurrentVehicle.OccupantNPCs.ToList<NPC>().IndexOf(this)].transform.position;
			base.transform.localRotation = Quaternion.identity;
			if (this.onEnterVehicle != null)
			{
				this.onEnterVehicle(veh);
			}
		}

		// Token: 0x06001785 RID: 6021 RVA: 0x00066F60 File Offset: 0x00065160
		private void RpcReader___Observers_EnterVehicle_3321926803(PooledReader PooledReader0, Channel channel)
		{
			LandVehicle veh = GeneratedReaders___Internal.Read___ScheduleOne.Vehicles.LandVehicleFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___EnterVehicle_3321926803(null, veh);
		}

		// Token: 0x06001786 RID: 6022 RVA: 0x00066F9C File Offset: 0x0006519C
		private void RpcWriter___Target_EnterVehicle_3321926803(NetworkConnection connection, LandVehicle veh)
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
			writer.Write___ScheduleOne.Vehicles.LandVehicleFishNet.Serializing.Generated(veh);
			base.SendTargetRpc(5U, writer, channel, 0, connection, false, true);
			writer.Store();
		}

		// Token: 0x06001787 RID: 6023 RVA: 0x00067054 File Offset: 0x00065254
		private void RpcReader___Target_EnterVehicle_3321926803(PooledReader PooledReader0, Channel channel)
		{
			LandVehicle veh = GeneratedReaders___Internal.Read___ScheduleOne.Vehicles.LandVehicleFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___EnterVehicle_3321926803(base.LocalConnection, veh);
		}

		// Token: 0x06001788 RID: 6024 RVA: 0x0006708C File Offset: 0x0006528C
		private void RpcWriter___Observers_ExitVehicle_2166136261()
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
			base.SendObserversRpc(6U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06001789 RID: 6025 RVA: 0x00067138 File Offset: 0x00065338
		public virtual void RpcLogic___ExitVehicle_2166136261()
		{
			if (this.CurrentVehicle == null)
			{
				return;
			}
			int seatIndex = this.CurrentVehicle.OccupantNPCs.ToList<NPC>().IndexOf(this);
			this.CurrentVehicle.RemoveNPCOccupant(this);
			this.CurrentVehicle.Agent.Flags.ResetFlags();
			if (this.CurrentVehicle.GetComponent<VehicleLights>() != null)
			{
				this.CurrentVehicle.GetComponent<VehicleLights>().headLightsOn = false;
			}
			Transform exitPoint = this.CurrentVehicle.GetExitPoint(seatIndex);
			base.transform.SetParent(NetworkSingleton<NPCManager>.Instance.NPCContainer);
			base.transform.position = exitPoint.position - exitPoint.up * 1f;
			this.movement.FaceDirection(exitPoint.forward, 0f);
			if (InstanceFinder.IsServer)
			{
				this.movement.Agent.enabled = true;
			}
			this.SetVisible(true);
			if (this.onExitVehicle != null)
			{
				this.onExitVehicle(this.CurrentVehicle);
			}
			this.CurrentVehicle = null;
		}

		// Token: 0x0600178A RID: 6026 RVA: 0x00067250 File Offset: 0x00065450
		private void RpcReader___Observers_ExitVehicle_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ExitVehicle_2166136261();
		}

		// Token: 0x0600178B RID: 6027 RVA: 0x0006727C File Offset: 0x0006547C
		private void RpcWriter___Server_SendWorldspaceDialogueKey_606697822(string key, float duration)
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
			writer.WriteString(key);
			writer.WriteSingle(duration, 0);
			base.SendServerRpc(7U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x0600178C RID: 6028 RVA: 0x00067335 File Offset: 0x00065535
		public void RpcLogic___SendWorldspaceDialogueKey_606697822(string key, float duration)
		{
			this.PlayWorldspaceDialogue(key, duration);
		}

		// Token: 0x0600178D RID: 6029 RVA: 0x00067340 File Offset: 0x00065540
		private void RpcReader___Server_SendWorldspaceDialogueKey_606697822(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string key = PooledReader0.ReadString();
			float duration = PooledReader0.ReadSingle(0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendWorldspaceDialogueKey_606697822(key, duration);
		}

		// Token: 0x0600178E RID: 6030 RVA: 0x00067394 File Offset: 0x00065594
		private void RpcWriter___Observers_PlayWorldspaceDialogue_606697822(string key, float duration)
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
			writer.WriteString(key);
			writer.WriteSingle(duration, 0);
			base.SendObserversRpc(8U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600178F RID: 6031 RVA: 0x0006745C File Offset: 0x0006565C
		private void RpcLogic___PlayWorldspaceDialogue_606697822(string key, float duration)
		{
			this.dialogueHandler.PlayReaction(key, duration, false);
		}

		// Token: 0x06001790 RID: 6032 RVA: 0x0006746C File Offset: 0x0006566C
		private void RpcReader___Observers_PlayWorldspaceDialogue_606697822(PooledReader PooledReader0, Channel channel)
		{
			string key = PooledReader0.ReadString();
			float duration = PooledReader0.ReadSingle(0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___PlayWorldspaceDialogue_606697822(key, duration);
		}

		// Token: 0x06001791 RID: 6033 RVA: 0x000674C0 File Offset: 0x000656C0
		private void RpcWriter___Server_SetConversant_3323014238(NetworkObject player)
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
			writer.WriteNetworkObject(player);
			base.SendServerRpc(9U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06001792 RID: 6034 RVA: 0x00067567 File Offset: 0x00065767
		public void RpcLogic___SetConversant_3323014238(NetworkObject player)
		{
			this.sync___set_value_PlayerConversant(player, true);
		}

		// Token: 0x06001793 RID: 6035 RVA: 0x00067574 File Offset: 0x00065774
		private void RpcReader___Server_SetConversant_3323014238(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject player = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetConversant_3323014238(player);
		}

		// Token: 0x06001794 RID: 6036 RVA: 0x000675B4 File Offset: 0x000657B4
		private void RpcWriter___Observers_EnterBuilding_3905681115(NetworkConnection connection, string buildingGUID, int doorIndex)
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
			writer.WriteString(buildingGUID);
			writer.WriteInt32(doorIndex, 1);
			base.SendObserversRpc(10U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06001795 RID: 6037 RVA: 0x0006767C File Offset: 0x0006587C
		public void RpcLogic___EnterBuilding_3905681115(NetworkConnection connection, string buildingGUID, int doorIndex)
		{
			NPCEnterableBuilding @object = GUIDManager.GetObject<NPCEnterableBuilding>(new Guid(buildingGUID));
			if (@object == null)
			{
				Console.LogWarning(this.fullName + ".EnterBuilding: building not found with given GUID", null);
				return;
			}
			this.awareness.VisionCone.ClearEvents();
			if (@object == this.CurrentBuilding)
			{
				if (InstanceFinder.IsServer)
				{
					this.Movement.Warp(@object.Doors[doorIndex].AccessPoint);
					this.Movement.Stop();
				}
				this.SetVisible(false);
				return;
			}
			if (this.CurrentBuilding != null)
			{
				Console.LogWarning("NPC.EnterBuilding called but NPC is already in a building. New building will still be entered.", null);
				this.ExitBuilding("");
			}
			this.CurrentBuilding = @object;
			this.LastEnteredDoor = @object.Doors[doorIndex];
			this.awareness.SetAwarenessActive(false);
			@object.NPCEnteredBuilding(this);
			this.SetVisible(false);
			this.Movement.Stop();
			this.Movement.Warp(@object.Doors[doorIndex].AccessPoint);
		}

		// Token: 0x06001796 RID: 6038 RVA: 0x0006777C File Offset: 0x0006597C
		private void RpcReader___Observers_EnterBuilding_3905681115(PooledReader PooledReader0, Channel channel)
		{
			string buildingGUID = PooledReader0.ReadString();
			int doorIndex = PooledReader0.ReadInt32(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___EnterBuilding_3905681115(null, buildingGUID, doorIndex);
		}

		// Token: 0x06001797 RID: 6039 RVA: 0x000677D0 File Offset: 0x000659D0
		private void RpcWriter___Target_EnterBuilding_3905681115(NetworkConnection connection, string buildingGUID, int doorIndex)
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
			writer.WriteString(buildingGUID);
			writer.WriteInt32(doorIndex, 1);
			base.SendTargetRpc(11U, writer, channel, 0, connection, false, true);
			writer.Store();
		}

		// Token: 0x06001798 RID: 6040 RVA: 0x00067898 File Offset: 0x00065A98
		private void RpcReader___Target_EnterBuilding_3905681115(PooledReader PooledReader0, Channel channel)
		{
			string buildingGUID = PooledReader0.ReadString();
			int doorIndex = PooledReader0.ReadInt32(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___EnterBuilding_3905681115(base.LocalConnection, buildingGUID, doorIndex);
		}

		// Token: 0x06001799 RID: 6041 RVA: 0x000678E8 File Offset: 0x00065AE8
		private void RpcWriter___Observers_ExitBuilding_3615296227(string buildingID = "")
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
			writer.WriteString(buildingID);
			base.SendObserversRpc(12U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600179A RID: 6042 RVA: 0x000679A0 File Offset: 0x00065BA0
		public void RpcLogic___ExitBuilding_3615296227(string buildingID = "")
		{
			if (buildingID == "" && this.CurrentBuilding != null)
			{
				buildingID = this.CurrentBuilding.GUID.ToString();
			}
			if (buildingID == "")
			{
				return;
			}
			NPCEnterableBuilding @object = GUIDManager.GetObject<NPCEnterableBuilding>(new Guid(buildingID));
			if (@object == null)
			{
				return;
			}
			if (this.LastEnteredDoor == null)
			{
				this.LastEnteredDoor = @object.Doors[0];
			}
			this.Avatar.transform.localPosition = Vector3.zero;
			this.Avatar.transform.localRotation = Quaternion.identity;
			NavMeshHit navMeshHit;
			Vector3 position = NavMeshUtility.SamplePosition(this.LastEnteredDoor.AccessPoint.transform.position, out navMeshHit, 2f, -1, true) ? navMeshHit.position : this.LastEnteredDoor.AccessPoint.transform.position;
			this.Movement.Warp(position);
			this.Movement.FaceDirection(-this.LastEnteredDoor.AccessPoint.transform.forward, 0f);
			this.awareness.SetAwarenessActive(true);
			@object.NPCExitedBuilding(this);
			this.CurrentBuilding = null;
			this.SetVisible(true);
		}

		// Token: 0x0600179B RID: 6043 RVA: 0x00067AE8 File Offset: 0x00065CE8
		private void RpcReader___Observers_ExitBuilding_3615296227(PooledReader PooledReader0, Channel channel)
		{
			string buildingID = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ExitBuilding_3615296227(buildingID);
		}

		// Token: 0x0600179C RID: 6044 RVA: 0x00067B24 File Offset: 0x00065D24
		private void RpcWriter___Observers_SetEquippable_Networked_2971853958(NetworkConnection conn, string assetPath)
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
			writer.WriteString(assetPath);
			base.SendObserversRpc(13U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600179D RID: 6045 RVA: 0x00067BDA File Offset: 0x00065DDA
		public void RpcLogic___SetEquippable_Networked_2971853958(NetworkConnection conn, string assetPath)
		{
			this.Avatar.SetEquippable(assetPath);
		}

		// Token: 0x0600179E RID: 6046 RVA: 0x00067BEC File Offset: 0x00065DEC
		private void RpcReader___Observers_SetEquippable_Networked_2971853958(PooledReader PooledReader0, Channel channel)
		{
			string assetPath = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetEquippable_Networked_2971853958(null, assetPath);
		}

		// Token: 0x0600179F RID: 6047 RVA: 0x00067C28 File Offset: 0x00065E28
		private void RpcWriter___Target_SetEquippable_Networked_2971853958(NetworkConnection conn, string assetPath)
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
			writer.WriteString(assetPath);
			base.SendTargetRpc(14U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x060017A0 RID: 6048 RVA: 0x00067CE0 File Offset: 0x00065EE0
		private void RpcReader___Target_SetEquippable_Networked_2971853958(PooledReader PooledReader0, Channel channel)
		{
			string assetPath = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetEquippable_Networked_2971853958(base.LocalConnection, assetPath);
		}

		// Token: 0x060017A1 RID: 6049 RVA: 0x00067D18 File Offset: 0x00065F18
		private void RpcWriter___Observers_SetEquippable_Networked_ExcludeServer_2971853958(NetworkConnection conn, string assetPath)
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
			writer.WriteNetworkConnection(conn);
			writer.WriteString(assetPath);
			base.SendObserversRpc(15U, writer, channel, 0, false, true, false);
			writer.Store();
		}

		// Token: 0x060017A2 RID: 6050 RVA: 0x00067BDA File Offset: 0x00065DDA
		private void RpcLogic___SetEquippable_Networked_ExcludeServer_2971853958(NetworkConnection conn, string assetPath)
		{
			this.Avatar.SetEquippable(assetPath);
		}

		// Token: 0x060017A3 RID: 6051 RVA: 0x00067DDC File Offset: 0x00065FDC
		private void RpcReader___Observers_SetEquippable_Networked_ExcludeServer_2971853958(PooledReader PooledReader0, Channel channel)
		{
			NetworkConnection conn = PooledReader0.ReadNetworkConnection();
			string assetPath = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetEquippable_Networked_ExcludeServer_2971853958(conn, assetPath);
		}

		// Token: 0x060017A4 RID: 6052 RVA: 0x00067E20 File Offset: 0x00066020
		private void RpcWriter___Observers_SendEquippableMessage_Networked_2971853958(NetworkConnection conn, string message)
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
			writer.WriteString(message);
			base.SendObserversRpc(16U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060017A5 RID: 6053 RVA: 0x00067ED6 File Offset: 0x000660D6
		public void RpcLogic___SendEquippableMessage_Networked_2971853958(NetworkConnection conn, string message)
		{
			this.Avatar.ReceiveEquippableMessage(message, null);
		}

		// Token: 0x060017A6 RID: 6054 RVA: 0x00067EE8 File Offset: 0x000660E8
		private void RpcReader___Observers_SendEquippableMessage_Networked_2971853958(PooledReader PooledReader0, Channel channel)
		{
			string message = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SendEquippableMessage_Networked_2971853958(null, message);
		}

		// Token: 0x060017A7 RID: 6055 RVA: 0x00067F24 File Offset: 0x00066124
		private void RpcWriter___Target_SendEquippableMessage_Networked_2971853958(NetworkConnection conn, string message)
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
			writer.WriteString(message);
			base.SendTargetRpc(17U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x060017A8 RID: 6056 RVA: 0x00067FDC File Offset: 0x000661DC
		private void RpcReader___Target_SendEquippableMessage_Networked_2971853958(PooledReader PooledReader0, Channel channel)
		{
			string message = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SendEquippableMessage_Networked_2971853958(base.LocalConnection, message);
		}

		// Token: 0x060017A9 RID: 6057 RVA: 0x00068014 File Offset: 0x00066214
		private void RpcWriter___Observers_SendEquippableMessage_Networked_Vector_4022222929(NetworkConnection conn, string message, Vector3 data)
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
			writer.WriteString(message);
			writer.WriteVector3(data);
			base.SendObserversRpc(18U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060017AA RID: 6058 RVA: 0x000680D7 File Offset: 0x000662D7
		public void RpcLogic___SendEquippableMessage_Networked_Vector_4022222929(NetworkConnection conn, string message, Vector3 data)
		{
			this.Avatar.ReceiveEquippableMessage(message, data);
		}

		// Token: 0x060017AB RID: 6059 RVA: 0x000680EC File Offset: 0x000662EC
		private void RpcReader___Observers_SendEquippableMessage_Networked_Vector_4022222929(PooledReader PooledReader0, Channel channel)
		{
			string message = PooledReader0.ReadString();
			Vector3 data = PooledReader0.ReadVector3();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SendEquippableMessage_Networked_Vector_4022222929(null, message, data);
		}

		// Token: 0x060017AC RID: 6060 RVA: 0x0006813C File Offset: 0x0006633C
		private void RpcWriter___Target_SendEquippableMessage_Networked_Vector_4022222929(NetworkConnection conn, string message, Vector3 data)
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
			writer.WriteString(message);
			writer.WriteVector3(data);
			base.SendTargetRpc(19U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x060017AD RID: 6061 RVA: 0x00068200 File Offset: 0x00066400
		private void RpcReader___Target_SendEquippableMessage_Networked_Vector_4022222929(PooledReader PooledReader0, Channel channel)
		{
			string message = PooledReader0.ReadString();
			Vector3 data = PooledReader0.ReadVector3();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SendEquippableMessage_Networked_Vector_4022222929(base.LocalConnection, message, data);
		}

		// Token: 0x060017AE RID: 6062 RVA: 0x00068248 File Offset: 0x00066448
		private void RpcWriter___Server_SendAnimationTrigger_3615296227(string trigger)
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
			writer.WriteString(trigger);
			base.SendServerRpc(20U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060017AF RID: 6063 RVA: 0x000682EF File Offset: 0x000664EF
		public void RpcLogic___SendAnimationTrigger_3615296227(string trigger)
		{
			this.SetAnimationTrigger_Networked(null, trigger);
		}

		// Token: 0x060017B0 RID: 6064 RVA: 0x000682FC File Offset: 0x000664FC
		private void RpcReader___Server_SendAnimationTrigger_3615296227(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string trigger = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendAnimationTrigger_3615296227(trigger);
		}

		// Token: 0x060017B1 RID: 6065 RVA: 0x00068330 File Offset: 0x00066530
		private void RpcWriter___Observers_SetAnimationTrigger_Networked_2971853958(NetworkConnection conn, string trigger)
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
			writer.WriteString(trigger);
			base.SendObserversRpc(21U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060017B2 RID: 6066 RVA: 0x000683E6 File Offset: 0x000665E6
		public void RpcLogic___SetAnimationTrigger_Networked_2971853958(NetworkConnection conn, string trigger)
		{
			this.SetAnimationTrigger(trigger);
		}

		// Token: 0x060017B3 RID: 6067 RVA: 0x000683F0 File Offset: 0x000665F0
		private void RpcReader___Observers_SetAnimationTrigger_Networked_2971853958(PooledReader PooledReader0, Channel channel)
		{
			string trigger = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetAnimationTrigger_Networked_2971853958(null, trigger);
		}

		// Token: 0x060017B4 RID: 6068 RVA: 0x0006842C File Offset: 0x0006662C
		private void RpcWriter___Target_SetAnimationTrigger_Networked_2971853958(NetworkConnection conn, string trigger)
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
			writer.WriteString(trigger);
			base.SendTargetRpc(22U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x060017B5 RID: 6069 RVA: 0x000684E4 File Offset: 0x000666E4
		private void RpcReader___Target_SetAnimationTrigger_Networked_2971853958(PooledReader PooledReader0, Channel channel)
		{
			string trigger = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetAnimationTrigger_Networked_2971853958(base.LocalConnection, trigger);
		}

		// Token: 0x060017B6 RID: 6070 RVA: 0x0006851C File Offset: 0x0006671C
		private void RpcWriter___Observers_ResetAnimationTrigger_Networked_2971853958(NetworkConnection conn, string trigger)
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
			writer.WriteString(trigger);
			base.SendObserversRpc(23U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060017B7 RID: 6071 RVA: 0x000685D2 File Offset: 0x000667D2
		public void RpcLogic___ResetAnimationTrigger_Networked_2971853958(NetworkConnection conn, string trigger)
		{
			this.ResetAnimationTrigger(trigger);
		}

		// Token: 0x060017B8 RID: 6072 RVA: 0x000685DC File Offset: 0x000667DC
		private void RpcReader___Observers_ResetAnimationTrigger_Networked_2971853958(PooledReader PooledReader0, Channel channel)
		{
			string trigger = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ResetAnimationTrigger_Networked_2971853958(null, trigger);
		}

		// Token: 0x060017B9 RID: 6073 RVA: 0x00068618 File Offset: 0x00066818
		private void RpcWriter___Target_ResetAnimationTrigger_Networked_2971853958(NetworkConnection conn, string trigger)
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
			writer.WriteString(trigger);
			base.SendTargetRpc(24U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x060017BA RID: 6074 RVA: 0x000686D0 File Offset: 0x000668D0
		private void RpcReader___Target_ResetAnimationTrigger_Networked_2971853958(PooledReader PooledReader0, Channel channel)
		{
			string trigger = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ResetAnimationTrigger_Networked_2971853958(base.LocalConnection, trigger);
		}

		// Token: 0x060017BB RID: 6075 RVA: 0x00068708 File Offset: 0x00066908
		private void RpcWriter___Observers_SetCrouched_Networked_1140765316(bool crouched)
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
			writer.WriteBoolean(crouched);
			base.SendObserversRpc(25U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060017BC RID: 6076 RVA: 0x000687BE File Offset: 0x000669BE
		public void RpcLogic___SetCrouched_Networked_1140765316(bool crouched)
		{
			this.Avatar.Anim.SetCrouched(crouched);
		}

		// Token: 0x060017BD RID: 6077 RVA: 0x000687D4 File Offset: 0x000669D4
		private void RpcReader___Observers_SetCrouched_Networked_1140765316(PooledReader PooledReader0, Channel channel)
		{
			bool crouched = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetCrouched_Networked_1140765316(crouched);
		}

		// Token: 0x060017BE RID: 6078 RVA: 0x00068810 File Offset: 0x00066A10
		private void RpcWriter___Observers_SetAnimationBool_Networked_619441887(NetworkConnection conn, string id, bool value)
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
			writer.WriteString(id);
			writer.WriteBoolean(value);
			base.SendObserversRpc(26U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060017BF RID: 6079 RVA: 0x000688D3 File Offset: 0x00066AD3
		public void RpcLogic___SetAnimationBool_Networked_619441887(NetworkConnection conn, string id, bool value)
		{
			this.Avatar.Anim.SetBool(id, value);
		}

		// Token: 0x060017C0 RID: 6080 RVA: 0x000688E8 File Offset: 0x00066AE8
		private void RpcReader___Observers_SetAnimationBool_Networked_619441887(PooledReader PooledReader0, Channel channel)
		{
			string id = PooledReader0.ReadString();
			bool value = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetAnimationBool_Networked_619441887(null, id, value);
		}

		// Token: 0x060017C1 RID: 6081 RVA: 0x00068938 File Offset: 0x00066B38
		private void RpcWriter___Target_SetAnimationBool_Networked_619441887(NetworkConnection conn, string id, bool value)
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
			writer.WriteString(id);
			writer.WriteBoolean(value);
			base.SendTargetRpc(27U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x060017C2 RID: 6082 RVA: 0x000689FC File Offset: 0x00066BFC
		private void RpcReader___Target_SetAnimationBool_Networked_619441887(PooledReader PooledReader0, Channel channel)
		{
			string id = PooledReader0.ReadString();
			bool value = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetAnimationBool_Networked_619441887(base.LocalConnection, id, value);
		}

		// Token: 0x060017C3 RID: 6083 RVA: 0x00068A44 File Offset: 0x00066C44
		private void RpcWriter___Server_SetPanicked_2166136261()
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
			base.SendServerRpc(28U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060017C4 RID: 6084 RVA: 0x00068ADE File Offset: 0x00066CDE
		public void RpcLogic___SetPanicked_2166136261()
		{
			float timeSincePanicked = this.timeSincePanicked;
			this.timeSincePanicked = 0f;
			if (timeSincePanicked > 20f)
			{
				this.ReceivePanicked();
			}
		}

		// Token: 0x060017C5 RID: 6085 RVA: 0x00068B00 File Offset: 0x00066D00
		private void RpcReader___Server_SetPanicked_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SetPanicked_2166136261();
		}

		// Token: 0x060017C6 RID: 6086 RVA: 0x00068B20 File Offset: 0x00066D20
		private void RpcWriter___Observers_ReceivePanicked_2166136261()
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
			base.SendObserversRpc(29U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060017C7 RID: 6087 RVA: 0x00068BCC File Offset: 0x00066DCC
		private void RpcLogic___ReceivePanicked_2166136261()
		{
			this.Avatar.EmotionManager.AddEmotionOverride("Scared", "panicked", 0f, 10);
			if (this.CurrentVehicle != null)
			{
				this.CurrentVehicle.Agent.Flags.OverriddenSpeed = 50f;
				this.CurrentVehicle.Agent.Flags.OverriddenReverseSpeed = 20f;
				this.CurrentVehicle.Agent.Flags.OverrideSpeed = true;
				this.CurrentVehicle.Agent.Flags.IgnoreTrafficLights = true;
				this.CurrentVehicle.Agent.Flags.ObstacleMode = DriveFlags.EObstacleMode.IgnoreOnlySquishy;
				return;
			}
			this.behaviour.CoweringBehaviour.Enable();
		}

		// Token: 0x060017C8 RID: 6088 RVA: 0x00068C90 File Offset: 0x00066E90
		private void RpcReader___Observers_ReceivePanicked_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceivePanicked_2166136261();
		}

		// Token: 0x060017C9 RID: 6089 RVA: 0x00068CB0 File Offset: 0x00066EB0
		private void RpcWriter___Observers_RemovePanicked_2166136261()
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
			base.SendObserversRpc(30U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060017CA RID: 6090 RVA: 0x00068D5C File Offset: 0x00066F5C
		private void RpcLogic___RemovePanicked_2166136261()
		{
			this.Avatar.EmotionManager.RemoveEmotionOverride("panicked");
			if (this.CurrentVehicle != null)
			{
				this.CurrentVehicle.Agent.Flags.ResetFlags();
			}
			this.behaviour.CoweringBehaviour.Disable();
		}

		// Token: 0x060017CB RID: 6091 RVA: 0x00068DB4 File Offset: 0x00066FB4
		private void RpcReader___Observers_RemovePanicked_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___RemovePanicked_2166136261();
		}

		// Token: 0x060017CC RID: 6092 RVA: 0x00068DD4 File Offset: 0x00066FD4
		private void RpcWriter___Target_ReceiveRelationshipData_4052192084(NetworkConnection conn, float relationship, bool unlocked)
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
			writer.WriteSingle(relationship, 0);
			writer.WriteBoolean(unlocked);
			base.SendTargetRpc(31U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x060017CD RID: 6093 RVA: 0x00068E9B File Offset: 0x0006709B
		public void RpcLogic___ReceiveRelationshipData_4052192084(NetworkConnection conn, float relationship, bool unlocked)
		{
			this.RelationData.SetRelationship(relationship);
			Console.Log("Received relationship data for " + this.fullName + " Unlocked: " + unlocked.ToString(), null);
			if (unlocked)
			{
				this.RelationData.Unlock(NPCRelationData.EUnlockType.DirectApproach, false);
			}
		}

		// Token: 0x060017CE RID: 6094 RVA: 0x00068EDC File Offset: 0x000670DC
		private void RpcReader___Target_ReceiveRelationshipData_4052192084(PooledReader PooledReader0, Channel channel)
		{
			float relationship = PooledReader0.ReadSingle(0);
			bool unlocked = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveRelationshipData_4052192084(base.LocalConnection, relationship, unlocked);
		}

		// Token: 0x060017CF RID: 6095 RVA: 0x00068F2C File Offset: 0x0006712C
		private void RpcWriter___Server_SetIsBeingPickPocketed_1140765316(bool pickpocketed)
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
			writer.WriteBoolean(pickpocketed);
			base.SendServerRpc(32U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060017D0 RID: 6096 RVA: 0x00068FD3 File Offset: 0x000671D3
		public void RpcLogic___SetIsBeingPickPocketed_1140765316(bool pickpocketed)
		{
			if (pickpocketed)
			{
				this.behaviour.StationaryBehaviour.Enable_Networked(null);
				return;
			}
			this.behaviour.StationaryBehaviour.Disable_Networked(null);
		}

		// Token: 0x060017D1 RID: 6097 RVA: 0x00068FFC File Offset: 0x000671FC
		private void RpcReader___Server_SetIsBeingPickPocketed_1140765316(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			bool pickpocketed = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SetIsBeingPickPocketed_1140765316(pickpocketed);
		}

		// Token: 0x060017D2 RID: 6098 RVA: 0x00069030 File Offset: 0x00067230
		private void RpcWriter___Server_SendRelationship_431000436(float relationship)
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
			writer.WriteSingle(relationship, 0);
			base.SendServerRpc(33U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060017D3 RID: 6099 RVA: 0x000690DC File Offset: 0x000672DC
		public void RpcLogic___SendRelationship_431000436(float relationship)
		{
			this.SetRelationship(relationship);
		}

		// Token: 0x060017D4 RID: 6100 RVA: 0x000690E8 File Offset: 0x000672E8
		private void RpcReader___Server_SendRelationship_431000436(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			float relationship = PooledReader0.ReadSingle(0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendRelationship_431000436(relationship);
		}

		// Token: 0x060017D5 RID: 6101 RVA: 0x00069120 File Offset: 0x00067320
		private void RpcWriter___Observers_SetRelationship_431000436(float relationship)
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
			writer.WriteSingle(relationship, 0);
			base.SendObserversRpc(34U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060017D6 RID: 6102 RVA: 0x000691DB File Offset: 0x000673DB
		private void RpcLogic___SetRelationship_431000436(float relationship)
		{
			this.RelationData.SetRelationship(relationship);
		}

		// Token: 0x060017D7 RID: 6103 RVA: 0x000691EC File Offset: 0x000673EC
		private void RpcReader___Observers_SetRelationship_431000436(PooledReader PooledReader0, Channel channel)
		{
			float relationship = PooledReader0.ReadSingle(0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetRelationship_431000436(relationship);
		}

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x060017D8 RID: 6104 RVA: 0x00069222 File Offset: 0x00067422
		// (set) Token: 0x060017D9 RID: 6105 RVA: 0x0006922A File Offset: 0x0006742A
		public NetworkObject SyncAccessor_PlayerConversant
		{
			get
			{
				return this.PlayerConversant;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.PlayerConversant = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___PlayerConversant.SetValue(value, value);
				}
			}
		}

		// Token: 0x060017DA RID: 6106 RVA: 0x00069268 File Offset: 0x00067468
		public override bool NPC(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 != 0U)
			{
				return false;
			}
			if (PooledReader0 == null)
			{
				this.sync___set_value_PlayerConversant(this.syncVar___PlayerConversant.GetValue(true), true);
				return true;
			}
			NetworkObject value = PooledReader0.ReadNetworkObject();
			this.sync___set_value_PlayerConversant(value, Boolean2);
			return true;
		}

		// Token: 0x060017DB RID: 6107 RVA: 0x000692BC File Offset: 0x000674BC
		protected virtual void dll()
		{
			this.GetHealth();
			this.intObj.onHovered.AddListener(new UnityAction(this.Hovered_Internal));
			this.intObj.onInteractStart.AddListener(new UnityAction(this.Interacted_Internal));
			this.Inventory = base.GetComponent<NPCInventory>();
			Player.onLocalPlayerSpawned = (Action)Delegate.Remove(Player.onLocalPlayerSpawned, new Action(this.PlayerSpawned));
			Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(this.PlayerSpawned));
			if (!NPCManager.NPCRegistry.Contains(this))
			{
				NPCManager.NPCRegistry.Add(this);
			}
			this.awareness.onNoticedGeneralCrime.AddListener(new UnityAction<Player>(this.SetUnsettled_30s));
			this.awareness.onNoticedPettyCrime.AddListener(new UnityAction<Player>(this.SetUnsettled_30s));
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in this.Avatar.BodyMeshes)
			{
				this.OutlineRenderers.Add(skinnedMeshRenderer.gameObject);
			}
			if (this.VoiceOverEmitter == null)
			{
				this.VoiceOverEmitter = this.Avatar.HeadBone.GetComponentInChildren<VOEmitter>();
			}
			this.RelationData.Init(this);
			if (this.RelationData.Unlocked)
			{
				this.<Awake>g__Unlocked|114_0(NPCRelationData.EUnlockType.DirectApproach, false);
			}
			else
			{
				NPCRelationData relationData = this.RelationData;
				relationData.onUnlocked = (Action<NPCRelationData.EUnlockType, bool>)Delegate.Combine(relationData.onUnlocked, new Action<NPCRelationData.EUnlockType, bool>(this.<Awake>g__Unlocked|114_0));
			}
			foreach (NPC x in this.RelationData.Connections)
			{
				if (!(x == null) && x == this)
				{
					Console.LogWarning("NPC " + this.fullName + " has a connection to itself", null);
				}
			}
			this.headlightStartTime = 1700 + Mathf.RoundToInt(90f * Mathf.Clamp01((float)(this.fullName[0].GetHashCode() / 1000 % 10) / 10f));
			this.InitializeSaveable();
			this.defaultAggression = this.Aggression;
		}

		// Token: 0x04001520 RID: 5408
		public const float PANIC_DURATION = 20f;

		// Token: 0x04001521 RID: 5409
		public const bool RequiresRegionUnlocked = true;

		// Token: 0x04001522 RID: 5410
		[Header("Info Settings")]
		public string FirstName = string.Empty;

		// Token: 0x04001523 RID: 5411
		public bool hasLastName = true;

		// Token: 0x04001524 RID: 5412
		public string LastName = string.Empty;

		// Token: 0x04001526 RID: 5414
		public string ID = string.Empty;

		// Token: 0x04001527 RID: 5415
		public bool AutoGenerateMugshot = true;

		// Token: 0x04001528 RID: 5416
		public Sprite MugshotSprite;

		// Token: 0x04001529 RID: 5417
		public EMapRegion Region = EMapRegion.Downtown;

		// Token: 0x0400152A RID: 5418
		[Header("If true, NPC will respawn next day instead of waiting 3 days.")]
		public bool IsImportant;

		// Token: 0x0400152B RID: 5419
		[Header("Personality")]
		[Range(0f, 1f)]
		public float Aggression;

		// Token: 0x0400152C RID: 5420
		[Header("References")]
		[SerializeField]
		protected Transform modelContainer;

		// Token: 0x0400152D RID: 5421
		[SerializeField]
		protected NPCMovement movement;

		// Token: 0x0400152E RID: 5422
		[SerializeField]
		protected InteractableObject intObj;

		// Token: 0x0400152F RID: 5423
		public DialogueHandler dialogueHandler;

		// Token: 0x04001530 RID: 5424
		public Avatar Avatar;

		// Token: 0x04001531 RID: 5425
		public NPCAwareness awareness;

		// Token: 0x04001532 RID: 5426
		public NPCResponses responses;

		// Token: 0x04001533 RID: 5427
		public NPCActions actions;

		// Token: 0x04001534 RID: 5428
		public NPCBehaviour behaviour;

		// Token: 0x04001536 RID: 5430
		public VOEmitter VoiceOverEmitter;

		// Token: 0x04001537 RID: 5431
		public NPCHealth Health;

		// Token: 0x04001539 RID: 5433
		public Action<LandVehicle> onEnterVehicle;

		// Token: 0x0400153A RID: 5434
		public Action<LandVehicle> onExitVehicle;

		// Token: 0x0400153D RID: 5437
		[Header("Summoning")]
		public bool CanBeSummoned = true;

		// Token: 0x0400153E RID: 5438
		[Header("Relationship")]
		public NPCRelationData RelationData;

		// Token: 0x0400153F RID: 5439
		public string NPCUnlockedVariable = string.Empty;

		// Token: 0x04001540 RID: 5440
		public bool ShowRelationshipInfo = true;

		// Token: 0x04001541 RID: 5441
		[Header("Messaging")]
		public List<EConversationCategory> ConversationCategories;

		// Token: 0x04001543 RID: 5443
		public bool ConversationCanBeHidden = true;

		// Token: 0x04001544 RID: 5444
		public Action onConversationCreated;

		// Token: 0x04001545 RID: 5445
		[Header("Other Settings")]
		public bool CanOpenDoors = true;

		// Token: 0x04001546 RID: 5446
		[SerializeField]
		protected List<GameObject> OutlineRenderers = new List<GameObject>();

		// Token: 0x04001547 RID: 5447
		protected Outlinable OutlineEffect;

		// Token: 0x0400154B RID: 5451
		[Header("GUID")]
		public string BakedGUID = string.Empty;

		// Token: 0x0400154E RID: 5454
		public Action<bool> onVisibilityChanged;

		// Token: 0x0400154F RID: 5455
		[HideInInspector]
		[SyncVar]
		public NetworkObject PlayerConversant;

		// Token: 0x04001551 RID: 5457
		private Coroutine resetUnsettledCoroutine;

		// Token: 0x04001553 RID: 5459
		private List<int> impactHistory = new List<int>();

		// Token: 0x04001554 RID: 5460
		private int headlightStartTime = 1700;

		// Token: 0x04001555 RID: 5461
		private int heaedLightsEndTime = 600;

		// Token: 0x04001556 RID: 5462
		protected float defaultAggression;

		// Token: 0x04001557 RID: 5463
		private Coroutine lerpScaleRoutine;

		// Token: 0x04001558 RID: 5464
		public SyncVar<NetworkObject> syncVar___PlayerConversant;

		// Token: 0x04001559 RID: 5465
		private bool dll_Excuted;

		// Token: 0x0400155A RID: 5466
		private bool dll_Excuted;
	}
}
