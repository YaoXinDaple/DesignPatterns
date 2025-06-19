using DecoratePattern.Models;
using DecoratePattern.Services;

namespace DecoratePattern.Decorator
{
    public class IdempotentHandler<TDomainEvent>(IDomainEventHandler<TDomainEvent> domainEventHandler)
        :DomainEventHandler<TDomainEvent>
        where TDomainEvent : IDomainEvent
    {
        public override Task HandleAsync(TDomainEvent domainEvent)
        {
            Console.WriteLine("Start Idempotent!");

            //use domainEvent context to check if the event has been processed 

            domainEventHandler.HandleAsync(domainEvent);

            Console.WriteLine("End Idempotent!");

            return Task.CompletedTask;
        }
    }
}
