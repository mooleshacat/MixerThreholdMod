using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Object.Synchronizing;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using ScheduleOne.Variables;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Money
{
	// Token: 0x02000BDD RID: 3037
	public class MoneyManager : NetworkSingleton<MoneyManager>, IBaseSaveable, ISaveable
	{
		// Token: 0x17000B00 RID: 2816
		// (get) Token: 0x060050B5 RID: 20661 RVA: 0x00155507 File Offset: 0x00153707
		public float LifetimeEarnings
		{
			get
			{
				return this.SyncAccessor_lifetimeEarnings;
			}
		}

		// Token: 0x17000B01 RID: 2817
		// (get) Token: 0x060050B6 RID: 20662 RVA: 0x0015550F File Offset: 0x0015370F
		// (set) Token: 0x060050B7 RID: 20663 RVA: 0x00155517 File Offset: 0x00153717
		public float LastCalculatedNetworth { get; protected set; }

		// Token: 0x17000B02 RID: 2818
		// (get) Token: 0x060050B8 RID: 20664 RVA: 0x00155520 File Offset: 0x00153720
		public float cashBalance
		{
			get
			{
				return this.cashInstance.Balance;
			}
		}

		// Token: 0x17000B03 RID: 2819
		// (get) Token: 0x060050B9 RID: 20665 RVA: 0x0015552D File Offset: 0x0015372D
		protected CashInstance cashInstance
		{
			get
			{
				return PlayerSingleton<PlayerInventory>.Instance.cashInstance;
			}
		}

		// Token: 0x17000B04 RID: 2820
		// (get) Token: 0x060050BA RID: 20666 RVA: 0x00155539 File Offset: 0x00153739
		public string SaveFolderName
		{
			get
			{
				return "Money";
			}
		}

		// Token: 0x17000B05 RID: 2821
		// (get) Token: 0x060050BB RID: 20667 RVA: 0x00155539 File Offset: 0x00153739
		public string SaveFileName
		{
			get
			{
				return "Money";
			}
		}

		// Token: 0x17000B06 RID: 2822
		// (get) Token: 0x060050BC RID: 20668 RVA: 0x00155540 File Offset: 0x00153740
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x17000B07 RID: 2823
		// (get) Token: 0x060050BD RID: 20669 RVA: 0x00014B5A File Offset: 0x00012D5A
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000B08 RID: 2824
		// (get) Token: 0x060050BE RID: 20670 RVA: 0x00155548 File Offset: 0x00153748
		// (set) Token: 0x060050BF RID: 20671 RVA: 0x00155550 File Offset: 0x00153750
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x17000B09 RID: 2825
		// (get) Token: 0x060050C0 RID: 20672 RVA: 0x00155559 File Offset: 0x00153759
		// (set) Token: 0x060050C1 RID: 20673 RVA: 0x00155561 File Offset: 0x00153761
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x17000B0A RID: 2826
		// (get) Token: 0x060050C2 RID: 20674 RVA: 0x0015556A File Offset: 0x0015376A
		// (set) Token: 0x060050C3 RID: 20675 RVA: 0x00155572 File Offset: 0x00153772
		public bool HasChanged { get; set; }

		// Token: 0x060050C4 RID: 20676 RVA: 0x0015557B File Offset: 0x0015377B
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Money.MoneyManager_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060050C5 RID: 20677 RVA: 0x0003DA73 File Offset: 0x0003BC73
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x060050C6 RID: 20678 RVA: 0x00155590 File Offset: 0x00153790
		protected override void Start()
		{
			base.Start();
			Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(this.Loaded));
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
			TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
			instance2.onDayPass = (Action)Delegate.Combine(instance2.onDayPass, new Action(this.CheckNetworthAchievements));
			Singleton<HUD>.Instance.OnlineBalanceDisplay.SetBalance(this.SyncAccessor_onlineBalance);
		}

		// Token: 0x060050C7 RID: 20679 RVA: 0x0015561F File Offset: 0x0015381F
		public override void OnStartServer()
		{
			base.OnStartServer();
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("LifetimeEarnings", this.lifetimeEarnings.ToString(), true);
		}

		// Token: 0x060050C8 RID: 20680 RVA: 0x00155642 File Offset: 0x00153842
		public override void OnStartClient()
		{
			base.OnStartClient();
			Singleton<HUD>.Instance.OnlineBalanceDisplay.SetBalance(this.SyncAccessor_onlineBalance);
		}

		// Token: 0x060050C9 RID: 20681 RVA: 0x00155660 File Offset: 0x00153860
		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
				TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
				instance2.onDayPass = (Action)Delegate.Remove(instance2.onDayPass, new Action(this.CheckNetworthAchievements));
			}
			if (Singleton<LoadManager>.InstanceExists)
			{
				Singleton<LoadManager>.Instance.onLoadComplete.RemoveListener(new UnityAction(this.Loaded));
			}
		}

		// Token: 0x060050CA RID: 20682 RVA: 0x001556E8 File Offset: 0x001538E8
		private void Loaded()
		{
			this.GetNetWorth();
			Singleton<HUD>.Instance.OnlineBalanceDisplay.SetBalance(this.SyncAccessor_onlineBalance);
		}

		// Token: 0x060050CB RID: 20683 RVA: 0x00155706 File Offset: 0x00153906
		private void Update()
		{
			this.HasChanged = true;
		}

		// Token: 0x060050CC RID: 20684 RVA: 0x00155710 File Offset: 0x00153910
		private void MinPass()
		{
			if (NetworkSingleton<VariableDatabase>.InstanceExists)
			{
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Online_Balance", this.onlineBalance.ToString(), false);
				if (PlayerSingleton<PlayerInventory>.InstanceExists)
				{
					NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Cash_Balance", this.cashBalance.ToString(), false);
					NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Total_Money", (this.SyncAccessor_onlineBalance + this.cashBalance).ToString(), false);
				}
			}
		}

		// Token: 0x060050CD RID: 20685 RVA: 0x00155789 File Offset: 0x00153989
		public CashInstance GetCashInstance(float amount)
		{
			CashInstance cashInstance = Registry.GetItem<CashDefinition>("cash").GetDefaultInstance(1) as CashInstance;
			cashInstance.SetBalance(amount, false);
			return cashInstance;
		}

		// Token: 0x060050CE RID: 20686 RVA: 0x001557A8 File Offset: 0x001539A8
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void CreateOnlineTransaction(string _transaction_Name, float _unit_Amount, float _quantity, string _transaction_Note)
		{
			this.RpcWriter___Server_CreateOnlineTransaction_1419830531(_transaction_Name, _unit_Amount, _quantity, _transaction_Note);
			this.RpcLogic___CreateOnlineTransaction_1419830531(_transaction_Name, _unit_Amount, _quantity, _transaction_Note);
		}

		// Token: 0x060050CF RID: 20687 RVA: 0x001557D8 File Offset: 0x001539D8
		[ObserversRpc]
		private void ReceiveOnlineTransaction(string _transaction_Name, float _unit_Amount, float _quantity, string _transaction_Note)
		{
			this.RpcWriter___Observers_ReceiveOnlineTransaction_1419830531(_transaction_Name, _unit_Amount, _quantity, _transaction_Note);
		}

		// Token: 0x060050D0 RID: 20688 RVA: 0x001557FB File Offset: 0x001539FB
		protected IEnumerator ShowOnlineBalanceChange(RectTransform changeDisplay)
		{
			TextMeshProUGUI text = changeDisplay.GetComponent<TextMeshProUGUI>();
			float startVert = changeDisplay.anchoredPosition.y;
			float lerpTime = 2.5f;
			float vertOffset = startVert + 60f;
			for (float i = 0f; i < lerpTime; i += Time.unscaledDeltaTime)
			{
				text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(1f, 0f, i / lerpTime));
				changeDisplay.anchoredPosition = new Vector2(changeDisplay.anchoredPosition.x, Mathf.Lerp(startVert, vertOffset, i / lerpTime));
				yield return new WaitForEndOfFrame();
			}
			UnityEngine.Object.Destroy(changeDisplay.gameObject);
			yield break;
		}

		// Token: 0x060050D1 RID: 20689 RVA: 0x0015580A File Offset: 0x00153A0A
		[ServerRpc(RequireOwnership = false)]
		public void ChangeLifetimeEarnings(float change)
		{
			this.RpcWriter___Server_ChangeLifetimeEarnings_431000436(change);
		}

		// Token: 0x060050D2 RID: 20690 RVA: 0x00155818 File Offset: 0x00153A18
		public void ChangeCashBalance(float change, bool visualizeChange = true, bool playCashSound = false)
		{
			float num = Mathf.Clamp(this.cashInstance.Balance + change, 0f, float.MaxValue) - this.cashInstance.Balance;
			this.cashInstance.ChangeBalance(change);
			if (playCashSound && num != 0f)
			{
				Console.Log("Playing cash sound: " + num.ToString(), null);
				this.CashSound.Play();
			}
			if (visualizeChange && num != 0f)
			{
				RectTransform component = UnityEngine.Object.Instantiate<GameObject>(this.cashChangePrefab, Singleton<HUD>.Instance.cashSlotContainer).GetComponent<RectTransform>();
				component.position = new Vector3(Singleton<HUD>.Instance.cashSlotUI.position.x, component.position.y);
				component.anchoredPosition = new Vector2(component.anchoredPosition.x, 10f);
				TextMeshProUGUI component2 = component.GetComponent<TextMeshProUGUI>();
				if (num > 0f)
				{
					component2.text = "+ " + MoneyManager.FormatAmount(num, false, false);
					component2.color = new Color32(25, 240, 30, byte.MaxValue);
				}
				else
				{
					component2.text = MoneyManager.FormatAmount(num, false, false);
					component2.color = new Color32(176, 63, 59, byte.MaxValue);
				}
				Singleton<CoroutineService>.Instance.StartCoroutine(this.ShowCashChange(component));
			}
		}

		// Token: 0x060050D3 RID: 20691 RVA: 0x0015597D File Offset: 0x00153B7D
		protected IEnumerator ShowCashChange(RectTransform changeDisplay)
		{
			TextMeshProUGUI text = changeDisplay.GetComponent<TextMeshProUGUI>();
			float startVert = changeDisplay.anchoredPosition.y;
			float lerpTime = 2.5f;
			float vertOffset = startVert + 60f;
			for (float i = 0f; i < lerpTime; i += Time.unscaledDeltaTime)
			{
				text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(1f, 0f, i / lerpTime));
				changeDisplay.anchoredPosition = new Vector2(changeDisplay.anchoredPosition.x, Mathf.Lerp(startVert, vertOffset, i / lerpTime));
				yield return new WaitForEndOfFrame();
			}
			UnityEngine.Object.Destroy(changeDisplay.gameObject);
			yield break;
		}

		// Token: 0x060050D4 RID: 20692 RVA: 0x0015598C File Offset: 0x00153B8C
		public static string FormatAmount(float amount, bool showDecimals = false, bool includeColor = false)
		{
			string text = string.Empty;
			if (includeColor)
			{
				text += "<color=#54E717>";
			}
			if (amount < 0f)
			{
				text = "-";
			}
			if (showDecimals)
			{
				text += string.Format(new CultureInfo("en-US"), "{0:C}", Mathf.Abs(amount));
			}
			else
			{
				text += string.Format(new CultureInfo("en-US"), "{0:C0}", Mathf.RoundToInt(Mathf.Abs(amount)));
			}
			if (includeColor)
			{
				text += "</color>";
			}
			return text;
		}

		// Token: 0x060050D5 RID: 20693 RVA: 0x00155A22 File Offset: 0x00153C22
		public virtual string GetSaveString()
		{
			return new MoneyData(this.SyncAccessor_onlineBalance, this.GetNetWorth(), this.SyncAccessor_lifetimeEarnings, ATM.WeeklyDepositSum).GetJson(true);
		}

		// Token: 0x060050D6 RID: 20694 RVA: 0x00155A48 File Offset: 0x00153C48
		public void Load(MoneyData data)
		{
			this.sync___set_value_onlineBalance(Mathf.Clamp(data.OnlineBalance, 0f, float.MaxValue), true);
			this.sync___set_value_lifetimeEarnings(Mathf.Clamp(data.LifetimeEarnings, 0f, float.MaxValue), true);
			Singleton<HUD>.Instance.OnlineBalanceDisplay.SetBalance(this.SyncAccessor_onlineBalance);
			ATM.WeeklyDepositSum = data.WeeklyDepositSum;
		}

		// Token: 0x060050D7 RID: 20695 RVA: 0x00155AAD File Offset: 0x00153CAD
		public void CheckNetworthAchievements()
		{
			float netWorth = this.GetNetWorth();
			if (netWorth >= 100000f)
			{
				Singleton<AchievementManager>.Instance.UnlockAchievement(AchievementManager.EAchievement.BUSINESSMAN);
			}
			if (netWorth >= 1000000f)
			{
				Singleton<AchievementManager>.Instance.UnlockAchievement(AchievementManager.EAchievement.BIGWIG);
			}
			if (netWorth >= 10000000f)
			{
				Singleton<AchievementManager>.Instance.UnlockAchievement(AchievementManager.EAchievement.MAGNATE);
			}
		}

		// Token: 0x060050D8 RID: 20696 RVA: 0x00155AF0 File Offset: 0x00153CF0
		public float GetNetWorth()
		{
			float num = 0f;
			num += this.SyncAccessor_onlineBalance;
			if (this.onNetworthCalculation != null)
			{
				MoneyManager.FloatContainer floatContainer = new MoneyManager.FloatContainer();
				this.onNetworthCalculation(floatContainer);
				num += floatContainer.value;
			}
			this.LastCalculatedNetworth = num;
			return num;
		}

		// Token: 0x060050DA RID: 20698 RVA: 0x00155B6C File Offset: 0x00153D6C
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Money.MoneyManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Money.MoneyManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___lifetimeEarnings = new SyncVar<float>(this, 1U, 1, 0, -1f, 0, this.lifetimeEarnings);
			this.syncVar___onlineBalance = new SyncVar<float>(this, 0U, 1, 0, -1f, 0, this.onlineBalance);
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_CreateOnlineTransaction_1419830531));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveOnlineTransaction_1419830531));
			base.RegisterServerRpc(2U, new ServerRpcDelegate(this.RpcReader___Server_ChangeLifetimeEarnings_431000436));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.Money.MoneyManager));
		}

		// Token: 0x060050DB RID: 20699 RVA: 0x00155C3D File Offset: 0x00153E3D
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Money.MoneyManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Money.MoneyManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___lifetimeEarnings.SetRegistered();
			this.syncVar___onlineBalance.SetRegistered();
		}

		// Token: 0x060050DC RID: 20700 RVA: 0x00155C6C File Offset: 0x00153E6C
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060050DD RID: 20701 RVA: 0x00155C7C File Offset: 0x00153E7C
		private void RpcWriter___Server_CreateOnlineTransaction_1419830531(string _transaction_Name, float _unit_Amount, float _quantity, string _transaction_Note)
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
			writer.WriteString(_transaction_Name);
			writer.WriteSingle(_unit_Amount, 0);
			writer.WriteSingle(_quantity, 0);
			writer.WriteString(_transaction_Note);
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060050DE RID: 20702 RVA: 0x00155D54 File Offset: 0x00153F54
		public void RpcLogic___CreateOnlineTransaction_1419830531(string _transaction_Name, float _unit_Amount, float _quantity, string _transaction_Note)
		{
			this.ReceiveOnlineTransaction(_transaction_Name, _unit_Amount, _quantity, _transaction_Note);
		}

		// Token: 0x060050DF RID: 20703 RVA: 0x00155D64 File Offset: 0x00153F64
		private void RpcReader___Server_CreateOnlineTransaction_1419830531(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string transaction_Name = PooledReader0.ReadString();
			float unit_Amount = PooledReader0.ReadSingle(0);
			float quantity = PooledReader0.ReadSingle(0);
			string transaction_Note = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___CreateOnlineTransaction_1419830531(transaction_Name, unit_Amount, quantity, transaction_Note);
		}

		// Token: 0x060050E0 RID: 20704 RVA: 0x00155DE0 File Offset: 0x00153FE0
		private void RpcWriter___Observers_ReceiveOnlineTransaction_1419830531(string _transaction_Name, float _unit_Amount, float _quantity, string _transaction_Note)
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
			writer.WriteString(_transaction_Name);
			writer.WriteSingle(_unit_Amount, 0);
			writer.WriteSingle(_quantity, 0);
			writer.WriteString(_transaction_Note);
			base.SendObserversRpc(1U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060050E1 RID: 20705 RVA: 0x00155EC8 File Offset: 0x001540C8
		private void RpcLogic___ReceiveOnlineTransaction_1419830531(string _transaction_Name, float _unit_Amount, float _quantity, string _transaction_Note)
		{
			Transaction transaction = new Transaction(_transaction_Name, _unit_Amount, _quantity, _transaction_Note);
			this.ledger.Add(transaction);
			this.sync___set_value_onlineBalance(this.SyncAccessor_onlineBalance + transaction.total_Amount, true);
			Singleton<HUD>.Instance.OnlineBalanceDisplay.SetBalance(this.SyncAccessor_onlineBalance);
			Singleton<HUD>.Instance.OnlineBalanceDisplay.Show();
			RectTransform component = UnityEngine.Object.Instantiate<GameObject>(this.moneyChangePrefab, Singleton<HUD>.Instance.cashSlotContainer).GetComponent<RectTransform>();
			component.position = new Vector3(Singleton<HUD>.Instance.onlineBalanceSlotUI.position.x, component.position.y);
			component.anchoredPosition = new Vector2(component.anchoredPosition.x, 10f);
			TextMeshProUGUI component2 = component.GetComponent<TextMeshProUGUI>();
			if (transaction.total_Amount > 0f)
			{
				component2.text = "+ " + MoneyManager.FormatAmount(transaction.total_Amount, false, false);
				component2.color = new Color32(25, 190, 240, byte.MaxValue);
			}
			else
			{
				component2.text = MoneyManager.FormatAmount(transaction.total_Amount, false, false);
				component2.color = new Color32(176, 63, 59, byte.MaxValue);
			}
			Singleton<CoroutineService>.Instance.StartCoroutine(this.ShowOnlineBalanceChange(component));
			this.HasChanged = true;
		}

		// Token: 0x060050E2 RID: 20706 RVA: 0x00156024 File Offset: 0x00154224
		private void RpcReader___Observers_ReceiveOnlineTransaction_1419830531(PooledReader PooledReader0, Channel channel)
		{
			string transaction_Name = PooledReader0.ReadString();
			float unit_Amount = PooledReader0.ReadSingle(0);
			float quantity = PooledReader0.ReadSingle(0);
			string transaction_Note = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveOnlineTransaction_1419830531(transaction_Name, unit_Amount, quantity, transaction_Note);
		}

		// Token: 0x060050E3 RID: 20707 RVA: 0x00156094 File Offset: 0x00154294
		private void RpcWriter___Server_ChangeLifetimeEarnings_431000436(float change)
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
			writer.WriteSingle(change, 0);
			base.SendServerRpc(2U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060050E4 RID: 20708 RVA: 0x00156140 File Offset: 0x00154340
		public void RpcLogic___ChangeLifetimeEarnings_431000436(float change)
		{
			this.sync___set_value_lifetimeEarnings(Mathf.Clamp(this.SyncAccessor_lifetimeEarnings + change, 0f, float.MaxValue), true);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("LifetimeEarnings", this.lifetimeEarnings.ToString(), true);
		}

		// Token: 0x060050E5 RID: 20709 RVA: 0x0015617C File Offset: 0x0015437C
		private void RpcReader___Server_ChangeLifetimeEarnings_431000436(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			float change = PooledReader0.ReadSingle(0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___ChangeLifetimeEarnings_431000436(change);
		}

		// Token: 0x17000B0B RID: 2827
		// (get) Token: 0x060050E6 RID: 20710 RVA: 0x001561B2 File Offset: 0x001543B2
		// (set) Token: 0x060050E7 RID: 20711 RVA: 0x001561BA File Offset: 0x001543BA
		public float SyncAccessor_onlineBalance
		{
			get
			{
				return this.onlineBalance;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.onlineBalance = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___onlineBalance.SetValue(value, value);
				}
			}
		}

		// Token: 0x060050E8 RID: 20712 RVA: 0x001561F8 File Offset: 0x001543F8
		public override bool MoneyManager(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 == 1U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_lifetimeEarnings(this.syncVar___lifetimeEarnings.GetValue(true), true);
					return true;
				}
				float value = PooledReader0.ReadSingle(0);
				this.sync___set_value_lifetimeEarnings(value, Boolean2);
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
					this.sync___set_value_onlineBalance(this.syncVar___onlineBalance.GetValue(true), true);
					return true;
				}
				float value2 = PooledReader0.ReadSingle(0);
				this.sync___set_value_onlineBalance(value2, Boolean2);
				return true;
			}
		}

		// Token: 0x17000B0C RID: 2828
		// (get) Token: 0x060050E9 RID: 20713 RVA: 0x00156298 File Offset: 0x00154498
		// (set) Token: 0x060050EA RID: 20714 RVA: 0x001562A0 File Offset: 0x001544A0
		public float SyncAccessor_lifetimeEarnings
		{
			get
			{
				return this.lifetimeEarnings;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.lifetimeEarnings = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___lifetimeEarnings.SetValue(value, value);
				}
			}
		}

		// Token: 0x060050EB RID: 20715 RVA: 0x001562DC File Offset: 0x001544DC
		protected override void dll()
		{
			base.Awake();
			this.InitializeSaveable();
		}

		// Token: 0x04003C98 RID: 15512
		public const string MONEY_TEXT_COLOR = "#54E717";

		// Token: 0x04003C99 RID: 15513
		public const string MONEY_TEXT_COLOR_DARKER = "#46CB4F";

		// Token: 0x04003C9A RID: 15514
		public const string ONLINE_BALANCE_COLOR = "#4CBFFF";

		// Token: 0x04003C9B RID: 15515
		public List<Transaction> ledger = new List<Transaction>();

		// Token: 0x04003C9C RID: 15516
		[SyncVar]
		public float onlineBalance;

		// Token: 0x04003C9D RID: 15517
		[SyncVar]
		public float lifetimeEarnings;

		// Token: 0x04003C9F RID: 15519
		public AudioSourceController CashSound;

		// Token: 0x04003CA0 RID: 15520
		[Header("Prefabs")]
		[SerializeField]
		protected GameObject moneyChangePrefab;

		// Token: 0x04003CA1 RID: 15521
		[SerializeField]
		protected GameObject cashChangePrefab;

		// Token: 0x04003CA2 RID: 15522
		public Sprite LaunderingNotificationIcon;

		// Token: 0x04003CA3 RID: 15523
		public Action<MoneyManager.FloatContainer> onNetworthCalculation;

		// Token: 0x04003CA4 RID: 15524
		private MoneyLoader loader = new MoneyLoader();

		// Token: 0x04003CA8 RID: 15528
		public SyncVar<float> syncVar___onlineBalance;

		// Token: 0x04003CA9 RID: 15529
		public SyncVar<float> syncVar___lifetimeEarnings;

		// Token: 0x04003CAA RID: 15530
		private bool dll_Excuted;

		// Token: 0x04003CAB RID: 15531
		private bool dll_Excuted;

		// Token: 0x02000BDE RID: 3038
		public class FloatContainer
		{
			// Token: 0x17000B0D RID: 2829
			// (get) Token: 0x060050EC RID: 20716 RVA: 0x001562EA File Offset: 0x001544EA
			// (set) Token: 0x060050ED RID: 20717 RVA: 0x001562F2 File Offset: 0x001544F2
			public float value { get; private set; }

			// Token: 0x060050EE RID: 20718 RVA: 0x001562FB File Offset: 0x001544FB
			public void ChangeValue(float value)
			{
				this.value += value;
			}
		}
	}
}
