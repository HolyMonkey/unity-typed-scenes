# Typed Scenes
Инструментарий для передачи данных между сценами. Он предоставляет строго-типизированные обёртки для Unity сцен через которые можно комфортно загружать сцены и передавать им данные для работы.

Плагин разработан учениками школы программирования ЯЮниор - https://ijunior.ru/

## Как установить?

Пакет представляет из себя плагин для Unity с открытым исходным кодом. Вам нужно перенести папку Plugins из репозитория в свой Unity проект. 

![Adding plugins folder](https://lk.ijunior.ru/external/typed-scenes/DropPlugins.gif)

После установки у вас может появится ошибка о том, что наш пакет не может обнаружить System.CodeDom.
Чтобы это исправить вам нужно изменить версию .Net для проекта, по умолчанию стоит 2.0, вам нужно выбрать 4.0.

![Change API Level to 4.0](https://lk.ijunior.ru/external/typed-scenes/ApiLvl.png)

После этого всё готово к работе. 

## Как пользоваться?

Пакет самостоятельно генерирует классы-обёртки над сценами в Unity. Вам достаточно добавить сцену в проект и всё произойдёт само. 
Если в вашем проекте уже есть сцены то их достаточно реимпортировать. 

![Change API Level to 4.0](https://lk.ijunior.ru/external/typed-scenes/IncludeScenes.png)

Теперь вы можете запускать сцены через сгенерированные классы. 

```
using UnityEngine;
using IJunior.TypedScenes;

public class Menu : MonoBehaviour
{
    private void OnPlayButtonClick()
    {
        Game.Load();
    }
}
```

## Как передавать данные?

Основная идея этого компонента в том, что у сцены может быть некоторая модель для загрузки\отображения и что бы запустить сцену нужно ОБЯЗАТЕЛЬНО передать объект в корректном состояние.

Во-первых вам нужно задать точку входа в сцене, т.е некоторые код который будет обрабатывать её загрузку. 

```
using IJunior.TypedScenes;
using UnityEngine;

public class StringHandler : MonoBehaviour, ISceneLoadHandler<string>
{
    public void OnSceneLoaded(string argument)
    {
        Debug.Log(argument);
    }
}
```

Для этого вам нужно создать любой компонент реализующий интерфейс ISceneLoadHandler<T>. В качестве T вы указываете те данные которые нужны сцене для запуска.
Наш инструмент сам добавить необходимый метод Load в класс сцены который будет принимать в качестве аргумента подходящие данные. 
После добавления указанного компонента на сцену Game, в класс-сцены появится метод Load(string argument)
После его вызова запустится сцена Game а у всех компонентов которые реализуют ISceneLoadHandler<string> вызовется метод OnSceneLoaded. 

```
using UnityEngine;
using IJunior.TypedScenes;

public class Menu : MonoBehaviour
{
    private void OnPlayButtonClick()
    {
        Game.Load("Room Name");
    }
}
```

