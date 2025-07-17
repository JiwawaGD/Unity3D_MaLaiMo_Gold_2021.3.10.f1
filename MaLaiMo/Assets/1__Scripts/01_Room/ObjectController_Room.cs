using UnityEngine;

public class ObjectController_Room : MonoBehaviour
{
    #region ============ 物件 相關 ============
    [HideInInspector]
    public GameObject _player;

    [HideInInspector]
    public GameObject _wallClock;
    #endregion

    #region ============ Item 相關 ============
    [HideInInspector]
    public ItemController _grandmaRoomDoor;

    [HideInInspector]
    public ItemController _lotusPaper;

    [HideInInspector]
    public ItemController _foldedLotusPaper;

    [HideInInspector]
    public ItemController _offeringPlace;

    [HideInInspector]
    public ItemController _grandmaRoomCloset;

    [HideInInspector]
    public ItemController _clothesInCloset;

    [HideInInspector]
    public ItemController _graffitiInCloset;
    #endregion
}
