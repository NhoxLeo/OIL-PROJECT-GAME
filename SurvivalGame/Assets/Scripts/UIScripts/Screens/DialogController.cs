using Assets.Scripts.UIScripts.Screens;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogController : MonoBehaviour {

  [SerializeField]
  private UIDialog techTree;
  [SerializeField]
  private UIDialog emergencies;
  [SerializeField]
  private UIDialog populationInformation;
  [SerializeField]
  private UIDialog pauseDialog;
  [SerializeField]
  private UIDialog toolTipDialog;

  DialogType _currentDialog;
  Dictionary<DialogType, UIDialog> _dialogs;



  void Start () {
    _dialogs = new Dictionary<DialogType, UIDialog>()
    {
      {DialogType.EmergencyDialog, emergencies },
      {DialogType.TechTreeDialog, techTree },
      {DialogType.InformationDialog, populationInformation },
      {DialogType.ToolTipDialog, toolTipDialog },
      {DialogType.TechTreeDialog, pauseDialog },
    };
	}
	

  public void ChangeDialog()
  {

  }
}
