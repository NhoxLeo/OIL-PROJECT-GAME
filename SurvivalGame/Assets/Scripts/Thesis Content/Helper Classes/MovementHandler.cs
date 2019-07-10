using System;
using UnityEngine;
using UnityEngine.AI;

public class MovementHandler : MonoBehaviour
{
    private Movement move;
    private Human owner;
    private readonly bool targetLocationChanged;
    public LocationTarget Target { get; private set; }
    public bool DoOnTargetAction { get; private set; }

    private void Awake()
    {
        owner = gameObject.GetComponent<Human>();
        move = new Movement(gameObject, owner.moveToLocation);
    }

    public void TryMove(Vector3 pos)
    {
        try
        {
            move.PrepareMove(pos);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public void NewDestination(LocationTarget newTarget)
    {       
        Target = newTarget;
        owner.TargetLocationChanged = true;
    }

    public void TryHaltAndHide()
    {
        try
        {
            move.HaltAndHide();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public void TryHalt()
    {
        try
        {
            move.Halt();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public void CheckPoximity()
    {
        try
        {          
           DoOnTargetAction = move.CheckIfNearEnterableTarget(Target);           
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public void SetNewIdlePos(Vector3 _idlePos)
    {
        move.idlePos = _idlePos;
    }

    public bool CheckIfStuck()
    {
        return move.IsStuck();
    }
}
