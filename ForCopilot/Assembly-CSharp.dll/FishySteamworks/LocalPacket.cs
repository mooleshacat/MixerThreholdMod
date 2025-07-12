using System;
using FishNet.Utility.Performance;

namespace FishySteamworks
{
	// Token: 0x02000C97 RID: 3223
	internal struct LocalPacket
	{
		// Token: 0x06005A6A RID: 23146 RVA: 0x0017CF38 File Offset: 0x0017B138
		public LocalPacket(ArraySegment<byte> data, byte channel)
		{
			this.Data = ByteArrayPool.Retrieve(data.Count);
			this.Length = data.Count;
			Buffer.BlockCopy(data.Array, data.Offset, this.Data, 0, this.Length);
			this.Channel = channel;
		}

		// Token: 0x04004270 RID: 17008
		public byte[] Data;

		// Token: 0x04004271 RID: 17009
		public int Length;

		// Token: 0x04004272 RID: 17010
		public byte Channel;
	}
}
