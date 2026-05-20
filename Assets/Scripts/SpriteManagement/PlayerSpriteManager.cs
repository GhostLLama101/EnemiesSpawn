using UnityEngine;
using UnityEngine.UI;

public class PlayerSpriteManager : IconManager
{
  public Image image;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.playerSpriteManager = this;
    }

}
