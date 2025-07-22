using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float zOffset = 1f;

    public static bool SavedCards = false;
    public static bool SceneReload = false;
    public static bool GameOver = false;

    private CardLocalData cardLocalData = null;

    public SFXManagger SFXManager;

    private void Awake()
    {
        SavedCards = false;

        CardInfo[,] cardInfos = null;
        if (SaveSystem.HasKey(UserData.CardsData_Constant))
        {
            cardLocalData = new CardLocalData();
            cardLocalData.cardInfos = SaveSystem.LoadJson<CardInfo[][]>(UserData.CardsData_Constant);

            if (cardLocalData.cardInfos != null)
                cardInfos = CardUtility.ConvertTo2D(cardLocalData.cardInfos);

            if (cardInfos != null &&
                cardInfos.GetLength(0) * cardInfos.GetLength(1) == gridManager.XDim * gridManager.YDim)
            {
                SavedCards = true;
            }
            else
            {
                SaveSystem.DeleteSavedFile(UserData.CardsData_Constant);
            }
        }

        gridManager.Init(cardLocalData != null && cardInfos != null ? cardInfos : default);
        SetCameraPosition();
    }

    private void SetCameraPosition()
    {
        float value = (gridManager.YDim * gridManager.CardScaleY) + (gridManager.YDim * gridManager.YSpacing);
        mainCamera.transform.position = new Vector3(0, value/2, - (value + zOffset));
    }

    public void Restart()
    {
        SceneReload = true;

        Invoke(nameof(LoadActiveScene), 1f);
    }

    private void LoadActiveScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
