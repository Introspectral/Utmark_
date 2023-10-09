using System;
using System.Collections.Generic;

public class RandomMessagePicker
{
    private Random _random;
    private List<string> _messages;

    public RandomMessagePicker()
    {
        _random = new Random();

        _messages = new List<string>
        {
            "You see nothing to pick up.",
            "There's nothing here to grab.",
            "Your hands find nothing.",
            "Nothing worth picking up here.",
            "The ground is empty; nothing to collect.",
            "Seems like there's nothing for you to take.",
            "It's barren; not a thing to gather.",
            "You look around but find nothing to pick up.",
            "No items catch your eye.",
            "The area is clear; no objects to retrieve.",
            "You search, but there's nothing to take.",
            "It's all clear; nothing to hold onto.",
            "You can't find a single thing to collect.",
            "Your search yields no items to grab.",
            "Not a single item in sight to pick up.",
            "There's a lack of things to gather here.",
            "Your hands remain empty; nothing to get.",
            "You don't spot any items to collect.",
            "The vicinity has no objects for you to take.",
            "Seems empty; no items to retrieve."
        };
    }

    public string GetRandomMessage()
    {
        if (_messages.Count == 0)
        {
            throw new InvalidOperationException("No messages available to choose from.");
        }

        int randomIndex = _random.Next(_messages.Count);
        return _messages[randomIndex];
    }
}


