using Frontend.Model;
using Frontend.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Numerics;

namespace Frontend.Controllers
{
    public class DoctorController : Controller
    {
        private string BaseUrl = new CommonConstants().BaseUrl;
        private async Task<Doctor> GetDoctorDetail (int id)
        {
            Doctor doctor = new Doctor();

            using (var apiClient = new HttpClient())
            {
                apiClient.BaseAddress = new Uri(BaseUrl);
                apiClient.DefaultRequestHeaders.Clear();
                apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage doctorResponse = await apiClient.GetAsync($"api/doctors/{id}");

                if (doctorResponse.IsSuccessStatusCode)
                {
                    var jsonDoctorResult = doctorResponse.Content.ReadAsStringAsync().Result;

                    doctor = JsonConvert.DeserializeObject<Doctor>(jsonDoctorResult);
                }
            }

            return doctor;
        }
        // GET: DoctorContoller
        public async Task<ActionResult> Index()
        {
            List<Doctor> doctorList = new List<Doctor>();

            using (var apiClient = new HttpClient() )
            {
                apiClient.BaseAddress = new Uri(BaseUrl);
                apiClient.DefaultRequestHeaders.Clear();
                apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage doctorResponse = await apiClient.GetAsync("api/doctors");

                if(doctorResponse.IsSuccessStatusCode)
                {
                    var doctorData = doctorResponse.Content.ReadAsStringAsync().Result;

                    doctorList = JsonConvert.DeserializeObject<List<Doctor>>(doctorData);
                }
            }
            return View(doctorList);
        }

        // GET: DoctorController/Details/5
        public async Task< ActionResult> Details(int id)
        {
            var doctorRecord = await GetDoctorDetail(id);
            return View(doctorRecord);
        }

        // GET: DoctorController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DoctorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            Doctor doctor = new Doctor();
          
            try
            {
                using (var apiClient = new HttpClient())
                {
                    apiClient.BaseAddress = new Uri(BaseUrl);
                    apiClient.DefaultRequestHeaders.Clear();
                    apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    

                    doctor.FirstName = collection["FirstName"];
                    doctor.LastName = collection["LastName"];
                    doctor.Department = collection["Department"];
                    doctor.WorkingHours = Convert.ToDecimal(collection["WorkingHours"]);
                    HttpResponseMessage doctorResponse = await apiClient.PostAsJsonAsync("api/doctors", doctor);

                    if (doctorResponse.IsSuccessStatusCode)
                    {
                       return RedirectToAction(nameof(Index));
                    } else
                    {
                        return View();
                    }
                }
               
            }
            catch
            {
                ViewData["Error"] = "Error while creating a doctor";
                return View();
            }
        }

        // GET: DoctorController/Edit/5
        public async Task< ActionResult> Edit(int id)
        {
            var doctor = await GetDoctorDetail(id);
            return View(doctor);
        }

        // POST: DoctorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, IFormCollection collection)
        {
            var doctor = new Doctor();
            try
            {
                using (var apiClient = new HttpClient())
                {
                    apiClient.BaseAddress = new Uri(BaseUrl);
                    apiClient.DefaultRequestHeaders.Clear();
                    apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    doctor.Id = id;
                    doctor.FirstName = collection["FirstName"];
                    doctor.LastName = collection["LastName"];
                    doctor.Department = collection["Department"];
                    doctor.WorkingHours = Convert.ToDecimal(collection["WorkingHours"]);
                    HttpResponseMessage doctorResponse = await apiClient.PutAsJsonAsync($"api/doctors/{id}", doctor);

                    if (doctorResponse.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return View();
                    }
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: DoctorController/Delete/5
        public async Task< ActionResult> Delete(int id)
        {
            var doctorRecord = await GetDoctorDetail(id);
            return View(doctorRecord);
        }

        // POST: DoctorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {

                using (var apiClient = new HttpClient())
                {
                    apiClient.BaseAddress = new Uri(BaseUrl);
                    apiClient.DefaultRequestHeaders.Clear();
                    apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    
                    HttpResponseMessage doctorResponse = await apiClient.DeleteAsync($"api/doctors/{id}");

                    if (doctorResponse.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return View();
                    }
                }
            }
            catch
            {
                return View();
            }
        }
    }
}
