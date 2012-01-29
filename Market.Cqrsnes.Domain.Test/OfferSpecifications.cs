using System;
using System.Collections.Generic;
using Cqrsnes.Infrastructure;
using Cqrsnes.Test;
using Market.Cqrsnes.Domain.Commands;
using Market.Cqrsnes.Domain.Events;
using Market.Cqrsnes.Domain.Handlers;
using NUnit.Framework;

namespace Market.Cqrsnes.Domain.Test
{
    public class OfferSpecifications : ISpecificationHolder
    {
        private readonly Guid id = Guid.NewGuid();
        private readonly Guid articleId = Guid.NewGuid();
        private readonly Guid storeId = Guid.NewGuid();
        private readonly Guid userId = Guid.NewGuid();
        private readonly Guid purchaseId = Guid.NewGuid();

        public string Name { get { return "Offer Specifications"; } }

        public ExecutionResult Create()
        {
            return new DomainSpecification<CreateOffer, OfferCommandHandler>
                       {
                           Name = "Offer Creation",
                           When = new CreateOffer
                                      {
                                          Id = id,
                                          ArticleId = articleId,
                                          StoreId = storeId,
                                          Count = 10,
                                          Price = 55.7
                                      },
                           Expect = new[]
                                        {
                                            new OfferCreated
                                                {
                                                    Id = id,
                                                    ArticleId = articleId,
                                                    StoreId = storeId,
                                                    Count = 10,
                                                    Price = 55.7
                                                }
                                        }
                       }.Run();
        }

        public ExecutionResult ReserveArticle()
        {
            return new DomainSpecification<ReserveArticle, OfferCommandHandler>
                       {
                           Name = "Article Reservation",
                           Given = new Event[]
                                       {
                                           new OfferCreated
                                               {
                                                   Id = id,
                                                   ArticleId = articleId,
                                                   StoreId = storeId,
                                                   Price = 12.55,
                                                   Count = 50
                                               },
                                           new PurchaseCreated
                                               {
                                                   OfferId = id,
                                                   Id = purchaseId,
                                                   UserId = userId,
                                                   Amount = 25.10,
                                                   Count = 2
                                               }
                                       },
                           When = new ReserveArticle
                                      {
                                          PurchaseId = purchaseId
                                      },
                           Expect = new Event[]
                                        {
                                            new ArticleReserved
                                                {
                                                    PurchaseId = purchaseId,
                                                    Count = 2
                                                }
                                        }

                       }.Run();
        }

        public IEnumerable<ExecutionResult> ExecuteAll()
        {
            return new[]
                       {
                           Create(),
                           ReserveArticle()
                       };
        }
    }

    [TestFixture]
    public class OfferTests
    {
        [Test]
        public void Create()
        {
            new OfferSpecifications().Create().AssertResult();
        }

        [Test]
        public void ReserveArticle()
        {
            new OfferSpecifications().ReserveArticle().AssertResult();
        }
    }
}