# load-json-data-with-load-more-functionality-in-.net-maui-datagrid
This article explains how to load JSON data incrementally with the built-in Load More functionality of the .NET MAUI DataGrid (SfDataGrid). The sample binds the grid to a view model that exposes an observable collection, a loading state, and an ICommand that retrieves the next page of records when the user reaches the end of the view. Use this README with the provided MainPage.xaml and MainPage.xaml.cs to understand the end-to-end flow and adapt it for your own API or local data scenarios. Refer to the official documentation for additional details: [Load More](https://help.syncfusion.com/maui/datagrid/load-more).

## xaml
It disables auto-generated columns to define a predictable schema and wires up the properties that drive Load More behavior. The ItemsSource is bound to the view model’s Records, IsBusy drives the progress indicator, and LoadMoreCommand is executed by the grid when more rows are requested.
```
<ContentPage.BindingContext>
    <local:ViewModel/>
</ContentPage.BindingContext>

<ContentPage.Content>
    <syncfusion:SfDataGrid x:Name="dataGrid" 
                            AutoGenerateColumnsMode="None"
                            ColumnWidthMode="FitByHeader"
                            ItemsSource="{Binding Records}"
                            AllowLoadMore="True"
                            IsBusy="{Binding IsBusy}"
                            LoadMoreCommand="{Binding LoadMoreRecordsCommand}">
        <syncfusion:SfDataGrid.Columns>
            <syncfusion:DataGridNumericColumn  HeaderText="ID"
                                                Format="D"
                                MappingName="[OrderID]" />
            <syncfusion:DataGridTextColumn  HeaderText="Customer ID"
                                MappingName="[CustomerID]" />
            <syncfusion:DataGridTextColumn  HeaderText="City"
                                MappingName="[ShipCity]" />
            <syncfusion:DataGridTextColumn  HeaderText="Country"
                                MappingName="[ShipCountry]" />
            <syncfusion:DataGridDateColumn  HeaderText="Order Date"
                    MappingName="[OrderDate]" />
            <syncfusion:DataGridNumericColumn  HeaderText="Freight"
                                MappingName="[Freight]" Format="C"/>
        </syncfusion:SfDataGrid.Columns>
    </syncfusion:SfDataGrid>
</ContentPage.Content>
```

## C#
The code-behind in MainPage.xaml.cs only initializes the page. The Load More behavior is driven by the view model bound in XAML. Implement the view model with three key members:
- Records: ObservableCollection<T> holding current rows shown in the grid.
- IsBusy: bool flag the grid binds to for showing its busy indicator during fetches.
- LoadMoreRecordsCommand: ICommand that fetches the next page and appends items to Records.

Typical flow for JSON loading with server or local file:
1) Detect grid request: the grid triggers LoadMoreRecordsCommand when the end is reached.
2) Set IsBusy = true to show the loader and guard against re-entry.
3) Fetch JSON page (HTTP or embedded/local file), deserialize into a batch of model items.
4) Append the new items to Records. Do not replace the collection instance to preserve bindings and virtualization.
5) Optionally stop when total items fetched reaches the known count or server signals no more data.
6) Set IsBusy = false.

Example pseudo-implementation outline:
- Keep pageIndex and pageSize fields in the view model.
- Build your JSON endpoint with paging parameters (e.g., /orders?page=2&pageSize=50) or slice a local JSON array.
- Use HttpClient and System.Text.Json (or Newtonsoft.Json) to deserialize.
- AddRange by looping and Records.Add(item) or use a helper that batches updates.

## Data flow and binding notes
- AutoGenerateColumnsMode is None so the column layout is fixed and mapping names can point to model properties or DataTable columns. Bracket syntax like [OrderID] is supported when the source is a tabular structure.
- AllowLoadMore enables the grid’s built-in trigger for requesting additional data. The command is executed automatically based on the grid’s scroll position.
- IsBusy should be toggled immediately before/after a fetch. If the loader does not appear, verify the property raises change notifications (INotifyPropertyChanged).
- If you need initial data, pre-load the first page in the view model constructor or OnAppearing and then rely on LoadMore for subsequent pages.
- When the server indicates the end (empty page or HasMore=false), ignore further requests or disable AllowLoadMore.

## References
- Control overview: https://www.syncfusion.com/maui-controls/maui-datagrid
- Getting started: https://help.syncfusion.com/maui/datagrid/getting-started

##### Conclusion
 
I hope you enjoyed learning about how to populate the JSON data with load-more functionality in .NET MAUI DataGrid (SfDataGrid).
 
You can refer to our [.NET MAUI DataGrid’s feature tour](https://www.syncfusion.com/maui-controls/maui-datagrid) page to learn about its other groundbreaking feature representations. You can also explore our [.NET MAUI DataGrid Documentation](https://help.syncfusion.com/maui/datagrid/getting-started) to understand how to present and manipulate data. 
For current customers, you can check out our .NET MAUI components on the [License and Downloads](https://www.syncfusion.com/sales/teamlicense) page. If you are new to Syncfusion, you can try our 30-day [free trial](https://www.syncfusion.com/downloads/maui) to explore our .NET MAUI DataGrid and other .NET MAUI components.
 
If you have any queries or require clarifications, please let us know in the comments below. You can also contact us through our [support forums](https://www.syncfusion.com/forums), [Direct-Trac](https://support.syncfusion.com/create) or [feedback portal](https://www.syncfusion.com/feedback/maui?control=sfdatagrid), or the feedback portal. We are always happy to assist you!
