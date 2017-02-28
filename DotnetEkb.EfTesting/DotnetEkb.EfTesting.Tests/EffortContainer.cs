using System;
using System.Data.Common;
using System.Data.Entity;
using System.Linq.Expressions;
using DotnetEkb.EfTesting.CommonData.Repositories;
using DotnetEkb.EfTesting.Tests.Helpers.CollectionHelpers;
using Effort;
using Effort.Extra;

namespace DotnetEkb.EfTesting.Tests
{
    public class EffortContainer<TContext> where TContext : DbContext
    {
        private readonly Func<DbConnection, TContext> _constructor;
        private readonly ObjectData _objectData;
        private TContext _context;

        public EffortContainer(Func<DbConnection, TContext> _constructor)
        {
            this._constructor = _constructor;
            _objectData = new ObjectData(TableNamingStrategy.Pluralised);
        }

        public DbConnection CurrentConnection { get; private set; }

        public TContext NewContext()
        {
            CurrentConnection = CurrentConnection ?? CreateTransientDbConnection();
            //_context = _context ?? (_context = CreateContext());
            return CreateContext();
        }

        public RepositoryManager RepositoryManager => new RepositoryManager(NewContext());

        public DbConnection CreateTransientDbConnection()
        {
            return DbConnectionFactory.CreateTransient(new ObjectDataLoader(_objectData));
        }

        private TContext CreateContext() => _constructor(CurrentConnection);

        public EffortContainer<TContext> FillData(Action<ObjectData> fillAction)
        {
            fillAction(_objectData);
            return this;
        }

        public EffortContainer<TContext> FillData(Action<TContext> fillAction)
        {
            fillAction(NewContext());
            return this;
        }

        public EffortContainer<TContext> AddTableData<TType>(Expression<Func<TContext, IDbSet<TType>>> memberExpression, params TType[] data) where TType : class
        {
            _objectData.Table<TType>((memberExpression.Body as MemberExpression).Member.Name).AddAll(data);
            return this;
        }

        public EffortContainer<TContext> AddTableData<TType>(params TType[] data) where TType : class
        {
            _objectData.Table<TType>().AddAll(data);
            return this;
        }

        public EffortContainer<TContext> AddTableData<TType>(string tableName, params TType[] data) where TType : class
        {
            _objectData.Table<TType>(tableName).AddAll(data);
            return this;
        }
    }
}
