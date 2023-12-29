﻿
using Microsoft.AspNetCore.Hosting;
using Pipelights.Database.Models;
using Pipelights.Database.Services;
using Pipelights.Website.BusinessService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pipelights.Website.Services.Interfaces;

namespace Pipelights.Website.BusinessService
{
    public interface ICategoryService
    {
        CategoryEntity GetById(string id);
        bool AddAsync(CategoryEntity item);
        bool UpdateAsync(string id, CategoryEntity item);
        bool DeleteAsync(string id);
        IEnumerable<CategoryEntity> GetMultiple(string query);
        IEnumerable<CategoryEntity> GetMultiple(int max = 1000);
    }

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryDbService _categoryDbService;

        public CategoryService(ICategoryDbService categoryDbService)
        {
            _categoryDbService = categoryDbService;
        }

        public CategoryEntity GetById(string id)
        {
            return _categoryDbService.GetAsync(id).Result;
        }

        public bool AddAsync(CategoryEntity item)
        {

            try
            {
                var task = Task.Run(async () => await _categoryDbService.AddAsync(item));
                task.GetAwaiter().GetResult();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }


        }

        public bool UpdateAsync(string id, CategoryEntity item)
        {

            try
            {
                var task = Task.Run(async () => await _categoryDbService.UpdateAsync(item));
                task.GetAwaiter().GetResult();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool DeleteAsync(string id)
        {
            _categoryDbService.DeleteAsync(id);

            return true;
        }

        public IEnumerable<CategoryEntity> GetMultiple(string query)
        {

            var dbResult = _categoryDbService.GetMultipleAsync(query).Result;


            return dbResult;
        }

        public IEnumerable<CategoryEntity> GetMultiple(int max = 1000)
        {
            var dbResult = GetMultiple("SELECT * FROM c order by c._ts DESC");

            var result = dbResult.Take(max);

            return result;
        }
    }
}
