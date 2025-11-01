# Challenge by Celonis
Repository for the coding challenge for creating a currency converter with Frankfurter API using WPF/C# while employing MVVM, and later enhancing it with a query - caching mechanism

**Approach:**
1. The solution will first be realized into a core MVVM structure, from which we can have further enchancements, as and when needed.  
2. The layers will be as follows:  
   a. **Presentation Layer:** Consisting of the UI in WPF and the code-behind.  
   b. **Business Layer:** Consisting of the the interaction logic that would call the services.  
   c. **Service Layer:** Consisting of the service mechanisms which allow the client to convey requests to the API, and fetch responses.  

3. According to MVVM:
   a. **View:** MainWindow.xaml/MainWindow.xaml.cs - For defining the UI components and the respective code behind.
   b. **Models:** -->  
        **APIResponseModels** : LatestRateResponse, HistoricalRateResponse - For handling the Latest and the historical data   
        **CurrencyRate** - For handling currency rates being displayed in the Historical data  
        **CacheEntry** - For handling cache entries in the implemented caching mechanism.  
   c. **ViewModels:**  MainViewModel.cs - For propagating refreshing the UI for changes, by using service calls to the Frankfurter API.




