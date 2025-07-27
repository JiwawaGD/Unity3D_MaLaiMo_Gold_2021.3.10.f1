using UnityEngine;

public class ObjectController_Room : MonoBehaviour
{
    #region ============ 座標 相關 ============
    [HideInInspector]
    public Transform _playerWakeUpPos;
    #endregion

    #region ============ 物件 相關 ============
    [HideInInspector]
    public GameObject _player;

    [HideInInspector]
    public GameObject _tv;

    [HideInInspector]
    public GameObject _wallClock;

    [HideInInspector]
    public GameObject _filialPietyCurtain;

    [HideInInspector]
    public GameObject _riceAndSoup;
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

    [HideInInspector]
    public ItemController _mom;

    [HideInInspector]
    public ItemController _frontDoor;

    [HideInInspector]
    public ItemController _piano;

    [HideInInspector]
    public ItemController _grandmaDeadBody;
    #endregion
}
