using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Misc.Techs
{
  public class TechLevelObject
  {
    public bool Locked;
    public bool Unlocked;
    public bool Researched;

    int oilLvlNeeded;
    int woodNeeded;
    int steelNeeded;
    int oilNeeded;
    int researchTime;

    public TechLevelObject(int OilLvlNeeded, int WoodNeeded, int SteelNeeded, int OilNeeded, int ResearchTime)
    {
      oilLvlNeeded = OilLvlNeeded;
      woodNeeded = WoodNeeded;
      steelNeeded = SteelNeeded;
      oilNeeded = OilNeeded;
      researchTime = ResearchTime;
    }

    public bool TryResearh(int OilLvl, int Wood, int Steel, int Oil)
    {
      if (Unlocked)
      {
        if (OilLvl >= oilLvlNeeded && Wood >= woodNeeded && Steel >= steelNeeded && Oil >= oilNeeded)
          Unlocked = false;
        return true;

      }
      return false;

    }


  }
}
