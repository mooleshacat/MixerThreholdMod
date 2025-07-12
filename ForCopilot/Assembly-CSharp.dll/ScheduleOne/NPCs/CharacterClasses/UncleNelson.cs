using System;
using System.Collections.Generic;
using ScheduleOne.UI.Phone.Messages;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x02000517 RID: 1303
	public class UncleNelson : NPC
	{
		// Token: 0x06001CB6 RID: 7350 RVA: 0x000778F8 File Offset: 0x00075AF8
		public void SendInitialMessage()
		{
			if (base.MSGConversation.messageChainHistory.Count > 0 || base.MSGConversation.messageHistory.Count > 0)
			{
				return;
			}
			base.MSGConversation.SetIsKnown(false);
			base.MSGConversation.SendMessageChain(new MessageChain
			{
				Messages = new List<string>
				{
					this.InitialMessage
				}
			}, 0f, false, true);
			base.MSGConversation.SetRead(false);
		}

		// Token: 0x06001CB8 RID: 7352 RVA: 0x00077990 File Offset: 0x00075B90
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.UncleNelsonAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.UncleNelsonAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001CB9 RID: 7353 RVA: 0x000779A9 File Offset: 0x00075BA9
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.UncleNelsonAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.UncleNelsonAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001CBA RID: 7354 RVA: 0x000779C2 File Offset: 0x00075BC2
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001CBB RID: 7355 RVA: 0x000779D0 File Offset: 0x00075BD0
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040017A5 RID: 6053
		public string InitialMessage_Demo = "I’ve heard you’re in some trouble. Best not to talk over your mobile phone. Go find a payphone.\n- U.N.";

		// Token: 0x040017A6 RID: 6054
		public string InitialMessage = "You get out alright? Best not to talk over your mobile phone. Go find a payphone.\n- U.N.";

		// Token: 0x040017A7 RID: 6055
		private bool dll_Excuted;

		// Token: 0x040017A8 RID: 6056
		private bool dll_Excuted;
	}
}
