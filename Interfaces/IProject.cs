using ProjectTracker.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Interfaces
{
    interface IProject
    {
        List<projectMaster> GetListofProjects();
        bool CheckProjectIdExists(string ProjectCode);
        bool CheckProjectNameExists(string ProjectName);
        int SaveProject(projectMaster ProjectMaster);
        IQueryable<projectMaster> ShowProjects(string sortColumn, string sortColumnDir, string Search);
        bool CheckProjectIDExistsInTimesheet(string ProjectID);
        int ProjectDelete(string ProjectID);
        int GetTotalProjectsCounts();
    }
}
