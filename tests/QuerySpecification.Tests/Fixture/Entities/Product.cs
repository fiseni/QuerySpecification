﻿namespace Pozitron.QuerySpecification.Tests.Fixture;

public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public int StoreId { get; set; }
    public Store? Store { get; set; }
}
