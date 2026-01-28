using Content.Server._CE.GameTicking.Components;
using Content.Server._CE.ZLevels.Core;
using Content.Server.GameTicking.Rules;
using Content.Server.Ghost.Roles;
using Content.Server.Ghost.Roles.Components;
using Content.Server.Mapping;
using Content.Shared._CE.DayCycle;
using Content.Shared.GameTicking;
using Content.Shared.GameTicking.Components;
using Robust.Shared.Map;
using Robust.Shared.Random;

namespace Content.Server._CE.GameTicking;

public sealed class CELurkerRuleSystem : GameRuleSystem<CELurkerRuleComponent>
{
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly CEZLevelsSystem _zlevel = default!;
    [Dependency] private readonly GhostRoleSystem _ghostRole = default!;


    public override void Initialize()
    {
        base.Initialize();

    }
    protected override void Started(EntityUid ruleUid,
        CELurkerRuleComponent component,
        GameRuleComponent gameRule,
        GameRuleStartedEvent args)
    {
        base.Started(ruleUid, component, gameRule, args);
        var ent = Spawn(component.LurkerProto, new MapCoordinates(_random.NextVector2(), new MapId(1)));
        var ghostComp = new GhostRoleComponent();
        AddComp(ent, ghostComp);
        _ghostRole.RegisterGhostRole((ent, ghostComp));

    }
}
