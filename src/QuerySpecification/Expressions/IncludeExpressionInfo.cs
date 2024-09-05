﻿using System.Linq.Expressions;

namespace Pozitron.QuerySpecification;

public class IncludeExpressionInfo
{
    public LambdaExpression LambdaExpression { get; }
    public Type EntityType { get; }
    public Type PropertyType { get; }
    public Type? PreviousPropertyType { get; }
    public IncludeTypeEnum Type { get; }

    private IncludeExpressionInfo(
        LambdaExpression expression,
        Type entityType,
        Type propertyType,
        Type? previousPropertyType,
        IncludeTypeEnum includeType)

    {
        ArgumentNullException.ThrowIfNull(expression);
        ArgumentNullException.ThrowIfNull(entityType);
        ArgumentNullException.ThrowIfNull(propertyType);

        if (includeType == IncludeTypeEnum.ThenInclude)
        {
            ArgumentNullException.ThrowIfNull(previousPropertyType);
        }

        LambdaExpression = expression;
        EntityType = entityType;
        PropertyType = propertyType;
        PreviousPropertyType = previousPropertyType;
        Type = includeType;
    }

    public IncludeExpressionInfo(
        LambdaExpression expression,
        Type entityType,
        Type propertyType)
        : this(expression, entityType, propertyType, null, IncludeTypeEnum.Include)
    {
    }

    public IncludeExpressionInfo(
        LambdaExpression expression,
        Type entityType,
        Type propertyType,
        Type previousPropertyType)
        : this(expression, entityType, propertyType, previousPropertyType, IncludeTypeEnum.ThenInclude)
    {
    }
}
