using System;
using Microsoft.EntityFrameworkCore; 

namespace PlantApp.Data
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();   // создать новую
            Database.EnsureCreated();   // создать новую
        }

        public DbSet<Plant> Plants { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Chat> Chats { get; set; } 
        public DbSet<FavoritePlant> FavoritePlants { get; set; }
        public DbSet<UserPlant> UserPlants { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<SwapOffer> SwapOffers { get; set; }
        public DbSet<SwapRequest> SwapRequests { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Chat>()
                .HasMany(c => c.Messages)
                .WithOne(m => m.Chat)
                .HasForeignKey(m => m.ChatId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserPlant>()
                .HasOne(p => p.Plant)
                .WithMany()
                .HasForeignKey(p => p.PlantId);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Login)
                .IsUnique();

            modelBuilder.Entity<UserProfile>()
                .HasIndex(p => p.UserId)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasOne(u => u.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<UserProfile>(p => p.UserId) 
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FavoritePlant>()
                .HasKey(fp => new { fp.UserId, fp.PlantId });

            modelBuilder.Entity<FavoritePlant>()
                .HasOne(f => f.Plant)
                .WithMany()
                .HasForeignKey(f => f.PlantId);

            modelBuilder.Entity<Plant>().HasData(
            new Plant
            {
                Id = 1,
                NamePlant = "Монстера",
                DescriptionPlant = "Монстера (Monstera) — популярное комнатное растение семейства Ароидные, родом из тропиков Центральной и Южной Америки",
                PlantCare = "Освещение: Яркий рассеянный свет. Прямые солнечные лучи могут обжечь листья, а в глубокой тени листья перестают быть резными.\r\nПолив: Умеренный, после просыхания верхнего слоя почвы на 2–3 см. Не терпит застоя воды.\r\nВлажность: Любит регулярное опрыскивание и протирание листьев влажной губкой.\r\nОпора: Из-за тяжелых стеблей и воздушных корней монстере необходима опора (кокосовая палка или моховой столб).",
                PlantImage = "aloe_centaurium.png",

                Care_Light = "Яркий рассеянный",
                Care_Watering = "1–2 раза в неделю",
                Care_Temperature = "18–27°C",
                Care_Complexity = "Лёгкий",

                Feature_One = "Быстрый рост",
                Feature_Two = "Очищает воздух",
                Feature_Three = "Нужна опора",
                Feature_Four = "Токсична",

                EstimatedHeight = 200,
                SearchNames = "Монстера Monstera"
            },
            new Plant
            {
                Id = 2,
                NamePlant = "Алоказия",
                DescriptionPlant = "Алоказия (Alocasia) — эффектное тропическое растение с крупными стреловидными листьями, которое часто называют «слоновьим ухом» за их необычную форму и размер.",
                PlantCare = "Освещение: Яркое, но рассеянное. Беречь от прямых лучей, чтобы не было ожогов. Виды с темными листьями более теневыносливы.\r\nПолив: Регулярный, но без застоя влаги. Земля должна оставаться слегка влажной, но не мокрой.\r\nВлажность: Высокая (60-80%). Требует частого опрыскивания или использования увлажнителя воздуха.\r\nОсобенности: Растение теплолюбиво и может впадать в период покоя зимой, сбрасывая часть листьев.",
                PlantImage = "alocasia.png",

                Care_Light = "Яркий рассеянный",
                Care_Watering = "Регулярный",
                Care_Temperature = "20–28°C",
                Care_Complexity = "Средний",

                Feature_One = "Тропическое",
                Feature_Two = "Любит влажность",
                Feature_Three = "Крупные листья",
                Feature_Four = "Токсична",

                EstimatedHeight = 200,
                SearchNames = "Алоказия Alocasia"
            },
            new Plant
            {
                Id = 3,
                NamePlant = "Алоэ",
                DescriptionPlant = "Алоэ древовидное (Aloe arborescens) — суккулент, широко известный как «столетник». Ценится за свои лечебные свойства и неприхотливость в уходе.",
                PlantCare = "Освещение: Светолюбиво, отлично переносит прямые солнечные лучи. Лучше всего чувствует себя на южных или восточных подоконниках.\r\nПолив: Редкий. Летом — после полного просыхания почвы, зимой — не чаще одного раза в месяц. Избыток влаги губителен.\r\nВлажность: Хорошо переносит сухой комнатный воздух. В опрыскивании не нуждается, достаточно иногда протирать листья от пыли.\r\nПочва: Требует рыхлого грунта с хорошим дренажем (подходит смесь для кактусов и суккулентов).",
                PlantImage = "aloe_centaurium.png",

                Care_Light = "Прямое солнце",
                Care_Watering = "Редкий",
                Care_Temperature = "18–30°C",
                Care_Complexity = "Очень лёгкий",

                Feature_One = "Лечебный",
                Feature_Two = "Суккулент",
                Feature_Three = "Не боится засухи",
                Feature_Four = "Медленный рост",


                EstimatedHeight = 200,
                SearchNames = "Алоэ Aloe"
            },
            new Plant
            {
                Id = 4,
                NamePlant = "Хлорофитум",
                DescriptionPlant = "Хлорофитум (Chlorophytum) — одно из самых неприхотливых и полезных комнатных растений, эффективно очищающее воздух от токсинов и формальдегидов.",
                PlantCare = "Освещение: Универсальное. Хорошо растет как на ярком рассеянном свету, так и в полутени. На солнце окраска листьев становится ярче.\r\nПолив: Обильный с весны до осени, зимой — умеренный. Легко прощает забывчивость в поливе благодаря утолщенным корням, запасающим влагу.\r\nВлажность: Адаптируется к любым условиям, но любит периодический теплый душ для очистки листьев.\r\nРазмножение: Легко размножается «детками», которые вырастают на длинных свисающих усах.",
                PlantImage = "chlorophytum.png",

                Care_Light = "Любой",
                Care_Watering = "Умеренный",
                Care_Temperature = "16–25°C",
                Care_Complexity = "Лёгкий",

                Feature_One = "Очищает воздух",
                Feature_Two = "Быстро растёт",
                Feature_Three = "Даёт деток",
                Feature_Four = "Неприхотлив",

                EstimatedHeight = 150,
                SearchNames = "Хлорофитум Chlorophytum"
            },
            new Plant
            {
                Id = 5,
                NamePlant = "Фикус",
                DescriptionPlant = "Фикус (Ficus) — величественное древовидное растение. В домашних условиях наиболее популярны мелколистный фикус Бенджамина и крупнолистный каучуконосный фикус.",
                PlantCare = "Освещение: Предпочитает светлое место без прямых солнечных лучей. Сорта с пестрыми листьями требуют больше света, иначе они становятся просто зелеными.\r\nПолив: Умеренный, строго после просыхания верхнего слоя земли на 2–4 см. Очень чувствителен к заливу.\r\nСтабильность: Не любит смену места и сквозняки — в ответ на стресс фикус может стремительно сбросить все листья.\r\nВлажность: Любит регулярное опрыскивание мягкой водой и протирание листьев от пыли.",
                PlantImage = "ficus.png",

                Care_Light = "Яркий рассеянный",
                Care_Watering = "Умеренный",
                Care_Temperature = "18–25°C",
                Care_Complexity = "Средний",

                Feature_One = "Боится сквозняков",
                Feature_Two = "Сбрасывает листья",
                Feature_Three = "Любит стабильность",
                Feature_Four = "Декоративный",

                EstimatedHeight = 100,
                SearchNames = "Фикус Ficus"
            },
            new Plant
            {
                Id = 6,
                NamePlant = "Сансевиерия",
                DescriptionPlant = "Сансевиерия (Sansevieria) — сверхвыносливое растение, известное под народными названиями «щучий хвост» или «тещин язык». Один из лучших очистителей воздуха, выделяющий кислород даже ночью.",
                PlantCare = "Освещение: Абсолютно неприхотлива. Одинаково хорошо растет как на ярком солнце, так и в глубине темных комнат.\r\nПолив: Крайне редкий. Поливать нужно только тогда, когда земляной ком полностью просох. Зимой достаточно одного раза в 3–4 недели.\r\nВлажность: Легко переносит сухой воздух. Опрыскивание не требуется, достаточно изредка протирать плотные листья от пыли.\r\nОсобенности: Главная опасность — перелив и попадание воды в центр розетки, что может вызвать гниение.",
                PlantImage = "sansevieria_pike_tail.png",

                Care_Light = "Любой",
                Care_Watering = "Очень редкий",
                Care_Temperature = "16–30°C",
                Care_Complexity = "Очень лёгкий",

                Feature_One = "Неубиваемая",
                Feature_Two = "Очищает воздух",
                Feature_Three = "Растёт в тени",
                Feature_Four = "Боится перелива",

                EstimatedHeight = 200,
                SearchNames = "Сансевиерия Sansevieria"
            },
            new Plant
            {
                Id = 7,
                NamePlant = "Эпипремнум",
                DescriptionPlant = "Эпипремнум (Epipremnum) — эффектная и неприхотливая тропическая лиана с сердцевидными листьями. Ценится за быстрый рост и способность эффективно очищать воздух от токсинов. Может выращиваться как ампельное растение или карабкаться вверх по опоре.",
                PlantCare = "Освещение: Предпочитает яркий рассеянный свет. В тени теряет яркость окраски (вариегатность), а прямые солнечные лучи могут оставить ожоги.\r\nПолив: Умеренный, после просыхания верхнего слоя субстрата примерно на 1/3. Не терпит застоя воды у корней.\r\nВлажность: Хорошо адаптируется к сухому воздуху, но любит регулярное опрыскивание и протирание листьев. При высокой влажности листья растут крупнее.\r\nОсобенности: Легко размножается черенками в воде. Важно помнить, что сок растения ядовит и может вызвать раздражение при попадании на кожу или слизистые.",
                PlantImage = "epipremnum.png",

                Care_Light = "Яркий рассеянный",
                Care_Watering = "Умеренный",
                Care_Temperature = "18–26°C",
                Care_Complexity = "Лёгкий",

                Feature_One = "Лиана",
                Feature_Two = "Быстро растёт",
                Feature_Three = "Очищает воздух",
                Feature_Four = "Токсичен",

                EstimatedHeight = 120,
                SearchNames = "Эпипремнум Epipremnum"
            },
            new Plant
            {
                Id = 8,
                NamePlant = "Замиокулькас",
                DescriptionPlant = "Замиокулькас (Zamioculcas) — харизматичный суккулент, получивший прозвище «долларовое дерево». Обладает глянцевыми темно-зелеными листьями и мощными клубнями, запасающими влагу, что делает его практически «неубиваемым» в домашних условиях.",
                PlantCare = "Освещение: Вынослив к любому свету. Идеально чувствует себя при ярком рассеянном освещении, но спокойно переносит полутень. На сильном солнце может получить ожоги.\r\nПолив: Главное правило — лучше недолить, чем перелить. Полив только после полного просыхания всего земляного кома. Зимой полив сокращают до минимума.\r\nВлажность: Абсолютно равнодушен к сухости воздуха. Опрыскивание не требуется, достаточно мыть под душем или протирать листья для блеска.\r\nОсобенности: Имеет очень мощную корневую систему, которая может деформировать или даже разорвать пластиковый горшок при нехватке места.",
                PlantImage = "zamioculcas.png",

                Care_Light = "Полутень",
                Care_Watering = "Редкий",
                Care_Temperature = "18–26°C",
                Care_Complexity = "Очень лёгкий",

                Feature_One = "Запасает воду",
                Feature_Two = "Не боится засухи",
                Feature_Three = "Медленный рост",
                Feature_Four = "Боится перелива",

                EstimatedHeight = 200,
                SearchNames = "Замиокулькас Zamioculcas"
            },
            new Plant
            {
                Id = 9,
                NamePlant = "Спатифиллум",
                DescriptionPlant = "Спатифиллум (Spathiphyllum) — изящное комнатное растение, широко известное как «Женское счастье». Ценится за элегантные белые соцветия-паруса и пышную ярко-зеленую листву. Является отличным природным фильтром для очистки воздуха.",
                PlantCare = "Освещение: Теневынослив, предпочитает мягкий рассеянный свет или полутень. Прямое солнце обжигает нежные листья, оставляя на них желтые пятна.\r\nПолив: Обильный в период цветения. Растение само «сигнализирует» о жажде, эффектно опуская листья. После полива тургор быстро восстанавливается.\r\nВлажность: Крайне влаголюбив. Требует частого опрыскивания, купания под душем или установки на поддон с влажным керамзитом.\r\nОсобенности: Не переносит сквозняков и застоя холодной воды в корнях. Для стимуляции цветения важно своевременно удалять старые пожелтевшие листья и увядшие цветоносы.",
                PlantImage = "spathiphyllum.png",

                Care_Light = "Полутень",
                Care_Watering = "Обильный",
                Care_Temperature = "18–25°C",
                Care_Complexity = "Средний",

                Feature_One = "Цветёт",
                Feature_Two = "Любит влагу",
                Feature_Three = "Очищает воздух",
                Feature_Four = "Сигналит о жажде",

                EstimatedHeight = 160,
                SearchNames = "Спатифиллум Spathiphyllum"
            });
                    

    }
    }
}
