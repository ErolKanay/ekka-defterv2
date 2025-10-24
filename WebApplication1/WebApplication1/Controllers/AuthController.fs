namespace WebApplication1.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Authentication
open Microsoft.AspNetCore.Authentication.Cookies
open System.Security.Claims
open System.Threading.Tasks

type AuthController() =
    inherit Controller()

    [<HttpGet>]
    member this.Login() =
        this.View()

    [<HttpPost>]
    member this.Login(username: string, password: string) =
        // Sabit kodlanmış kullanıcı bilgileri
        let validUsers = [
            ("Erol", "Ekka1")
            ("Egemen", "Ekka2")
        ]

        let isValidUser = validUsers |> List.exists (fun (user, pass) -> user = username && pass = password)

        if isValidUser then
            let claims = [
                Claim(ClaimTypes.Name, username)
                Claim(ClaimTypes.NameIdentifier, username)
            ]
            
            let identity = ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)
            let principal = ClaimsPrincipal(identity)
            
            this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal) |> ignore
            
            this.RedirectToAction("Index", "Urun") :> IActionResult
        else
            this.ModelState.AddModelError("", "Geçersiz kullanıcı adı veya şifre")
            this.View() :> IActionResult

    [<HttpPost>]
    member this.Logout() =
        this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme) |> ignore
        this.RedirectToAction("Login") :> IActionResult
