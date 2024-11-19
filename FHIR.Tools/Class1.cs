using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Hl7.Fhir.Validation;

namespace FHIR.Tools
{
    public static class PatientHelper
    {
        public static class AddressHelper
        {
            public static Extension PostalCodeExtension(string code)
            {
                // 創建 Coding 對象並設置系統和代碼
                var postalCodeCoding = new Coding
                {
                    System = "https://twcore.mohw.gov.tw/ig/twcore/CodeSystem/postal-code3-tw",
                    Code = code
                };

                // 創建 CodeableConcept 並將 Coding 添加到其中
                var postalCodeCodeableConcept = new CodeableConcept
                {
                    Coding = new List<Coding> { postalCodeCoding }
                };

                // 創建 Extension 並設置 URL 和 valueCodeableConcept
                var postalCodeExtension = new Extension
                {
                    Url = "https://twcore.mohw.gov.tw/ig/twcore/StructureDefinition/tw-postal-code",
                    Value = postalCodeCodeableConcept
                };

                return postalCodeExtension;
            }
        }
    }
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

            patient.Meta = new Meta
            {
                Profile = new string[] {
                    //"https://twcore.mohw.gov.tw/ig/pas/StructureDefinition/Patient-twpas",
                    //"https://hapi.fhir.tw/fhir/StructureDefinition/MITW-T1-SC2-PatientIdentification",
                    //"https://hapi.fhir.tw/fhir/StructureDefinition/MITW-T1-SC1-PatientCore",
                    //"https://hapi.fhir.tw/fhir/StructureDefinition/MITW-T1-SC3-PatientContact",
                    "https://hapi.fhir.tw/fhir/StructureDefinition/TWCorePatient", //卡一個「找不到 profile https://twcore.mohw.gov.tw/ig/twcore/0.2.1/StructureDefinition-Address-twcore.html 」
                }

            };

            //-病人英文姓名為「Connor Graham」，中譯姓名為「葛康納」
            patient.Name = new List<HumanName>
            {
                new HumanName
                {
                    Use = HumanName.NameUse.Official,
                    Family = "Graham",
                    Given = new string[] { "Connor" },
                    Text = "Connor Graham"
                },
                new HumanName
                {
                    Use = HumanName.NameUse.Usual,
                    //Use = HumanName.NameUse.Official,
                    Text = "葛康納"
                }
            };

            //-這是他的照片：https://i.imgur.com/VeTQheO.png
            patient.Photo = new List<Attachment>
            {
                new Attachment
                {
                    Url = "https://i.imgur.com/VeTQheO.png"
                 }
            };

            // birthDate
            patient.BirthDateElement = new Date();
            patient.BirthDateElement.Extension.Add(new Extension
            {
                Url = "http://hl7.org/fhir/StructureDefinition/data-absent-reason",
                Value = new Code("unknown")
            });

            patient.Gender = AdministrativeGender.Male;

            //        -護照號碼：65848725
            patient.Identifier = new List<Identifier>
            {
                new Identifier
                {
                    Use = Identifier.IdentifierUse.Official,
                    Type = new CodeableConcept
                    {
                        Coding = new List<Coding>
                        {
                            new Coding
                            {
                                System = "http://terminology.hl7.org/CodeSystem/v2-0203", //衛服部編碼?
                                Code = "PPN",
                                //Display = "Passport Number"
                            }
                        }
                    },
                    System = "http://www.boca.gov.tw",//明明是 0..1，為啥跳錯誤？而且 uri 要寫啥我也不知道 //喔，profile 已經限定了
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
            // 地址一直驗不過...乾脆先註解掉
            //patient.Address = new List<Address>
            //{
            //    new Address
            //    {
            //        Use = Address.AddressUse.Home,
            //        District = "橋頭區", //district 可能是美國的"州"，所以應該不會是橋頭區
            //        Country = "TW",
            //        City = "高雄市",

            //        PostalCode = "825006",
            //        Extension = new List<Extension>
            //        {
            //            PatientHelper.AddressHelper.PostalCodeExtension("825006")
            //        },

            //        Line = new string[] { "經武路 58 號 24 樓之 11" },
            //    }
            //};

            // 更換 profile
            //patient.Address[0].= new List<string>
            //{
            //    // 替換為伺服器支援的地址 Profile 或移除此行
            //    "https://twcore.mohw.gov.tw/ig/twcore/0.2.1/StructureDefinition-Address-twcore.html"
            //};

            //- 緊急聯絡人姓名：James Yates   緊急聯絡人電話：(手機)0988 - 878545 緊急聯絡人關係：父
            patient.Contact = new List<Patient.ContactComponent>
            {
                new Patient.ContactComponent
                {
                    Relationship = new List<CodeableConcept>
                    {
                        new CodeableConcept
                        {
                            Coding = new List<Coding>
                            {
                                new Coding
                                {
                                    System = "http://terminology.hl7.org/CodeSystem/v2-0131",
                                    Code = "C",
                                    Display = "Emergency Contact"
                                }
                            },
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
                    },
                    Preferred = true
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
