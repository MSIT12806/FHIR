using FHIR.Tools;
using System.Text;
namespace FHIR.Practicce
{
    internal class Program
    {
        const bool PRACTICE1 = true;
        const bool PRACTICE2 = true;
        const bool PRACTICE3 = true;
        const bool PRACTICE4 = true;
        const bool PRACTICE5 = true;
        const bool PRACTICE6 = true;
        static async Task Main(string[] args)
        {
            if (PRACTICE1 == false)
            {
                var p = Homework.GetPatientData();
                Console.WriteLine(p);
            }

            if (PRACTICE2)
            {
                // 1. FHIR Server URL (您需要替換為實際的 FHIR Server URL)
                const string fhirServerUrl = "https://hapi.fhir.tw/fhir";

                // 2. 設定 Patient Resource 資料
                var patientJson = Homework.GetPatientJson();

                // 3. 使用 HttpClient 進行 POST 請求
                using (HttpClient client = new HttpClient())
                {
                    // 設置 Content-Type 標頭為 FHIR JSON 格式
                    client.DefaultRequestHeaders
                        .Accept
                        .Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/fhir+json"));

                    // 將 JSON 內容轉換為 StringContent
                    var content = new StringContent(patientJson, Encoding.UTF8, "application/fhir+json");

                    // 發送 POST 請求
                    HttpResponseMessage response = await client.PostAsync($"{fhirServerUrl}/{nameof(Hl7.Fhir.Model.Patient)}", content);

                    // 確認回應結果
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Patient Resource 已成功上傳！");
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("FHIR Server 回應: " + responseBody);
                    }
                    else
                    {
                        Console.WriteLine("上傳失敗。");
                        Console.WriteLine("Status Code: " + response.StatusCode);
                        string errorBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Error Message: " + errorBody);
                    }
                }
            }
        }
    }
}
