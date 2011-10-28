using System;
using System.Linq;
using Market.Cqrsnes.Domain;
using Market.Cqrsnes.Web.Service;

namespace Market.Cqrsnes.Web.Test
{
    public class ArticleViewModelManagerSpecifications
    {
        private readonly Guid id = Guid.NewGuid();

        private readonly string name = "New Article";

        public ExecutionResult ArticleCreated()
        {
            return new Specification<ArticleViewModelManager>()
                .When(new ArticleCreated(id, name))
                .Expect(x => x.GetArticleListViewModel().Articles.Count == 1)
                .Expect(x => x.GetArticleListViewModel().Articles.First().Id == id)
                .Expect(x => x.GetArticleListViewModel().Articles.First().Name == name)
                .Run();
        }
    }
}