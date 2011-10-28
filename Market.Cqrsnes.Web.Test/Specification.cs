using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Web.Test
{
    public class Specification<TProjector>
    {
        public Specification()
        {
            Name = "Projection logic of " + Prettify(typeof(TProjector).Name) + " (SUT)";
            Given_ = new Event[0];
        }

        public string Name { get; set; }

        public IEnumerable<Event> Given_ { get; set; }

        public Event When_ { get; set; }

        public IList<Expression<Func<TProjector, bool>>> Expect_ { get; set; }

        public bool IsExceptionExpected { get; set; }

        public Specification<TProjector> When(Event @event)
        {
            When_ = @event;
            return this;
        }

        public Specification<TProjector> Expect(Expression<Func<TProjector, bool>> expression)
        {
            if (Expect_ == null)
            {
                Expect_ = new List<Expression<Func<TProjector, bool>>>();
            }

            Expect_.Add(expression);

            return this;
        }

        public ExecutionResult Run()
        {
            var result = new ExecutionResult {IsPassed = true};

            var eventHandlerType = typeof (IEventHandler<>);
            var projectorType = typeof (TProjector);

            var projector = (TProjector) Activator.CreateInstance(projectorType, new TestRepository());

            foreach (var @event in Given_)
            {
                var eventType = @event.GetType();

                var type = eventHandlerType.MakeGenericType(eventType);
                if (!type.IsAssignableFrom(projectorType))
                {
                    throw new InvalidOperationException(
                        "Projector can't handle one of the given events.");
                }

                projectorType.GetMethod("Handle", new[] {eventType})
                    .Invoke(projector, new[] {@event});
            }

            var exceptionMessage = string.Empty;
            try
            {
                projectorType.GetMethod("Handle",
                                        new[] {When_.GetType()}).Invoke(projector, new[] {When_});
            }
            catch (Exception exception)
            {
                exceptionMessage = exception.Message;
            }

            PrintSpecification(
                Expect_.Select(x => ProcessExpectation(projector, x)),
                result, exceptionMessage);

            return result;
        }

        private void PrintSpecification(IEnumerable<ExecutionResult> results, ExecutionResult result, string exceptionMessage)
        {
            var s = new StringBuilder();

            s.AppendFormat("Specification: \"{0}\"\n", Name);
            s.AppendLine();

            var hasGiven = Given_.Count() > 0;
            if (hasGiven)
            {
                s.AppendLine("Given:");
            }
            foreach (var @event in Given_)
            {
                s.AppendFormat("\t{0}\n", @event);
            }
            if (hasGiven)
            {
                s.AppendLine();
            }

            s.AppendLine("When:");
            s.AppendFormat("\t{0}\n\n", When_);

            s.AppendLine("Expect:");
            foreach (var expectationResult in results)
            {
                s.AppendFormat("\t{0}\n", expectationResult);
            }
            var gotException = !string.IsNullOrEmpty(exceptionMessage);
            s.AppendFormat(
                "\t{0}: {1}\n",
                IsExceptionExpected ? "exception" : "no exception",
                IsExceptionExpected
                    ? (gotException ? string.Format("passed \"({0})\"", exceptionMessage) : "failed")
                    : (gotException ? string.Format("failed \"({0})\"", exceptionMessage) : "passed"));
            s.AppendLine();

            result.IsPassed = results.All(x => x.IsPassed) &&
                              (IsExceptionExpected && gotException ||
                               !IsExceptionExpected && !gotException);

            s.AppendFormat("Done ({0}).", result.IsPassed ? "passed" : "failed");

            result.Description = s.ToString();
        }

        private ExecutionResult ProcessExpectation<T>(T instance, Expression<Func<T, bool>> expression)
        {
            var comparison = expression.Body as BinaryExpression;

            if (comparison == null)
            {
                throw new InvalidOperationException(
                    "Provided expression is not a binary one.");
            }

            if (comparison.NodeType != ExpressionType.Equal &&
                comparison.NodeType != ExpressionType.NotEqual)
            {
                throw new InvalidOperationException(
                    "Only strict equality comparison expressions are supported.");
            }

            var actualValueDescription = GetActualValueDescription(comparison.Left);
            var expectedValue = GetExpectedValue(comparison.Right);
            var result = expression.Compile()(instance);

            return new ExecutionResult()
                       {
                           IsPassed = result,
                           Description = string.Format(
                               "{0} {1} {2}: {3}",
                               actualValueDescription,
                               comparison.NodeType == ExpressionType.Equal
                                   ? "must be equal to"
                                   : "must not be equal to",
                               expectedValue,
                               result
                                   ? "passed"
                                   : "failed")
                       };
        }

        private object GetExpectedValue(Expression expected)
        {
            object expectedValue = null;
            if (expected is ConstantExpression)
            {
                expectedValue = (expected as ConstantExpression).Value;
            }
            else if (expected is MemberExpression)
            {
                var objectMember = Expression.Convert(expected, typeof(object));
                var getterLambda = Expression.Lambda<Func<object>>(objectMember);
                var getter = getterLambda.Compile();
                expectedValue = getter();
            }
            else
            {
                throw new InvalidOperationException(
                    "Expected value must be a right hand operand of comparison.");
            }
            return expectedValue;
        }

        private string GetActualValueDescription(Expression expression)
        {
            if (expression is MemberExpression)
            {
                var memberExpression = expression as MemberExpression;
                return Prettify(memberExpression.Member.Name) +
                       (memberExpression.Expression == null
                           ? ""
                           : " of " + GetActualValueDescription(memberExpression.Expression));
            }

            if (expression is MethodCallExpression)
            {
                var methodCallExpression = expression as MethodCallExpression;
                return Prettify(methodCallExpression.Method.Name) +
                       (methodCallExpression.Object == null
                            ? (methodCallExpression.Method.IsDefined(typeof (ExtensionAttribute), true)
                                   ? " of " + GetActualValueDescription(methodCallExpression.Arguments[0])
                                   : "")
                            : " of " + GetActualValueDescription(methodCallExpression.Object));
            }

            if (expression is ParameterExpression)
            {
                var parameterExpression = expression as ParameterExpression;
                return parameterExpression.Name.Length < 3 ? "SUT" : Prettify(parameterExpression.Name);
            }

            throw new InvalidOperationException("Unexpected expression.");
        }

        private string Prettify(string name)
        {
            if (name.ToLower().StartsWith("get"))
            {
                name = name.Substring(3);
            }

            return Regex
                .Replace(name, "([A-Z])", " $1", RegexOptions.Compiled)
                .TrimStart()
                .Replace('_', ' ')
                .ToLower();
        }
    }

    public class ExecutionResult
    {
        public bool IsPassed { get; set; }

        public string Description { get; set; }

        public override string ToString()
        {
            return Description;
        }
    }
}