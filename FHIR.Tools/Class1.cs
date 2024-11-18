using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Hl7.Fhir.Validation;

namespace FHIR.Tools
{

    public static class Homework
    {
        public static Patient GetPatientData()
        {
            //        以下是某位病患的資料：

            //-病人英文姓名為「Connor Graham」，中譯姓名為「葛康納」
            //-這是他的照片：https://i.imgur.com/VeTQheO.png
            //        -護照號碼：65848725
            //        - 聯絡電話：(宅)07 - 2159685(公) 07 - 7938888(手機) 0912 - 354879
            //        - 聯絡地址：高雄市橋頭區經武路 58 號 24 樓之 11
            //        - 戶籍地址同聯絡地址
            //- 緊急聯絡人姓名：James Yates   緊急聯絡人電話：(手機)0988 - 878545 緊急聯絡人關係：父
            //- 病人慣用溝通之語言為英文（en - US）

            Patient patient = new Patient();

            //-病人英文姓名為「Connor Graham」，中譯姓名為「葛康納」
            patient.Name = new List<HumanName>
            {
                new HumanName
                {
                    Use = HumanName.NameUse.Official,
                    Family = "Graham",
                    Given = new string[] { "Connor" }
                },
                new HumanName
                {
                    Use = HumanName.NameUse.Usual,
                    Text = "葛康納"
                }
            };

            //        -護照號碼：65848725
            patient.Identifier = new List<Identifier>
            {
                new Identifier
                {
                    Use = Identifier.IdentifierUse.Official,
                    System = "http://terminology.hl7.org/CodeSystem/v2-0203", //衛服部編碼?
                    Value = "65848725",
                }
            };

            //        - 聯絡電話：(宅)07 - 2159685(公) 07 - 7938888(手機) 0912 - 354879
            patient.Telecom = new List<ContactPoint>
            {
                new ContactPoint
                {
                    System = ContactPoint.ContactPointSystem.Phone,
                    Value = "07-2159685",
                    Use = ContactPoint.ContactPointUse.Home
                },
                new ContactPoint
                {
                    System = ContactPoint.ContactPointSystem.Phone,
                    Value = "07-7938888",
                    Use = ContactPoint.ContactPointUse.Work
                },
                new ContactPoint
                {
                    System = ContactPoint.ContactPointSystem.Phone,
                    Value = "0912-354879",
                    Use = ContactPoint.ContactPointUse.Mobile
                }
            };

            //        - 聯絡地址：高雄市橋頭區經武路 58 號 24 樓之 11
            patient.Address = new List<Address>
            {
                new Address
                {
                    Use = Address.AddressUse.Home,
                    Line = new string[] { "經武路 58 號 24 樓之 11" },
                    City = "高雄市",
                    District = "橋頭區" //district 可能是美國的"州"，所以應該不會是橋頭區
                }
            };

            //- 緊急聯絡人姓名：James Yates   緊急聯絡人電話：(手機)0988 - 878545 緊急聯絡人關係：父
            patient.Contact = new List<Patient.ContactComponent>
            {
                new Patient.ContactComponent
                {
                    Relationship = new List<CodeableConcept>
                    {
                        new CodeableConcept
                        {
                            Text = "父"
                        }
                    },
                    Name = new HumanName
                    {
                        Text = "James Yates"
                    },
                    Telecom = new List<ContactPoint>
                    {
                        new ContactPoint
                        {
                            System = ContactPoint.ContactPointSystem.Phone,
                            Value = "0988-878545",
                            Use = ContactPoint.ContactPointUse.Mobile
                        }
                    }
                }
            };

            //- 病人慣用溝通之語言為英文（en - US）
            patient.Communication = new List<Patient.CommunicationComponent>
            {
                new Patient.CommunicationComponent
                {
                    Language = new CodeableConcept
                    {
                        Coding = new List<Coding>
                        {
                            new Coding
                            {
                                System = "urn:ietf:bcp:47",
                                Code = "en-US"
                            }
                        }
                    }
                }
            };


            // 驗證 Patient 物件是否符合 FHIR 規範
            patient.Validate(recurse: true, narrativeValidation: NarrativeValidationKind.FhirXhtml);
            return patient;
        }

        public static string GetPatientJson()
        {
            Patient patient = GetPatientData();

            // 創建 FHIR JSON 序列化器
            var serializer = new FhirJsonSerializer();
            // 將 Patient 物件序列化為 JSON 格式
            string json = serializer.SerializeToString(patient);
            return json;
        }

        public static string GetPatientXml()
        {
            Patient patient = GetPatientData();
            // 創建 FHIR XML 序列化器
            var serializer = new FhirXmlSerializer();
            // 將 Patient 物件序列化為 XML 格式
            string xml = serializer.SerializeToString(patient);
            return xml;
        }
    }
}
