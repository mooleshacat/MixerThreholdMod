using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dialogue;
using ScheduleOne.Doors;
using ScheduleOne.Interaction;
using ScheduleOne.Levelling;
using ScheduleOne.NPCs.CharacterClasses;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.Map
{
	// Token: 0x02000C6D RID: 3181
	public class DarkMarketMainDoor : MonoBehaviour
	{
		// Token: 0x17000C6E RID: 3182
		// (get) Token: 0x06005982 RID: 22914 RVA: 0x0017A218 File Offset: 0x00178418
		// (set) Token: 0x06005983 RID: 22915 RVA: 0x0017A220 File Offset: 0x00178420
		public bool KnockingEnabled { get; private set; } = true;

		// Token: 0x06005984 RID: 22916 RVA: 0x0017A229 File Offset: 0x00178429
		private void Start()
		{
			this.Igor.gameObject.SetActive(false);
		}

		// Token: 0x06005985 RID: 22917 RVA: 0x0017A23C File Offset: 0x0017843C
		public void SetKnockingEnabled(bool enabled)
		{
			this.InteractableObject.gameObject.SetActive(enabled);
			this.KnockingEnabled = enabled;
		}

		// Token: 0x06005986 RID: 22918 RVA: 0x0017A258 File Offset: 0x00178458
		public void Hovered()
		{
			if (this.KnockingEnabled && this.knockRoutine == null && Player.Local.CrimeData.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.None)
			{
				this.InteractableObject.SetMessage("Knock");
				this.InteractableObject.SetInteractableState(InteractableObject.EInteractableState.Default);
				return;
			}
			this.InteractableObject.SetInteractableState(InteractableObject.EInteractableState.Disabled);
		}

		// Token: 0x06005987 RID: 22919 RVA: 0x0017A2AF File Offset: 0x001784AF
		public void Interacted()
		{
			this.Knocked();
		}

		// Token: 0x06005988 RID: 22920 RVA: 0x0017A2B7 File Offset: 0x001784B7
		private void Knocked()
		{
			this.knockRoutine = base.StartCoroutine(this.<Knocked>g__Knock|16_0());
		}

		// Token: 0x0600598A RID: 22922 RVA: 0x0017A2DA File Offset: 0x001784DA
		[CompilerGenerated]
		private IEnumerator <Knocked>g__Knock|16_0()
		{
			this.KnockSound.Play();
			this.Igor.gameObject.SetActive(true);
			this.Igor.Avatar.LookController.ForceLookTarget = PlayerSingleton<PlayerCamera>.Instance.transform;
			yield return new WaitForSeconds(0.75f);
			this.Igor.gameObject.SetActive(true);
			this.Peephole.Open();
			yield return new WaitForSeconds(0.3f);
			bool shouldUnlock = false;
			if (Vector3.Distance(Player.Local.transform.position, base.transform.position) < 3f)
			{
				shouldUnlock = (NetworkSingleton<LevelManager>.Instance.GetFullRank() >= NetworkSingleton<DarkMarket>.Instance.UnlockRank);
				DialogueContainer container = shouldUnlock ? (NetworkSingleton<DarkMarket>.Instance.IsOpen ? this.SuccessDialogue : this.SuccessDialogueNotOpen) : this.FailDialogue;
				this.Igor.dialogueHandler.InitializeDialogue(container);
				yield return new WaitUntil(() => !this.Igor.dialogueHandler.IsPlaying);
			}
			else
			{
				yield return new WaitForSeconds(1f);
			}
			yield return new WaitForSeconds(0.2f);
			this.Peephole.Close();
			yield return new WaitForSeconds(0.2f);
			if (shouldUnlock)
			{
				NetworkSingleton<DarkMarket>.Instance.SendUnlocked();
			}
			else
			{
				HintDisplay instance = Singleton<HintDisplay>.Instance;
				string str = "Reach the rank of <h1>";
				FullRank unlockRank = NetworkSingleton<DarkMarket>.Instance.UnlockRank;
				instance.ShowHint(str + unlockRank.ToString() + "</h> to access this area.", 15f);
			}
			yield return new WaitForSeconds(0.5f);
			this.Igor.gameObject.SetActive(false);
			this.knockRoutine = null;
			yield break;
		}

		// Token: 0x0400419C RID: 16796
		public AudioSource KnockSound;

		// Token: 0x0400419D RID: 16797
		public InteractableObject InteractableObject;

		// Token: 0x0400419E RID: 16798
		public Peephole Peephole;

		// Token: 0x0400419F RID: 16799
		public Igor Igor;

		// Token: 0x040041A0 RID: 16800
		public DialogueContainer FailDialogue;

		// Token: 0x040041A1 RID: 16801
		public DialogueContainer SuccessDialogue;

		// Token: 0x040041A2 RID: 16802
		public DialogueContainer SuccessDialogueNotOpen;

		// Token: 0x040041A3 RID: 16803
		private Coroutine knockRoutine;
	}
}
