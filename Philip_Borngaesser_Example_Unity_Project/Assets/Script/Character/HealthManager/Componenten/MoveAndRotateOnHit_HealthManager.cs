using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthManager))]
public class MoveAndRotateOnHit_HealthManager : MonoBehaviour
{
    HealthManager _healthManager;
    float _nodeSize;
    bool isMoving;
    bool isMovingBack;
    Vector3 _startPos;
    Vector3 _endPos;
    Vector3 _moveBackDir;
    public Transform spriteRoot;
    public float moveDistance = 0.75f;
    public float moveSpeed = 6f;

    void Start()
    {
        _healthManager = GetComponent<HealthManager>();
        _healthManager.OnCalculateDamage.AddListener(OnDamageTaken);
        _nodeSize = MovementGrid.instance.movementNodeSize;
    }

    void OnDamageTaken(bool isDead, int damage, Transform hitpos)
    {
        Vector3 dir = hitpos.transform.position - transform.position;
        if (Vector3.Angle(dir, Vector3.up) < 5f) MoveAndRotate(Vector3.down);
        else if (Vector3.Angle(dir, Vector3.down) < 5f) MoveAndRotate(Vector3.up);
        else if (Vector3.Angle(dir, Vector3.left) < 5f) MoveAndRotate(Vector3.right);
        else if (Vector3.Angle(dir, Vector3.right) < 5f) MoveAndRotate(Vector3.left);
    }

    void MoveAndRotate(Vector3 direction)
    {
        _startPos = transform.position;
        _endPos = direction * moveDistance + transform.position;
        _startPos = transform.position;
        _moveBackDir = direction;
        isMoving = true;
        isMovingBack = true;
    }

    void ResetMovement()
    {
        transform.position = _startPos;
    }

    private void Update()
    {
        if (isMoving)
        {
            Vector3 dir = Vector3.zero;
            float percentValue = 0;

            if (isMovingBack)
            {
                dir = _moveBackDir * moveDistance;
                float currentDistance = Vector3.Distance(spriteRoot.transform.position, _endPos);
                percentValue = 1 - (currentDistance / _nodeSize);

                if (percentValue > 0.5f) isMovingBack = false;
            }
            else
            {
                dir = _startPos - spriteRoot.transform.position;
                float currentDistance = Vector3.Distance(spriteRoot.transform.position,_startPos);
                percentValue = 1 - (currentDistance / _nodeSize);


                if (percentValue > 0.98f)
                {
                    ResetMovement();
                }

            }

            Vector3 movementVec = dir * moveSpeed * Time.deltaTime;
            spriteRoot.transform.position += movementVec;

        }
    }
}
