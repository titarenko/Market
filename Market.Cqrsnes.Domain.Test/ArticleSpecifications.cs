using System;
using System.Collections.Generic;
using Cqrsnes.Infrastructure;
using Cqrsnes.Test;

namespace Market.Cqrsnes.Domain.Test
{
    public class ArticleSpecifications : ISpecificationHolder
    {
        private readonly Guid id = Guid.NewGuid();
        private readonly string name = "New Article";

        public ExecutionResult Create()
        {
            return new DomainSpecification<CreateArticle, ArticleCommandsHandler>
                       {
                           Name = "Create Article",
                           When = new CreateArticle(id, name),
                           Expect = new[] {new ArticleCreated(id, name)}
                       }.Run();
        }

        public ExecutionResult Deliver()
        {
            return new DomainSpecification<DeliverArticle, ArticleCommandsHandler>
                       {
                           Name = "Deliver Article",
                           Given = new[] {new ArticleCreated(id, name)},
                           When = new DeliverArticle(id, 1),
                           Expect = new[] {new ArticleDelivered(id, 1)}
                       }.Run();
        }

        public ExecutionResult BuyLessThanExist()
        {
            return new DomainSpecification<BuyArticle, ArticleCommandsHandler>
                       {
                           Name = "Article: Buy Less Than Exist",
                           Given = new Event[]
                                       {
                                           new ArticleCreated(id, name),
                                           new ArticleDelivered(id, 100)
                                       },
                           When = new BuyArticle(id, 1),
                           Expect = new[] {new ArticleBought(id, 1)}
                       }.Run();
        }

        public ExecutionResult BuyMoreThanExist()
        {
            return new DomainSpecification<BuyArticle, ArticleCommandsHandler>
                       {
                           Name = "Article: Buy More Than Exist",
                           Given = new Event[]
                                       {
                                           new ArticleCreated(id, name),
                                           new ArticleDelivered(id, 10)
                                       },
                           When = new BuyArticle(id, 100),
                           IsExceptionExpected = true
                       }.Run();
        }

        public IEnumerable<ExecutionResult> ExecuteAll()
        {
            return new[]
                       {
                           Create(),
                           Deliver(),
                           BuyLessThanExist(),
                           BuyMoreThanExist()
                       };
        }
    }
}