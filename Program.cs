
using MatrixMultiplication.Classes;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using MatrixMultiplication;
using System.Text;


// When creating a new matrix comment MainAsync().Wait(); and uncomment CreateAsync().Wait();
// When getting and computing comment CreateAsync().Wait(); and uncomment MainAsync().Wait();
public class Program
{
    public static void Main(string[] args)
    {
        //CreateAsync().Wait();
        MainAsync().Wait();
    }

    public async static Task<int> CreateAsync()
    {
        HttpClient httpClient = new HttpClient();
        var createMatrix = await httpClient.GetAsync("https://recruitment-test.investcloud.com/api/numbers/init/1000");
        return 0;
    }

    public async static Task<int> MainAsync()
    {
        int N = 1000;
        RowOrColumn result = new RowOrColumn();

        int nearestPowerOf2 = (int)GetPowerOf2(N);

        Matrix matrixC = await MultiplyMatrices(N, nearestPowerOf2);

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                sb.Append(matrixC[i, j].ToString());
            }
        }

        string md5 = CreateMD5(sb.ToString());
        HttpClient httpClient = new HttpClient();

        var httpContent = new StringContent(md5,Encoding.Default, "application/json");
        var res = await httpClient.PostAsync("https://recruitment-test.investcloud.com/api/numbers/validate", httpContent);
        if (res.IsSuccessStatusCode)
        {
            var resul = await res.Content.ReadAsStringAsync();
            dynamic ress = JsonConvert.DeserializeObject(resul);
            Console.WriteLine(ress.Value);
        }
        Console.ReadLine();

        return 0;
    }

    public static async Task<Matrix> MultiplyMatrices(int size, int nearestToPowerOf2)
    {
        Matrix matrixA = new Matrix(nearestToPowerOf2, nearestToPowerOf2);
        Matrix matrixB = new Matrix(nearestToPowerOf2, nearestToPowerOf2);

        await GetMatrixAsync(size, 'A', matrixA);
        await GetMatrixAsync(size, 'B', matrixB);

        return matrixA * matrixB;
    }

    public async static Task<int> GetMatrixAsync(int N, char c, Matrix matrixA)
    {
        RowOrColumn result = new RowOrColumn();
        HttpClient httpClient = new HttpClient();
        var getMatrix1 = await httpClient.GetAsync("https://recruitment-test.investcloud.com/api/numbers/" + c + "/row/" + 0);
        var customerJsonString1 = await getMatrix1.Content.ReadAsStringAsync();
        result = JsonConvert.DeserializeObject<RowOrColumn>(custome‌​rJsonString1);
        for (int i = 0; i < N; i++)
        {
            int val = result.Value[i];
            for (int j = 0; j <= i; j++)
            {
                matrixA.mat[i - j, j] = val;
                //A[i - j, j] = val;
            }
        }

        var getMatrix = await httpClient.GetAsync("https://recruitment-test.investcloud.com/api/numbers/" + c + "/row/" + (N - 1));
        var customerJsonString = await getMatrix.Content.ReadAsStringAsync();
        result = JsonConvert.DeserializeObject<RowOrColumn>(custome‌​rJsonString);

        for (int i = N - 1; i >= 0; i--)
        {
            int val = result.Value[i];
            int count = 0;
            for (int j = N - 1; j >= i; j--)
            {
                matrixA.mat[j, i + count] = val;
                count++;
            }
        }

        return 0;
    }

    public static double GetPowerOf2(int n)
    {
        int i = 0;
        for(i = 0; ; i++)
        {
            if(n <= Math.Pow(2, i))
            {
                break;
            }
        }

        return Math.Pow(2, i);
    }

    public static string CreateMD5(string input)
    {
        // Use input string to calculate MD5 hash
        using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
        {
            byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString());
            }
            return sb.ToString();
        }
    }



}

