using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using Avalonia.Markup.Xaml;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;

namespace SmartHouse;

public partial class MainWindow : Window
{
    private Room room;
    
    private List<Room> rooms = new List<Room>();
    
    
    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
        
        room= (new Room("Кухня",20));
        room= rooms.FirstOrDefault();
        if(room!=null)
        {room.OnLightStateChanged += message => RoomInfo.Text = message;
        room.OnTemperatureChanged += message => RoomInfo.Text = message;
       
        room.GetRoomInfo();
        RoomInfo.Text = room.GetRoomInfo();}
        
        RoomsListBox.ItemsSource = rooms;
    }
    public List<Room> Rooms => rooms;
  
     private void RoomsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
     {
         if (RoomsListBox.SelectedItem is Room selectedRoom)
         {
             room = selectedRoom;
             RoomInfo.Text = room.GetRoomInfo();
         }
    }
    private void nameChange(object sender, Avalonia.Controls.TextChangedEventArgs e)
    {
        string name = nameBox.Text;
        if (!string.IsNullOrWhiteSpace(name))
        {
            room.SetName(name);
            RoomInfo.Text = room.GetRoomInfo();
        }
    }
    private void SliderTemperature(object sender, Avalonia.Controls.Primitives.RangeBaseValueChangedEventArgs e)
    {
        if (room != null)
        {
        int new_temperature = (int)e.NewValue;
        room.SetTemperature(new_temperature);
        RoomInfo.Text = room.GetRoomInfo();
        }
        else
        {
            Console.WriteLine("noo");
        }
    }
    private void CheckBoxLight(object sender, RoutedEventArgs e)
    {
        if (LightBox.IsChecked == true)
        {
            room.TurnLightOn();
            RoomInfo.Text = room.GetRoomInfo();
        }
        else
        {
            room.TurnLightOff();
            RoomInfo.Text = room.GetRoomInfo();
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
            _temperature = temperature;

            isLightOn = true;
        }

        public bool IsLightOn
        {
            get { return isLightOn;}
            set { isLightOn = value; }
        }

        public string LightState
        {
            get { return isLightOn ? "ВКЛ" : "ВЫКЛ"; }
        }
        public string Name
        {
            
            get { return _name; }
            private set {
                if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException("Поле не должно быть пустым");
            }
                _name = value;
            }

        }

        public int Temperature
        {
            get { return _temperature; }
            private set { _temperature = value; }
        }

        public void TurnLightOn()
        {
            isLightOn=true;
            OnLightStateChanged.Invoke(LightState);
            
        }

        public void TurnLightOff()
        {
            isLightOn=false;
            OnLightStateChanged.Invoke(LightState);
        }

        public void SetName(string new_name)
        {
            if (string.IsNullOrWhiteSpace(new_name))
            {
                throw new ArgumentNullException("NOOOOO");
            }
            _name = new_name;
        }

        public void SetTemperature(int new_temperature)
        {
            if (new_temperature <= 15||new_temperature>=30)
            {
                throw new ArgumentOutOfRangeException("Неверный диапазон");
            }
            else
            {
                _temperature = new_temperature;
                OnTemperatureChanged.Invoke($"{_temperature}");
            }
            
        }

        public event Action<string>OnLightStateChanged;
        public event Action<string> OnTemperatureChanged;
       

        public string GetRoomInfo()
        {
            return $"Комната: {Name}\nТемпература: {Temperature}°C\nСвет: {LightState}";
            
        }
    }
}

    