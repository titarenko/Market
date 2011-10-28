using System;
using System.Linq;
using Machine.Specifications;
using Market.Cqrsnes.Domain;
using Market.Cqrsnes.Web.Service;

namespace Market.Cqrsnes.Web.Test
{
    public class CreatingArticle
    {
        private static Guid id;
        private static string name;

        private static ArticleViewModelManager manager;
        
        private Establish context = () =>
                                        {
                                            id = Guid.NewGuid();
                                            name = "New Article";

                                            manager = new ArticleViewModelManager(
                                                new SingleItemTestRepository());
                                        };

        private Because of = () => manager.Handle(new ArticleCreated(id, name));

        private It mustHasSingleArticleWithCertainId = () => manager
                                                                 .GetArticleListViewModel()
                                                                 .Articles.Single()
                                                                 .Id.ShouldEqual(id);

        private It mustHasSingleArticleWithCertainName = () => manager
                                                                   .GetArticleListViewModel()
                                                                   .Articles.Single()
                                                                   .Name.ShouldEqual(name);
    }
}