using AssismentStudent.MVC.Models;
using AssismentStudent.MVC.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
namespace AssismentStudent.MVC.Controllers;


public class StudentController : Controller
{
   

    private readonly HttpClient _client;

    public StudentController(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient("StudentAPI");
    }


    //public IActionResult Index()
    //{
    //    List<StudentViewModel> students = new List<StudentViewModel>();
    //    HttpResponseMessage respone = _client.GetAsync("student/Get").Result;



    //    if (respone.IsSuccessStatusCode)
    //    {
    //        string data = respone.Content.ReadAsStringAsync().Result;
    //        var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<StudentViewModel>>>(data);
    //        if (apiResponse != null && apiResponse.Success)
    //        {
    //            students = apiResponse.Data;
    //        }

    //    }
    //    return View(students);


    //}


    [HttpGet]
    public IActionResult Index(int pageNumber = 1, int pageSize = 5)
    {
        PaginationResponse<StudentViewModel> paginatedResponse = new PaginationResponse<StudentViewModel>();

        HttpResponseMessage response = _client.GetAsync($"student/GetPaginated?pageNumber={pageNumber}&pageSize={pageSize}").Result;

        if (response.IsSuccessStatusCode)
        {
            string data = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(data);
            paginatedResponse = JsonConvert.DeserializeObject<PaginationResponse<StudentViewModel>>(data);
        }

        return View(paginatedResponse);
    }


    //[HttpGet]
    //public IActionResult Search(string name = "", string address = "")
    //{
    //    List<StudentViewModel> students = new List<StudentViewModel>();
    //    HttpResponseMessage respone = _client.GetAsync($"student/Get?name={Uri.EscapeDataString(name)}&address={Uri.EscapeDataString(address)}").Result;



    //    if (respone.IsSuccessStatusCode)
    //    {
    //        string data = respone.Content.ReadAsStringAsync().Result;
    //        var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<StudentViewModel>>>(data);
    //        if (apiResponse != null && apiResponse.Success)
    //        {
    //            students = apiResponse.Data;
    //        }

    //    }
    //    return View("Index", students);
    //}

    [HttpGet]
    public IActionResult Search(string name = "", string address = "")
    {
        PaginationResponse<StudentViewModel> paginatedResponse = new PaginationResponse<StudentViewModel>();

        HttpResponseMessage response = _client.GetAsync($"student/Get?name={Uri.EscapeDataString(name)}&address={Uri.EscapeDataString(address)}").Result;

        if (response.IsSuccessStatusCode)
        {
            string data = response.Content.ReadAsStringAsync().Result;
            var apiResponse = JsonConvert.DeserializeObject<PaginationResponse<StudentViewModel>>(data);

            if (apiResponse != null && apiResponse.Success)
            {
                paginatedResponse = apiResponse;
            }
        }

        ViewBag.SearchName = name;
        ViewBag.SearchAddress = address;
        return View("Index", paginatedResponse);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(CreateStudentViewModel model)
    {
        if (!ModelState.IsValid)
        {
           
            return View(model);
        }

        try
        {
            

            string data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage respone = _client.PostAsync("student/Create",content).Result; 

            if (respone.IsSuccessStatusCode)
            {

                TempData["Message"] = "Student created successfully!";
                TempData["MessageType"] = "success";
                return RedirectToAction("Index");

            }
        }
        catch (Exception ex)
        {
            TempData["Message"] = "Error: " + ex.Message;
            TempData["MessageType"] = "error";
            throw;
        }

        return View();
    }


    [HttpGet]
    public IActionResult Delete(Guid id)

    {
        try
        {
            StudentViewModel student = new StudentViewModel();


            HttpResponseMessage respone = _client.GetAsync("student/GetStudentById/" +id).Result;
           

            if (respone.IsSuccessStatusCode)
            {
                string data = respone.Content.ReadAsStringAsync().Result;
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse<StudentViewModel>>(data);
                if (apiResponse != null && apiResponse.Success)
                {
                    student = apiResponse.Data;
                }

            }
            return View(student);
        }
        catch (Exception ex)
        {
            TempData["errorMessage"] = ex.Message;
            throw;
        }

    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)

    {
        try
        {

            HttpResponseMessage respone = _client.DeleteAsync( "student/Delete/" + id).Result;

            if (respone.IsSuccessStatusCode)
            {
                TempData["Message"] = "Student deleted successfully!";
                TempData["MessageType"] = "success";
                return RedirectToAction("Index");

            }
            return View();
        }
        catch (Exception ex)
        {
            TempData["Message"] = "Error: " + ex.Message;
            TempData["MessageType"] = "error";
            throw;
        }
    }

        [HttpGet]
        public IActionResult Edit(int id)

        {
            try
            {
                StudentViewModel student = new StudentViewModel();

           
            HttpResponseMessage respone = _client.GetAsync("student/GetStudentById/" + id).Result;


                if (respone.IsSuccessStatusCode)
                {
                    string data = respone.Content.ReadAsStringAsync().Result;
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<StudentViewModel>>(data);
                    if (apiResponse != null && apiResponse.Success)
                    {
                        student = apiResponse.Data;
                    }

                }
                return View(student);
            }
            catch (Exception ex)
            {
                
                throw;
            }

        }


    [HttpPost]
    public IActionResult Edit(StudentViewModel model)

    {
        try
        {
            var jsonData = new
            {
                
             Name = model.Name,
             Email = model.Email,
             Address = model.Address,
             Gender = model.Gender

             };

            var Id = model.Id;
           string data = JsonConvert.SerializeObject(jsonData);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            
            HttpResponseMessage respone = _client.PutAsync("student/Update/" + Id, content).Result;

            if (respone.IsSuccessStatusCode)
            {
                TempData["Message"] = "Student edited successfully!";
                TempData["MessageType"] = "success";
                return RedirectToAction("Index");

            }
            return View();
        }
        catch (Exception ex)
        {
            TempData["Message"] = "Error: " + ex.Message;
            TempData["MessageType"] = "error";
            throw;
        }

    }

    [HttpGet]
    public async Task<IActionResult> DownloadReport()
    {
        try
        {
            
            var response = await _client.GetAsync("student/GeneratePdf");

            if (!response.IsSuccessStatusCode)
            {
                var errorText = await response.Content.ReadAsStringAsync();
                Console.WriteLine(errorText);
                return RedirectToAction("Index");
            }

            var pdfBytes = await response.Content.ReadAsByteArrayAsync();
            return File(pdfBytes, "application/pdf", "StudentReport.pdf");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return RedirectToAction("Index");
        }
    }



}





