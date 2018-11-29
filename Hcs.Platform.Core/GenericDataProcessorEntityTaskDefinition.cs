using System;

namespace Hcs.Platform
{
    internal class GenericDataProcessorEntityTaskDefinition
    {
        public GenericDataProcessorEntityTaskDefinition(Type taskType, Type entityTypeConstraint)
        {
            TaskType = taskType;
            EntityTypeConstraint = entityTypeConstraint;
        }

        public Type TaskType { get; }
        public Type EntityTypeConstraint { get; }
    }
}