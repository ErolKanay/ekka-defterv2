namespace WebApplication1.Data

open Microsoft.EntityFrameworkCore
open WebApplication1.Models

type EkkaDefterDbContext(options: DbContextOptions<EkkaDefterDbContext>) =
    inherit DbContext(options)

    member this.Urunler = this.Set<Urun>()

    override this.OnModelCreating(modelBuilder: ModelBuilder) =
        base.OnModelCreating(modelBuilder)
        
        modelBuilder.Entity<Urun>(fun entity ->
            entity.HasKey([|"Id"|]) |> ignore
            entity.Property(fun u -> u.UrunAdi).IsRequired().HasMaxLength(100) |> ignore
            entity.Property(fun u -> u.Fiyati).HasColumnType("decimal(18,2)") |> ignore
            entity.Property(fun u -> u.Bedeni).IsRequired().HasMaxLength(20) |> ignore
            entity.Property(fun u -> u.OdemeBilgisi).IsRequired().HasMaxLength(100) |> ignore
            entity.Property(fun u -> u.OdemeTarihi).IsRequired() |> ignore
            entity.Property(fun u -> u.AlanKisiAdi).IsRequired().HasMaxLength(100) |> ignore
        ) |> ignore
