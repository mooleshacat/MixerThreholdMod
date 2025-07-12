using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Object.Synchronizing;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dialogue;
using ScheduleOne.GameTime;
using ScheduleOne.Management;
using ScheduleOne.NPCs;
using ScheduleOne.NPCs.Behaviour;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property;
using ScheduleOne.UI;
using ScheduleOne.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Employees
{
	// Token: 0x02000682 RID: 1666
	public class Employee : NPC
	{
		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x06002C47 RID: 11335 RVA: 0x000B68D2 File Offset: 0x000B4AD2
		// (set) Token: 0x06002C48 RID: 11336 RVA: 0x000B68DA File Offset: 0x000B4ADA
		public Property AssignedProperty { get; protected set; }

		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x06002C49 RID: 11337 RVA: 0x000B68E3 File Offset: 0x000B4AE3
		// (set) Token: 0x06002C4A RID: 11338 RVA: 0x000B68EB File Offset: 0x000B4AEB
		public int EmployeeIndex { get; protected set; }

		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x06002C4B RID: 11339 RVA: 0x000B68F4 File Offset: 0x000B4AF4
		// (set) Token: 0x06002C4C RID: 11340 RVA: 0x000B68FC File Offset: 0x000B4AFC
		public bool PaidForToday
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<PaidForToday>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.sync___set_value_<PaidForToday>k__BackingField(value, true);
			}
		}

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x06002C4D RID: 11341 RVA: 0x000B6906 File Offset: 0x000B4B06
		// (set) Token: 0x06002C4E RID: 11342 RVA: 0x000B690E File Offset: 0x000B4B0E
		public bool Fired { get; private set; }

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x06002C4F RID: 11343 RVA: 0x000B6917 File Offset: 0x000B4B17
		public bool IsWaitingOutside
		{
			get
			{
				return this.WaitOutside.Active;
			}
		}

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x06002C50 RID: 11344 RVA: 0x000B6924 File Offset: 0x000B4B24
		// (set) Token: 0x06002C51 RID: 11345 RVA: 0x000B692C File Offset: 0x000B4B2C
		public bool IsMale { get; private set; } = true;

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x06002C52 RID: 11346 RVA: 0x000B6935 File Offset: 0x000B4B35
		// (set) Token: 0x06002C53 RID: 11347 RVA: 0x000B693D File Offset: 0x000B4B3D
		private protected int AppearanceIndex { protected get; private set; }

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x06002C54 RID: 11348 RVA: 0x000B6946 File Offset: 0x000B4B46
		public EEmployeeType EmployeeType
		{
			get
			{
				return this.Type;
			}
		}

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x06002C55 RID: 11349 RVA: 0x000B694E File Offset: 0x000B4B4E
		// (set) Token: 0x06002C56 RID: 11350 RVA: 0x000B6956 File Offset: 0x000B4B56
		public int TimeSinceLastWorked { get; private set; }

		// Token: 0x06002C57 RID: 11351 RVA: 0x000B6960 File Offset: 0x000B4B60
		protected override void Start()
		{
			base.Start();
			DialogueController.DialogueChoice dialogueChoice = new DialogueController.DialogueChoice();
			dialogueChoice.ChoiceText = "I need to trade some items";
			dialogueChoice.Enabled = true;
			dialogueChoice.onChoosen.AddListener(new UnityAction(this.TradeItems));
			this.dialogueHandler.GetComponent<DialogueController>().AddDialogueChoice(dialogueChoice, 3);
			DialogueController.DialogueChoice dialogueChoice2 = new DialogueController.DialogueChoice();
			dialogueChoice2.ChoiceText = "Why aren't you working?";
			dialogueChoice2.Enabled = true;
			dialogueChoice2.shouldShowCheck = new DialogueController.DialogueChoice.ShouldShowCheck(this.ShouldShowNoWorkDialogue);
			dialogueChoice2.onChoosen.AddListener(new UnityAction(this.OnNotWorkingDialogue));
			this.dialogueHandler.GetComponent<DialogueController>().AddDialogueChoice(dialogueChoice2, 2);
			DialogueController.DialogueChoice dialogueChoice3 = new DialogueController.DialogueChoice();
			dialogueChoice3.ChoiceText = "I need to transfer you to another property";
			dialogueChoice3.Enabled = true;
			dialogueChoice3.Conversation = this.TransferDialogue;
			this.dialogueHandler.GetComponent<DialogueController>().AddDialogueChoice(dialogueChoice3, 1);
			DialogueController.DialogueChoice dialogueChoice4 = new DialogueController.DialogueChoice();
			dialogueChoice4.ChoiceText = "Your services are no longer required.";
			dialogueChoice4.Enabled = true;
			dialogueChoice4.shouldShowCheck = new DialogueController.DialogueChoice.ShouldShowCheck(this.ShouldShowFireDialogue);
			dialogueChoice4.Conversation = this.FireDialogue;
			this.dialogueHandler.GetComponent<DialogueController>().AddDialogueChoice(dialogueChoice4, -1);
			this.dialogueHandler.onDialogueChoiceChosen.AddListener(new UnityAction<string>(this.CheckDialogueChoice));
		}

		// Token: 0x06002C58 RID: 11352 RVA: 0x000B6AA6 File Offset: 0x000B4CA6
		public override void OnStartServer()
		{
			base.OnStartServer();
			this.Health.onDie.AddListener(new UnityAction(this.SendFire));
		}

		// Token: 0x06002C59 RID: 11353 RVA: 0x000B6ACC File Offset: 0x000B4CCC
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (connection.IsLocalClient)
			{
				return;
			}
			this.Initialize(connection, this.FirstName, this.LastName, this.ID, base.GUID.ToString(), this.AssignedProperty.PropertyCode, this.IsMale, this.AppearanceIndex);
		}

		// Token: 0x06002C5A RID: 11354 RVA: 0x000B6B30 File Offset: 0x000B4D30
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public virtual void Initialize(NetworkConnection conn, string firstName, string lastName, string id, string guid, string propertyID, bool male, int appearanceIndex)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_Initialize_2260823878(conn, firstName, lastName, id, guid, propertyID, male, appearanceIndex);
				this.RpcLogic___Initialize_2260823878(conn, firstName, lastName, id, guid, propertyID, male, appearanceIndex);
			}
			else
			{
				this.RpcWriter___Target_Initialize_2260823878(conn, firstName, lastName, id, guid, propertyID, male, appearanceIndex);
			}
		}

		// Token: 0x06002C5B RID: 11355 RVA: 0x000B6BBC File Offset: 0x000B4DBC
		protected virtual void AssignProperty(Property prop, bool warp)
		{
			this.AssignedProperty = prop;
			this.EmployeeIndex = this.AssignedProperty.RegisterEmployee(this);
			if (warp)
			{
				this.movement.Warp(prop.NPCSpawnPoint.position);
			}
			this.WaitOutside.IdlePoint = prop.EmployeeIdlePoints[this.EmployeeIndex];
			this.cachedNPCSpawnPoint = prop.NPCSpawnPoint;
		}

		// Token: 0x06002C5C RID: 11356 RVA: 0x000B6C1F File Offset: 0x000B4E1F
		protected virtual void UnassignProperty()
		{
			if (this.AssignedProperty == null)
			{
				return;
			}
			this.ResetConfiguration();
			this.AssignedProperty.DeregisterEmployee(this);
			this.AssignedProperty = null;
		}

		// Token: 0x06002C5D RID: 11357 RVA: 0x000B6C49 File Offset: 0x000B4E49
		[ServerRpc(RequireOwnership = false)]
		public void SendTransfer(string propertyCode)
		{
			this.RpcWriter___Server_SendTransfer_3615296227(propertyCode);
		}

		// Token: 0x06002C5E RID: 11358 RVA: 0x000B6C58 File Offset: 0x000B4E58
		[ObserversRpc]
		private void TransferToProperty(string code)
		{
			this.RpcWriter___Observers_TransferToProperty_3615296227(code);
		}

		// Token: 0x06002C5F RID: 11359 RVA: 0x000B6C70 File Offset: 0x000B4E70
		protected virtual void TransferToProperty(Property prop)
		{
			if (this.AssignedProperty == prop)
			{
				return;
			}
			Console.Log(string.Concat(new string[]
			{
				"Transferring employee ",
				this.FirstName,
				" ",
				this.LastName,
				" to property ",
				prop.PropertyName
			}), null);
			this.UnassignProperty();
			this.AssignProperty(prop, false);
		}

		// Token: 0x06002C60 RID: 11360 RVA: 0x000B6CDE File Offset: 0x000B4EDE
		protected virtual void InitializeInfo(string firstName, string lastName, string id)
		{
			this.FirstName = firstName;
			this.LastName = lastName;
			this.ID = id;
			NetworkSingleton<EmployeeManager>.Instance.RegisterName(firstName + " " + lastName);
		}

		// Token: 0x06002C61 RID: 11361 RVA: 0x000B6D0C File Offset: 0x000B4F0C
		protected virtual void InitializeAppearance(bool male, int index)
		{
			this.IsMale = male;
			this.AppearanceIndex = index;
			EmployeeManager.EmployeeAppearance appearance = NetworkSingleton<EmployeeManager>.Instance.GetAppearance(male, index);
			appearance.Settings.BodyLayerSettings.Clear();
			this.Avatar.LoadNakedSettings(appearance.Settings, 100);
			this.MugshotSprite = appearance.Mugshot;
			this.VoiceOverEmitter.SetDatabase(NetworkSingleton<EmployeeManager>.Instance.GetVoice(male, index), true);
			int num = (this.FirstName + this.LastName).GetHashCode() / 1000;
			this.VoiceOverEmitter.PitchMultiplier = 0.9f + (float)(num % 10) / 10f * 0.2f;
			NetworkSingleton<EmployeeManager>.Instance.RegisterAppearance(male, index);
			float num2 = male ? 0.8f : 1.3f;
			float num3 = 0.2f;
			float num4 = -num3 / 2f + Mathf.Clamp01((float)(this.FirstName.GetHashCode() % 10) / 10f) * num3;
			num2 += num4;
			this.VoiceOverEmitter.PitchMultiplier = num2;
		}

		// Token: 0x06002C62 RID: 11362 RVA: 0x000B6E14 File Offset: 0x000B5014
		protected virtual void CheckDialogueChoice(string choiceLabel)
		{
			if (choiceLabel == "CONFIRM_FIRE")
			{
				this.SendFire();
			}
		}

		// Token: 0x06002C63 RID: 11363 RVA: 0x000B6E29 File Offset: 0x000B5029
		[ServerRpc(RequireOwnership = false)]
		public void SendFire()
		{
			this.RpcWriter___Server_SendFire_2166136261();
		}

		// Token: 0x06002C64 RID: 11364 RVA: 0x000B6E31 File Offset: 0x000B5031
		[ObserversRpc]
		private void ReceiveFire()
		{
			this.RpcWriter___Observers_ReceiveFire_2166136261();
		}

		// Token: 0x06002C65 RID: 11365 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void ResetConfiguration()
		{
		}

		// Token: 0x06002C66 RID: 11366 RVA: 0x000B6E3C File Offset: 0x000B503C
		protected virtual void Fire()
		{
			Console.Log("Firing employee " + this.FirstName + " " + this.LastName, null);
			this.ResetConfiguration();
			this.UnassignProperty();
			this.Avatar.EmotionManager.AddEmotionOverride("Concerned", "fired", 0f, 0);
			this.SetWaitOutside(false);
			this.Fired = true;
		}

		// Token: 0x06002C67 RID: 11367 RVA: 0x000B6EA4 File Offset: 0x000B50A4
		protected bool CanWork()
		{
			return this.GetHome() != null && this.PaidForToday && !NetworkSingleton<TimeManager>.Instance.IsEndOfDay;
		}

		// Token: 0x06002C68 RID: 11368 RVA: 0x000B6ECC File Offset: 0x000B50CC
		protected new virtual void OnDestroy()
		{
			if (InstanceFinder.IsServer)
			{
				TimeManager.onSleepEnd = (Action<int>)Delegate.Remove(TimeManager.onSleepEnd, new Action<int>(this.OnSleepEnd));
			}
			if (NetworkSingleton<EmployeeManager>.InstanceExists)
			{
				NetworkSingleton<EmployeeManager>.Instance.AllEmployees.Remove(this);
			}
		}

		// Token: 0x06002C69 RID: 11369 RVA: 0x000B6F18 File Offset: 0x000B5118
		protected virtual void UpdateBehaviour()
		{
			if (this.Fired)
			{
				return;
			}
			if (this.behaviour.activeBehaviour == null || this.behaviour.activeBehaviour == this.WaitOutside)
			{
				bool flag = false;
				bool flag2 = false;
				if (this.GetHome() == null)
				{
					flag = true;
					this.SubmitNoWorkReason("I haven't been assigned a locker", "You can use your management clipboard to assign me a locker.", 0);
				}
				else if (NetworkSingleton<TimeManager>.Instance.IsEndOfDay)
				{
					flag = true;
					this.SubmitNoWorkReason("Sorry boss, my shift ends at 4AM.", string.Empty, 0);
				}
				else if (!this.PaidForToday)
				{
					if (this.IsPayAvailable())
					{
						flag2 = true;
					}
					else
					{
						flag = true;
						this.SubmitNoWorkReason("I haven't been paid yet", "You can place cash in my locker.", 0);
					}
				}
				if (flag)
				{
					this.SetWaitOutside(true);
					return;
				}
				if (InstanceFinder.IsServer && flag2 && this.IsPayAvailable())
				{
					this.RemoveDailyWage();
					this.SetIsPaid();
				}
			}
		}

		// Token: 0x06002C6A RID: 11370 RVA: 0x000B6FF3 File Offset: 0x000B51F3
		protected void MarkIsWorking()
		{
			this.TimeSinceLastWorked = 0;
		}

		// Token: 0x06002C6B RID: 11371 RVA: 0x000B6FFC File Offset: 0x000B51FC
		private void SetWaitOutside(bool wait)
		{
			if (wait)
			{
				if (!this.WaitOutside.Enabled)
				{
					this.WaitOutside.Enable_Networked(null);
					return;
				}
			}
			else if (this.WaitOutside.Enabled || this.WaitOutside.Active)
			{
				this.WaitOutside.Disable_Networked(null);
				this.WaitOutside.End_Networked(null);
			}
		}

		// Token: 0x06002C6C RID: 11372 RVA: 0x00014B5A File Offset: 0x00012D5A
		protected virtual bool ShouldIdle()
		{
			return false;
		}

		// Token: 0x06002C6D RID: 11373 RVA: 0x00014B5A File Offset: 0x00012D5A
		protected override bool ShouldNoticeGeneralCrime(Player player)
		{
			return false;
		}

		// Token: 0x06002C6E RID: 11374 RVA: 0x000B7058 File Offset: 0x000B5258
		protected override void MinPass()
		{
			base.MinPass();
			int timeSinceLastWorked = this.TimeSinceLastWorked;
			this.TimeSinceLastWorked = timeSinceLastWorked + 1;
			this.WorkIssues.Clear();
			this.UpdateBehaviour();
		}

		// Token: 0x06002C6F RID: 11375 RVA: 0x000B708C File Offset: 0x000B528C
		private void OnSleepEnd(int sleepTime)
		{
			this.PaidForToday = false;
		}

		// Token: 0x06002C70 RID: 11376 RVA: 0x000B7095 File Offset: 0x000B5295
		public void SetIsPaid()
		{
			this.PaidForToday = true;
		}

		// Token: 0x06002C71 RID: 11377 RVA: 0x00014B5A File Offset: 0x00012D5A
		public override bool ShouldSave()
		{
			return false;
		}

		// Token: 0x06002C72 RID: 11378 RVA: 0x000B70A0 File Offset: 0x000B52A0
		public override NPCData GetNPCData()
		{
			return new EmployeeData(this.ID, this.AssignedProperty.PropertyCode, this.FirstName, this.LastName, this.IsMale, this.AppearanceIndex, base.transform.position, base.transform.rotation, base.GUID, this.PaidForToday);
		}

		// Token: 0x06002C73 RID: 11379 RVA: 0x000B70FD File Offset: 0x000B52FD
		public virtual EmployeeHome GetHome()
		{
			Console.LogError("GETBED NOT IMPLEMENTED", null);
			return null;
		}

		// Token: 0x06002C74 RID: 11380 RVA: 0x000B710C File Offset: 0x000B530C
		public bool IsPayAvailable()
		{
			EmployeeHome home = this.GetHome();
			return !(home == null) && home.GetCashSum() >= this.DailyWage;
		}

		// Token: 0x06002C75 RID: 11381 RVA: 0x000B713C File Offset: 0x000B533C
		public void RemoveDailyWage()
		{
			EmployeeHome home = this.GetHome();
			if (home == null)
			{
				return;
			}
			if (home.GetCashSum() >= this.DailyWage)
			{
				home.RemoveCash(this.DailyWage);
			}
		}

		// Token: 0x06002C76 RID: 11382 RVA: 0x000B7174 File Offset: 0x000B5374
		public virtual bool GetWorkIssue(out DialogueContainer notWorkingReason)
		{
			if (this.GetHome() == null)
			{
				notWorkingReason = this.BedNotAssignedDialogue;
				return true;
			}
			if (!this.PaidForToday)
			{
				notWorkingReason = this.NotPaidDialogue;
				return true;
			}
			if (this.TimeSinceLastWorked >= 5 && this.WorkIssues.Count > 0)
			{
				notWorkingReason = UnityEngine.Object.Instantiate<DialogueContainer>(this.WorkIssueDialogueTemplate);
				notWorkingReason.GetDialogueNodeByLabel("ENTRY").DialogueText = this.WorkIssues[0].Reason;
				if (!string.IsNullOrEmpty(this.WorkIssues[0].Fix))
				{
					notWorkingReason.GetDialogueNodeByLabel("FIX").DialogueText = this.WorkIssues[0].Fix;
				}
				else
				{
					notWorkingReason.GetDialogueNodeByLabel("ENTRY").choices = new DialogueChoiceData[0];
				}
				return true;
			}
			notWorkingReason = null;
			return false;
		}

		// Token: 0x06002C77 RID: 11383 RVA: 0x000B7250 File Offset: 0x000B5450
		public virtual void SetIdle(bool idle)
		{
			this.SetWaitOutside(idle);
		}

		// Token: 0x06002C78 RID: 11384 RVA: 0x000B725C File Offset: 0x000B545C
		protected void LeavePropertyAndDespawn()
		{
			if (this.movement.IsMoving)
			{
				return;
			}
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.movement.IsAsCloseAsPossible(this.cachedNPCSpawnPoint.position, 1f))
			{
				base.Despawn(base.NetworkObject, null);
				return;
			}
			this.SetDestination(this.cachedNPCSpawnPoint.position, true);
		}

		// Token: 0x06002C79 RID: 11385 RVA: 0x000B72C4 File Offset: 0x000B54C4
		[ObserversRpc(RunLocally = true)]
		public void SubmitNoWorkReason(string reason, string fix, int priority = 0)
		{
			this.RpcWriter___Observers_SubmitNoWorkReason_15643032(reason, fix, priority);
			this.RpcLogic___SubmitNoWorkReason_15643032(reason, fix, priority);
		}

		// Token: 0x06002C7A RID: 11386 RVA: 0x000B72F8 File Offset: 0x000B54F8
		private bool ShouldShowNoWorkDialogue(bool enabled)
		{
			DialogueContainer dialogueContainer;
			return !this.Fired && this.WaitOutside.Active && this.GetWorkIssue(out dialogueContainer);
		}

		// Token: 0x06002C7B RID: 11387 RVA: 0x000B7328 File Offset: 0x000B5528
		private void OnNotWorkingDialogue()
		{
			DialogueContainer container;
			if (!this.GetWorkIssue(out container))
			{
				return;
			}
			this.dialogueHandler.InitializeDialogue(container);
		}

		// Token: 0x06002C7C RID: 11388 RVA: 0x000B734C File Offset: 0x000B554C
		private bool ShouldShowFireDialogue(bool enabled)
		{
			return !this.Fired;
		}

		// Token: 0x06002C7D RID: 11389 RVA: 0x000B735C File Offset: 0x000B555C
		private void TradeItems()
		{
			this.dialogueHandler.SkipNextDialogueBehaviourEnd();
			Singleton<StorageMenu>.Instance.Open(base.Inventory, base.fullName + "'s Inventory", string.Empty);
			Singleton<StorageMenu>.Instance.onClosed.AddListener(new UnityAction(this.TradeItemsDone));
		}

		// Token: 0x06002C7E RID: 11390 RVA: 0x000B73B4 File Offset: 0x000B55B4
		private void TradeItemsDone()
		{
			Singleton<StorageMenu>.Instance.onClosed.RemoveListener(new UnityAction(this.TradeItemsDone));
			this.behaviour.GenericDialogueBehaviour.SendDisable();
		}

		// Token: 0x06002C7F RID: 11391 RVA: 0x000B73E1 File Offset: 0x000B55E1
		protected void SetDestination(ITransitEntity transitEntity, bool teleportIfFail = true)
		{
			this.SetDestination(NavMeshUtility.GetAccessPoint(transitEntity, this).position, teleportIfFail);
		}

		// Token: 0x06002C80 RID: 11392 RVA: 0x000B73F8 File Offset: 0x000B55F8
		protected void SetDestination(Vector3 position, bool teleportIfFail = true)
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (teleportIfFail && this.consecutivePathingFailures >= 5 && !base.Movement.CanGetTo(position, 1f))
			{
				Console.LogWarning(base.fullName + " too many pathing failures. Warping to " + position.ToString(), null);
				base.Movement.Warp(position);
				this.WalkCallback(NPCMovement.WalkResult.Success);
			}
			base.Movement.SetDestination(position, new Action<NPCMovement.WalkResult>(this.WalkCallback), 1f, 0.1f);
		}

		// Token: 0x06002C81 RID: 11393 RVA: 0x000B7485 File Offset: 0x000B5685
		protected virtual void WalkCallback(NPCMovement.WalkResult result)
		{
			if (result == NPCMovement.WalkResult.Failed)
			{
				if (Time.timeSinceLevelLoad - this.timeOnLastPathingFailure > 0.2f)
				{
					this.timeOnLastPathingFailure = Time.timeSinceLevelLoad;
					this.consecutivePathingFailures++;
					return;
				}
			}
			else
			{
				this.consecutivePathingFailures = 0;
			}
		}

		// Token: 0x06002C83 RID: 11395 RVA: 0x000B74F0 File Offset: 0x000B56F0
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Employees.EmployeeAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Employees.EmployeeAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___<PaidForToday>k__BackingField = new SyncVar<bool>(this, 1U, 0, 0, -1f, 0, this.<PaidForToday>k__BackingField);
			base.RegisterObserversRpc(35U, new ClientRpcDelegate(this.RpcReader___Observers_Initialize_2260823878));
			base.RegisterTargetRpc(36U, new ClientRpcDelegate(this.RpcReader___Target_Initialize_2260823878));
			base.RegisterServerRpc(37U, new ServerRpcDelegate(this.RpcReader___Server_SendTransfer_3615296227));
			base.RegisterObserversRpc(38U, new ClientRpcDelegate(this.RpcReader___Observers_TransferToProperty_3615296227));
			base.RegisterServerRpc(39U, new ServerRpcDelegate(this.RpcReader___Server_SendFire_2166136261));
			base.RegisterObserversRpc(40U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveFire_2166136261));
			base.RegisterObserversRpc(41U, new ClientRpcDelegate(this.RpcReader___Observers_SubmitNoWorkReason_15643032));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.Employees.Employee));
		}

		// Token: 0x06002C84 RID: 11396 RVA: 0x000B75F2 File Offset: 0x000B57F2
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Employees.EmployeeAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Employees.EmployeeAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___<PaidForToday>k__BackingField.SetRegistered();
		}

		// Token: 0x06002C85 RID: 11397 RVA: 0x000B7616 File Offset: 0x000B5816
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002C86 RID: 11398 RVA: 0x000B7624 File Offset: 0x000B5824
		private void RpcWriter___Observers_Initialize_2260823878(NetworkConnection conn, string firstName, string lastName, string id, string guid, string propertyID, bool male, int appearanceIndex)
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
			writer.WriteString(firstName);
			writer.WriteString(lastName);
			writer.WriteString(id);
			writer.WriteString(guid);
			writer.WriteString(propertyID);
			writer.WriteBoolean(male);
			writer.WriteInt32(appearanceIndex, 1);
			base.SendObserversRpc(35U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002C87 RID: 11399 RVA: 0x000B7730 File Offset: 0x000B5930
		public virtual void RpcLogic___Initialize_2260823878(NetworkConnection conn, string firstName, string lastName, string id, string guid, string propertyID, bool male, int appearanceIndex)
		{
			if (this.initialized)
			{
				return;
			}
			NetworkSingleton<EmployeeManager>.Instance.AllEmployees.Add(this);
			this.initialized = true;
			base.SetGUID(new Guid(guid));
			this.InitializeInfo(firstName, lastName, id);
			this.InitializeAppearance(male, appearanceIndex);
			this.AssignProperty(Singleton<PropertyManager>.Instance.GetProperty(propertyID), InstanceFinder.IsServer);
			this.movement.Agent.avoidancePriority = 10 + appearanceIndex;
			if (InstanceFinder.IsServer)
			{
				if (!NetworkSingleton<VariableDatabase>.Instance.GetValue<bool>("ClipboardAcquired"))
				{
					NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("ClipboardAcquired", true.ToString(), true);
				}
				TimeManager.onSleepEnd = (Action<int>)Delegate.Combine(TimeManager.onSleepEnd, new Action<int>(this.OnSleepEnd));
			}
		}

		// Token: 0x06002C88 RID: 11400 RVA: 0x000B77FC File Offset: 0x000B59FC
		private void RpcReader___Observers_Initialize_2260823878(PooledReader PooledReader0, Channel channel)
		{
			string firstName = PooledReader0.ReadString();
			string lastName = PooledReader0.ReadString();
			string id = PooledReader0.ReadString();
			string guid = PooledReader0.ReadString();
			string propertyID = PooledReader0.ReadString();
			bool male = PooledReader0.ReadBoolean();
			int appearanceIndex = PooledReader0.ReadInt32(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___Initialize_2260823878(null, firstName, lastName, id, guid, propertyID, male, appearanceIndex);
		}

		// Token: 0x06002C89 RID: 11401 RVA: 0x000B78A4 File Offset: 0x000B5AA4
		private void RpcWriter___Target_Initialize_2260823878(NetworkConnection conn, string firstName, string lastName, string id, string guid, string propertyID, bool male, int appearanceIndex)
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
			writer.WriteString(firstName);
			writer.WriteString(lastName);
			writer.WriteString(id);
			writer.WriteString(guid);
			writer.WriteString(propertyID);
			writer.WriteBoolean(male);
			writer.WriteInt32(appearanceIndex, 1);
			base.SendTargetRpc(36U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06002C8A RID: 11402 RVA: 0x000B79AC File Offset: 0x000B5BAC
		private void RpcReader___Target_Initialize_2260823878(PooledReader PooledReader0, Channel channel)
		{
			string firstName = PooledReader0.ReadString();
			string lastName = PooledReader0.ReadString();
			string id = PooledReader0.ReadString();
			string guid = PooledReader0.ReadString();
			string propertyID = PooledReader0.ReadString();
			bool male = PooledReader0.ReadBoolean();
			int appearanceIndex = PooledReader0.ReadInt32(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___Initialize_2260823878(base.LocalConnection, firstName, lastName, id, guid, propertyID, male, appearanceIndex);
		}

		// Token: 0x06002C8B RID: 11403 RVA: 0x000B7A50 File Offset: 0x000B5C50
		private void RpcWriter___Server_SendTransfer_3615296227(string propertyCode)
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
			writer.WriteString(propertyCode);
			base.SendServerRpc(37U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002C8C RID: 11404 RVA: 0x000B7AF7 File Offset: 0x000B5CF7
		public void RpcLogic___SendTransfer_3615296227(string propertyCode)
		{
			this.TransferToProperty(propertyCode);
		}

		// Token: 0x06002C8D RID: 11405 RVA: 0x000B7B00 File Offset: 0x000B5D00
		private void RpcReader___Server_SendTransfer_3615296227(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string propertyCode = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendTransfer_3615296227(propertyCode);
		}

		// Token: 0x06002C8E RID: 11406 RVA: 0x000B7B34 File Offset: 0x000B5D34
		private void RpcWriter___Observers_TransferToProperty_3615296227(string code)
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
			writer.WriteString(code);
			base.SendObserversRpc(38U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002C8F RID: 11407 RVA: 0x000B7BEC File Offset: 0x000B5DEC
		private void RpcLogic___TransferToProperty_3615296227(string code)
		{
			Property property = Singleton<PropertyManager>.Instance.GetProperty(code);
			if (property == null)
			{
				Console.LogError("Property not found: " + code, null);
				return;
			}
			this.TransferToProperty(property);
		}

		// Token: 0x06002C90 RID: 11408 RVA: 0x000B7C28 File Offset: 0x000B5E28
		private void RpcReader___Observers_TransferToProperty_3615296227(PooledReader PooledReader0, Channel channel)
		{
			string code = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___TransferToProperty_3615296227(code);
		}

		// Token: 0x06002C91 RID: 11409 RVA: 0x000B7C5C File Offset: 0x000B5E5C
		private void RpcWriter___Server_SendFire_2166136261()
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
			base.SendServerRpc(39U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002C92 RID: 11410 RVA: 0x000B7CF6 File Offset: 0x000B5EF6
		public void RpcLogic___SendFire_2166136261()
		{
			this.ReceiveFire();
		}

		// Token: 0x06002C93 RID: 11411 RVA: 0x000B7D00 File Offset: 0x000B5F00
		private void RpcReader___Server_SendFire_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendFire_2166136261();
		}

		// Token: 0x06002C94 RID: 11412 RVA: 0x000B7D20 File Offset: 0x000B5F20
		private void RpcWriter___Observers_ReceiveFire_2166136261()
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
			base.SendObserversRpc(40U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002C95 RID: 11413 RVA: 0x000B7DC9 File Offset: 0x000B5FC9
		private void RpcLogic___ReceiveFire_2166136261()
		{
			this.Fire();
		}

		// Token: 0x06002C96 RID: 11414 RVA: 0x000B7DD4 File Offset: 0x000B5FD4
		private void RpcReader___Observers_ReceiveFire_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveFire_2166136261();
		}

		// Token: 0x06002C97 RID: 11415 RVA: 0x000B7DF4 File Offset: 0x000B5FF4
		private void RpcWriter___Observers_SubmitNoWorkReason_15643032(string reason, string fix, int priority = 0)
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
			writer.WriteString(reason);
			writer.WriteString(fix);
			writer.WriteInt32(priority, 1);
			base.SendObserversRpc(41U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002C98 RID: 11416 RVA: 0x000B7ECC File Offset: 0x000B60CC
		public void RpcLogic___SubmitNoWorkReason_15643032(string reason, string fix, int priority = 0)
		{
			Employee.NoWorkReason noWorkReason = new Employee.NoWorkReason(reason, fix, priority);
			for (int i = 0; i < this.WorkIssues.Count; i++)
			{
				if (this.WorkIssues[i].Priority < noWorkReason.Priority)
				{
					this.WorkIssues.Insert(i, noWorkReason);
					return;
				}
			}
			this.WorkIssues.Add(noWorkReason);
		}

		// Token: 0x06002C99 RID: 11417 RVA: 0x000B7F2C File Offset: 0x000B612C
		private void RpcReader___Observers_SubmitNoWorkReason_15643032(PooledReader PooledReader0, Channel channel)
		{
			string reason = PooledReader0.ReadString();
			string fix = PooledReader0.ReadString();
			int priority = PooledReader0.ReadInt32(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SubmitNoWorkReason_15643032(reason, fix, priority);
		}

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x06002C9A RID: 11418 RVA: 0x000B7F8E File Offset: 0x000B618E
		// (set) Token: 0x06002C9B RID: 11419 RVA: 0x000B7F96 File Offset: 0x000B6196
		public bool SyncAccessor_<PaidForToday>k__BackingField
		{
			get
			{
				return this.<PaidForToday>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<PaidForToday>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<PaidForToday>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x06002C9C RID: 11420 RVA: 0x000B7FD4 File Offset: 0x000B61D4
		public override bool Employee(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 != 1U)
			{
				return false;
			}
			if (PooledReader0 == null)
			{
				this.sync___set_value_<PaidForToday>k__BackingField(this.syncVar___<PaidForToday>k__BackingField.GetValue(true), true);
				return true;
			}
			bool value = PooledReader0.ReadBoolean();
			this.sync___set_value_<PaidForToday>k__BackingField(value, Boolean2);
			return true;
		}

		// Token: 0x06002C9D RID: 11421 RVA: 0x000B8026 File Offset: 0x000B6226
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001FC6 RID: 8134
		public const int MAX_CONSECUTIVE_PATHING_FAILURES = 5;

		// Token: 0x04001FC7 RID: 8135
		public bool DEBUG;

		// Token: 0x04001FCE RID: 8142
		[SerializeField]
		protected EEmployeeType Type;

		// Token: 0x04001FCF RID: 8143
		[Header("Payment")]
		public float SigningFee = 500f;

		// Token: 0x04001FD0 RID: 8144
		public float DailyWage = 100f;

		// Token: 0x04001FD1 RID: 8145
		[Header("References")]
		public IdleBehaviour WaitOutside;

		// Token: 0x04001FD2 RID: 8146
		public MoveItemBehaviour MoveItemBehaviour;

		// Token: 0x04001FD3 RID: 8147
		public DialogueContainer BedNotAssignedDialogue;

		// Token: 0x04001FD4 RID: 8148
		public DialogueContainer NotPaidDialogue;

		// Token: 0x04001FD5 RID: 8149
		public DialogueContainer WorkIssueDialogueTemplate;

		// Token: 0x04001FD6 RID: 8150
		public DialogueContainer FireDialogue;

		// Token: 0x04001FD7 RID: 8151
		public DialogueContainer TransferDialogue;

		// Token: 0x04001FD8 RID: 8152
		private List<Employee.NoWorkReason> WorkIssues = new List<Employee.NoWorkReason>();

		// Token: 0x04001FDA RID: 8154
		protected bool initialized;

		// Token: 0x04001FDB RID: 8155
		protected int consecutivePathingFailures;

		// Token: 0x04001FDC RID: 8156
		private float timeOnLastPathingFailure;

		// Token: 0x04001FDD RID: 8157
		private Transform cachedNPCSpawnPoint;

		// Token: 0x04001FDE RID: 8158
		public SyncVar<bool> syncVar___<PaidForToday>k__BackingField;

		// Token: 0x04001FDF RID: 8159
		private bool dll_Excuted;

		// Token: 0x04001FE0 RID: 8160
		private bool dll_Excuted;

		// Token: 0x02000683 RID: 1667
		public class NoWorkReason
		{
			// Token: 0x06002C9E RID: 11422 RVA: 0x000B803A File Offset: 0x000B623A
			public NoWorkReason(string reason, string fix, int priority)
			{
				this.Reason = reason;
				this.Fix = fix;
				this.Priority = priority;
			}

			// Token: 0x04001FE1 RID: 8161
			public string Reason;

			// Token: 0x04001FE2 RID: 8162
			public string Fix;

			// Token: 0x04001FE3 RID: 8163
			public int Priority;
		}
	}
}
