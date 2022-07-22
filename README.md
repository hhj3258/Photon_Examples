# Photon_Examples

## Photon RPC Bullet Code

```csharp
    /* PlayerScript.cs */
    
    private void Update()
    {
        ...

        bool fire = Input.GetMouseButton(0);
        if (fire)
        {
            // RpcTarget.AllViaServer = RPC 호출을 서버에게 요청하고 서버가 '나'를 포함한 모든 클라이언트에게 '순서'대로 쏴준다
            photonView.RPC("Fire", RpcTarget.AllViaServer, rigidbody.position, rigidbody.rotation);
        }
    }
    
    [PunRPC]
    public void Fire(Vector3 position, Quaternion rotation, PhotonMessageInfo info)
    {
        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);
        GameObject bullet;
        
        /** Use this if you want to fire one bullet at a time **/
        // 불릿 생성
        bullet = Instantiate(BulletPrefab, gunOffset.position, Quaternion.identity) as GameObject;
        // 불릿 초기화 로직
        bullet.GetComponent<BulletCtrl>().InitializeBullet(photonView.Owner, (rotation * Vector3.forward), Mathf.Abs(lag));
    }
```

```csharp
    /* BulletCtrl.cs */

    // 불릿 초기화
    // lag = time lag. 서버 시간 - RPC 발신자의 발신 당시의 서버 시간
    public void InitializeBullet(Player owner, Vector3 originalDirection, float lag)
    {
        Owner = owner;

        transform.forward = originalDirection;

        Rigidbody rigidbody = GetComponent<Rigidbody>();

        // 속도 = 방향 * 속력
        rigidbody.velocity = originalDirection * 100.0f;
        // 초기 총알 생성 위치 += 속도 * 타임랙
        rigidbody.position += rigidbody.velocity * lag;
    }
```

