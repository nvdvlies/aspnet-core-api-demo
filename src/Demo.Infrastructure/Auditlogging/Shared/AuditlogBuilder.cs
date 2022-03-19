﻿using Demo.Domain.Auditlog;
using Demo.Domain.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Demo.Infrastructure.Auditlogging.Shared
{
    internal class AuditlogBuilder<T>
    {
        private List<AuditlogItem> _auditlogItems;
        private T _current;
        private T _previous;

        private List<Action> _actions;

        public AuditlogBuilder()
        {
            _auditlogItems = new List<AuditlogItem>();
            _actions = new List<Action>();
        }

        public AuditlogBuilder<T> WithProperty(Expression<Func<T, string>> expression, AuditlogType type = AuditlogType.Text, Func<string, string> customFormatter = null)
        {
            _actions.Add(new Action(() => WithPropertyInternal(expression, type, customFormatter ?? AuditlogFormatters.StringFormatter)));
            return this;
        }

        public AuditlogBuilder<T> WithProperty(Expression<Func<T, Guid>> expression, AuditlogType type = AuditlogType.Text, Func<Guid, string> customFormatter = null)
        {
            _actions.Add(new Action(() => WithPropertyInternal(expression, type, customFormatter ?? AuditlogFormatters.StringFormatter)));
            return this;
        }

        public AuditlogBuilder<T> WithProperty(Expression<Func<T, Enum>> expression, AuditlogType type = AuditlogType.Text, Func<Enum, string> customFormatter = null)
        {
            _actions.Add(new Action(() => WithPropertyInternal(expression, type, customFormatter ?? AuditlogFormatters.EnumFormatter)));
            return this;
        }

        public AuditlogBuilder<T> WithProperty<T2>(Expression<Func<T, IList<T2>>> expression, AuditlogType type = AuditlogType.Text, Func<IList<T2>, string> customFormatter = null)
        {
            _actions.Add(new Action(() => WithPropertyInternal(expression, type, customFormatter ?? AuditlogFormatters.ListFormatter)));
            return this;
        }

        public AuditlogBuilder<T> WithProperty(Expression<Func<T, DateTime>> expression, AuditlogType type = AuditlogType.DateTime, Func<DateTime, string> customFormatter = null)
        {
            _actions.Add(new Action(() => WithPropertyInternal(expression, type, customFormatter ?? AuditlogFormatters.DateFormatter)));
            return this;
        }

        public AuditlogBuilder<T> WithProperty(Expression<Func<T, DateTime?>> expression, AuditlogType type = AuditlogType.DateTime, Func<DateTime?, string> customFormatter = null)
        {
            _actions.Add(new Action(() => WithPropertyInternal(expression, type, customFormatter ?? AuditlogFormatters.DateFormatter)));
            return this;
        }

        public AuditlogBuilder<T> WithProperty(Expression<Func<T, int>> expression, AuditlogType type = AuditlogType.Number, Func<int, string> customFormatter = null)
        {
            _actions.Add(new Action(() => WithPropertyInternal(expression, type, customFormatter ?? AuditlogFormatters.NumberFormatter)));
            return this;
        }

        public AuditlogBuilder<T> WithProperty(Expression<Func<T, long>> expression, AuditlogType type = AuditlogType.Number, Func<long, string> customFormatter = null)
        {
            _actions.Add(new Action(() => WithPropertyInternal(expression, type, customFormatter ?? AuditlogFormatters.NumberFormatter)));
            return this;
        }

        public AuditlogBuilder<T> WithProperty(Expression<Func<T, decimal>> expression, AuditlogType type = AuditlogType.Decimal, Func<decimal, string> customFormatter = null)
        {
            _actions.Add(new Action(() => WithPropertyInternal(expression, type, customFormatter ?? AuditlogFormatters.DecimalFormatter)));
            return this;
        }

        public AuditlogBuilder<T> WithProperty(Expression<Func<T, bool>> expression, AuditlogType type = AuditlogType.YesNo, Func<bool, string> customFormatter = null)
        {
            _actions.Add(new Action(() => WithPropertyInternal(expression, type, customFormatter ?? AuditlogFormatters.BooleanFormatter)));
            return this;
        }

        public AuditlogBuilder<T> WithChildEntity<T2>(Expression<Func<T, T2>> expression, AuditlogBuilder<T2> builder)
        {
            _actions.Add(new Action(() => WithChildEntityInternal(expression, builder)));
            return this;
        }

        public AuditlogBuilder<T> WithChildEntityCollection<T2, T3>(Expression<Func<T, T2>> expression, AuditlogBuilder<T3> builder) where T2 : IEnumerable<T3> where T3 : IEntity
        {
            return WithChildEntityCollection(expression, null, builder);
        }

        public AuditlogBuilder<T> WithChildEntityCollection<T2, T3>(Expression<Func<T, T2>> expression, Expression<Func<T3, string>> collectionItemPropertyname, AuditlogBuilder<T3> builder) where T2 : IEnumerable<T3> where T3 : IEntity
        {
            _actions.Add(new Action(() => WithChildEntityCollectionInternal(expression, collectionItemPropertyname, builder)));
            return this;
        }

        public List<AuditlogItem> Build(T current, T previous)
        {
            _auditlogItems = new List<AuditlogItem>();
            _current = current;
            _previous = previous;
            foreach (var action in _actions)
            {
                action.Invoke();
            }
            return _auditlogItems;
        }

        private void WithPropertyInternal<T2>(Expression<Func<T, T2>> expression, AuditlogType type, Func<T2, string> formatter)
        {
            var propertyName = GetPropertyName(expression);
            T2 currentValue = _current != null ? expression.Compile()(_current) : default;
            T2 previousValue = _previous != null ? expression.Compile()(_previous) : default;

            var status = GetAuditlogStatus(currentValue, previousValue);

            if (status != AuditlogStatus.Unchanged)
            {
                var item = new AuditlogItem
                {
                    PropertyName = propertyName,
                    Type = type,
                    Status = status,
                    CurrentValueAsString = formatter(currentValue),
                    PreviousValueAsString = formatter(previousValue),
                };
                _auditlogItems.Add(item);
            }
        }

        private void WithChildEntityInternal<T2>(Expression<Func<T, T2>> expression, AuditlogBuilder<T2> builder)
        {
            var propertyName = ((MemberExpression)expression.Body).Member.Name;
            T2 currentValue = _current != null ? expression.Compile()(_current) : default;
            T2 previousValue = _previous != null ? expression.Compile()(_previous) : default;

            var auditLogItems = builder.Build(currentValue, previousValue);

            if (auditLogItems?.Count == 0)
            {
                return;
            }

            var status = GetAuditlogStatus(currentValue, previousValue);
            if (status == AuditlogStatus.Unchanged)
            {
                // Set status to updated because a child has changes
                status = AuditlogStatus.Updated;
            }

            var item = new AuditlogItem
            {
                PropertyName = propertyName,
                Status = status,
                Type = AuditlogType.None,
                AuditlogItems = auditLogItems
            };
            _auditlogItems.Add(item);
        }

        private void WithChildEntityCollectionInternal<T2, T3>(Expression<Func<T, T2>> expression, Expression<Func<T3, string>> collectionItemPropertyname, AuditlogBuilder<T3> builder) where T2 : IEnumerable<T3> where T3 : IEntity
        {
            var propertyName = ((MemberExpression)expression.Body).Member.Name;
            T2 currentValue = _current != null ? expression.Compile()(_current) : default;
            T2 previousValue = _previous != null ? expression.Compile()(_previous) : default;

            var status = GetAuditlogStatus(currentValue, previousValue);

            currentValue ??= (T2)Activator.CreateInstance(typeof(List<>).MakeGenericType(typeof(T3)));
            previousValue ??= (T2)Activator.CreateInstance(typeof(List<>).MakeGenericType(typeof(T3)));

            var updated = currentValue
                .Join(previousValue, x => x.Id, x => x.Id, (current, previous) =>
                {
                    return new AuditlogPair<T3>
                    {
                        Id = current.Id,
                        CurrentValue = current,
                        PreviousValue = previous
                    };
                });
            var added = currentValue.Where(x => !previousValue.Any(y => y.Id == x.Id))
                    .Select(x => new AuditlogPair<T3>
                    {
                        Id = x.Id,
                        CurrentValue = x
                    });
            var removed = previousValue.Where(x => !currentValue.Any(y => y.Id == x.Id))
                    .Select(previous => new AuditlogPair<T3>
                    {
                        Id = previous.Id,
                        PreviousValue = previous
                    });

            var auditlogPairs = updated.Concat(added).Concat(removed);

            var item = new AuditlogItem
            {
                PropertyName = propertyName,
                Status = status,
                Type = AuditlogType.None,
                AuditlogItems = new List<AuditlogItem>()
            };

            var i = 0;
            foreach (var auditlogPair in auditlogPairs)
            {
                var auditLogItems = builder.Build(auditlogPair.CurrentValue, auditlogPair.PreviousValue);

                if (auditLogItems.Count > 0)
                {
                    i++;

                    var auditlogPairStatus = GetAuditlogStatus(auditlogPair.CurrentValue, auditlogPair.PreviousValue);

                    var item2 = new AuditlogItem
                    {
                        PropertyName = collectionItemPropertyname != null
                            ? collectionItemPropertyname.Compile()(auditlogPair.CurrentValue ?? auditlogPair.PreviousValue).ToString()
                            : i.ToString(),
                        Status = auditlogPairStatus,
                        Type = AuditlogType.None,
                        AuditlogItems = auditLogItems
                    };

                    item.AuditlogItems.Add(item2);
                }
            }

            if (item.AuditlogItems.Count == 0)
            {
                return;
            }

            _auditlogItems.Add(item);
        }

        private static AuditlogStatus GetAuditlogStatus<TType>(TType current, TType previous)
        {
            var comparer = EqualityComparer<TType>.Default;
            if (comparer.Equals(current, previous))
            {
                return AuditlogStatus.Unchanged;
            }
            else if (comparer.Equals(current, default) && !comparer.Equals(previous, default))
            {
                return AuditlogStatus.Removed;
            }
            else if (!comparer.Equals(current, default) && comparer.Equals(previous, default))
            {
                return AuditlogStatus.Added;
            }
            return AuditlogStatus.Updated;
        }

        private static string GetPropertyName<T2>(Expression<Func<T, T2>> expression)
        {
            if (expression.Body is MemberExpression)
            {
                return ((MemberExpression)expression.Body).Member.Name;
            }
            else
            {
                var operand = ((UnaryExpression)expression.Body).Operand;
                return ((MemberExpression)operand).Member.Name;
            }
        }
    }
}
