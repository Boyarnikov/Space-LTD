using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
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

    public Dialogue(ref Phrase[] _phrases, Func<float> _hook)
    {
        place = -1;
        phrases = _phrases;
        hook = _hook;
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

    // 0 - êîíâååð
    // 1 - êîíâååð_êîóíòåð
    // 2 - ïîëå
    // 3 - òàêñ
    // 4 - òàñê êàóíòåð
    // 5 - 1é ðåöåïò
    // 6 - 2é ðåöåïò
    // 7 - 3é ðåöåïò
    // 8 - 4é ðåöåïò

    public Dialogue InitDialogs0(Func<float> hook)
    {
        string[] text = {
            "You finally arrived! I was told it would happen soon, but you know, time passes a bit differently here…",
            "Uh, anyway, on behalf of our team, I’m honored to greet you here, on Mars!",
            "We’ve finished the basic preparations, but we still need more space to live and work.",
            "That’s why we need help to expand our facilities. And that’s exactly what you’re going to help with.",
            "As you know, construction here is not an easy task – lack of oxygen and whatnot.",
            "So we build everything here and then transport the structure to wherever we need it! Isn’t it ingenious?",
            "Your job as an engineer is to build complex facilities out of smaller units. You can see them above",
            "Use LMB to move a unit to the grid.",
            "You can rotate a unit by clicking RMB.",
            "Let's install the first module in the grid",
        };
        Phrase[] phrases = {
            new Phrase(text[0], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.LEFT), new PhraseDispellerData(false, new Vector2(0, 0), new Vector2(0, 0))),
            new Phrase(text[1], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.LEFT), new PhraseDispellerData(false, new Vector2(0, 0), new Vector2(0, 0))),
            new Phrase(text[2], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.LEFT), new PhraseDispellerData(false, new Vector2(0, 0), new Vector2(0, 0))),
            new Phrase(text[3], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.LEFT), new PhraseDispellerData(false, new Vector2(0, 0), new Vector2(0, 0))),
            new Phrase(text[4], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.LEFT), new PhraseDispellerData(false, new Vector2(0, 0), new Vector2(0, 0))),
            new Phrase(text[5], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.LEFT), new PhraseDispellerData(false, new Vector2(0, 0), new Vector2(0, 0))),
            new Phrase(text[6], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(true, targets[0].position , targets[0].scale)),
            new Phrase(text[7], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(true, targets[2].position , targets[2].scale)),
            new Phrase(text[8], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.LEFT), new PhraseDispellerData(false, new Vector2(0, 0), new Vector2(0, 0))),
            new Phrase(text[9], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.LEFT), new PhraseDispellerData(false, new Vector2(0, 0), new Vector2(0, 0))),
        };
        return new Dialogue(ref phrases, hook);
    }

    public Dialogue InitDialogs1(Func<float> hook)
    {
        string[] text = {
            "Your tasks are on the right: you can see blueprints that you have to copy. ",
            "To submit a blueprint drag it over your units on the grid",
            "You can rotate blueprints as well!",
        };
        Phrase[] phrases = {
            new Phrase(text[0], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(true, targets[3].position , targets[3].scale)),
            new Phrase(text[1], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(true, targets[2].position , targets[2].scale)),
            new Phrase(text[2], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(true, targets[2].position , new Vector2(0, 0))),
        };
        return new Dialogue(ref phrases, hook);
    }

    public Dialogue InitDialogs2(Func<float> hook)
    {
        string[] text = {
            "Congratulations on your first building!",
            "Here you can see how many blueprints are left.",
            "Try to finish all of them.",
            "I have also provided you with additional missions!",
            "They are optional, so don't worry if you don't complete them.",
            "Good luck!",
        };
        Phrase[] phrases = {
            new Phrase(text[0], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(true, targets[4].position , new Vector2(0, 0))),
            new Phrase(text[1], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.LEFT), new PhraseDispellerData(true, targets[4].position , targets[4].scale)),
            new Phrase(text[2], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(false, targets[4].position , targets[4].scale)),
            new Phrase(text[3], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(true, targets[3].position + new Vector2(0, 3.2f), targets[3].scale * new Vector3(1, 0.35f, 1))),
            new Phrase(text[4], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(true, targets[3].position + new Vector2(0, 3.2f), targets[3].scale * new Vector3(1, 0.35f, 1))),
            new Phrase(text[5], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(true, targets[4].position , new Vector2(0, 0))),
        };
        return new Dialogue(ref phrases, hook);
    }

    public Dialogue InitDialogs3(Func<float> hook)
    {
        string[] text = {
            "Great, the first batch of buildings is finished!",
            "And by the way, you can skip dialogues by pressing RMB.",
        };
        Phrase[] phrases = {
            new Phrase(text[0], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.LEFT), new PhraseDispellerData(false, new Vector2(0, 0), new Vector2(0, 0))),
            new Phrase(text[1], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(false, new Vector2(0, 0), new Vector2(0, 0))),
        };
        return new Dialogue(ref phrases, hook);
    }

    public Dialogue InitDialogs4(Func<float> hook)
    {
        string[] text = {
            "Last time we only used units of one type – houses.",
            "Obviously, we can’t live here without oxygen, so there’s another unit with various photosynthesizing flora, let’s call it a park for short.",
            "Some blueprints need both houses and parks.",
            "Don’t forget that you can rotate units with RMB if you need a different configuration.",
            "This works both on lying objects and on those that are carried.",
        };
        Phrase[] phrases = {
            new Phrase(text[0], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.LEFT), new PhraseDispellerData(false, new Vector2(0, 0), new Vector2(0, 0))),
            new Phrase(text[1], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(false, new Vector2(0, 0), new Vector2(0, 0))),
            new Phrase(text[2], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(true, targets[3].position , targets[3].scale)),
            new Phrase(text[3], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(false, new Vector2(0, 0), new Vector2(0, 0))),
            new Phrase(text[4], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(true, targets[3].position, new Vector2(0, 0))),
        };
        return new Dialogue(ref phrases, hook);
    }

    public Dialogue InitDialogs5(Func<float> hook)
    {
        string[] text = {
            "Good job! Now the ecology here is probably better than back on Earth.",
            "Not that that's much praise these days",
        };
        Phrase[] phrases = {
            new Phrase(text[0], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(false, new Vector2(0, 0), new Vector2(0, 0))),
            new Phrase(text[1], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(false, new Vector2(0, 0), new Vector2(0, 0))),
        };
        return new Dialogue(ref phrases, hook);
    }

    public Dialogue InitDialogs6(Func<float> hook)
    {
        string[] text = {
            "Our colony also needs utility units – for example, solar panels and batteries for producing and storing electricity.",
            "The more units we build, the easier our life here will be!",
        };
        Phrase[] phrases = {
            new Phrase(text[0], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.LEFT), new PhraseDispellerData(false, new Vector2(0, 0), new Vector2(0, 0))),
            new Phrase(text[1], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(false, new Vector2(0, 0), new Vector2(0, 0))),
        };
        return new Dialogue(ref phrases, hook);
    }

    public Dialogue InitDialogs7(Func<float> hook)
    {
        string[] text = {
            "I forgot to mention: you know how sometimes the units are already in place and you can use them to complete the blueprints?",
            "If these units get in your way, you can dispose of them!",
            "You can take an order from Pow-powder Solutions™ to get explosives.",
            "When you complete an order, the explosives tile will appear on the conveyor next time you take something from it.",
        };
        Phrase[] phrases = {
            new Phrase(text[0], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.RIGHT), new PhraseDispellerData(true, targets[2].position, targets[2].scale - new Vector2(2, 2))),
            new Phrase(text[1], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.RIGHT), new PhraseDispellerData(true, targets[2].position, targets[2].scale - new Vector2(2, 2))),
            new Phrase(text[2], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.RIGHT), new PhraseDispellerData(false, targets[3].position , targets[3].scale)),
            new Phrase(text[3], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.RIGHT), new PhraseDispellerData(false, targets[3].position, new Vector2(0, 0))),
        };
        return new Dialogue(ref phrases, hook);
    }

    public Dialogue InitDialogs8(Func<float> hook)
    {
        string[] text = {
            "When you send a special order new tiles appear in the conveyor queue",
            "Clear the conveyor to gain access to it",
        };
        Phrase[] phrases = {
            new Phrase(text[0], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.RIGHT), new PhraseDispellerData(true, targets[1].position , targets[1].scale)),
            new Phrase(text[1], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.RIGHT), new PhraseDispellerData(true, targets[0].position , targets[0].scale)),
        };
        return new Dialogue(ref phrases, hook);
    }

    public Dialogue InitDialogs16(Func<float> hook)
    {
        string[] text = {
            "Fantastic! These multifunctional modules will surely help our colleagues!",
        };
        Phrase[] phrases = {
            new Phrase(text[0], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.LEFT), new PhraseDispellerData(false, new Vector2(0, 0), new Vector2(0, 0))),
        };
        return new Dialogue(ref phrases, hook);
    }

    public Dialogue InitDialogs9(Func<float> hook)
    {
        string[] text = {
            "Oh no… It seems like yesterday’s sandstorm caused some units to malfunction and collapse. ",
            "Fortunately, no one got hurt. ",
            "Let’s start by clearing the grid from these debris. You can use dynamite to do it.",
        };
        Phrase[] phrases = {
            new Phrase(text[0], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.LEFT), new PhraseDispellerData(true, targets[2].position , targets[2].scale - new Vector2(2, 2))),
            new Phrase(text[1], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.LEFT), new PhraseDispellerData(true, targets[2].position , targets[2].scale - new Vector2(2, 2))),
            new Phrase(text[2], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.RIGHT), new PhraseDispellerData(true, targets[5].position , targets[5].scale)),
        };
        return new Dialogue(ref phrases, hook);
    }

    public Dialogue InitDialogs10(Func<float> hook)
    {
        string[] text = {
            "Some of the facilities require complex units: these are tiles that are made from two basic buildings of different types.",
            "For example, here we have hotels. Imagine how much money people pay to spend their vacation here…",
            "Anyway, to make a hotel unit you need to combine house tiles and park tiles. ",
            "After you complete a recipe, the tile will appear in the conveyor queue.",
        };
        Phrase[] phrases = {
            new Phrase(text[0], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.LEFT), new PhraseDispellerData(true, targets[3].position , targets[3].scale)),
            new Phrase(text[1], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.RIGHT), new PhraseDispellerData(true, targets[6].position , targets[6].scale)),
            new Phrase(text[2], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.RIGHT), new PhraseDispellerData(false, targets[5].position , targets[5].scale)),
            new Phrase(text[3], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.RIGHT), new PhraseDispellerData(true, targets[1].position , targets[1].scale)),
        };
        return new Dialogue(ref phrases, hook);
    }

    public Dialogue InitDialogs11(Func<float> hook)
    {
        string[] text = {
            "I wonder if I could stay in a hotel like this at least for a couple of days…",
            "Do you think they have discounts for Space Limited™ employees?",
        };
        Phrase[] phrases = {
            new Phrase(text[0], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.LEFT), new PhraseDispellerData(false, new Vector2(0, 0), new Vector2(0, 0))),
            new Phrase(text[1], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.LEFT), new PhraseDispellerData(false, new Vector2(0, 0), new Vector2(0, 0))),
        };
        return new Dialogue(ref phrases, hook);
    }

    public Dialogue InitDialogs12(Func<float> hook)
    {
        string[] text = {
            "Alright, we’re really close to making our base totally functional! We just need some more units:",
            "Combine batteries and houses to make labs",
            "Or combine batteries and parks to make farms",
            "What’s an astronomer’s favorite meal?",
            "A satellite dish! Hehe.. Anyway…",
        };
        Phrase[] phrases = {
            new Phrase(text[0], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.LEFT), new PhraseDispellerData(true, targets[3].position , targets[3].scale)),
            new Phrase(text[1], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.RIGHT), new PhraseDispellerData(true, targets[7].position , targets[7].scale)),
            new Phrase(text[2], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.RIGHT), new PhraseDispellerData(true, targets[8].position , targets[8].scale)),
            new Phrase(text[3], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.RIGHT), new PhraseDispellerData(false, targets[1].position , targets[1].scale)),
            new Phrase(text[4], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.RIGHT), new PhraseDispellerData(true, targets[8].position , new Vector2(0, 0))),
        };
        return new Dialogue(ref phrases, hook);
    }
    public Dialogue InitDialogs13(Func<float> hook)
    {
        string[] text = {
            "Since you have a lot of buildings here, the grid and the conveyor may become full really soon.",
            "You can make complex buildings and then blow them up to waste awkward tiles.",
            "Don’t worry about the budget, it’s practically limitless",
            "Unlike our grid here.",
        };
        Phrase[] phrases = {
            new Phrase(text[0], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.LEFT), new PhraseDispellerData(true, targets[2].position ,  targets[2].scale - new Vector2(2, 2))),
            new Phrase(text[1], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.RIGHT), new PhraseDispellerData(true, targets[5].position , targets[5].scale)),
            new Phrase(text[2], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.RIGHT), new PhraseDispellerData(false, targets[8].position , targets[8].scale)),
            new Phrase(text[3], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.RIGHT), new PhraseDispellerData(true, targets[5].position , new Vector2(0, 0))),
        };
        return new Dialogue(ref phrases, hook);
    }

    public Dialogue InitDialogs14(Func<float> hook)
    {
        string[] text = {
            "Well done!",
            "Now you know everything about building on Mars, you can get to work",
            "More tasks await!",
            "Endless mode? That's what they call it",
        };
        Phrase[] phrases = {
            new Phrase(text[0], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.LEFT), new PhraseDispellerData(true, targets[3].position ,  targets[3].scale)),
            new Phrase(text[1], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(true, targets[3].position , new Vector2(0, 0))),
            new Phrase(text[2], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(false, targets[8].position , targets[8].scale)),
            new Phrase(text[3], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(false, targets[8].position , new Vector2(0, 0))),
        };
        return new Dialogue(ref phrases, hook);
    }

    public Dialogue InitDialogs15(Func<float> hook)
    {
        string[] text = {
            "õóõ, ýòî áûë ïîñëåäíèé çàêàç, êàæåòñÿ âñ¸",
            "îòëè÷íî ïîðàáîòàëè!",
            "òåïåðü, êîãäà âñå ìåõàíèêè èçó÷åíû, ìîæåì ïðèñòóïèòü ê áåñêîíå÷íîé ðàáîòå",
            "ñëàâà àðñòîöêå",
        };
        Phrase[] phrases = {
            new Phrase(text[0], new PhraseActorData(Actors.GIRL1, true, -1, ActorPosition.LEFT), new PhraseDispellerData(false, targets[3].position ,  targets[3].scale)),
            new Phrase(text[1], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(false, targets[3].position , new Vector2(0, 0))),
            new Phrase(text[2], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(false, targets[8].position , targets[8].scale)),
            new Phrase(text[3], new PhraseActorData(Actors.GIRL1, true, -2, ActorPosition.LEFT), new PhraseDispellerData(false, targets[8].position , new Vector2(0, 0))),
        };
        return new Dialogue(ref phrases, hook);
    }

    public Dialogue GetDialogue(int number, Func<float> hook)
    {
        if (number == 0) return InitDialogs0(hook);
        if (number == 1) return InitDialogs1(hook);
        if (number == 2) return InitDialogs2(hook);
        if (number == 3) return InitDialogs3(hook);
        if (number == 4) return InitDialogs4(hook);
        if (number == 5) return InitDialogs5(hook);
        if (number == 6) return InitDialogs6(hook);
        if (number == 7) return InitDialogs7(hook);
        if (number == 8) return InitDialogs8(hook);
        if (number == 9) return InitDialogs9(hook);
        if (number == 10) return InitDialogs10(hook);
        if (number == 11) return InitDialogs11(hook);
        if (number == 12) return InitDialogs12(hook);
        if (number == 13) return InitDialogs13(hook);
        if (number == 14) return InitDialogs14(hook);
        if (number == 15) return InitDialogs15(hook);
        if (number == 16) return InitDialogs16(hook);
        return InitDialogs0(hook);
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

    public void SelectDialogue(int dialogue_number, Func<float> hook)
    {
        if (in_dialogue == false)
        {
            selected_dialogue = GetDialogue(dialogue_number, hook);
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
        cover_controller.CloseCover(selected_dialogue.hook);
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
        /*
        if (Input.GetKeyDown(KeyCode.V))
        {
            SelectDialogue(dialog_number, () => { return 0; });
        }
        */
    }
}
