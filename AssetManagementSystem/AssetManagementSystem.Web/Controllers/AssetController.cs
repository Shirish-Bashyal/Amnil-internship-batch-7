using AssetManagementSystem.Shared;
using AssetManagementSystem.Shared.Dtos;
using AssetManagementSystem.Shared.Dtos.Asset;
using AssetManagementSystem.Shared.Dtos.Department;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;

namespace AssetManagementSystem.Web.Controllers;

public class AssetController : Controller
{
    private readonly HttpClient _httpClient;

    public AssetController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("AssetAPI");
    }


    //public async Task<IActionResult> Index()
    //{

    //    IEnumerable<AssetReadDto> assets = new List<AssetReadDto>();

    //    // Call the API
    //    var responseMessage = await _httpClient.GetAsync("Asset");

    //    if (responseMessage.IsSuccessStatusCode)
    //    {
    //        var jsonData = await responseMessage.Content.ReadAsStringAsync();
    //        Console.WriteLine(jsonData);
    //        var apiResponse = JsonConvert.DeserializeObject<ResponseData<IEnumerable<AssetReadDto>>>(jsonData);

    //        if (apiResponse != null && apiResponse.IsSuccess)
    //        {
    //            assets = apiResponse.Data;
    //        }
    //    }



    //    return View(assets);
    //}

    public async Task<IActionResult> Index(string? searchTerm, string? sortOrder, int pageNumber = 1, int pageSize = 10)
    {
        IEnumerable<AssetReadDto> assets = new List<AssetReadDto>();

        try
        {
            var skip = (pageNumber - 1) * pageSize;

            // Prepare query params
            var query = $"SearchTerm={searchTerm}&SortOrder={sortOrder}&SkipCount={skip}&MaxResultCount={pageSize}";

            var response = await _httpClient.GetAsync($"Asset/List?{query}");

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                // Deserialize the wrapped API response
                var apiResponse = JsonConvert.DeserializeObject<ResponseData<PagedResponseDto<AssetReadDto>>>(jsonData);
                var count = apiResponse.Data.TotalCount;

                if (apiResponse != null && apiResponse.IsSuccess && apiResponse.Data != null)
                {
                   
                    assets = apiResponse.Data.Items;
                    
                }

                ViewBag.Search = searchTerm;
                ViewBag.PageNumber = pageNumber;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = (int)Math.Ceiling((double)count / pageSize);
                ViewBag.SortOrder = sortOrder;
            }
            else
            {
                TempData["Message"] = "Failed to load assets from API.";
                TempData["MessageType"] = "error";
            }
        }
        catch (Exception ex)
        {
            TempData["Message"] = $"Error: {ex.Message}";
            TempData["MessageType"] = "error";
        }

        
        return View(assets);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        // Get all departments for dropdown
        HttpResponseMessage deptResponse = await _httpClient.GetAsync("Department"); // endpoint to get all departments
        List<SelectListItem> departments = new List<SelectListItem>();
        if (deptResponse.IsSuccessStatusCode)
        {
            string deptData = await deptResponse.Content.ReadAsStringAsync();
            var deptApiResponse = JsonConvert.DeserializeObject<ResponseData<IEnumerable<DepartmentReadDto>>>(deptData);
            if (deptApiResponse != null && deptApiResponse.IsSuccess)
            {
                departments = deptApiResponse.Data
                    .Select(d => new SelectListItem
                    {
                        Value = d.Id.ToString(),
                        Text = d.Name,

                    })
                    .ToList();
            }
        }

        ViewBag.Departments = departments; // pass to view
        return View();
    }

    [HttpPost]

    public async Task<IActionResult> Create(AssetDto model)
    {
        if (!ModelState.IsValid)
        {

            return View(model);
        }

        try
        {
            string data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage respone = await _httpClient.PostAsync("asset", content);

            if (respone.IsSuccessStatusCode)
            {

                TempData["Message"] = "asset created successfully!";
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
    public async Task<IActionResult> Delete(Guid id)

    {
        try
        {
            AssetReadDto department = new AssetReadDto();


            HttpResponseMessage respone = await _httpClient.GetAsync("Asset/" + id);


            if (respone.IsSuccessStatusCode)
            {
                string data = respone.Content.ReadAsStringAsync().Result;
                var apiResponse = JsonConvert.DeserializeObject<ResponseData<AssetReadDto>>(data);
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
    public async Task<IActionResult> DeleteConfirmed(Guid id)

    {
        try
        {

            HttpResponseMessage respone = await _httpClient.DeleteAsync("Asset/" + id);

            if (respone.IsSuccessStatusCode)
            {
                TempData["Message"] = "Asset deleted successfully!";
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
    public async Task<IActionResult> Edit(Guid id)

    {
        try
        {
            AssetReadDto asset = new AssetReadDto();


            HttpResponseMessage respone = await _httpClient.GetAsync("Asset/" + id);


            if (respone.IsSuccessStatusCode)
            {
                string data = respone.Content.ReadAsStringAsync().Result;
                var apiResponse = JsonConvert.DeserializeObject<ResponseData<AssetReadDto>>(data);
                if (apiResponse != null && apiResponse.IsSuccess)
                {
                    asset = apiResponse.Data;
                }

            }

            // Get all departments for dropdown
            HttpResponseMessage deptResponse = await _httpClient.GetAsync("Department"); // endpoint to get all departments
            List<SelectListItem> departments = new List<SelectListItem>();
            if (deptResponse.IsSuccessStatusCode)
            {
                string deptData = await deptResponse.Content.ReadAsStringAsync();
                var deptApiResponse = JsonConvert.DeserializeObject<ResponseData<IEnumerable<DepartmentReadDto>>>(deptData);
                if (deptApiResponse != null && deptApiResponse.IsSuccess)
                {
                    departments = deptApiResponse.Data
                        .Select(d => new SelectListItem
                        {
                            Value = d.Id.ToString(),
                            Text = d.Name,
                            Selected = d.Id == asset.DepartmentId
                        })
                        .ToList();
                }
            }

            ViewBag.Departments = departments; // pass to view


            return View(asset);
        }
        catch (Exception ex)
        {

            throw;
        }

    }


    [HttpPost]
    public async Task<IActionResult> Edit(AssetReadDto model)

    {
        try
        {
            var jsonData = new
            {

                AssetName = model.AssetName,
                SerialNumber = model.SerialNumber,
                AssetCategory = model.AssetCategory,
                ReceivedDate = model.ReceivedDate,
                DepartmentId = model.DepartmentId,
                TagId = model.TagId,
                Description = model.Description

            };

            Guid ID = model.Id;

            string data = JsonConvert.SerializeObject(jsonData);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage respone = await _httpClient.PutAsync("asset/" + ID, content);

            if (respone.IsSuccessStatusCode)
            {
                TempData["Message"] = "asset edited successfully!";
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



    [HttpPost]
    public async Task<IActionResult> ToggleActivation(Guid id, bool isActivated)
    {
        try
        {
            
            var content = new StringContent(JsonConvert.SerializeObject(isActivated), Encoding.UTF8, "application/json");
            var response = await _httpClient.PatchAsync($"asset/{id}/activation", content);

            
            var responseContent = await response.Content.ReadAsStringAsync();

           
            var apiResponse = JsonConvert.DeserializeObject<ResponseData<bool>>(responseContent);

           
            if (apiResponse != null)
            {
                TempData["Message"] = apiResponse.Message;
                TempData["MessageType"] = "success"; // You can keep it "error" or set based on IsSuccess
            }

        }
        catch (Exception ex)
        {
            TempData["Message"] = $"Error: {ex.Message}";
            TempData["MessageType"] = "error";
        }

        return RedirectToAction("Index");
    }
}