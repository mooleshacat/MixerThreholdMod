using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dialogue;
using ScheduleOne.Persistence;
using ScheduleOne.UI.Phone.Delivery;
using ScheduleOne.UI.Shop;
using ScheduleOne.Variables;
using ScheduleOne.VoiceOver;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x02000508 RID: 1288
	public class Oscar : NPC
	{
		// Token: 0x06001C4D RID: 7245 RVA: 0x00076CF2 File Offset: 0x00074EF2
		protected override void Start()
		{
			base.Start();
			this.ShopInterface.onOrderCompleted.AddListener(new UnityAction(this.OrderCompleted));
			Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(this.Loaded));
		}

		// Token: 0x06001C4E RID: 7246 RVA: 0x00076D31 File Offset: 0x00074F31
		private void OrderCompleted()
		{
			base.PlayVO(EVOLineType.Thanks);
			this.dialogueHandler.ShowWorldspaceDialogue(this.OrderCompletedLines[UnityEngine.Random.Range(0, this.OrderCompletedLines.Length)], 5f);
		}

		// Token: 0x06001C4F RID: 7247 RVA: 0x00076D5F File Offset: 0x00074F5F
		private void Loaded()
		{
			Singleton<LoadManager>.Instance.onLoadComplete.RemoveListener(new UnityAction(this.Loaded));
			if (!NetworkSingleton<VariableDatabase>.Instance.GetValue<bool>(this.GreetedVariable))
			{
				this.EnableGreeting();
			}
		}

		// Token: 0x06001C50 RID: 7248 RVA: 0x00076D94 File Offset: 0x00074F94
		private void EnableGreeting()
		{
			this.dialogueHandler.GetComponent<DialogueController>().OverrideContainer = this.GreetingDialogue;
			this.dialogueHandler.onConversationStart.AddListener(new UnityAction(this.SetGreeted));
		}

		// Token: 0x06001C51 RID: 7249 RVA: 0x00076DC8 File Offset: 0x00074FC8
		private void SetGreeted()
		{
			this.dialogueHandler.onConversationStart.RemoveListener(new UnityAction(this.SetGreeted));
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue(this.GreetedVariable, true.ToString(), true);
			this.dialogueHandler.GetComponent<DialogueController>().OverrideContainer = null;
		}

		// Token: 0x06001C52 RID: 7250 RVA: 0x00076E1C File Offset: 0x0007501C
		public void EnableDeliveries()
		{
			Singleton<CoroutineService>.Instance.StartCoroutine(this.<EnableDeliveries>g__Wait|9_0());
		}

		// Token: 0x06001C54 RID: 7252 RVA: 0x00076E42 File Offset: 0x00075042
		[CompilerGenerated]
		private IEnumerator <EnableDeliveries>g__Wait|9_0()
		{
			yield return new WaitUntil(() => PlayerSingleton<DeliveryApp>.InstanceExists);
			PlayerSingleton<DeliveryApp>.Instance.GetShop(this.ShopInterface).SetIsAvailable();
			yield break;
		}

		// Token: 0x06001C55 RID: 7253 RVA: 0x00076E51 File Offset: 0x00075051
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.OscarAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.OscarAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001C56 RID: 7254 RVA: 0x00076E6A File Offset: 0x0007506A
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.OscarAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.OscarAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001C57 RID: 7255 RVA: 0x00076E83 File Offset: 0x00075083
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001C58 RID: 7256 RVA: 0x00076E91 File Offset: 0x00075091
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001771 RID: 6001
		public ShopInterface ShopInterface;

		// Token: 0x04001772 RID: 6002
		[Header("Settings")]
		public string[] OrderCompletedLines;

		// Token: 0x04001773 RID: 6003
		public DialogueContainer GreetingDialogue;

		// Token: 0x04001774 RID: 6004
		public string GreetedVariable = "OscarGreeted";

		// Token: 0x04001775 RID: 6005
		private bool dll_Excuted;

		// Token: 0x04001776 RID: 6006
		private bool dll_Excuted;
	}
}
