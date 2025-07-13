using System;
using EasyButtons;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.Interaction;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Casino
{
	// Token: 0x020007AA RID: 1962
	public class SlotMachine : NetworkBehaviour
	{
		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x0600353D RID: 13629 RVA: 0x000DE927 File Offset: 0x000DCB27
		// (set) Token: 0x0600353E RID: 13630 RVA: 0x000DE92F File Offset: 0x000DCB2F
		public bool IsSpinning { get; private set; }

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x0600353F RID: 13631 RVA: 0x000DE938 File Offset: 0x000DCB38
		private int currentBetAmount
		{
			get
			{
				return SlotMachine.BetAmounts[this.currentBetIndex];
			}
		}

		// Token: 0x06003540 RID: 13632 RVA: 0x000DE948 File Offset: 0x000DCB48
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Casino.SlotMachine_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003541 RID: 13633 RVA: 0x000DE967 File Offset: 0x000DCB67
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (this.currentBetIndex != 1)
			{
				this.SetBetIndex(connection, this.currentBetIndex);
			}
		}

		// Token: 0x06003542 RID: 13634 RVA: 0x000DE986 File Offset: 0x000DCB86
		private void DownHovered()
		{
			if (this.IsSpinning)
			{
				this.DownButton.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				return;
			}
			this.DownButton.SetInteractableState(InteractableObject.EInteractableState.Default);
			this.DownButton.SetMessage("Decrease bet");
		}

		// Token: 0x06003543 RID: 13635 RVA: 0x000DE9B9 File Offset: 0x000DCBB9
		private void DownInteracted()
		{
			if (this.onDownPressed != null)
			{
				this.onDownPressed.Invoke();
			}
			this.SendBetIndex(this.currentBetIndex - 1);
		}

		// Token: 0x06003544 RID: 13636 RVA: 0x000DE9DC File Offset: 0x000DCBDC
		private void UpHovered()
		{
			if (this.IsSpinning)
			{
				this.UpButton.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				return;
			}
			this.UpButton.SetInteractableState(InteractableObject.EInteractableState.Default);
			this.UpButton.SetMessage("Increase bet");
		}

		// Token: 0x06003545 RID: 13637 RVA: 0x000DEA0F File Offset: 0x000DCC0F
		private void UpInteracted()
		{
			if (this.onUpPressed != null)
			{
				this.onUpPressed.Invoke();
			}
			this.SendBetIndex(this.currentBetIndex + 1);
		}

		// Token: 0x06003546 RID: 13638 RVA: 0x000DEA34 File Offset: 0x000DCC34
		private void HandleHovered()
		{
			if (this.IsSpinning)
			{
				this.HandleIntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				return;
			}
			int currentBetAmount = this.currentBetAmount;
			if (NetworkSingleton<MoneyManager>.Instance.cashBalance < (float)currentBetAmount)
			{
				this.HandleIntObj.SetInteractableState(InteractableObject.EInteractableState.Invalid);
				this.HandleIntObj.SetMessage("Insufficient cash");
				return;
			}
			this.HandleIntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
			this.HandleIntObj.SetMessage("Pull handle");
		}

		// Token: 0x06003547 RID: 13639 RVA: 0x000DEAA4 File Offset: 0x000DCCA4
		[Button]
		public void HandleInteracted()
		{
			if (this.IsSpinning)
			{
				return;
			}
			if (this.onHandlePulled != null)
			{
				this.onHandlePulled.Invoke();
			}
			NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance((float)(-(float)this.currentBetAmount), true, false);
			this.SendStartSpin(Player.Local.LocalConnection, this.currentBetAmount);
		}

		// Token: 0x06003548 RID: 13640 RVA: 0x000DEAF7 File Offset: 0x000DCCF7
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		private void SendBetIndex(int index)
		{
			this.RpcWriter___Server_SendBetIndex_3316948804(index);
			this.RpcLogic___SendBetIndex_3316948804(index);
		}

		// Token: 0x06003549 RID: 13641 RVA: 0x000DEB0D File Offset: 0x000DCD0D
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SetBetIndex(NetworkConnection conn, int index)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetBetIndex_2681120339(conn, index);
				this.RpcLogic___SetBetIndex_2681120339(conn, index);
			}
			else
			{
				this.RpcWriter___Target_SetBetIndex_2681120339(conn, index);
			}
		}

		// Token: 0x0600354A RID: 13642 RVA: 0x000DEB44 File Offset: 0x000DCD44
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendStartSpin(NetworkConnection spinner, int betAmount)
		{
			this.RpcWriter___Server_SendStartSpin_2681120339(spinner, betAmount);
			this.RpcLogic___SendStartSpin_2681120339(spinner, betAmount);
		}

		// Token: 0x0600354B RID: 13643 RVA: 0x000DEB70 File Offset: 0x000DCD70
		[ObserversRpc(RunLocally = true)]
		public void StartSpin(NetworkConnection spinner, SlotMachine.ESymbol[] symbols, int betAmount)
		{
			this.RpcWriter___Observers_StartSpin_2659526290(spinner, symbols, betAmount);
			this.RpcLogic___StartSpin_2659526290(spinner, symbols, betAmount);
		}

		// Token: 0x0600354C RID: 13644 RVA: 0x000DEBA1 File Offset: 0x000DCDA1
		private SlotMachine.EOutcome EvaluateOutcome(SlotMachine.ESymbol[] outcome)
		{
			if (this.IsUniform(outcome))
			{
				if (outcome[0] == SlotMachine.ESymbol.Seven)
				{
					return SlotMachine.EOutcome.Jackpot;
				}
				if (outcome[0] == SlotMachine.ESymbol.Bell)
				{
					return SlotMachine.EOutcome.BigWin;
				}
				if (this.IsFruit(outcome[0]))
				{
					return SlotMachine.EOutcome.SmallWin;
				}
			}
			if (this.IsAllFruit(outcome))
			{
				return SlotMachine.EOutcome.MiniWin;
			}
			return SlotMachine.EOutcome.NoWin;
		}

		// Token: 0x0600354D RID: 13645 RVA: 0x000DEBD5 File Offset: 0x000DCDD5
		private int GetWinAmount(SlotMachine.EOutcome outcome, int betAmount)
		{
			switch (outcome)
			{
			case SlotMachine.EOutcome.Jackpot:
				return betAmount * 100;
			case SlotMachine.EOutcome.BigWin:
				return betAmount * 25;
			case SlotMachine.EOutcome.SmallWin:
				return betAmount * 10;
			case SlotMachine.EOutcome.MiniWin:
				return betAmount * 2;
			default:
				return 0;
			}
		}

		// Token: 0x0600354E RID: 13646 RVA: 0x000DEC04 File Offset: 0x000DCE04
		private void DisplayOutcome(SlotMachine.EOutcome outcome, int winAmount)
		{
			TextMeshProUGUI[] winAmountLabels = this.WinAmountLabels;
			for (int i = 0; i < winAmountLabels.Length; i++)
			{
				winAmountLabels[i].text = MoneyManager.FormatAmount((float)winAmount, false, false);
			}
			if (outcome == SlotMachine.EOutcome.Jackpot)
			{
				this.ScreenAnimation.Play(this.JackpotAnimation.name);
				ParticleSystem[] jackpotParticles = this.JackpotParticles;
				for (int i = 0; i < jackpotParticles.Length; i++)
				{
					jackpotParticles[i].Play();
				}
				return;
			}
			if (outcome == SlotMachine.EOutcome.BigWin)
			{
				this.ScreenAnimation.Play(this.BigWinAnimation.name);
				this.BigWinSound.Play();
				return;
			}
			if (outcome == SlotMachine.EOutcome.SmallWin)
			{
				this.ScreenAnimation.Play(this.SmallWinAnimation.name);
				this.SmallWinSound.Play();
				return;
			}
			if (outcome == SlotMachine.EOutcome.MiniWin)
			{
				this.ScreenAnimation.Play(this.MiniWinAnimation.name);
				this.MiniWinSound.Play();
			}
		}

		// Token: 0x0600354F RID: 13647 RVA: 0x000DECE3 File Offset: 0x000DCEE3
		public static SlotMachine.ESymbol GetRandomSymbol()
		{
			if (Application.isEditor)
			{
				return SlotMachine.ESymbol.Seven;
			}
			return (SlotMachine.ESymbol)UnityEngine.Random.Range(0, Enum.GetValues(typeof(SlotMachine.ESymbol)).Length);
		}

		// Token: 0x06003550 RID: 13648 RVA: 0x000DED08 File Offset: 0x000DCF08
		private bool IsFruit(SlotMachine.ESymbol symbol)
		{
			return symbol == SlotMachine.ESymbol.Cherry || symbol == SlotMachine.ESymbol.Lemon || symbol == SlotMachine.ESymbol.Grape || symbol == SlotMachine.ESymbol.Watermelon;
		}

		// Token: 0x06003551 RID: 13649 RVA: 0x000DED1C File Offset: 0x000DCF1C
		private bool IsAllFruit(SlotMachine.ESymbol[] symbols)
		{
			for (int i = 0; i < symbols.Length; i++)
			{
				if (!this.IsFruit(symbols[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003552 RID: 13650 RVA: 0x000DED48 File Offset: 0x000DCF48
		private bool IsUniform(SlotMachine.ESymbol[] symbols)
		{
			for (int i = 1; i < symbols.Length; i++)
			{
				if (symbols[i] != symbols[i - 1])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003553 RID: 13651 RVA: 0x000DED70 File Offset: 0x000DCF70
		[Button]
		public void SimulateMany()
		{
			int num = 100;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			for (int i = 0; i < num; i++)
			{
				num2--;
				SlotMachine.ESymbol[] array = new SlotMachine.ESymbol[this.Reels.Length];
				for (int j = 0; j < this.Reels.Length; j++)
				{
					array[j] = SlotMachine.GetRandomSymbol();
				}
				SlotMachine.EOutcome eoutcome = this.EvaluateOutcome(array);
				if (eoutcome == SlotMachine.EOutcome.MiniWin)
				{
					num4++;
				}
				if (eoutcome == SlotMachine.EOutcome.SmallWin)
				{
					num3++;
				}
				if (eoutcome == SlotMachine.EOutcome.BigWin)
				{
					num5++;
				}
				if (eoutcome == SlotMachine.EOutcome.Jackpot)
				{
					num6++;
				}
				int winAmount = this.GetWinAmount(eoutcome, 1);
				num2 += winAmount;
			}
			Console.Log("Simulated " + num.ToString() + " spins. Net win: " + num2.ToString(), null);
			Console.Log(string.Concat(new string[]
			{
				"Mini wins: ",
				num4.ToString(),
				" Small wins: ",
				num3.ToString(),
				" Big wins: ",
				num5.ToString(),
				" Jackpots: ",
				num6.ToString()
			}), null);
		}

		// Token: 0x06003556 RID: 13654 RVA: 0x000DEEB4 File Offset: 0x000DD0B4
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Casino.SlotMachineAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Casino.SlotMachineAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendBetIndex_3316948804));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_SetBetIndex_2681120339));
			base.RegisterTargetRpc(2U, new ClientRpcDelegate(this.RpcReader___Target_SetBetIndex_2681120339));
			base.RegisterServerRpc(3U, new ServerRpcDelegate(this.RpcReader___Server_SendStartSpin_2681120339));
			base.RegisterObserversRpc(4U, new ClientRpcDelegate(this.RpcReader___Observers_StartSpin_2659526290));
		}

		// Token: 0x06003557 RID: 13655 RVA: 0x000DEF45 File Offset: 0x000DD145
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Casino.SlotMachineAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Casino.SlotMachineAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06003558 RID: 13656 RVA: 0x000DEF58 File Offset: 0x000DD158
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003559 RID: 13657 RVA: 0x000DEF68 File Offset: 0x000DD168
		private void RpcWriter___Server_SendBetIndex_3316948804(int index)
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
			writer.WriteInt32(index, 1);
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x0600355A RID: 13658 RVA: 0x000DF014 File Offset: 0x000DD214
		private void RpcLogic___SendBetIndex_3316948804(int index)
		{
			this.SetBetIndex(null, index);
		}

		// Token: 0x0600355B RID: 13659 RVA: 0x000DF020 File Offset: 0x000DD220
		private void RpcReader___Server_SendBetIndex_3316948804(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int index = PooledReader0.ReadInt32(1);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendBetIndex_3316948804(index);
		}

		// Token: 0x0600355C RID: 13660 RVA: 0x000DF064 File Offset: 0x000DD264
		private void RpcWriter___Observers_SetBetIndex_2681120339(NetworkConnection conn, int index)
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
			writer.WriteInt32(index, 1);
			base.SendObserversRpc(1U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600355D RID: 13661 RVA: 0x000DF11F File Offset: 0x000DD31F
		public void RpcLogic___SetBetIndex_2681120339(NetworkConnection conn, int index)
		{
			this.currentBetIndex = Mathf.Clamp(index, 0, SlotMachine.BetAmounts.Length - 1);
			this.BetAmountLabel.text = MoneyManager.FormatAmount((float)this.currentBetAmount, false, false);
		}

		// Token: 0x0600355E RID: 13662 RVA: 0x000DF150 File Offset: 0x000DD350
		private void RpcReader___Observers_SetBetIndex_2681120339(PooledReader PooledReader0, Channel channel)
		{
			int index = PooledReader0.ReadInt32(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetBetIndex_2681120339(null, index);
		}

		// Token: 0x0600355F RID: 13663 RVA: 0x000DF194 File Offset: 0x000DD394
		private void RpcWriter___Target_SetBetIndex_2681120339(NetworkConnection conn, int index)
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
			writer.WriteInt32(index, 1);
			base.SendTargetRpc(2U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06003560 RID: 13664 RVA: 0x000DF250 File Offset: 0x000DD450
		private void RpcReader___Target_SetBetIndex_2681120339(PooledReader PooledReader0, Channel channel)
		{
			int index = PooledReader0.ReadInt32(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetBetIndex_2681120339(base.LocalConnection, index);
		}

		// Token: 0x06003561 RID: 13665 RVA: 0x000DF28C File Offset: 0x000DD48C
		private void RpcWriter___Server_SendStartSpin_2681120339(NetworkConnection spinner, int betAmount)
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
			writer.WriteNetworkConnection(spinner);
			writer.WriteInt32(betAmount, 1);
			base.SendServerRpc(3U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06003562 RID: 13666 RVA: 0x000DF348 File Offset: 0x000DD548
		public void RpcLogic___SendStartSpin_2681120339(NetworkConnection spinner, int betAmount)
		{
			SlotMachine.ESymbol[] array = new SlotMachine.ESymbol[this.Reels.Length];
			for (int i = 0; i < this.Reels.Length; i++)
			{
				array[i] = SlotMachine.GetRandomSymbol();
			}
			this.StartSpin(spinner, array, betAmount);
		}

		// Token: 0x06003563 RID: 13667 RVA: 0x000DF388 File Offset: 0x000DD588
		private void RpcReader___Server_SendStartSpin_2681120339(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkConnection spinner = PooledReader0.ReadNetworkConnection();
			int betAmount = PooledReader0.ReadInt32(1);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendStartSpin_2681120339(spinner, betAmount);
		}

		// Token: 0x06003564 RID: 13668 RVA: 0x000DF3DC File Offset: 0x000DD5DC
		private void RpcWriter___Observers_StartSpin_2659526290(NetworkConnection spinner, SlotMachine.ESymbol[] symbols, int betAmount)
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
			writer.WriteNetworkConnection(spinner);
			writer.Write___ScheduleOne.Casino.SlotMachine/ESymbol[]FishNet.Serializing.Generated(symbols);
			writer.WriteInt32(betAmount, 1);
			base.SendObserversRpc(4U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06003565 RID: 13669 RVA: 0x000DF4B4 File Offset: 0x000DD6B4
		public void RpcLogic___StartSpin_2659526290(NetworkConnection spinner, SlotMachine.ESymbol[] symbols, int betAmount)
		{
			SlotMachine.<>c__DisplayClass41_0 CS$<>8__locals1 = new SlotMachine.<>c__DisplayClass41_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.symbols = symbols;
			CS$<>8__locals1.betAmount = betAmount;
			CS$<>8__locals1.spinner = spinner;
			if (this.IsSpinning)
			{
				return;
			}
			this.IsSpinning = true;
			Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<StartSpin>g__Spin|0());
		}

		// Token: 0x06003566 RID: 13670 RVA: 0x000DF504 File Offset: 0x000DD704
		private void RpcReader___Observers_StartSpin_2659526290(PooledReader PooledReader0, Channel channel)
		{
			NetworkConnection spinner = PooledReader0.ReadNetworkConnection();
			SlotMachine.ESymbol[] symbols = GeneratedReaders___Internal.Read___ScheduleOne.Casino.SlotMachine/ESymbol[]FishNet.Serializing.Generateds(PooledReader0);
			int betAmount = PooledReader0.ReadInt32(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___StartSpin_2659526290(spinner, symbols, betAmount);
		}

		// Token: 0x06003567 RID: 13671 RVA: 0x000DF568 File Offset: 0x000DD768
		private void dll()
		{
			this.DownButton.onHovered.AddListener(new UnityAction(this.DownHovered));
			this.DownButton.onInteractStart.AddListener(new UnityAction(this.DownInteracted));
			this.UpButton.onHovered.AddListener(new UnityAction(this.UpHovered));
			this.UpButton.onInteractStart.AddListener(new UnityAction(this.UpInteracted));
			this.HandleIntObj.onHovered.AddListener(new UnityAction(this.HandleHovered));
			this.HandleIntObj.onInteractStart.AddListener(new UnityAction(this.HandleInteracted));
			this.SetBetIndex(null, this.currentBetIndex);
		}

		// Token: 0x0400259A RID: 9626
		public static int[] BetAmounts = new int[]
		{
			5,
			10,
			25,
			50,
			100
		};

		// Token: 0x0400259C RID: 9628
		[Header("References")]
		public InteractableObject DownButton;

		// Token: 0x0400259D RID: 9629
		public InteractableObject UpButton;

		// Token: 0x0400259E RID: 9630
		public InteractableObject HandleIntObj;

		// Token: 0x0400259F RID: 9631
		public TextMeshPro BetAmountLabel;

		// Token: 0x040025A0 RID: 9632
		public SlotReel[] Reels;

		// Token: 0x040025A1 RID: 9633
		public AudioSourceController SpinLoop;

		// Token: 0x040025A2 RID: 9634
		public Animation ScreenAnimation;

		// Token: 0x040025A3 RID: 9635
		public ParticleSystem[] JackpotParticles;

		// Token: 0x040025A4 RID: 9636
		[Header("Win Animations")]
		public TextMeshProUGUI[] WinAmountLabels;

		// Token: 0x040025A5 RID: 9637
		public AnimationClip MiniWinAnimation;

		// Token: 0x040025A6 RID: 9638
		public AnimationClip SmallWinAnimation;

		// Token: 0x040025A7 RID: 9639
		public AnimationClip BigWinAnimation;

		// Token: 0x040025A8 RID: 9640
		public AnimationClip JackpotAnimation;

		// Token: 0x040025A9 RID: 9641
		public AudioSourceController MiniWinSound;

		// Token: 0x040025AA RID: 9642
		public AudioSourceController SmallWinSound;

		// Token: 0x040025AB RID: 9643
		public AudioSourceController BigWinSound;

		// Token: 0x040025AC RID: 9644
		public AudioSourceController JackpotSound;

		// Token: 0x040025AD RID: 9645
		public UnityEvent onDownPressed;

		// Token: 0x040025AE RID: 9646
		public UnityEvent onUpPressed;

		// Token: 0x040025AF RID: 9647
		public UnityEvent onHandlePulled;

		// Token: 0x040025B0 RID: 9648
		private int currentBetIndex = 1;

		// Token: 0x040025B1 RID: 9649
		private bool dll_Excuted;

		// Token: 0x040025B2 RID: 9650
		private bool dll_Excuted;

		// Token: 0x020007AB RID: 1963
		public enum ESymbol
		{
			// Token: 0x040025B4 RID: 9652
			Cherry,
			// Token: 0x040025B5 RID: 9653
			Lemon,
			// Token: 0x040025B6 RID: 9654
			Grape,
			// Token: 0x040025B7 RID: 9655
			Watermelon,
			// Token: 0x040025B8 RID: 9656
			Bell,
			// Token: 0x040025B9 RID: 9657
			Seven
		}

		// Token: 0x020007AC RID: 1964
		public enum EOutcome
		{
			// Token: 0x040025BB RID: 9659
			Jackpot,
			// Token: 0x040025BC RID: 9660
			BigWin,
			// Token: 0x040025BD RID: 9661
			SmallWin,
			// Token: 0x040025BE RID: 9662
			MiniWin,
			// Token: 0x040025BF RID: 9663
			NoWin
		}
	}
}
