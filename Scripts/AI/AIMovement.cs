using Godot;
using System;
using System.Collections.Generic;
using EventCallback;
public class AIMovement : Node
{

    //The movemnt speed of the tank
    [Export] int speed = 80;
    //The collection of vector two points making up the path to the target
    private List<Vector2> path;
    //Reference to the navigation node thats a child of the main node
    Navigation2D nav2d;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //Register the listener for the event message of the SetAIMoveEvent
        SetAIMoveEvent.RegisterListener(OnSetAIMoveEvent);
        //Create a new vector two list path at the creation of the ship
        path = new List<Vector2>();
        //Set the nav2d to the node object in game
        nav2d = GetNode<Navigation2D>("../Navigation2D");
    }

    private void OnSetAIMoveEvent(SetAIMoveEvent saime)
    {
        //Sets the  process function as active when the move ai function is called from the AI state manager
        SetProcess(saime.active);
    }

    public override void _Process(float delta)
    {
        //How far the next movedistance is of the ship determined by the speed of the ship multiplied by the delta time interval
        float walkDistance = delta * speed;
        //Call the move along path with the walkDistance as the injected value
        MoveAlongPath(walkDistance);
    }

    private void MoveAlongPath(float distance)
    {
        Vector2 lastPosition = ((Node2D)GetParent()).Position;

        while (path.Count != 0)
        {
            float distanceBetweenPoints = lastPosition.DistanceTo(path[0]);

            if (distance < distanceBetweenPoints)
            {
                ((Node2D)GetParent()).Position = lastPosition.LinearInterpolate(path[0], distance / distanceBetweenPoints);
                return;
            }

            distance -= distanceBetweenPoints;
            lastPosition = path[0];
            path.Remove(lastPosition);
        }

        ((Node2D)GetParent()).Position = lastPosition;
        SetProcess(false);
    }

    private void UpdateNavigationPath(Vector2 startPosition, Vector2 endPosition)
    {
        GD.Print("AIMovement - _Input: UpdateNavigationPath called");
        //Get the vector2 points for the path
        path.AddRange(nav2d.GetSimplePath(startPosition, endPosition, true));
        //Start the process function to move towards the target position
        SetProcess(true);
    }

    public override void _Input(InputEvent @event)
    {
        // Mouse in viewport coordinates.
        if (@event is InputEventMouseButton eventMouseButton)
        {

            GD.Print("AIMovement - _Input: Mouse be clicked");
            UpdateNavigationPath(((Node2D)GetParent()).Position, eventMouseButton.Position);
        }
    }

    /*
    //Signal to send to the health script attached to the player
    [Signal]
    public delegate void Hit(String targetName, String attackerName);
    //The current state of the AI used for behaviour, for now very low level
    AIStates myState;
    Sprite gunSprite, muzzleFlash;
    //The timer for the muzzle flash and the attack interval
    Timer attackTimer, flashTimer;
    //The name of the target
    Node2D target;

    bool canAttack = true;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        attackTimer = new Timer();
        //Set up the timer for the muzzle flash
        attackTimer.WaitTime = 2f;
        attackTimer.OneShot = true;
        attackTimer.Name = "attacktTimer";
        //Create the new timer
        AddChild(attackTimer, true);

        GetNode<Timer>(attackTimer.Name).Connect("timeout", this, nameof(attackReset));

        flashTimer = new Timer();
        //Set up the timer for the muzzle flash
        flashTimer.WaitTime = .05f;
        flashTimer.OneShot = true;
        flashTimer.Name = "flashTimer";
        AddChild(flashTimer, true);

        //Grab the timer node for the muzzle flash
        GetNode<Timer>(flashTimer.Name).Connect("timeout", this, nameof(HideFlash));
        //Get this nodes health scripgt and init the conevtion
        //GetNode<Health>("Health").Connect("BeingAttacked", this, nameof(BeingAttacked));

        UnitHitEvent.RegisterListener(BeingAttacked);

        gunSprite = (Sprite)FindNode("Gun");

        muzzleFlash = (Sprite)FindNode("MuzzleFlash");

        myState = AIStates.MoveTo;
        //Set the defualt target to the crystal
        target = GetNode<Node2D>("../../Crystal");

        //Set the target to the dome by defualt so it goes and attacks it by defualt
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        //targetPos = crystalKB2D.Position;
        //Cycle through and run the ai behaviour that is set to the ais currnet state
        switch (myState)
        {
            case AIStates.Attack:
                Attack();
                break;
            case AIStates.MoveTo:
                MoveTo();
                break;
            case AIStates.BeingAttacked:
                break;
            case AIStates.LookForTarget:
                break;
        }

        //Godot.Collections.Array incommingSignals = GetIncomingConnections();
        //Godot.Collections.Dictionary temDict = (Godot.Collections.Dictionary)incommingSignals[0];
        //GD.Print(temDict["signal_name"]);

        //If im being attacked then switch target to the attacker


    }

    private void SetTarget(Vector2 target)
    {

    }

    private void WasHit(int id)
    {
        GD.Print("AI checking if I was hit");
    }

    private void Wander()
    {

    }

    private Vector2 ScanForTarget()
    {
        return Vector2.Zero;
    }

    private void MoveTo()
    {
        //If the target is in position we switch tto the attack state
        if (Position.DistanceTo(target.Position) < 300)
        {
            //Change the state to the attack state
            myState = AIStates.Attack;
        }
        else
        {
            //Get the direction to move in
            Vector2 dir = target.Position - Position;
            //Normalizing direction for movement
            dir = dir.Normalized();
            //Move and slide has the phsics delta already worked in and using it helps the objects slide past each other
            MoveAndSlide(dir * speed);
            //Rotate to target
            Rotation = Mathf.LerpAngle(Rotation, dir.Angle(), 0.2f);
        }
    }

    private void Attack()
    {
        //Check if the target is still in the desired distane
        if (Position.DistanceTo(target.Position) > 300)
        {
            //Change the state to the attack state
            myState = AIStates.MoveTo;
        }
        else
        {
            //Get the direction to move in
            Vector2 dir = target.Position - Position;
            //Normalizing direction for movement
            dir = dir.Normalized();
            //gunSprite.Rotation = Mathf.LerpAngle(gunSprite.Rotation, dir.Angle(), 0.2f);
            gunSprite.LookAt(target.Position);
        }

        //If the attack timer has not run out yet we just return out of the method
        if (!canAttack) return;
        //Reset the can attack bool
        canAttack = false;
        //Start the attack timer
        attackTimer.Start();

        //so I am brute forcing the heck out of this mthod sorry 
        ShowFlash();

        Physics2DDirectSpaceState worldState = GetWorld2d().DirectSpaceState;
        //Get the raycast hits and store them in a dictionary
        Godot.Collections.Dictionary hits = worldState.IntersectRay(GlobalPosition, target.GlobalPosition, new Godot.Collections.Array { this }, this.CollisionMask);
        //Check if there was a hit
        if (hits.Count > 0)
        {
            if (hits.Contains("collider"))
            {
                UnitHitEvent uhei = new UnitHitEvent();
                uhei.attacker = (Node2D)GetParent();
                uhei.target = (Node2D)hits["collider"];
                uhei.damage = 5;
                uhei.Description = uhei.attacker.Name + " attacked " + uhei.target.Name;
                uhei.FireEvent();
            }
        }

    }
    private void BeingAttacked(UnitHitEvent unitHit)
    {
        if (this.Name == unitHit.target.Name)
        {
            GD.Print("AttackerName = " + unitHit.attacker.Name);
            target = (Node2D)unitHit.attacker;
        }

    }

    private void ShowFlash()
    {
        flashTimer.Start();
        muzzleFlash.Visible = true;
    }

    private void HideFlash()
    {
        muzzleFlash.Visible = false;
    }

    private void attackReset()
    {
        canAttack = true;
    }

    public override void _ExitTree()
    {
        UnitHitEvent.UnregisterListener(BeingAttacked);
    }
    */
}
