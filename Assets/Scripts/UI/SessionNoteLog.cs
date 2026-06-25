using System.Collections.Generic;

public static class SessionNoteLog
{
    private static readonly string[] DefaultCatalogIds =
    {
        "LOG KSDFVI1",
        "LOG 83749A9",
        "LOG ASIXVNC 21",
        "LOG 12112091",
        "LOG 61244A2A",
        "LOG 9128CIAM",
        "MANUAL 101",
        "LOG SSUVBNSAS12",
        "LOG HVDUVHN1",
        "LOG 7812394327",
        "LOG KSNAI1",
        "LOG 781239"
    };

    public readonly struct Entry
    {
        public Entry(string id, string title, string body, bool isUnlocked)
        {
            Id = id;
            Title = title;
            Body = body;
            IsUnlocked = isUnlocked;
        }

        public string Id { get; }
        public string Title { get; }
        public string Body { get; }
        public bool IsUnlocked { get; }
    }

    private static readonly List<Entry> EntriesInternal = new List<Entry>();
    private static readonly Dictionary<string, int> EntryIndexesById = new Dictionary<string, int>();

    static SessionNoteLog()
    {
        foreach (var id in DefaultCatalogIds)
        {
            RegisterCatalogEntry(id);
        }
    }

    public static IReadOnlyList<Entry> Entries => EntriesInternal;
    public static int Count => EntriesInternal.Count;

    public static void Record(string id, string title, string body)
    {
        if (string.IsNullOrWhiteSpace(id))
            return;

        var normalizedId = id.Trim();

        var normalizedTitle = string.IsNullOrWhiteSpace(title) ? "Untitled Log" : title.Trim();
        var normalizedBody = body ?? string.Empty;
        var entry = new Entry(normalizedId, normalizedTitle, normalizedBody, true);

        if (EntryIndexesById.TryGetValue(normalizedId, out var index))
        {
            EntriesInternal[index] = entry;
            return;
        }

        EntryIndexesById[entry.Id] = EntriesInternal.Count;
        EntriesInternal.Add(entry);
    }

    private static void RegisterCatalogEntry(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return;

        var normalizedId = id.Trim();
        if (EntryIndexesById.ContainsKey(normalizedId))
            return;

        EntryIndexesById[normalizedId] = EntriesInternal.Count;
        EntriesInternal.Add(new Entry(normalizedId, normalizedId, string.Empty, false));
    }

    public static void Clear()
    {
        EntriesInternal.Clear();
        EntryIndexesById.Clear();

        foreach (var id in DefaultCatalogIds)
        {
            RegisterCatalogEntry(id);
        }
    }
}