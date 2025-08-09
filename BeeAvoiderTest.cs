using System;
using GorillaExtensions;
using UnityEngine;

// Token: 0x02000109 RID: 265
public class BeeAvoiderTest : MonoBehaviour
{
	// Token: 0x06000695 RID: 1685 RVA: 0x000268F0 File Offset: 0x00024AF0
	public void Update()
	{
		Vector3 position = this.patrolPoints[this.nextPatrolPoint].transform.position;
		Vector3 position2 = base.transform.position;
		Vector3 vector = (position - position2).normalized * this.speed;
		this.velocity = Vector3.MoveTowards(this.velocity * this.drag, vector, this.acceleration);
		if ((position2 - position).IsLongerThan(this.instabilityOffRadius))
		{
			this.velocity += Random.insideUnitSphere * this.instability * Time.deltaTime;
		}
		Vector3 vector2 = position2 + this.velocity * Time.deltaTime;
		GameObject[] array = this.avoidancePoints;
		for (int i = 0; i < array.Length; i++)
		{
			Vector3 position3 = array[i].transform.position;
			if ((vector2 - position3).IsShorterThan(this.avoidRadius))
			{
				Vector3 normalized = Vector3.Cross(position3 - vector2, position - vector2).normalized;
				Vector3 normalized2 = (position - position3).normalized;
				float num = Vector3.Dot(vector2 - position3, normalized);
				Vector3 vector3 = (this.avoidRadius - num) * normalized;
				vector2 += vector3;
				this.velocity += vector3;
			}
		}
		base.transform.position = vector2;
		base.transform.rotation = Quaternion.LookRotation(position - vector2);
		if ((vector2 - position).IsShorterThan(this.patrolArrivedRadius))
		{
			this.nextPatrolPoint = (this.nextPatrolPoint + 1) % this.patrolPoints.Length;
		}
	}

	// Token: 0x06000696 RID: 1686 RVA: 0x000026E9 File Offset: 0x000008E9
	public BeeAvoiderTest()
	{
	}

	// Token: 0x0400080E RID: 2062
	public GameObject[] patrolPoints;

	// Token: 0x0400080F RID: 2063
	public GameObject[] avoidancePoints;

	// Token: 0x04000810 RID: 2064
	public float speed;

	// Token: 0x04000811 RID: 2065
	public float acceleration;

	// Token: 0x04000812 RID: 2066
	public float instability;

	// Token: 0x04000813 RID: 2067
	public float instabilityOffRadius;

	// Token: 0x04000814 RID: 2068
	public float drag;

	// Token: 0x04000815 RID: 2069
	public float avoidRadius;

	// Token: 0x04000816 RID: 2070
	public float patrolArrivedRadius;

	// Token: 0x04000817 RID: 2071
	private int nextPatrolPoint;

	// Token: 0x04000818 RID: 2072
	private Vector3 velocity;
}
