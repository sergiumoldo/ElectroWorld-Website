
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
    public interface ITestimonialService
    {
        TestimonialEntity GetById(string id);
        bool AddAsync(TestimonialEntity item);
        bool UpdateAsync(string id, TestimonialEntity item);
        bool DeleteAsync(string id);
        IEnumerable<TestimonialEntity> GetMultiple(string query);
        IEnumerable<TestimonialEntity> GetMultiple(int max = 1000);
    }

    public class TestimonialService : ITestimonialService
    {
        private readonly ITestimonialDbService _testimonialDbService;

        public TestimonialService(ITestimonialDbService testimonialDbService)
        {
            _testimonialDbService = testimonialDbService;
        }

        public TestimonialEntity GetById(string id)
        {
            return _testimonialDbService.GetAsync(id).Result;
        }

        public bool AddAsync(TestimonialEntity item)
        {

            try
            {
                var task = Task.Run(async () => await _testimonialDbService.AddAsync(item));
                task.GetAwaiter().GetResult();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }


        }

        public bool UpdateAsync(string id, TestimonialEntity item)
        {

            try
            {
                var task = Task.Run(async () => await _testimonialDbService.UpdateAsync(item));
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
            _testimonialDbService.DeleteAsync(id);

            return true;
        }

        public IEnumerable<TestimonialEntity> GetMultiple(string query)
        {

            var dbResult = _testimonialDbService.GetMultipleAsync(query).Result;


            return dbResult;
        }

        public IEnumerable<TestimonialEntity> GetMultiple(int max = 1000)
        {
            var dbResult = GetMultiple("SELECT * FROM c order by c._ts DESC");

            var result = dbResult.Take(max);

            return result;
        }
    }
}
