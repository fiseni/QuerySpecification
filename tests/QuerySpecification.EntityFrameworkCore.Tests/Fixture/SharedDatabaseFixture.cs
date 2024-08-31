using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Pozitron.QuerySpecification.EntityFrameworkCore.Tests.Fixture
{
    public class SharedDatabaseFixture : IDisposable
    {
        public const string ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=QuerySpecificationEFTestsDB;ConnectRetryCount=0";
        private static readonly object _lock = new object();
        private static bool _databaseInitialized;

        public SharedDatabaseFixture()
        {
            Connection = new SqlConnection(ConnectionString);

            Seed();

            Connection.Open();
        }

        public DbConnection Connection { get; }

        public TestDbContext CreateContext(DbTransaction? transaction = null)
        {
            var context = new TestDbContext(new DbContextOptionsBuilder<TestDbContext>().UseSqlServer(Connection).Options);

            if (transaction != null)
            {
                context.Database.UseTransaction(transaction);
            }

            return context;
        }

        private void Seed()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using (var context = CreateContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();
                    }

                    _databaseInitialized = true;
                }
            }
        }

        public void Dispose() => Connection.Dispose();
    }
}
