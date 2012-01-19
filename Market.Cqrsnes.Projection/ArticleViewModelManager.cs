using System;
using System.Linq;
using Cqrsnes.Infrastructure;
using Market.Cqrsnes.Domain;
using Market.Cqrsnes.Domain.Events;

namespace Market.Cqrsnes.Projection
{
    public class ArticleViewModelManager :
        IEventHandler<OfferCreated>,
        IEventHandler<ArticleDelivered>,
        IEventHandler<ArticleBought>
    {
        private readonly IRepository repository;
        private readonly Guid viewId = new Guid("fb1364addfbb48ea9f71130e0aaae5bc");

        public ArticleViewModelManager(IRepository repository)
        {
            this.repository = repository;
        }

        public ArticleListViewModel GetArticleListViewModel()
        {
            return repository.GetById<ArticleListViewModel>(viewId)
                   ?? new ArticleListViewModel {Id = viewId};
        }

        public void Handle(OfferCreated command)
        {
            var view = repository.GetById<ArticleListViewModel>(viewId)
                       ?? new ArticleListViewModel {Id = viewId};

            if (view.Articles.Any(x => x.Id == command.Id))
            {
                throw new InvalidOperationException("Possible concurrency problem. Article duplicate was found.");
            }

            view.Articles.Add(new ArticleViewModel
                                  {
                                      Id = command.Id,
                                      Name = command.Name,
                                      Count = 0
                                  });

            repository.Save(view);
        }

        public void Handle(ArticleDelivered command)
        {
            var view = repository.GetById<ArticleListViewModel>(viewId)
                       ?? new ArticleListViewModel {Id = viewId};

            view.Articles.First(x => x.Id == command.Id).Count += command.Count;

            repository.Save(view);
        }

        public void Handle(ArticleBought command)
        {
            var view = repository.GetById<ArticleListViewModel>(viewId)
                       ?? new ArticleListViewModel();

            var articleViewModel = view.Articles.First(x => x.Id == command.Id);

            if (articleViewModel.Count < command.Count)
            {
                throw new InvalidOperationException("Possible concurrency problem. Bought more than exist.");
            }

            articleViewModel.Count -= command.Count;

            repository.Save(view);
        }
    }
}