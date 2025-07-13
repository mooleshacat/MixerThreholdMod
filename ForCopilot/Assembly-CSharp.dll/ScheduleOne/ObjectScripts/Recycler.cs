using System;
using System.Collections;
using System.Collections.Generic;
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
using ScheduleOne.GameTime;
using ScheduleOne.Interaction;
using ScheduleOne.Misc;
using ScheduleOne.Money;
using ScheduleOne.Trash;
using ScheduleOne.Variables;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000BE5 RID: 3045
	public class Recycler : NetworkBehaviour
	{
		// Token: 0x17000B15 RID: 2837
		// (get) Token: 0x0600510A RID: 20746 RVA: 0x00156932 File Offset: 0x00154B32
		// (set) Token: 0x0600510B RID: 20747 RVA: 0x0015693A File Offset: 0x00154B3A
		public Recycler.EState State { get; protected set; }

		// Token: 0x17000B16 RID: 2838
		// (get) Token: 0x0600510C RID: 20748 RVA: 0x00156943 File Offset: 0x00154B43
		// (set) Token: 0x0600510D RID: 20749 RVA: 0x0015694B File Offset: 0x00154B4B
		public bool IsHatchOpen { get; private set; }

		// Token: 0x0600510E RID: 20750 RVA: 0x00156954 File Offset: 0x00154B54
		public void Start()
		{
			this.HandleIntObj.onInteractStart.AddListener(new UnityAction(this.HandleInteracted));
			this.ButtonIntObj.onInteractStart.AddListener(new UnityAction(this.ButtonInteracted));
			this.CashIntObj.onInteractStart.AddListener(new UnityAction(this.CashInteracted));
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
		}

		// Token: 0x0600510F RID: 20751 RVA: 0x001569DB File Offset: 0x00154BDB
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			this.SetState(connection, this.State, true);
		}

		// Token: 0x06005110 RID: 20752 RVA: 0x001569F2 File Offset: 0x00154BF2
		private void OnDestroy()
		{
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			}
		}

		// Token: 0x06005111 RID: 20753 RVA: 0x00156A24 File Offset: 0x00154C24
		private void MinPass()
		{
			if (this.State == Recycler.EState.HatchOpen)
			{
				this.OpenHatchInstruction.gameObject.SetActive(false);
				this.InsertTrashInstruction.gameObject.SetActive(false);
				this.PressBeginInstruction.gameObject.SetActive(false);
				this.ProcessingScreen.gameObject.SetActive(false);
				if (this.GetTrash().Length != 0)
				{
					this.ButtonIntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
					this.ButtonLight.isOn = true;
					this.PressBeginInstruction.gameObject.SetActive(true);
					return;
				}
				this.ButtonIntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
				this.ButtonLight.isOn = false;
				this.InsertTrashInstruction.gameObject.SetActive(true);
			}
		}

		// Token: 0x06005112 RID: 20754 RVA: 0x00156ADD File Offset: 0x00154CDD
		public void HandleInteracted()
		{
			this.SendState(Recycler.EState.HatchOpen);
		}

		// Token: 0x06005113 RID: 20755 RVA: 0x00156AE8 File Offset: 0x00154CE8
		public void ButtonInteracted()
		{
			this.ProcessingLabel.text = "Processing...";
			this.ValueLabel.text = MoneyManager.FormatAmount(0f, false, false);
			this.PressSound.Play();
			this.SendState(Recycler.EState.Processing);
			base.StartCoroutine(this.Process(true));
		}

		// Token: 0x06005114 RID: 20756 RVA: 0x00156B3C File Offset: 0x00154D3C
		public void CashInteracted()
		{
			NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(this.cashValue, true, false);
			NetworkSingleton<MoneyManager>.Instance.ChangeLifetimeEarnings(this.cashValue);
			this.SendState(Recycler.EState.HatchClosed);
			this.BankNote.gameObject.SetActive(false);
			this.cashValue = 0f;
			this.SendCashCollected();
		}

		// Token: 0x06005115 RID: 20757 RVA: 0x00156B94 File Offset: 0x00154D94
		[ServerRpc(RequireOwnership = false)]
		private void SendCashCollected()
		{
			this.RpcWriter___Server_SendCashCollected_2166136261();
		}

		// Token: 0x06005116 RID: 20758 RVA: 0x00156B9C File Offset: 0x00154D9C
		[ObserversRpc(RunLocally = true)]
		private void CashCollected()
		{
			this.RpcWriter___Observers_CashCollected_2166136261();
			this.RpcLogic___CashCollected_2166136261();
		}

		// Token: 0x06005117 RID: 20759 RVA: 0x00156BAA File Offset: 0x00154DAA
		[ObserversRpc(RunLocally = true)]
		private void EnableCash()
		{
			this.RpcWriter___Observers_EnableCash_2166136261();
			this.RpcLogic___EnableCash_2166136261();
		}

		// Token: 0x06005118 RID: 20760 RVA: 0x00156BB8 File Offset: 0x00154DB8
		[ObserversRpc(RunLocally = true)]
		private void SetCashValue(float amount)
		{
			this.RpcWriter___Observers_SetCashValue_431000436(amount);
			this.RpcLogic___SetCashValue_431000436(amount);
		}

		// Token: 0x06005119 RID: 20761 RVA: 0x00156BCE File Offset: 0x00154DCE
		private IEnumerator Process(bool startedByLocalPlayer)
		{
			yield return new WaitForSeconds(0.5f);
			if (this.onStart != null)
			{
				this.onStart.Invoke();
			}
			TrashItem[] trash = this.GetTrash();
			if (startedByLocalPlayer)
			{
				int num = trash.Length;
				float num2 = NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("TrashRecycled") + (float)num;
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("TrashRecycled", num2.ToString(), true);
				if (num2 >= 500f)
				{
					Singleton<AchievementManager>.Instance.UnlockAchievement(AchievementManager.EAchievement.UPSTANDING_CITIZEN);
				}
			}
			float value = 0f;
			TrashItem[] array = trash;
			int j = 0;
			while (j < array.Length)
			{
				TrashItem trashItem = array[j];
				if (trashItem is TrashBag)
				{
					using (List<TrashContent.Entry>.Enumerator enumerator = ((TrashBag)trashItem).Content.Entries.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							TrashContent.Entry entry = enumerator.Current;
							value += (float)(entry.UnitValue * entry.Quantity);
						}
						goto IL_14A;
					}
					goto IL_135;
				}
				goto IL_135;
				IL_14A:
				if (InstanceFinder.IsServer)
				{
					trashItem.DestroyTrash();
				}
				j++;
				continue;
				IL_135:
				value += (float)trashItem.SellValue;
				goto IL_14A;
			}
			if (this.cashValue <= 0f)
			{
				this.SetCashValue(value);
			}
			float lerpTime = 1.5f;
			for (float i = 0f; i < lerpTime; i += Time.deltaTime)
			{
				float t = i / lerpTime;
				float amount = Mathf.Lerp(0f, this.cashValue, t);
				this.ValueLabel.text = MoneyManager.FormatAmount(amount, true, false);
				yield return new WaitForEndOfFrame();
			}
			if (this.onStop != null)
			{
				this.onStop.Invoke();
			}
			this.ProcessingLabel.text = "Thank you";
			this.ValueLabel.text = MoneyManager.FormatAmount(value, false, false);
			this.DoneSound.Play();
			yield return new WaitForSeconds(0.3f);
			this.CashEjectSound.Play();
			this.CashAnim.Play();
			yield return new WaitForSeconds(0.25f);
			if (InstanceFinder.IsServer)
			{
				this.EnableCash();
			}
			yield break;
		}

		// Token: 0x0600511A RID: 20762 RVA: 0x00156BE4 File Offset: 0x00154DE4
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendState(Recycler.EState state)
		{
			this.RpcWriter___Server_SendState_3569965459(state);
			this.RpcLogic___SendState_3569965459(state);
		}

		// Token: 0x0600511B RID: 20763 RVA: 0x00156BFC File Offset: 0x00154DFC
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		private void SetState(NetworkConnection conn, Recycler.EState state, bool force = false)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetState_3790170803(conn, state, force);
				this.RpcLogic___SetState_3790170803(conn, state, force);
			}
			else
			{
				this.RpcWriter___Target_SetState_3790170803(conn, state, force);
			}
		}

		// Token: 0x0600511C RID: 20764 RVA: 0x00156C4C File Offset: 0x00154E4C
		private void SetHatchOpen(bool open)
		{
			if (open == this.IsHatchOpen)
			{
				return;
			}
			this.IsHatchOpen = open;
			if (this.IsHatchOpen)
			{
				this.OpenSound.Play();
				this.HatchAnim.Play("Recycler open");
				return;
			}
			this.CloseSound.Play();
			this.HatchAnim.Play("Recycler close");
		}

		// Token: 0x0600511D RID: 20765 RVA: 0x00156CAC File Offset: 0x00154EAC
		private TrashItem[] GetTrash()
		{
			List<TrashItem> list = new List<TrashItem>();
			Vector3 vector = this.CheckCollider.transform.TransformPoint(this.CheckCollider.center);
			Vector3 vector2 = Vector3.Scale(this.CheckCollider.size, this.CheckCollider.transform.lossyScale) * 0.5f;
			Collider[] array = Physics.OverlapBox(vector, vector2, this.CheckCollider.transform.rotation, this.DetectionMask, 2);
			for (int i = 0; i < array.Length; i++)
			{
				TrashItem componentInParent = array[i].GetComponentInParent<TrashItem>();
				if (componentInParent != null && !list.Contains(componentInParent))
				{
					list.Add(componentInParent);
				}
			}
			return list.ToArray();
		}

		// Token: 0x0600511E RID: 20766 RVA: 0x00156D64 File Offset: 0x00154F64
		private void OnDrawGizmos()
		{
			Vector3 center = this.CheckCollider.transform.TransformPoint(this.CheckCollider.center);
			Vector3 a = Vector3.Scale(this.CheckCollider.size, this.CheckCollider.transform.lossyScale) * 0.5f;
			Gizmos.color = Color.red;
			Gizmos.DrawWireCube(center, a * 2f);
		}

		// Token: 0x06005120 RID: 20768 RVA: 0x00156DD4 File Offset: 0x00154FD4
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.RecyclerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.RecyclerAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendCashCollected_2166136261));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_CashCollected_2166136261));
			base.RegisterObserversRpc(2U, new ClientRpcDelegate(this.RpcReader___Observers_EnableCash_2166136261));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_SetCashValue_431000436));
			base.RegisterServerRpc(4U, new ServerRpcDelegate(this.RpcReader___Server_SendState_3569965459));
			base.RegisterObserversRpc(5U, new ClientRpcDelegate(this.RpcReader___Observers_SetState_3790170803));
			base.RegisterTargetRpc(6U, new ClientRpcDelegate(this.RpcReader___Target_SetState_3790170803));
		}

		// Token: 0x06005121 RID: 20769 RVA: 0x00156E93 File Offset: 0x00155093
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.RecyclerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.RecyclerAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06005122 RID: 20770 RVA: 0x00156EA6 File Offset: 0x001550A6
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06005123 RID: 20771 RVA: 0x00156EB4 File Offset: 0x001550B4
		private void RpcWriter___Server_SendCashCollected_2166136261()
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
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06005124 RID: 20772 RVA: 0x00156F4E File Offset: 0x0015514E
		private void RpcLogic___SendCashCollected_2166136261()
		{
			this.CashCollected();
		}

		// Token: 0x06005125 RID: 20773 RVA: 0x00156F58 File Offset: 0x00155158
		private void RpcReader___Server_SendCashCollected_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendCashCollected_2166136261();
		}

		// Token: 0x06005126 RID: 20774 RVA: 0x00156F78 File Offset: 0x00155178
		private void RpcWriter___Observers_CashCollected_2166136261()
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
			base.SendObserversRpc(1U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06005127 RID: 20775 RVA: 0x00157021 File Offset: 0x00155221
		private void RpcLogic___CashCollected_2166136261()
		{
			this.SendState(Recycler.EState.HatchClosed);
			this.BankNote.gameObject.SetActive(false);
			this.cashValue = 0f;
		}

		// Token: 0x06005128 RID: 20776 RVA: 0x00157048 File Offset: 0x00155248
		private void RpcReader___Observers_CashCollected_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___CashCollected_2166136261();
		}

		// Token: 0x06005129 RID: 20777 RVA: 0x00157074 File Offset: 0x00155274
		private void RpcWriter___Observers_EnableCash_2166136261()
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
			base.SendObserversRpc(2U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600512A RID: 20778 RVA: 0x0015711D File Offset: 0x0015531D
		private void RpcLogic___EnableCash_2166136261()
		{
			this.CashIntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
		}

		// Token: 0x0600512B RID: 20779 RVA: 0x0015712C File Offset: 0x0015532C
		private void RpcReader___Observers_EnableCash_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___EnableCash_2166136261();
		}

		// Token: 0x0600512C RID: 20780 RVA: 0x00157158 File Offset: 0x00155358
		private void RpcWriter___Observers_SetCashValue_431000436(float amount)
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
			base.SendObserversRpc(3U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600512D RID: 20781 RVA: 0x00157213 File Offset: 0x00155413
		private void RpcLogic___SetCashValue_431000436(float amount)
		{
			this.cashValue = amount;
		}

		// Token: 0x0600512E RID: 20782 RVA: 0x0015721C File Offset: 0x0015541C
		private void RpcReader___Observers_SetCashValue_431000436(PooledReader PooledReader0, Channel channel)
		{
			float amount = PooledReader0.ReadSingle(0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetCashValue_431000436(amount);
		}

		// Token: 0x0600512F RID: 20783 RVA: 0x0015725C File Offset: 0x0015545C
		private void RpcWriter___Server_SendState_3569965459(Recycler.EState state)
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
			writer.Write___ScheduleOne.ObjectScripts.Recycler/EStateFishNet.Serializing.Generated(state);
			base.SendServerRpc(4U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06005130 RID: 20784 RVA: 0x00157303 File Offset: 0x00155503
		public void RpcLogic___SendState_3569965459(Recycler.EState state)
		{
			this.SetState(null, state, false);
		}

		// Token: 0x06005131 RID: 20785 RVA: 0x00157310 File Offset: 0x00155510
		private void RpcReader___Server_SendState_3569965459(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			Recycler.EState state = GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.Recycler/EStateFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendState_3569965459(state);
		}

		// Token: 0x06005132 RID: 20786 RVA: 0x00157350 File Offset: 0x00155550
		private void RpcWriter___Observers_SetState_3790170803(NetworkConnection conn, Recycler.EState state, bool force = false)
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
			writer.Write___ScheduleOne.ObjectScripts.Recycler/EStateFishNet.Serializing.Generated(state);
			writer.WriteBoolean(force);
			base.SendObserversRpc(5U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06005133 RID: 20787 RVA: 0x00157414 File Offset: 0x00155614
		private void RpcLogic___SetState_3790170803(NetworkConnection conn, Recycler.EState state, bool force = false)
		{
			if (this.State == state && !force)
			{
				return;
			}
			this.State = state;
			this.HandleIntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
			this.ButtonIntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
			this.CashIntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
			this.OpenHatchInstruction.gameObject.SetActive(false);
			this.InsertTrashInstruction.gameObject.SetActive(false);
			this.PressBeginInstruction.gameObject.SetActive(false);
			this.ProcessingScreen.gameObject.SetActive(false);
			this.ButtonLight.isOn = false;
			this.Cash.gameObject.SetActive(false);
			switch (this.State)
			{
			case Recycler.EState.HatchClosed:
				this.HandleIntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				this.OpenHatchInstruction.gameObject.SetActive(true);
				return;
			case Recycler.EState.HatchOpen:
				if (this.GetTrash().Length != 0)
				{
					this.ButtonIntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
					this.ButtonLight.isOn = true;
					this.PressBeginInstruction.gameObject.SetActive(true);
				}
				else
				{
					this.InsertTrashInstruction.gameObject.SetActive(true);
				}
				this.SetHatchOpen(true);
				return;
			case Recycler.EState.Processing:
				base.StartCoroutine(this.Process(false));
				this.ProcessingScreen.gameObject.SetActive(true);
				this.ButtonAnim.Play();
				this.SetHatchOpen(false);
				return;
			default:
				return;
			}
		}

		// Token: 0x06005134 RID: 20788 RVA: 0x00157574 File Offset: 0x00155774
		private void RpcReader___Observers_SetState_3790170803(PooledReader PooledReader0, Channel channel)
		{
			Recycler.EState state = GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.Recycler/EStateFishNet.Serializing.Generateds(PooledReader0);
			bool force = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetState_3790170803(null, state, force);
		}

		// Token: 0x06005135 RID: 20789 RVA: 0x001575C4 File Offset: 0x001557C4
		private void RpcWriter___Target_SetState_3790170803(NetworkConnection conn, Recycler.EState state, bool force = false)
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
			writer.Write___ScheduleOne.ObjectScripts.Recycler/EStateFishNet.Serializing.Generated(state);
			writer.WriteBoolean(force);
			base.SendTargetRpc(6U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x06005136 RID: 20790 RVA: 0x00157688 File Offset: 0x00155888
		private void RpcReader___Target_SetState_3790170803(PooledReader PooledReader0, Channel channel)
		{
			Recycler.EState state = GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.Recycler/EStateFishNet.Serializing.Generateds(PooledReader0);
			bool force = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetState_3790170803(base.LocalConnection, state, force);
		}

		// Token: 0x06005137 RID: 20791 RVA: 0x00156EA6 File Offset: 0x001550A6
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04003CD2 RID: 15570
		public LayerMask DetectionMask;

		// Token: 0x04003CD3 RID: 15571
		[Header("References")]
		public InteractableObject HandleIntObj;

		// Token: 0x04003CD4 RID: 15572
		public InteractableObject ButtonIntObj;

		// Token: 0x04003CD5 RID: 15573
		public InteractableObject CashIntObj;

		// Token: 0x04003CD6 RID: 15574
		public ToggleableLight ButtonLight;

		// Token: 0x04003CD7 RID: 15575
		public Animation ButtonAnim;

		// Token: 0x04003CD8 RID: 15576
		public Animation HatchAnim;

		// Token: 0x04003CD9 RID: 15577
		public Animation CashAnim;

		// Token: 0x04003CDA RID: 15578
		public RectTransform OpenHatchInstruction;

		// Token: 0x04003CDB RID: 15579
		public RectTransform InsertTrashInstruction;

		// Token: 0x04003CDC RID: 15580
		public RectTransform PressBeginInstruction;

		// Token: 0x04003CDD RID: 15581
		public RectTransform ProcessingScreen;

		// Token: 0x04003CDE RID: 15582
		public TextMeshProUGUI ProcessingLabel;

		// Token: 0x04003CDF RID: 15583
		public TextMeshProUGUI ValueLabel;

		// Token: 0x04003CE0 RID: 15584
		public BoxCollider CheckCollider;

		// Token: 0x04003CE1 RID: 15585
		public Transform Cash;

		// Token: 0x04003CE2 RID: 15586
		public GameObject BankNote;

		// Token: 0x04003CE3 RID: 15587
		[Header("Sound")]
		public AudioSourceController OpenSound;

		// Token: 0x04003CE4 RID: 15588
		public AudioSourceController CloseSound;

		// Token: 0x04003CE5 RID: 15589
		public AudioSourceController PressSound;

		// Token: 0x04003CE6 RID: 15590
		public AudioSourceController DoneSound;

		// Token: 0x04003CE7 RID: 15591
		public AudioSourceController CashEjectSound;

		// Token: 0x04003CE8 RID: 15592
		private float cashValue;

		// Token: 0x04003CE9 RID: 15593
		public UnityEvent onStart;

		// Token: 0x04003CEA RID: 15594
		public UnityEvent onStop;

		// Token: 0x04003CEB RID: 15595
		private bool dll_Excuted;

		// Token: 0x04003CEC RID: 15596
		private bool dll_Excuted;

		// Token: 0x02000BE6 RID: 3046
		public enum EState
		{
			// Token: 0x04003CEE RID: 15598
			HatchClosed,
			// Token: 0x04003CEF RID: 15599
			HatchOpen,
			// Token: 0x04003CF0 RID: 15600
			Processing
		}
	}
}
