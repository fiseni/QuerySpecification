﻿namespace Pozitron.QuerySpecification.EntityFrameworkCore.Benchmarks;

public class Store
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public int CompanyId { get; set; }
    public Company Company { get; set; } = default!;

    public Address Address { get; set; } = default!;

    public List<Product> Products { get; set; } = [];
}