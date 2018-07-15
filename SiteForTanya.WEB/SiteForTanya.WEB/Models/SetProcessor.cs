using SiteForTanya.WEB.Models.CommonViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using SiteForTanya.Models;
using SiteForTanya.DAL.EntityFramework;

namespace SiteForTanya.WEB.Models
{
    public class SetProcessor
    {
        public SetViewModel GetSetsNames(string keyWords, int setsCountOnPage, int pageNumber)
        {
            Repository<SetEntity> setEntityRepository = new Repository<SetEntity>();
            SetViewModel setViewModel = new SetViewModel();

            if (keyWords == null || keyWords == String.Empty)
            {
                var allSets = setEntityRepository.GetList();
                List<SetBriefInfo> sets = allSets.OrderByDescending(set => set.AddingTime).Skip((pageNumber - 1) * setsCountOnPage).
                    Take(setsCountOnPage).Select(set => new SetBriefInfo { setName = set.Name, mainImageName = set.MainImageName }).ToList();
                return new SetViewModel { sets = sets, setsCount = allSets.Count() };
            }
            else
            {
                List<string> words = keyWords.Trim().Split(' ').ToList();
                var allSets = setEntityRepository.GetList().Where(set => CommonMethods.TagsContainWords(set, words));
                List<SetBriefInfo> sets = allSets.OrderByDescending(set => set.AddingTime).Skip((pageNumber - 1) * setsCountOnPage).
                    Take(setsCountOnPage).Select(set => new SetBriefInfo { setName = set.Name, mainImageName = set.MainImageName }).ToList();
                return new SetViewModel { sets = sets, setsCount = allSets.Count() };
            }
        }

    }
}