namespace FAKECore
open System
open Duality
open Duality.Components
open Duality.Components.Physics

[<RequiredComponent(typedefof<Transform>)>]
[<AllowNullLiteral>]
type InputControllerCmp() = class
    inherit Component()    
    member val PaddleSpeed = 100.F with get, set
    member val BallInitialSpeed = 100.0F with get, set
    member val Paddle : GameObject = null with get, set
    member val Ball : GameObject = null with get, set
    member val GameManager : GameManager = null with get, set

    interface ICmpUpdatable with
        member this.OnUpdate() =
            let mutable input = 0.0F

            if DualityApp.Keyboard.[Input.Key.Left] then input <- -1.0F
            else if DualityApp.Keyboard.[Input.Key.Right] then input <- 1.0F

            this.Paddle.Transform.MoveBy(Vector2(this.PaddleSpeed * input * Time.TimeMult / Time.FPSMult, 0.0F))

            if DualityApp.Keyboard.KeyHit(Input.Key.Space) && this.Ball.Transform.Vel.LengthSquared = 0.0F then
                this.Ball.GetComponent<RigidBody>().ApplyLocalImpulse(Vector2.UnitY * this.BallInitialSpeed)
            if DualityApp.Keyboard.[Input.Key.Enter] && this.GameObj.ParentScene.FindGameObject("Brick") |> isNull then
                this.GameManager.PrepareLevel()
                this.GameManager.ResetBall this.Ball true

    interface ICmpInitializable with
        member this.OnInit context =
            if context = Component.InitContext.Activate && DualityApp.ExecContext = DualityApp.ExecutionContext.Game then
                this.Ball <- this.GameObj.ParentScene.FindGameObject<BallCmp>()
                this.Paddle <- this.GameObj.ParentScene.FindGameObject "Paddle"
                this.GameManager <- this.GameObj.ParentScene.FindComponent<GameManager>()

        member this.OnShutdown context = ()
end