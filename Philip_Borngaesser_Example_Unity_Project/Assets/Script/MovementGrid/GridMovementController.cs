using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GridMovementController : MonoBehaviour
{
    public bool isPlayer;
    private float _distanceToOtherNode;
    [HideInInspector]public MovementNode beforeCurrentMovementNode;
    public MovementNode currentMovementNode;
    public MovementNode currentInteractNode;

    public float nodeMoveSpeed = 3f;
    private float _currentNodeMoveSpeed = 0f;
    public bool placeOnMovementGridOnStart = true;
    public AnimationCurve movementCurve = AnimationCurve.Linear(0,1,1,1);
    public AnimationCurve interactingCurve = AnimationCurve.Linear(0, 1, 1, 1);
    [HideInInspector]public bool isMoving;
    [HideInInspector] public bool isInteracting;
    [HideInInspector] public bool stopMovement = false;
    bool _moveBetweenNode;
    //Transorm is this transform
    [HideInInspector] public UnityEvent<Transform> OnOccupiedGridMovementRequest;
    //Transorm is the occupiedObject
    [HideInInspector] public UnityEvent<Transform> OnOccupiedGridMovementBackMessage = new UnityEvent<Transform>();
    [HideInInspector] public UnityEvent OnGridMovementStart;
    [HideInInspector] public UnityEvent OnGridMovementEnd;
    [HideInInspector] public UnityEvent<MovementNode> OnGridInteractStart = new UnityEvent<MovementNode>();
    [HideInInspector] public UnityEvent<MovementNode> OnGridInteractEnd = new UnityEvent<MovementNode>();
    [HideInInspector] public UnityEvent OnMovementNotAllowed;


    public bool extraAnimation;
    [Header("Movement")]
    [ConditionalHide("extraAnimation", true)]  public GameObject spriteGameobject;
    [ConditionalHide("extraAnimation", true)]  public Vector3 startScale_Movement = new Vector3(1.5f,0.5f,1f);
    [ConditionalHide("extraAnimation", true)]  public Vector3 topScale_Movement = new Vector3(0.5f, 1.5f, 1f);
    [ConditionalHide("extraAnimation", true)]  public float peekScalePercantage = 0.5f;

    [ConditionalHide("extraAnimation", true)] public Vector3 upPos = new Vector3(0,0.5f,0);
    Vector3 _startPos;
    [ConditionalHide("extraAnimation", true)]  public bool lookAtMoveDirection = true;

    [HideInInspector]
    [ConditionalHide("extraAnimation", true)]
    public bool isRight;

    [Header("Interacting")]
    [ConditionalHide("extraAnimation", true)] public Vector3 startScale_Interacting = new Vector3(1.5f, 0.5f, 1f);
    [ConditionalHide("extraAnimation", true)] public Vector3 topScale_Interacting = new Vector3(0.5f, 1.5f, 1f);

    void Start()
    {
        if (placeOnMovementGridOnStart)
        {
            PlaceOnNearestGridNode();
        }
        isRight = true;
        isMoving = false;
        _currentNodeMoveSpeed = nodeMoveSpeed;
        _distanceToOtherNode = MovementGrid.instance.movementNodeSize;
        if(extraAnimation) _startPos = spriteGameobject.transform.localPosition;
    }

    private void OnDisable()
    {
        GiveCurrentNodeFree();
    }

    public void PlaceOnNearestGridNode()
    {
        currentMovementNode = MovementGrid.instance.WorldToMovementNode(transform.position);
        currentMovementNode.isOccupied = true;
        currentMovementNode.occupiedObject = this;
        OnGridMovementStart.Invoke();
        transform.position = currentMovementNode.transform.position;
    }

    public void SetPositionnToMovementNode(MovementNode node)
    {
        transform.position = node.transform.position;
        currentMovementNode = node;
        currentMovementNode.isOccupied = true;
        currentMovementNode.occupiedObject = this;
    }

    void Update()
    {
        #region NormalMovement
        if (isMoving)
        {
            Vector3 dir = currentMovementNode.transform.position - transform.position;
            dir = dir.normalized;

            float currentDistance = Vector3.Distance(transform.position, currentMovementNode.transform.position);
            float percentValue = 1 - (currentDistance / _distanceToOtherNode);

            Vector3 movementVec = dir * movementCurve.Evaluate(percentValue) * _currentNodeMoveSpeed * Time.deltaTime;
            transform.position += movementVec;

            if (extraAnimation)
            {
                if (percentValue < peekScalePercantage)
                {
                    float newPercentageValue = percentValue / peekScalePercantage;

                    if (extraAnimation)
                    {
                        if (isRight) spriteGameobject.transform.localScale = Vector3.Lerp(startScale_Movement, topScale_Movement, newPercentageValue);
                        else spriteGameobject.transform.localScale = Vector3.Lerp(new Vector3(-startScale_Movement.x, startScale_Movement.y, startScale_Movement.z), new Vector3(-topScale_Movement.x, topScale_Movement.y, topScale_Movement.z), newPercentageValue);
                    }
                    spriteGameobject.transform.localPosition = Vector3.Lerp(_startPos, upPos, newPercentageValue);

                }
                else
                {
                    float newPercentageValue = (percentValue - peekScalePercantage) / peekScalePercantage;

                    if (extraAnimation)
                    {
                        if (isRight) spriteGameobject.transform.localScale = Vector3.Lerp(topScale_Movement, startScale_Movement, newPercentageValue);
                        else spriteGameobject.transform.localScale = Vector3.Lerp(new Vector3(-topScale_Movement.x, topScale_Movement.y, topScale_Movement.z), new Vector3(-startScale_Movement.x, startScale_Movement.y, startScale_Movement.z), newPercentageValue);
                    }

                    spriteGameobject.transform.localPosition = Vector3.Lerp(upPos, _startPos, newPercentageValue);
                }
            }


            if (Vector3.Distance(transform.position, currentMovementNode.transform.position) < 0.1)
            {
                transform.position = currentMovementNode.transform.position;
                currentMovementNode.onArrivedAtNode.Invoke(isPlayer);
                if (extraAnimation)
                {
                    if (extraAnimation)
                    {
                        if (isRight) spriteGameobject.transform.localScale = Vector3.one;
                        else spriteGameobject.transform.localScale = new Vector3(-1, 1, 1);
                    }
                    spriteGameobject.transform.localPosition = _startPos;
                }
                isMoving = false;
                OnGridMovementEnd.Invoke();
            }
        }
        #endregion

        if (isInteracting)
        {
            Vector3 dir = Vector3.zero;
            float percentValue = 0;

            if (_moveBetweenNode)
            {
                dir = currentInteractNode.transform.position - transform.position;
                dir = dir.normalized;
                float currentDistance = Vector3.Distance(transform.position, currentInteractNode.transform.position);
                percentValue = 1 - (currentDistance / _distanceToOtherNode);

                if (percentValue > 0.5f) _moveBetweenNode = false;

                
                if (extraAnimation)
                {
                    float newPercentageValue = percentValue;
                    if (extraAnimation)
                    {
                        if (isRight) spriteGameobject.transform.localScale = Vector3.Lerp(startScale_Interacting, topScale_Interacting, newPercentageValue);
                        else spriteGameobject.transform.localScale = Vector3.Lerp(new Vector3(-startScale_Interacting.x, startScale_Interacting.y, startScale_Interacting.z), new Vector3(-topScale_Interacting.x, topScale_Interacting.y, topScale_Interacting.z), newPercentageValue);
                    }
                    //spriteGameobject.transform.localPosition = Vector3.Lerp(_startPos, upPos, newPercentageValue);
                }
                

            }
            else
            {
                dir = currentMovementNode.transform.position - transform.position;
                dir = dir.normalized;
                float currentDistance = Vector3.Distance(transform.position, currentMovementNode.transform.position);
                percentValue = 1 - (currentDistance / _distanceToOtherNode);

                
                if (extraAnimation)
                {
                    float newPercentageValue = (percentValue - peekScalePercantage) / peekScalePercantage;
                    if (extraAnimation) { 
                        if (isRight) spriteGameobject.transform.localScale = Vector3.Lerp(topScale_Interacting, startScale_Interacting, newPercentageValue);
                        else spriteGameobject.transform.localScale = Vector3.Lerp(new Vector3(-topScale_Interacting.x, topScale_Interacting.y, topScale_Interacting.z), new Vector3(-startScale_Interacting.x, startScale_Interacting.y, startScale_Interacting.z), newPercentageValue);
                    }
                    //spriteGameobject.transform.localPosition = Vector3.Lerp(upPos, _startPos, newPercentageValue);
                }

                
                if (percentValue > 0.98f)
                {
                    transform.position = currentMovementNode.transform.position;
                    if (extraAnimation)
                    {
                        if (isRight) spriteGameobject.transform.localScale = Vector3.one;
                        else spriteGameobject.transform.localScale = new Vector3(-1, 1, 1);
                    }
                    //spriteGameobject.transform.localPosition = _startPos;
                    OnGridInteractEnd.Invoke(currentInteractNode);
                    isInteracting = false;
                }

            }

            Vector3 movementVec = dir * interactingCurve.Evaluate(percentValue) * _currentNodeMoveSpeed * Time.deltaTime;
            transform.position += movementVec;

        }

    }


    private void OnInteract(MovementNode movementNode)
    {
        currentInteractNode = movementNode;
        isInteracting = true;
        _moveBetweenNode = true;
        OnGridInteractStart.Invoke(currentInteractNode);
    }

    public void LookToTheRightSide()
    {
        spriteGameobject.transform.localScale = Vector3.one;
        isRight = true;
    }

    public void LookToTheLeftSide()
    {
        spriteGameobject.transform.localScale = new Vector3(-1,1,1);
        isRight = false;
    }

    float MIN_DIR_ANGLE = 5f;

    public void MoveDependingOnDirVector(Vector2 dir)
    {
        MoveDependingOnDirVector(new Vector3(dir.x, dir.y, 0));
    }

    public void MoveDependingOnDirVector(Vector3 dir)
    {
        if (Vector3.Angle(dir,Vector2.up) < MIN_DIR_ANGLE)
        {
            MoveToDownNode();
        }
        else if(Vector3.Angle(dir, -Vector2.up) < MIN_DIR_ANGLE)
        {
            MoveToUpNode();
        }
        else if(Vector3.Angle(dir, Vector2.right) < MIN_DIR_ANGLE)
        {
            MoveToLeftNode();
        }
        else
        {
            MoveToRightNode();
        }
    }

    public void MoveToTargetNode(MovementNode movementNode)
    {
        if (movementNode == null) return;
        if (stopMovement) return;
        if (!movementNode.isWalkable)
        {
            OnMovementNotAllowed.Invoke();
            return;
        }
        if (movementNode.isOccupied)
        {
            OnInteract(movementNode);
            currentMovementNode.occupiedRequesterObject = this;
            movementNode?.occupiedObject?.OnOccupiedGridMovementRequest?.Invoke(transform);
            if(movementNode.occupiedObject != null)
                OnOccupiedGridMovementBackMessage.Invoke(movementNode.occupiedObject.transform);
            return;
        }
        else
        {
            _distanceToOtherNode = Vector2.Distance(transform.position, movementNode.transform.position);
            isMoving = true;
            if (currentMovementNode != null)
            {
                currentMovementNode.isOccupied = false;
                currentMovementNode.occupiedObject = null;
                currentMovementNode.onAfterLeftNode.Invoke(isPlayer);
            }
            beforeCurrentMovementNode = currentMovementNode;
            currentMovementNode = movementNode;
            currentMovementNode.isOccupied = true;
            currentMovementNode.occupiedObject = this;
            Vector2 dir = currentMovementNode.transform.position - transform.position;
            dir = dir.normalized;
            if(dir == Vector2.right) isRight = true;
            else isRight = false;

            OnGridMovementStart.Invoke();
        }
        _currentNodeMoveSpeed = nodeMoveSpeed;

    }

    public void MoveToTargetNodeImmediately(MovementNode movementNode)
    {
        MoveToTargetNode(movementNode);
        _currentNodeMoveSpeed = 25;
    }

    public void MoveToUpNode()
    {
        MoveToTargetNode(currentMovementNode.upNeighbor);
    }

    public void MoveToDownNode()
    {
        MoveToTargetNode(currentMovementNode.downNeighbor);
    }

    public void MoveToRightNode()
    {
        if(lookAtMoveDirection) isRight = true;
        MoveToTargetNode(currentMovementNode.rightNeighbor);
    }

    public void MoveToLeftNode()
    {
        if (lookAtMoveDirection) isRight = false;
        MoveToTargetNode(currentMovementNode.leftNeighbor);
    }



    public MovementNode GetFarAwayDirectionUpNode(int iteration)
    {
        MovementNode movementNode = currentMovementNode;
        for(int i = 0; i< iteration; i++)
        {
            movementNode = movementNode.upNeighbor;
        }
        return movementNode;

    }

    public MovementNode GetFarAwayDirectionDownNode(int iteration)
    {
        MovementNode movementNode = currentMovementNode;
        for (int i = 0; i < iteration; i++)
        {
            movementNode = movementNode.downNeighbor;
        }
        return movementNode;
    }

    public MovementNode GetFarAwayDirectionRightNode(int iteration)
    {
        MovementNode movementNode = currentMovementNode;
        for (int i = 0; i < iteration; i++)
        {
            movementNode = movementNode.rightNeighbor;
        }
        return movementNode;

    }

    public MovementNode GetFarAwayDirectionLeftNode(int iteration)
    {
        MovementNode movementNode = currentMovementNode;
        for (int i = 0; i < iteration; i++)
        {
            movementNode = movementNode.leftNeighbor;
        }
        return movementNode;

    }

    public void StopMovement()
    {
        stopMovement = true;
    }

    public void ResumeMovement()
    {
        stopMovement = false;
    }

    public void GiveCurrentNodeFree()
    {
        if (currentMovementNode == null) return;
        currentMovementNode.isOccupied = false;
        currentMovementNode.occupiedObject = null;
        currentMovementNode = null;
        currentInteractNode = null;
    }

    
}
