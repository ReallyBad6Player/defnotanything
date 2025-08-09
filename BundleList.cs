﻿using System;

// Token: 0x0200041F RID: 1055
internal class BundleList
{
	// Token: 0x06001992 RID: 6546 RVA: 0x000899EC File Offset: 0x00087BEC
	public void FromJson(string jsonString)
	{
		this.data = JSonHelper.FromJson<BundleData>(jsonString);
		if (this.data.Length == 0)
		{
			return;
		}
		this.activeBundleIdx = 0;
		int num = this.data[0].majorVersion;
		int num2 = this.data[0].minorVersion;
		int num3 = this.data[0].minorVersion2;
		int gameMajorVersion = NetworkSystemConfig.GameMajorVersion;
		int gameMinorVersion = NetworkSystemConfig.GameMinorVersion;
		int gameMinorVersion2 = NetworkSystemConfig.GameMinorVersion2;
		for (int i = 1; i < this.data.Length; i++)
		{
			this.data[i].isActive = false;
			int num4 = gameMajorVersion * 1000000 + gameMinorVersion * 1000 + gameMinorVersion2;
			int num5 = this.data[i].majorVersion * 1000000 + this.data[i].minorVersion * 1000 + this.data[i].minorVersion2;
			if (num4 >= num5 && this.data[i].majorVersion >= num && this.data[i].minorVersion >= num2 && this.data[i].minorVersion2 >= num3)
			{
				this.activeBundleIdx = i;
				num = this.data[i].majorVersion;
				num2 = this.data[i].minorVersion;
				num3 = this.data[i].minorVersion2;
				break;
			}
		}
		this.data[this.activeBundleIdx].isActive = true;
	}

	// Token: 0x06001993 RID: 6547 RVA: 0x00089B8C File Offset: 0x00087D8C
	public bool HasSku(string skuName, out int idx)
	{
		if (this.data == null)
		{
			idx = -1;
			return false;
		}
		for (int i = 0; i < this.data.Length; i++)
		{
			if (this.data[i].skuName == skuName)
			{
				idx = i;
				return true;
			}
		}
		idx = -1;
		return false;
	}

	// Token: 0x06001994 RID: 6548 RVA: 0x00089BDB File Offset: 0x00087DDB
	public BundleData ActiveBundle()
	{
		return this.data[this.activeBundleIdx];
	}

	// Token: 0x06001995 RID: 6549 RVA: 0x00002050 File Offset: 0x00000250
	public BundleList()
	{
	}

	// Token: 0x040021EE RID: 8686
	private int activeBundleIdx;

	// Token: 0x040021EF RID: 8687
	public BundleData[] data;
}
