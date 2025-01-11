using Save.Data;

namespace Save
{
    public interface IDataPersistence
    {
        void LoadData(GameData data);

        void SaveData(GameData data);
    }


    public interface IPermanentDataPersistence
    {
        void LoadData(PermanentGameData data);

        void SaveData(PermanentGameData data);
    }
}