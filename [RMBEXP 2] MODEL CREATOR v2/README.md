<h1>Обновленный плагин для блендера RMB (Import-Export) для добавления статик моделей в клиент R2.</h1>

<p>Поддерживается Blender 3.6 - 4.0 (в других не тестил)</p>

<blockquote>файл&nbsp;<a href="https://vk.com/away.php?to=http://io_scene_rmb.zip&amp;cc_key=" title="https://vk.com/away.php?to=http://io_scene_rmb.zip&amp;cc_key=">io_scene_rmb.zip</a>&nbsp;плагин<br />
файл&nbsp;<a href="https://vk.com/away.php?to=http://aion_cosa.zip&amp;cc_key=" title="https://vk.com/away.php?to=http://aion_cosa.zip&amp;cc_key=">aion_cosa.zip</a>&nbsp;готовая моделька с текстурами, нужно только закинуть в архивы и в базе прописать</blockquote>

<p>PS: автор изначального плагина не я, взял его отсюда&nbsp;<a href="https://vk.com/away.php?to=http%3A%2F%2Fforum.xentax.com%2Fviewtopic.php%3Ff%3D16%26t%3D3344&amp;cc_key=" rel="nofollow noopener" target="_blank">http://forum.xentax.com/viewtopic.php?f=16&amp;t=3344</a>&nbsp;(увы сайт подох так что ника автора не нашел)</p>

<p>Ссылка с веб архива с темой с форума Xentax:&nbsp;<a href="https://vk.com/away.php?to=http%3A%2F%2Fweb.archive.org%2Fweb%2F20210623115708%2Fhttps%3A%2F%2Fforum.xentax.com%2Fviewtopic.php%3Ff%3D16%26t%3D3344&amp;cc_key=" rel="nofollow noopener" target="_blank" title="http://web.archive.org/web/20210623115708/https://forum.xentax.com/viewtopic.php?f=16&amp;t=3344">тык</a></p>

<p><img alt="Обновленный плагин для блендера RMB (Import-Export) для добавления статик моделей в клиент R2., изображение №1" src="https://sun9-18.userapi.com/impg/mRQPiB0VwQ6C7ky-qujbuAxucEHNnz7w1teOhg/DgVt9myK2Iw.jpg?size=522x495&amp;quality=95&amp;sign=319f10d0d77e86597d9376266f5471d9&amp;type=album" /></p>

<p>Ссылка на видео превьюшку:&nbsp;<a href="https://vk.com/im?peers=c30&amp;sel=c25&amp;z=video610842470_456239022/958765df07eee4ffbe" title="https://vk.com/im?peers=c30&amp;sel=c25&amp;z=video610842470_456239022/958765df07eee4ffbe">тык</a></p>

<blockquote>Шаги которые нужно проделать, если ваша 3д модель, состоит не из треугольников, либо превышает poly count поддерживаемый клиентом r2 :<br />
Для моделек не из старых мморпг игр типа aion а к примеру из sketchfab нужно пройти пару доп этапов<br />
1. Понизить поликаунт<br />
2. Порезать по граням<br />
3. Конвертнуть в triangles если там есть quads<br />
для 1 есть модификатор в Blender называется Decimate<br />
для 2 и 3 вместе с плагинов идет туляка<br />
Check mesh type - выводит внизу в консоли тип, должен быть Tringle, если же Quad или Mixed то нужно поработать, порезать и конвернуть 2 и 3 шаг<br />
Split mesh by faces - режет модельку по граням, хак для плавильного наложения UV<br />
Convert mesh to triangle - собственно конвертирует в треугольники</blockquote>

<h3>Попробую как можно вкратце описать процесс, как пользоваться инструментом, так как сам столкнулся с сложностями с его использованием.</h3>

<blockquote>Для примера будет использоваться модель из игры Aion.<br />
Которая имеет формат .CGF</blockquote>

<p><img alt="Обновленный плагин для блендера RMB (Import-Export) для добавления статик моделей в клиент R2., изображение №2" src="https://sun9-74.userapi.com/impg/ZYUNsy07-aOIxw2RIXxdJa9oyTHrJrZJAOnKLg/bmj7VC3yCFM.jpg?size=372x494&amp;quality=95&amp;sign=9c551b2a6a56d475152af0dc579deed4&amp;type=album" /></p>

<blockquote>Для этого вам нужна крякнутая версия программы 3D Object converter, с её помощью вы сможете открыть почти любую 3д модель. (Архив с программой прикреплю к посту)<br />
Она нам нужна для экспорта 3д модели в формат Wavefront (.Obj)<br />
Для того чтобы сделать экспорт, жмете File &rarr;Save as и выбираете формат в котором хотите экспортировать модель, листайте в самый низ списка пока не увидите Wavefront (*.obj)</blockquote>

<p>Далее, вам нужно установить addon для Blender, я использую версию 4.0 Которая есть в&nbsp;<a href="https://vk.com/away.php?to=https%3A%2F%2Fstore.steampowered.com%2Fapp%2F365670%2FBlender%2F&amp;cc_key=" rel="nofollow noopener" target="_blank">стиме</a>, бесплатно.</p>

<p>Устанавливаете blender, затем открываете папку куда установили, ( правой по программе в стиме &rarr; Свойства &rarr; Установленные файлы &rarr; Обзор</p>

<blockquote>X:\SteamLibrary\steamapps\common\Blender\4.0\scripts\addons</blockquote>

<p>Находите эту папку и открываете, затем распаковываете в эту папку архив который закреплен под постом io_scene_rmb</p>

<p><img alt="Обновленный плагин для блендера RMB (Import-Export) для добавления статик моделей в клиент R2., изображение №3" src="https://sun9-43.userapi.com/impg/T--J6QTCA5GlakIJD3q09P0oCe_O_x2ZHyp2Og/5yx6T2w0JYo.jpg?size=547x604&amp;quality=95&amp;sign=d4af81b4f5923ecd12c212999ab8c138&amp;type=album" /></p>

<p>Должно получиться вот так.</p>

<p>Далее открываем Blender &rarr; Edit &rarr; Preferences</p>

<p>Переходим во вкладку Addons &rarr; Install</p>

<p><img alt="Обновленный плагин для блендера RMB (Import-Export) для добавления статик моделей в клиент R2., изображение №4" src="https://sun9-62.userapi.com/impg/y6_gSdSLydZ68n59witMqw3ZvRWnJ9IEyp5slA/q2c1k7hCrCA.jpg?size=667x231&amp;quality=95&amp;sign=96f0f9797c310130a0bc1ca97ec5e1e6&amp;type=album" /></p>

<p>Откроется окно, переходите к папке куда разархивировали архив и выбираете io_scene_rmb &rarr; Install addon. На этом установка аддона завершена</p>

<p><img alt="Обновленный плагин для блендера RMB (Import-Export) для добавления статик моделей в клиент R2., изображение №5" src="https://sun9-58.userapi.com/impg/KRAUZ7Reh25m5g2d-d8LNspt6--NEKujZsE4rg/Aead5D8oYPU.jpg?size=807x411&amp;quality=95&amp;sign=5e2dd22539cdfbbd9f9504afbd78eebb&amp;type=album" /></p>

<p>Далее, в идеале вам нужно распаковать архив Model.rfs и Textures.rfs (текстуры опционально)</p>

<blockquote>На будущее из архива Model.rfs вам нужно выбрать по одной пушке каждого вида, которые есть в R2 online, можете ориентироваться по картинке ниже, распакованный архив model.rfs так же пригодится, чтобы вы смотрели какие ID предметов у вас уже заняты в клиенте, чтобы не поломать существующие предметы, возьмем на пример Алебарды, свободный id под алебарду в любом клиенте будет i009016 (В конце зайда прикреплю готовую алебарду, именно под этот id.) Так что последующие алебарды добавляйте уже под номером i009017, и тд.</blockquote>

<p><img alt="Обновленный плагин для блендера RMB (Import-Export) для добавления статик моделей в клиент R2., изображение №6" src="https://sun9-48.userapi.com/impg/GbJLwgyzpNogkB5j95yiecZZuytkMMUZX1oK8w/w6VFt6eT1YU.jpg?size=286x211&amp;quality=95&amp;sign=e0c0567799f9867f6a236723ac0277fd&amp;type=album" /></p>

<blockquote>И так вы выбрали пушку, скопируйте rmb файл, и её текстовый файл куда нибудь в отдельную папку, для удобства.</blockquote>

<p>Теперь, открываете 2 окна программы Blender, в одной открываете оригинальную пушку например Дамасскую Алебарду)</p>

<p><img alt="Обновленный плагин для блендера RMB (Import-Export) для добавления статик моделей в клиент R2., изображение №7" src="https://sun9-10.userapi.com/impg/Sc9jQMOleOBpwo8XEC_UcZC_DctBK5lJKpKBKw/DJb4FZjXes4.jpg?size=807x426&amp;quality=95&amp;sign=0e154a3ea87b3b14e1ec79a8692e8eda&amp;type=album" /></p>

<p>Как видите оригинальная модель алебарды открываете вот с такими параметрами. Rotation X 90&ordm;<br />
Scale 1000 по всем 3 осям.</p>

<p>и слева Dimension X Y Z это размер игровой модели.</p>

<blockquote>В дальнейшем вы будете ориентироваться на эти цифры, чтобы подогнать новую модель алебарды под &plusmn; похожие размеры, чтобы она хорошо выглядела в руках у персонажа, и в целом подходила под анимации R2.</blockquote>

<p>Теперь переходим во второе окно Blender&#39;a, и удаляем оттуда всё что автоматически создается когда открываешь новую сцену в Blender,<br />
у вас должна быть абсолютно пустая сцена, как на скриншоте.</p>

<p><img alt="Обновленный плагин для блендера RMB (Import-Export) для добавления статик моделей в клиент R2., изображение №8" src="https://sun9-36.userapi.com/impg/gjwZuazvAHkX5Yj6JAsQIflb4tsw-xzkpSdzsw/3EeyKaehCsk.jpg?size=807x406&amp;quality=95&amp;sign=3d07f61c4e18d23e7b0e3bfb33fc3500&amp;type=album" /></p>

<p>Теперь импортируем нашу новую алебарду которая в .Obj формате.</p>

<blockquote>Ещё одно замечание* Не помню как по дефолту в блендере устроено, но у вас должно быть так Forward Axis -Z, Up Axis Y</blockquote>

<p><img alt="Обновленный плагин для блендера RMB (Import-Export) для добавления статик моделей в клиент R2., изображение №9" src="https://sun9-79.userapi.com/impg/apNk7_AiOBMzOmoYlxj7OzbKq5o4SikEbSM8hw/c_LUyr2R_CQ.jpg?size=289x201&amp;quality=95&amp;sign=8282335fd7b5f0c0d6b9a8c7f24c0ac8&amp;type=album" /></p>

<p>File&rarr; Import&rarr; Wavefront (.obj) &rarr; выбираете алебарду &rarr; Import Wavefront OBJ</p>

<p><img alt="Обновленный плагин для блендера RMB (Import-Export) для добавления статик моделей в клиент R2., изображение №10" src="https://sun9-59.userapi.com/impg/xmvjcKCVZsr-cLOA0zLsv-GRmHElOxB2kfsHeQ/yDfV6N332Tg.jpg?size=807x459&amp;quality=95&amp;sign=33082c362cd8d2e123e8d44851621821&amp;type=album" /></p>

<blockquote>Чтобы открыть окно Transform, чтобы увидеть размер алебарды нажмите клавишу N</blockquote>

<p>Как видите, всё так же как и с оригинальной алебардой, если судить по Rotation X 90&ordm;<br />
и Scale 1000</p>

<p>Но сама алебарда расположена на сцене иначе, вам нужно повернуть её так же как оригинальную алебарду на сцене.</p>

<blockquote>Жмете клавишу A чтобы выбрать всё что есть на сцене,</blockquote>

<p><img alt="Обновленный плагин для блендера RMB (Import-Export) для добавления статик моделей в клиент R2., изображение №11" src="https://sun9-74.userapi.com/impg/F8441l1u5J4AFcpI0tVknb5s4YGamnaCyHNQFg/6crh18UW2cY.jpg?size=807x321&amp;quality=95&amp;sign=f691c20fe2f28ce2a80c20e977c2fdad&amp;type=album" /></p>

<p>Жмете Rotate, и поворачиваете её с 90&ordm; на 0&ordm; градусов по оси Х вручную, либо справа можете просто вбить число 0 вместо 90, и она сама повернется.</p>

<p>Теперь над надо повернуть её на бок, для этого меняем ось Y с 0 на -90&ordm;</p>

<p><img alt="Должно получиться как на скриншоте" src="https://sun9-37.userapi.com/impg/di95fjoHAt_hN_e8vSlgQaHV-BPKDA0sXU5gAQ/kaudNdKZ570.jpg?size=807x240&amp;quality=95&amp;sign=7a53832af35819ffd2e9da4ef12b97ba&amp;type=album" /></p>

<p>Должно получиться как на скриншоте</p>

<p>Теперь, нужно подогнать модель новой алебарды, под размеры для примера мы берем размер Дамасскую алебарду с игры.</p>

<p><img alt="Оригинальные размеры модели с айона" src="https://sun9-4.userapi.com/impg/oMYAcdR1Q1I4O55AYUwVEa2mpSiJbfuv4DEm1A/wpBQkU7dQVc.jpg?size=807x174&amp;quality=95&amp;sign=2e07163c6c40e3e9b114e9e4aa720540&amp;type=album" /></p>

<p>Оригинальные размеры модели с айона</p>

<p><img alt="Измененные размеры, под нужды р2." src="https://sun9-71.userapi.com/impg/VjtkTp191EqPZWCqXte1suf3tFa8qDMtcowHDA/vNzHpV_KP8k.jpg?size=807x275&amp;quality=95&amp;sign=db4c2e2f80618ddafb087ef7660d0843&amp;type=album" /></p>

<p>Измененные размеры, под нужды р2.</p>

<p>Теперь жмем клавишу A и переходим в режим &laquo;Move&raquo;</p>

<p><img alt="Обновленный плагин для блендера RMB (Import-Export) для добавления статик моделей в клиент R2., изображение №15" src="https://sun9-28.userapi.com/impg/HSQeSgVeBpdTLNLjfs0GvgrewnUxcCkQV5Bwvw/rfL4F3H17gE.jpg?size=807x503&amp;quality=95&amp;sign=35c197c3ebafdfac77b7eedd328f7ea8&amp;type=album" /></p>

<p>Теперь наводимся на зеленую стрелочку, зажимаем левую кнопку мыши, и тянем в правую сторону, пока середина алебарды не окажется в центре сцены то есть на нулевой позиции.</p>

<p><img alt="Обновленный плагин для блендера RMB (Import-Export) для добавления статик моделей в клиент R2., изображение №16" src="https://sun9-14.userapi.com/impg/8VkPmsOy9x863s_gHboxvD81bKZXmOveQ3NMiA/r-xkhBA16A4.jpg?size=807x502&amp;quality=95&amp;sign=49ec2c7a22b547e3c1f56be6dceddc4b&amp;type=album" /></p>

<p><img alt="Обновленный плагин для блендера RMB (Import-Export) для добавления статик моделей в клиент R2., изображение №17" src="https://sun9-50.userapi.com/impg/_dCv2eYHZG_qWyl8sx6fI0N3gBxqpctmUHMVVg/osLCpRLF13M.jpg?size=807x279&amp;quality=95&amp;sign=4b004727e197f7dfe7ef18735dae8cda&amp;type=album" /></p>

<p>Теперь как видите, после всех манипуляций с алебардой, location, rotation, scale показывают не нужные нам значения, чтобы вернуть всё опять на 90&ordm; по оси X, и Scale 1000, берем просто напросто экспортируем нашу алебарду в Obj формат</p>

<blockquote>File &rarr; Export &rarr; Wavefront (.obj). И сохраняете её в любое удобное для вас место (Можете заменить им изначальный Obj, который использовали впервые)</blockquote>

<blockquote>(При экспорте ничего менять не надо, оставляете всё как есть) теперь удаляете алебарду со сцены, жмете File &rarr; Import &rarr; Wavefront (.obj) и выбираете алебарду.</blockquote>

<p><img alt="Обновленный плагин для блендера RMB (Import-Export) для добавления статик моделей в клиент R2., изображение №18" src="https://sun9-38.userapi.com/impg/D74xTwg7hhtaClDSKR3lc8E8w3EVnBAYwhqsSA/f0-bvDBtjxM.jpg?size=807x270&amp;quality=95&amp;sign=986c9c0104c3c57818c0c73b80d9014f&amp;type=album" /></p>

<p>Как видите после импорта все значения вернулись к дефолтным, а размер алебарды остался тем, который мы задавали ранее.</p>

<p>Теперь, открываете папку где у вас лежит модель которую вы хотите добавить в клиент, и текстуры к ней.</p>

<p><img alt="Обновленный плагин для блендера RMB (Import-Export) для добавления статик моделей в клиент R2., изображение №19" src="https://sun9-39.userapi.com/impg/6wBWxafieZeUPter6pRnpeLRckdIl0Q1EXqZhQ/gcpyY_NHPpY.jpg?size=386x449&amp;quality=95&amp;sign=387c40b73197a4cedbfc477e8f2d236e&amp;type=album" /></p>

<blockquote>У вашей модели должно быть 3 карты текстур. это Diffuse, Normal map, Specular map. Спутать их сложно, выглядят они всегда примерно одинаково, и обычно имеют приписку _n (для карты нормалей) и _sp (для карты отражений) ну и diffuse map (собственно сама текстура модели)</blockquote>

<p>Теперь вам нужно открыть все эти три текстуры в фотошопе, (обычно модифицировать ничего не нужно) у вас просто должен быть установлен плагин для работы с dds текстурами от Nvidia. либо ищие аналоги.</p>

<p>Начнем с основной текстуры, после открытия её в фотошопе, жмете File&rarr; Save as &rarr; .DDS</p>

<p><img alt="Обновленный плагин для блендера RMB (Import-Export) для добавления статик моделей в клиент R2., изображение №20" src="https://sun9-17.userapi.com/impg/REmmYqM5pctNFcTldPmNbVjrfrfXgEIxy-RztA/g1L0Os-6ynI.jpg?size=807x95&amp;quality=95&amp;sign=268c7ea6b78005cca9de310b23ff566c&amp;type=album" /></p>

<p>и называете её так же как называется ваш будущий .rmb файл для примера возьмем i009016</p>

<p>Теперь, возвращаетесь в фотошоп, и жмете Image&rarr; Image size</p>

<p><img alt="Обновленный плагин для блендера RMB (Import-Export) для добавления статик моделей в клиент R2., изображение №21" src="https://sun9-14.userapi.com/impg/2FyzghkljnQ2sjRckFU_p5Pyg9YtcihaKM7qtg/0i2GR5Wi6KQ.jpg?size=653x364&amp;quality=95&amp;sign=bd2512f7f01235b0bb3783f7610cca2b&amp;type=album" /></p>

<blockquote>Предположим у вас текстура 1к (1024х1024) пикселей. Вводите 512 и жмите ОК<br />
Затем так же сохраняете эту текстуру, но уже с названием i009016_1 (Это вторая текстура, которую использует клиент, для низкой настройки графики, поэтому размер ей нужен не большой, обычно это 512/256 пикселей.)</blockquote>

<p>Проделываете аналогичные действия с картой нормалей, и с картой отражений.</p>

<p><img alt="Обновленный плагин для блендера RMB (Import-Export) для добавления статик моделей в клиент R2., изображение №22" src="https://sun9-79.userapi.com/impg/6_WSYwy4-fvHfjy6FdMRqvAuQts7INg4ds3AYQ/44-25vo2Y6g.jpg?size=397x559&amp;quality=95&amp;sign=9f5e100992cd34c1d60fe9b40605d1f0&amp;type=album" /></p>

<p>Должно получиться так.</p>

<h3>Возвращаемся в Blender, к нашей алебарде.</h3>

<p>Если у вас нет материала у модели, создаете новый, но обычно должнен быть, из старой игры или еще откуда то</p>

<p><img alt="Обновленный плагин для блендера RMB (Import-Export) для добавления статик моделей в клиент R2., изображение №23" src="https://sun9-49.userapi.com/impg/hDk1NcmDM675WsRz6HznKdwuyjpKDgH2MAZ78g/_wBXnuliL54.jpg?size=807x442&amp;quality=95&amp;sign=28647e55a41b27c9202996f736504ad2&amp;type=album" /></p>

<p><img alt="Обновленный плагин для блендера RMB (Import-Export) для добавления статик моделей в клиент R2., изображение №24" src="https://sun9-16.userapi.com/impg/mUhQ9QlkoHAnWbXJ48OgWihflQ8SxR4xje56LQ/IXTxUdmkfX0.jpg?size=338x600&amp;quality=95&amp;sign=2e60e7b95b66d2e70e222dd98c21be67&amp;type=album" /></p>

<p>Напротив Surface, нажимаете на этот синий треугольник, у вас откроется меню, как на скриншоте.<br />
Выбираете там Principled BSDF</p>

<p>Переходите во владку Shading</p>

<p><img alt="Обновленный плагин для блендера RMB (Import-Export) для добавления статик моделей в клиент R2., изображение №25" src="https://sun9-64.userapi.com/impg/sYGLo9Pvi2YoEn1OuIHmkXSR4KanJu1tVvhf2w/Qtp21HoWaGg.jpg?size=807x516&amp;quality=95&amp;sign=7e7fca3e67f3a8cd35f02bbca080f5a7&amp;type=album" /></p>

<p>Как видите здесь уже есть ддс текстура, которая подключена к Principled BSDF, удаляете то что выделено красным, если там что то есть.</p>

<p>Открываете папку где у вас лежат текстуры к алебарде, и перетягиваете её из папки на обозаченое место в Blender.</p>

<p>Вот так</p>

<p><img alt="Обновленный плагин для блендера RMB (Import-Export) для добавления статик моделей в клиент R2., изображение №26" src="https://sun9-59.userapi.com/impg/MaNksYPqmyjY2Yg45UmcC66lhcuZX9Lrtxt0xg/Xm_2e4yEH6A.jpg?size=784x285&amp;quality=95&amp;sign=0d1dedc38dd757ab5852cd2df9903725&amp;type=album" /></p>

<p>И подключаете Color к Base color.</p>

<p><img alt="Обновленный плагин для блендера RMB (Import-Export) для добавления статик моделей в клиент R2., изображение №27" src="https://sun9-44.userapi.com/impg/l9W4jMjDL0k153PzUThQjkXvlpRyI6lUr-qHkw/4Bhr7j-M6pg.jpg?size=807x739&amp;quality=95&amp;sign=caa2b61c5f4e325275c75e4d09f7fb5d&amp;type=album" /></p>

<p>Это нужно для того чтобы вручную не сидеть и не редактировать rmb файл вручную, что долго и кропотно.</p>

<blockquote>Проделав эти действия, аддон (плагин) сам добавить в rmb файл, название текстуры которую должен использовать клиент.</blockquote>

<p>Финишная прямая, теперь в Blender&#39;e жмете<br />
File&rarr; Export&rarr; RMB и выбираете путь там где у вас лежат текстуры и текстовый файл нужный для клиента, чтобы он понимал какой тип оружия, вы добавили.</p>

<p><img alt="Обновленный плагин для блендера RMB (Import-Export) для добавления статик моделей в клиент R2., изображение №28" src="https://sun9-6.userapi.com/impg/eKVGxI4amgQwuYkIxnTEhYZ_mufpyw77ENfEgQ/xkhWkVoKmkc.jpg?size=332x258&amp;quality=95&amp;sign=766fbeb113d48385548eaa0f89ca25e6&amp;type=album" /></p>

<p>Вот так выглядит текстовый файл для нашей алебарды.<br />
Теперь дело за малым, раскидать файлы по архивам клиента, и добавить себе в базу новый предмет, и указать для него item resource, чтобы клиент знал какую модель и иконку подгружать.</p>

<p><img alt="Обновленный плагин для блендера RMB (Import-Export) для добавления статик моделей в клиент R2., изображение №29" src="https://sun9-76.userapi.com/impg/oVwxOVxoXjIr4vvIzQ1ZG-IOxmzAS8_VuUbraw/LY7OvfqIAsA.jpg?size=807x536&amp;quality=95&amp;sign=7f9cd17b1a32aed185374197889f696d&amp;type=album" /></p>
