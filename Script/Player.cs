using Godot;
using System;

public partial class Player : CharacterBody3D
{
	const float speed = 10f;
	const float gravity = 30f;
	const float jumpForce = 10f;
	const float acceleration = 0.5f;
	const float sensitivity = 0.01f;

	public Node3D head;
	public Camera3D cam;
	private Vector3 velocity = Vector3.Zero;

	public override void _Ready() {
		head = GetNode<Node3D>("Head");
		cam = GetNode<Camera3D>("Head/Camera3D");
		
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _Input(InputEvent @event) {
		if(@event is InputEventMouseMotion) {
			InputEventMouseMotion mouseMotion = @event as InputEventMouseMotion;
			head.RotateY(-mouseMotion.Relative.X * sensitivity);
			cam.RotateX(-mouseMotion.Relative.Y * sensitivity);

			Vector3 cameraRot = cam.Rotation;
			cameraRot.X = Mathf.Clamp(cameraRot.X, Mathf.DegToRad(-80f), Mathf.DegToRad(80f));
			cam.Rotation = cameraRot;
		}
	}
	public override void _PhysicsProcess(double delta) {
		Vector2 inputDir = Input.GetVector("left", "right", "up", "down");
		Vector3 Direction = (head.GlobalTransform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

		if(Direction != Vector3.Zero) {
			velocity.X = Direction.X * speed;
			velocity.Z = Direction.Z * speed;
		}
		else {
			velocity.X = Mathf.MoveToward(Velocity.X, 0, acceleration);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, acceleration);
		}

		if(!IsOnFloor()) {
			velocity.Y -= gravity * (float)delta;
		}

		if(IsOnFloor() && Input.IsActionJustPressed("Jump")) {
			velocity.Y = jumpForce;
		}

		Velocity = velocity;
		MoveAndSlide();
	}
	
}
