namespace FAKECore
open Duality
open Duality.Components.Renderers
open Duality.Drawing

[<RequiredComponent(typedefof<SpriteRenderer>)>]
[<AllowNullLiteral>]
type BrickCmp() = class
    inherit Component()

    [<DontSerialize>]
    let mutable hitPoints = 0
    member this.HitPoints
        with get() = hitPoints
        and set(value) =
            hitPoints <- value
            this.GameObj.GetComponent<SpriteRenderer>().ColorTint <- ColorRgba(float32 hitPoints / 5.0F, 0.0F, 0.0F)

    member this.Hit() =
        match hitPoints with
        | x when x > 1 ->
            this.HitPoints <- this.HitPoints - 1
            false
        | _ ->
            this.GameObj.DisposeLater()
            true

end

