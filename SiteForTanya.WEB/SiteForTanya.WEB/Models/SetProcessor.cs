using SiteForTanya.WEB.Models.CommonViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using SiteForTanya.Models;

namespace SiteForTanya.WEB.Models
{
    public class SetProcessor
    {
        public SetViewModel GetSetsNames(string keyWords, int setsCountOnPage, int pageNumber)
        {
            Repository<SetEntity> setEntityRepository = new Repository<SetEntity>();
            SetViewModel setViewModel = new SetViewModel();

            if (keyWords == String.Empty)
            {
                var allSets = setEntityRepository.GetList();
                List<SetBriefInfo> sets = allSets.OrderByDescending(set => set.AddingTime).Skip((pageNumber - 1) * setsCountOnPage).
                    Take(setsCountOnPage).Select(set => new SetBriefInfo { setName = set.Name, mainImageName = set.MainImageName }).ToList();
                return new SetViewModel { sets = sets, setsCount = allSets.Count() };
            }
            else
            {
                List<string> words = keyWords.Split(' ').ToList();
                var allSets = setEntityRepository.GetList().Where(set => CommonMethods.TagContainsWord(set, words));
                List<SetBriefInfo> sets = allSets.OrderByDescending(set => set.AddingTime).Skip((pageNumber - 1) * setsCountOnPage).
                    Take(setsCountOnPage).Select(set => new SetBriefInfo { setName = set.Name, mainImageName = set.MainImageName }).ToList();
                return new SetViewModel { sets = sets, setsCount = allSets.Count() };
            }
        }

    }
}