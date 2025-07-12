using System;
using System.Collections.Generic;
using System.Linq;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Combat;
using ScheduleOne.DevUtilities;
using ScheduleOne.Doors;
using ScheduleOne.GameTime;
using ScheduleOne.ItemFramework;
using ScheduleOne.Map;
using ScheduleOne.Product;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x0200055C RID: 1372
	public class NPCBehaviour : NetworkBehaviour
	{
		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x0600208C RID: 8332 RVA: 0x000858B3 File Offset: 0x00083AB3
		// (set) Token: 0x0600208D RID: 8333 RVA: 0x000858BB File Offset: 0x00083ABB
		public Behaviour activeBehaviour { get; set; }

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x0600208E RID: 8334 RVA: 0x000858C4 File Offset: 0x00083AC4
		// (set) Token: 0x0600208F RID: 8335 RVA: 0x000858CC File Offset: 0x00083ACC
		public NPC Npc { get; private set; }

		// Token: 0x06002090 RID: 8336 RVA: 0x000858D8 File Offset: 0x00083AD8
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.Behaviour.NPCBehaviour_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002091 RID: 8337 RVA: 0x000858F8 File Offset: 0x00083AF8
		protected virtual void Start()
		{
			this.Npc.Avatar.Anim.onHeavyFlinch.AddListener(new UnityAction(this.HeavyFlinchBehaviour.Flinch));
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
			instance2.onMinutePass = (Action)Delegate.Combine(instance2.onMinutePass, new Action(this.MinPass));
			for (int i = 0; i < this.behaviourStack.Count; i++)
			{
				Behaviour b = this.behaviourStack[i];
				if (b.Enabled)
				{
					this.enabledBehaviours.Add(b);
				}
				b.onEnable.AddListener(delegate()
				{
					this.AddEnabledBehaviour(b);
				});
				b.onDisable.AddListener(delegate()
				{
					this.RemoveEnabledBehaviour(b);
				});
			}
		}

		// Token: 0x06002092 RID: 8338 RVA: 0x00085A0C File Offset: 0x00083C0C
		private void OnDestroy()
		{
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			}
		}

		// Token: 0x06002093 RID: 8339 RVA: 0x00085A3C File Offset: 0x00083C3C
		protected override void OnValidate()
		{
			base.OnValidate();
			this.behaviourStack = base.GetComponentsInChildren<Behaviour>().ToList<Behaviour>();
			this.SortBehaviourStack();
		}

		// Token: 0x06002094 RID: 8340 RVA: 0x00085A5B File Offset: 0x00083C5B
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (this.activeBehaviour != null)
			{
				this.activeBehaviour.Begin_Networked(connection);
			}
		}

		// Token: 0x06002095 RID: 8341 RVA: 0x00085A80 File Offset: 0x00083C80
		[ServerRpc(RequireOwnership = false)]
		public void Summon(string buildingGUID, int doorIndex, float duration)
		{
			this.RpcWriter___Server_Summon_900355577(buildingGUID, doorIndex, duration);
		}

		// Token: 0x06002096 RID: 8342 RVA: 0x00085A9F File Offset: 0x00083C9F
		[ServerRpc(RequireOwnership = false)]
		public void ConsumeProduct(ProductItemInstance product)
		{
			this.RpcWriter___Server_ConsumeProduct_2622925554(product);
		}

		// Token: 0x06002097 RID: 8343 RVA: 0x00085AAC File Offset: 0x00083CAC
		protected virtual void OnKnockOut()
		{
			this.CoweringBehaviour.Disable_Networked(null);
			this.RagdollBehaviour.Disable_Networked(null);
			this.CallPoliceBehaviour.Disable_Networked(null);
			this.GenericDialogueBehaviour.Disable_Networked(null);
			this.HeavyFlinchBehaviour.Disable_Networked(null);
			this.FacePlayerBehaviour.Disable_Networked(null);
			this.SummonBehaviour.Disable_Networked(null);
			this.ConsumeProductBehaviour.Disable_Networked(null);
			this.CombatBehaviour.Disable_Networked(null);
			this.FleeBehaviour.Disable_Networked(null);
			this.StationaryBehaviour.Disable_Networked(null);
			this.RequestProductBehaviour.Disable_Networked(null);
			foreach (Behaviour behaviour in this.behaviourStack)
			{
				if (!(behaviour == this.DeadBehaviour) && !(behaviour == this.UnconsciousBehaviour) && behaviour.Active)
				{
					behaviour.End_Networked(null);
				}
			}
		}

		// Token: 0x06002098 RID: 8344 RVA: 0x00085BB4 File Offset: 0x00083DB4
		protected virtual void OnDie()
		{
			this.OnKnockOut();
			this.UnconsciousBehaviour.Disable_Networked(null);
		}

		// Token: 0x06002099 RID: 8345 RVA: 0x00085BC8 File Offset: 0x00083DC8
		public Behaviour GetBehaviour(string BehaviourName)
		{
			Behaviour behaviour = this.behaviourStack.Find((Behaviour x) => x.Name.ToLower() == BehaviourName.ToLower());
			if (behaviour == null)
			{
				Console.LogWarning("No behaviour found with name '" + BehaviourName + "'", null);
			}
			return behaviour;
		}

		// Token: 0x0600209A RID: 8346 RVA: 0x00085C20 File Offset: 0x00083E20
		public virtual void Update()
		{
			if (this.DEBUG_MODE && this.activeBehaviour != null)
			{
				Debug.Log("Active behaviour: " + this.activeBehaviour.Name);
			}
			if (InstanceFinder.IsHost)
			{
				Behaviour enabledBehaviour = this.GetEnabledBehaviour();
				if (enabledBehaviour != this.activeBehaviour)
				{
					if (this.activeBehaviour != null)
					{
						this.activeBehaviour.Pause_Networked(null);
					}
					if (enabledBehaviour != null)
					{
						if (enabledBehaviour.Started)
						{
							enabledBehaviour.Resume_Networked(null);
						}
						else
						{
							enabledBehaviour.Begin_Networked(null);
						}
					}
				}
			}
			if (this.activeBehaviour != null && this.activeBehaviour.Active)
			{
				this.activeBehaviour.BehaviourUpdate();
			}
		}

		// Token: 0x0600209B RID: 8347 RVA: 0x00085CDA File Offset: 0x00083EDA
		public virtual void LateUpdate()
		{
			if (this.activeBehaviour != null && this.activeBehaviour.Active)
			{
				this.activeBehaviour.BehaviourLateUpdate();
			}
		}

		// Token: 0x0600209C RID: 8348 RVA: 0x00085D02 File Offset: 0x00083F02
		protected virtual void MinPass()
		{
			if (this.activeBehaviour != null && this.activeBehaviour.Active)
			{
				this.activeBehaviour.ActiveMinPass();
			}
		}

		// Token: 0x0600209D RID: 8349 RVA: 0x00085D2A File Offset: 0x00083F2A
		public void SortBehaviourStack()
		{
			this.behaviourStack = (from x in this.behaviourStack
			orderby x.Priority descending
			select x).ToList<Behaviour>();
		}

		// Token: 0x0600209E RID: 8350 RVA: 0x00085D61 File Offset: 0x00083F61
		private Behaviour GetEnabledBehaviour()
		{
			return this.enabledBehaviours.FirstOrDefault<Behaviour>();
		}

		// Token: 0x0600209F RID: 8351 RVA: 0x00085D70 File Offset: 0x00083F70
		private void AddEnabledBehaviour(Behaviour b)
		{
			if (!this.enabledBehaviours.Contains(b))
			{
				this.enabledBehaviours.Add(b);
				this.enabledBehaviours = (from x in this.enabledBehaviours
				orderby x.Priority descending
				select x).ToList<Behaviour>();
			}
		}

		// Token: 0x060020A0 RID: 8352 RVA: 0x00085DCC File Offset: 0x00083FCC
		private void RemoveEnabledBehaviour(Behaviour b)
		{
			if (this.enabledBehaviours.Contains(b))
			{
				this.enabledBehaviours.Remove(b);
				this.enabledBehaviours = (from x in this.enabledBehaviours
				orderby x.Priority descending
				select x).ToList<Behaviour>();
			}
		}

		// Token: 0x060020A2 RID: 8354 RVA: 0x00085E48 File Offset: 0x00084048
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.NPCBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.NPCBehaviourAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_Summon_900355577));
			base.RegisterServerRpc(1U, new ServerRpcDelegate(this.RpcReader___Server_ConsumeProduct_2622925554));
		}

		// Token: 0x060020A3 RID: 8355 RVA: 0x00085E94 File Offset: 0x00084094
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.NPCBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.NPCBehaviourAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x060020A4 RID: 8356 RVA: 0x00085EA7 File Offset: 0x000840A7
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060020A5 RID: 8357 RVA: 0x00085EB8 File Offset: 0x000840B8
		private void RpcWriter___Server_Summon_900355577(string buildingGUID, int doorIndex, float duration)
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
			writer.WriteString(buildingGUID);
			writer.WriteInt32(doorIndex, 1);
			writer.WriteSingle(duration, 0);
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060020A6 RID: 8358 RVA: 0x00085F84 File Offset: 0x00084184
		public void RpcLogic___Summon_900355577(string buildingGUID, int doorIndex, float duration)
		{
			NPCBehaviour.<>c__DisplayClass32_0 CS$<>8__locals1 = new NPCBehaviour.<>c__DisplayClass32_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.duration = duration;
			NPCEnterableBuilding @object = GUIDManager.GetObject<NPCEnterableBuilding>(new Guid(buildingGUID));
			if (@object == null)
			{
				Console.LogError("Failed to find building with GUID: " + buildingGUID, null);
				return;
			}
			if (@object.Doors.Length <= doorIndex)
			{
				Console.LogError("Door index out of range: " + doorIndex.ToString() + " / " + @object.Doors.Length.ToString(), null);
				return;
			}
			StaticDoor lastEnteredDoor = @object.Doors[doorIndex];
			this.Npc.LastEnteredDoor = lastEnteredDoor;
			this.SummonBehaviour.Enable_Networked(null);
			if (this.summonRoutine != null)
			{
				base.StopCoroutine(this.summonRoutine);
			}
			this.summonRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<Summon>g__Routine|0());
		}

		// Token: 0x060020A7 RID: 8359 RVA: 0x00086050 File Offset: 0x00084250
		private void RpcReader___Server_Summon_900355577(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string buildingGUID = PooledReader0.ReadString();
			int doorIndex = PooledReader0.ReadInt32(1);
			float duration = PooledReader0.ReadSingle(0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___Summon_900355577(buildingGUID, doorIndex, duration);
		}

		// Token: 0x060020A8 RID: 8360 RVA: 0x000860B0 File Offset: 0x000842B0
		private void RpcWriter___Server_ConsumeProduct_2622925554(ProductItemInstance product)
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
			writer.WriteProductItemInstance(product);
			base.SendServerRpc(1U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060020A9 RID: 8361 RVA: 0x00086157 File Offset: 0x00084357
		public void RpcLogic___ConsumeProduct_2622925554(ProductItemInstance product)
		{
			this.ConsumeProductBehaviour.SendProduct(product);
			this.ConsumeProductBehaviour.Enable_Networked(null);
		}

		// Token: 0x060020AA RID: 8362 RVA: 0x00086174 File Offset: 0x00084374
		private void RpcReader___Server_ConsumeProduct_2622925554(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			ProductItemInstance product = PooledReader0.ReadProductItemInstance();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___ConsumeProduct_2622925554(product);
		}

		// Token: 0x060020AB RID: 8363 RVA: 0x000861A8 File Offset: 0x000843A8
		protected virtual void dll()
		{
			this.Npc = base.GetComponentInParent<NPC>();
			this.Npc.Health.onKnockedOut.AddListener(new UnityAction(this.OnKnockOut));
			this.Npc.Health.onDie.AddListener(new UnityAction(this.OnDie));
		}

		// Token: 0x04001913 RID: 6419
		public bool DEBUG_MODE;

		// Token: 0x04001914 RID: 6420
		[Header("References")]
		public NPCScheduleManager ScheduleManager;

		// Token: 0x04001915 RID: 6421
		[Header("Default Behaviours")]
		public CoweringBehaviour CoweringBehaviour;

		// Token: 0x04001916 RID: 6422
		public RagdollBehaviour RagdollBehaviour;

		// Token: 0x04001917 RID: 6423
		public CallPoliceBehaviour CallPoliceBehaviour;

		// Token: 0x04001918 RID: 6424
		public GenericDialogueBehaviour GenericDialogueBehaviour;

		// Token: 0x04001919 RID: 6425
		public HeavyFlinchBehaviour HeavyFlinchBehaviour;

		// Token: 0x0400191A RID: 6426
		public FacePlayerBehaviour FacePlayerBehaviour;

		// Token: 0x0400191B RID: 6427
		public DeadBehaviour DeadBehaviour;

		// Token: 0x0400191C RID: 6428
		public UnconsciousBehaviour UnconsciousBehaviour;

		// Token: 0x0400191D RID: 6429
		public Behaviour SummonBehaviour;

		// Token: 0x0400191E RID: 6430
		public ConsumeProductBehaviour ConsumeProductBehaviour;

		// Token: 0x0400191F RID: 6431
		public CombatBehaviour CombatBehaviour;

		// Token: 0x04001920 RID: 6432
		public FleeBehaviour FleeBehaviour;

		// Token: 0x04001921 RID: 6433
		public StationaryBehaviour StationaryBehaviour;

		// Token: 0x04001922 RID: 6434
		public RequestProductBehaviour RequestProductBehaviour;

		// Token: 0x04001923 RID: 6435
		[SerializeField]
		protected List<Behaviour> behaviourStack = new List<Behaviour>();

		// Token: 0x04001926 RID: 6438
		private Coroutine summonRoutine;

		// Token: 0x04001927 RID: 6439
		[SerializeField]
		private List<Behaviour> enabledBehaviours = new List<Behaviour>();

		// Token: 0x04001928 RID: 6440
		private bool dll_Excuted;

		// Token: 0x04001929 RID: 6441
		private bool dll_Excuted;
	}
}
