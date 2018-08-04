using System;
using System.Collections;
using System.Collections.Generic;
using RLCreature.Sample.Common.UI.UIComponents;
using UnityEngine;

namespace RLCreature.Sample.Common.UI.Actions
{
    public class SystemActions : IToolBarActions
    {
        public SystemActions()
        {
        }

        public string GetCategory()
        {
            return "System";
        }

        public Dictionary<string, Action> GetActions()
        {
            return new Dictionary<string, Action>
            {
                {
                    "TimeScale x1", () => { Time.timeScale = 1; }
                },
                {
                    "TimeScale x2", () => { Time.timeScale = 2; }
                },
                {
                    "TimeScale x5", () => { Time.timeScale = 5; }
                },
                {
                    "TimeScale x10", () => { Time.timeScale = 10; }
                }
            };
        }
    }
}