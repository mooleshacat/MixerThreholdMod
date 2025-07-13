using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.Interaction;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000C4A RID: 3146
	public class SoilPourer : GridItem
	{
		// Token: 0x17000C5B RID: 3163
		// (get) Token: 0x06005885 RID: 22661 RVA: 0x001765C6 File Offset: 0x001747C6
		// (set) Token: 0x06005886 RID: 22662 RVA: 0x001765CE File Offset: 0x001747CE
		public string SoilID { get; protected set; } = string.Empty;

		// Token: 0x06005887 RID: 22663 RVA: 0x001765D8 File Offset: 0x001747D8
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (this.SoilID != string.Empty)
			{
				SoilDefinition item = Registry.GetItem<SoilDefinition>(this.SoilID);
				this.DirtPlane.material = item.DrySoilMat;
				this.SetSoilLevel(1f);
			}
		}

		// Token: 0x06005888 RID: 22664 RVA: 0x00176626 File Offset: 0x00174826
		public void HandleHovered()
		{
			if (!string.IsNullOrEmpty(this.SoilID) && !this.isDispensing)
			{
				this.HandleIntObj.SetMessage("Dispense soil");
				this.HandleIntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				return;
			}
			this.HandleIntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
		}

		// Token: 0x06005889 RID: 22665 RVA: 0x00176666 File Offset: 0x00174866
		public void HandleInteracted()
		{
			if (!string.IsNullOrEmpty(this.SoilID) && !this.isDispensing)
			{
				this.SendPourSoil();
				this.isDispensing = true;
			}
		}

		// Token: 0x0600588A RID: 22666 RVA: 0x0017668A File Offset: 0x0017488A
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		private void SendPourSoil()
		{
			this.RpcWriter___Server_SendPourSoil_2166136261();
			this.RpcLogic___SendPourSoil_2166136261();
		}

		// Token: 0x0600588B RID: 22667 RVA: 0x00176698 File Offset: 0x00174898
		[ObserversRpc(RunLocally = true)]
		private void PourSoil()
		{
			this.RpcWriter___Observers_PourSoil_2166136261();
			this.RpcLogic___PourSoil_2166136261();
		}

		// Token: 0x0600588C RID: 22668 RVA: 0x001766A8 File Offset: 0x001748A8
		private void ApplySoil(string ID)
		{
			Pot[] array = this.GetPots().ToArray();
			if (array != null && array.Length != 0 && array[0].SoilID == string.Empty)
			{
				array[0].SetSoilID(ID);
				array[0].SetSoilState(Pot.ESoilState.Flat);
				array[0].AddSoil(array[0].SoilCapacity);
				array[0].SetSoilUses(Registry.GetItem<SoilDefinition>(ID).Uses);
				if (InstanceFinder.IsServer)
				{
					array[0].PushSoilDataToServer();
				}
			}
		}

		// Token: 0x0600588D RID: 22669 RVA: 0x00176720 File Offset: 0x00174920
		public void FillHovered()
		{
			bool flag = false;
			if (PlayerSingleton<PlayerInventory>.Instance.isAnythingEquipped && PlayerSingleton<PlayerInventory>.Instance.equippedSlot.ItemInstance.Definition is SoilDefinition)
			{
				flag = true;
			}
			if (this.SoilID == string.Empty && flag)
			{
				this.FillIntObj.SetMessage("Insert soil");
				this.FillIntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				return;
			}
			this.FillIntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
		}

		// Token: 0x0600588E RID: 22670 RVA: 0x00176798 File Offset: 0x00174998
		public void FillInteracted()
		{
			bool flag = false;
			if (PlayerSingleton<PlayerInventory>.Instance.isAnythingEquipped && PlayerSingleton<PlayerInventory>.Instance.equippedSlot.ItemInstance.Definition is SoilDefinition)
			{
				flag = true;
			}
			if (this.SoilID == string.Empty && flag)
			{
				this.FillSound.Play();
				this.SendSoil(PlayerSingleton<PlayerInventory>.Instance.equippedSlot.ItemInstance.Definition.ID);
				PlayerSingleton<PlayerInventory>.Instance.equippedSlot.ChangeQuantity(-1, false);
			}
		}

		// Token: 0x0600588F RID: 22671 RVA: 0x00176820 File Offset: 0x00174A20
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendSoil(string ID)
		{
			this.RpcWriter___Server_SendSoil_3615296227(ID);
			this.RpcLogic___SendSoil_3615296227(ID);
		}

		// Token: 0x06005890 RID: 22672 RVA: 0x00176838 File Offset: 0x00174A38
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		protected void SetSoil(NetworkConnection conn, string ID)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetSoil_2971853958(conn, ID);
				this.RpcLogic___SetSoil_2971853958(conn, ID);
			}
			else
			{
				this.RpcWriter___Target_SetSoil_2971853958(conn, ID);
			}
		}

		// Token: 0x06005891 RID: 22673 RVA: 0x0017687C File Offset: 0x00174A7C
		public void SetSoilLevel(float level)
		{
			this.DirtPlane.transform.localPosition = Vector3.Lerp(this.Dirt_Min.localPosition, this.Dirt_Max.localPosition, level);
			this.DirtPlane.gameObject.SetActive(level > 0f);
		}

		// Token: 0x06005892 RID: 22674 RVA: 0x001768D0 File Offset: 0x00174AD0
		protected virtual List<Pot> GetPots()
		{
			List<Pot> list = new List<Pot>();
			Coordinate coord = new Coordinate(this.OriginCoordinate) + Coordinate.RotateCoordinates(new Coordinate(0, 1), (float)this.Rotation);
			Coordinate coord2 = new Coordinate(this.OriginCoordinate) + Coordinate.RotateCoordinates(new Coordinate(1, 1), (float)this.Rotation);
			Tile tile = base.OwnerGrid.GetTile(coord);
			Tile tile2 = base.OwnerGrid.GetTile(coord2);
			if (tile != null && tile2 != null)
			{
				Pot pot = null;
				foreach (GridItem gridItem in tile.BuildableOccupants)
				{
					if (gridItem is Pot)
					{
						pot = (gridItem as Pot);
						break;
					}
				}
				if (pot != null && tile2.BuildableOccupants.Contains(pot))
				{
					list.Add(pot);
				}
			}
			return list;
		}

		// Token: 0x06005893 RID: 22675 RVA: 0x001769D4 File Offset: 0x00174BD4
		public override BuildableItemData GetBaseData()
		{
			return new SoilPourerData(base.GUID, base.ItemInstance, 0, base.OwnerGrid, this.OriginCoordinate, this.Rotation, this.SoilID);
		}

		// Token: 0x06005895 RID: 22677 RVA: 0x00176A1E File Offset: 0x00174C1E
		[CompilerGenerated]
		private IEnumerator <PourSoil>g__PourRoutine|20_0()
		{
			SoilDefinition item = Registry.GetItem<SoilDefinition>(this.SoilID);
			if (item == null)
			{
				Console.LogError("Soil definition not found for ID: " + this.SoilID, null);
				this.isDispensing = false;
				yield break;
			}
			this.ActivateSound.Play();
			this.PourParticles.startColor = item.ParticleColor;
			this.PourParticles.Play();
			this.PourAnimation.Play();
			this.DirtPourSound.Play();
			Pot targetPot = this.GetPots().FirstOrDefault<Pot>();
			if (targetPot != null)
			{
				targetPot.SetSoilID(this.SoilID);
				targetPot.SetSoilState(Pot.ESoilState.Flat);
				targetPot.SetSoilUses(item.Uses);
			}
			for (float i = 0f; i < this.AnimationDuration; i += Time.deltaTime)
			{
				float num = i / this.AnimationDuration;
				this.SetSoilLevel(1f - num);
				if (targetPot != null)
				{
					targetPot.AddSoil(targetPot.SoilCapacity * (Time.deltaTime / this.AnimationDuration));
				}
				yield return new WaitForEndOfFrame();
			}
			if (targetPot != null)
			{
				targetPot.AddSoil(targetPot.SoilCapacity - targetPot.SoilLevel);
			}
			this.ApplySoil(this.SoilID);
			this.SetSoil(null, string.Empty);
			this.PourParticles.Stop();
			this.isDispensing = false;
			yield return new WaitForSeconds(1f);
			this.DirtPourSound.Stop();
			yield break;
		}

		// Token: 0x06005896 RID: 22678 RVA: 0x00176A30 File Offset: 0x00174C30
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.SoilPourerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.SoilPourerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(8U, new ServerRpcDelegate(this.RpcReader___Server_SendPourSoil_2166136261));
			base.RegisterObserversRpc(9U, new ClientRpcDelegate(this.RpcReader___Observers_PourSoil_2166136261));
			base.RegisterServerRpc(10U, new ServerRpcDelegate(this.RpcReader___Server_SendSoil_3615296227));
			base.RegisterObserversRpc(11U, new ClientRpcDelegate(this.RpcReader___Observers_SetSoil_2971853958));
			base.RegisterTargetRpc(12U, new ClientRpcDelegate(this.RpcReader___Target_SetSoil_2971853958));
		}

		// Token: 0x06005897 RID: 22679 RVA: 0x00176AC7 File Offset: 0x00174CC7
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.SoilPourerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.SoilPourerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06005898 RID: 22680 RVA: 0x00176AE0 File Offset: 0x00174CE0
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06005899 RID: 22681 RVA: 0x00176AF0 File Offset: 0x00174CF0
		private void RpcWriter___Server_SendPourSoil_2166136261()
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

		// Token: 0x0600589A RID: 22682 RVA: 0x00176B8A File Offset: 0x00174D8A
		private void RpcLogic___SendPourSoil_2166136261()
		{
			this.PourSoil();
		}

		// Token: 0x0600589B RID: 22683 RVA: 0x00176B94 File Offset: 0x00174D94
		private void RpcReader___Server_SendPourSoil_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendPourSoil_2166136261();
		}

		// Token: 0x0600589C RID: 22684 RVA: 0x00176BC4 File Offset: 0x00174DC4
		private void RpcWriter___Observers_PourSoil_2166136261()
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

		// Token: 0x0600589D RID: 22685 RVA: 0x00176C6D File Offset: 0x00174E6D
		private void RpcLogic___PourSoil_2166136261()
		{
			if (this.isDispensing)
			{
				return;
			}
			this.isDispensing = true;
			base.StartCoroutine(this.<PourSoil>g__PourRoutine|20_0());
		}

		// Token: 0x0600589E RID: 22686 RVA: 0x00176C8C File Offset: 0x00174E8C
		private void RpcReader___Observers_PourSoil_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___PourSoil_2166136261();
		}

		// Token: 0x0600589F RID: 22687 RVA: 0x00176CB8 File Offset: 0x00174EB8
		private void RpcWriter___Server_SendSoil_3615296227(string ID)
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
			writer.WriteString(ID);
			base.SendServerRpc(10U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060058A0 RID: 22688 RVA: 0x00176D5F File Offset: 0x00174F5F
		public void RpcLogic___SendSoil_3615296227(string ID)
		{
			this.SetSoil(null, ID);
		}

		// Token: 0x060058A1 RID: 22689 RVA: 0x00176D6C File Offset: 0x00174F6C
		private void RpcReader___Server_SendSoil_3615296227(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string id = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendSoil_3615296227(id);
		}

		// Token: 0x060058A2 RID: 22690 RVA: 0x00176DAC File Offset: 0x00174FAC
		private void RpcWriter___Observers_SetSoil_2971853958(NetworkConnection conn, string ID)
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
			writer.WriteString(ID);
			base.SendObserversRpc(11U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060058A3 RID: 22691 RVA: 0x00176E64 File Offset: 0x00175064
		protected void RpcLogic___SetSoil_2971853958(NetworkConnection conn, string ID)
		{
			this.SoilID = ID;
			if (ID != string.Empty)
			{
				SoilDefinition item = Registry.GetItem<SoilDefinition>(this.SoilID);
				this.DirtPlane.material = item.DrySoilMat;
				this.SetSoilLevel(1f);
			}
		}

		// Token: 0x060058A4 RID: 22692 RVA: 0x00176EB0 File Offset: 0x001750B0
		private void RpcReader___Observers_SetSoil_2971853958(PooledReader PooledReader0, Channel channel)
		{
			string id = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetSoil_2971853958(null, id);
		}

		// Token: 0x060058A5 RID: 22693 RVA: 0x00176EEC File Offset: 0x001750EC
		private void RpcWriter___Target_SetSoil_2971853958(NetworkConnection conn, string ID)
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
			writer.WriteString(ID);
			base.SendTargetRpc(12U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x060058A6 RID: 22694 RVA: 0x00176FA4 File Offset: 0x001751A4
		private void RpcReader___Target_SetSoil_2971853958(PooledReader PooledReader0, Channel channel)
		{
			string id = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetSoil_2971853958(base.LocalConnection, id);
		}

		// Token: 0x060058A7 RID: 22695 RVA: 0x00176FDB File Offset: 0x001751DB
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040040EA RID: 16618
		public float AnimationDuration = 8f;

		// Token: 0x040040EB RID: 16619
		[Header("References")]
		public InteractableObject HandleIntObj;

		// Token: 0x040040EC RID: 16620
		public InteractableObject FillIntObj;

		// Token: 0x040040ED RID: 16621
		public MeshRenderer DirtPlane;

		// Token: 0x040040EE RID: 16622
		public Transform Dirt_Min;

		// Token: 0x040040EF RID: 16623
		public Transform Dirt_Max;

		// Token: 0x040040F0 RID: 16624
		public ParticleSystem PourParticles;

		// Token: 0x040040F1 RID: 16625
		public Animation PourAnimation;

		// Token: 0x040040F2 RID: 16626
		public AudioSourceController FillSound;

		// Token: 0x040040F3 RID: 16627
		public AudioSourceController ActivateSound;

		// Token: 0x040040F4 RID: 16628
		public AudioSourceController DirtPourSound;

		// Token: 0x040040F5 RID: 16629
		private bool isDispensing;

		// Token: 0x040040F6 RID: 16630
		private bool dll_Excuted;

		// Token: 0x040040F7 RID: 16631
		private bool dll_Excuted;
	}
}
