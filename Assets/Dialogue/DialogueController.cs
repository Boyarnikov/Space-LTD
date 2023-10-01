using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public enum Actors
{
    NONE = -1,
    GIRL1 = 0,
}

public enum ActorPosition
{
    FIXED = -1,
    AUTO = 0,
    RIGHT = 1,
    LEFT = 2,
}

public class PhraseDispellerData
{
    public bool use_dispeller;
    public Vector2 dispeller_target_position;
    public Vector2 dispeller_target_size;

    public PhraseDispellerData()
    {
        use_dispeller = false;
    }

    public PhraseDispellerData(bool _use_dispeller)
    {
        use_dispeller = _use_dispeller;
    }

    public PhraseDispellerData(bool _use_dispeller, Vector2 _dispeller_target_position, Vector2 _dispeller_target_size)
    {
        use_dispeller = _use_dispeller;
        dispeller_target_position = _dispeller_target_position;
        dispeller_target_size = _dispeller_target_size;
    }
}

public class PhraseActorData
{
    public Actors actor;
    public bool active; 
    public int actor_state;
    public ActorPosition actor_position;

    public PhraseActorData()
    {
        actor = Actors.NONE;
        actor_state = -1;
        actor_position = ActorPosition.FIXED;
        active = false;
    }

    public PhraseActorData(Actors _actor, bool _active, int _actor_state, ActorPosition _actor_position)
    {
        active = _active;
        actor = _actor;
        actor_state = _actor_state;
        actor_position = _actor_position;
    }
}

public class Phrase
{
    public string phrase;
    public PhraseActorData actor_data;
    public PhraseDispellerData dispeller_data;

    public Phrase() {
        phrase = "";
        dispeller_data = new PhraseDispellerData();
        actor_data = new PhraseActorData();
    }

    public Phrase(string _phrase, PhraseActorData _actor_data, PhraseDispellerData _dispeller_data)
    {
        phrase = _phrase;
        actor_data = _actor_data;
        dispeller_data = _dispeller_data;
    }
}
public class Dialogue
{
    public Phrase[] phrases;
    public int place;
    public Func<float> hook;

    public Dialogue(ref Phrase[] _phrases)
    {
        place = -1;
        phrases = _phrases;
    }

    public Phrase Next()
    {
        place++;
        if (place < phrases.Length)
        {
            return phrases[place];
        }
        else {
            place = -1;
            return null;
        }
    }
}
 
public class DialogueController : MonoBehaviour
{
    [SerializeField] GameObject[] actors_prefabs;
    [SerializeField] GameObject cover_controller_prefab;
    [SerializeField] GameObject text_controller_prefab;
    [SerializeField] DialogueDispellerTarget[] targets;
    CoverController cover_controller;
    TextWriter text_controller;
    List<AvatarController> actors_controllers;
    bool in_dialogue;
    int dialog_number = 0;
    Dialogue selected_dialogue;
    void Start()
    {
        actors_controllers = new List<AvatarController>();
        in_dialogue = false;
        InitCover();
        InitText();
        InitActors();
    }

    public void InitDialogsTest() {
        Phrase[] phrases = {
            new Phrase("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.LEFT), new PhraseDispellerData(true, new Vector2(0, 0), new Vector2(4, 4))),
            new Phrase("Now I Dont Active", new PhraseActorData(Actors.GIRL1, false, -2, ActorPosition.RIGHT), new PhraseDispellerData(true, new Vector2(2, 2), new Vector2(3, 2))),
            new Phrase("Now I Active again", new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.RIGHT), new PhraseDispellerData(true, new Vector2(-1, -1), new Vector2(2, 3))),
        };
        Dialogue dialog = new Dialogue(ref phrases);
    }

    // 0 - конвеер
    // 1 - конвеер_коунтер
    // 2 - поле
    // 3 - такс
    // 4 - таск каунтер
    // 5 - 1й рецепт
    // 6 - 2й рецепт
    // 7 - 3й рецепт
    // 8 - 4й рецепт

    public Dialogue InitDialogs0()
    {
        string[] text = {
            "вы инженер стройте блоки чтобы отправлять их в колонии",
            "сверху конвеер ня",
            "а это сборочная площадка",
            "чтобы перетаскивать блоки можно использовать лкм",
            "а чтобы вращать - пкм. Попробуйте ня",
        };
        Phrase[] phrases = {
            new Phrase(text[0], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.LEFT), new PhraseDispellerData(false, new Vector2(0, 0), new Vector2(0, 0))),
            new Phrase(text[1], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(true, targets[0].position , targets[0].scale)),
            new Phrase(text[2], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(true, targets[2].position , targets[2].scale)),
            new Phrase(text[3], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(false, targets[2].position , targets[2].scale)),
            new Phrase(text[4], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(false, targets[2].position , targets[2].scale)),
        };
        return new Dialogue(ref phrases);
    }

    public Dialogue InitDialogs1()
    {
        string[] text = {
            "сё кайфово ня",
            "справа есть задания их надо выполнять",
            "чтобы выполнить задание нужно перенести его блюпринт на готовую структуру",
            "блюпринты тоже можно врощать! попробуйте ня",
        };
        Phrase[] phrases = {
            new Phrase(text[0], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.LEFT), new PhraseDispellerData(false, new Vector2(0, 0), new Vector2(0, 0))),
            new Phrase(text[1], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(true, targets[3].position , targets[3].scale)),
            new Phrase(text[2], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(true, targets[2].position , targets[2].scale)),
            new Phrase(text[3], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(true, targets[2].position , new Vector2(0, 0))),
        };
        return new Dialogue(ref phrases);
    }

    public Dialogue InitDialogs2()
    {
        string[] text = {
            "эти кружочки показывают сколько у вас заданий в запасе",
            "ваша задача как инженера блоков выполнить все задания",
            "приступем!",
        };
        Phrase[] phrases = {
            new Phrase(text[0], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.LEFT), new PhraseDispellerData(true, targets[4].position , targets[4].scale)),
            new Phrase(text[1], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(false, targets[4].position , targets[4].scale)),
            new Phrase(text[2], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(true, targets[4].position , new Vector2(0, 0))),
        };
        return new Dialogue(ref phrases);
    }

    public Dialogue InitDialogs3()
    {
        string[] text = {
            "отличная работа!",
            "в дальнейшем не забывайте что чтобы скипать диалоги можно использовать пкм",
        };
        Phrase[] phrases = {
            new Phrase(text[0], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.LEFT), new PhraseDispellerData(false, new Vector2(0, 0), new Vector2(0, 0))),
            new Phrase(text[1], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(false, new Vector2(0, 0), new Vector2(0, 0))),
        };
        return new Dialogue(ref phrases);
    }

    public Dialogue InitDialogs4()
    {
        string[] text = {
            "ух, как хорошо получить ангар по-больше",
            "в прошлый раз мы использовали только жилые блоки",
            "в этот раз будем строить ещё и парки, они полезны для людей блаблабла",
            "Поэтору некоторые заказы требуют оба вида блоков",
            "не забывайте что вращать блоки можно нажимая на пкм, даже если вы их не переносите!",
        };
        Phrase[] phrases = {
            new Phrase(text[0], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.LEFT), new PhraseDispellerData(true, targets[2].position , targets[2].scale - new Vector2(2, 2))),
            new Phrase(text[1], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(false, new Vector2(0, 0), new Vector2(0, 0))),
            new Phrase(text[2], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(true, targets[3].position , targets[3].scale)),
            new Phrase(text[3], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(false, new Vector2(0, 0), new Vector2(0, 0))),
            new Phrase(text[4], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(true, targets[3].position, new Vector2(0, 0))),
        };
        return new Dialogue(ref phrases);
    }

    public Dialogue InitDialogs5()
    {
        string[] text = {
            "Экология очень важна блаблабла",
        };
        Phrase[] phrases = {
            new Phrase(text[0], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(false, new Vector2(0, 0), new Vector2(0, 0))),
        };
        return new Dialogue(ref phrases);
    }

    public Dialogue InitDialogs6()
    {
        string[] text = {
            "Колониям так же нужны батареи и прочие ютилити чтобы выживать",
            "Собирая конструкции таким образом мы можем упростить постройку колоний на всём марсе!",
        };
        Phrase[] phrases = {
            new Phrase(text[0], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.LEFT), new PhraseDispellerData(false, new Vector2(0, 0), new Vector2(0, 0))),
            new Phrase(text[1], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(false, new Vector2(0, 0), new Vector2(0, 0))),
        };
        return new Dialogue(ref phrases);
    }

    public Dialogue InitDialogs7()
    {
        string[] text = {
            "заказы модулей всегда ориентируются на неровную местность марса",
            "поэтому часто их сложно собрать из уже имеющихся модулей",
            "иногда бывает полезно утилизировать уже установленные тайлы",
            "взывчатку можно получить выполнив заказ на модуль для компании динамит(с)",
            "не забывайте что вращать блоки можно нажимая на пкм, даже если вы их не переносите!",
        };
        Phrase[] phrases = {
            new Phrase(text[0], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.RIGHT), new PhraseDispellerData(true, targets[5].position , targets[5].scale)),
            new Phrase(text[1], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.RIGHT), new PhraseDispellerData(false, new Vector2(0, 0), new Vector2(0, 0))),
            new Phrase(text[2], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.RIGHT), new PhraseDispellerData(false, targets[3].position , targets[3].scale)),
            new Phrase(text[3], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.RIGHT), new PhraseDispellerData(false, new Vector2(0, 0), new Vector2(0, 0))),
            new Phrase(text[4], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.RIGHT), new PhraseDispellerData(false, targets[3].position, new Vector2(0, 0))),
        };
        return new Dialogue(ref phrases);
    }

    public Dialogue InitDialogs8()
    {
        string[] text = {
            "когда вы выполняете спецзаказ - тайлы оказываются в очереди конвеера",
            "освободите конвеер чтобы получить к нему доступ",
        };
        Phrase[] phrases = {
            new Phrase(text[0], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.RIGHT), new PhraseDispellerData(true, targets[1].position , targets[1].scale)),
            new Phrase(text[1], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.RIGHT), new PhraseDispellerData(true, targets[0].position , targets[0].scale)),
        };
        return new Dialogue(ref phrases);
    }

    public Dialogue InitDialogs9()
    {
        string[] text = {
            "после вчерашней песчаной бури у нас серьёзные поломки в модулях",
            "давайте начнём с того что расчистим завали при помощи динамита",
        };
        Phrase[] phrases = {
            new Phrase(text[0], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.LEFT), new PhraseDispellerData(true, targets[2].position , targets[2].scale - new Vector2(2, 2))),
            new Phrase(text[1], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.RIGHT), new PhraseDispellerData(true, targets[5].position , targets[5].scale)),
        };
        return new Dialogue(ref phrases);
    }

    public Dialogue InitDialogs10()
    {
        string[] text = {
            "некоторые станции требуют более продвинутые и требуют более крутых зданий",
            "\"отели\" - апартаменты высшего класса для крутых",
            "для их конструкции требуются структуры с парками и домами",
            "после выполнения они так же появятся в очереди на конвеер",
        };
        Phrase[] phrases = {
            new Phrase(text[0], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.LEFT), new PhraseDispellerData(true, targets[3].position , targets[3].scale)),
            new Phrase(text[1], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.RIGHT), new PhraseDispellerData(true, targets[6].position , targets[6].scale)),
            new Phrase(text[2], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.RIGHT), new PhraseDispellerData(false, targets[5].position , targets[5].scale)),
            new Phrase(text[3], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.RIGHT), new PhraseDispellerData(true, targets[1].position , targets[1].scale)),
        };
        return new Dialogue(ref phrases);
    }

    public Dialogue InitDialogs11()
    {
        string[] text = {
            "надеюсь нам дадут пожить в этих крутых отелях)))",
        };
        Phrase[] phrases = {
            new Phrase(text[0], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.LEFT), new PhraseDispellerData(false, new Vector2(0, 0), new Vector2(0, 0))),
        };
        return new Dialogue(ref phrases);
    }

    public Dialogue InitDialogs12()
    {
        string[] text = {
            "Для полноценной работы большим колониям нужны ещё здания",
            "Например фермы, они кормят народ кайф\r\nв) для их сборки нужны батареи и парки",
            "Чтобы двигать нашу науку так же нужны лаборатории",
            "их делают из батарей и зданий",
            "постараемся!",
        };
        Phrase[] phrases = {
            new Phrase(text[0], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.LEFT), new PhraseDispellerData(true, targets[3].position , targets[3].scale)),
            new Phrase(text[1], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.RIGHT), new PhraseDispellerData(true, targets[7].position , targets[7].scale)),
            new Phrase(text[2], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.RIGHT), new PhraseDispellerData(true, targets[8].position , targets[8].scale)),
            new Phrase(text[3], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.RIGHT), new PhraseDispellerData(false, targets[1].position , targets[1].scale)),
            new Phrase(text[4], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.RIGHT), new PhraseDispellerData(true, targets[8].position , new Vector2(0, 0))),
        };
        return new Dialogue(ref phrases);
    }
    public Dialogue InitDialogs13()
    {
        string[] text = {
            "с таким количеством построек ангар и конвеер могут быстро переполниться",
            "не нужно бояться собирать сложные здания и потом взрывать их",
            "так мы можем сэкономить место",
            "благо бюджет у нас считай бесконечный)",
        };
        Phrase[] phrases = {
            new Phrase(text[0], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.LEFT), new PhraseDispellerData(true, targets[2].position ,  targets[2].scale - new Vector2(2, 2))),
            new Phrase(text[1], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.RIGHT), new PhraseDispellerData(true, targets[5].position , targets[5].scale)),
            new Phrase(text[2], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.RIGHT), new PhraseDispellerData(false, targets[8].position , targets[8].scale)),
            new Phrase(text[3], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.RIGHT), new PhraseDispellerData(true, targets[5].position , new Vector2(0, 0))),
        };
        return new Dialogue(ref phrases);
    }

    public Dialogue InitDialogs14()
    {
        string[] text = {
            "хух, это был последний заказ, кажется всё",
            "отлично поработали!",
            "теперь, когда все механики изучены, можем приступить к бесконечной работе",
            "слава арстоцке",
        };
        Phrase[] phrases = {
            new Phrase(text[0], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.LEFT), new PhraseDispellerData(true, targets[3].position ,  targets[3].scale)),
            new Phrase(text[1], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(true, targets[3].position , new Vector2(0, 0))),
            new Phrase(text[2], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(false, targets[8].position , targets[8].scale)),
            new Phrase(text[3], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(false, targets[8].position , new Vector2(0, 0))),
        };
        return new Dialogue(ref phrases);
    }

    public Dialogue InitDialogs15()
    {
        string[] text = {
            "хух, это был последний заказ, кажется всё",
            "отлично поработали!",
            "теперь, когда все механики изучены, можем приступить к бесконечной работе",
            "слава арстоцке",
        };
        Phrase[] phrases = {
            new Phrase(text[0], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.LEFT), new PhraseDispellerData(false, targets[3].position ,  targets[3].scale)),
            new Phrase(text[1], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(false, targets[3].position , new Vector2(0, 0))),
            new Phrase(text[2], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(false, targets[8].position , targets[8].scale)),
            new Phrase(text[3], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(false, targets[8].position , new Vector2(0, 0))),
        };
        return new Dialogue(ref phrases);
    }

    public Dialogue GetDialogue(int number)
    {
        if (number == 0) return InitDialogs0();
        if (number == 1) return InitDialogs1();
        if (number == 2) return InitDialogs2();
        if (number == 3) return InitDialogs3();
        if (number == 4) return InitDialogs4();
        if (number == 5) return InitDialogs5();
        if (number == 6) return InitDialogs6();
        if (number == 7) return InitDialogs7();
        if (number == 8) return InitDialogs8();
        if (number == 9) return InitDialogs9();
        if (number == 10) return InitDialogs10();
        if (number == 11) return InitDialogs11();
        if (number == 12) return InitDialogs12();
        if (number == 13) return InitDialogs13();
        if (number == 14) return InitDialogs14();
        if (number == 15) return InitDialogs15();
        return InitDialogs0();
    }

    public void InitCover()
    {
        GameObject cover = Instantiate(cover_controller_prefab);
        cover_controller = cover.GetComponent<CoverController>();
    }

    public void InitText()
    {
        GameObject text = Instantiate(text_controller_prefab);
        text_controller = text.GetComponent<TextWriter>();
    }

    public void InitActors() { 
        for (int i = 0; i < actors_prefabs.Length; i++)
        {
            GameObject actor = Instantiate(actors_prefabs[i]);
            actors_controllers.Add(actor.GetComponent<AvatarController>());
        }
    }

    public void SelectDialogue(int dialogue_number)
    {
        if (in_dialogue == false)
        {
            selected_dialogue = GetDialogue(dialogue_number);
            cover_controller.OpenCover(() => {
                RenderNext();
                in_dialogue = true;
                return 0;
            });
        }
    }

    public void OpenDialogieHook()
    {
        RenderNext();

    }

    public void CloseDialogue()
    {
        in_dialogue = false;
        cover_controller.CloseCover(() => { return 0; });
        for (int i = 0; i < actors_controllers.Count; i++)
        {
            actors_controllers[i].SetInactive();
            actors_controllers[i].Disable();
        }
        text_controller.Disable();
        dialog_number++;
    }

    public void RenderPhrase(ref Phrase phrase)
    {
        if (phrase.phrase != "*")
        {
            text_controller.Enable();
            text_controller.SetText(phrase.phrase);
        }
    }
    public void RenderActor(ref Phrase phrase)
    {
        if ((int) phrase.actor_data.actor >= 0)
        {
            if (phrase.actor_data.active) actors_controllers[(int)phrase.actor_data.actor].SetActive();
            else actors_controllers[(int)phrase.actor_data.actor].SetInactive();

            actors_controllers[(int)phrase.actor_data.actor].SetPostion(phrase.actor_data.actor_position, phrase.actor_data.actor_state);
        }
    }
    public void RenderDispeller(ref Phrase phrase)
    {
        if (phrase.dispeller_data.use_dispeller)
        {
            cover_controller.SetDispellerTarget(phrase.dispeller_data.dispeller_target_position, phrase.dispeller_data.dispeller_target_size);
        }
    }
    public void Render(ref Phrase phrase)
    {
        RenderPhrase(ref phrase);
        RenderActor(ref phrase);
        RenderDispeller(ref phrase);
    }

    public void RenderNext()
    {
        Phrase next_phrase = selected_dialogue.Next();
        if (next_phrase != null) Render(ref next_phrase);
        else CloseDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && in_dialogue)
        {
            RenderNext();
        }
        if (Input.GetMouseButtonDown(1) && in_dialogue)
        {
            CloseDialogue();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            SelectDialogue(dialog_number);
        }
    }
}
