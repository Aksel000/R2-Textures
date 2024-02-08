<h1 class="article_decoration_first article_decoration_last">Инструмент для добавления статик моделей в клиент (оружия).<span id="instrument-dlya-dobavlenia-statik-modeley-v-klient-oruzhia" class="article_anchor_button"></span></h1>
<h3 class="article_decoration_first article_decoration_last">Експорт / импорт для файлов .rmb<span id="export-import-dlya-faylov-rmb" class="article_anchor_button"></span></h3>
<p class="article_decoration_first">Для запуска нужен&nbsp;<strong class="">Node.js</strong></p>
<p class="">Если у вас в системе еще нет Нода - заходим на сайт:</p>
<p class=""><a href="https://vk.com/away.php?to=https%3A%2F%2Fnodejs.org&amp;cc_key=" target="_blank" rel="nofollow noopener">https://nodejs.org</a></p>
<p class="">скачиваем рекомендованную версию (левый зеленый квадрат), после устанавливаем в систему.</p>
<p class=""><strong class="">Возможно два режима:</strong></p>
<p class="">1) Експорт моделей из&nbsp;<strong class="">.rmb</strong>&nbsp;в геометрию&nbsp;<strong class="">.obj</strong></p>
<p class=" article_decoration_last">Запуск из командной строки:</p>
<blockquote class="article_decoration_first article_decoration_last">node conv.js [файл.рмб]</blockquote>
<p class="article_decoration_first article_decoration_last"><em class="">пример:</em></p>
<blockquote class="article_decoration_first article_decoration_last">node conv.js i009006.rmb</blockquote>
<p class="article_decoration_first">результат: в данной директории будут созданы файлы</p>
<p class=""><strong class=""><em class="">i009006.obj</em></strong></p>
<p class="">2) Импорт геометрии из<strong class="">&nbsp;.obj</strong>&nbsp;в меш структуру&nbsp;<strong class="">.rmb</strong></p>
<p class=" article_decoration_last">Запуск из командной строки:</p>
<blockquote class="article_decoration_first article_decoration_last">node conv.js [файл.рмб] [файл.obj]</blockquote>
<p class="article_decoration_first article_decoration_last"><em class="">пример:</em></p>
<blockquote class="article_decoration_first article_decoration_last">node conv.js i009006.rmb i009006.obj</blockquote>
<p class="article_decoration_first">в результате будет создан файл&nbsp;<strong class=""><em class="">i009006.new.rmb</em></strong>&nbsp;в котором в качестве меша будет залита геометрия из&nbsp;<strong class="">.obj</strong></p>
<p class=" article_decoration_last">Примеры запуска даю в cmd файлах.</p>
<h3 class="article_decoration_first article_decoration_last">Важные аспекты:<span id="vazhnye-aspekty" class="article_anchor_button"></span></h3>
<ul class="article_decoration_first article-list">
<li class="article-list__item">только геометрия обьекта, без костей, весов, анимации и прочих эффектов;</li>
</ul>
<ul class=" article-list">
<li class="article-list__item">при сохранении&nbsp;<strong class="">.obj</strong>&nbsp;в 3д редактора нужно выбирать галочку - записывать нормали, если точнее:&nbsp;<strong class="">Only currnet object, Write normals, Include UV</strong>&nbsp;(НЕ нужны - модификаторы, анимация, nurbs и прочее);</li>
</ul>
<ul class=" article-list">
<li class="article-list__item">геометрия должна быть треугольной, с сеткой UV и нормалями;</li>
</ul>
<ul class=" article_decoration_last article-list">
<li class="article-list__item">Важно структура меша должна быть триангулирована, для красоты желательно равномерность полигонов и сетки;</li>
</ul>
<p class="article_decoration_first article_decoration_last"><strong class="">Предполагаемая схема работы:</strong></p>
<ol class="article_decoration_first article-list" start="1">
<li class="article-list__item">Берем нужный файл&nbsp;<strong class="">.rmb</strong>;</li>
</ol>
<ol class=" article-list" start="2">
<li class="article-list__item">Екпортируем меш из него;</li>
</ol>
<ol class=" article-list" start="3">
<li class="article-list__item">Редактируем в 3д редакторе (как вариант подходит Blender);</li>
</ol>
<ol class=" article-list" start="4">
<li class="article-list__item">Сохраняем модель из редакторе в&nbsp;<strong class="">.obj</strong>, не забываем выставить параметры:&nbsp;<strong class="">Only currnet object, Write normals, Include UV</strong>;</li>
</ol>
<ol class=" article-list" start="5">
<li class="article-list__item">Импортируем геометрию в меш;</li>
</ol>
<ol class=" article_decoration_last article-list" start="6">
<li class="article-list__item">Проверяем в игре что вышло;</li>
</ol>