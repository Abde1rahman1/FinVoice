using FinVoice.Contracts.AiExpense;
using Microsoft.EntityFrameworkCore;
using FinVoice.Database;
using FinVoice.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace FinVoice.Services.ExpenseService;

public class VoiceAnalysisService : IVoiceAnalysisService
{
    private readonly HttpClient _http;
    private readonly ApplicationDbContext _context;
    private readonly string _huggingFaceKey = "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";
    private readonly string _openRouterKey = "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";

    public VoiceAnalysisService(HttpClient http, ApplicationDbContext context)
    {
        _http = http;
        _context = context;
    }

    public async Task<List<VoiceAnalysisResponse>> GetAllVoiceAnalysis(string userId)
    {
        var data = await _context.Expenses
                          .Where(x => x.UserId == userId)
                          .Select(x => new VoiceAnalysisResponse(
                              x.Id,
                              x.UserId,
                              x.Amount,
                              x.Category,
                              x.Date,
                              x.Note
                              ))
                          .ToListAsync();

        return data;
    }


    public async Task<List<Expense>> AnalyzeAudioAsync(IFormFile audioFile, string userId)
    {
        var transcribedText = await TranscribeWithWhisperAsync(audioFile);
        var aiResult = await ExtractExpenseFromTextAsync(transcribedText!);

        var expenses = new List<Expense>();

        foreach (var item in aiResult.Items)
        {
            var expense = new Expense
            {
                UserId = userId,
                Amount = item.TotalPrice,
                Category = item.Product,
                Date = DateTime.UtcNow,
                Note = null,
                SourceText = aiResult.TranscribedText,

            };

            _context.Expenses.Add(expense);
            expenses.Add(expense);
        }
        await _context.SaveChangesAsync();


        return expenses;
    }


    private async Task<string?> TranscribeWithWhisperAsync(IFormFile audioFile)
    {
        var apiUrl = "https://router.huggingface.co/hf-inference/models/openai/whisper-large-v3";

        using var stream = audioFile.OpenReadStream();
        using var content = new StreamContent(stream);
        content.Headers.ContentType = new MediaTypeHeaderValue("audio/wav");

        _http.DefaultRequestHeaders.Clear();
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _huggingFaceKey);

        var response = await _http.PostAsync(apiUrl, content);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception("Whisper failed: " + responseContent);

        try
        {
            var json = JsonConvert.DeserializeObject<dynamic>(responseContent);
            return json?.text?.ToString(); 
        }
        catch (Exception ex)
        {
            throw new Exception("Whisper response parse error: " + ex.Message);
        }
    }

    private async Task<ExpenseAIResult> ExtractExpenseFromTextAsync(string text)
    {
        var prompt = $@"
            You are an AI that extracts structured data from Arabic spoken expense sentences.

            Given this sentence: ""{text}""

            Extract a list of purchased items. For each item, return:
            - product name (translated to English)
            - quantity (number, default is 1)
            - unit price (as explicitly stated in the sentence)
            - total price (unit price * quantity)

            VERY IMPORTANT RULES:
            - NEVER guess the price. Extract exactly what is said, even if written in Arabic words (e.g. 'متين' means 200).
            - ALWAYS translate product names to English.
            - If quantity is not mentioned, assume it's 1.
            - Your response MUST be a clean JSON list with no extra text before or after.
            - Ensure all numeric values are in digits.

            Expected format:
            [
              {{
                ""product"": ""Pizza"",
                ""quantity"": 1,
                ""unitPrice"": 10,
                ""totalPrice"": 10
              }},
              {{
                ""product"": ""Shirt"",
                ""quantity"": 1,
                ""unitPrice"": 100,
                ""totalPrice"": 100
              }}
            ]
            ";
        var body = new
        {
            model = "gpt-3.5-turbo",
            messages = new[]
            {
                    new { role = "system", content = "You're an AI that extracts product details from Arabic spoken expenses and returns structured English output." },
                    new { role = "user", content = prompt }
                }
        };

        _http.DefaultRequestHeaders.Clear();
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _openRouterKey);

        var response = await _http.PostAsync(
            "https://openrouter.ai/api/v1/chat/completions",
            new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json")
        );

        var responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception("OpenRouter GPT error: " + responseString);

        try
        {
            var resultJson = JsonConvert.DeserializeObject<dynamic>(responseString);
            var gptText = resultJson?.choices?[0]?.message?.content?.ToString();

            var startIndex = gptText?.IndexOf('[');
            var endIndex = gptText?.LastIndexOf(']');

            if (startIndex == -1 || endIndex == -1 || endIndex <= startIndex)
                throw new Exception("Failed to extract JSON array from GPT response.");

            var jsonText = gptText?.Substring(startIndex, endIndex - startIndex + 1).Trim();

            var res = new ExpenseAIResult
            {
                TranscribedText = text,
                Items = JsonConvert.DeserializeObject<List<ExpenseItem>>(jsonText)
            };
            return res ;
        }
        catch (Exception ex)
        {
            throw new Exception("GPT response parse error: " + ex.Message);
        }
    }
}

