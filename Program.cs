//1 Система библиотеки
using System;
using System.Collections.Generic;

public enum AvailabilityStatus
{
    Available,
    Rented
}

public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public AvailabilityStatus Status { get; set; }

    public Book(string title, string author, string isbn)
    {
        Title = title;
        Author = author;
        ISBN = isbn;
        Status = AvailabilityStatus.Available;
    }

    public void GetBookInfo()
    {
        Console.WriteLine($"Title: {Title}, Author: {Author}, ISBN: {ISBN}, Status: {Status}");
    }

    public void ChangeStatus(AvailabilityStatus status)
    {
        Status = status;
    }
}

public class Reader
{
    public string Name { get; set; }
    public List<Book> RentedBooks { get; set; }

    public Reader(string name)
    {
        Name = name;
        RentedBooks = new List<Book>();
    }

    public void RentBook(Book book)
    {
        if (book.Status == AvailabilityStatus.Available)
        {
            book.ChangeStatus(AvailabilityStatus.Rented);
            RentedBooks.Add(book);
            Console.WriteLine($"{Name} взял книгу напрокат: {book.Title}");
        }
        else
        {
            Console.WriteLine($"Извините, книга '{book.Title}' в настоящее время сдается в аренду.");
        }
    }

    public void ReturnBook(Book book)
    {
        if (RentedBooks.Contains(book))
        {
            book.ChangeStatus(AvailabilityStatus.Available);
            RentedBooks.Remove(book);
            Console.WriteLine($"{Name} вернул книгу: {book.Title}");
        }
        else
        {
            Console.WriteLine($"{Name} не брал эту книгу напрокат.");
        }
    }
}

public class Librarian
{
    public string Name { get; set; }

    public Librarian(string name)
    {
        Name = name;
    }

    public void ManageBook(Book book)
    {
        // Example of librarian's ability to manage books
        Console.WriteLine($"Библиотекарь {Name} занимается ведением книги: {book.Title}");
    }

    public Book SearchBook(string title, List<Book> books)
    {
        foreach (var book in books)
        {
            if (book.Title.Contains(title))
            {
                return book;
            }
        }
        return null;
    }
}

public class Library
{
    public List<Book> Books { get; set; }

    public Library()
    {
        Books = new List<Book>();
    }

    public void AddBook(Book book)
    {
        Books.Add(book);
        Console.WriteLine($"Добавлен {book.Title} в библиотеку.");
    }

    public void RemoveBook(Book book)
    {
        Books.Remove(book);
        Console.WriteLine($"Удален {book.Title} из библиотеки.");
    }

    public void DisplayBooks()
    {
        Console.WriteLine("Книги в библиотеке:");
        foreach (var book in Books)
        {
            book.GetBookInfo();
        }
    }

    public Book SearchBooks(string query)
    {
        foreach (var book in Books)
        {
            if (book.Title.Contains(query) || book.Author.Contains(query))
            {
                return book;
            }
        }
        Console.WriteLine("Книг, соответствующих запросу, не найдено.");
        return null;
    }
}
class Program
{
    static void Main(string[] args)
    {
        // Создаем библиотеку
        Library library = new Library();
        Book book1 = new Book("C# Programming", "John Doe", "12345");
        Book book2 = new Book("Design Patterns", "Erich Gamma", "67890");
        library.AddBook(book1);
        library.AddBook(book2);

        // Создаем читателя и библиотекаря
        Reader reader = new Reader("Alice");
        Librarian librarian = new Librarian("Bob");

        // Библиотекарь управляет книгами
        librarian.ManageBook(book1);

        // Читатель арендует книгу
        reader.RentBook(book1);

        // Показать все книги в библиотеке
        library.DisplayBooks();

        // Читатель возвращает книгу
        reader.ReturnBook(book1);

        // Показать все книги в библиотеке
        library.DisplayBooks();
    }
}

//2 Архитектура и система брони
public interface IHotelService
{
    List<Hotel> SearchHotels(string location, string roomType, decimal? maxPrice);
    Hotel GetHotelInfo(int hotelId);
}

public interface IBookingService
{
    bool CheckAvailability(int hotelId, DateTime startDate, DateTime endDate);
    Booking MakeBooking(int hotelId, int userId, DateTime startDate, DateTime endDate);
}

public interface IPaymentService
{
    bool ProcessPayment(int userId, decimal amount);
    bool CheckPaymentStatus(int paymentId);
}

public interface INotificationService
{
    void SendConfirmation(int bookingId);
    void SendReminder(int bookingId);
}

public interface IUserManagementService
{
    void Register(User user);
    bool Login(string username, string password);
    User GetUserInfo(int userId);
}
public class HotelService : IHotelService
{
    private List<Hotel> hotels = new List<Hotel>();

    public HotelService()
    {
        // Инициализация отелей (для тестирования)
        hotels.Add(new Hotel { Id = 1, Name = "Hotel A", Location = "City A", RoomType = "Single", Price = 100 });
        hotels.Add(new Hotel { Id = 2, Name = "Hotel B", Location = "City B", RoomType = "Double", Price = 150 });
    }

    public List<Hotel> SearchHotels(string location, string roomType, decimal? maxPrice)
    {
        return hotels.Where(h => h.Location.Contains(location) && h.RoomType.Contains(roomType)
                                    && (maxPrice == null || h.Price <= maxPrice.Value)).ToList();
    }

    public Hotel GetHotelInfo(int hotelId)
    {
        return hotels.FirstOrDefault(h => h.Id == hotelId);
    }
}

public class Hotel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public string RoomType { get; set; }
    public decimal Price { get; set; }
}
public class BookingService : IBookingService
{
    private List<Booking> bookings = new List<Booking>();

    public bool CheckAvailability(int hotelId, DateTime startDate, DateTime endDate)
    {
        // Простейшая логика проверки наличия номера (для демонстрации)
        return !bookings.Any(b => b.HotelId == hotelId && b.StartDate < endDate && b.EndDate > startDate);
    }

    public Booking MakeBooking(int hotelId, int userId, DateTime startDate, DateTime endDate)
    {
        var booking = new Booking { HotelId = hotelId, UserId = userId, StartDate = startDate, EndDate = endDate };
        bookings.Add(booking);
        return booking;
    }
}

public class Booking
{
    public int HotelId { get; set; }
    public int UserId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
public class PaymentService : IPaymentService
{
    public bool ProcessPayment(int userId, decimal amount)
    {
        // Простая проверка оплаты (демонстрация)
        Console.WriteLine($"Обработка платежей по {amount} для пользователя {userId}.");
        return true;
    }

    public bool CheckPaymentStatus(int paymentId)
    {
        // Проверка состояния оплаты (для демонстрации)
        return true;
    }
}
public class NotificationService : INotificationService
{
    public void SendConfirmation(int bookingId)
    {
        Console.WriteLine($"Бронирование {bookingId} подтвержден.");
    }

    public void SendReminder(int bookingId)
    {
        Console.WriteLine($"Напоминание: Бронирование {bookingId} приближается.");
    }
}
public class UserManagementService : IUserManagementService
{
    private List<User> users = new List<User>();

    public void Register(User user)
    {
        users.Add(user);
        Console.WriteLine($"Пользователь {user.Username} зарегистрированный.");
    }

    public bool Login(string username, string password)
    {
        return users.Any(u => u.Username == username && u.Password == password);
    }

    public User GetUserInfo(int userId)
    {
        return users.FirstOrDefault(u => u.Id == userId);
    }
}

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
public class Program
{
    static void Main(string[] args)
    {
        IHotelService hotelService = new HotelService();
        IBookingService bookingService = new BookingService();
        IPaymentService paymentService = new PaymentService();
        INotificationService notificationService = new NotificationService();
        IUserManagementService userManagementService = new UserManagementService();

        // Пример работы с системой
        var hotels = hotelService.SearchHotels("City A", "Single", 120);
        foreach (var hotel in hotels)
        {
            Console.WriteLine($"Найденный отель: {hotel.Name}, {hotel.Location}, {hotel.Price}$ за ночь");
        }

        var user = new User { Id = 1, Username = "john_doe", Password = "password123" };
        userManagementService.Register(user);

        if (userManagementService.Login("john_doe", "password123"))
        {
            Console.WriteLine("Пользователь успешно вошел в систему!");
            var booking = bookingService.MakeBooking(1, 1, DateTime.Now, DateTime.Now.AddDays(2));
            paymentService.ProcessPayment(user.Id, 200);
            notificationService.SendConfirmation(booking.GetHashCode());
        }
    }
}
