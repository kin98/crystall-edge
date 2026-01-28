using Content.Shared._CE.Skill.Prototypes;
using Robust.Shared.Prototypes;

namespace Content.Server._CE.GameTicking.Components;

/// <summary>
/// Stores data for <see cref="CELurkerRuleSystem"/>.
/// </summary>
[RegisterComponent, Access(typeof(CELurkerRuleSystem))]
public sealed partial class CELurkerRuleComponent : Component
{
    [DataField]
    public ProtoId<CESkillTreePrototype> SkillTree = "Lurker";

    [DataField]
    public EntProtoId LurkerProto = "CEMobLurker";
}
