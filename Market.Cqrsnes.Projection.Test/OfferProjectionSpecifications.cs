using System;
using System.Collections.Generic;
using Cqrsnes.Infrastructure;
using Cqrsnes.Test;
using Market.Cqrsnes.Domain.Events;
using Market.Cqrsnes.Projection.Handlers;
using Market.Cqrsnes.Projection.Models;
using NUnit.Framework;

namespace Market.Cqrsnes.Projection.Test
{
    public class OfferProjectionSpecifications : ISpecificationHolder
    {
        private readonly Guid offerId = Guid.NewGuid();
        private readonly Guid articleId = Guid.NewGuid();
        private readonly Guid storeId = Guid.NewGuid();
            
        public ExecutionResult Create()
        {
            return new ProjectionSpecification<OfferEventHandler>
                {
                    Name = "Creation Of Offer"
                }
                .Given(x => x.Save(new Store
                    {
                        Id = storeId,
                        Name = "Ivushka",
                        OwnerId = Guid.NewGuid()
                    }))
                .Given(x => x.Save(new Article
                    {
                        Id = articleId,
                        Name = "Milk"
                    }))
                .When(new OfferCreated
                    {
                        Id = offerId,
                        ArticleId = articleId,
                        StoreId = storeId,
                        Price = 10.15,
                        Count = 3
                    })
                .Expect(x => x.GetSingle<Offer>().Id == offerId)
                .Expect(x => x.GetSingle<Offer>().ArticleId == articleId)
                .Expect(x => x.GetSingle<Offer>().ArticleName == "Milk")
                .Expect(x => x.GetSingle<Offer>().StoreId == storeId)
                .Expect(x => x.GetSingle<Offer>().StoreName == "Ivushka")
                .Expect(x => x.GetSingle<Offer>().Count == 3)
                .Expect(x => x.GetSingle<Offer>().Price == 10.15)
                .Run();
        }

        public IEnumerable<ExecutionResult> ExecuteAll()
        {
            return new[]
                       {
                           Create()
                       };
        }
    }

    [TestFixture]
    public class OfferProjectionTests
    {
        [Test]
        public void Create()
        {
            new OfferProjectionSpecifications().Create().AssertResult();
        }
    }
}