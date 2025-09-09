using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using library.Models;

namespace library.ViewModels;

public partial class NotificationsViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<Notification> todayNotifications 
        = new ObservableCollection<Notification>();

    [ObservableProperty] private ObservableCollection<Notification> weekNotifications 
        = new ObservableCollection<Notification>();

    public NotificationsViewModel()
    {
        TodayNotifications.Add(new Notification
        {
            Title = "Elayamari",
            Message = "te dio like",
            TimeAgo = "2h",
            Avatar = "C:\\Users\\Alienware\\RiderProjects\\library\\Assets\\21249201424022cdd93cd144f099b056.jpg"
        });
        TodayNotifications.Add(new Notification
        {
            Title = "Johny vino",
            Message = "te mencionó en un comentario",
            TimeAgo = "8h",
            Avatar = "C:\\Users\\Alienware\\RiderProjects\\library\\Assets\\21249201424022cdd93cd144f099b056.jpg"

        });

        WeekNotifications.Add(new Notification
        {
            Title = "Brice seraphin",
            Message = "te dio like",
            TimeAgo = "6 Jun",
            Avatar = "C:\\Users\\Alienware\\RiderProjects\\library\\Assets\\21249201424022cdd93cd144f099b056.jpg"

        });
        WeekNotifications.Add(new Notification
        {
            Title = "Best UI Design",
            Message = "empezó a seguirte",
            TimeAgo = "5 Jun",
            Avatar = "C:\\Users\\Alienware\\RiderProjects\\library\\Assets\\21249201424022cdd93cd144f099b056.jpg"

        });
    }
}