using System;
using System.Collections.Generic;
using System.Linq;
using FishNet;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.ItemFramework;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A0F RID: 2575
	public class DailySummary : NetworkSingleton<DailySummary>
	{
		// Token: 0x170009AD RID: 2477
		// (get) Token: 0x06004544 RID: 17732 RVA: 0x00122B32 File Offset: 0x00120D32
		// (set) Token: 0x06004545 RID: 17733 RVA: 0x00122B3A File Offset: 0x00120D3A
		public bool IsOpen { get; private set; }

		// Token: 0x170009AE RID: 2478
		// (get) Token: 0x06004546 RID: 17734 RVA: 0x00122B43 File Offset: 0x00120D43
		// (set) Token: 0x06004547 RID: 17735 RVA: 0x00122B4B File Offset: 0x00120D4B
		public int xpGained { get; private set; }

		// Token: 0x06004548 RID: 17736 RVA: 0x00122B54 File Offset: 0x00120D54
		protected override void Start()
		{
			base.Start();
			this.IsOpen = false;
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			NetworkSingleton<TimeManager>.Instance._onSleepEnd.AddListener(new UnityAction(this.SleepEnd));
		}

		// Token: 0x06004549 RID: 17737 RVA: 0x00122BA8 File Offset: 0x00120DA8
		public void Open()
		{
			DailySummary.<>c__DisplayClass21_0 CS$<>8__locals1 = new DailySummary.<>c__DisplayClass21_0();
			CS$<>8__locals1.<>4__this = this;
			this.IsOpen = true;
			this.TitleLabel.text = NetworkSingleton<TimeManager>.Instance.CurrentDay.ToString() + ", Day " + (NetworkSingleton<TimeManager>.Instance.ElapsedDays + 1).ToString();
			CS$<>8__locals1.items = this.itemsSoldByPlayer.Keys.ToArray<string>();
			for (int i = 0; i < this.ProductEntries.Length; i++)
			{
				if (i < CS$<>8__locals1.items.Length)
				{
					ItemDefinition item = Registry.GetItem(CS$<>8__locals1.items[i]);
					this.ProductEntries[i].Find("Quantity").GetComponent<TextMeshProUGUI>().text = this.itemsSoldByPlayer[CS$<>8__locals1.items[i]].ToString() + "x";
					this.ProductEntries[i].Find("Image").GetComponent<Image>().sprite = item.Icon;
					this.ProductEntries[i].Find("Name").GetComponent<TextMeshProUGUI>().text = item.Name;
					this.ProductEntries[i].gameObject.SetActive(true);
				}
				else
				{
					this.ProductEntries[i].gameObject.SetActive(false);
				}
			}
			this.PlayerEarningsLabel.text = MoneyManager.FormatAmount(this.moneyEarnedByPlayer, false, false);
			this.DealerEarningsLabel.text = MoneyManager.FormatAmount(this.moneyEarnedByDealers, false, false);
			this.XPGainedLabel.text = this.xpGained.ToString() + " XP";
			this.Anim.Play("Daily summary 1");
			this.Canvas.enabled = true;
			this.Container.gameObject.SetActive(true);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			base.StartCoroutine(CS$<>8__locals1.<Open>g__Wait|0());
		}

		// Token: 0x0600454A RID: 17738 RVA: 0x00122DA3 File Offset: 0x00120FA3
		public void Close()
		{
			if (!this.IsOpen)
			{
				return;
			}
			this.IsOpen = false;
			this.Anim.Stop();
			this.Anim.Play("Daily summary close");
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
		}

		// Token: 0x0600454B RID: 17739 RVA: 0x00122DE1 File Offset: 0x00120FE1
		private void SleepEnd()
		{
			this.ClearStats();
		}

		// Token: 0x0600454C RID: 17740 RVA: 0x00122DEC File Offset: 0x00120FEC
		[ObserversRpc]
		public void AddSoldItem(string id, int amount)
		{
			this.RpcWriter___Observers_AddSoldItem_3643459082(id, amount);
		}

		// Token: 0x0600454D RID: 17741 RVA: 0x00122E07 File Offset: 0x00121007
		[ObserversRpc]
		public void AddPlayerMoney(float amount)
		{
			this.RpcWriter___Observers_AddPlayerMoney_431000436(amount);
		}

		// Token: 0x0600454E RID: 17742 RVA: 0x00122E13 File Offset: 0x00121013
		[ObserversRpc]
		public void AddDealerMoney(float amount)
		{
			this.RpcWriter___Observers_AddDealerMoney_431000436(amount);
		}

		// Token: 0x0600454F RID: 17743 RVA: 0x00122E1F File Offset: 0x0012101F
		[ObserversRpc]
		public void AddXP(int xp)
		{
			this.RpcWriter___Observers_AddXP_3316948804(xp);
		}

		// Token: 0x06004550 RID: 17744 RVA: 0x00122E2B File Offset: 0x0012102B
		private void ClearStats()
		{
			this.itemsSoldByPlayer.Clear();
			this.moneyEarnedByPlayer = 0f;
			this.moneyEarnedByDealers = 0f;
			this.xpGained = 0;
		}

		// Token: 0x06004552 RID: 17746 RVA: 0x00122E68 File Offset: 0x00121068
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.UI.DailySummaryAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.UI.DailySummaryAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(0U, new ClientRpcDelegate(this.RpcReader___Observers_AddSoldItem_3643459082));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_AddPlayerMoney_431000436));
			base.RegisterObserversRpc(2U, new ClientRpcDelegate(this.RpcReader___Observers_AddDealerMoney_431000436));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_AddXP_3316948804));
		}

		// Token: 0x06004553 RID: 17747 RVA: 0x00122EE8 File Offset: 0x001210E8
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.UI.DailySummaryAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.UI.DailySummaryAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06004554 RID: 17748 RVA: 0x00122F01 File Offset: 0x00121101
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06004555 RID: 17749 RVA: 0x00122F10 File Offset: 0x00121110
		private void RpcWriter___Observers_AddSoldItem_3643459082(string id, int amount)
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
			writer.WriteInt32(amount, 1);
			base.SendObserversRpc(0U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06004556 RID: 17750 RVA: 0x00122FD8 File Offset: 0x001211D8
		public void RpcLogic___AddSoldItem_3643459082(string id, int amount)
		{
			if (this.itemsSoldByPlayer.ContainsKey(id))
			{
				Dictionary<string, int> dictionary = this.itemsSoldByPlayer;
				dictionary[id] += amount;
				return;
			}
			this.itemsSoldByPlayer.Add(id, amount);
		}

		// Token: 0x06004557 RID: 17751 RVA: 0x0012301C File Offset: 0x0012121C
		private void RpcReader___Observers_AddSoldItem_3643459082(PooledReader PooledReader0, Channel channel)
		{
			string id = PooledReader0.ReadString();
			int amount = PooledReader0.ReadInt32(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___AddSoldItem_3643459082(id, amount);
		}

		// Token: 0x06004558 RID: 17752 RVA: 0x00123064 File Offset: 0x00121264
		private void RpcWriter___Observers_AddPlayerMoney_431000436(float amount)
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
			writer.WriteSingle(amount, 0);
			base.SendObserversRpc(1U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06004559 RID: 17753 RVA: 0x0012311F File Offset: 0x0012131F
		public void RpcLogic___AddPlayerMoney_431000436(float amount)
		{
			this.moneyEarnedByPlayer += amount;
		}

		// Token: 0x0600455A RID: 17754 RVA: 0x00123130 File Offset: 0x00121330
		private void RpcReader___Observers_AddPlayerMoney_431000436(PooledReader PooledReader0, Channel channel)
		{
			float amount = PooledReader0.ReadSingle(0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___AddPlayerMoney_431000436(amount);
		}

		// Token: 0x0600455B RID: 17755 RVA: 0x00123168 File Offset: 0x00121368
		private void RpcWriter___Observers_AddDealerMoney_431000436(float amount)
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
			writer.WriteSingle(amount, 0);
			base.SendObserversRpc(2U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600455C RID: 17756 RVA: 0x00123223 File Offset: 0x00121423
		public void RpcLogic___AddDealerMoney_431000436(float amount)
		{
			this.moneyEarnedByDealers += amount;
		}

		// Token: 0x0600455D RID: 17757 RVA: 0x00123234 File Offset: 0x00121434
		private void RpcReader___Observers_AddDealerMoney_431000436(PooledReader PooledReader0, Channel channel)
		{
			float amount = PooledReader0.ReadSingle(0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___AddDealerMoney_431000436(amount);
		}

		// Token: 0x0600455E RID: 17758 RVA: 0x0012326C File Offset: 0x0012146C
		private void RpcWriter___Observers_AddXP_3316948804(int xp)
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
			writer.WriteInt32(xp, 1);
			base.SendObserversRpc(3U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600455F RID: 17759 RVA: 0x00123327 File Offset: 0x00121527
		public void RpcLogic___AddXP_3316948804(int xp)
		{
			this.xpGained += xp;
		}

		// Token: 0x06004560 RID: 17760 RVA: 0x00123338 File Offset: 0x00121538
		private void RpcReader___Observers_AddXP_3316948804(PooledReader PooledReader0, Channel channel)
		{
			int xp = PooledReader0.ReadInt32(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___AddXP_3316948804(xp);
		}

		// Token: 0x06004561 RID: 17761 RVA: 0x0012336E File Offset: 0x0012156E
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400320A RID: 12810
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x0400320B RID: 12811
		public RectTransform Container;

		// Token: 0x0400320C RID: 12812
		public Animation Anim;

		// Token: 0x0400320D RID: 12813
		public TextMeshProUGUI TitleLabel;

		// Token: 0x0400320E RID: 12814
		public RectTransform[] ProductEntries;

		// Token: 0x0400320F RID: 12815
		public TextMeshProUGUI PlayerEarningsLabel;

		// Token: 0x04003210 RID: 12816
		public TextMeshProUGUI DealerEarningsLabel;

		// Token: 0x04003211 RID: 12817
		public TextMeshProUGUI XPGainedLabel;

		// Token: 0x04003212 RID: 12818
		public UnityEvent onClosed;

		// Token: 0x04003213 RID: 12819
		private Dictionary<string, int> itemsSoldByPlayer = new Dictionary<string, int>();

		// Token: 0x04003214 RID: 12820
		private float moneyEarnedByPlayer;

		// Token: 0x04003215 RID: 12821
		private float moneyEarnedByDealers;

		// Token: 0x04003217 RID: 12823
		private bool dll_Excuted;

		// Token: 0x04003218 RID: 12824
		private bool dll_Excuted;
	}
}
