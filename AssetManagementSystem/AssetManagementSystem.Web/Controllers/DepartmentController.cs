using AssetManagementSystem.Shared.Dtos;
using AssetManagementSystem.Shared.Dtos.Department;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;
using static AssetManagementSystem.Shared.Constrains.EntityConstrains;

namespace AssetManagementSystem.Web.Controllers;

public class DepartmentController : Controller
{
    private readonly HttpClient _httpClient;

    public DepartmentController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("AssetAPI");
    }


    public async Task<IActionResult> Index()
    {

        IEnumerable<DepartmentReadDto> departments = new List<DepartmentReadDto>();

        // Call the API
        var responseMessage = await _httpClient.GetAsync("Department");

        if (responseMessage.IsSuccessStatusCode)
        {
            var jsonData = await responseMessage.Content.ReadAsStringAsync();

            var apiResponse = JsonConvert.DeserializeObject<ResponseData<IEnumerable<DepartmentReadDto>>>(jsonData);

            if (apiResponse != null && apiResponse.IsSuccess)
            {
                departments = apiResponse.Data;
            }
        }

        return View(departments);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]

    public async Task<IActionResult> Create( DepartmentDto model)
    {
        if (!ModelState.IsValid)
        {

            return View(model);
        }

        try
        {
            string data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage respone = await _httpClient.PostAsync("department", content);

            if (respone.IsSuccessStatusCode)
            {

                TempData["Message"] = "department created successfully!";
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
            DepartmentReadDto department = new DepartmentReadDto();


            HttpResponseMessage respone = _httpClient.GetAsync("Department/" + id).Result;


            if (respone.IsSuccessStatusCode)
            {
                string data = respone.Content.ReadAsStringAsync().Result;
                var apiResponse = JsonConvert.DeserializeObject<ResponseData<DepartmentReadDto>>(data);
                if (apiResponse != null && apiResponse.IsSuccess)
                {
                    department = apiResponse.Data;
                }

            }
            return View(department);
        }
        catch (Exception ex)
        {
            TempData["errorMessage"] = ex.Message;
            throw;
        }

    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(Guid id)

    {
        try
        {

            HttpResponseMessage respone = _httpClient.DeleteAsync("Department/" + id).Result;

            if (respone.IsSuccessStatusCode)
            {
                TempData["Message"] = "department deleted successfully!";
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
    public IActionResult Edit(Guid id)

    {
        try
        {
            DepartmentReadDto department = new DepartmentReadDto();


            HttpResponseMessage respone = _httpClient.GetAsync("department/" + id).Result;


            if (respone.IsSuccessStatusCode)
            {
                string data = respone.Content.ReadAsStringAsync().Result;
                var apiResponse = JsonConvert.DeserializeObject<ResponseData<DepartmentReadDto>>(data);
                if (apiResponse != null && apiResponse.IsSuccess)
                {
                    department = apiResponse.Data;
                }

            }
            return View(department);
        }
        catch (Exception ex)
        {

            throw;
        }

    }


    [HttpPost]
    public IActionResult Edit(DepartmentDto model)

    {
        try
        {
            var jsonData = new
            {

                Name = model.Name,
                Description = model.Description

            };

            Guid Id = model.Id;

            string data = JsonConvert.SerializeObject(jsonData);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage respone = _httpClient.PutAsync("department/" + Id, content).Result;

            if (respone.IsSuccessStatusCode)
            {
                TempData["Message"] = "department edited successfully!";
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
}

