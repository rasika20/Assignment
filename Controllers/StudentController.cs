using jQueryAjaxInAsp.NETMVC.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jQueryAjaxInAsp.NETMVC.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewAll()
        {
            List<Student> obj = new List<Student>();
            obj = GetAllStudent().ToList();
           ViewBag.data = JsonConvert.SerializeObject(obj);
            return View(obj);
        }

        IEnumerable<Student> GetAllStudent()
        {
            using (DBModel db = new DBModel())
            {
                return db.Students.ToList<Student>();
            }

        }

        public ActionResult AddOrEdit(int id = 0)
        {
            Student std = new Student();
            if (id != 0)
            {
                using (DBModel db = new DBModel())
                {
                    std = db.Students.Where(x => x.ID == id).FirstOrDefault<Student>();
                }
            }
            return View(std);
        }

        [HttpPost]
        public ActionResult AddOrEdit(Student std)
        {
            try
            {
              
                using (DBModel db = new DBModel())
                {
                    if (std.ID == 0)
                    {
                        db.Students.Add(std);
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Entry(std).State = EntityState.Modified;
                        db.SaveChanges();

                    }
                }
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllStudent()), message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
     
    public ActionResult Delete(int id)
        {
            try
            {
                using (DBModel db = new DBModel())
                {
                    Student std = db.Students.Where(x => x.ID == id).FirstOrDefault<Student>();
                    db.Students.Remove(std);
                    db.SaveChanges();
                }
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllStudent()), message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
   