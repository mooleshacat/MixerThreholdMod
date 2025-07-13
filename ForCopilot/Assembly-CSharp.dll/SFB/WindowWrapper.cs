using System;
using System.Windows.Forms;

namespace SFB
{
	// Token: 0x02000173 RID: 371
	public class WindowWrapper : IWin32Window
	{
		// Token: 0x06000720 RID: 1824 RVA: 0x000201AF File Offset: 0x0001E3AF
		public WindowWrapper(IntPtr handle)
		{
			this._hwnd = handle;
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000721 RID: 1825 RVA: 0x000201BE File Offset: 0x0001E3BE
		public IntPtr Handle
		{
			get
			{
				return this._hwnd;
			}
		}

		// Token: 0x040007E4 RID: 2020
		private IntPtr _hwnd;
	}
}
