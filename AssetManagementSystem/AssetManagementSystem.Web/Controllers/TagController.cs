using AssetManagementSystem.Shared.Dtos;
using AssetManagementSystem.Shared.Dtos.Asset;
using AssetManagementSystem.Shared.Dtos.Tag;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;

namespace AssetManagementSystem.Web.Controllers;

public class TagController : Controller
{
    private readonly HttpClient _httpClient;

    public TagController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("AssetAPI");
    }


    public async Task<IActionResult> Index()
    {

        IEnumerable<TagReadDto> tags = new List<TagReadDto>();
        IEnumerable<AssetReadDto> assets = new List<AssetReadDto>();


        // Call the API
        var responseMessage = await _httpClient.GetAsync("Tag");

        if (responseMessage.IsSuccessStatusCode)
        {
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            Console.WriteLine(jsonData);
            var apiResponse = JsonConvert.DeserializeObject<ResponseData<IEnumerable<TagReadDto>>>(jsonData);

            if (apiResponse != null && apiResponse.IsSuccess)
            {
                tags = apiResponse.Data;
            }
        }

       


        return View(tags);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var assetsResponse = await _httpClient.GetAsync("Asset");
        List<SelectListItem> assetList = new List<SelectListItem>();

        if (assetsResponse.IsSuccessStatusCode)
        {
            var jsonData = await assetsResponse.Content.ReadAsStringAsync();
            var assetApiResponse = JsonConvert.DeserializeObject<ResponseData<IEnumerable<AssetReadDto>>>(jsonData);

            if (assetApiResponse != null && assetApiResponse.IsSuccess)
            {
                assetList = assetApiResponse.Data.Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.AssetName
                }).ToList();
            }
        }

        ViewBag.AssetList = assetList;

        return View();
    }

    [HttpPost]

    public async Task<IActionResult> Create(TagDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
           


            string data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage respone = await _httpClient.PostAsync("tag", content);

            if (respone.IsSuccessStatusCode)
            {

                TempData["Message"] = "tag created successfully!";
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
            TagReadDto tag = new TagReadDto();


            HttpResponseMessage respone = _httpClient.GetAsync("tag/" + id).Result;


            if (respone.IsSuccessStatusCode)
            {
                string data = respone.Content.ReadAsStringAsync().Result;
                var apiResponse = JsonConvert.DeserializeObject<ResponseData<TagReadDto>>(data);
                if (apiResponse != null && apiResponse.IsSuccess)
                {
                    tag = apiResponse.Data;
                }

            }
            return View(tag);
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

            HttpResponseMessage respone = _httpClient.DeleteAsync("tag/" + id).Result;

            if (respone.IsSuccessStatusCode)
            {
                TempData["Message"] = "Tag deleted successfully!";
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
            TagReadDto tag = new TagReadDto();


            HttpResponseMessage respone = await _httpClient.GetAsync("Tag/" + id);


            if (respone.IsSuccessStatusCode)
            {
                string data = respone.Content.ReadAsStringAsync().Result;
                var apiResponse = JsonConvert.DeserializeObject<ResponseData<TagReadDto>>(data);
                if (apiResponse != null && apiResponse.IsSuccess)
                {
                    tag = apiResponse.Data;
                }

            }

            // Get all assets for the dropdown
            var assetsResponse = await _httpClient.GetAsync("Asset");
            List<SelectListItem> assetList = new List<SelectListItem>();

            if (assetsResponse.IsSuccessStatusCode)
            {
                var jsonData = await assetsResponse.Content.ReadAsStringAsync();
                var assetApiResponse = JsonConvert.DeserializeObject<ResponseData<IEnumerable<AssetReadDto>>>(jsonData);

                if (assetApiResponse != null && assetApiResponse.IsSuccess)
                {
                    assetList = assetApiResponse.Data.Select(a => new SelectListItem
                    {
                        Value = a.Id.ToString(),
                        Text = a.AssetName
                    }).ToList();
                }
            }

            ViewBag.AssetList = assetList;

            return View(tag);
        }
        catch (Exception ex)
        {

            throw;
        }

    }


    [HttpPost]
    public IActionResult Edit(TagDto model)

    {
        try
        {
            var jsonData = new
            {

                MacAddress = model.MacAddress,   
                AssetId = model.AssetId,         
                Description = model.Description

            };

            Guid ID = model.TagId;

            string data = JsonConvert.SerializeObject(jsonData);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage respone = _httpClient.PutAsync("tag/" + ID, content).Result;

            if (respone.IsSuccessStatusCode)
            {
                TempData["Message"] = "Tag edited successfully!";
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

