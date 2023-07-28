using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;

using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1_upStock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string BaseUrl = "https://api.upstox.com/";
        private const string AccessTokenUrl = "https://api.upstox.com/index/dialog/authorize";
        private const string RedirectUrl = "https://www.creativemindze.com/";
        private const string ApiKey = "b9e50d22-7758-4b3a-a8bc-bfe733d5bac8";
        private const string ApiSecret = "6y9o1c1dz1";
        private const string Symbol = "NSE:NIFTY50";
        private RestClient _client;
        private const string code = "AUTHORIZATION_CODE"; // This code should be obtained from the redirect URL after user authentication

        public MainWindow()
        {
            InitializeComponent();
            _client = new RestClient("https://api.upstox.com/index/dialog/authorize");
            MainTask();



        }


        static async Task MainTask()
        {
           
            // Create an HttpClient instance
            using (var httpClient = new HttpClient())
            {
                // Define the API endpoint URL
                string tokenEndpoint = "https://api.upstox.com/index/oauth/token";

                // Create the request body with the necessary parameters
                var requestBody = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("client_id", ApiKey),
                new KeyValuePair<string, string>("client_secret", ApiSecret),
                new KeyValuePair<string, string>("redirect_uri", RedirectUrl),
                new KeyValuePair<string, string>("code", code),
            });

                try
                {
                    // Send the POST request to the API
                    var response = await httpClient.PostAsync(tokenEndpoint, requestBody);

                    // Read the response content
                    string responseContent = await response.Content.ReadAsStringAsync();

                    // Parse the response to get the access token
                    // In a real application, you should use a JSON parsing library
                    string accessToken = responseContent.Split(':')[1].Split(',')[0].Trim('"');

                    // Use the access token in your subsequent API requests
                    Console.WriteLine($"Access Token: {accessToken}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        private void OnLoginButtonClick(object sender, RoutedEventArgs e)
        {
            // Step 1: Construct the Authorization URL
            string authorizationUrl = ConstructAuthorizationUrl();

            // Step 2: Open the Authorization URL in the WebBrowser
            webBrowser.Visibility = Visibility.Visible;
            webBrowser.Navigate(new Uri(authorizationUrl));
        }

        private void OnNavigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            // Step 3: Capture the Authorization Code
            string currentUrl = webBrowser.Source?.AbsoluteUri;

            // Check if the current URL contains the redirect URI
            if (currentUrl.Contains(RedirectUrl))
            {
                // Extract the authorization code from the query parameter
                string queryString = currentUrl.Substring(currentUrl.IndexOf('?') + 1);
                var queryParams = HttpUtility.ParseQueryString(queryString);
                string authorizationCode = queryParams["code"];

                // Hide the WebBrowser after capturing the authorization code
                webBrowser.Visibility = Visibility.Collapsed;

                // Step 4: Exchange the Authorization Code for the Access Token
                string accessToken = GetAccessToken(authorizationCode);

                // Step 5: Display the Access Token
                txtAccessToken.Text = accessToken;
            }
        }
        private string ConstructAuthorizationUrl()
        {
            // Construct the Authorization URL
            var queryParams = new System.Collections.Specialized.NameValueCollection
            {
                { "response_type", "code" },
                { "client_id", ApiKey },
                { "redirect_uri", RedirectUrl }
            };

            string authorizationUrl = "https://api.upstox.com/login/authorization/dialog?" + ToQueryString(queryParams);
            return authorizationUrl;
        }

        private string GetAccessToken(string authorizationCode)
        {
            // Construct the request to exchange the authorization code for the access token
            var request = new RestRequest(Method.Post.ToString());

            // Add the required parameters for obtaining the access token
            request.AddParameter("code", authorizationCode);
            request.AddParameter("client_id", ApiKey);
            request.AddParameter("client_secret", ApiSecret);
            request.AddParameter("redirect_uri", RedirectUrl);
            request.AddParameter("grant_type", "authorization_code");

            // Execute the request
            RestResponse response = _client.Execute(request);
            var content = response.Content;

            // Parse the response and extract the access token
            // The response should contain the access token as a JSON property named "access_token".
            // You'll need to implement the logic to parse the response here based on Upstox's API response format.
            string accessToken =""; // Extract the access token from the response

            return accessToken;
        }

        // Helper method to convert a collection of key-value pairs to a query string
        private string ToQueryString(System.Collections.Specialized.NameValueCollection nvc)
        {
            return string.Join("&", Array.ConvertAll(nvc.AllKeys, key => $"{key}={Uri.EscapeDataString(nvc[key])}"));
        }
    

        private void FetchHistoricalData_Click(object sender, RoutedEventArgs e)
        {
            string accessToken = GetAccessToken(" ");
            var historicalData = FetchHistoricalData(accessToken);
            DisplayHistoricalData(historicalData);
        }

       
        private void MakeAuthenticatedApiRequest()
        {
            // Create a new RestSharp client and request object
            var client = new RestClient("https://api.upstox.com/some/endpoint");
            var request = new RestRequest(Method.Get.ToString());

            // Add the access token to the request headers
            string accessToken = "YOUR_ACCESS_TOKEN"; // Replace with the actual access token
            request.AddHeader("Authorization", "Bearer " + accessToken);

            // Add any other required parameters to the request (if needed)
            // request.AddParameter("param_name", "param_value");

            // Execute the request and handle the response
            RestResponse response = client.Execute(request);
            var content = response.Content;

            // Process the API response as needed
        }


        private List<HistoricalData> FetchHistoricalData(string accessToken)
    {
        var client = GetAuthenticatedClient(accessToken);
        var request = new RestRequest("index/historical/nifty_50", Method.Get);
        request.AddParameter("symbol", Symbol);
        request.AddParameter("format", "json");
        request.AddParameter("from", "2023-01-01");
        request.AddParameter("to", "2023-01-10");

        RestResponse response = client.Execute(request);
        var content = response.Content;

        // Parse the historical data from the response (in JSON format)
        List<HistoricalData> historicalData = JsonConvert.DeserializeObject<List<HistoricalData>>(content);

        return historicalData;
    }

        private RestClient GetAuthenticatedClient(string accessToken)
        {
            var client = new RestClient(BaseUrl);
            client.AddDefaultHeader("x-api-key", ApiKey);
            client.AddDefaultHeader("Authorization", $"Bearer {accessToken}");
            return client;
        }


        private void DisplayHistoricalData(List<HistoricalData> data)
        {
            // Implement the logic to display historical data in the WPF application (e.g., update the text box or data grid).
            // For simplicity, let's assume you have a TextBox named "txtHistoricalData" in the XAML file.
            // You can set its Text property to display the historical data.
        }


    }
       
}

