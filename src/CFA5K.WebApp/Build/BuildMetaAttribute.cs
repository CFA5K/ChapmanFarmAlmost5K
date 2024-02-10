// Chapman Farm Almost 5K.
// Copyright (C) Eugene Bekker.

namespace CFA5K.WebApp.Build;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
public class BuildMetaAttribute : Attribute
{
    public DateTime? Date { get; set; }

    public string? Hash { get; set; }

    public string? Host { get; set; }

    public string? User { get; set; }

    public string? Path { get; set; }

    public string? Conf { get; set; }
}
