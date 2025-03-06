using System.Collections.Generic;
using UnityEngine;
using Scripts.Boss.Combat; // adjust namespace as needed

[CreateAssetMenu(fileName = "WeaponListWrapper", menuName = "Combat/WeaponListWrapper")]
public class WeaponListWrapper : ScriptableObject
{
    public List<Weapon> weapons;
}
