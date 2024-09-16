using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Pozitron.QuerySpecification.EntityFrameworkCore.Tests.Fixture;

public class Repository<T> : RepositoryBase<T> where T : class
{
    private static readonly Lazy<IMapper> _mapper = new(() =>
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(typeof(Repository<>).Assembly);
        });
        return config.CreateMapper();
    });

    public Repository(DbContext context) : base(context)
    {
    }

    protected override IQueryable<TResult> Map<TResult>(IQueryable<T> queryable)
    {
        var result = queryable
            .ProjectTo<TResult>(_mapper.Value.ConfigurationProvider);

        return result;
    }
}
