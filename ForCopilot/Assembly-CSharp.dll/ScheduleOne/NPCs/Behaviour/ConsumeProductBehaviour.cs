using System;
using System.Collections;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Audio;
using ScheduleOne.AvatarFramework.Equipping;
using ScheduleOne.GameTime;
using ScheduleOne.ItemFramework;
using ScheduleOne.Product;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000524 RID: 1316
	public class ConsumeProductBehaviour : Behaviour
	{
		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x06001D9C RID: 7580 RVA: 0x0007B52E File Offset: 0x0007972E
		// (set) Token: 0x06001D9D RID: 7581 RVA: 0x0007B536 File Offset: 0x00079736
		public ProductItemInstance ConsumedProduct { get; private set; }

		// Token: 0x06001D9E RID: 7582 RVA: 0x0007B540 File Offset: 0x00079740
		protected virtual void Start()
		{
			TimeManager.onSleepEnd = (Action<int>)Delegate.Remove(TimeManager.onSleepEnd, new Action<int>(this.DayPass));
			TimeManager.onSleepEnd = (Action<int>)Delegate.Combine(TimeManager.onSleepEnd, new Action<int>(this.DayPass));
			if (this.TestProduct != null && Application.isEditor)
			{
				this.product = (this.TestProduct.GetDefaultInstance(1) as ProductItemInstance);
			}
		}

		// Token: 0x06001D9F RID: 7583 RVA: 0x0007B5B9 File Offset: 0x000797B9
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendProduct(ProductItemInstance _product)
		{
			this.RpcWriter___Server_SendProduct_2622925554(_product);
			this.RpcLogic___SendProduct_2622925554(_product);
		}

		// Token: 0x06001DA0 RID: 7584 RVA: 0x0007B5CF File Offset: 0x000797CF
		[ObserversRpc(RunLocally = true)]
		public void SetProduct(ProductItemInstance _product)
		{
			this.RpcWriter___Observers_SetProduct_2622925554(_product);
			this.RpcLogic___SetProduct_2622925554(_product);
		}

		// Token: 0x06001DA1 RID: 7585 RVA: 0x0007B5E5 File Offset: 0x000797E5
		[ObserversRpc(RunLocally = true)]
		public void ClearEffects()
		{
			this.RpcWriter___Observers_ClearEffects_2166136261();
			this.RpcLogic___ClearEffects_2166136261();
		}

		// Token: 0x06001DA2 RID: 7586 RVA: 0x0007B5F3 File Offset: 0x000797F3
		protected override void Begin()
		{
			base.Begin();
			this.TryConsume();
		}

		// Token: 0x06001DA3 RID: 7587 RVA: 0x0007B601 File Offset: 0x00079801
		protected override void Resume()
		{
			base.Resume();
			this.TryConsume();
		}

		// Token: 0x06001DA4 RID: 7588 RVA: 0x0007B610 File Offset: 0x00079810
		private void TryConsume()
		{
			if (this.product == null)
			{
				Console.LogError("No product to consume", null);
				this.Disable();
				return;
			}
			switch ((this.product.Definition as ProductDefinition).DrugType)
			{
			case EDrugType.Marijuana:
				this.ConsumeWeed();
				return;
			case EDrugType.Methamphetamine:
				this.ConsumeMeth();
				return;
			case EDrugType.Cocaine:
				this.ConsumeCocaine();
				return;
			default:
				return;
			}
		}

		// Token: 0x06001DA5 RID: 7589 RVA: 0x0007B674 File Offset: 0x00079874
		public override void Disable()
		{
			base.Disable();
			this.Clear();
			this.End();
		}

		// Token: 0x06001DA6 RID: 7590 RVA: 0x0007B688 File Offset: 0x00079888
		protected override void End()
		{
			base.End();
			if (this.consumeRoutine != null)
			{
				base.StopCoroutine(this.consumeRoutine);
				this.consumeRoutine = null;
			}
			base.Npc.SetEquippable_Return(string.Empty);
		}

		// Token: 0x06001DA7 RID: 7591 RVA: 0x0007B6BC File Offset: 0x000798BC
		private void ConsumeWeed()
		{
			this.consumeRoutine = base.StartCoroutine(this.<ConsumeWeed>g__ConsumeWeedRoutine|23_0());
		}

		// Token: 0x06001DA8 RID: 7592 RVA: 0x0007B6D0 File Offset: 0x000798D0
		private void ConsumeMeth()
		{
			this.consumeRoutine = base.StartCoroutine(this.<ConsumeMeth>g__ConsumeWeedRoutine|24_0());
		}

		// Token: 0x06001DA9 RID: 7593 RVA: 0x0007B6E4 File Offset: 0x000798E4
		private void ConsumeCocaine()
		{
			this.consumeRoutine = base.StartCoroutine(this.<ConsumeCocaine>g__ConsumeWeedRoutine|25_0());
		}

		// Token: 0x06001DAA RID: 7594 RVA: 0x0007B6F8 File Offset: 0x000798F8
		[ObserversRpc]
		private void ApplyEffects()
		{
			this.RpcWriter___Observers_ApplyEffects_2166136261();
		}

		// Token: 0x06001DAB RID: 7595 RVA: 0x0007B700 File Offset: 0x00079900
		private void Clear()
		{
			base.Npc.Avatar.Anim.SetBool("Smoking", false);
		}

		// Token: 0x06001DAC RID: 7596 RVA: 0x0007B71D File Offset: 0x0007991D
		private void DayPass(int minsSlept)
		{
			if (this.ConsumedProduct != null)
			{
				this.ClearEffects();
			}
		}

		// Token: 0x06001DAE RID: 7598 RVA: 0x0007B72D File Offset: 0x0007992D
		[CompilerGenerated]
		private IEnumerator <ConsumeWeed>g__ConsumeWeedRoutine|23_0()
		{
			base.Npc.SetEquippable_Return(this.JointPrefab.AssetPath);
			base.Npc.Avatar.Anim.SetBool("Smoking", true);
			this.WeedConsumeSound.Play();
			yield return new WaitForSeconds(3f);
			this.SmokeExhaleParticles.Play();
			yield return new WaitForSeconds(1.5f);
			base.Npc.Avatar.Anim.SetBool("Smoking", false);
			if (InstanceFinder.IsServer)
			{
				this.ApplyEffects();
				base.Disable_Networked(null);
			}
			if (this.onConsumeDone != null)
			{
				this.onConsumeDone.Invoke();
			}
			yield break;
		}

		// Token: 0x06001DAF RID: 7599 RVA: 0x0007B73C File Offset: 0x0007993C
		[CompilerGenerated]
		private IEnumerator <ConsumeMeth>g__ConsumeWeedRoutine|24_0()
		{
			base.Npc.SetEquippable_Return(this.PipePrefab.AssetPath);
			base.Npc.Avatar.Anim.SetBool("Smoking", true);
			this.MethConsumeSound.Play();
			yield return new WaitForSeconds(3f);
			this.SmokeExhaleParticles.Play();
			yield return new WaitForSeconds(1.5f);
			base.Npc.Avatar.Anim.SetBool("Smoking", false);
			if (InstanceFinder.IsServer)
			{
				this.ApplyEffects();
				base.Disable_Networked(null);
			}
			if (this.onConsumeDone != null)
			{
				this.onConsumeDone.Invoke();
			}
			yield break;
		}

		// Token: 0x06001DB0 RID: 7600 RVA: 0x0007B74B File Offset: 0x0007994B
		[CompilerGenerated]
		private IEnumerator <ConsumeCocaine>g__ConsumeWeedRoutine|25_0()
		{
			base.Npc.Avatar.Anim.SetTrigger("Snort");
			yield return new WaitForSeconds(0.8f);
			this.SnortSound.Play();
			yield return new WaitForSeconds(1f);
			if (InstanceFinder.IsServer)
			{
				this.ApplyEffects();
				base.Disable_Networked(null);
			}
			if (this.onConsumeDone != null)
			{
				this.onConsumeDone.Invoke();
			}
			yield break;
		}

		// Token: 0x06001DB1 RID: 7601 RVA: 0x0007B75C File Offset: 0x0007995C
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.ConsumeProductBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.ConsumeProductBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(15U, new ServerRpcDelegate(this.RpcReader___Server_SendProduct_2622925554));
			base.RegisterObserversRpc(16U, new ClientRpcDelegate(this.RpcReader___Observers_SetProduct_2622925554));
			base.RegisterObserversRpc(17U, new ClientRpcDelegate(this.RpcReader___Observers_ClearEffects_2166136261));
			base.RegisterObserversRpc(18U, new ClientRpcDelegate(this.RpcReader___Observers_ApplyEffects_2166136261));
		}

		// Token: 0x06001DB2 RID: 7602 RVA: 0x0007B7DC File Offset: 0x000799DC
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.ConsumeProductBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.ConsumeProductBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001DB3 RID: 7603 RVA: 0x0007B7F5 File Offset: 0x000799F5
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001DB4 RID: 7604 RVA: 0x0007B804 File Offset: 0x00079A04
		private void RpcWriter___Server_SendProduct_2622925554(ProductItemInstance _product)
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
			writer.WriteProductItemInstance(_product);
			base.SendServerRpc(15U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06001DB5 RID: 7605 RVA: 0x0007B8AB File Offset: 0x00079AAB
		public void RpcLogic___SendProduct_2622925554(ProductItemInstance _product)
		{
			this.SetProduct(_product);
		}

		// Token: 0x06001DB6 RID: 7606 RVA: 0x0007B8B4 File Offset: 0x00079AB4
		private void RpcReader___Server_SendProduct_2622925554(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			ProductItemInstance productItemInstance = PooledReader0.ReadProductItemInstance();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendProduct_2622925554(productItemInstance);
		}

		// Token: 0x06001DB7 RID: 7607 RVA: 0x0007B8F4 File Offset: 0x00079AF4
		private void RpcWriter___Observers_SetProduct_2622925554(ProductItemInstance _product)
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
			writer.WriteProductItemInstance(_product);
			base.SendObserversRpc(16U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06001DB8 RID: 7608 RVA: 0x0007B9AA File Offset: 0x00079BAA
		public void RpcLogic___SetProduct_2622925554(ProductItemInstance _product)
		{
			this.product = _product;
		}

		// Token: 0x06001DB9 RID: 7609 RVA: 0x0007B9B4 File Offset: 0x00079BB4
		private void RpcReader___Observers_SetProduct_2622925554(PooledReader PooledReader0, Channel channel)
		{
			ProductItemInstance productItemInstance = PooledReader0.ReadProductItemInstance();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetProduct_2622925554(productItemInstance);
		}

		// Token: 0x06001DBA RID: 7610 RVA: 0x0007B9F0 File Offset: 0x00079BF0
		private void RpcWriter___Observers_ClearEffects_2166136261()
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
			base.SendObserversRpc(17U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06001DBB RID: 7611 RVA: 0x0007BA99 File Offset: 0x00079C99
		public void RpcLogic___ClearEffects_2166136261()
		{
			if (this.ConsumedProduct == null)
			{
				return;
			}
			this.ConsumedProduct.ClearEffectsFromNPC(base.Npc);
			this.ConsumedProduct = null;
		}

		// Token: 0x06001DBC RID: 7612 RVA: 0x0007BABC File Offset: 0x00079CBC
		private void RpcReader___Observers_ClearEffects_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ClearEffects_2166136261();
		}

		// Token: 0x06001DBD RID: 7613 RVA: 0x0007BAE8 File Offset: 0x00079CE8
		private void RpcWriter___Observers_ApplyEffects_2166136261()
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
			base.SendObserversRpc(18U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06001DBE RID: 7614 RVA: 0x0007BB91 File Offset: 0x00079D91
		private void RpcLogic___ApplyEffects_2166136261()
		{
			if (this.ConsumedProduct != null)
			{
				this.ClearEffects();
			}
			this.ConsumedProduct = this.product;
			if (this.product != null)
			{
				this.product.ApplyEffectsToNPC(base.Npc);
			}
		}

		// Token: 0x06001DBF RID: 7615 RVA: 0x0007BBC8 File Offset: 0x00079DC8
		private void RpcReader___Observers_ApplyEffects_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ApplyEffects_2166136261();
		}

		// Token: 0x06001DC0 RID: 7616 RVA: 0x0007BBE8 File Offset: 0x00079DE8
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040017F3 RID: 6131
		public AvatarEquippable JointPrefab;

		// Token: 0x040017F4 RID: 6132
		public AvatarEquippable PipePrefab;

		// Token: 0x040017F6 RID: 6134
		private ProductItemInstance product;

		// Token: 0x040017F7 RID: 6135
		private Coroutine consumeRoutine;

		// Token: 0x040017F8 RID: 6136
		public AudioSourceController WeedConsumeSound;

		// Token: 0x040017F9 RID: 6137
		public AudioSourceController MethConsumeSound;

		// Token: 0x040017FA RID: 6138
		public AudioSourceController SnortSound;

		// Token: 0x040017FB RID: 6139
		public ParticleSystem SmokeExhaleParticles;

		// Token: 0x040017FC RID: 6140
		[Header("Debug")]
		public ProductDefinition TestProduct;

		// Token: 0x040017FD RID: 6141
		public UnityEvent onConsumeDone;

		// Token: 0x040017FE RID: 6142
		private bool dll_Excuted;

		// Token: 0x040017FF RID: 6143
		private bool dll_Excuted;
	}
}
