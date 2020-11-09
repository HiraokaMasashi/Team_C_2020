using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour
{
	[SerializeField]
	private GameObject bulletPrefab;
	private SoundManager soundManager;
	[SerializeField]
	private string shotSe;

	private void Start()
	{
		soundManager = SoundManager.Instance;
	}


	public void GenerateBullet(Vector3 position, Vector3 direction, 
		float speed, float destroyTime, string tagName = "Player")
	{
		//チャージ段階に応じて弾を変える
		//GameObject obj = bulletPrefab;
		//if (chargeStage == ChargeBullet.ChargeMode.STAGE_1 || chargeStage == ChargeBullet.ChargeMode.STAGE_2)
		//{
		//	obj = bulletPrefabs[0];
		//	shotSe = shotSes[0];
		//}
		//else if (chargeStage == ChargeBullet.ChargeMode.STAGE_3)
		//{
		//	obj = bulletPrefabs[1];
		//	shotSe = shotSes[1];
		//}
		//else
		//{
		//	obj = bulletPrefabs[2];
		//	shotSe = shotSes[2];
		//}

		GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
		bullet.tag = tagName + "Bullet";
		bullet.GetComponent<Rigidbody>().AddForce(direction.normalized * speed);
		//if (chargeStage == ChargeBullet.ChargeMode.STAGE_3 || chargeStage == ChargeBullet.ChargeMode.STAGE_4)
		//	bullet.GetComponent<BulletCollision>().IsPenetrate = true;
		Destroy(bullet, destroyTime);

		//2段階目のときだけ、3wayにする
		//if (chargeStage == ChargeBullet.ChargeMode.STAGE_2)
		//{
		//	GameObject bullet2 = Instantiate(obj, position, Quaternion.identity);
		//	bullet2.GetComponent<Rigidbody>().AddForce((direction + Vector3.right).normalized * speed);
		//	bullet2.tag = tagName + "Bullet";
		//	Destroy(bullet2, destroyTime);
		//	GameObject bullet3 = Instantiate(obj, position, Quaternion.identity);
		//	bullet3.GetComponent<Rigidbody>().AddForce((direction + Vector3.left).normalized * speed);
		//	bullet3.tag = tagName + "Bullet";
		//	Destroy(bullet3, destroyTime);
		//}

		//if (shotSe == "") return;
		soundManager.PlaySeByName(shotSe);
	}
}