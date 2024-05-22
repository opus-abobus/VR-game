namespace DataPersistence {
    public interface ISaveSystem {
        void Save<TData>(TData gameData, string fullPath);
        TData Load<TData>(string fullPath);
    }
}
