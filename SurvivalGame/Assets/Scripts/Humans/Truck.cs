using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// A truck driving resources back and forth. Has no real functionality but immersion.
/// </summary>
public class Truck : MonoBehaviour {
    private Transform workSite, deliverySite;
    private bool operational, inactive, isStopped;
    private NavMeshAgent agent;
    //private Quaternion targetRotation;

    /// <summary>
    /// Initializes a truck.
    /// </summary>
    /// <param name="workSite">Where the truck picks up resources (forest etc.)</param>
    /// <param name="deliverySite">Where the truck delivers the resources (sawmill etc.)</param>
    public void Initialize(Transform workSite, Transform deliverySite) {
        this.workSite = workSite;
        this.deliverySite = deliverySite;
        operational = true;
        inactive = false;
        isStopped = false;
        agent = GetComponent<NavMeshAgent>();
        foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
            ps.Play();
    }

    /// <summary>
    /// Updates the truck, making it go back and forth from the work site to the delivery site.
    /// </summary>
    public void Update() {
        if (operational) {  // Drive back and forth.
            if (Vector3.Distance(workSite.position, transform.position) < 7f)
                agent.SetDestination(deliverySite.position);
            if (Vector3.Distance(deliverySite.position, transform.position) < deliverySite.GetComponent<BuildingProduction>().GetInteractionRadius())
                agent.SetDestination(workSite.position);            
        }
        else if (!operational && !inactive) {   // Truck is ordered to remain inactive.
            agent.SetDestination(deliverySite.position);
            inactive = true;
        }
        else if (inactive && operational) { // Truck is returned to service.
            agent.SetDestination(workSite.position);
            inactive = false;
        }
        if (agent.isStopped && !isStopped) {
            foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
                ps.Stop();
            isStopped = true;
        }
        else if (!agent.isStopped && isStopped) {
            foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
                ps.Play();
            isStopped = false;
        }
        /*RaycastHit hit;   // For lol-driving.
        Vector3 ray = transform.TransformDirection(Vector3.down);
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), ray, out hit, 20)) {
            Vector3 normal = hit.normal;
            targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.1f / Time.deltaTime);*/
    }

    public void TurnOnOffSpotlight(bool on) {
        transform.Find("Spot Light").gameObject.SetActive(on);
    }
}
