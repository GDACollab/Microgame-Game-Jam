using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperGolf_ClubController : MonoBehaviour
{
    public Transform clubPosition;
    public SniperGolf_GolfHit golfController;
    SniperGolf_PowerMinigameController minigame;
    Vector3 targetPos;
    Vector3 targRot;
    Vector3 golfSwingRot;
    float snapBackSpeed = 1;
    // Start is called before the first frame update
    void Start()
    {
        minigame = golfController.powerMinigame;
        minigame.hitSpaceUpdate.AddListener(AddToTargetRot);
        minigame.hitSpace.AddListener(SnapBack);
    }

    void AddToTargetRot(float progress) {
        golfSwingRot = new Vector3(0, 0, -progress * 90);
    }

    void SnapBack(float progress) {
        golfSwingRot = Vector3.zero;
        snapBackSpeed = 5.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (golfController.canHit)
        {
            targetPos = golfController.transform.position - (golfController.transform.position - golfController.player.transform.position).normalized *0.3f;
            var targetRot = golfController.transform.position - this.transform.position;
            var angle = Mathf.Atan2(targetRot.y, targetRot.x) * Mathf.Rad2Deg;
            targRot = new Vector3(0, 0, angle);
        }
        else
        {
            targetPos = clubPosition.position;
            targRot = Vector3.zero;
        }
        var target = Vector3.Lerp(this.transform.position, targetPos, Time.deltaTime * 3);
        this.transform.position = new Vector3(target.x, target.y, this.transform.position.z);
        if (snapBackSpeed > 1) {
            snapBackSpeed -= Time.deltaTime * 3;
        }

        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(targRot + golfSwingRot), Time.deltaTime * 3 * snapBackSpeed);
    }
}
