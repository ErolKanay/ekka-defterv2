namespace WebApplication1.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Authorization
open Microsoft.EntityFrameworkCore
open WebApplication1.Data
open WebApplication1.Models
open System.Linq
open System

[<Authorize>]
type UrunController(dbContext: EkkaDefterDbContext) =
    inherit Controller()

    [<HttpGet>]
    member this.Index() =
        let urunler = dbContext.Urunler.ToList()
        this.View(urunler) :> IActionResult

    [<HttpGet("ekle")>]
    member this.Ekle() =
        this.View() :> IActionResult

    [<HttpPost("ekle")>]
    member this.Ekle(urun: Urun) =
        if this.ModelState.IsValid then
            // Id'yi 0 yaparak auto-increment'i tetikle
            let yeniUrun = { urun with Id = 0 }
            dbContext.Urunler.Add(yeniUrun) |> ignore
            dbContext.SaveChanges() |> ignore
            this.RedirectToAction("Index") :> IActionResult
        else
            this.View(urun) :> IActionResult

    [<HttpGet("detay/{id}")>]
    member this.Detay(id: int) =
        let urun = dbContext.Urunler |> Seq.tryFind (fun u -> u.Id = id)
        match urun with
        | Some u -> this.View(u) :> IActionResult
        | None -> this.NotFound() :> IActionResult


    [<HttpPost("sil/{id}")>]
    member this.Sil(id: int) =
        let urun = dbContext.Urunler |> Seq.tryFind (fun u -> u.Id = id)
        match urun with
        | Some u ->
            dbContext.Urunler.Remove(u) |> ignore
            dbContext.SaveChanges() |> ignore
            this.RedirectToAction("Index") :> IActionResult
        | None -> this.NotFound() :> IActionResult
