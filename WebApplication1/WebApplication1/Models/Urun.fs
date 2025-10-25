namespace WebApplication1.Models

open System.ComponentModel.DataAnnotations

[<CLIMutable>]
type Urun = {
    Id: int
    [<Required>]
    [<StringLength(100)>]
    UrunAdi: string
    [<Required>]
    [<Range(0.01, 9999.99)>]
    Fiyati: decimal
    [<Required>]
    [<StringLength(20)>]
    Bedeni: string
    [<Required>]
    [<StringLength(100)>]
    OdemeBilgisi: string
    [<Required>]
    OdemeTarihi: System.DateTime
    [<Required>]
    [<StringLength(100)>]
    AlanKisiAdi: string
}
