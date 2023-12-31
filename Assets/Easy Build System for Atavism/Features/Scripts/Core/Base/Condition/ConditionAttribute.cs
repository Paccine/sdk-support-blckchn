﻿using EasyBuildSystem.Features.Scripts.Core.Base.Condition.Enums;
using System;

namespace EasyBuildSystem.Features.Scripts.Core.Base.Condition
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConditionAttribute : Attribute
    {
        #region Fields

        public readonly string Name;
        public readonly string Description;
        public ConditionTarget Target;
        public Type Behaviour;

        #endregion Fields

        #region Methods

        public ConditionAttribute(string name, string description, ConditionTarget target)
        {
            Name = name;
            Description = description;
            Target = target;
        }

        #endregion Methods
    }
}