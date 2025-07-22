using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UserData
{
    public static string CardsData_Constant = "CardsData";
    public static string ChancesData_Constant = "ChancesData";

    public static CardLocalData cardLocalData;

    static UserData()
    {
        cardLocalData = new CardLocalData();
    }

    public static void SaveCardInfos(CardInfo[,] cardInfos, bool unmatchedCardLeft)
    {
        CardUtility.ConvertToJagged(cardInfos, out cardLocalData.cardInfos);

        Debug.Log($"SceneReload: SaveCardInfo: {Level.SceneReload}");
        if (!Level.SceneReload && unmatchedCardLeft)
            SaveSystem.SaveFile(cardLocalData.cardInfos, CardsData_Constant);
        else
            SaveSystem.DeleteSavedFile(CardsData_Constant);
    }

    public static void SaveChancesInfo(ChancesInfo chancesInfo)
    {
        Debug.Log($"SceneReload: SaveChancesInfo: {Level.SceneReload}");
        if (!Level.SceneReload)
        {
            cardLocalData.chancesInfo = chancesInfo;
            SaveSystem.SaveFile(cardLocalData.chancesInfo, ChancesData_Constant);
        }
        else
            SaveSystem.DeleteSavedFile(ChancesData_Constant);
    }
}
