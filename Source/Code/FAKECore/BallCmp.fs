namespace FAKECore
open Duality
open Duality.Components.Renderers
open Duality.Components.Physics

[<RequiredComponent(typedefof<SpriteRenderer>)>]
[<RequiredComponent(typedefof<RigidBody>)>]
[<AllowNullLiteral>]
type BallCmp() = class
    inherit Component()

    member val PaddleImpulse = 1.01F with get, set
    member this.GameManager = this.GameObj.ParentScene.FindComponent<GameManager>()

    interface ICmpCollisionListener with
        member this.OnCollisionBegin(sender, args) = ()
        member this.OnCollisionEnd(sender, args) = ()
        member this.OnCollisionSolve(sender, args) =
            match args.CollideWith.Name with
            | "Paddle" ->
                let rb = this.GameObj.GetComponent<RigidBody>()
                let newVel = -Vector2.UnitY * rb.LinearVelocity.Length * this.PaddleImpulse
                let deltaX = args.CollisionData.Pos.X - args.CollideWith.Transform.Pos.X
                let angle = MathF.RadAngle45 / args.CollideWith.GetComponent<SpriteRenderer>().Rect.W / 2.0F * deltaX
                let xRef = ref newVel.X
                let yRef = ref newVel.Y
                MathF.TransformCoord(xRef, yRef, angle)
                rb.LinearVelocity <- Vector2(!xRef,!yRef)
            | "Brick" ->
                this.GameManager.BrickCollision this.GameObj (args.CollideWith.GetComponent<BrickCmp>())
            | _ -> ()

    interface ICmpUpdatable with
        member this.OnUpdate() =
            if this.GameObj.Transform.Pos.Y > 320.0F then
                this.GameManager.ResetBall this.GameObj false
end
