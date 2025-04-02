using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using Avalonia.Markup.Xaml;
using System.Linq;
using System.Collections.Generic;

namespace SmartHouse;

public partial class MainWindow : Window
{
    private Room? room;
    private readonly List<Room> rooms = new();
    
    public MainWindow()
    {
        InitializeComponent();
        
        rooms.Add(new Room("Кухня", 20));
        rooms.Add(new Room("Гостиная", 22));
        rooms.Add(new Room("Спальня", 21));
        
        RoomsListBox.ItemsSource = rooms;
        room = rooms.FirstOrDefault();
        
        if(room != null)
        {
            room.OnLightStateChanged += message => RoomInfo.Text = message;
            room.OnTemperatureChanged += message => RoomInfo.Text = message;
            RoomInfo.Text = room.GetRoomInfo();
            nameBox.Text = room.Name;
            TemperatureSlider.Value = room.Temperature;
            LightBox.IsChecked = room.IsLightOn;
        }
    }
  
    private void RoomsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (RoomsListBox.SelectedItem is Room selectedRoom)
        {
            room = selectedRoom;
            RoomInfo.Text = room.GetRoomInfo();
            nameBox.Text = room.Name;
            TemperatureSlider.Value = room.Temperature;
            LightBox.IsChecked = room.IsLightOn;
        }
    }
    
    private void nameChange(object sender, Avalonia.Controls.TextChangedEventArgs e)
    {
        if (room != null && !string.IsNullOrWhiteSpace(nameBox.Text))
        {
            room.SetName(nameBox.Text);
            RoomInfo.Text = room.GetRoomInfo();
            
            RoomsListBox.ItemsSource = null;
            RoomsListBox.ItemsSource = rooms;
        }
    }
    
    private void SliderTemperature(object sender, Avalonia.Controls.Primitives.RangeBaseValueChangedEventArgs e)
    {
        if (room != null)
        {
            room.SetTemperature((int)e.NewValue);
            RoomInfo.Text = room.GetRoomInfo();
        }
    }
    
    private void CheckBoxLight(object sender, RoutedEventArgs e)
    {
        if (room != null)
        {
            if (LightBox.IsChecked == true)
                room.TurnLightOn();
            else
                room.TurnLightOff();
                
            RoomInfo.Text = room.GetRoomInfo();
        }
    }

    private void OpenNewWindow(object sender, RoutedEventArgs e)
    {
        var addWindow = new ADDWindow();
        addWindow.Show();
    }
}

public class Room 
{
    private string _name;
    private bool isLightOn;
    private int _temperature;
    
    public Room(string name, int temperature)
    {
        Name = name;
        Temperature = temperature;
        isLightOn = true;
    }

    public bool IsLightOn
    {
        get => isLightOn;
        set => isLightOn = value;
    }

    public string LightState => isLightOn ? "ВКЛ" : "ВЫКЛ";
    
    public string Name
    {
        get => _name;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("Поле не должно быть пустым");
            _name = value;
        }
    }

    public int Temperature
    {
        get => _temperature;
        private set => _temperature = value;
    }

    public void TurnLightOn()
    {
        isLightOn = true;
        OnLightStateChanged?.Invoke($"Свет: {LightState}");
    }

    public void TurnLightOff()
    {
        isLightOn = false;
        OnLightStateChanged?.Invoke($"Свет: {LightState}");
    }

    public void SetName(string new_name)
    {
        if (string.IsNullOrWhiteSpace(new_name))
            throw new ArgumentNullException("Имя не может быть пустым");
        _name = new_name;
    }

    public void SetTemperature(int new_temperature)
    {
        if (new_temperature < 15 || new_temperature > 30)
            throw new ArgumentOutOfRangeException("Температура должна быть между 15 и 30");
        
        _temperature = new_temperature;
        OnTemperatureChanged?.Invoke($"Температура: {_temperature}°C");
    }

    public event Action<string>? OnLightStateChanged;
    public event Action<string>? OnTemperatureChanged;

    public string GetRoomInfo() => $"Комната: {Name}\nТемпература: {Temperature}°C\nСвет: {LightState}";
}