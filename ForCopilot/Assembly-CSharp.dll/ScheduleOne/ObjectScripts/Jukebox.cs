using System;
using System.Collections.Generic;
using System.Linq;
using EasyButtons;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using GameKit.Utilities;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.Persistence.Datas;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000BEC RID: 3052
	public class Jukebox : GridItem
	{
		// Token: 0x17000B23 RID: 2851
		// (get) Token: 0x0600518F RID: 20879 RVA: 0x00158A83 File Offset: 0x00156C83
		public int CurrentVolume
		{
			get
			{
				return this._jukeboxState.CurrentVolume;
			}
		}

		// Token: 0x17000B24 RID: 2852
		// (get) Token: 0x06005190 RID: 20880 RVA: 0x00158A90 File Offset: 0x00156C90
		public float NormalizedVolume
		{
			get
			{
				return (float)this.CurrentVolume / 8f;
			}
		}

		// Token: 0x17000B25 RID: 2853
		// (get) Token: 0x06005191 RID: 20881 RVA: 0x00158A9F File Offset: 0x00156C9F
		public bool IsPlaying
		{
			get
			{
				return this._jukeboxState.IsPlaying;
			}
		}

		// Token: 0x17000B26 RID: 2854
		// (get) Token: 0x06005192 RID: 20882 RVA: 0x00158AAC File Offset: 0x00156CAC
		public float CurrentTrackTime
		{
			get
			{
				return this._jukeboxState.CurrentTrackTime;
			}
		}

		// Token: 0x17000B27 RID: 2855
		// (get) Token: 0x06005193 RID: 20883 RVA: 0x00158AB9 File Offset: 0x00156CB9
		private int[] TrackOrder
		{
			get
			{
				return this._jukeboxState.TrackOrder;
			}
		}

		// Token: 0x17000B28 RID: 2856
		// (get) Token: 0x06005194 RID: 20884 RVA: 0x00158AC6 File Offset: 0x00156CC6
		public int CurrentTrackOrderIndex
		{
			get
			{
				return this._jukeboxState.CurrentTrackOrderIndex;
			}
		}

		// Token: 0x17000B29 RID: 2857
		// (get) Token: 0x06005195 RID: 20885 RVA: 0x00158AD3 File Offset: 0x00156CD3
		public bool Shuffle
		{
			get
			{
				return this._jukeboxState.Shuffle;
			}
		}

		// Token: 0x17000B2A RID: 2858
		// (get) Token: 0x06005196 RID: 20886 RVA: 0x00158AE0 File Offset: 0x00156CE0
		public Jukebox.ERepeatMode RepeatMode
		{
			get
			{
				return this._jukeboxState.RepeatMode;
			}
		}

		// Token: 0x17000B2B RID: 2859
		// (get) Token: 0x06005197 RID: 20887 RVA: 0x00158AED File Offset: 0x00156CED
		public bool Sync
		{
			get
			{
				return this._jukeboxState.Sync;
			}
		}

		// Token: 0x17000B2C RID: 2860
		// (get) Token: 0x06005198 RID: 20888 RVA: 0x00158AFA File Offset: 0x00156CFA
		public Jukebox.Track currentTrack
		{
			get
			{
				return this.GetTrack(this.CurrentTrackOrderIndex);
			}
		}

		// Token: 0x17000B2D RID: 2861
		// (get) Token: 0x06005199 RID: 20889 RVA: 0x00158B08 File Offset: 0x00156D08
		private AudioClip currentClip
		{
			get
			{
				return this.GetTrack(this.CurrentTrackOrderIndex).Clip;
			}
		}

		// Token: 0x0600519A RID: 20890 RVA: 0x00158B1C File Offset: 0x00156D1C
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.ObjectScripts.Jukebox_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600519B RID: 20891 RVA: 0x00158B3C File Offset: 0x00156D3C
		private void FixedUpdate()
		{
			if (this.IsPlaying)
			{
				this.ApplyVolume();
				this._jukeboxState.CurrentTrackTime += Time.fixedDeltaTime;
			}
			if (this.IsPlaying && this.CurrentTrackTime >= this.currentClip.length)
			{
				if (this.RepeatMode == Jukebox.ERepeatMode.None && this.CurrentTrackOrderIndex == 26)
				{
					this._jukeboxState.IsPlaying = false;
					return;
				}
				this.Next();
			}
		}

		// Token: 0x0600519C RID: 20892 RVA: 0x00158BAE File Offset: 0x00156DAE
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (connection.IsLocalClient)
			{
				return;
			}
			this.SetJukeboxState(connection, this._jukeboxState, true, true);
		}

		// Token: 0x0600519D RID: 20893 RVA: 0x00158BCF File Offset: 0x00156DCF
		public void ChangeVolume(int change)
		{
			this.SetVolume(this.CurrentVolume + change, true);
		}

		// Token: 0x0600519E RID: 20894 RVA: 0x00158BE0 File Offset: 0x00156DE0
		public void SetVolume(int volume, bool replicate)
		{
			this._jukeboxState.CurrentVolume = Mathf.Clamp(volume, 0, 8);
			for (int i = 0; i < this.VolumeIndicatorBars.Length; i++)
			{
				this.VolumeIndicatorBars[i].SetActive(i < this.CurrentVolume);
			}
			this.ApplyVolume();
			if (replicate)
			{
				this.ReplicateStateToOtherClients(false);
			}
		}

		// Token: 0x0600519F RID: 20895 RVA: 0x00158C39 File Offset: 0x00156E39
		private void ApplyVolume()
		{
			this.AudioSourceController.VolumeMultiplier = this.NormalizedVolume * Singleton<AudioManager>.Instance.GetScaledMusicVolumeMultiplier(0.4f);
			this.AudioSourceController.ApplyVolume();
		}

		// Token: 0x060051A0 RID: 20896 RVA: 0x00158C67 File Offset: 0x00156E67
		[Button]
		public void TogglePlay()
		{
			if (this.IsPlaying)
			{
				this._jukeboxState.IsPlaying = false;
			}
			else
			{
				this._jukeboxState.IsPlaying = true;
			}
			this.ReplicateStateToOtherClients(true);
			if (this.Sync)
			{
				this.ReplicateStateToOtherJukeboxes(true);
			}
		}

		// Token: 0x060051A1 RID: 20897 RVA: 0x00158CA4 File Offset: 0x00156EA4
		[Button]
		public void Back()
		{
			if (this._jukeboxState.CurrentTrackTime < 1f)
			{
				this._jukeboxState.CurrentTrackOrderIndex = this.GetPreviousTrackOrderIndex();
				this._jukeboxState.CurrentTrackTime = 0f;
			}
			else
			{
				this._jukeboxState.CurrentTrackTime = 0f;
			}
			this.ReplicateStateToOtherClients(true);
			if (this.Sync)
			{
				this.ReplicateStateToOtherJukeboxes(true);
			}
		}

		// Token: 0x060051A2 RID: 20898 RVA: 0x00158D0C File Offset: 0x00156F0C
		[Button]
		public void Next()
		{
			this._jukeboxState.CurrentTrackTime = 0f;
			this._jukeboxState.CurrentTrackOrderIndex = this.GetNextTrackOrderIndex();
			this._jukeboxState.IsPlaying = true;
			this.ReplicateStateToOtherClients(true);
			if (this.Sync)
			{
				this.ReplicateStateToOtherJukeboxes(true);
			}
		}

		// Token: 0x060051A3 RID: 20899 RVA: 0x00158D5C File Offset: 0x00156F5C
		private int GetPreviousTrackOrderIndex()
		{
			if (this.RepeatMode == Jukebox.ERepeatMode.RepeatTrack)
			{
				return this.CurrentTrackOrderIndex;
			}
			if (this.CurrentTrackOrderIndex == 0)
			{
				return 26;
			}
			return this.CurrentTrackOrderIndex - 1;
		}

		// Token: 0x060051A4 RID: 20900 RVA: 0x00158D81 File Offset: 0x00156F81
		private int GetNextTrackOrderIndex()
		{
			if (this.RepeatMode == Jukebox.ERepeatMode.RepeatTrack)
			{
				return this.CurrentTrackOrderIndex;
			}
			if (this.CurrentTrackOrderIndex == 26)
			{
				return 0;
			}
			return this.CurrentTrackOrderIndex + 1;
		}

		// Token: 0x060051A5 RID: 20901 RVA: 0x00158DA8 File Offset: 0x00156FA8
		[Button]
		public void ToggleShuffle()
		{
			if (this.Shuffle)
			{
				this._jukeboxState.Shuffle = false;
				int currentTrackOrderIndex = this.TrackOrder[this._jukeboxState.CurrentTrackOrderIndex];
				this._jukeboxState.TrackOrder = new int[27];
				for (int i = 0; i < this.TrackOrder.Length; i++)
				{
					this.TrackOrder[i] = i;
				}
				this._jukeboxState.CurrentTrackOrderIndex = currentTrackOrderIndex;
			}
			else
			{
				this._jukeboxState.Shuffle = true;
				int item = this.TrackOrder[this._jukeboxState.CurrentTrackOrderIndex];
				this._jukeboxState.CurrentTrackOrderIndex = 0;
				List<int> list = new List<int>(this.TrackOrder);
				list.Remove(item);
				Arrays.Shuffle<int>(list);
				list.Insert(0, item);
				this._jukeboxState.TrackOrder = list.ToArray();
			}
			this.ReplicateStateToOtherClients(false);
			if (this.Sync)
			{
				this.ReplicateStateToOtherJukeboxes(false);
			}
		}

		// Token: 0x060051A6 RID: 20902 RVA: 0x00158E8C File Offset: 0x0015708C
		[Button]
		public void ToggleRepeatMode()
		{
			if (this.RepeatMode == Jukebox.ERepeatMode.RepeatQueue)
			{
				this._jukeboxState.RepeatMode = Jukebox.ERepeatMode.RepeatTrack;
			}
			else if (this.RepeatMode == Jukebox.ERepeatMode.RepeatTrack)
			{
				this._jukeboxState.RepeatMode = Jukebox.ERepeatMode.None;
			}
			else
			{
				this._jukeboxState.RepeatMode = Jukebox.ERepeatMode.RepeatQueue;
			}
			this.ReplicateStateToOtherClients(false);
			if (this.Sync)
			{
				this.ReplicateStateToOtherJukeboxes(false);
			}
		}

		// Token: 0x060051A7 RID: 20903 RVA: 0x00158EE9 File Offset: 0x001570E9
		[Button]
		public void ToggleSync()
		{
			if (this.Sync)
			{
				this._jukeboxState.Sync = false;
			}
			else
			{
				this._jukeboxState.Sync = true;
				this.ReplicateStateToOtherJukeboxes(true);
			}
			this.ReplicateStateToOtherClients(false);
		}

		// Token: 0x060051A8 RID: 20904 RVA: 0x00158F1C File Offset: 0x0015711C
		public void PlayTrack(int trackID)
		{
			this._jukeboxState.IsPlaying = true;
			this._jukeboxState.CurrentTrackTime = 0f;
			if (this.Shuffle)
			{
				this._jukeboxState.CurrentTrackOrderIndex = 0;
				List<int> list = new List<int>(this.TrackOrder);
				list.Remove(trackID);
				Arrays.Shuffle<int>(list);
				list.Insert(0, trackID);
				this._jukeboxState.TrackOrder = list.ToArray();
			}
			else
			{
				this._jukeboxState.CurrentTrackOrderIndex = this.TrackOrder.ToList<int>().IndexOf(trackID);
			}
			this.ReplicateStateToOtherClients(true);
			if (this.Sync)
			{
				this.ReplicateStateToOtherJukeboxes(true);
			}
		}

		// Token: 0x060051A9 RID: 20905 RVA: 0x00158FC0 File Offset: 0x001571C0
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendJukeboxState(Jukebox.JukeboxState state, bool setTrackTime, bool setSync)
		{
			this.RpcWriter___Server_SendJukeboxState_1728100027(state, setTrackTime, setSync);
			this.RpcLogic___SendJukeboxState_1728100027(state, setTrackTime, setSync);
		}

		// Token: 0x060051AA RID: 20906 RVA: 0x00158FE8 File Offset: 0x001571E8
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void SetJukeboxState(NetworkConnection conn, Jukebox.JukeboxState state, bool setTrackTime, bool setSync)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetJukeboxState_2499833112(conn, state, setTrackTime, setSync);
				this.RpcLogic___SetJukeboxState_2499833112(conn, state, setTrackTime, setSync);
			}
			else
			{
				this.RpcWriter___Target_SetJukeboxState_2499833112(conn, state, setTrackTime, setSync);
			}
		}

		// Token: 0x060051AB RID: 20907 RVA: 0x00159044 File Offset: 0x00157244
		public void SetJukeboxState(Jukebox.JukeboxState state, bool setTrackTime)
		{
			this.SetVolume(state.CurrentVolume, false);
			if (this.ValidateQueue(state.TrackOrder))
			{
				this._jukeboxState.TrackOrder = state.TrackOrder;
			}
			else
			{
				Console.LogWarning("Invalid queue data received. Using default queue. Invalid queue: " + string.Join<int>(", ", state.TrackOrder), null);
			}
			this._jukeboxState.CurrentTrackOrderIndex = state.CurrentTrackOrderIndex;
			this._jukeboxState.IsPlaying = state.IsPlaying;
			this._jukeboxState.CurrentTrackTime = state.CurrentTrackTime;
			this._jukeboxState.Shuffle = state.Shuffle;
			this._jukeboxState.RepeatMode = state.RepeatMode;
			this._jukeboxState.Sync = state.Sync;
			if (setTrackTime)
			{
				this.AudioSourceController.AudioSource.time = this.CurrentTrackTime;
			}
			this.AudioSourceController.AudioSource.clip = this.GetTrack(this.CurrentTrackOrderIndex).Clip;
			if (this.IsPlaying)
			{
				if (!this.AudioSourceController.isPlaying)
				{
					this.AudioSourceController.Play();
				}
			}
			else if (this.AudioSourceController.isPlaying)
			{
				this.AudioSourceController.Stop();
			}
			if (setTrackTime)
			{
				this.AudioSourceController.AudioSource.time = this.CurrentTrackTime;
			}
			if (this.onStateChanged != null)
			{
				this.onStateChanged();
			}
		}

		// Token: 0x060051AC RID: 20908 RVA: 0x001591A5 File Offset: 0x001573A5
		private Jukebox.Track GetTrack(int orderIndex)
		{
			if (orderIndex < 0 || orderIndex >= this.TrackList.Length)
			{
				Console.LogWarning(string.Format("Invalid track index: {0}. Returning null.", orderIndex), null);
				return null;
			}
			return this.TrackList[this.TrackOrder[orderIndex]];
		}

		// Token: 0x060051AD RID: 20909 RVA: 0x001591E0 File Offset: 0x001573E0
		private bool ValidateQueue(int[] queue)
		{
			if (queue == null || queue.Length != 27)
			{
				Console.LogWarning("Queue is null or has invalid length.", null);
				return false;
			}
			if (queue.Distinct<int>().Count<int>() != 27)
			{
				Console.LogWarning("Queue has duplicates.", null);
				return false;
			}
			foreach (int num in queue)
			{
				if (num < 0 || num >= 27)
				{
					Console.LogWarning(string.Format("Queue has invalid value: {0}. Must be between 0 and {1}.", num, 26), null);
					return false;
				}
			}
			return true;
		}

		// Token: 0x060051AE RID: 20910 RVA: 0x0015925D File Offset: 0x0015745D
		private void ReplicateStateToOtherClients(bool setTrackTime)
		{
			this.SendJukeboxState(this._jukeboxState, setTrackTime, true);
		}

		// Token: 0x060051AF RID: 20911 RVA: 0x00159270 File Offset: 0x00157470
		private void ReplicateStateToOtherJukeboxes(bool setTrackTime)
		{
			foreach (Jukebox jukebox in base.ParentProperty.GetBuildablesOfType<Jukebox>())
			{
				if (!(jukebox == this))
				{
					jukebox.SendJukeboxState(this._jukeboxState, setTrackTime, false);
				}
			}
		}

		// Token: 0x060051B0 RID: 20912 RVA: 0x001592D8 File Offset: 0x001574D8
		public override BuildableItemData GetBaseData()
		{
			return new JukeboxData(base.GUID, base.ItemInstance, 0, base.OwnerGrid, this.OriginCoordinate, this.Rotation, this._jukeboxState);
		}

		// Token: 0x060051B2 RID: 20914 RVA: 0x00159304 File Offset: 0x00157504
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.JukeboxAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.JukeboxAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(8U, new ServerRpcDelegate(this.RpcReader___Server_SendJukeboxState_1728100027));
			base.RegisterObserversRpc(9U, new ClientRpcDelegate(this.RpcReader___Observers_SetJukeboxState_2499833112));
			base.RegisterTargetRpc(10U, new ClientRpcDelegate(this.RpcReader___Target_SetJukeboxState_2499833112));
		}

		// Token: 0x060051B3 RID: 20915 RVA: 0x0015936D File Offset: 0x0015756D
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.JukeboxAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.JukeboxAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x060051B4 RID: 20916 RVA: 0x00159386 File Offset: 0x00157586
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060051B5 RID: 20917 RVA: 0x00159394 File Offset: 0x00157594
		private void RpcWriter___Server_SendJukeboxState_1728100027(Jukebox.JukeboxState state, bool setTrackTime, bool setSync)
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
			writer.Write___ScheduleOne.ObjectScripts.Jukebox/JukeboxStateFishNet.Serializing.Generated(state);
			writer.WriteBoolean(setTrackTime);
			writer.WriteBoolean(setSync);
			base.SendServerRpc(8U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x060051B6 RID: 20918 RVA: 0x00159455 File Offset: 0x00157655
		public void RpcLogic___SendJukeboxState_1728100027(Jukebox.JukeboxState state, bool setTrackTime, bool setSync)
		{
			this.SetJukeboxState(null, state, setTrackTime, setSync);
		}

		// Token: 0x060051B7 RID: 20919 RVA: 0x00159464 File Offset: 0x00157664
		private void RpcReader___Server_SendJukeboxState_1728100027(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			Jukebox.JukeboxState state = GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.Jukebox/JukeboxStateFishNet.Serializing.Generateds(PooledReader0);
			bool setTrackTime = PooledReader0.ReadBoolean();
			bool setSync = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendJukeboxState_1728100027(state, setTrackTime, setSync);
		}

		// Token: 0x060051B8 RID: 20920 RVA: 0x001594C4 File Offset: 0x001576C4
		private void RpcWriter___Observers_SetJukeboxState_2499833112(NetworkConnection conn, Jukebox.JukeboxState state, bool setTrackTime, bool setSync)
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
			writer.Write___ScheduleOne.ObjectScripts.Jukebox/JukeboxStateFishNet.Serializing.Generated(state);
			writer.WriteBoolean(setTrackTime);
			writer.WriteBoolean(setSync);
			base.SendObserversRpc(9U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060051B9 RID: 20921 RVA: 0x00159594 File Offset: 0x00157794
		public void RpcLogic___SetJukeboxState_2499833112(NetworkConnection conn, Jukebox.JukeboxState state, bool setTrackTime, bool setSync)
		{
			this.SetJukeboxState(state, setTrackTime);
		}

		// Token: 0x060051BA RID: 20922 RVA: 0x001595A0 File Offset: 0x001577A0
		private void RpcReader___Observers_SetJukeboxState_2499833112(PooledReader PooledReader0, Channel channel)
		{
			Jukebox.JukeboxState state = GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.Jukebox/JukeboxStateFishNet.Serializing.Generateds(PooledReader0);
			bool setTrackTime = PooledReader0.ReadBoolean();
			bool setSync = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetJukeboxState_2499833112(null, state, setTrackTime, setSync);
		}

		// Token: 0x060051BB RID: 20923 RVA: 0x00159600 File Offset: 0x00157800
		private void RpcWriter___Target_SetJukeboxState_2499833112(NetworkConnection conn, Jukebox.JukeboxState state, bool setTrackTime, bool setSync)
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
			writer.Write___ScheduleOne.ObjectScripts.Jukebox/JukeboxStateFishNet.Serializing.Generated(state);
			writer.WriteBoolean(setTrackTime);
			writer.WriteBoolean(setSync);
			base.SendTargetRpc(10U, writer, channel, 0, conn, false, true);
			writer.Store();
		}

		// Token: 0x060051BC RID: 20924 RVA: 0x001596D0 File Offset: 0x001578D0
		private void RpcReader___Target_SetJukeboxState_2499833112(PooledReader PooledReader0, Channel channel)
		{
			Jukebox.JukeboxState state = GeneratedReaders___Internal.Read___ScheduleOne.ObjectScripts.Jukebox/JukeboxStateFishNet.Serializing.Generateds(PooledReader0);
			bool setTrackTime = PooledReader0.ReadBoolean();
			bool setSync = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetJukeboxState_2499833112(base.LocalConnection, state, setTrackTime, setSync);
		}

		// Token: 0x060051BD RID: 20925 RVA: 0x0015972C File Offset: 0x0015792C
		protected override void dll()
		{
			base.Awake();
			if (!this.isGhost)
			{
				this._jukeboxState = new Jukebox.JukeboxState();
				this._jukeboxState.TrackOrder = new int[27];
				for (int i = 0; i < this.TrackOrder.Length; i++)
				{
					this.TrackOrder[i] = i;
				}
				for (int j = 0; j < this.TrackList.Length; j++)
				{
					if (this.TrackList[j].Clip == null)
					{
						Console.LogError(string.Format("Track {0} does not have a clip assigned.", j), null);
					}
				}
				this.SetVolume(this.CurrentVolume, true);
			}
		}

		// Token: 0x04003D2A RID: 15658
		public const float MUSIC_FADE_MULTIPLIER = 0.4f;

		// Token: 0x04003D2B RID: 15659
		public const int TRACK_COUNT = 27;

		// Token: 0x04003D2C RID: 15660
		private Jukebox.JukeboxState _jukeboxState;

		// Token: 0x04003D2D RID: 15661
		[Header("References")]
		public Jukebox.Track[] TrackList;

		// Token: 0x04003D2E RID: 15662
		public GameObject[] VolumeIndicatorBars;

		// Token: 0x04003D2F RID: 15663
		public AudioSourceController AudioSourceController;

		// Token: 0x04003D30 RID: 15664
		public Action onStateChanged;

		// Token: 0x04003D31 RID: 15665
		private bool dll_Excuted;

		// Token: 0x04003D32 RID: 15666
		private bool dll_Excuted;

		// Token: 0x02000BED RID: 3053
		[Serializable]
		public class Track
		{
			// Token: 0x04003D33 RID: 15667
			public string TrackName;

			// Token: 0x04003D34 RID: 15668
			public AudioClip Clip;

			// Token: 0x04003D35 RID: 15669
			public string ArtistName = "KAESUL";
		}

		// Token: 0x02000BEE RID: 3054
		[Serializable]
		public class JukeboxState
		{
			// Token: 0x04003D36 RID: 15670
			public int CurrentVolume = 4;

			// Token: 0x04003D37 RID: 15671
			public bool IsPlaying;

			// Token: 0x04003D38 RID: 15672
			public float CurrentTrackTime;

			// Token: 0x04003D39 RID: 15673
			public int[] TrackOrder;

			// Token: 0x04003D3A RID: 15674
			public int CurrentTrackOrderIndex;

			// Token: 0x04003D3B RID: 15675
			public bool Shuffle;

			// Token: 0x04003D3C RID: 15676
			public Jukebox.ERepeatMode RepeatMode = Jukebox.ERepeatMode.RepeatQueue;

			// Token: 0x04003D3D RID: 15677
			public bool Sync;
		}

		// Token: 0x02000BEF RID: 3055
		public enum ERepeatMode
		{
			// Token: 0x04003D3F RID: 15679
			None,
			// Token: 0x04003D40 RID: 15680
			RepeatQueue,
			// Token: 0x04003D41 RID: 15681
			RepeatTrack
		}
	}
}
