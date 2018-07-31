using System;
using System.Collections.Generic;

namespace RLCreature.Sample.Common.UI.UIComponents
{
    public interface IToolBarActions
    {
        string GetCategory();
        Dictionary<string, Action> GetActions();
    }
}