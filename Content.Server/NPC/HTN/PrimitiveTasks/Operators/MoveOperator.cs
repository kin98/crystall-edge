using System.Threading;
using System.Threading.Tasks;
using Content.Server.NPC.Components;
using Content.Server.NPC.Pathfinding;
using Content.Server.NPC.Systems;
using NetCord;
using Robust.Server.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Physics.Components;
using Robust.Shared.Timing;

namespace Content.Server.NPC.HTN.PrimitiveTasks.Operators;

/// <summary>
/// Moves an NPC to the specified target key. Hands the actual steering off to NPCSystem.Steering
/// </summary>
public sealed partial class MoveOperator : HTNOperator
{
    [Dependency] private readonly IEntityManager _entManager = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    private PhysicsSystem _physics = default!;


    [DataField("durationKey")]
    public string DurationKey = "MovementDuration";
    public string DurationTimeSpanKey = "MovementTimeSpanDuration";

    [DataField("startKey")]
    public string StartTimeKey = "StartTime";

    /// <summary>
    /// How close we need to get before considering movement finished.
    /// </summary>
    [DataField("rangeKey")]
    public string RangeKey = "MovementRange";



    public override void Initialize(IEntitySystemManager sysManager)
    {
        base.Initialize(sysManager);
        _physics = sysManager.GetEntitySystem<PhysicsSystem>();
    }



    public override void Startup(NPCBlackboard blackboard)
    {
        base.Startup(blackboard);

        blackboard.SetValue(StartTimeKey, _timing.CurTime);
        var durationFloat = blackboard.GetValueOrDefault<float>(DurationKey, _entManager);
        blackboard.SetValue(DurationTimeSpanKey, TimeSpan.FromSeconds(durationFloat));
    }

    public override HTNOperatorStatus Update(NPCBlackboard blackboard, float frameTime)
    {
        if (!blackboard.TryGetValue<TimeSpan>(DurationTimeSpanKey, out var duration, _entManager)) return HTNOperatorStatus.Failed;
        var start = blackboard.GetValue<TimeSpan>(StartTimeKey);
        var time = _timing.CurTime;
        var elapsed = time - start;
        var has_elapsed = elapsed >= duration;
        var owner = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);
        _physics.ApplyLinearImpulse(owner, new System.Numerics.Vector2(0, 1000) * frameTime);
        _physics.ApplyAngularImpulse(owner, 1000f * frameTime);

        return has_elapsed ? HTNOperatorStatus.Finished : HTNOperatorStatus.Continuing;
    }

}
