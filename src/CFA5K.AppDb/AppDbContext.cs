// Chapman Farm Almost 5K.
// Copyright (C) Eugene Bekker.


using CFA5K.AppDb.Entities;

namespace CFA5K.AppDb;

public class AppDbContext : DbContext
{
    public const string DefaultConnectionStringName = nameof(CFA5K);

    private readonly ILogger<AppDbContext> _logger;
    private readonly IServiceProvider _services;

    public AppDbContext(ILogger<AppDbContext> logger,
        IServiceProvider services,
        DbContextOptions options)
        : base(options)
    {
        _logger = logger;
        _services = services;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly,
            // Optional filter
            predicate: type => true);
    }

    public virtual DbSet<Occasion> Occasions { get; set; } = default!;
    public virtual DbSet<PlacementToken> PlacementTokens { get; set; } = default!;
}
