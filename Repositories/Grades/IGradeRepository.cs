using DinnamuSWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DinnamuSWebApi.Repositories.Grades
{
    public interface IGradeRepository
    {
        List<Grade> Get();
        Grade Get(int chaveunica);
        void Insert(Grade grade);
        void Update(Grade grade);
        void Delete(int chaveunica);
    }
}