namespace FAKECore
open Duality
open Duality.Components.Physics
open Duality.Resources

[<AllowNullLiteral>]
type GameManager() = class
    inherit Component()

    [<DontSerialize>]
    let mutable score = 0
    [<DontSerialize>]
    let mutable lives = 3

    member val BrickPrefab : ContentRef<Prefab> = new ContentRef<Prefab>() with get,set

    member this.PrepareLevel() =
        let createBrick pos hitPoints =
            let brick = this.BrickPrefab.Res.Instantiate()
            brick.Name <- "Brick"
            brick.BreakPrefabLink()
            brick.GetComponent<BrickCmp>().HitPoints <- hitPoints
            brick.Transform.Pos <- pos
            brick.Parent <- this.GameObj

        let descartesProduct xs ys =
            xs |> List.collect(fun x -> ys |> List.map(fun y -> x,y))

        descartesProduct [0..4] [0..7] |> List.iter (fun t ->
            match t with
            | row, col -> createBrick (Vector3(float32 col * 40.0F - 140.0F, float32 row * 20.0F - 200.0F, 0.0F)) (row + 1)
        )

    member this.BrickCollision (inBall : GameObject) (inBrick : BrickCmp) =
        let destroyed = 
            match inBrick.Hit() with
            | true -> 5
            | false -> 1

        score <- score + destroyed

        match this.GameObj.ParentScene.FindGameObject("Brick") with
        | null -> inBall.GetComponent<RigidBody>().LinearVelocity <- Vector2.Zero
        | _ -> ()

    member this.ResetBall (inBall : GameObject) (inResetGame : bool) =
        inBall.Transform.Pos <- Vector3.Zero
        inBall.GetComponent<RigidBody>().LinearVelocity <- Vector2.Zero

        if inResetGame then
            lives <- 3
            score <- 0
        else
            lives <- lives - 1
end

