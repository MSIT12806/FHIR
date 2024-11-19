using FHIR.Tools;
using Firely.Fhir.Packages;
using Firely.Fhir.Validation;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Hl7.Fhir.Specification;
using Hl7.Fhir.Specification.Source;
using Hl7.Fhir.Specification.Terminology;
using JsonDiffPatchDotNet;
using Newtonsoft.Json.Linq;
using System.Text;
namespace FHIR.Practicce
{
    internal class Program
    {
        const bool PRACTICE1 = true;
        const bool PRACTICE2 = true;
        const bool PRACTICE2_5 = true;
        const bool PRACTICE3 = true;
        const bool PRACTICE4 = true;
        const bool PRACTICE5 = true;
        const bool PRACTICE6 = true;
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            if (PRACTICE1)
            {
                var p = Homework.GetPatientData();

                const string answer = @"
{
  ""resourceType"": ""Patient"",
  ""identifier"": [
    {
      ""use"": ""official"",
      ""type"": {
        ""coding"": [
          {
            ""system"": ""http://terminology.hl7.org/CodeSystem/v2-0203"",
            ""code"": ""PPN"",
            ""display"": ""Passport Number""
          }
        ]
      },
      ""value"": ""65848725""
    }
  ],
  ""name"": [
    {
      ""use"": ""official"",
      ""text"": ""Connor Graham"",
      ""family"": ""Graham"",
      ""given"": [
        ""Connor""
      ]
    },
    {
      ""use"": ""official"",
      ""text"": ""葛康納""
    }
  ],
  ""telecom"": [
    {
      ""system"": ""phone"",
      ""value"": ""07-2159685"",
      ""use"": ""home""
    },
    {
      ""system"": ""phone"",
      ""value"": ""07-7938888"",
      ""use"": ""work""
    },
    {
      ""system"": ""phone"",
      ""value"": ""0912-354879"",
      ""use"": ""mobile""
    }
  ],
  ""gender"": ""male"",
  ""address"": [
    {
      ""use"": ""home"",
      ""type"": ""both"",
      ""text"": ""高雄市橋頭區經武路58號24樓之11"",
      ""line"": [
        ""經武路58號24樓之11""
      ],
      ""city"": ""高雄市"",
      ""district"": ""橋頭區"",
      ""postalCode"": ""825006"",
      ""country"": ""TW""
    }
  ],
  ""contact"": [
    {
      ""relationship"": [
        {
          ""coding"": [
            {
              ""system"": ""http://terminology.hl7.org/CodeSystem/v2-0131"",
              ""code"": ""C"",
              ""display"": ""Emergency Contact""
            }
          ],
          ""text"": ""父""
        }
      ],
      ""name"": {
        ""use"": ""official"",
        ""text"": ""James Yates""
      },
      ""telecom"": [
        {
          ""system"": ""phone"",
          ""value"": ""0988-878545"",
          ""use"": ""mobile""
        }
      ]
    }
  ],
  ""photo"": [
    {
      ""url"": ""https://i.imgur.com/VeTQheO.png""
    }
  ],
  ""communication"": [
    {
      ""language"": {
        ""coding"": [
          {
            ""system"": ""urn:ietf:bcp:47"",
            ""code"": ""en-US""
          }
        ],
        ""text"": ""English (US)""
      }
    }
  ]
}";

                // 使用 jsondiffpatch.net 套件比對答案與您的 Patient 資料差異
                var jdp = new JsonDiffPatch();
                var jTokenAnswer = JToken.Parse(answer);
                var jTokenPatient = JToken.Parse(Homework.GetPatientJson());
                var patch = jdp.Diff(jTokenPatient, jTokenAnswer);
                var diff = jdp.Patch(jTokenPatient, patch);

                Console.WriteLine(Homework.GetPatientJson());

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
                        //解析 FHIR OperationOutcome 資料
                        var outcome = new FhirJsonParser().Parse<OperationOutcome>(errorBody);
                        foreach (var issue in outcome.Issue)
                        {
                            if (issue.Severity == OperationOutcome.IssueSeverity.Error)
                            {
                                Console.WriteLine($"Severity: {issue.Severity}");
                                Console.WriteLine($"Code: {issue.Code}");
                                Console.WriteLine($"Diagnostics: {issue.Diagnostics}");
                                // 如果有 `details` 的額外編碼，則列出
                                if (issue.Details != null && issue.Details.Coding != null)
                                {
                                    foreach (var coding in issue.Details.Coding)
                                    {
                                        Console.WriteLine($"Details Code System: {coding.System}, Code: {coding.Code}");
                                    }
                                }

                                // 顯示相關位置
                                if (issue.Location != null)
                                {
                                    Console.WriteLine("Location(s): " + string.Join(", ", issue.Location));
                                }

                                Console.WriteLine("------");
                            }
                        }
                    }
                }
            }

            if (PRACTICE2_5 == false)
            {
                // 1. FHIR Server URL (您需要替換為實際的 FHIR Server URL)
                const string profileUrl = "https://hapi.fhir.tw/fhir/StructureDefinition/TWCorePatient\"";

                // 2. 設定 Patient Resource 資料
                var patient = Homework.GetPatientData();

                var packageServer = "https://hapi.fhir.tw";
                var fhirRelease = FhirRelease.R4;
                var packageResolver = FhirPackageSource.CreateCorePackageSource(ModelInfo.ModelInspector, fhirRelease, packageServer);
                var resourceResolver = new CachedResolver(packageResolver);
                var terminologyService = new LocalTerminologyService(resourceResolver);

                var validator = new Validator(resourceResolver, terminologyService);
                var profile = "https://hapi.fhir.tw/fhir/StructureDefinition/TWCorePatient";
                var result = validator.Validate(patient, profile);


                // 確認回應結果
                if (result.Success)
                {
                    Console.WriteLine("Patient Resource 已成功上傳！");
                }
                else
                {
                    Console.WriteLine("上傳失敗。");
                    foreach (var issue in result.Issue)
                    {
                        if (issue.Severity == OperationOutcome.IssueSeverity.Error)
                        {
                            Console.WriteLine($"Severity: {issue.Severity}");
                            Console.WriteLine($"Code: {issue.Code}");
                            Console.WriteLine($"Diagnostics: {issue.Diagnostics}");
                            // 如果有 `details` 的額外編碼，則列出
                            if (issue.Details != null && issue.Details.Coding != null)
                            {
                                foreach (var coding in issue.Details.Coding)
                                {
                                    Console.WriteLine($"Details Code System: {coding.System}, Code: {coding.Code}");
                                }
                            }

                            // 顯示相關位置
                            if (issue.Location != null)
                            {
                                Console.WriteLine("Location(s): " + string.Join(", ", issue.Location));
                            }

                            Console.WriteLine("------");
                        }
                    }
                }
            }
        }
    }
}
