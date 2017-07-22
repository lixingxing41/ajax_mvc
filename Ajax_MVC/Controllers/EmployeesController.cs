using Ajax_MVC.Models;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace Ajax_MVC.Controllers
{
    public class EmployeesController : Controller
    {
        // GET: Employees
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData(int page, string sname, string stitle)
        {
            string jstring = "";
            using (NorthwindChineseEntities1 db = new NorthwindChineseEntities1())
            {
                var query = db.Employees.AsQueryable();

                if (!string.IsNullOrEmpty(sname))
                {
                    query = query.Where(x => x.EmployeeName.Contains(sname));
                }
                if (!string.IsNullOrEmpty(stitle))
                {
                    query = query.Where(x => x.Title.Contains(stitle));
                }

                query = query.OrderBy(x => x.EmployeeID).Skip((page - 1) * 5).Take(5);

                jstring = JsonConvert.SerializeObject(query.ToArray()); //序列化
            }

            return Json(jstring);
        }

        public ActionResult GetEmployeeData(int eid)
        {
            string jstring = "";
            using (NorthwindChineseEntities1 db = new NorthwindChineseEntities1())
            {
                var query = db.Employees.Where(x => x.EmployeeID == eid);

                jstring = JsonConvert.SerializeObject(query.ToArray());
            }
            return Json(jstring);
        }

        public ActionResult GetNum(int totalpage, string sname, string stitle)
        {
            using (NorthwindChineseEntities1 db = new NorthwindChineseEntities1())
            {
                var query = db.Employees.AsQueryable();

                if (!string.IsNullOrWhiteSpace(sname))
                {
                    query = query.Where(x => x.EmployeeName.Contains(sname));
                }
                if (!string.IsNullOrWhiteSpace(stitle))
                {
                    query = query.Where(x => x.Title.Contains(stitle));
                }

                totalpage = query.Count();
                if (totalpage % 5 == 0) totalpage = totalpage / 5;
                else totalpage = totalpage / 5 + 1;
            }

            return Json(totalpage);
        }

        public ActionResult Update(int eid, int did, string name, string title, string titleofc, string bdate, string hdate, string addr,
                                    string hphone, string exten, string ppath, string notes, string mid, string salary)
        {
            string obj = "";
            using (NorthwindChineseEntities1 db = new NorthwindChineseEntities1())
            {
                Employees e;

                //新增
                if (eid == 0 && did == 0)
                {
                    e = new Employees
                    {
                        EmployeeName = name,
                        Title = title,
                        TitleOfCourtesy = titleofc,
                        BirthDate = DateTime.Parse(bdate),
                        HireDate = DateTime.Parse(hdate),
                        Address = addr,
                        Extension = exten,
                        HomePhone = hphone,
                        Notes = notes,
                        PhotoPath = ppath,
                        ManagerID = Convert.ToInt32(mid),
                        Salary = Convert.ToInt32(salary)
                    };
                    db.Employees.Add(e);
                    obj = "上傳成功!";
                }

                //編輯
                if (eid != 0)
                {
                    e = db.Employees.Find(eid);
                    e.EmployeeName = name;
                    e.Title = title;
                    e.TitleOfCourtesy = titleofc;
                    e.BirthDate = DateTime.Parse(bdate);
                    e.HireDate = DateTime.Parse(hdate);
                    e.Address = addr;
                    e.Extension = exten;
                    e.HomePhone = hphone;
                    e.Notes = notes;
                    e.PhotoPath = ppath;
                    e.ManagerID = Convert.ToInt32(mid);
                    e.Salary = Convert.ToInt32(salary);
                    obj = "上傳成功!";
                }
                //刪除
                if (did != 0)
                {
                    e = db.Employees.Find(did);
                    db.Employees.Remove(e);
                    obj = "刪除成功";
                }

                db.SaveChanges();
            }

            return Json(obj);
        }

    }
}