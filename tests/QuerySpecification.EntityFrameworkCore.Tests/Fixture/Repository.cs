using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Tests.Fixture;

public class Repository<T>(DbContext context) : RepositoryBase<T>(context) where T : class
{
    private static readonly Lazy<IMapper> _mapper = new(() =>
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(typeof(Repository<>).Assembly);
        });
        return config.CreateMapper();
    });

    protected override IQueryable<TResult> Map<TResult>(IQueryable<T> source)
    {
        var result = source
            .ProjectTo<TResult>(_mapper.Value.ConfigurationProvider);

        return result;
    }
}
