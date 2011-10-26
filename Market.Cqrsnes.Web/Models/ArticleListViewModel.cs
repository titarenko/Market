using System;
using System.Collections.Generic;

namespace Market.Cqrsnes.Web.Models
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