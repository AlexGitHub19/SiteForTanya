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
        public static List<AutocompleteViewModel> AutocompleteSearch<T>(string term) where T : class, IEntity
        {
            if (term == null || term == "" || LineContainsOnlySpaces(term))
            {
                return null;
            }
            bool endsWithSpace = term.EndsWith(" ");          
            List<string> tags = term.Trim().Split(' ').ToList();
            List<AutocompleteViewModel> words = new List<AutocompleteViewModel>();
            if (tags.Count == 1 && !endsWithSpace)
            {
                words = getResultWordsIfOneWord<T>(tags[0]).OrderBy(w => w).Select(a => new AutocompleteViewModel { value = a }).ToList();
            }
            else if (endsWithSpace)
            {
                   words = getResultWordsIfEndsWithSpace<T>(tags).OrderBy(w=>w).Select(a => new AutocompleteViewModel { value = a }).ToList();
            }
            else
            {
                words = getResultWordsIfMoreThanOneWords<T>(tags).OrderBy(w => w).Select(a => new AutocompleteViewModel { value = a }).ToList();
            }
           

            return words;
        }

        private static List<string> getResultWordsIfOneWord<T>(string word) where T: class, IEntity
        {
            Repository<T> entityRepository = new Repository<T>();
            IEnumerable<T> allEntities = entityRepository.GetList();
            List<string> resultList = new List<string>();
            foreach (T entity in allEntities)
            {
                if (entity.Tags != null)
                {
                    foreach (string tag in entity.Tags.Split(';'))
                    {
                        if (tag.StartsWith(word.Trim().ToLower()))
                        {
                            if (!resultList.Contains(tag))
                            {
                                resultList.Add(tag);
                            }                           
                        }
                    }
                }               
            }
            return resultList;
        }

        private static List<string> getResultWordsIfMoreThanOneWords<T>(List<string> words) where T : class, IEntity
        {
            Repository<T> entityRepository = new Repository<T>();
            IEnumerable<T> allEntities = entityRepository.GetList();
            List<string> resultList = new List<string>();
            foreach (T entity in allEntities)
            {
                List<string> wordsWithoutLast = words.Take(words.Count - 1).ToList();
                if (TagsContainWords(entity, wordsWithoutLast))
                {
                    if (entity.Tags != null)
                    {
                        foreach (string tag in entity.Tags.Split(';'))
                        {
                            if (!words.Any(w => w.ToLower() == tag) && tag.StartsWith(words.Last().Trim()))
                            {
                                string result = "";
                                foreach (string w in wordsWithoutLast)
                                {
                                    result += w.Trim().ToLower() + " ";
                                }
                                result += tag;
                                if (!resultList.Contains(result))
                                {
                                    resultList.Add(result);
                                }                              
                            }
                        }
                    }                    
                }              
            }

            return resultList;
        }

        private static List<string> getResultWordsIfEndsWithSpace<T>(List<string> words) where T : class, IEntity
        {
            Repository<T> entityRepository = new Repository<T>();
            IEnumerable<T> allEntities = entityRepository.GetList();
            List<string> resultList = new List<string>();
            foreach (T entity in allEntities)
            {
                if (TagsContainWords(entity, words))
                {
                    if (entity.Tags != null)
                    {
                        foreach (string tag in entity.Tags.Split(';'))
                        {
                            if (!words.Any(w=>w.ToLower() == tag))
                            {
                                string result = "";
                                foreach (string w in words)
                                {
                                    result += w.Trim().ToLower() + " ";
                                }
                                result += tag;
                                if (!resultList.Contains(result))
                                {
                                    resultList.Add(result);
                                }
                            }
                        }
                    }
                }
            }

            return resultList;
        }

        private static bool LineContainsOnlySpaces(string line)
        {
            foreach (char c in line.ToCharArray())
            {
                if (c != ' ')
                {
                    return false;
                }
            }
            return true;
        }

        public static bool TagsContainWords<T>(T item, List<string> words) where T : IEntity
        {
            if (item.Tags != null)
            {
                int containsCount = 0;
                foreach (string word in words)
                {
                    foreach (string tag in item.Tags.Split(';'))
                    {
                        if (word.Trim().ToLower() == tag)
                        {
                            containsCount++;
                            break;
                        }
                    }
                }

                if (containsCount == words.Count)
                {
                    return true;
                }
            }
                       
            return false;
        }
    }
}