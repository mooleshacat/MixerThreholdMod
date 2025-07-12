using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dialogue;
using ScheduleOne.Persistence;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using ScheduleOne.Variables;
using ScheduleOne.Vehicles;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x02000502 RID: 1282
	public class Marco : NPC
	{
		// Token: 0x06001C21 RID: 7201 RVA: 0x000767B4 File Offset: 0x000749B4
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.CharacterClasses.Marco_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001C22 RID: 7202 RVA: 0x000767D3 File Offset: 0x000749D3
		protected override void Start()
		{
			base.Start();
			Singleton<VehicleModMenu>.Instance.onPaintPurchased.AddListener(delegate()
			{
				this.dialogueHandler.ShowWorldspaceDialogue_5s("Thanks buddy");
			});
			Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(this.Loaded));
		}

		// Token: 0x06001C23 RID: 7203 RVA: 0x00076811 File Offset: 0x00074A11
		private bool ShouldShowRecoverVehicle(bool enabled)
		{
			return Player.Local.LastDrivenVehicle != null;
		}

		// Token: 0x06001C24 RID: 7204 RVA: 0x00076823 File Offset: 0x00074A23
		private bool RecoverVehicleValid(out string reason)
		{
			if (Player.Local.LastDrivenVehicle == null)
			{
				reason = "You have no vehicle to recover";
				return false;
			}
			if (Player.Local.LastDrivenVehicle.isOccupied)
			{
				reason = "Someone is in the vehicle";
				return false;
			}
			reason = string.Empty;
			return true;
		}

		// Token: 0x06001C25 RID: 7205 RVA: 0x00076864 File Offset: 0x00074A64
		private bool RepaintVehicleValid(out string reason)
		{
			if (this.VehicleDetector.closestVehicle == null)
			{
				reason = "Vehicle must be parked inside the shop";
				return false;
			}
			if (this.VehicleDetector.closestVehicle.isOccupied)
			{
				reason = "Someone is in the vehicle";
				return false;
			}
			reason = string.Empty;
			return true;
		}

		// Token: 0x06001C26 RID: 7206 RVA: 0x000768B0 File Offset: 0x00074AB0
		private void RecoverVehicle()
		{
			LandVehicle lastDrivenVehicle = Player.Local.LastDrivenVehicle;
			if (lastDrivenVehicle == null)
			{
				return;
			}
			lastDrivenVehicle.AlignTo(this.VehicleRecoveryPoint, EParkingAlignment.RearToKerb, true);
		}

		// Token: 0x06001C27 RID: 7207 RVA: 0x000768E0 File Offset: 0x00074AE0
		private void Loaded()
		{
			Singleton<LoadManager>.Instance.onLoadComplete.RemoveListener(new UnityAction(this.Loaded));
			if (!NetworkSingleton<VariableDatabase>.Instance.GetValue<bool>(this.GreetedVariable))
			{
				this.EnableGreeting();
			}
		}

		// Token: 0x06001C28 RID: 7208 RVA: 0x00076915 File Offset: 0x00074B15
		private void EnableGreeting()
		{
			this.dialogueHandler.GetComponent<DialogueController>().OverrideContainer = this.GreetingDialogue;
			this.dialogueHandler.onConversationStart.AddListener(new UnityAction(this.SetGreeted));
		}

		// Token: 0x06001C29 RID: 7209 RVA: 0x0007694C File Offset: 0x00074B4C
		private void SetGreeted()
		{
			this.dialogueHandler.onConversationStart.RemoveListener(new UnityAction(this.SetGreeted));
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue(this.GreetedVariable, true.ToString(), true);
			this.dialogueHandler.GetComponent<DialogueController>().OverrideContainer = null;
		}

		// Token: 0x06001C2D RID: 7213 RVA: 0x000769DC File Offset: 0x00074BDC
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.MarcoAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.MarcoAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001C2E RID: 7214 RVA: 0x000769F5 File Offset: 0x00074BF5
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.MarcoAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.MarcoAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001C2F RID: 7215 RVA: 0x00076A0E File Offset: 0x00074C0E
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001C30 RID: 7216 RVA: 0x00076A1C File Offset: 0x00074C1C
		protected override void dll()
		{
			base.Awake();
			DialogueController.DialogueChoice dialogueChoice = new DialogueController.DialogueChoice();
			dialogueChoice.ChoiceText = "My vehicle is stuck";
			dialogueChoice.Enabled = true;
			dialogueChoice.shouldShowCheck = new DialogueController.DialogueChoice.ShouldShowCheck(this.ShouldShowRecoverVehicle);
			dialogueChoice.isValidCheck = new DialogueController.DialogueChoice.IsChoiceValid(this.RecoverVehicleValid);
			dialogueChoice.Conversation = this.RecoveryConversation;
			dialogueChoice.onChoosen.AddListener(new UnityAction(this.RecoverVehicle));
			DialogueController.DialogueChoice dialogueChoice2 = new DialogueController.DialogueChoice();
			dialogueChoice2.ChoiceText = "I'd like to repaint my vehicle";
			dialogueChoice2.Enabled = true;
			dialogueChoice2.isValidCheck = new DialogueController.DialogueChoice.IsChoiceValid(this.RepaintVehicleValid);
			dialogueChoice2.onChoosen.AddListener(delegate()
			{
				Singleton<VehicleModMenu>.Instance.Open(this.VehicleDetector.closestVehicle);
			});
			this.dialogueHandler.GetComponent<DialogueController>().Choices.Add(dialogueChoice2);
			this.dialogueHandler.GetComponent<DialogueController>().Choices.Add(dialogueChoice);
		}

		// Token: 0x0400175F RID: 5983
		public Transform VehicleRecoveryPoint;

		// Token: 0x04001760 RID: 5984
		public VehicleDetector VehicleDetector;

		// Token: 0x04001761 RID: 5985
		public DialogueContainer RecoveryConversation;

		// Token: 0x04001762 RID: 5986
		public DialogueContainer GreetingDialogue;

		// Token: 0x04001763 RID: 5987
		public string GreetedVariable = "MarcoGreeted";

		// Token: 0x04001764 RID: 5988
		private bool dll_Excuted;

		// Token: 0x04001765 RID: 5989
		private bool dll_Excuted;
	}
}
