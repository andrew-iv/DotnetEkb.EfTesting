using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DotnetEkb.EfTesting.CommonData.Interfaces;
using DotnetEkb.EfTesting.CommonData.Repositories.Interfaces;
using DotnetEkb.EfTesting.Tests.Helpers.EnumerableHelpers;

namespace DotnetEkb.EfTesting.Tests.Stubs.Relations
{
    public class EntityDependenciesContainer
    {
        private readonly Func<IRepositoryManager> _managerResolver;
        private readonly List<object> _links = new List<object>();

        public EntityDependenciesContainer(Func<IRepositoryManager> managerResolver)
        {
            _managerResolver = managerResolver;
        }

        public EntityDependenciesContainer AddOneToManyLink<TSource, TDestination>(Expression<Func<TSource, int?>> fkId,
            Expression<Func<TSource, TDestination>> navProperty,
            Expression<Func<TDestination, ICollection<TSource>>> reverse = null) where TSource : class, IWithId where TDestination : class, IWithId
        {
            var oneToManyLink = new OneToManyLink<TSource, TDestination>(fkId, navProperty, reverse);
            var reverseLink = oneToManyLink.ReverseLink;
            _links.Add(oneToManyLink);
            if(reverseLink != null)
                _links.Add(reverseLink);
            return this;
        }

        public void Link<TObject>(TObject @object) where TObject : class
        {
            foreach (var toLink in _links.OfType<ILink<TObject>>().NotNull())
            {
                toLink.Link(_managerResolver(), @object);
            }
        }
    }
}