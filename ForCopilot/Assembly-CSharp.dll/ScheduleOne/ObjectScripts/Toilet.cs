using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.EntityFramework;
using ScheduleOne.Interaction;
using ScheduleOne.Trash;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000BF2 RID: 3058
	public class Toilet : GridItem
	{
		// Token: 0x060051DA RID: 20954 RVA: 0x00159F14 File Offset: 0x00158114
		public void Hovered()
		{
			if (!this.isFlushing)
			{
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				this.IntObj.SetMessage("Flush");
				return;
			}
			this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
		}

		// Token: 0x060051DB RID: 20955 RVA: 0x00159F47 File Offset: 0x00158147
		public void Interacted()
		{
			this.isFlushing = true;
			this.SendFlush();
		}

		// Token: 0x060051DC RID: 20956 RVA: 0x00159F56 File Offset: 0x00158156
		[ServerRpc(RequireOwnership = false)]
		private void SendFlush()
		{
			this.RpcWriter___Server_SendFlush_2166136261();
		}

		// Token: 0x060051DD RID: 20957 RVA: 0x00159F5E File Offset: 0x0015815E
		[ObserversRpc]
		private void Flush()
		{
			this.RpcWriter___Observers_Flush_2166136261();
		}

		// Token: 0x060051DF RID: 20959 RVA: 0x00159F84 File Offset: 0x00158184
		[CompilerGenerated]
		private IEnumerator <Flush>g__Routine|11_0()
		{
			if (this.OnFlush != null)
			{
				this.OnFlush.Invoke();
			}
			yield return new WaitForSeconds(this.InitialDelay);
			float checkRate = 0.5f;
			int reps = (int)(this.FlushTime / checkRate);
			int j;
			for (int i = 0; i < reps; i = j + 1)
			{
				if (InstanceFinder.IsServer)
				{
					Collider[] array = Physics.OverlapSphere(this.ItemDetectionCollider.transform.position, this.ItemDetectionCollider.radius, this.ItemLayerMask);
					List<TrashItem> list = new List<TrashItem>();
					Collider[] array2 = array;
					for (j = 0; j < array2.Length; j++)
					{
						TrashItem componentInParent = array2[j].GetComponentInParent<TrashItem>();
						if (componentInParent != null && !list.Contains(componentInParent))
						{
							list.Add(componentInParent);
						}
					}
					if (list.Count > 0)
					{
						foreach (TrashItem trashItem in list)
						{
							trashItem.DestroyTrash();
						}
					}
				}
				yield return new WaitForSeconds(checkRate);
				j = i;
			}
			this._flushCoroutine = null;
			this.isFlushing = false;
			yield break;
		}

		// Token: 0x060051E0 RID: 20960 RVA: 0x00159F94 File Offset: 0x00158194
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.ToiletAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.ToiletAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(8U, new ServerRpcDelegate(this.RpcReader___Server_SendFlush_2166136261));
			base.RegisterObserversRpc(9U, new ClientRpcDelegate(this.RpcReader___Observers_Flush_2166136261));
		}

		// Token: 0x060051E1 RID: 20961 RVA: 0x00159FE6 File Offset: 0x001581E6
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.ToiletAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.ToiletAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x060051E2 RID: 20962 RVA: 0x00159FFF File Offset: 0x001581FF
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060051E3 RID: 20963 RVA: 0x0015A010 File Offset: 0x00158210
		private void RpcWriter___Server_SendFlush_2166136261()
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
			base.SendServerRpc(8U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060051E4 RID: 20964 RVA: 0x0015A0AA File Offset: 0x001582AA
		private void RpcLogic___SendFlush_2166136261()
		{
			this.Flush();
		}

		// Token: 0x060051E5 RID: 20965 RVA: 0x0015A0B4 File Offset: 0x001582B4
		private void RpcReader___Server_SendFlush_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendFlush_2166136261();
		}

		// Token: 0x060051E6 RID: 20966 RVA: 0x0015A0D4 File Offset: 0x001582D4
		private void RpcWriter___Observers_Flush_2166136261()
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
			base.SendObserversRpc(9U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060051E7 RID: 20967 RVA: 0x0015A17D File Offset: 0x0015837D
		private void RpcLogic___Flush_2166136261()
		{
			this.isFlushing = true;
			this._flushCoroutine = base.StartCoroutine(this.<Flush>g__Routine|11_0());
		}

		// Token: 0x060051E8 RID: 20968 RVA: 0x0015A198 File Offset: 0x00158398
		private void RpcReader___Observers_Flush_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___Flush_2166136261();
		}

		// Token: 0x060051E9 RID: 20969 RVA: 0x0015A1B8 File Offset: 0x001583B8
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04003D5D RID: 15709
		public float InitialDelay = 0.5f;

		// Token: 0x04003D5E RID: 15710
		public float FlushTime = 5f;

		// Token: 0x04003D5F RID: 15711
		public InteractableObject IntObj;

		// Token: 0x04003D60 RID: 15712
		public LayerMask ItemLayerMask;

		// Token: 0x04003D61 RID: 15713
		public SphereCollider ItemDetectionCollider;

		// Token: 0x04003D62 RID: 15714
		public UnityEvent OnFlush;

		// Token: 0x04003D63 RID: 15715
		private Coroutine _flushCoroutine;

		// Token: 0x04003D64 RID: 15716
		private bool isFlushing;

		// Token: 0x04003D65 RID: 15717
		private bool dll_Excuted;

		// Token: 0x04003D66 RID: 15718
		private bool dll_Excuted;
	}
}
