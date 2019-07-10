using UnityEngine;

public class IdlingMovement
{
    private Vector3 idleCenterPos;
    private float min, max, distX, distZ;   
    private Vector3 idlePos = new Vector3(42, 0, 47);
    LocationTarget targetLocationType;

    public Vector3 GetNewDestination(Human _human, LocationTarget _targetLocationType)
    {
        targetLocationType = _targetLocationType;
        if (targetLocationType == LocationTarget.Idle)
        {
            idleCenterPos = GameObject.Find("BuildingOilProduction").transform.position;
            min = 5f;           
        }
        else
        {
            idleCenterPos = _human.LocationService[targetLocationType].transform.position;
            min = _human.LocationService[targetLocationType].GetComponent<Building>().GetInteractionRadius();         
        }

        max = min + 10;        
        GetValidPos();

        Vector3 newDest = idleCenterPos + new Vector3(distX, 0, distZ);

        while (CheckCollisions(newDest, _human))
        {
            GetValidPos();
            newDest = idleCenterPos + new Vector3(distX, 0, distZ);
        }      

        return newDest;
    }   

    float GetRandimzedValue()
    {
      return Random.Range(-max, max);
    }
    bool CheckBounds(float value)
    {
        if(value > -min && value < min)
        {
            return false;
        }
        return true;
    }

    void GetValidPos()
    {
        distX = GetRandimzedValue();
        distZ = GetRandimzedValue();

        while (!CheckBounds(distX))
        {
            distX = GetRandimzedValue();
        }

        while (!CheckBounds(distZ))
        {
            distZ = GetRandimzedValue();
        }
    }

    public bool CheckCollisions(Vector3 _newDest, Human _human)
    {
        Ray ray = new Ray(_newDest, _human.transform.position);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, 5f) == false)
        {
            return false;
        }
        else
        {
            if (hit.transform.tag != "Building" || hit.transform.tag != "BuildingOilProduction" || hit.transform.tag != "BuildingResourceDepot")       
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
