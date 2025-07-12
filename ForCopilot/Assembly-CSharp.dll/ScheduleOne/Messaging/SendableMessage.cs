using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.Messaging
{
	// Token: 0x0200058E RID: 1422
	public class SendableMessage
	{
		// Token: 0x0600227D RID: 8829 RVA: 0x0008E89F File Offset: 0x0008CA9F
		public SendableMessage(string text, MSGConversation conversation)
		{
			this.Text = text;
			this.conversation = conversation;
		}

		// Token: 0x0600227E RID: 8830 RVA: 0x0008E8C0 File Offset: 0x0008CAC0
		public virtual bool ShouldShow()
		{
			return this.ShouldShowCheck == null || this.ShouldShowCheck(this);
		}

		// Token: 0x0600227F RID: 8831 RVA: 0x0008E8D8 File Offset: 0x0008CAD8
		public virtual bool IsValid(out string invalidReason)
		{
			if (this.IsValidCheck != null)
			{
				return this.IsValidCheck(this, out invalidReason);
			}
			invalidReason = "";
			return true;
		}

		// Token: 0x06002280 RID: 8832 RVA: 0x0008E8F8 File Offset: 0x0008CAF8
		public virtual void Send(bool network, int id = -1)
		{
			if (id != -1)
			{
				if (this.sentIDs.Contains(id))
				{
					return;
				}
			}
			else
			{
				id = UnityEngine.Random.Range(0, int.MaxValue);
			}
			if (this.onSelected != null)
			{
				this.onSelected();
			}
			if (this.disableDefaultSendBehaviour)
			{
				return;
			}
			if (network)
			{
				this.conversation.SendPlayerMessage(this.conversation.Sendables.IndexOf(this), id, true);
				return;
			}
			this.sentIDs.Add(id);
			this.conversation.RenderPlayerMessage(this);
			if (this.onSent != null)
			{
				this.onSent();
			}
		}

		// Token: 0x04001A2F RID: 6703
		public string Text;

		// Token: 0x04001A30 RID: 6704
		public SendableMessage.BoolCheck ShouldShowCheck;

		// Token: 0x04001A31 RID: 6705
		public SendableMessage.ValidityCheck IsValidCheck;

		// Token: 0x04001A32 RID: 6706
		public Action onSelected;

		// Token: 0x04001A33 RID: 6707
		public Action onSent;

		// Token: 0x04001A34 RID: 6708
		private MSGConversation conversation;

		// Token: 0x04001A35 RID: 6709
		public bool disableDefaultSendBehaviour;

		// Token: 0x04001A36 RID: 6710
		private List<int> sentIDs = new List<int>();

		// Token: 0x0200058F RID: 1423
		// (Invoke) Token: 0x06002282 RID: 8834
		public delegate bool BoolCheck(SendableMessage message);

		// Token: 0x02000590 RID: 1424
		// (Invoke) Token: 0x06002286 RID: 8838
		public delegate bool ValidityCheck(SendableMessage message, out string invalidReason);
	}
}
