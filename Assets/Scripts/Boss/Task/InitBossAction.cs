using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Core.UI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "InitBoss", story: "Initialize Boss", category: "Action", id: "4ce1966dc0e233a4e33ae06d78099448")]
public partial class InitBossAction : EnemyAction
{
    [SerializeReference] public BlackboardVariable<string> bossName;

    protected override Status OnUpdate()
    {
        GuiManager guiManager = GuiManager.Instance;
        guiManager.ShowBossName(bossName.Value);
        guiManager.FadeBossInitVignette(2f);
        return Status.Success;
    }

}

