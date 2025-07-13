using System;
using FishNet.Serializing.Helping;

namespace ScheduleOne.Messaging
{
	// Token: 0x0200058D RID: 1421
	[Serializable]
	public class Response
	{
		// Token: 0x0600227B RID: 8827 RVA: 0x0008E87A File Offset: 0x0008CA7A
		public Response(string _text, string _label, Action _callback = null, bool _disableDefaultResponseBehaviour = false)
		{
			this.text = _text;
			this.label = _label;
			this.callback = _callback;
			this.disableDefaultResponseBehaviour = _disableDefaultResponseBehaviour;
		}

		// Token: 0x0600227C RID: 8828 RVA: 0x0000494F File Offset: 0x00002B4F
		public Response()
		{
		}

		// Token: 0x04001A2B RID: 6699
		public string text;

		// Token: 0x04001A2C RID: 6700
		public string label;

		// Token: 0x04001A2D RID: 6701
		[CodegenExclude]
		public Action callback;

		// Token: 0x04001A2E RID: 6702
		public bool disableDefaultResponseBehaviour;
	}
}
