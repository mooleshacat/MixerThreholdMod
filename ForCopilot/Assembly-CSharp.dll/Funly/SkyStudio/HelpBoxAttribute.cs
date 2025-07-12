using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001F4 RID: 500
	public class HelpBoxAttribute : PropertyAttribute
	{
		// Token: 0x06000B1D RID: 2845 RVA: 0x00030DEB File Offset: 0x0002EFEB
		public HelpBoxAttribute(string text, HelpBoxMessageType messageType = HelpBoxMessageType.None)
		{
			this.text = text;
			this.messageType = messageType;
		}

		// Token: 0x04000BD7 RID: 3031
		public string text;

		// Token: 0x04000BD8 RID: 3032
		public HelpBoxMessageType messageType;
	}
}
