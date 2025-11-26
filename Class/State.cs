using Godot;
using System;

public partial class State : Node
{
    [Export] public StateMachine_Move FSM { get; set; }

    public virtual void Enter() { }
    public virtual void Exit() { }

    public virtual void PhysicsProcess(double delta) { }

    // 매 물리 프레임마다 호출됨.
    // 물리 검사, 버퍼 검사, 코요테 등을 판단
    // 입력중인지를 가끔 검사하긴 함
    public virtual void HandleTransState (double delta) { }

    // 입력 감지는 여기서 함.
    public virtual void HandleInputEvent(InputEvent @event) { }
}
