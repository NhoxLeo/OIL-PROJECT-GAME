using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff{
    private string buffName;
    private float magnitude, duration;
    private GameObject human;
    private bool isAffecting;

  public string Name { get { return buffName; } }

    /// <summary>
    /// A (de)buff that can affect a worker. Lasts for a period of time.
    /// </summary>
    /// <param name="buffName">The name of the (de)buff.</param>
    /// <param name="magnitude">The magnitude of the (de)buff.</param>
    /// <param name="duration">The duration of the (de)buff.</param>
    /// <param name="human">The bearer of this (de)buff.</param>
    public Buff(string buffName, float magnitude, float duration, GameObject human) {
        this.buffName = buffName;
        this.magnitude = magnitude;
        this.duration = duration;
        this.human = human;
        isAffecting = true;
        human.GetComponent<Human>().ManipulateDebuff(magnitude);
    }

    /// <summary>
    /// A (de)buff that can affect a worker. This one's permanent!
    /// </summary>
    /// <param name="buffName">The name of the (de)buff.</param>
    /// <param name="magnitude">The magnitude of the (de)buff.</param>
    /// <param name="human">The human that has this (de)buff.</param>
    public Buff(string buffName, float magnitude, GameObject human) {
        this.buffName = buffName;
        this.magnitude = magnitude;
        this.human = human;
        isAffecting = false;
        human.GetComponent<Human>().ManipulateDebuff(magnitude);
    }

    public void UpdateDuration() {
        if (isAffecting) {
            duration -= Time.deltaTime;
            if (duration <= 0) {
                isAffecting = false;
                human.GetComponent<Human>().ManipulateDebuff(-magnitude);
            }
        }                            
    }

    public bool IsTimedOut() {
        return !isAffecting;
    }
}
