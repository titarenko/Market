using System;
using System.Collections.Generic;

namespace Market.Cqrsnes.Projection
{
    public class ArticleListViewModel
    {
        public Guid Id { get; set; }

        public IList<ArticleViewModel> Articles { get; set; }

        public ArticleListViewModel()
        {
            Articles = Articles ?? new List<ArticleViewModel>();
        }
    }
}