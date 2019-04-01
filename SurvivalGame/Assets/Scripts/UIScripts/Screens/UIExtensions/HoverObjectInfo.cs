using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UIScripts.Screens.UIExtensions
{
  public class HoverObjectInfo : MonoBehaviour
  {
    [SerializeField]
    Text infoText;

    [SerializeField]
    GameObject containerPanel;


    Transform target;

    float timer = 0;
    float openInterval = 0.75f;

    static HoverObjectInfo Instance;
    public static HoverObjectInfo Singleton
    {
      get
      {
        return Instance;
      }
    }

    private void Start()
    {
      if (Instance != null)
      {
        Destroy(gameObject);
        return;
      }
      Instance = this;
    }

    public void ShowInfo(Transform parentTransform, string info)
    {
      timer = 0;
      target = parentTransform;
      infoText.text = info;
      show();
    }

    private void Update()
    {
      if (target)
      {
        var newPos = Camera.main.WorldToScreenPoint(target.position + Vector3.up);
        transform.position = Vector3.Lerp(transform.position, newPos, 0.90f);
        timer += Time.deltaTime;
        if (timer > openInterval)
        {
          target = null;
          Close();
        }

      }
    }

    private void show()
    {
      containerPanel.SetActive(true);
    }

    private void Close()
    {
      containerPanel.SetActive(false);
    }
  }
}
