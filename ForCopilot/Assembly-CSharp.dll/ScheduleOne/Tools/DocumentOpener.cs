using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x0200088F RID: 2191
	public class DocumentOpener : MonoBehaviour
	{
		// Token: 0x06003BCA RID: 15306 RVA: 0x000FCDAF File Offset: 0x000FAFAF
		public void Open()
		{
			Singleton<DocumentViewer>.Instance.Open(this.DocumentName);
		}

		// Token: 0x04002AB7 RID: 10935
		public string DocumentName;
	}
}
