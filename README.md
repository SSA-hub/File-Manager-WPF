# File-Manager-WPF
![alt text](https://github.com/SSA-hub/File-Manager-WPF/blob/main/image.png)

### Возможности:
При двойном клике на директорю открывается директория  
При двойном клике на файл он открывается средствами Windows  
При нажатии на выбранную директорию отображается количество файло в ней и их объем  
При нажатии на выбранный файл отображаются его размер, дата создания и изменения  
При открытии файла осуществляется запись данных об этом событии в БД PostgreSQL (имя файла, дата и время открытия)  
При запуске приложения проходят миграции для создания таблицы истории открытия файлов  
В файле конфигурации appsettings.json можно задать Connection String для подключения к БД  
Строка пути кликабельна - по двойному клику можно перейти в выбранную директорию пути  
Реализован поиск по файлам в текущей директории, по двойному клику по выбранному варианту из предложенных открывается соответсвующая директория или файл  
Реализована кнопка "Назад" для открытия предыдущей директории  
Стартовая страница - список дисков
