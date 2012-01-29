using System;
using System.Collections.Generic;
using System.Text;
using Cqrsnes.Infrastructure;
using Cqrsnes.Infrastructure.Impl;

namespace Cqrsnes.Test
{
    /// <summary>
    /// Represents saga (behavior) specification.
    /// </summary>
    /// <typeparam name="TEvent">
    /// Incoming event type.
    /// </typeparam>
    /// <typeparam name="TSaga">
    /// Saga type.
    /// </typeparam>
    public class SagaSpecification<TEvent, TSaga> : ISpecification
        where TEvent : Event
        where TSaga : IEventHandler<TEvent>
    {
        private TEvent when;
        private IEnumerable<Event> expect;

        /// <summary>
        /// Initializes a new instance of the <see cref="SagaSpecification{TEvent,TSaga}"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        public SagaSpecification(string name = null)
        {
            Name = name ?? "Unnamed saga specification.";
        }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Sets up initial event (cause).
        /// </summary>
        /// <param name="event">
        /// The event.
        /// </param>
        /// <returns>
        /// Reference to self.
        /// </returns>
        public SagaSpecification<TEvent, TSaga> When(TEvent @event)
        {
            when = @event;
            return this;
        }

        /// <summary>
        /// Sets up expectation.
        /// </summary>
        /// <param name="events">
        /// The events.
        /// </param>
        /// <returns>
        /// Reference to self.
        /// </returns>
        public SagaSpecification<TEvent, TSaga> Expect(IEnumerable<Event> events)
        {
            expect = events;
            return this;
        }

        /// <summary>
        /// Executes specification.
        /// </summary>
        /// <returns>Execution result.</returns>
        public ExecutionResult Run()
        {
            var result = new ExecutionResult { IsPassed = true };
            var s = new StringBuilder();

            PrintSpecification(s);

            try
            {
                var store = new TestEventStore(new Event[] { when });
                var handler = (IEventHandler<TEvent>) Activator.CreateInstance(
                    typeof(TSaga), new object[] { new CommonAggregateRootRepository(store, new TestBus()) });

                handler.Handle(when);

                var produced = store.GetProducedEvents();
                var correct = Infrastructure.Impl.Utilities.SequenceEqual(expect, produced);
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

            s.AppendLine("When:");
            s.AppendFormat("\t{0}\n", Utilities.Describe(when));
            s.AppendLine();

            s.AppendLine("Expect:");
            foreach (var @event in expect)
            {
                s.AppendFormat("\t{0}\n", Utilities.Describe(@event));
            }

            s.AppendLine();
        }
    }
}
