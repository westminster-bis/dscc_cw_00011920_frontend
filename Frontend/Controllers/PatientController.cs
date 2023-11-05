using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Frontend.Shared;
using Frontend.Model;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Frontend.Controllers
{
    public class PatientController : Controller
    {
        private string BaseUrl = new CommonConstants().BaseUrl;
        private async Task<List<Doctor>> GetDoctorList()
        {
            List<Doctor> doctorList = new List<Doctor>();

            using (var apiClient = new HttpClient())
            {
                apiClient.BaseAddress = new Uri(BaseUrl);
                apiClient.DefaultRequestHeaders.Clear();
                apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage doctorResponse = await apiClient.GetAsync("api/doctors");

                if (doctorResponse.IsSuccessStatusCode)
                {
                    var doctorData = doctorResponse.Content.ReadAsStringAsync().Result;

                    doctorList = JsonConvert.DeserializeObject<List<Doctor>>(doctorData);
                }
            }
            return doctorList;
        }
        private async Task<Patient> GetPatientDetail(int id)
        {
            Patient patient = new Patient();

            using (var apiClient = new HttpClient())
            {
                apiClient.BaseAddress = new Uri(BaseUrl);
                apiClient.DefaultRequestHeaders.Clear();
                apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage patientResponse = await apiClient.GetAsync($"api/patient/{id}");

                if (patientResponse.IsSuccessStatusCode)
                {
                    var jsonPatientResult = patientResponse.Content.ReadAsStringAsync().Result;

                    patient = JsonConvert.DeserializeObject<Patient>(jsonPatientResult);
                }
            }

            return patient;
        }
        // GET: PatientController
        public async Task< ActionResult> Index()
        {
            List<Patient> patientList = new List<Patient>();

            using (var apiClient = new HttpClient())
            {
                apiClient.BaseAddress = new Uri(BaseUrl);
                apiClient.DefaultRequestHeaders.Clear();
                apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage patientResponse = await apiClient.GetAsync("api/patient");

                if (patientResponse.IsSuccessStatusCode)
                {
                    var patientData = patientResponse.Content.ReadAsStringAsync().Result;

                    patientList = JsonConvert.DeserializeObject<List<Patient>>(patientData);
                }
            }
            return View(patientList);
        }

        // GET: PatientController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var patientDetail = await GetPatientDetail(id);
            return View(patientDetail);
        }

        // GET: PatientController/Create
        public async Task< ActionResult> Create()
        {
            var doctorsList = await GetDoctorList();
            ViewBag.doctorsList = doctorsList;
            return View();
        }

        // POST: PatientController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< ActionResult> Create(IFormCollection collection)
        {
            Patient patient = new Patient();

            try
            {
                using (var apiClient = new HttpClient())
                {
                    apiClient.BaseAddress = new Uri(BaseUrl);
                    apiClient.DefaultRequestHeaders.Clear();
                    apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                    patient.FullName = collection["FullName"];
                    patient.PatientIllness = collection["PatientIllness"];
                    patient.Age = Convert.ToInt32(collection["Age"]);
                    patient.AssignedDoctorId = Convert.ToInt32(collection["AssignedDoctorId"]);
                    HttpResponseMessage doctorResponse = await apiClient.PostAsJsonAsync("api/patient", patient);

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
                ViewData["Error"] = "Error while creating a patient";
                return View();
            }
        }

        // GET: PatientController/Edit/5
        public async Task< ActionResult> Edit(int id)
        {
            var patientDetail = await GetPatientDetail(id);
            var doctorsList = await GetDoctorList();
            ViewBag.doctorsList = doctorsList;
            return View(patientDetail);
        }

        // POST: PatientController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< ActionResult> Edit(int id, IFormCollection collection)
        {
            var patient = new Patient();
            try
            {
                using (var apiClient = new HttpClient())
                {
                    apiClient.BaseAddress = new Uri(BaseUrl);
                    apiClient.DefaultRequestHeaders.Clear();
                    apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    patient.Id = id;
                    patient.FullName = collection["FullName"];
                    patient.PatientIllness = collection["PatientIllness"];
                    patient.Age = Convert.ToInt32(collection["Age"]);
                    patient.AssignedDoctorId = Convert.ToInt32(collection["AssignedDoctorId"]);
                    HttpResponseMessage patientResponse = await apiClient.PutAsJsonAsync($"api/patient/{id}", patient);

                    if (patientResponse.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return View(patient);
                    }
                }
            }
            catch
            {
                return View(patient);
            }
        }

        // GET: PatientController/Delete/5
        public async Task< ActionResult> Delete(int id)
        {
            var patientDetail = await GetPatientDetail(id);
            return View(patientDetail);
        }

        // POST: PatientController/Delete/5
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


                    HttpResponseMessage patientResponse = await apiClient.DeleteAsync($"api/patient/{id}");

                    if (patientResponse.IsSuccessStatusCode)
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
