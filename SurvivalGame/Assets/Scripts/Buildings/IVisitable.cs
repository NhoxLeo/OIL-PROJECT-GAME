using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For building like knick-Knack, Bar to use to handle a cutomers stay.
/// </summary>
public interface IVisitable
{
  /// <summary>
  /// Set this timer to whatever time the human should spend in the building.
  /// </summary>
  float VisitTime { get; }

  /// <summary>
  /// Do all the stuff to the human when it enters the building with this method.
  /// </summary>
  /// <param name="visitor"></param>
  void HandleVisitor(GameObject visitor);
}
