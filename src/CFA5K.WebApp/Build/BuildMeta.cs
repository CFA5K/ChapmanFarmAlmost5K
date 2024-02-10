// Chapman Farm Almost 5K.
// Copyright (C) Eugene Bekker.

using System.Reflection;

namespace CFA5K.WebApp.Build;

public class BuildMeta
{
    private readonly Type? _type;
    private readonly Assembly? _assembly;
    private readonly Version? _version;
    private readonly BuildMetaAttribute? _meta;

    private BuildMeta(
        BuildMetaAttribute? meta = null,
        Version? version = null,
        Assembly? assembly = null,
        Type? type = null)
    {
        _meta = meta;
        _version = version;
        _assembly = assembly;
        _type = type;
    }

    public Version? Version => _version;
    public DateTime? Date => _meta?.Date;
    public string? Hash => _meta?.Hash;
    public string? Host => _meta?.Host;
    public string? User => _meta?.User;
    public string? Path => _meta?.Path;
    public string? Conf => _meta?.Conf;

    public static BuildMeta Get<T>()
    {
        var asm = typeof(Program).Assembly;
        var anm = asm.GetName();
        var ver = anm.Version;
        var atr = asm.GetCustomAttribute<BuildMetaAttribute>();

        return new(atr, ver, asm, typeof(T));
    }
}
