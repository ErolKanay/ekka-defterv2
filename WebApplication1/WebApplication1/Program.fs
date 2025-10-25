namespace WebApplication1

#nowarn "20"

open System
open System.Collections.Generic
open System.IO
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.HttpsPolicy
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.EntityFrameworkCore
open Microsoft.AspNetCore.Authentication.Cookies
open WebApplication1.Data

module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =
        let builder = WebApplication.CreateBuilder(args)

        builder
            .Services
            .AddControllersWithViews()
            .AddRazorRuntimeCompilation()

        // Railway için DATABASE_URL environment variable'ını kullan
        let connectionString = 
            match Environment.GetEnvironmentVariable("DATABASE_URL") with
            | null -> builder.Configuration.GetConnectionString("DefaultConnection")
            | url -> url
        
        builder.Services.AddDbContext<EkkaDefterDbContext>(fun options ->
            options.UseNpgsql(connectionString) |> ignore
        )

        // Authentication servislerini ekle
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(fun options ->
                options.LoginPath <- "/Auth/Login"
                options.LogoutPath <- "/Auth/Logout"
                options.AccessDeniedPath <- "/Auth/Login"
            ) |> ignore

        builder.Services.AddRazorPages()

        let app = builder.Build()

        // Veritabanı tablolarını oluştur
        use scope = app.Services.CreateScope()
        let dbContext = scope.ServiceProvider.GetRequiredService<EkkaDefterDbContext>()
        try
            // Önce EnsureCreated ile tabloları oluştur
            dbContext.Database.EnsureCreated() |> ignore
            printfn "Veritabanı tabloları başarıyla oluşturuldu"
            
            // Eğer tablolar boşsa, örnek veri ekle
            if not (dbContext.Urunler.Any()) then
                printfn "Veritabanı boş, örnek veri ekleniyor..."
                // Burada örnek veri ekleyebiliriz
                printfn "Veritabanı hazır"
        with
        | ex -> printfn "Veritabanı oluşturma hatası: %s" ex.Message

        if not (builder.Environment.IsDevelopment()) then
            app.UseExceptionHandler("/Home/Error")
            app.UseHsts() |> ignore 

        app.UseHttpsRedirection()

        app.UseStaticFiles()
        app.UseRouting()
        
  
        app.UseAuthentication()
        app.UseAuthorization()

        app.MapControllerRoute(name = "default", pattern = "{controller=Auth}/{action=Login}/{id?}")

        app.MapRazorPages()

        app.Run()

        0
        