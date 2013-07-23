namespace Chiffon.WebSite.Controllers

open System.Web
open System.Web.Mvc

[<HandleError>]
type MemberController() =
    inherit Controller()
    member this.Index () =
        this.View() :> ActionResult