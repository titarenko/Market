using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cqrsnes.Infrastructure;
using Cqrsnes.Infrastructure.Impl;

namespace Cqrsnes.Test
{
    public class DomainSpecification<TCommand, THandlerType> 
        where TCommand : Command
        where THandlerType : ICommandHandler<TCommand>
    {
        public string Name { get; set; }

        public IEnumerable<Event> Given { get; set; }

        public TCommand When { get; set; }

        public IEnumerable<Event> Expect { get; set; }

        public bool IsExceptionExpected { get; set; }

        public DomainSpecification()
        {
            Name = "Unnamed Specification";
            Given = new Event[0];
            Expect = new Event[0];
            IsExceptionExpected = false;
        }

        public ExecutionResult Run()
        {
            var result = new ExecutionResult {IsPassed = true};
            var s = new StringBuilder();

            PrintSpecification(s);

            try
            {
                var store = new TestEventStore(Given);
                var handler = (ICommandHandler<TCommand>) Activator.CreateInstance(
                    typeof (THandlerType), new object[] {new CommonAggregateRootRepository(store, new TestBus())});

                try
                {
                    handler.Handle(When);
                    result.IsPassed = result.IsPassed && !IsExceptionExpected;
                    if (IsExceptionExpected)
                    {
                        s.AppendLine("Failure: exception was not thrown.");
                    }
                }
                catch (Exception e)
                {
                    result.IsPassed = result.IsPassed && IsExceptionExpected;
                    s.AppendFormat(
                        !IsExceptionExpected
                            ? "Failure: unexpected exception (\"{0}\").\n"
                            : "Success: got expected exception (\"{0}\").\n", e.Message);
                }

                var produced = store.GetProducedEvents();
                var correct = Expect.SequenceEqual(produced);
                result.IsPassed = result.IsPassed && correct;
                s.AppendLine(!correct
                                 ? "Failure: produced events didn't match expected."
                                 : "Success: produced events matched expected.");
            }
            catch (Exception e)
            {
                s.AppendFormat("Failure: really unexpected exception (\"{0}\").\n", e.Message);
                result.IsPassed = false;
            }

            s.AppendFormat("Done ({0}).\n", result.IsPassed ? "passed" : "failed");
            result.Details = s.ToString();
            return result;
        }

        private void PrintSpecification(StringBuilder s)
        {
            s.AppendFormat("Specification: \"{0}\"\n", Name);
            s.AppendLine();

            if (Given.Count() > 0)
            {
                s.AppendLine("Given:");
            }
            foreach (var @event in Given)
            {
                s.AppendFormat("\t{0}\n", @event);
            }
            if (Given.Count() > 0)
            {
                s.AppendLine();
            }

            s.AppendLine("When:");
            s.AppendFormat("\t{0}\n", When);
            s.AppendLine();

            s.AppendLine("Expect:");
            foreach (var @event in Expect)
            {
                s.AppendFormat("\t{0}\n", @event);
            }
            if (IsExceptionExpected)
            {
                s.AppendLine("\tException is thrown.");
            }
            s.AppendLine();
        }
    }
}