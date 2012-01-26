//using System;
//using System.Collections.Generic;
//using Cqrsnes.Infrastructure;
//using Cqrsnes.Test;
//using Market.Cqrsnes.Domain.Commands;
//using Market.Cqrsnes.Domain.Events;
//using Market.Cqrsnes.Domain.Handlers;

//namespace Market.Cqrsnes.Domain.Test
//{
//    public class ArticleSpecifications : ISpecificationHolder
//    {
//        private readonly Guid id = Guid.NewGuid();
//        private readonly string name = "New Article";

//        public ExecutionResult Create()
//        {
//            return new DomainSpecification<CreateArticle, ArticleCommandHandler>
//                       {
//                           Name = "Create Article",
//                           When = new CreateArticle(id, name),
//                           Expect = new[] { new OfferCreated(id, name) }
//                       }.Run();
//        }

//        public ExecutionResult Deliver()
//        {
//            return new DomainSpecification<SupplyArticle, ArticleCommandHandler>
//                       {
//                           Name = "Deliver Article",
//                           Given = new[] { new OfferCreated(id, name) },
//                           When = new SupplyArticle(id, 1),
//                           Expect = new[] { new ArticleDelivered(id, 1) }
//                       }.Run();
//        }

//        public ExecutionResult BuyLessThanExist()
//        {
//            return new DomainSpecification<BuyArticle, ArticleCommandHandler>
//                       {
//                           Name = "Article: Buy Less Than Exist",
//                           Given = new Event[]
//                                       {
//                                           new OfferCreated(id, name),
//                                           new ArticleDelivered(id, 100)
//                                       },
//                           When = new BuyArticle(id, 1),
//                           Expect = new[] { new ArticleBought(id, 1) }
//                       }.Run();
//        }

//        public ExecutionResult BuyMoreThanExist()
//        {
//            return new DomainSpecification<BuyArticle, ArticleCommandHandler>
//                       {
//                           Name = "Article: Buy More Than Exist",
//                           Given = new Event[]
//                                       {
//                                           new OfferCreated(id, name),
//                                           new ArticleDelivered(id, 10)
//                                       },
//                           When = new BuyArticle(id, 100),
//                           IsExceptionExpected = true
//                       }.Run();
//        }

//        public IEnumerable<ExecutionResult> ExecuteAll()
//        {
//            return new[]
//                       {
//                           Create(),
//                           Deliver(),
//                           BuyLessThanExist(),
//                           BuyMoreThanExist()
//                       };
//        }
//    }
//}