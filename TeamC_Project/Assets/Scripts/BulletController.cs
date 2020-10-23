using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {
	[SerializeField]
	private GameObject[] bulletPrefabs;

	public void GenerateBullet(ChargeBullet.ChargeMode chargeStage, Vector3 position, Vector3 direction, float speed, float destroyTime)
    {
		//チャージ段階に応じて弾を変える
		GameObject obj;
		if (chargeStage == ChargeBullet.ChargeMode.STAGE_1 || chargeStage==ChargeBullet.ChargeMode.STAGE_2)
			obj = bulletPrefabs[0];
		else if (chargeStage == ChargeBullet.ChargeMode.STAGE_3)
			obj = bulletPrefabs[1];
		else
			obj = bulletPrefabs[2];

		GameObject bullet = Instantiate(obj, position, Quaternion.identity);
		bullet.GetComponent<Rigidbody>().AddForce(direction.normalized * speed);
		if (chargeStage == ChargeBullet.ChargeMode.STAGE_3 || chargeStage == ChargeBullet.ChargeMode.STAGE_4)
			bullet.GetComponent<BulletCollision>().IsPenetrate = true;
		Destroy(bullet, destroyTime);

		//2段階目のときだけ、3wayにする
        if (chargeStage == ChargeBullet.ChargeMode.STAGE_2)
        {
			GameObject bullet2 = Instantiate(obj, position, Quaternion.identity);
			bullet2.GetComponent<Rigidbody>().AddForce((direction + Vector3.right).normalized * speed);
			Destroy(bullet2, destroyTime);
			GameObject bullet3 = Instantiate(obj, position, Quaternion.identity);
			bullet3.GetComponent<Rigidbody>().AddForce((direction + Vector3.left).normalized * speed);
			Destroy(bullet3, destroyTime);
        }
    }
}