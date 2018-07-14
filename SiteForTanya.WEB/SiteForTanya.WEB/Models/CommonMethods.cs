using System;
using System.Collections.Generic;
using System.Linq;
using SiteForTanya.WEB.Models.CommonViewModels;
using SiteForTanya.Models;
using SiteForTanya.DAL.EntityFramework;

namespace SiteForTanya.WEB.Models
{
    public static class CommonMethods
    {
        public static List<AutocompleteViewModel> AutocompleteSearch<T>(string term) where T : class, IEntityInfo
        {
            Repository<T> entityInfoRepository = new Repository<T>();
            T entityInfo = entityInfoRepository.GetList().First();
            if (entityInfo.AllTags == String.Empty)
            {
                return null;
            }
            List<AutocompleteViewModel> words = entityInfo.AllTags.Split(',').Where(x => x.Contains(term.ToLower())).OrderBy(s => s).Select(a => new AutocompleteViewModel { value = a }).ToList();

            return words;
        }

        public static bool TagContainsWord<T>(T item, List<string> words) where T : IEntity
        {
            if (item.Tags == null)
            {
                return false;
            }
            foreach (string tag in item.Tags.Split(','))
            {
                foreach (string word in words)
                {
                    if (word == tag)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}