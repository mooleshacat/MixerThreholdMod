using System;

namespace ScheduleOne
{
	// Token: 0x02000278 RID: 632
	public interface IGUIDRegisterable
	{
		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000D37 RID: 3383
		Guid GUID { get; }

		// Token: 0x06000D38 RID: 3384 RVA: 0x0003A6E4 File Offset: 0x000388E4
		void SetGUID(string guid)
		{
			Guid guid2;
			if (Guid.TryParse(guid, out guid2))
			{
				this.SetGUID(guid2);
				return;
			}
			Console.LogWarning(guid + " is not a valid GUID.", null);
		}

		// Token: 0x06000D39 RID: 3385
		void SetGUID(Guid guid);
	}
}
