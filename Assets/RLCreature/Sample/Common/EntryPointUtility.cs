using System.Collections;
using RLCreature.BodyGenerator;
using RLCreature.BodyGenerator.Manipulatables;
using RLCreature.Sample.Common.UI.UIComponents;
using UnityEngine;

namespace RLCreature.Sample.Common
{
    public static class EntryPointUtility
    {
        public static IEnumerator Rename(CreatureInfoCell cell, Agent agent, Mouth mouth)
        {
            while (true)
            {
                yield return new WaitForSeconds(10);
                cell.DisplayName = agent.name + ":" + mouth.EatenCount;
            }
        }
        
        public static IEnumerator DeleteTimer(GameObject targetObject, int seconds)
        {
            yield return new WaitForSeconds(seconds);
            GameObject.Destroy(targetObject);
        }

    }
}