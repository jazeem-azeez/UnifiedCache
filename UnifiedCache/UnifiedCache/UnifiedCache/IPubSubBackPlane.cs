namespace UnifiedCache.Lib.UnifiedCache
{
    public interface IPubSubBackPlane
    {
        string ChannelName { get; }

        void Publish(string key);

        void StartSubscribe();

        void RunAction<T>(T val);
    }
}