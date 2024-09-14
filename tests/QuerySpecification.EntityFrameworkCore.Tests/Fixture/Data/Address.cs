﻿namespace Pozitron.QuerySpecification.EntityFrameworkCore.Tests.Fixture;

public class Address
{
    public int Id { get; set; }
    public string? Street { get; set; }

    public int StoreId { get; set; }
    public Store Store { get; set; } = default!;

    public object GetSomethingFromAddress()
    {
        return new object();
    }
}
