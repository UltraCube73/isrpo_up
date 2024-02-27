# Онлайн-магазин с подарками (вариант 3)
---
### Руководство администратора
Для добавления товара в магазин необходимо добавить запись в таблицу БД Products и внести файл-изображение в папку Images.
Таблицы БД:
- Id - идентификатор, присваевается автоматически
- Code - программное название товара на латинице, пробелы и спецсимволы недопустимы, числа не должны стоять в начале строки
- Name - название товара
- Cost - целочисленная стоимость товара
- Path - название файла с изображением

Желательно, чтобы файлы были в формате .PNG
**Пример таблицы с товарами приведен в файле "Товары.xlsx"**
---
### Руководство пользователя
##### Регистрация и авторизация
При открытии приложения пользователя встречает форма авторизации.
Зайдя в приложение первый раз, пользователь должен создать аккаунт, перейдя на форму регистрации по кнопке "Регистрация" и введя адрес почты, желаемые логин и пароль и нажав на кнопку "Зарегистрироваться". Пользователя перекинет на форму авторизации с уже введенными данными, при следующем заходе пользователь должен ввести эти данные самостоятельно.
##### Каталог
В каталоге пользователь может увидеть ассортимент товаров, предлагаемых магазином и добавить желаемые позиции в корзину по кнопке "Добавить". В случае, если пользователь добавляет уже имеющийся в корзине товар, его количество в корзине увеличивается на единицу.
Пользователь может перейти в корзину или выйти из аккаунта по кнопкам "В корзину" и "Выйти" соответственно.
##### Корзина
В корзине пользователь увидит позиции, выбранные в каталоге, количество единиц заказываемой позиции, стоимость за единицу товара и итоговую сумму покупки. Нажимая кнопки "+" и "-", пользователь увеличивает или уменьшает количество единиц заказываемой позиции, при уменьшении количества до нуля позиция убирается из корзины. Итоговая сумма складывается из сум цен товаров, умноженных на их количество.
По кнопке "В каталог" и "Выйти" пользователь может вернуться в каталог для докупки или выйти из аккаунта соответственно.
По кнопке "Купить" появляется плейсхолдер формы оплаты для ее имитации, после чего корзина очищается. По кнопке "Очистить корзину" все позиции удаляются из корзины.