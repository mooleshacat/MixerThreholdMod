using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence.Loaders;
using UnityEngine;

namespace ScheduleOne.Persistence
{
	// Token: 0x02000392 RID: 914
	public class LoadRequest
	{
		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x060014B9 RID: 5305 RVA: 0x0005BDB8 File Offset: 0x00059FB8
		// (set) Token: 0x060014BA RID: 5306 RVA: 0x0005BDC0 File Offset: 0x00059FC0
		public bool IsDone { get; private set; }

		// Token: 0x060014BB RID: 5307 RVA: 0x0005BDC9 File Offset: 0x00059FC9
		public LoadRequest(string filePath, Loader loader)
		{
			if (loader == null)
			{
				Debug.LogError("Loader is null for file path: " + filePath);
				return;
			}
			this.Path = filePath;
			this.Loader = loader;
			Singleton<LoadManager>.Instance.QueueLoadRequest(this);
		}

		// Token: 0x060014BC RID: 5308 RVA: 0x0005BDFE File Offset: 0x00059FFE
		public void Complete()
		{
			Singleton<LoadManager>.Instance.DequeueLoadRequest(this);
			this.Loader.Load(this.Path);
			this.IsDone = true;
		}

		// Token: 0x0400135F RID: 4959
		public string Path;

		// Token: 0x04001360 RID: 4960
		public Loader Loader;
	}
}
