namespace Demo.Scaffold.Tool.Changes
{
    internal interface IChange
    {
        ModificationTypes ModificationType { get; }
        string DirectoryAndFileName { get; }

        void Apply();
    }
}
