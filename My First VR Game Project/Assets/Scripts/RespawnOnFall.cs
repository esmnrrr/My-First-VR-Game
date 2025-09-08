using UnityEngine;

public class RespawnOnFall : MonoBehaviour
{
    public Transform respawnPoint;
    public float killY = -10f;
    XRJump jump;

    void Awake(){ jump = GetComponent<XRJump>(); }

    void Update()
    {
        if (transform.position.y < killY)
        {
            var cc = GetComponent<CharacterController>();
            cc.enabled = false;
            transform.position = respawnPoint.position;
            if (jump) jump.ResetVertical();
            cc.enabled = true;
        }
    }
}
