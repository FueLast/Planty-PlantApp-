using System.Xml.Linq;

namespace PlantApp.ViewModels
{
    internal class GPTViewModel
    {
    }
}


//< !--нижняя часть экрана-->

//        <Grid BackgroundColor="White" 
//            HeightRequest="100" 
//            VerticalOptions="End"
//            RowSpacing="5" 
//            ColumnSpacing="5"
//            Padding="10">


//            <Grid.RowDefinitions>
//                <RowDefinition Height="*"/>
//            </Grid.RowDefinitions>

//            <Grid.ColumnDefinitions>
//                <ColumnDefinition  Width="*"/>
//                <ColumnDefinition  Width="*"/>
//                <ColumnDefinition  Width="*"/>
//                <ColumnDefinition  Width="*"/>
//            </Grid.ColumnDefinitions>


//<!-- Кнопка главной страницы -->
//            <Border 
//                x:Name = "HomePageButton"
//                Grid.Row = "0" Grid.Column = "0"
//                BackgroundColor = "White"
//                Stroke = "Transparent"
//                Padding = "10" >

//                < Border.GestureRecognizers >
//                    < TapGestureRecognizer Command = "{Binding OnHomePageCommand}" />
//                </ Border.GestureRecognizers >
//                < !--Вставляем содержимое внутрь кнопки -->
//                <StackLayout Spacing="5" VerticalOptions="Center" HorizontalOptions="Center">
//                    <Image Source="D:\Диплом\PlantApp\PlantApp\Resources\Images\main_page_plant.png" HeightRequest="32" WidthRequest="23"/>
//                    <Label Text="Главная"
//                        FontSize="12"
//                        TextColor="Black" 
//                        HorizontalTextAlignment="Center"/>
//                </StackLayout>
//            </Border>



//< !--Кнопка страницы чата -->
//            <Border 
//                x:Name = "ChatPageButton"
//                Grid.Row = "0" Grid.Column = "1"
//                BackgroundColor = "White"
//                Stroke = "Transparent"
//                Padding = "10" >

//                < Border.GestureRecognizers >
//                    < TapGestureRecognizer Command = "{Binding OnChatPageCommand}" />
//                </ Border.GestureRecognizers >
//                < !--Вставляем содержимое внутрь кнопки -->
//                <StackLayout Spacing="5" VerticalOptions="Center" HorizontalOptions="Center">
//                    <Image Source="D:\Диплом\PlantApp\PlantApp\Resources\Images\chat_page.png" HeightRequest="32" WidthRequest="23"/>
//                    <Label Text="Чат"
//                        FontSize="12"
//                        TextColor="Black" 
//                        HorizontalTextAlignment="Center"/>
//                </StackLayout>
//            </Border>


//<!-- Кнопка страницы календаря -->
//            <Border 
//                x:Name = "CalendarPageButton"
//                Grid.Row = "0" Grid.Column = "2"
//                BackgroundColor = "White"
//                Stroke = "Transparent"
//                Padding = "10" >

//                < Border.GestureRecognizers >
//                    < TapGestureRecognizer Command = "{Binding OnCalendarPageCommand}" />
//                </ Border.GestureRecognizers >
//                < !--Вставляем содержимое внутрь кнопки -->
//                <StackLayout Spacing="5" VerticalOptions="Center" HorizontalOptions="Center">
//                    <Image Source="D:\Диплом\PlantApp\PlantApp\Resources\Images\calendar.png" HeightRequest="32" WidthRequest="23"/>
//                    <Label Text="Календарь"
//                        FontSize="12"
//                        TextColor="Black" 
//                        HorizontalTextAlignment="Center"/>
//                </StackLayout>
//            </Border>


//<!-- Кнопка страницы профиля -->
//            <Border 
//                x:Name = "ProfilePageButton"
//                Grid.Row = "0" Grid.Column = "3"
//                BackgroundColor = "White"
//                Stroke = "Transparent"
//                Padding = "10" >

//                < Border.GestureRecognizers >
//                    < TapGestureRecognizer Command = "{Binding OnProfilePageCommand}" />
//                </ Border.GestureRecognizers >
//                < !--Вставляем содержимое внутрь кнопки -->
//                <StackLayout Spacing="5" VerticalOptions="Center" HorizontalOptions="Center">
//                    <Image Source="D:\Диплом\PlantApp\PlantApp\Resources\Images\profile_page.png" HeightRequest="32" WidthRequest="23"/>
//                    <Label Text="Профиль"
//                        FontSize="12"
//                        TextColor="Black" 
//                        HorizontalTextAlignment="Center"/>
//                </StackLayout>
//            </Border>

//        </Grid>
