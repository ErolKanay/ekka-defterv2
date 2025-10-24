namespace WebApplication1.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open System.Diagnostics

open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Authorization
open Microsoft.Extensions.Logging

open WebApplication1.Models
open WebApplication1.Data

[<Authorize>]
type HomeController (logger : ILogger<HomeController>, dbContext: EkkaDefterDbContext) =
    inherit Controller()

    member this.Index () =
        let toplamUrun = dbContext.Urunler.Count()
        let toplamGelir = dbContext.Urunler.Sum(fun u -> u.Fiyati)
        let buAyUrunler = dbContext.Urunler.Where(fun u -> u.OdemeTarihi.Month = DateTime.Now.Month).Count()
        let buAyGelir = dbContext.Urunler.Where(fun u -> u.OdemeTarihi.Month = DateTime.Now.Month).Sum(fun u -> u.Fiyati)
        
        let dashboardData: DashboardViewModel = {
            ToplamUrun = toplamUrun
            ToplamGelir = toplamGelir
            BuAyUrun = buAyUrunler
            BuAyGelir = buAyGelir
        }
        
        this.View(dashboardData) :> IActionResult

    member this.Privacy () =
        this.View()

    [<ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)>]
    member this.Error () =
        let reqId = 
            if isNull Activity.Current then
                this.HttpContext.TraceIdentifier
            else
                Activity.Current.Id

        this.View({ RequestId = reqId })
