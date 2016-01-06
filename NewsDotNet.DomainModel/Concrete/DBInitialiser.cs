using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using NewsDotNet.DomainModel.Entities;

namespace NewsDotNet.DomainModel.Concrete
{
    /// <summary>
    /// Every time the model changes, drops old database, creates new based on new model and populates it with some data
    /// </summary>
    class DBInitialiser : DropCreateDatabaseIfModelChanges<EFDBContext>
    {
        protected override void Seed(EFDBContext context)
        {
            base.Seed(context);

            var tags = new List<Tag>{
                new Tag{Name = "C#", AddressName = "C#"},
                new Tag{Name = "Software", AddressName = "Software"},
                new Tag{Name = ".NET", AddressName = "NET"}
            };
            context.Tags.AddRange(tags);

            var articles = new List<Article>{
                new Article{
                    Title = "Только Java, только Хардкор!",
                    AddressName = "tolko-java-tolko-hardkor",
                    Body = @"Крупнейшая в России Java-конференция JPoint 2015 пройдет при информационной поддержке DataArt и IT talk Spb 20 апреля в Москве.
                    JPoint соберет более тысячи русскоязычных Java-специалистов из России и стран-соседей. На конференции будут представлены и традиционные технические доклады, и круглые столы на актуальные для Java-программистов темы. Планируется более 20 докладов продолжительностью по 50 минут каждый и несколько круглых столов.
                    Доклады будут проходить параллельно в нескольких залах. Основные темы конференции: Java 9, производительность, большие рефакторинги, Groovy, Spring, распределенные системы, высоконагруженные системы и Enterprise-разработка.
                    Подробности на сайте конференции: http://javapoint.ru
                    ",
                     Tags = tags,
                     CreatedTime = DateTime.Now,
                     LastChangedTime = DateTime.Now
                },
                new Article{
                    Title = "Десяточка одесских пианистов: Маэстро Шелковые Пальцы, визуализация Баха и Моцарт-джазмен",
                    AddressName = "desyatochka-odesskih-pianistov",
                    Tags = tags,
                    CreatedTime = DateTime.Now,
                    LastChangedTime = DateTime.Now,
                    Body = @"<strong>THE CHALLENGE?</strong> Немало копий можно сломать, обсуждая, каких десять пианистов Одессы следует считать самыми лучшими, город-то консерваторский!
						А «Думская» не заморачивается, и предлагает своим уважаемым читателям вносить коррективы в комментариях. Имеющий уши да слышит — акробаты клавиатуры и повелители 
						темперированного звукоряда в Одессе никогда не переводятся, исчислять их одним десятком тяжеловато…
                       И на первом месте, как ни крутите, будет Сергей Терентьев, он же Маэстро Шелковые Пальцы, великолепный 
					   интерпретатор классики и джаза, старейшина цеха. Вот во всем, как говорил классик, еще может быть сомненье, но тут даже нечего обсуждать, первый и лучший!"
                },
                new Article{
                    Title = " Академия Яндекса",
                    AddressName = "Yandex-Academy",
                    Body = @" В этом году Яндекс запустил образовательный канал на YouTube http://www.youtube.com/channel/UCTUyoZMfksbNIHfWJjwr5aQ, на котором собраны лекции, семинары, выступления с различных школ Яндекса и конференций от ведущих экспертов по различным направлениям: компьютерные науки, разработка, фронтенд, управление проектами, системное администрирование, тестирование. ",
                     Tags = tags,
                     CreatedTime = DateTime.Now,
                     LastChangedTime = DateTime.Now
                },
                new Article{
                    Title = " Seagate выпустила винчестер на 8 ТБ ",
                    AddressName = "Seagate",
                    Tags = tags,
                    CreatedTime = DateTime.Now,
                    LastChangedTime = DateTime.Now,
                    Body = @" Производители жёстких дисков Western Digital и Seagate практически одновременно объявили о выпуске новых моделей. Компания Seagate даже превзошла конкурента и уже начала рассылать образцы винчестеров на 8 терабайт «основным партнёрам». В отличие от Western Digital, диски Seagate, судя по всему, не используют гелиевое наполнение.  "
                },
                new Article{
                    Title = " Project Spartan",
                    AddressName = "Project-Spartan",
                    Tags = tags,
                    CreatedTime = DateTime.Now,
                    LastChangedTime = DateTime.Now,
                    Body = @" У нового браузера Project Spartan появился слоган.
Фраза звучит просто: «Browser for Doing». Что можно перевести как «Браузер для дел» или «Браузер для работы». "
                },
                new Article{
                    Title = "Servicely автоматически закрывает фоновые процессы Android для экономии заряда батареи ",
                    AddressName = "Servicely",
                    Tags = tags,
                    CreatedTime = DateTime.Now,
                    LastChangedTime = DateTime.Now,
                    Body = @" С помощью выгрузки ненужных фоновых процессов можно значительно ускорить работу Android и уменьшить количество используемой оперативной памяти. Однако операционная система не содержит в своём составе удобных инструментов для управления работающими в фоне приложениями. Решить эту проблему поможет небольшая утилита Servicely. Читать далее: http://www.it-news.club/servicely-avtomaticheski-zakr.. "
                },
                new Article{
                    Title = "Mouse Computer оснастила новый ПК-брелок активной системой охлаждения",
                    AddressName = "Mouse-Computer",
                    Tags = tags,
                    CreatedTime = DateTime.Now,
                    LastChangedTime = DateTime.Now,
                    Body = @"Гаджет выполнен на платформе Intel Bay Trail. Он оборудован процессором Atom Z3735F, который располагает четырьмя вычислительными ядрами с тактовой частотой 1,33 ГГц (динамически повышается до 1,83 ГГц) и графическим контроллером Intel HD Graphics. Объём оперативной памяти равен 2 Гбайт; размер присутствующей на борту флеш-памяти — 32 Гбайт.
На мини-компьютер инсталлирована операционная система Windows 8.1 with Bing. Продажи начнутся в конце апреля; ориентировочная цена гаджета — 175 долларов США. "
                },
                new Article{
                    Title = "Отключение показа рекламы в настройках Скайпа",
                    AddressName = "Skype",
                    Tags = tags,
                    CreatedTime = DateTime.Now,
                    LastChangedTime = DateTime.Now,
                    Body = @"Для того, чтобы отключить показ рекламы в настройках программы Skype, вам нужно будет войти в настройки программы.
Войдите в меню «Инструменты», выберите пункт «Настройки…». Затем, в левой колонке выберите раздел «Безопасность». Во вкладке «Настройки безопасности», необходимо будет снять флажок, который находится напротив пункта «Разрешить показ целевой рекламы Microsoft на основании данных о возрасте и поле, указанных в личных данных в Skype».
После выполнения этого действия, нажмите на кнопку «Сохранить». "
                },
                new Article{
                    Title = "Телескоп Хаббл",
                    AddressName = "Hubble-Telescope",
                    Tags = tags,
                    CreatedTime = DateTime.Now,
                    LastChangedTime = DateTime.Now,
                    Body = @" Космический телескоп Хаббл, разрешающая способность которого в 10 раз больше, чем у аналогичных устройств на Земле, совсем недавно воспользовался этой форой в полной мере. Из-за того, что он находится далеко за пределами земной атмосферы, искажающей изображение, Хабблу удалось сделать, пожалуй, самый подробный космический снимок соседней галактики. Фотография Андромеды получилась настолько большой (1,5 миллиарда пикселей, «весящих» 4,3 гигабайта), что из изображения решено было сделать ВИДЕО. "
                },
                new Article{
                    Title = "Разработчик YotaPhone запатентовал механические часы с NFC-модулем",
                    AddressName = "YotaPhone",
                    Tags = tags,
                    CreatedTime = DateTime.Now,
                    LastChangedTime = DateTime.Now,
                    Body = @"Один из создателей отечественного смартфона YotaPhone в соавторстве с несколькими людьми запатентовали механические часы с NFC-чипом.
Разработчики хотят создать устройство с возможностью оплаты покупок, не подвергая серьёзным изменениям конструктив.
Идея заключается во встраивании в «обыкновенные часы» NFC-чипа, не требующего питания. С помощью чего можно было бы оплачивать покупки в магазинах и проезд в общественном транспорте (в Москве и Санкт-Петербурге для этого применяются специальные карты с той же технологией), а также открывать дверь автомобиля.
Авторы проекта — Дмитрий Гориловский, Денис Свердлов (бывшие сотрудники Yota Devices) и Кирилл Горыня (бывший директор i-Free) в настоящий момент ищут производителя, который бы согласился заняться выпуском подобных часов. Предполагаемые сроки реализации проекта не раньше, чем через год. "
                },
                new Article{
                    Title = "Slando меняет название",
                    AddressName = "OLX",
                    Tags = tags,
                    CreatedTime = DateTime.Now,
                    LastChangedTime = DateTime.Now,
                    Body = @" Крупнейшая в уанете доска объявлений Slando меняет название на OLX.ua. Таким образом, украинский сайт станет частью глобальной сети сайтов бесплатных объявлений OLX, которая входит в группу Naspers."
                },
                new Article{
                    Title = "Google Translate сможет переводить текст с помощью камеры",
                    AddressName = "Google-Translate",
                    Tags = tags,
                    CreatedTime = DateTime.Now,
                    LastChangedTime = DateTime.Now,
                    Body = @" “Переводчик” от Google совсем скоро сможет переводить текст в режиме реального времени с помощью камеры смартфона или планшета.
Такая возможность была реализована в приложении Word Lens ещё несколько лет назад, а сегодня журналисты портала Android Police сообщили о том, что разработчики Google уже сделали обновление, которое вот-вот увидит свет.
Согласно источнику, Google Translate получит функцию перевода текста с помощью камеры мобильных устройств. Владельцу смартфона или планшета достаточно будет оцифровать непонятное словосочетание или предложение, а программа в режиме реального времени переведет его. Сообщается, что в первое время переводить можно будет только с английского или на английский.
Кроме того, сервис обзаведется ещё одной интересной функцией. Программа автоматически сможет определить, а затем перевести человеческую речь во время диалога.
Напомним, что в этом году Google приобрела компанию-разработчика приложение World Lens — Quest Visua. Теперь поисковый гигант решил интегрировать возможности нашумевшего бесплатного переводчика в собственный сервис. На сегодняшний день неизвестно, когда выйдет данное обновление. "
                },
                new Article{
                    Title = "Google выбрала лучшие Android-приложения 2014 года",
                    AddressName = "Android-app-2014",
                    Tags = tags,
                    CreatedTime = DateTime.Now,
                    LastChangedTime = DateTime.Now,
                    Body = @" На главной страничке магазина Google Play появилась подборка лучших приложений, составленных самой компанией Google.
Список содержит 63 программы, 57 из которых бесплатны для скачивания.
Критерии, по которым Google отбирала приложения, неизвестны. Однако скромности интернет-гиганту не занимать — в списке всего 1 приложение, разработанное компанией. И это Google Fit.
Большая часть программ известна и действительно пользуется популярностью, несмотря на невысокие оценки у некоторых. "
                },
                new Article{
                    Title = " Microsoft Lumia 940",
                    AddressName = "Microsoft-Lumia-940",
                    Tags = tags,
                    CreatedTime = DateTime.Now,
                    LastChangedTime = DateTime.Now,
                    Body = @" Флагманский Microsoft Lumia 940 может получить 5.2-дюймовый экран и камеру на 25 МП
Как известно, новые флагманские смартфоны Microsoft планирует выпустить только после выхода Windows 10. Наконец появилась первая информация, которая похожа на правду. Источник утверждает, что в разработке находятся два флагманских аппарата: Lumia 940 и Lumia 940 XL"
                },
                new Article{
                    Title = "Слон шагает по стране",
                    AddressName = "Elephant-walks-along-the-country",
                    Tags = tags,
                    CreatedTime = DateTime.Now,
                    LastChangedTime = DateTime.Now,
                    Body = @" Мега быстрые карты памяти Strontium Nistro Plus появились в российском ритейле.
Компания с логотипом, изображающим слона, выпустила на российский рынок карты памяти, которые при равной другим предложениям цене предлагают гораздо более высокую скорость чтения и записи (ну или при равной скорости они дешевле примерно на 30%). 
Есть Micro SD Nitro и Micro SD Nitro Plus, которые подойдут для планшетов и смартфонов, и SD Nitro и SD Nitro Plus для фото- и видеокамер. 
В комплектации есть обычные кардридеры или даже OTG-кардридеры для подключения и к USB, и к micro USB - для простого переноса данных с телефона на компьютер.
Вообще ребята жгут: в ассортименте есть, например, зололая флешка и самая малешькая OTG-флешка в мире (это та, что с USB для компьютера с одной стороны и micro USB для смартфона - с другой). "
                }
            };

            foreach(var article in articles)
            {
                article.TitleImagePath = "~/Content/Images/Articles/" + article.AddressName + ".jpg";
                article.State = ArticleStates.Published;
            }

            context.Articles.AddRange(articles);
            var mainPage = new List<MainPageEntity>();
            for(int i = 0; i< articles.Count;i++)
            {
                var entity = new MainPageEntity { Article = articles[i]};
                if(i <= 3)
                {
                    entity.IsFeatured = true;
                }
                entity.GridData = new GridData
                {
                    Col = i % 6 + 1,
                    Row = i / 6 + 1,
                    SizeX = 1,
                    SizeY = 1
                };
                mainPage.Add(entity);
            }

            context.MainPageEntities.AddRange(mainPage);
            context.SaveChanges();
        }
    }
}
