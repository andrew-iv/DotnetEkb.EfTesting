using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DotnetEkb.EfTesting.CommonData.Interfaces;
using DotnetEkb.EfTesting.CommonData.Repositories.Interfaces;
using DotnetEkb.EfTesting.Tests.Reflection;

namespace DotnetEkb.EfTesting.Tests.Stubs.Relations
{
    //TODO: реанимировать
    class OneToManyLink<TSource, TDestination> : ILink<TSource> where TSource : class, IWithId where TDestination : class, IWithId
    {
        private readonly IMemberAccessor<TSource, int?> _fkIdAccessor;
        private readonly IMemberAccessor<TSource, TDestination> _navPropertyAccessor;
        private readonly IMemberAccessor<TDestination, ICollection<TSource>> _reverseAccessor;
        
        public OneToManyLink(Expression<Func<TSource, int?>> fkId,
            Expression<Func<TSource, TDestination>> navProperty,
            Expression<Func<TDestination, ICollection<TSource>>> reverse = null
            )
        {
            _fkIdAccessor = ReflectionHelper.CreatePropertyAccessor(fkId);
            _navPropertyAccessor = ReflectionHelper.CreatePropertyAccessor(navProperty);
            _reverseAccessor = reverse != null ? ReflectionHelper.CreatePropertyAccessor(reverse) : null;
        }

        public void Link(IRepositoryManager repositoryManager, TSource @object)
        {
            var destRepo = repositoryManager.GetCommonRepository<TDestination>();
            var idValue = _fkIdAccessor.GetValue(@object);
            var navPropertyValue = _navPropertyAccessor.GetValue(@object);
            var idHasValue = idValue != null && idValue != 0;
            if (!idHasValue && navPropertyValue != null)
            {
                _fkIdAccessor.SetValue(@object, navPropertyValue.Id);
            }
            else if (idHasValue)
            {
                navPropertyValue = destRepo.FindById(idValue.Value);
                _navPropertyAccessor.SetValue(@object, navPropertyValue);
            }
            if (navPropertyValue != null && _reverseAccessor != null)
            {
                var collection = GetReverseCollection(navPropertyValue);
                if (CanAddToCollectionPredicate(collection)(@object))
                {
                    collection.Add(@object);
                }
            }
        }

        private Func<TSource, bool> ReversePredicate(TDestination @object)
        {
            //if (@object.Id != 0)
            //    return source => _fkIdAccessor.GetValue(source) == @object.Id || _navPropertyAccessor.GetValue(source) == @object;
            return source => _navPropertyAccessor.GetValue(source) == @object;
        }

        public ILink<TDestination> ReverseLink => _reverseAccessor != null ? new ReverseLinkAdaptor(this) : null;


        private class ReverseLinkAdaptor : ILink<TDestination>
        {
            private readonly OneToManyLink<TSource, TDestination> _oneToManyLink;

            public ReverseLinkAdaptor(OneToManyLink<TSource, TDestination> oneToManyLink)
            {
                _oneToManyLink = oneToManyLink;
            }

            public void Link(IRepositoryManager repositoryManager, TDestination @object)
            {
                _oneToManyLink.Link(repositoryManager, @object);
            }
        }

        private static Func<TObject, bool> CanAddToCollectionPredicate<TObject>(ICollection<TObject> collection) where TObject : class
        {
            return (toAdd) => !collection.Contains(toAdd);/* && collection.All(xx => xx.Id != toAdd.Id);*/
        }

        public void Link(IRepositoryManager repositoryManager, TDestination @object)
        {
            var sourceRepo = repositoryManager.GetCommonRepository<TSource>();
            var collectionValue = GetReverseCollection(@object);

            var predicate = ReversePredicate(@object);
            foreach (var related in sourceRepo.Find().Where(predicate).Where(CanAddToCollectionPredicate(collectionValue)))
            {
                collectionValue.Add(related);
            }

            if (collectionValue != null)
                foreach (var collectionItem in collectionValue)
                {
                    if (@object.Id != 0)
                        _fkIdAccessor.SetValue(collectionItem, @object.Id);
                    if (_navPropertyAccessor.GetValue(collectionItem) == null)
                        _navPropertyAccessor.SetValue(collectionItem, @object);
                }
        }

        private ICollection<TSource> GetReverseCollection(TDestination @object)
        {
            var collectionValue = _reverseAccessor.GetValue(@object);
            if (collectionValue == null || collectionValue.IsReadOnly)
            {
                collectionValue = collectionValue == null ? new List<TSource>() : new List<TSource>(collectionValue);
                _reverseAccessor.SetValue(@object, collectionValue);
            }
            return collectionValue;
        }
    }
}