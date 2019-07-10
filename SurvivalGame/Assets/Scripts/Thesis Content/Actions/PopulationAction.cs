using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PopulationAction
{        
   
    
    /// <summary>
    /// check prerequirements against human status return evaluation results
    /// </summary>
    /// <param name="human"></param>
    /// <returns></returns>
    List<string> PreRequisites();

    /// <summary>
    /// Impact on world state
    /// </summary>
    /// <returns></returns>
    List<string> OutCome();
   
}
