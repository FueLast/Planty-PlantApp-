namespace PlantApp.Services;

// интерфейс для передачи параметров при навигации
public interface IInitialize<T>
{
    void Initialize(T parameter);
}