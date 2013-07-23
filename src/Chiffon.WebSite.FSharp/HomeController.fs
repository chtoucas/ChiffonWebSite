namespace Chiffon.WebSite.Controllers

open System.Web
open System.Web.Mvc

[<HandleError>]
type HomeController() =
    inherit Controller()
    member this.Index () =
        this.View() :> ActionResult
    member this.Contact() =
        this.View() :> ActionResult
    member this.About() =
        this.View() :> ActionResult
