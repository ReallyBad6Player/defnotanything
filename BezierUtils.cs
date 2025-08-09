using System;
using UnityEngine;

// Token: 0x02000A9F RID: 2719
public class BezierUtils
{
	// Token: 0x060041D2 RID: 16850 RVA: 0x0014B76C File Offset: 0x0014996C
	public static Vector3 BezierSolve(float t, Vector3 startPos, Vector3 ctrl1, Vector3 ctrl2, Vector3 endPos)
	{
		float num = 1f - t;
		float num2 = num * num * num;
		float num3 = 3f * num * num * t;
		float num4 = 3f * num * t * t;
		float num5 = t * t * t;
		return startPos * num2 + ctrl1 * num3 + ctrl2 * num4 + endPos * num5;
	}

	// Token: 0x060041D3 RID: 16851 RVA: 0x00002050 File Offset: 0x00000250
	public BezierUtils()
	{
	}
}
