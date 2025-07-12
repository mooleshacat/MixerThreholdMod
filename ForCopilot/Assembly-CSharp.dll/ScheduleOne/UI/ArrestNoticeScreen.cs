using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.FX;
using ScheduleOne.ItemFramework;
using ScheduleOne.Law;
using ScheduleOne.Map;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Product;
using ScheduleOne.Product.Packaging;
using ScheduleOne.Vehicles;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.UI
{
	// Token: 0x02000A5B RID: 2651
	public class ArrestNoticeScreen : Singleton<ArrestNoticeScreen>
	{
		// Token: 0x170009F6 RID: 2550
		// (get) Token: 0x0600474F RID: 18255 RVA: 0x0012BB4A File Offset: 0x00129D4A
		// (set) Token: 0x06004750 RID: 18256 RVA: 0x0012BB52 File Offset: 0x00129D52
		public bool isOpen { get; protected set; }

		// Token: 0x06004751 RID: 18257 RVA: 0x0012BB5C File Offset: 0x00129D5C
		protected override void Awake()
		{
			base.Awake();
			this.isOpen = false;
			this.Canvas.enabled = false;
			this.CanvasGroup.alpha = 0f;
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 20);
			Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(this.PlayerSpawned));
		}

		// Token: 0x06004752 RID: 18258 RVA: 0x0012BBC5 File Offset: 0x00129DC5
		private void PlayerSpawned()
		{
			Player.Local.onArrested.AddListener(new UnityAction(this.RecordCrimes));
		}

		// Token: 0x06004753 RID: 18259 RVA: 0x0012BBE2 File Offset: 0x00129DE2
		private void Exit(ExitAction action)
		{
			if (action.Used)
			{
				return;
			}
			if (!this.isOpen)
			{
				return;
			}
			if (action.exitType == ExitType.Escape)
			{
				action.Used = true;
				this.Close();
			}
		}

		// Token: 0x06004754 RID: 18260 RVA: 0x0012BC0C File Offset: 0x00129E0C
		public void Open()
		{
			this.ClearEntries();
			this.isOpen = true;
			this.Canvas.enabled = true;
			this.CanvasGroup.alpha = 1f;
			this.CanvasGroup.interactable = true;
			Singleton<PostProcessingManager>.Instance.SetBlur(1f);
			Crime[] array = this.recordedCrimes.Keys.ToArray<Crime>();
			for (int i = 0; i < array.Length; i++)
			{
				UnityEngine.Object.Instantiate<RectTransform>(this.CrimeEntryPrefab, this.CrimeEntryContainer).GetComponentInChildren<TextMeshProUGUI>().text = this.recordedCrimes[array[i]].ToString() + "x " + array[i].CrimeName.ToLower();
			}
			List<string> list = PenaltyHandler.ProcessCrimeList(this.recordedCrimes);
			this.ConfiscateItems(EStealthLevel.None);
			for (int j = 0; j < list.Count; j++)
			{
				UnityEngine.Object.Instantiate<RectTransform>(this.PenaltyEntryPrefab, this.PenaltyEntryContainer).GetComponentInChildren<TextMeshProUGUI>().text = list[j];
			}
			if (this.vehicle != null && !this.vehicle.isOccupied)
			{
				Transform[] possessedVehicleSpawnPoints = Singleton<Map>.Instance.PoliceStation.PossessedVehicleSpawnPoints;
				Transform target = possessedVehicleSpawnPoints[UnityEngine.Random.Range(0, possessedVehicleSpawnPoints.Length - 1)];
				Tuple<Vector3, Quaternion> alignmentTransform = this.vehicle.GetAlignmentTransform(target, EParkingAlignment.RearToKerb);
				this.vehicle.SetTransform_Server(alignmentTransform.Item1, alignmentTransform.Item2);
			}
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			Player.Deactivate(true);
		}

		// Token: 0x06004755 RID: 18261 RVA: 0x0012BD8B File Offset: 0x00129F8B
		public void Close()
		{
			if (!this.CanvasGroup.interactable || !this.isOpen)
			{
				return;
			}
			this.CanvasGroup.interactable = false;
			base.StartCoroutine(this.<Close>g__CloseRoutine|17_0());
		}

		// Token: 0x06004756 RID: 18262 RVA: 0x0012BDBC File Offset: 0x00129FBC
		public void RecordCrimes()
		{
			Debug.Log("Crimes recorded");
			this.recordedCrimes.Clear();
			if (Player.Local.LastDrivenVehicle != null && (Player.Local.TimeSinceVehicleExit < 30f || Player.Local.CrimeData.IsCrimeOnRecord(typeof(TransportingIllicitItems))))
			{
				this.vehicle = Player.Local.LastDrivenVehicle;
			}
			for (int i = 0; i < Player.Local.CrimeData.Crimes.Keys.Count; i++)
			{
				this.recordedCrimes.Add(Player.Local.CrimeData.Crimes.Keys.ElementAt(i), Player.Local.CrimeData.Crimes.Values.ElementAt(i));
			}
			if (Player.Local.CrimeData.EvadedArrest)
			{
				this.recordedCrimes.Add(new Evading(), 1);
			}
			this.RecordPossession(EStealthLevel.None);
		}

		// Token: 0x06004757 RID: 18263 RVA: 0x0012BEB8 File Offset: 0x0012A0B8
		private void RecordPossession(EStealthLevel maxStealthLevel)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			List<ItemSlot> allInventorySlots = PlayerSingleton<PlayerInventory>.Instance.GetAllInventorySlots();
			if (Player.Local.LastDrivenVehicle != null && Player.Local.TimeSinceVehicleExit < 30f && Player.Local.LastDrivenVehicle.Storage != null)
			{
				allInventorySlots.AddRange(Player.Local.LastDrivenVehicle.Storage.ItemSlots);
			}
			foreach (ItemSlot itemSlot in allInventorySlots)
			{
				if (itemSlot.ItemInstance != null)
				{
					if (itemSlot.ItemInstance is ProductItemInstance)
					{
						ProductItemInstance productItemInstance = itemSlot.ItemInstance as ProductItemInstance;
						if (productItemInstance.AppliedPackaging == null || productItemInstance.AppliedPackaging.StealthLevel <= maxStealthLevel)
						{
							switch (itemSlot.ItemInstance.Definition.legalStatus)
							{
							case ELegalStatus.ControlledSubstance:
								num += productItemInstance.Quantity;
								break;
							case ELegalStatus.LowSeverityDrug:
								num2 += productItemInstance.Quantity;
								break;
							case ELegalStatus.ModerateSeverityDrug:
								num3 += productItemInstance.Quantity;
								break;
							case ELegalStatus.HighSeverityDrug:
								num4 += productItemInstance.Quantity;
								break;
							}
						}
					}
					else
					{
						switch (itemSlot.ItemInstance.Definition.legalStatus)
						{
						case ELegalStatus.ControlledSubstance:
							num += itemSlot.ItemInstance.Quantity;
							break;
						case ELegalStatus.LowSeverityDrug:
							num2 += itemSlot.ItemInstance.Quantity;
							break;
						case ELegalStatus.ModerateSeverityDrug:
							num3 += itemSlot.ItemInstance.Quantity;
							break;
						case ELegalStatus.HighSeverityDrug:
							num4 += itemSlot.ItemInstance.Quantity;
							break;
						}
					}
				}
			}
			if (num > 0)
			{
				this.recordedCrimes.Add(new PossessingControlledSubstances(), num);
			}
			if (num2 > 0)
			{
				this.recordedCrimes.Add(new PossessingLowSeverityDrug(), num2);
			}
			if (num3 > 0)
			{
				this.recordedCrimes.Add(new PossessingModerateSeverityDrug(), num3);
			}
			if (num4 > 0)
			{
				this.recordedCrimes.Add(new PossessingHighSeverityDrug(), num4);
			}
		}

		// Token: 0x06004758 RID: 18264 RVA: 0x0012C0F4 File Offset: 0x0012A2F4
		private void ConfiscateItems(EStealthLevel maxStealthLevel)
		{
			List<ItemSlot> allInventorySlots = PlayerSingleton<PlayerInventory>.Instance.GetAllInventorySlots();
			if (Player.Local.LastDrivenVehicle != null && Player.Local.TimeSinceVehicleExit < 30f && Player.Local.LastDrivenVehicle.Storage != null)
			{
				allInventorySlots.AddRange(Player.Local.LastDrivenVehicle.Storage.ItemSlots);
			}
			foreach (ItemSlot itemSlot in allInventorySlots)
			{
				if (itemSlot.ItemInstance != null)
				{
					if (itemSlot.ItemInstance is ProductItemInstance)
					{
						ProductItemInstance productItemInstance = itemSlot.ItemInstance as ProductItemInstance;
						if (productItemInstance.AppliedPackaging == null || productItemInstance.AppliedPackaging.StealthLevel <= maxStealthLevel)
						{
							itemSlot.ClearStoredInstance(false);
						}
					}
					else if (itemSlot.ItemInstance.Definition.legalStatus != ELegalStatus.Legal)
					{
						itemSlot.ClearStoredInstance(false);
					}
				}
			}
		}

		// Token: 0x06004759 RID: 18265 RVA: 0x0012C1FC File Offset: 0x0012A3FC
		private void ClearEntries()
		{
			int childCount = this.CrimeEntryContainer.childCount;
			for (int i = 0; i < childCount; i++)
			{
				UnityEngine.Object.Destroy(this.CrimeEntryContainer.GetChild(i).gameObject);
			}
			childCount = this.PenaltyEntryContainer.childCount;
			for (int j = 0; j < childCount; j++)
			{
				UnityEngine.Object.Destroy(this.PenaltyEntryContainer.GetChild(j).gameObject);
			}
		}

		// Token: 0x0600475B RID: 18267 RVA: 0x0012C278 File Offset: 0x0012A478
		[CompilerGenerated]
		private IEnumerator <Close>g__CloseRoutine|17_0()
		{
			float lerpTime = 0.3f;
			for (float i = 0f; i < lerpTime; i += Time.deltaTime)
			{
				this.CanvasGroup.alpha = Mathf.Lerp(1f, 0f, i / lerpTime);
				Singleton<PostProcessingManager>.Instance.SetBlur(this.CanvasGroup.alpha);
				yield return new WaitForEndOfFrame();
			}
			this.CanvasGroup.alpha = 0f;
			this.Canvas.enabled = false;
			Singleton<PostProcessingManager>.Instance.SetBlur(0f);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			Player.Activate();
			this.ClearEntries();
			this.isOpen = false;
			yield break;
		}

		// Token: 0x04003426 RID: 13350
		public const float VEHICLE_POSSESSION_TIMEOUT = 30f;

		// Token: 0x04003428 RID: 13352
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x04003429 RID: 13353
		public CanvasGroup CanvasGroup;

		// Token: 0x0400342A RID: 13354
		public RectTransform CrimeEntryContainer;

		// Token: 0x0400342B RID: 13355
		public RectTransform PenaltyEntryContainer;

		// Token: 0x0400342C RID: 13356
		[Header("Prefabs")]
		public RectTransform CrimeEntryPrefab;

		// Token: 0x0400342D RID: 13357
		public RectTransform PenaltyEntryPrefab;

		// Token: 0x0400342E RID: 13358
		private Dictionary<Crime, int> recordedCrimes = new Dictionary<Crime, int>();

		// Token: 0x0400342F RID: 13359
		private LandVehicle vehicle;
	}
}
