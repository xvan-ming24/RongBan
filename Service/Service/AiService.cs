using System.Text;
using Newtonsoft.Json;
using Rongban.Models.Entities;

namespace PetApi.Services
{
    public class AiService
    {
        // 豆包API密钥（建议从配置文件读取，而非硬编码）
        private readonly string _apiKey = "55dc7be3-20fd-45b8-884a-2eb50a573a3b";

        /// <summary>
        /// 调用豆包API获取宠物诊断建议
        /// </summary>
        /// <param name="symptom">宠物症状描述（对应consults表的symptom字段）</param>
        /// <param name="pet">宠物信息实体（来自cat_info表）</param>
        /// <returns>AI诊断结果（对应consults表的ai_result字段）</returns>
        public async Task<string> GetDiagnosis(string symptom, PetInfo pet)
        {
            // 校验参数（确保宠物信息不为空）
            if (pet == null)
            {
                return "宠物信息不能为空";
            }

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

                // 1. 构造请求内容（适配cat_info表字段：物种、品种、年龄、性别等）
                var petInfo = new StringBuilder();
                petInfo.Append($"物种：{pet.Id}，");
                petInfo.Append($"品种：{pet.Breed ?? "未知"}，");
                petInfo.Append($"年龄：{pet.Age / 12}岁（{pet.Age}个月），"); // 转换为岁（原表单位为月）
                petInfo.Append($"性别：{GetGenderText(pet.Gender)}，");
                petInfo.Append($"是否绝育：{(pet.Sterilization == 1 ? "是" : "否")}");

                var requestBody = new
                {
                    model = "bot-20250731155745-ksmkw",
                    messages = new[]
                    {
                    new
                    {
                        role = "user",
                        content = $"我家宠物{petInfo}，症状：{symptom}。请作为兽医给出诊断建议，仅说中文，语言简洁。"
                    }
                }
                };

                // 2. 发送请求
                var content = new StringContent(
                    JsonConvert.SerializeObject(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );
                var response = await httpClient.PostAsync(
                    "https://ark.cn-beijing.volces.com/api/v3/bots/chat/completions",
                    content
                );
                var result = await response.Content.ReadAsStringAsync();

                // 3. 解析响应
                if (!response.IsSuccessStatusCode)
                {
                    return $"API调用失败（状态码：{response.StatusCode}）：{result}";
                }

                dynamic jsonResult;
                try
                {
                    jsonResult = JsonConvert.DeserializeObject(result);
                }
                catch (Exception ex)
                {
                    return $"结果解析失败：{ex.Message}，原始响应：{result}";
                }

                // 4. 提取诊断结果
                if (jsonResult?.choices == null || jsonResult.choices.Count == 0)
                {
                    return $"AI返回格式异常，原始响应：{result}";
                }

                return jsonResult.choices[0].message.content;
            }
        }

        /// <summary>
        /// 将性别数字转换为文本（适配cat_info表的gender字段：0-未知，1-公，2-母）
        /// </summary>
        private string GetGenderText(int? gender)
        {
            return gender switch
            {
                1 => "公",
                2 => "母",
                _ => "未知"
            };
        }
    }
}

