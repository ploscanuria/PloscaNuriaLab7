namespace PloscaNuriaLab7;
using PloscaNuriaLab7.Models;
using Plugin.LocalNotification;
using PloscaNuriaLab7.Data;

public partial class ShopPage : ContentPage
{
    public ShopPage()
    {
        InitializeComponent();
    }

    
    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;
        await App.Database.SaveShopAsync(shop);
        await Navigation.PopAsync();
    }

    
    async void OnShowMapButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;
        var address = shop.Adress;
        var locations = await Geocoding.GetLocationsAsync(address);

        var options = new MapLaunchOptions
        {
            Name = "Magazinul meu preferat"
        };
        var shoplocation = locations?.FirstOrDefault();
        

        var myLocation = await Geolocation.GetLocationAsync();
       

        var distance = myLocation.CalculateDistance(shoplocation, DistanceUnits.Kilometers);
        if (distance < 5)
        {
            var request = new NotificationRequest
            {
                Title = "Ai de facut cumparaturi in apropiere!",
                Description = address,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now.AddSeconds(1)
                }
            };
            await LocalNotificationCenter.Current.Show(request);
        }

        await Map.OpenAsync(shoplocation, options);
    }

    
    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;

       
        bool confirm = await DisplayAlert("Confirmare", $"Ești sigur că vrei să ștergi magazinul {shop.ShopName}?", "Da", "Nu");
        if (!confirm)
            return;

        
        await App.Database.DeleteShopAsync(shop);

        
        await Navigation.PopAsync();
    }
}
