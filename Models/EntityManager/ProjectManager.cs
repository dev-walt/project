using ProjectTracker.Interfaces;
using ProjectTracker.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Dynamic.Core;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;

namespace ProjectTracker.Models.EntityManager
{
    public class ProjectManager : IProject
    {
        public bool CheckProjectIdExists(string ProjectCode)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    var result = (from project in _context.ProjectMaster
                                  where project.projectId == ProjectCode
                                  select project).Count();

                    if (result > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckProjectNameExists(string ProjectName)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    var result = (from project in _context.ProjectMaster
                                  where project.projectName == ProjectName
                                  select project).Count();

                    if (result > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<projectMaster> GetListofProjects()
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    var listofProjects = (from project in _context.ProjectMaster
                                          select project).ToList();
                    return listofProjects;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int SaveProject(projectMaster ProjectMaster)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    _context.ProjectMaster.Add(ProjectMaster);
                    return _context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<projectMaster> ShowProjects(string sortColumn, string sortColumnDir, string Search)
        {
            var _context = new DatabaseContext();

            var IQueryableproject = (from projectmaster in _context.ProjectMaster
                                     select projectmaster);

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                IQueryableproject = IQueryableproject.OrderBy(sortColumn + " " + sortColumnDir);
            }
            if (!string.IsNullOrEmpty(Search))
            {
                IQueryableproject = IQueryableproject.Where(m => m.projectName == Search || m.projectId == Search);
            }

            return IQueryableproject;

        }

        public bool CheckProjectIDExistsInTimesheet(string ProjectID)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    var result = (from timesheet in _context.TimeSheetMaster
                                  where timesheet.projectId == ProjectID
                                  select timesheet).Count();

                    if (result > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

       
        public int ProjectDelete(string ProjectID)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    var project = (from projectDel in _context.ProjectMaster
                                   where projectDel.projectId == ProjectID
                                   select projectDel).SingleOrDefault(); ;

                    if (project != null)
                    {
                        _context.ProjectMaster.Remove(project);
                        int resultProject = _context.SaveChanges();
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetTotalProjectsCounts()
        {
            DatabaseContext db = new DatabaseContext();
            //var Count = db.UserRoleMapping.Count(a => a.roleId == db.RoleMaster.FirstOrDefault(b => b.roleName == "Admin").roleId);
            var Count = db.ProjectMaster.Count();
            if (Count > 0)
                {
                    return Count;
                }
                else
                {
                    return 0;
                }
            
        }
    }
}