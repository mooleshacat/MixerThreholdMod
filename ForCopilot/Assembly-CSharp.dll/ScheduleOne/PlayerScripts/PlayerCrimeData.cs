using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Object.Synchronizing;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Law;
using ScheduleOne.Levelling;
using ScheduleOne.Map;
using ScheduleOne.NPCs;
using ScheduleOne.Police;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.PlayerScripts
{
	// Token: 0x02000629 RID: 1577
	public class PlayerCrimeData : NetworkBehaviour
	{
		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x06002866 RID: 10342 RVA: 0x000A5BCC File Offset: 0x000A3DCC
		// (set) Token: 0x06002867 RID: 10343 RVA: 0x000A5BD4 File Offset: 0x000A3DD4
		public PlayerCrimeData.EPursuitLevel CurrentPursuitLevel
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<CurrentPursuitLevel>k__BackingField;
			}
			[CompilerGenerated]
			[ServerRpc(RunLocally = true)]
			protected set
			{
				this.RpcWriter___Server_set_CurrentPursuitLevel_2979171596(value);
				this.RpcLogic___set_CurrentPursuitLevel_2979171596(value);
			}
		}

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x06002868 RID: 10344 RVA: 0x000A5BEA File Offset: 0x000A3DEA
		// (set) Token: 0x06002869 RID: 10345 RVA: 0x000A5BF2 File Offset: 0x000A3DF2
		public Vector3 LastKnownPosition
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<LastKnownPosition>k__BackingField;
			}
			[CompilerGenerated]
			[ServerRpc(RunLocally = true)]
			protected set
			{
				this.RpcWriter___Server_set_LastKnownPosition_4276783012(value);
				this.RpcLogic___set_LastKnownPosition_4276783012(value);
			}
		}

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x0600286A RID: 10346 RVA: 0x000A5C08 File Offset: 0x000A3E08
		// (set) Token: 0x0600286B RID: 10347 RVA: 0x000A5C10 File Offset: 0x000A3E10
		public float CurrentArrestProgress { get; protected set; }

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x0600286C RID: 10348 RVA: 0x000A5C19 File Offset: 0x000A3E19
		// (set) Token: 0x0600286D RID: 10349 RVA: 0x000A5C21 File Offset: 0x000A3E21
		public float CurrentBodySearchProgress { get; protected set; }

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x0600286E RID: 10350 RVA: 0x000A5C2A File Offset: 0x000A3E2A
		// (set) Token: 0x0600286F RID: 10351 RVA: 0x000A5C32 File Offset: 0x000A3E32
		public float TimeSinceLastBodySearch { get; set; }

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x06002870 RID: 10352 RVA: 0x000A5C3B File Offset: 0x000A3E3B
		// (set) Token: 0x06002871 RID: 10353 RVA: 0x000A5C43 File Offset: 0x000A3E43
		public bool EvadedArrest { get; protected set; }

		// Token: 0x06002872 RID: 10354 RVA: 0x000A5C4C File Offset: 0x000A3E4C
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.PlayerScripts.PlayerCrimeData_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002873 RID: 10355 RVA: 0x000A5C6B File Offset: 0x000A3E6B
		private void Start()
		{
			NetworkSingleton<TimeManager>.Instance._onSleepStart.RemoveListener(new UnityAction(this.OnSleepStart));
			NetworkSingleton<TimeManager>.Instance._onSleepStart.AddListener(new UnityAction(this.OnSleepStart));
		}

		// Token: 0x06002874 RID: 10356 RVA: 0x000A5CA3 File Offset: 0x000A3EA3
		private void OnDestroy()
		{
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				NetworkSingleton<TimeManager>.Instance._onSleepStart.RemoveListener(new UnityAction(this.OnSleepStart));
			}
		}

		// Token: 0x06002875 RID: 10357 RVA: 0x000A5CC8 File Offset: 0x000A3EC8
		protected virtual void Update()
		{
			this.CurrentPursuitLevelDuration += Time.deltaTime;
			this.TimeSincePursuitStart += Time.deltaTime;
			this.TimeSinceSighted += Time.deltaTime;
			this.timeSinceLastShot += Time.deltaTime;
			this.TimeSinceLastBodySearch += Time.deltaTime;
			if (!this.Player.IsOwner)
			{
				return;
			}
			if (this.CurrentPursuitLevel != PlayerCrimeData.EPursuitLevel.None && this.CurrentPursuitLevel != PlayerCrimeData.EPursuitLevel.Lethal)
			{
				this.UpdateEscalation();
			}
			if (this.CurrentPursuitLevel != PlayerCrimeData.EPursuitLevel.None)
			{
				this.UpdateTimeout();
				this.UpdateMusic();
			}
			if (this.CurrentPursuitLevel != PlayerCrimeData.EPursuitLevel.None && this.TimeSinceSighted > 2f)
			{
				this.Player.VisualState.ApplyState("SearchedFor", PlayerVisualState.EVisualState.SearchedFor, 0f);
			}
			else
			{
				this.Player.VisualState.RemoveState("SearchedFor", 0f);
			}
			for (int i = 0; i < this.Collisions.Count; i++)
			{
				this.Collisions[i].TimeSince += Time.deltaTime;
				if (this.Collisions[i].TimeSince > 30f)
				{
					this.Collisions.RemoveAt(i);
					i--;
				}
			}
			Singleton<HUD>.Instance.CrimeStatusUI.UpdateStatus();
			if ((float)this.Collisions.Count >= 3f)
			{
				this.RecordLastKnownPosition(true);
				this.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.Investigating);
				this.AddCrime(new VehicularAssault(), this.Collisions.Count - 1);
				Singleton<LawManager>.Instance.PoliceCalled(this.Player, new VehicularAssault());
				this.Collisions.Clear();
			}
		}

		// Token: 0x06002876 RID: 10358 RVA: 0x000A5E78 File Offset: 0x000A4078
		protected virtual void LateUpdate()
		{
			if (this.CurrentArrestProgress > 0f)
			{
				Singleton<ProgressSlider>.Instance.Configure("Cuffing...", new Color32(75, 165, byte.MaxValue, byte.MaxValue));
				Singleton<ProgressSlider>.Instance.ShowProgress(this.CurrentArrestProgress);
			}
			else if (this.CurrentBodySearchProgress > 0f)
			{
				Singleton<ProgressSlider>.Instance.Configure("Being searched...", new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
				Singleton<ProgressSlider>.Instance.ShowProgress(this.CurrentBodySearchProgress);
			}
			this.CurrentArrestProgress = 0f;
			this.CurrentBodySearchProgress = 0f;
		}

		// Token: 0x06002877 RID: 10359 RVA: 0x000A5F30 File Offset: 0x000A4130
		public void SetPursuitLevel(PlayerCrimeData.EPursuitLevel level)
		{
			if (GameManager.IS_TUTORIAL)
			{
				return;
			}
			Debug.Log("New pursuit level: " + level.ToString());
			PlayerCrimeData.EPursuitLevel currentPursuitLevel = this.CurrentPursuitLevel;
			this.CurrentPursuitLevel = level;
			if (level != PlayerCrimeData.EPursuitLevel.None)
			{
				this.BodySearchPending = false;
			}
			if (currentPursuitLevel == PlayerCrimeData.EPursuitLevel.None && level != PlayerCrimeData.EPursuitLevel.None)
			{
				this.TimeSincePursuitStart = 0f;
				this.TimeSinceSighted = 0f;
				this.Player.VisualState.ApplyState("Wanted", PlayerVisualState.EVisualState.Wanted, 0f);
				if (this.Player.Owner.IsLocalClient)
				{
					this._lightCombatTrack.Enable();
				}
			}
			if (level == PlayerCrimeData.EPursuitLevel.Lethal && this.Player.Owner.IsLocalClient)
			{
				this._lightCombatTrack.Stop();
				this._heavyCombatTrack.Enable();
			}
			if (currentPursuitLevel != PlayerCrimeData.EPursuitLevel.None && level == PlayerCrimeData.EPursuitLevel.None)
			{
				this.ClearCrimes();
				this.Player.VisualState.RemoveState("Wanted", 0f);
				if (this.Player.Owner.IsLocalClient)
				{
					this._lightCombatTrack.Disable();
					this._lightCombatTrack.Stop();
					this._heavyCombatTrack.Disable();
					this._heavyCombatTrack.Stop();
				}
			}
			this.CurrentPursuitLevelDuration = 0f;
			if (this.Player.IsOwner)
			{
				Singleton<HUD>.Instance.CrimeStatusUI.UpdateStatus();
			}
		}

		// Token: 0x06002878 RID: 10360 RVA: 0x000A6084 File Offset: 0x000A4284
		public void Escalate()
		{
			if (this.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.None)
			{
				this.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.Investigating);
				return;
			}
			if (this.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.Investigating)
			{
				this.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.Arresting);
				return;
			}
			if (this.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.Arresting)
			{
				this.SetEvaded();
				this.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.NonLethal);
				if (PoliceStation.GetClosestPoliceStation(this.Player.Avatar.MiddleSpineRB.position).TimeSinceLastDispatch > 10f)
				{
					PoliceStation.GetClosestPoliceStation(this.Player.Avatar.MiddleSpineRB.position).Dispatch(1, this.Player, PoliceStation.EDispatchType.Auto, true);
					return;
				}
			}
			else if (this.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.NonLethal)
			{
				this.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.Lethal);
				PoliceStation.GetClosestPoliceStation(this.Player.Avatar.MiddleSpineRB.position);
				PoliceStation.GetClosestPoliceStation(this.Player.Avatar.MiddleSpineRB.position).Dispatch(1, this.Player, PoliceStation.EDispatchType.Auto, true);
			}
		}

		// Token: 0x06002879 RID: 10361 RVA: 0x000A616C File Offset: 0x000A436C
		public void Deescalate()
		{
			if (this.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.Investigating)
			{
				this.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.None);
				return;
			}
			if (this.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.Arresting)
			{
				this.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.Investigating);
				return;
			}
			if (this.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.NonLethal)
			{
				this.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.Arresting);
				return;
			}
			if (this.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.Lethal)
			{
				this.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.NonLethal);
			}
		}

		// Token: 0x0600287A RID: 10362 RVA: 0x000A61BC File Offset: 0x000A43BC
		[ObserversRpc(RunLocally = true)]
		public void RecordLastKnownPosition(bool resetTimeSinceSighted)
		{
			this.RpcWriter___Observers_RecordLastKnownPosition_1140765316(resetTimeSinceSighted);
			this.RpcLogic___RecordLastKnownPosition_1140765316(resetTimeSinceSighted);
		}

		// Token: 0x0600287B RID: 10363 RVA: 0x000A61D2 File Offset: 0x000A43D2
		public void SetArrestProgress(float progress)
		{
			this.CurrentArrestProgress = progress;
			if (progress >= 1f)
			{
				this.Player.Arrest();
				this.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.None);
			}
		}

		// Token: 0x0600287C RID: 10364 RVA: 0x000A61F5 File Offset: 0x000A43F5
		public void ResetBodysearchCooldown()
		{
			this.TimeSinceLastBodySearch = 0f;
		}

		// Token: 0x0600287D RID: 10365 RVA: 0x000A6202 File Offset: 0x000A4402
		public void SetBodySearchProgress(float progress)
		{
			this.CurrentBodySearchProgress = progress;
			if (this.CurrentBodySearchProgress >= 1f)
			{
				this.TimeSinceLastBodySearch = 0f;
				this.BodySearchPending = false;
			}
		}

		// Token: 0x0600287E RID: 10366 RVA: 0x000A622A File Offset: 0x000A442A
		private void OnDie()
		{
			if (this.CurrentPursuitLevel != PlayerCrimeData.EPursuitLevel.None)
			{
				this.SetArrestProgress(1f);
			}
		}

		// Token: 0x0600287F RID: 10367 RVA: 0x000A6240 File Offset: 0x000A4440
		public void AddCrime(Crime crime, int quantity = 1)
		{
			if (crime == null)
			{
				return;
			}
			Debug.Log("Adding crime: " + ((crime != null) ? crime.ToString() : null));
			Crime[] array = this.Crimes.Keys.ToArray<Crime>();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].GetType() == crime.GetType())
				{
					Dictionary<Crime, int> crimes = this.Crimes;
					Crime key = array[i];
					crimes[key] += quantity;
					return;
				}
			}
			this.Crimes.Add(crime, quantity);
		}

		// Token: 0x06002880 RID: 10368 RVA: 0x000A62CA File Offset: 0x000A44CA
		public void ClearCrimes()
		{
			this.Crimes.Clear();
			this.EvadedArrest = false;
		}

		// Token: 0x06002881 RID: 10369 RVA: 0x000A62E0 File Offset: 0x000A44E0
		public bool IsCrimeOnRecord(Type crime)
		{
			Crime[] array = this.Crimes.Keys.ToArray<Crime>();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].GetType() == crime)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002882 RID: 10370 RVA: 0x000A631F File Offset: 0x000A451F
		public void SetEvaded()
		{
			this.EvadedArrest = true;
		}

		// Token: 0x06002883 RID: 10371 RVA: 0x000A6328 File Offset: 0x000A4528
		private void OnSleepStart()
		{
			if (this.CurrentPursuitLevel != PlayerCrimeData.EPursuitLevel.None)
			{
				this.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.None);
				this.ClearCrimes();
			}
		}

		// Token: 0x06002884 RID: 10372 RVA: 0x000A6340 File Offset: 0x000A4540
		private void UpdateEscalation()
		{
			if (this.TimeSinceSighted > 1f)
			{
				return;
			}
			if (this.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.Arresting)
			{
				if (this.CurrentPursuitLevelDuration > 25f)
				{
					this.Escalate();
					return;
				}
			}
			else if (this.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.NonLethal && this.CurrentPursuitLevelDuration > 120f)
			{
				this.Escalate();
			}
		}

		// Token: 0x06002885 RID: 10373 RVA: 0x000A6394 File Offset: 0x000A4594
		private void UpdateTimeout()
		{
			if (!this.Player.IsOwner)
			{
				return;
			}
			if (this.TimeSinceSighted > this.GetSearchTime() + 3f)
			{
				this.TimeoutPursuit();
			}
		}

		// Token: 0x06002886 RID: 10374 RVA: 0x000A63C0 File Offset: 0x000A45C0
		private void UpdateMusic()
		{
			if (!this.Player.Owner.IsLocalClient)
			{
				return;
			}
			float num = this._lightCombatTrack.VolumeMultiplier;
			if (this.TimeSinceSighted > this.outOfSightTimeToDipMusic)
			{
				num -= this.musicChangeRate_Down * Time.deltaTime;
			}
			else
			{
				num += this.musicChangeRate_Up * Time.deltaTime;
			}
			num = Mathf.Clamp(num, this.minMusicVolume, 1f);
			this._lightCombatTrack.VolumeMultiplier = num;
			this._heavyCombatTrack.VolumeMultiplier = num;
		}

		// Token: 0x06002887 RID: 10375 RVA: 0x000A6444 File Offset: 0x000A4644
		private void TimeoutPursuit()
		{
			switch (this.CurrentPursuitLevel)
			{
			case PlayerCrimeData.EPursuitLevel.Arresting:
				NetworkSingleton<LevelManager>.Instance.AddXP(20);
				break;
			case PlayerCrimeData.EPursuitLevel.NonLethal:
				NetworkSingleton<LevelManager>.Instance.AddXP(40);
				break;
			case PlayerCrimeData.EPursuitLevel.Lethal:
				NetworkSingleton<LevelManager>.Instance.AddXP(60);
				break;
			}
			this.onPursuitEscapedSound.Play();
			this.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.None);
			this.ClearCrimes();
		}

		// Token: 0x06002888 RID: 10376 RVA: 0x000A64B4 File Offset: 0x000A46B4
		public float GetSearchTime()
		{
			switch (this.CurrentPursuitLevel)
			{
			case PlayerCrimeData.EPursuitLevel.Investigating:
				return 60f;
			case PlayerCrimeData.EPursuitLevel.Arresting:
				return 25f;
			case PlayerCrimeData.EPursuitLevel.NonLethal:
				return 30f;
			case PlayerCrimeData.EPursuitLevel.Lethal:
				return 40f;
			default:
				return 0f;
			}
		}

		// Token: 0x06002889 RID: 10377 RVA: 0x000A64FF File Offset: 0x000A46FF
		public void ResetShotAccuracy()
		{
			this.timeSinceLastShot = 0f;
		}

		// Token: 0x0600288A RID: 10378 RVA: 0x000A650C File Offset: 0x000A470C
		public float GetShotAccuracyMultiplier()
		{
			float num = 1f;
			if (this.timeSinceLastShot < 2f)
			{
				num = 0f;
			}
			if (this.timeSinceLastShot < 8f)
			{
				num = 1f - (this.timeSinceLastShot - 2f) / 6f;
			}
			float t = Mathf.Clamp01(Mathf.InverseLerp(0f, PlayerMovement.WalkSpeed * PlayerMovement.SprintMultiplier, this.Player.VelocityCalculator.Velocity.magnitude));
			float num2 = Mathf.Lerp(2f, 0.5f, t);
			int num3 = 0;
			for (int i = 0; i < PoliceOfficer.Officers.Count; i++)
			{
				if (PoliceOfficer.Officers[i].PursuitBehaviour.Active && PoliceOfficer.Officers[i].TargetPlayerNOB == this.Player.NetworkObject && Vector3.Distance(PoliceOfficer.Officers[i].transform.position, this.Player.Avatar.CenterPoint) < 20f)
				{
					num3++;
				}
			}
			float num4 = Mathf.Lerp(1f, 0.6f, Mathf.Clamp01((float)num3 / 3f));
			return num * num2 * num4;
		}

		// Token: 0x0600288B RID: 10379 RVA: 0x000A664C File Offset: 0x000A484C
		public void RecordVehicleCollision(NPC victim)
		{
			PlayerCrimeData.VehicleCollisionInstance item = new PlayerCrimeData.VehicleCollisionInstance(victim, 0f);
			this.Collisions.Add(item);
		}

		// Token: 0x0600288C RID: 10380 RVA: 0x000A6671 File Offset: 0x000A4871
		private void CheckNearestOfficer()
		{
			if (this.Player == null)
			{
				return;
			}
			this.NearestOfficer = (from x in PoliceOfficer.Officers
			orderby Vector3.Distance(x.Avatar.CenterPoint, this.Player.Avatar.CenterPoint)
			select x).FirstOrDefault<PoliceOfficer>();
		}

		// Token: 0x0600288D RID: 10381 RVA: 0x000A66A4 File Offset: 0x000A48A4
		public PlayerCrimeData()
		{
			this.<LastKnownPosition>k__BackingField = Vector3.zero;
			this.Pursuers = new List<PoliceOfficer>();
			this.TimeSinceSighted = 100000f;
			this.Crimes = new Dictionary<Crime, int>();
			this.TimeSinceLastBodySearch = 100000f;
			this.timeSinceLastShot = 1000f;
			this.Collisions = new List<PlayerCrimeData.VehicleCollisionInstance>();
			this.outOfSightTimeToDipMusic = 8f;
			this.minMusicVolume = 0.6f;
			this.musicChangeRate_Down = 0.04f;
			this.musicChangeRate_Up = 2f;
			base..ctor();
		}

		// Token: 0x06002890 RID: 10384 RVA: 0x000A675C File Offset: 0x000A495C
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.PlayerScripts.PlayerCrimeDataAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.PlayerScripts.PlayerCrimeDataAssembly-CSharp.dll_Excuted = true;
			this.syncVar___<LastKnownPosition>k__BackingField = new SyncVar<Vector3>(this, 1U, 1, 0, 0.5f, 0, this.<LastKnownPosition>k__BackingField);
			this.syncVar___<CurrentPursuitLevel>k__BackingField = new SyncVar<PlayerCrimeData.EPursuitLevel>(this, 0U, 1, 0, 0.5f, 0, this.<CurrentPursuitLevel>k__BackingField);
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_set_CurrentPursuitLevel_2979171596));
			base.RegisterServerRpc(1U, new ServerRpcDelegate(this.RpcReader___Server_set_LastKnownPosition_4276783012));
			base.RegisterObserversRpc(2U, new ClientRpcDelegate(this.RpcReader___Observers_RecordLastKnownPosition_1140765316));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.PlayerScripts.PlayerCrimeData));
		}

		// Token: 0x06002891 RID: 10385 RVA: 0x000A6827 File Offset: 0x000A4A27
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.PlayerScripts.PlayerCrimeDataAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.PlayerScripts.PlayerCrimeDataAssembly-CSharp.dll_Excuted = true;
			this.syncVar___<LastKnownPosition>k__BackingField.SetRegistered();
			this.syncVar___<CurrentPursuitLevel>k__BackingField.SetRegistered();
		}

		// Token: 0x06002892 RID: 10386 RVA: 0x000A6850 File Offset: 0x000A4A50
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002893 RID: 10387 RVA: 0x000A6860 File Offset: 0x000A4A60
		private void RpcWriter___Server_set_CurrentPursuitLevel_2979171596(PlayerCrimeData.EPursuitLevel value)
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.Write___ScheduleOne.PlayerScripts.PlayerCrimeData/EPursuitLevelFishNet.Serializing.Generated(value);
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002894 RID: 10388 RVA: 0x000A6961 File Offset: 0x000A4B61
		protected void RpcLogic___set_CurrentPursuitLevel_2979171596(PlayerCrimeData.EPursuitLevel value)
		{
			this.sync___set_value_<CurrentPursuitLevel>k__BackingField(value, true);
		}

		// Token: 0x06002895 RID: 10389 RVA: 0x000A696C File Offset: 0x000A4B6C
		private void RpcReader___Server_set_CurrentPursuitLevel_2979171596(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			PlayerCrimeData.EPursuitLevel value = GeneratedReaders___Internal.Read___ScheduleOne.PlayerScripts.PlayerCrimeData/EPursuitLevelFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___set_CurrentPursuitLevel_2979171596(value);
		}

		// Token: 0x06002896 RID: 10390 RVA: 0x000A69BC File Offset: 0x000A4BBC
		private void RpcWriter___Server_set_LastKnownPosition_4276783012(Vector3 value)
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteVector3(value);
			base.SendServerRpc(1U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002897 RID: 10391 RVA: 0x000A6ABD File Offset: 0x000A4CBD
		protected void RpcLogic___set_LastKnownPosition_4276783012(Vector3 value)
		{
			this.sync___set_value_<LastKnownPosition>k__BackingField(value, true);
		}

		// Token: 0x06002898 RID: 10392 RVA: 0x000A6AC8 File Offset: 0x000A4CC8
		private void RpcReader___Server_set_LastKnownPosition_4276783012(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			Vector3 value = PooledReader0.ReadVector3();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___set_LastKnownPosition_4276783012(value);
		}

		// Token: 0x06002899 RID: 10393 RVA: 0x000A6B18 File Offset: 0x000A4D18
		private void RpcWriter___Observers_RecordLastKnownPosition_1140765316(bool resetTimeSinceSighted)
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
			writer.WriteBoolean(resetTimeSinceSighted);
			base.SendObserversRpc(2U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600289A RID: 10394 RVA: 0x000A6BCE File Offset: 0x000A4DCE
		public void RpcLogic___RecordLastKnownPosition_1140765316(bool resetTimeSinceSighted)
		{
			this.LastKnownPosition = this.Player.Avatar.CenterPoint;
			if (resetTimeSinceSighted)
			{
				this.TimeSinceSighted = 0f;
			}
		}

		// Token: 0x0600289B RID: 10395 RVA: 0x000A6BF4 File Offset: 0x000A4DF4
		private void RpcReader___Observers_RecordLastKnownPosition_1140765316(PooledReader PooledReader0, Channel channel)
		{
			bool resetTimeSinceSighted = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___RecordLastKnownPosition_1140765316(resetTimeSinceSighted);
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x0600289C RID: 10396 RVA: 0x000A6C2F File Offset: 0x000A4E2F
		// (set) Token: 0x0600289D RID: 10397 RVA: 0x000A6C37 File Offset: 0x000A4E37
		public PlayerCrimeData.EPursuitLevel SyncAccessor_<CurrentPursuitLevel>k__BackingField
		{
			get
			{
				return this.<CurrentPursuitLevel>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<CurrentPursuitLevel>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<CurrentPursuitLevel>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x0600289E RID: 10398 RVA: 0x000A6C74 File Offset: 0x000A4E74
		public override bool PlayerCrimeData(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 == 1U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<LastKnownPosition>k__BackingField(this.syncVar___<LastKnownPosition>k__BackingField.GetValue(true), true);
					return true;
				}
				Vector3 value = PooledReader0.ReadVector3();
				this.sync___set_value_<LastKnownPosition>k__BackingField(value, Boolean2);
				return true;
			}
			else
			{
				if (UInt321 != 0U)
				{
					return false;
				}
				if (PooledReader0 == null)
				{
					this.sync___set_value_<CurrentPursuitLevel>k__BackingField(this.syncVar___<CurrentPursuitLevel>k__BackingField.GetValue(true), true);
					return true;
				}
				PlayerCrimeData.EPursuitLevel value2 = GeneratedReaders___Internal.Read___ScheduleOne.PlayerScripts.PlayerCrimeData/EPursuitLevelFishNet.Serializing.Generateds(PooledReader0);
				this.sync___set_value_<CurrentPursuitLevel>k__BackingField(value2, Boolean2);
				return true;
			}
		}

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x0600289F RID: 10399 RVA: 0x000A6D0A File Offset: 0x000A4F0A
		// (set) Token: 0x060028A0 RID: 10400 RVA: 0x000A6D12 File Offset: 0x000A4F12
		public Vector3 SyncAccessor_<LastKnownPosition>k__BackingField
		{
			get
			{
				return this.<LastKnownPosition>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<LastKnownPosition>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<LastKnownPosition>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x060028A1 RID: 10401 RVA: 0x000A6D50 File Offset: 0x000A4F50
		private void dll()
		{
			this.Player.Health.onDie.AddListener(new UnityAction(this.OnDie));
			this.Player.onFreed.AddListener(new UnityAction(this.ClearCrimes));
			this.Player.onFreed.AddListener(delegate()
			{
				this.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.None);
			});
			base.InvokeRepeating("CheckNearestOfficer", 0f, 0.2f);
			this._lightCombatTrack = Singleton<MusicPlayer>.Instance.Tracks.Find((MusicTrack t) => t.TrackName == "Light Combat");
			this._heavyCombatTrack = Singleton<MusicPlayer>.Instance.Tracks.Find((MusicTrack t) => t.TrackName == "Heavy Combat");
		}

		// Token: 0x04001D11 RID: 7441
		public const float SEARCH_TIME_INVESTIGATING = 60f;

		// Token: 0x04001D12 RID: 7442
		public const float SEARCH_TIME_ARRESTING = 25f;

		// Token: 0x04001D13 RID: 7443
		public const float SEARCH_TIME_NONLETHAL = 30f;

		// Token: 0x04001D14 RID: 7444
		public const float SEARCH_TIME_LETHAL = 40f;

		// Token: 0x04001D15 RID: 7445
		public const float ESCALATION_TIME_ARRESTING = 25f;

		// Token: 0x04001D16 RID: 7446
		public const float ESCALATION_TIME_NONLETHAL = 120f;

		// Token: 0x04001D17 RID: 7447
		public const float SHOT_COOLDOWN_MIN = 2f;

		// Token: 0x04001D18 RID: 7448
		public const float SHOT_COOLDOWN_MAX = 8f;

		// Token: 0x04001D19 RID: 7449
		public const float VEHICLE_COLLISION_LIFETIME = 30f;

		// Token: 0x04001D1A RID: 7450
		public const float VEHICLE_COLLISION_LIMIT = 3f;

		// Token: 0x04001D1B RID: 7451
		public PoliceOfficer NearestOfficer;

		// Token: 0x04001D1C RID: 7452
		public Player Player;

		// Token: 0x04001D1D RID: 7453
		public AudioSourceController onPursuitEscapedSound;

		// Token: 0x04001D20 RID: 7456
		public List<PoliceOfficer> Pursuers;

		// Token: 0x04001D23 RID: 7459
		public float TimeSincePursuitStart;

		// Token: 0x04001D24 RID: 7460
		public float CurrentPursuitLevelDuration;

		// Token: 0x04001D25 RID: 7461
		public float TimeSinceSighted;

		// Token: 0x04001D26 RID: 7462
		public Dictionary<Crime, int> Crimes;

		// Token: 0x04001D27 RID: 7463
		public bool BodySearchPending;

		// Token: 0x04001D2A RID: 7466
		public float timeSinceLastShot;

		// Token: 0x04001D2B RID: 7467
		protected List<PlayerCrimeData.VehicleCollisionInstance> Collisions;

		// Token: 0x04001D2C RID: 7468
		private MusicTrack _lightCombatTrack;

		// Token: 0x04001D2D RID: 7469
		private MusicTrack _heavyCombatTrack;

		// Token: 0x04001D2E RID: 7470
		private float outOfSightTimeToDipMusic;

		// Token: 0x04001D2F RID: 7471
		private float minMusicVolume;

		// Token: 0x04001D30 RID: 7472
		private float musicChangeRate_Down;

		// Token: 0x04001D31 RID: 7473
		private float musicChangeRate_Up;

		// Token: 0x04001D32 RID: 7474
		public SyncVar<PlayerCrimeData.EPursuitLevel> syncVar___<CurrentPursuitLevel>k__BackingField;

		// Token: 0x04001D33 RID: 7475
		public SyncVar<Vector3> syncVar___<LastKnownPosition>k__BackingField;

		// Token: 0x04001D34 RID: 7476
		private bool dll_Excuted;

		// Token: 0x04001D35 RID: 7477
		private bool dll_Excuted;

		// Token: 0x0200062A RID: 1578
		public class VehicleCollisionInstance
		{
			// Token: 0x060028A2 RID: 10402 RVA: 0x000A6E33 File Offset: 0x000A5033
			public VehicleCollisionInstance(NPC victim, float timeSince)
			{
				this.Victim = victim;
				this.TimeSince = timeSince;
			}

			// Token: 0x04001D36 RID: 7478
			public NPC Victim;

			// Token: 0x04001D37 RID: 7479
			public float TimeSince;
		}

		// Token: 0x0200062B RID: 1579
		public enum EPursuitLevel
		{
			// Token: 0x04001D39 RID: 7481
			None,
			// Token: 0x04001D3A RID: 7482
			Investigating,
			// Token: 0x04001D3B RID: 7483
			Arresting,
			// Token: 0x04001D3C RID: 7484
			NonLethal,
			// Token: 0x04001D3D RID: 7485
			Lethal
		}
	}
}
