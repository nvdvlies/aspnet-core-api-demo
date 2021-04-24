namespace Demo.Scaffold.Tool.Changes
{
    internal interface IChange
    {
        string Description { get; }
        void Apply();
    }
}
